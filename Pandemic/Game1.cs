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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<Keys, LinkedList<keyboardEventListener>> keyboardEventListeners;

        public delegate void keyboardEventListener();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            keyboardEventListeners = new Dictionary<Keys, LinkedList<keyboardEventListener>>();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            keyboardEventListeners[Keys.Escape].AddLast(new keyboardEventListener(this.Exit));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            ProcessKeyboardEvent();

            base.Update(gameTime);
        }

        /// <summary>
        /// 키보드 이벤트 처리
        /// </summary>
        protected void ProcessKeyboardEvent()
        {
            // Key events
            Keys[] pressedKeys = Keyboard.GetState(PlayerIndex.One).GetPressedKeys();

            if (pressedKeys.Count() > 0)
            {
                foreach (Keys key in pressedKeys)
                {
                    if (key == Keys.Escape)
                    {
                        this.Exit();
                    }
                    else
                    {
                        LinkedList<keyboardEventListener> listeners = keyboardEventListeners[key];
                        foreach (keyboardEventListener listener in listeners)
                        {
                            listener();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 키보드 리스너 추가
        /// </summary>
        /// <param name="key">이벤트를 받을 키</param>
        /// <param name="listener">이벤트 리스너 딜리게이트</param>
        public void BindKeyboardEventListener(Keys key, keyboardEventListener listener)
        {
            keyboardEventListeners[key].AddLast(listener);
        }

        /// <summary>
        /// 키보드 리스너 제거
        /// </summary>
        /// <param name="listener">제거할 리스너 딜리게이트</param>
        public void UnbindKeyboardEventListener(keyboardEventListener listener)
        {
            foreach (KeyValuePair<Keys, LinkedList<keyboardEventListener>> listenerPair in keyboardEventListeners)
            {
                listenerPair.Value.Remove(listener);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
