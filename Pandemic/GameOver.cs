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
    class GameOver
    {
        Texture2D tex;

        public GameOver()
        {
        }

        public void Initialize()
        {
        }

        public void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>("GameOver\\gameover");
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
