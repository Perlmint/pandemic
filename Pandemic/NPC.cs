using System;
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
            alive, almost_dead, dead
        };
        Vector2 destination;
        const float Speed = 1.0f;
        const int MaxHP = 100;
        static Texture2D dead;
        static Texture2D tex;
        const int RectSize = 30;

        float corpseTimer;
        const float CorpseTimeOut = 3.0f;

        static Random randomGenerator = new Random();

        State state;

        public override void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>(Stage.stageInstance.Units.Enemy);
            dead = Content.Load<Texture2D>(Stage.stageInstance.Units.Dead);
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
                        Vector2 temp = destination - position;
                        if (temp.Length() > 100)
                        {
                            temp += new Vector2((float)(randomGenerator.NextDouble() - 0.5) * 100);
                        }

                        position += Vector2.Normalize(temp) * Speed;

                        if (hp < 0)
                        {
                            corpseTimer = 0;
                            state = State.dead;
                        }

                        rect.X = (int)position.X;
                        rect.Y = (int)position.Y;
                        rect.Width = RectSize;
                        rect.Height = RectSize;

                        break;
                    case State.almost_dead:
                        break;
                    case State.dead:
                        corpseTimer += elapsedGameTime;
                        if (corpseTimer >= CorpseTimeOut)
                            isAlive = false;
                        break;
                }
            }
        }

        public void CheckBulletCollision(Bullet[] bullets)
        {
            HashSet<Rectangle> hashSet;
            foreach (Bullet bullet in bullets)
            {
                if (bullet.IsAlive())
                {
                    if (bullet.Intersects(this.GetRectangle()))
                    {
                        this.AccHP(-bullet.GetDamageValue());
                        bullet.Explode();
                    }
                    hashSet = bullet.GetEffectRectangle();

                    foreach (Rectangle rectangle in hashSet)
                    {
                        if (rect.Intersects(rectangle))
                            this.AccHP(-bullet.GetDamageValue());
                    }
                }
            }
        }

        protected override Texture2D currentTexture
        {
            get
            {
                if (this.isAlive)
                {
                    switch (state)
                    {
                        case State.alive:
                            return tex;
                            break;
                        case State.dead:
                            return null;
                            break;
                    }
                }
                return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive())
            {
                switch (state)
                {
                    case State.alive:
                        //base.Draw(spriteBatch);
                        spriteBatch.Draw(tex, rect, Color.White);
                        break;
                    case State.almost_dead:
                        break;
                    case State.dead:
                        spriteBatch.Draw(dead, rect, Color.White);
                        break;
                }
            }
        }
    }
}
