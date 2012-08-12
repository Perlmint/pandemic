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
        int[, ] baseTxArr;
        int TCol, TRow;

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
                nObstacles.Add(new Obstacle(nMap, e.Key, e.Value));
            return nMap;
        }

        public void LoadContent(ContentManager Content)
        {
            baseTxs = new List<Texture2D> {};
            foreach (string path in Stage.stageInstance.NonUnits.Ground.ToArray())
                baseTxs.Add(Content.Load<Texture2D>(path));
            foreach(Obstacle o in Obstacles)
                o.LoadContent(Content);
            Random rnd = new Random();
            baseTxArr = new int [TCol = width / Stage.stageInstance.TileWidth, TRow = height / Stage.stageInstance.TileHeight];
            for (int i = 0; i < TCol; i++)
                for (int j = 0; j < TRow; j++)
                    baseTxArr[i, j] = rnd.Next() % baseTxs.Count;
        }

        public void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            for (int i = 0; i < TCol; i++)
                for (int j = 0; j < TRow; j++)
                    spriteBatch.Draw(baseTxs[baseTxArr[i, j]],
                        screen.translateWorldToScreen(new Rectangle()
                        {
                            X = -width / 2 + i * Stage.stageInstance.TileWidth,
                            Y = -height / 2 + j * Stage.stageInstance.TileHeight,
                            Width = Stage.stageInstance.TileWidth,
                            Height = Stage.stageInstance.TileHeight
                        }), Color.White);
            foreach (Obstacle o in Obstacles)
                o.Draw(spriteBatch, screen);
        }

        public Vector2 CalcRealDirection(GameObject gObject)
        {
            foreach (Obstacle o in Obstacles)
            {
                Rectangle tmpRect = o.GetRectangle();
                tmpRect.Offset(-(int)gObject.expectedSpeed.X, -(int)gObject.expectedSpeed.Y);
                if (gObject.Intersects(tmpRect) /*&&
                    gObject.CollidesWith(o, true, new Vector2(-(int)gObject.expectedSpeed.X, -(int)gObject.expectedSpeed.Y))*/)
                {
                    return new Vector2();
                }
            }
            return gObject.expectedSpeed;
        }
        
        public Rectangle ToRectangle()
        {
            return new Rectangle()
            {
                X = -width / 2,
                Y = -height / 2,
                Width = width,
                Height = height
            };
        }

        public bool isInMap(Vector2 pos)
        {
            return ToRectangle().Contains((int)pos.X, (int)pos.Y);
        }
        
        public Vector2 TotalRelativeCoord(Vector2 pos)
        {
            return new Vector2((pos.X) / (width / 2), (pos.Y) / (height / 2));
        }
    }
}
