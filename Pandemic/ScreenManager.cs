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
        int width, height;
        float scale;

        public ScreenManager()
        {
            screenCenter = new Vector2(0, 0);
            scale = 1;
        }

        protected Vector2 translateWorldToScreen(Vector2 worldCoord)
        {
            Vector2 result = (worldCoord - screenCenter) * scale;
            return result;
        }

        protected Vector2 translateScreenToWorld(Vector2 screenCoord)
        {
            Vector2 result = (screenCenter - screenCoord) / scale;
            return result;
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
        }

        public void applySizeToGraphicsMgr(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }
    }
}
