using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Map
    {
        int width, height;
        Obstacle[] Obstacles;

        public static Map createFromStage(Stage stage)
        {
            return new Map()
            {
                width = stage.MapWidth,
                height = stage.MapHeight,
                Obstacles = new Obstacle[] {}
            };
        }

        public void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {

        }
    }
}
