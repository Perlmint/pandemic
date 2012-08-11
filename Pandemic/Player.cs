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
    class Player : GameObject
    {
        public enum State
        {
            alive, almost_dead, dead
        };
        const float Speed = 3.0f;
        const int MaxHP = 100;
        const int MaxBullet = 50;
        const int RectSize = 30;
        Texture2D dead;

        float atkCooldown;

        Weapon weapon;
        Bullet[] bullets;

        State state;

        float corpseTimer;
        const float corpseTimeOut = 5.0f;

        public Player()
        {
            int i;

            int[,] area = {
                              {1,1,1},
                              {1,2,1},
                              {1,1,1}
                          };
            weapon = new Weapon(100, area, 0.5f, 100);
            bullets = new Bullet[MaxBullet];

            for(i = 0; i < MaxBullet; i++)
            {
                bullets[i] = Bullet.newBasicBullet(weapon.GetDamage());
            }
        }

        public void LoadContent(ContentManager Content, Dictionary<State, string> path)
        {
            base.LoadContent(Content, path[State.alive]);
            dead = Content.Load<Texture2D>(path[State.dead]);
            weapon.LoadContent(Content);

            //foreach (Bullet bullet in bullets)
            //{
                //bullet.LoadContent(Content, path[State.bullet]);
            //}
        }

        public Bullet[] GetBulletArray()
        {
            return bullets;
        }

        public void Initialize(Game1 game)
        {
            base.Initialize();

            game.BindKeyboardEventListener(Keys.Up, this.MoveUp);
            game.BindKeyboardEventListener(Keys.Down, this.MoveDown);
            game.BindKeyboardEventListener(Keys.Right, this.MoveRight);
            game.BindKeyboardEventListener(Keys.Left, this.MoveLeft);
            game.BindKeyboardEventListener(Keys.A, this.Fire);
        }

        public override void Spawn(Vector2 pos)
        {
            hp = MaxHP;
            base.Spawn(pos);
            atkCooldown = 0;
            state = State.alive;
        }

        public override void Update(float elapsedGameTime)
        {
            switch (state)
            {
                case State.alive:
                    foreach (Bullet bullet in bullets)
                        bullet.Update(elapsedGameTime);

                    if (atkCooldown > 0)
                        atkCooldown -= elapsedGameTime;
                    if (hp <= 0)
                    {
                        state = State.dead;
                        corpseTimer = 0;
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
                    if (corpseTimer >= corpseTimeOut)
                    {
                        isAlive = false;
                    }
                    break;
            }
        }

        void MoveUp()
        {
            position.Y -= Speed;
        }

        void MoveDown()
        {
            position.Y += Speed;
        }

        void MoveLeft()
        {
            position.X -= Speed;
        }

        void MoveRight()
        {
            position.X += Speed;
        }

        void Fire()
        {
            if (atkCooldown <= 0)
            {
                int i = 0;

                while (i < MaxBullet && bullets[i].IsAlive())
                    i++;

                if (i < MaxBullet)
                {
                    bullets[i].Spawn(position);
                    bullets[i].SetDestination(position + new Vector2(weapon.GetRange(), 0));
                    atkCooldown = weapon.GetCooldown();
                }
            }
        }

        public void GetWeapon(Weapon wpn)
        {
            weapon = wpn;

            foreach (Bullet bullet in bullets)
            {
                bullet.SetTexture(weapon.GetBulletTex(), weapon.GetEffectTex());
                bullet.SetEffectArea(weapon.GetArea());
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive())
            {
                switch (state)
                {
                    case State.alive:
                        base.Draw(spriteBatch);
                        break;
                    case State.dead:
                        spriteBatch.Draw(dead, rect, Color.White);
                        break;
                    case State.almost_dead:
                        break;
                }
            }

            foreach (Bullet bullet in bullets)
                bullet.Draw(spriteBatch);
        }
    }
}
