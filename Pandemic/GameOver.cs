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
        SpriteFont font;

        public struct ResultContext
        {
            public UInt32 kill;
            public float time;
        };

        ResultContext resultContext;

        public GameOver()
        {
        }

        public void Initialize(ResultContext result)
        {
            resultContext = result;
        }

        public void LoadContent(ContentManager Content)
        {
            tex = Content.Load<Texture2D>("GameOver\\gameover");
            font = Content.Load<SpriteFont>("font");
        }

        public void Update(float elapsedGameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, "Cured : " + resultContext.kill.ToString(), new Vector2(100, 400), Color.Black);
            spriteBatch.DrawString(font, "PlayTime : " + resultContext.time.ToString(), new Vector2(100, 420), Color.Black);
        }
    }
}