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

        const float Speed = 2.0f;
        const int MaxHP = 5;
        const int MaxBullet = 50;
        const int RectSize = 30;
        static Texture2D dead;
        static Texture2D tex;
        static Texture2D heart;
        static Dictionary<string, Texture2D> texUp;
        static Dictionary<string, Texture2D> texDown;
        static Dictionary<string, Texture2D> texLeft;
        static Dictionary<string, Texture2D> texRight;
        string weaponName;
        protected override int rectSize { get { return 30; } }

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

        ScreenManager screenManager;
        Map map;

        public Player(Game1 paramGame)
        {
            int i;
            weaponName = "rpg";
            game = paramGame;
            weapon = new Weapon(weaponName);

            bullets = new Bullet[MaxBullet];

            for (i = 0; i < MaxBullet; i++)
            {
                bullets[i] = Bullet.newBasicBullet(weapon.GetDamage(), weapon.GetEffectTimeOut());
            }
        }

        public override void LoadContent(ContentManager Content)
        {
        }

        public static void LoadCommonContent(ContentManager Content)
        {
            texUp = new Dictionary<string, Texture2D>();
            texDown = new Dictionary<string, Texture2D>();
            texLeft = new Dictionary<string, Texture2D>();
            texRight = new Dictionary<string, Texture2D>();

            //tex = Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["sword"].DefaultTexture);
            texUp.Add("sword", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["sword"][PlayerDirection.up]));
            texDown.Add("sword", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["sword"][PlayerDirection.down]));
            texLeft.Add("sword", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["sword"][PlayerDirection.left]));
            texRight.Add("sword", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["sword"][PlayerDirection.right]));

            texUp.Add("dagger", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["dagger"][PlayerDirection.up]));
            texDown.Add("dagger", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["dagger"][PlayerDirection.down]));
            texLeft.Add("dagger", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["dagger"][PlayerDirection.left]));
            texRight.Add("dagger", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["dagger"][PlayerDirection.right]));

            texUp.Add("handgun", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["handgun"][PlayerDirection.up]));
            texDown.Add("handgun", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["handgun"][PlayerDirection.down]));
            texLeft.Add("handgun", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["handgun"][PlayerDirection.left]));
            texRight.Add("handgun", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["handgun"][PlayerDirection.right]));

            texUp.Add("gatling", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["gatling"][PlayerDirection.up]));
            texDown.Add("gatling", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["gatling"][PlayerDirection.down]));
            texLeft.Add("gatling", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["gatling"][PlayerDirection.left]));
            texRight.Add("gatling", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["gatling"][PlayerDirection.right]));

            texUp.Add("rpg", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["rpg"][PlayerDirection.up]));
            texDown.Add("rpg", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["rpg"][PlayerDirection.down]));
            texLeft.Add("rpg", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["rpg"][PlayerDirection.left]));
            texRight.Add("rpg", Content.Load<Texture2D>(Stage.stageInstance.Units.PlayerArmed["rpg"][PlayerDirection.right]));

            dead = Content.Load<Texture2D>(Stage.stageInstance.Units.Player_Death);
            heart = Content.Load<Texture2D>("heart");
            Weapon.LoadCommonContent(Content);
        }

        public Bullet[] GetBulletArray()
        {
            return bullets;
        }

        public void updateScreenManager(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        public void updateMap(Map map)
        {
            this.map = map;
        }

        public void Initialize(Game1 game)
        {
            base.Initialize();

            game.BindKeyboardEventListener(Keys.W, this.MoveUp);
            game.BindKeyboardEventListener(Keys.S, this.MoveDown);
            game.BindKeyboardEventListener(Keys.D, this.MoveRight);
            game.BindKeyboardEventListener(Keys.A, this.MoveLeft);

            game.BindKeyboardEventListener(Keys.Left, () => { ChangeDirection(Direction.left); this.Fire(); });
            game.BindKeyboardEventListener(Keys.Right, () => { ChangeDirection(Direction.right); this.Fire(); });
            game.BindKeyboardEventListener(Keys.Up, () => { ChangeDirection(Direction.up); this.Fire(); });
            game.BindKeyboardEventListener(Keys.Down, () => { ChangeDirection(Direction.down); this.Fire(); });

            direction = Direction.right;
            tex = texRight[weaponName];
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
                    break;
            }

            if (!isAlive)
                game.GameOver();

            foreach (Bullet bullet in bullets)
                bullet.Update(elapsedGameTime);

            if (atkCooldown > 0)
                atkCooldown -= elapsedGameTime;
        }

        public override void PostUpdate()
        {
            foreach (Bullet bullet in bullets)
                bullet.PostUpdate();
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
            expectedSpeed_ += new Vector2(0, - Speed);
        }

        void MoveDown()
        {
            expectedSpeed_ += new Vector2(0, + Speed);
        }

        void MoveLeft()
        {
            expectedSpeed_ += new Vector2(- Speed, 0);
        }

        void MoveRight()
        {
            expectedSpeed_ += new Vector2(+ Speed, 0);
        }

        bool updatePosition(Vector2 offset)
        {
            if (map.isInMap(position + offset))
            {
                position += offset;
                screenManager.SetScreenCenter(position - screenManager.GetScreenSize() / 2 * map.TotalRelativeCoord(position));
                return true;
            }
            else
                return false;
        }

        public override void Move(Vector2 direction)
        {
            if (isAlive)
            {
                updatePosition(direction);

                expectedSpeed_ = new Vector2();
            }
        }

        void ChangeDirection(Direction newDirection)
        {
            if (direction != newDirection)
            {
                // TODO: change Sprite Image
                switch (newDirection)
                {
                    case Direction.up:
                        tex = texUp[weaponName];
                        break;
                    case Direction.down:
                        tex = texDown[weaponName];
                        break;
                    case Direction.left:
                        tex = texLeft[weaponName];
                        break;
                    case Direction.right:
                        tex = texRight[weaponName];
                        break;
                }
                direction = newDirection;
            }
        }

        void Fire()
        {
            if (atkCooldown <= 0 && state != State.dead)
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
                            bullets[i].SetEffectArea(weapon.GetAreaUp());
                            break;
                        case Direction.down:
                            bulletDirection = new Vector2(0, -weapon.GetRange());
                            bullets[i].SetEffectArea(weapon.GetAreaDown());
                            break;
                        case Direction.left:
                            bulletDirection = new Vector2(weapon.GetRange(), 0);
                            bullets[i].SetEffectArea(weapon.GetAreaLeft());
                            break;
                        case Direction.right:
                            bulletDirection = new Vector2(-weapon.GetRange(),0);
                            bullets[i].SetEffectArea(weapon.GetAreaRight());
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
            //Bullet.SetEffectArea(weapon.GetArea);
        }

        public Vector2 GetPosition()
        {
            return position;
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
                            ret = tex;
                            break;
                        case State.dead:
                            ret = dead;
                            break;
                        case State.absolute:
                            ret = tex;
                            break;
                        default:
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
                        //base.Draw(spriteBatch);
                        spriteBatch.Draw(tex, screen.translateWorldToScreen(GetRectangle()), Color.White);
                        break;
                    case State.dead:
                        spriteBatch.Draw(dead, screen.translateWorldToScreen(GetRectangle()), Color.White);
                        break;
                    case State.almost_dead:
                        break;
                    case State.absolute:
                        spriteBatch.Draw(tex, screen.translateWorldToScreen(GetRectangle()), new Color(1.0f, 1.0f, 1.0f, 0.5f));
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
