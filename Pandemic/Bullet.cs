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
        const float Speed = 3.0f;
        float displacement;
        float range;
        int[,] effectArea;
        Point bulletPos;

        float explodeTimeout;
        const float TimeOut = 1.0f;

        enum BulletState
        {
            Going,
            Explosion
        };

        BulletState state;

        Texture2D effectTex;

        public Bullet()
        {
        }

        public override void Spawn(Vector2 pos)
        {
            base.Spawn(pos);
            state = BulletState.Going;
        }

        public void SetTexture(Texture2D bullet, Texture2D effect)
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

            for (i = 0; i < (effectArea.Rank + 1); i++)
            {
                for (j = 0; j < effectArea.Length / (effectArea.Rank + 1); j++)
                {
                    if (area[i, j] == 2)
                        bulletPos = new Point(i, j);
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

        public void Explode()
        {
            displacement = 0;
            state = BulletState.Explosion;
            explodeTimeout = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isAlive)
            {
                switch (state)
                {
                    case BulletState.Going:
                        base.Draw(spriteBatch);
                        break;
                    case BulletState.Explosion:
                        int i, j;

                        for (i = 0; i < (effectArea.Rank + 1); i++)
                        {
                            for (j = 0; j < effectArea.Length / (effectArea.Rank + 1); j++)
                            {
                                spriteBatch.Draw(effectTex, position + new Vector2(30 * (i - bulletPos.X), 30 * (j - bulletPos.Y)), Color.White);
                            }
                        }
                        break;
                }
            }
        }
    }
}
