﻿using System;
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
    abstract class GameObject
    {
        protected Vector2 position;
        protected Rectangle rect;
        protected int hp;
        protected Texture2D tex;
        protected bool isAlive;

        public GameObject()
        {
            isAlive = false;
        }

        public virtual void Initialize()
        {
        }

        public virtual void LoadContent(ContentManager Content, string path)
        {
            tex = Content.Load<Texture2D>(path);
        }

        public virtual void Spawn(Vector2 pos)
        {
            position = pos;
            isAlive = true;
        }

        public abstract void Update(float elapsedGameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                spriteBatch.Draw(tex, position, Color.White);
            }
        }

        public bool IsAlive()
        {
            return isAlive;
        }
    }
}