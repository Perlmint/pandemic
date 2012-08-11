using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pandemic
{
    class ScreenManager
    {
        Vector2 screenCenter;
        Vector2 screenTranslate;
        int width, height;
        float scale;

        public ScreenManager()
        {
            screenCenter = new Vector2(0, 0);
            scale = 1;
        }

        public Vector2 translateWorldToScreen(Vector2 worldCoord)
        {
            return (worldCoord - screenCenter) * scale + screenTranslate;
        }

        public Vector2 translateScreenToWorld(Vector2 screenCoord)
        {
            return ((screenCoord - screenTranslate) + screenCenter) / scale;
        }

        public Rectangle translateWorldToScreen(Rectangle worldRect)
        {
            return new Rectangle()
            {
                X = (int) ((worldRect.X - screenCenter.X) * scale + screenTranslate.X),
                Y = (int) ((worldRect.Y - screenCenter.Y) * scale + screenTranslate.Y),
                Width = (int)(worldRect.Width * scale),
                Height = (int)(worldRect.Height * scale)
            };
        }

        public Rectangle translateScreenToWorld(Rectangle screenRect)
        {
            return new Rectangle()
            {
                X = (int)(((screenRect.X - screenTranslate.X) + screenCenter.X) * scale),
                Y = (int)(((screenRect.Y - screenTranslate.Y) + screenCenter.Y) * scale),
                Width = (int)(screenRect.Width * scale),
                Height = (int)(screenRect.Height * scale)
            };
        }

        public void Draw(SpriteBatch spriteBatch, GameObject []drawableObjects)
        {
            foreach (GameObject drawableObject in drawableObjects)
            {
                //Vector2 screenCoord;
                drawableObject.Draw(spriteBatch, this);
            }
        }

        public void setSizeFromStage(Stage stage)
        {
            this.width = stage.ScreenWidth;
            this.height = stage.ScreenHeight;
            this.screenTranslate = new Vector2(this.width / 2, this.height / 2);
        }

        public void applySizeToGraphicsMgr(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }
    }
}
