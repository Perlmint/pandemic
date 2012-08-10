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

using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Pandemic
{
    class Player : GameObject
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnt, String text, String caption, uint type);
        [Conditional("DEBUG")]
        public void ShowMessageBox(String text)
        {
            MessageBox(new IntPtr(0), text, "", 0);
        }

        public enum State
        {
            alive, almost_dead, dead
        };
        const float Speed = 3.0f;
        const int MaxHP = 100;
        Texture2D dead;

        public Player()
        {
        }

        public void LoadContent(ContentManager Content, Dictionary<State, string> path)
        {
            base.LoadContent(Content, path[State.alive]);
            dead = Content.Load<Texture2D>(path[State.dead]);
        }

        public void Initialize(Game1 game)
        {
            base.Initialize();

            game.BindKeyboardEventListener(Keys.Up, this.MoveUp);
            game.BindKeyboardEventListener(Keys.Down, this.MoveDown);
            game.BindKeyboardEventListener(Keys.Right, this.MoveRight);
            game.BindKeyboardEventListener(Keys.Left, this.MoveLeft);
        }

        public override void Spawn(Vector2 pos)
        {
            hp = MaxHP;
            base.Spawn(pos);
        }

        public override void Update(GameTime gameTime)
        {
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
            ShowMessageBox(position.X.ToString());
        }

        public void Fire()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive())
            {
                base.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(dead, position, Color.White);
            }
        }
    }
}
