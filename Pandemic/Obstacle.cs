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
    class Obstacle : GameObject
    {
        Vector2 position;
        string texpath;
        Texture2D tex;
        public Obstacle(Vector2 position, string texpath)
        {
            this.position = position;
            this.texpath = texpath;
        }

        public override void Update(float elapsedGameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, ScreenManager screen)
        {
        }

        public override void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>(texpath);
        }
    }
}
