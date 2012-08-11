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
    class ImageDisplay
    {
        Texture2D tex;

        public ImageDisplay()
        {
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager Content, string path)
        {
            tex = Content.Load<Texture2D>(path);
        }

        public void Update(float elapsedGameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(tex, new Rectangle(0, 0, 800, 600), Color.White);
        }
    }
}
