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
    class NPC : GameObject
    {
        public enum State
        {
            alive, almost_dead, dead
        };
        Vector2 destination;
        const float Speed = 3.0f;
        const int MaxHP = 100;
        Texture2D dead;

        public void LoadContent(ContentManager Content, Dictionary<State, string> path)
        {
            base.LoadContent(Content, path.Values.ToArray());
            dead = Content.Load<Texture2D>(path[State.dead]);
        }

        public override void Spawn(Vector2 pos)
        {
            hp = MaxHP;
            base.Spawn(pos);
        }

        void SetDestination(Vector2 dst)
        {
            destination = dst;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();

            position += Vector2.Normalize(destination - position) * Speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive())
            {
                base.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(dead, position, Color.White);
            }
        }
    }
}
