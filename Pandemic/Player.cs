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
            alive, almost_dead, dead, absolute
        };

        public enum Direction
        {
            left, right, up, down
        };

        const float Speed = 3.0f;
        const int MaxHP = 5;
        const int MaxBullet = 50;
        const int RectSize = 30;
        static Texture2D dead;
        static Texture2D tex;
        static Texture2D heart;

        float atkCooldown;

        Weapon weapon;
        Bullet[] bullets;

        State state;
        Direction direction;

        float corpseTimer;
        float absoluteTimer;
        const float absoluteTimeOut = 2.0f;
        const float corpseTimeOut = 5.0f;
        Game1 game;

        public Player(Game1 paramGame)
        {
            int i;
            int[,] area = {
                              {1,1,1},
                              {1,2,1},
                              {1,1,1}
                          };
            game = paramGame;
            weapon = new Weapon(100, area, 0.5f, 100);

            bullets = new Bullet[MaxBullet];

            for (i = 0; i < MaxBullet; i++)
            {
                bullets[i] = Bullet.newBasicBullet(weapon.GetDamage());
            }
        }

        public override void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["basic"].DefaultTexture);
            dead = Content.Load<Texture2D>(Stage.stageInstance.Units.Player_Death);
            heart = Content.Load<Texture2D>("heart");
            weapon.LoadContent(Content);
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

            game.BindKeyboardEventListener(Keys.Space, this.Fire);

            direction = Direction.right;
        }

        public override void Spawn(Vector2 pos)
        {
            hp = MaxHP;
            base.Spawn(pos);
            atkCooldown = 0;
            state = State.alive;
            GetWeapon(weapon);
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
                case State.absolute:
                    absoluteTimer += elapsedGameTime;
                    if (absoluteTimer >= absoluteTimeOut)
                    {
                        state = State.alive;
                    }

                    rect.X = (int)position.X;
                    rect.Y = (int)position.Y;
                    rect.Width = RectSize;
                    rect.Height = RectSize;
                    break;
            }

            if (!isAlive)
                game.GameOver();
        }

       public void NPCCollision(List<NPC> npcList)
        {
            if (state != State.absolute && state != State.dead)
            {
                foreach (NPC npc in npcList)
                {
                    if (this.CollidesWith(npc))
                    {
                        this.AccHP(-1);
                        if (hp > 0)
                        {
                            state = State.absolute;
                            absoluteTimer = 0;
                        }
                        else
                        {
                            state = State.dead;
                            corpseTimer = 0;
                        }
                        break;
                    }
                }
            }
        }

        void MoveUp()
        {
            position.Y -= Speed;
            ChangeDirection(Direction.up);
        }

        void MoveDown()
        {
            position.Y += Speed;
            ChangeDirection(Direction.down);
        }

        void MoveLeft()
        {
            position.X -= Speed;
            ChangeDirection(Direction.left);
        }

        void MoveRight()
        {
            position.X += Speed;
            ChangeDirection(Direction.right);
        }

        void ChangeDirection(Direction newDirection)
        {
            if (direction != newDirection)
            {
                // TODO: change Sprite Image
                direction = newDirection;
            }
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
                    Vector2 bulletDirection;
                    switch (direction)
                    {
                        case Direction.up:
                            bulletDirection = new Vector2(0, weapon.GetRange());
                            break;
                        case Direction.down:
                            bulletDirection = new Vector2(0, -weapon.GetRange());
                            break;
                        case Direction.left:
                            bulletDirection = new Vector2(weapon.GetRange(), 0);
                            break;
                        case Direction.right:
                            bulletDirection = new Vector2(-weapon.GetRange(),0);
                            break;
                        default:
                            bulletDirection = new Vector2();
                            break;
                    }
                    bullets[i].Spawn(position);
                    bullets[i].SetDestination(position + bulletDirection);
                    atkCooldown = weapon.GetCooldown();
                }
            }
        }

        public void GetWeapon(Weapon wpn)
        {
            weapon = wpn;
            Bullet.SetTexture(weapon.GetBulletTex(), weapon.GetEffectTex());
            Bullet.SetEffectArea(weapon.GetArea());
        }

        public Vector2 GetPosition()
        {
            return position;
<<<<<<< HEAD
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
                            return dead;
                            break;
                        case State.absolute:
                            return tex;
                            break;
                        default:
                            break;
                    }
                }
                return null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
=======
        }

        public override void Draw(SpriteBatch spriteBatch, ScreenManager screen)
>>>>>>> Eon
        {
            if (IsAlive())
            {
                switch (state)
                {
                    case State.alive:
                        //base.Draw(spriteBatch);
                        spriteBatch.Draw(tex, rect, Color.White);
                        break;
                    case State.dead:
                        spriteBatch.Draw(dead, rect, Color.White);
                        break;
                    case State.almost_dead:
                        break;
                    case State.absolute:
                        spriteBatch.Draw(tex, rect, new Color(1.0f, 1.0f, 1.0f, 0.5f));
                        break;
                }
            }

            foreach (Bullet bullet in bullets)
                bullet.Draw(spriteBatch, screen);

            for (int i = 0; i < hp; i++)
            {
                spriteBatch.Draw(heart, new Rectangle(20 * i, 0, 20, 20), Color.White);
            }
        }
    }
}
