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
    class Bullet : GameObject
    {
        Vector2 destination;
        float Speed;
        float displacement;
        float range;
        int damage;
        int effectDamage;
        int RectSize;
        protected override int rectSize { get { return RectSize; } }
        static int[,] effectArea;
        static Point bulletPos;        

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
            expectedSpeed_ = Vector2.Normalize(position - destination) * Speed;
        }

        public void SetDamage(int value)
        {
            damage = value;
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
                case BulletState.Explosion:
                    explodeTimeout += elapsedGameTime;
                    if (explodeTimeout >= TimeOut)
                    {
                        isAlive = false;
                    }
                    break;
            }
        }

        public override void PostUpdate()
        {
            switch (state)
            {
                case BulletState.Going:
                    position += expectedSpeed_;
                    displacement += Speed;

                    if (displacement >= range)
                    {
                        Explode();
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
                            rectHashSet.Add(new Rectangle((i - bulletPos.X) * RectSize + GetRectangle().X,
                                (j - bulletPos.Y) * RectSize + GetRectangle().Y,
                                GetRectangle().Width, GetRectangle().Height));
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
                Texture2D ret = null;
                if (this.isAlive)
                {
                    switch (state)
                    {
                        case BulletState.Going:
                            ret = tex;
                            break;
                        default:
                            break;
                    }
                }
                return ret;
            } }

        public override void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            if (this.isAlive)
            {
                switch (state)
                {
                    case BulletState.Going:
                        spriteBatch.Draw(tex, screen.translateWorldToScreen(GetRectangle()), Color.White);
                        break;
                    case BulletState.Explosion:
                        int i, j;

                        for (i = 0; i < Math.Sqrt(effectArea.Length); i++)
                        {
                            for (j = 0; j < Math.Sqrt(effectArea.Length); j++)
                            {
                                if (effectArea[i,j] == 1)
                                {
                                    Rectangle effectRect = new Rectangle()
                                    {
                                        X = RectSize * (i - bulletPos.X) + GetRectangle().X,
                                        Y = RectSize * (j - bulletPos.Y) + GetRectangle().Y,
                                        Width = RectSize,
                                        Height = RectSize
                                    };
                                    spriteBatch.Draw(effectTex, screen.translateWorldToScreen(effectRect), Color.White);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
