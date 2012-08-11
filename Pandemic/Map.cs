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
        HashSet<Obstacle> Obstacles;
        List<Texture2D> baseTxs;

        public static Map createFromStage(Stage stage)
        {
            HashSet<Obstacle> nObstacles;
            Map nMap = new Map()
            {
                width = stage.MapWidth,
                height = stage.MapHeight,
                Obstacles = nObstacles = new HashSet<Obstacle>() {}
            };
            foreach (KeyValuePair<Vector2, string> e in Stage.stageInstance.ObstaclesArrangeSpec)
                nObstacles.Add(new Obstacle(e.Key, e.Value));
            return nMap;
        }

        public void LoadContent(ContentManager Content)
        {
            baseTxs = new List<Texture2D> {};
            foreach (string path in Stage.stageInstance.NonUnits.Ground.ToArray())
                baseTxs.Add(Content.Load<Texture2D>(path));
            foreach(Obstacle o in Obstacles)
                o.LoadContent(Content);
        }

        public void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            
        }
    }
}
