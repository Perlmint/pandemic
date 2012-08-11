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
    class Bullet : GameObject
    {
        Vector2 destination;
        float Speed;
        float displacement;
        float range;
        int damage;
        int effectDamage;
        int RectSize;

        int[,] effectArea;
        Point bulletPos;
        

        float explodeTimeout;
        float TimeOut;

        enum BulletState
        {
            Going,
            Explosion
        };

        BulletState state;

        static Texture2D effectTex;
        static Texture2D tex;

        public static Bullet newBasicBullet(int dmg, float timeOut)
        {
            return new Bullet()
            {
                Speed = 4.0f,
                RectSize = 30,
                TimeOut = timeOut,
                damage = dmg
            };
        }

        public Bullet()
        {
        }

        public override void LoadContent(ContentManager Content)
        {
            //throw new NotImplementedException();
        }

        public override void Spawn(Vector2 pos)
        {
            base.Spawn(pos);
            state = BulletState.Going;
        }

        public static void SetTexture(Texture2D bullet, Texture2D effect)
        {
            tex = bullet;
            effectTex = effect;
        }

        public void SetDestination(Vector2 dst)
        {
            destination = dst;
            range = (dst - position).Length();
        }

        public void SetEffectArea(int[,] area)
        {
            int i, j;

            effectArea = area;

            for (i = 0; i < Math.Sqrt(effectArea.Length); i++)
            {
                for (j = 0; j < Math.Sqrt(effectArea.Length); j++)
                {
                    if (area[i, j] == 2)
                    {
                        bulletPos = new Point(i, j);
                        return;
                    }
                }
            }
        }

        public override void Update(float elapsedGameTime)
        {
            switch (state)
            {
                case BulletState.Going:
                    position += Vector2.Normalize(position - destination) * Speed;
                    displacement += Speed;

                    if (displacement >= range)
                    {
                        Explode();
                    }

                    rect.X = (int)position.X;
                    rect.Y = (int)position.Y;
                    rect.Width = RectSize;
                    rect.Height = RectSize;
                    break;
                case BulletState.Explosion:
                    explodeTimeout += elapsedGameTime;
                    if (explodeTimeout >= TimeOut)
                    {
                        isAlive = false;
                    }
                    break;
            }
        }

        public int GetDamageValue()
        {
            return damage;
        }

        public HashSet<Rectangle> GetEffectRectangle()
        {
            HashSet<Rectangle> rectHashSet = new HashSet<Rectangle>();
            int i, j;


            if (state == BulletState.Explosion)
            {
                for (i = 0; i < Math.Sqrt(effectArea.Length); i++)
                {
                    for (j = 0; j < Math.Sqrt(effectArea.Length); j++)
                    {
                        if (effectArea[i, j] == 1)
                        {
                            rectHashSet.Add(new Rectangle((i - bulletPos.X) * RectSize + rect.X,
                                (j - bulletPos.Y) * RectSize + rect.Y,
                                RectSize, RectSize));
                        }
                    }
                }
            }

            return rectHashSet;
        }

        public void Explode()
        {
            displacement = 0;
            state = BulletState.Explosion;
            explodeTimeout = 0;
        }

        protected override Texture2D currentTexture
        {
            get {
                if (this.isAlive)
                {
                    switch (state)
                    {
                        case BulletState.Going:
                            return tex;
                            break;
                        default:
                            break;
                    }
                }
                return null;
            } }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isAlive)
            {
                switch (state)
                {
                    case BulletState.Going:
                        spriteBatch.Draw(tex, rect, Color.White);
                        break;
                    case BulletState.Explosion:
                        int i, j;
                        Rectangle effectRect = new Rectangle(0, 0, RectSize, RectSize);

                        for (i = 0; i < Math.Sqrt(effectArea.Length); i++)
                        {
                            for (j = 0; j < Math.Sqrt(effectArea.Length); j++)
                            {
                                if (effectArea[i,j] == 1)
                                {
                                    effectRect.X = RectSize * (i - bulletPos.X) + rect.X;
                                    effectRect.Y = RectSize * (j - bulletPos.Y) + rect.Y;
                                    spriteBatch.Draw(effectTex, effectRect, Color.White);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
