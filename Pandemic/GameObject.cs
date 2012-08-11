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
        protected virtual Texture2D currentTexture { get { return null; } }
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

        public virtual Rectangle GetRectangle()
        {
            if (isAlive)
                return rect;
            else
                return new Rectangle();
        }

        private bool Intersects(GameObject gameObject)
        {
            return this.rect.Intersects(gameObject.GetRectangle());
        }

        public bool Intersects(Rectangle rectangle)
        {
            return this.rect.Intersects(rectangle);
        }

        public bool CollidesWith(GameObject other)
        {
            // Default behavior uses per-pixel collision detection
            return CollidesWith(other, true);
        }

        public bool CollidesWith(GameObject other, bool calcPerPixel)
        {
            // Get dimensions of texture
            if (other.currentTexture == null || currentTexture == null)
                return false;
            int widthOther = other.currentTexture.Width;
            int heightOther = other.currentTexture.Height;
            int widthMe = currentTexture.Width;
            int heightMe = currentTexture.Height;

            if (calcPerPixel)          // for small sizes (nobody will notice :P)
            {
                return Bounds.Intersects(other.Bounds) // If simple intersection fails, don't even bother with per-pixel
                    && PerPixelCollision(this, other);
            }

            return Bounds.Intersects(other.Bounds);
        }

        static bool PerPixelCollision(GameObject a, GameObject b)
        {
            // Get Color data of each Texture
            Color[] bitsA = new Color[a.currentTexture.Width * a.currentTexture.Height];
            a.currentTexture.GetData(bitsA);
            Color[] bitsB = new Color[b.currentTexture.Width * b.currentTexture.Height];
            b.currentTexture.GetData(bitsB);

            // Calculate the intersecting rectangle
            int x1 = (int)Math.Max((int)a.position.X, (int)b.position.X);
            int x2 = (int)Math.Min((int)a.position.X + a.currentTexture.Width, (int)b.position.X + b.currentTexture.Width);

            int y1 = (int)Math.Max((int)a.position.Y, (int)b.position.Y);
            int y2 = (int)Math.Min((int)a.position.Y + a.currentTexture.Height, (int)b.position.Y + b.currentTexture.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color A = bitsA[(int)((x - (int)a.position.X) + (y - (int)a.position.Y) * a.currentTexture.Width)];
                    Color B = bitsB[(int)((x - (int)b.position.X) + (y - (int)b.position.Y) * b.currentTexture.Width)];

                    if (A.A != 0 && B.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        return true;
                    }
                }
            }
            // If no collision occurred by now, we're clear.
            return false;
        }

        private Rectangle bounds = Rectangle.Empty;
        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)position.X - currentTexture.Width,
                    (int)position.Y - currentTexture.Height,
                    currentTexture.Width,
                    currentTexture.Height);
            }

        }

        public abstract void Update(float elapsedGameTime);
        
        public abstract void Draw(SpriteBatch spriteBatch, ScreenManager screen);

        public bool IsAlive()
        {
            return isAlive;
        }
    }
}