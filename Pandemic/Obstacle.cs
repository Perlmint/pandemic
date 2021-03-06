﻿using System;
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
    class Obstacle : GameObject
    {
        string texpath;
        Texture2D tex;
        Map map;
        int Tx, Ty;

        protected override int rectSize
        {
            get
            {
                return Stage.stageInstance.TileWidth;
            }
        }

        public override Rectangle GetRectangle()
        {
            if (isAlive)
                return new Rectangle((int)(position.X * Stage.stageInstance.TileWidth),
                                    (int)(position.Y * Stage.stageInstance.TileHeight), this.rectSize, this.rectSize);
            else
                return new Rectangle();
        }

        public Obstacle(Map map, Vector2 position, string texpath)
        {
            this.map = map;
            this.position = position;
            this.texpath = texpath;
            this.isAlive = true;
        }

        public override void Update(float elapsedGameTime)
        {
        }

        public override void PostUpdate()
        {
        }

        public override void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
            spriteBatch.Draw(tex,
                screen.translateWorldToScreen(new Rectangle()
                {
                    X = (int) (position.X * Stage.stageInstance.TileWidth),
                    Y = (int) (position.Y * Stage.stageInstance.TileHeight),
                    Width = Stage.stageInstance.TileWidth,
                    Height = Stage.stageInstance.TileHeight
                }), Color.White);
        }

        public override void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>(texpath);
        }
    }
}
