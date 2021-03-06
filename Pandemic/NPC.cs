﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Pandemic
{
    class NPC : GameObject
    {
        public enum State
        {
            alive, almost_dead, dead, absolute
        };
        Vector2 destination;
        const float Speed = 2.0f;
        const int MaxHP = 100;
        static Texture2D dead;
        static Texture2D tex;
        protected override int rectSize { get { return 30; } }
        public bool IsDead { get { return state == State.dead; } }

        float corpseTimer;
        const float CorpseTimeOut = 3.0f;
        float absoluteTimer;
        const float absoluteTimeOut = 0.5f;

        static Random randomGenerator = new Random();

        State state;

        public static void LoadCommonContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>(Stage.stageInstance.Units.Enemy);
            dead = Content.Load<Texture2D>(Stage.stageInstance.Units.Dead);
        }

        public override void LoadContent(ContentManager Content)
        {
        }

        public override void Spawn(Vector2 pos)
        {
            hp = MaxHP;
            base.Spawn(pos);
            state = State.alive;
        }

        public void SetDestination(Vector2 dst)
        {
            destination = dst;
        }

        public override void Update(float elapsedGameTime)
        {
            //throw new NotImplementedException();

            if (isAlive)
            {
                switch (state)
                {
                    case State.alive:
                        {
                            if (hp < 0)
                            {
                                corpseTimer = 0;
                                state = State.dead;
                                return;
                            }

                            Vector2 temp = destination - position;
                            if (temp.Length() > 100)
                            {
                                temp += new Vector2((float)(randomGenerator.NextDouble() - 0.5) * 100);
                            }

                            expectedSpeed_ = Vector2.Normalize(temp) * Speed;
                        }
                        break;
                    case State.almost_dead:
                        break;
                    case State.dead:
                        corpseTimer += elapsedGameTime;
                        if (corpseTimer >= CorpseTimeOut)
                            isAlive = false;
                        break;
                    case State.absolute:
                        {
                            absoluteTimer += elapsedGameTime;
                            if (absoluteTimer >= absoluteTimeOut)
                            {
                                state = State.alive;
                            }

                            Vector2 temp = destination - position;

                            if (temp.Length() > 100)
                            {
                                temp += new Vector2((float)(randomGenerator.NextDouble() - 0.5) * 100);
                            }

                            expectedSpeed_ = Vector2.Normalize(temp) * Speed;
                        }
                        break;
                }
            }
        }

        public override void PostUpdate()
        {
        }


        public void CheckBulletCollision(Bullet[] bullets)
        {
            if (state == State.absolute)
                return;

            HashSet<Rectangle> hashSet;
            foreach (Bullet bullet in bullets)
            {
                if (bullet.IsAlive())
                {
                    if (bullet.CollidesWith(this))
                    {
                        this.AccHP(-bullet.GetDamageValue());
                        bullet.Explode();
                        if (this.hp > 0)
                        {
                            state = State.absolute;
                            absoluteTimer = 0;
                        }
                        else
                        {
                            corpseTimer = 0;
                            state = State.dead;
                        }
                        return;
                    }
                    hashSet = bullet.GetEffectRectangle();

                    foreach (Rectangle rectangle in hashSet)
                    {
                        if (GetRectangle().Intersects(rectangle))
                        {
                            this.AccHP(-bullet.GetDamageValue());
                            if (this.hp > 0)
                            {
                                state = State.absolute;
                                absoluteTimer = 0;
                            }
                            else
                            {
                                corpseTimer = 0;
                                state = State.dead;
                            }
                            return;
                        }
                    }
                }
            }
        }

        protected override Texture2D currentTexture
        {
            get
            {
                Texture2D ret = null;
                if (this.isAlive)
                {
                    switch (state)
                    {
                        case State.alive:
                        case State.absolute:
                            ret = tex;
                            break;
                        case State.dead:
                            //ret = dead;
                            ret = null;
                            break;
                    }
                }
                return ret;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            if (IsAlive())
            {
                switch (state)
                {
                    case State.alive:
                    case State.absolute:
                        //base.Draw(spriteBatch);
                        spriteBatch.Draw(tex, screen.translateWorldToScreen(GetRectangle()), Color.White);
                        break;
                    case State.almost_dead:
                        break;
                    case State.dead:
                        spriteBatch.Draw(dead, screen.translateWorldToScreen(GetRectangle()), Color.White);
                        break;
                }
            }
        }
    }
}
