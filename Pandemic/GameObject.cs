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
    abstract class GameObject
    {
        protected Vector2 position;
        protected Rectangle rect;
        protected int hp;
        //protected Texture2D tex;
        protected bool isAlive;

        public GameObject()
        {
            isAlive = false;
            rect = new Rectangle();
        }

        public virtual void Initialize()
        {
        }

        public abstract void LoadContent(ContentManager Content);

        public virtual void Spawn(Vector2 pos)
        {
            position = pos;
            isAlive = true;
        }

        public void AccHP(int value)
        {
            hp += value;
        }

        public Rectangle GetRectangle()
        {
            return rect;
        }

        private bool Intersects(GameObject gameObject)
        {
            return this.rect.Intersects(gameObject.GetRectangle());
        }

        public bool Intersects(Rectangle rectangle)
        {
            return this.rect.Intersects(rectangle);
        }

        public abstract void Update(float elapsedGameTime);
        
        public abstract void Draw(SpriteBatch spriteBatch);

        public bool IsAlive()
        {
            return isAlive;
        }
    }
}