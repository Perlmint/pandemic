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

        ScreenManager screenManager;
        GameState state;
        Player player;
		NPCManager npcManager;
        Stage stage;
        Map map;
        MainMenu mainMenu;
        Song bgm;
        Song bgm2;
        GameOver gameOver;
        ImageDisplay help;
        Weapon[] weaponBox;
        string[] weaponNameSet = { "dagger", "sword", "handgun", "gatling", "rpg" };
        Random rand;

        SpriteFont font;

        float elapsedTime;

        enum GameState
        {
            main,
            play,
            gameover,
            help
        };
		
        public delegate void keyboardEventListener();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            keyboardEventListeners = new Dictionary<Keys, LinkedList<keyboardEventListener>>();
            screenManager = new ScreenManager();
            npcManager = NPCManager.SharedManager;

            rand = new Random();

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
            
            state = GameState.main;
            //setupOpeningState();
            BindKeyboardEventListener(Keys.Escape, new keyboardEventListener(this.Exit));

            mainMenu = new MainMenu(this);
            gameOver = new GameOver();
            help = new ImageDisplay();
            weaponBox = new Weapon[5];
            for(int i = 0; i < 5; i++){
                weaponBox[i] = new Weapon(weaponNameSet[rand.Next(weaponNameSet.Length)]);
            }

            //mainMenu.Initialize();

            setupMainState();
            
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

            NPC.LoadCommonContent(Content);
            Player.LoadCommonContent(Content);

            mainMenu.LoadContent(Content);
            gameOver.LoadContent(Content);

            for(int i = 0; i < 5; i++){
                weaponBox[i].LoadContent(Content);
            }

            help.LoadContent(Content, "Help\\Help");
            bgm = Content.Load<Song>(Constants.MusicFolder + "\\" + Constants.BackgroundMusic);
            bgm2 = Content.Load<Song>(Constants.MusicFolder + "\\they are comming");

            font = Content.Load<SpriteFont>("font");
            
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgm);
            //MediaPlayer.Pause();

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

            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (state)
            {
                case GameState.main:
                    mainMenu.Update(elapsedTime);
                    break;
                case GameState.play:
                    player.Update(elapsedTime);
                    npcManager.Update(elapsedTime, player.GetPosition(), player.GetBulletArray());
                    player.NPCCollision(npcManager.NPCs);

                    player.Move(player.expectedSpeed);//map.CalcRealDirection(player));
                    npcManager.Move();

                    player.PostUpdate();
                    npcManager.PostUpdate();

                    foreach (Weapon weapon in weaponBox)
                    {
                        weapon.Update(elapsedTime);
                        if (player.Intersects(weapon.GetRectangle()))
                        {
                            player.GetWeapon(weapon);
                            weapon.KillObject();
                        }
                    }

                    break;
                case GameState.gameover:
                    player.Update(elapsedTime);
                    npcManager.Update(elapsedTime, player.GetPosition(), player.GetBulletArray());
                    player.NPCCollision(npcManager.NPCs);

                    player.Move(player.expectedSpeed);//map.CalcRealDirection(player));
                    npcManager.Move();

                    player.PostUpdate();
                    npcManager.PostUpdate();
                    gameOver.Update(elapsedTime);
                    break;
                case GameState.help:
                    help.Update(elapsedTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// 키보드 이벤트 처리
        /// </summary>
        protected void ProcessKeyboardEvent()
        {
            // Key events
            Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();


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
                        LinkedList<keyboardEventListener> listeners;
                        if (keyboardEventListeners.TryGetValue(key, out listeners))
                        {
                            foreach (keyboardEventListener listener in listeners)
                            {
                                listener();
                            }
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
            LinkedList<keyboardEventListener> listeners;
            if (keyboardEventListeners.TryGetValue(key, out listeners))
            {
                listeners.AddLast(listener);
            }
            else
            {
                listeners = new LinkedList<keyboardEventListener>();
                listeners.AddLast(listener);
                keyboardEventListeners.Add(key, listeners);
            }
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

        public void UnbindKeyboardEvent(Keys key)
        {
            LinkedList<keyboardEventListener> listeners;
            if (keyboardEventListeners.TryGetValue(key, out listeners))
            {
                keyboardEventListeners.Remove(key);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            switch (state)
            {
                case GameState.main:
                    mainMenu.Draw(spriteBatch);
                    break;
                case GameState.play:
                    map.Draw(spriteBatch, screenManager);
                    npcManager.Draw(spriteBatch, screenManager);
                    player.Draw(spriteBatch, screenManager);
                    spriteBatch.DrawString(font, "Zombies : " + npcManager.NPCs.Count(npc => !npc.IsDead).ToString(), new Vector2(0, 500), Color.Black);
                    spriteBatch.DrawString(font, "PlayTime : " + npcManager.playTime.ToString(), new Vector2(0, 530), Color.Black);

                    foreach (Weapon weapon in weaponBox)
                        weapon.Draw(spriteBatch, screenManager);

                    break;
                case GameState.gameover:
                    map.Draw(spriteBatch, screenManager);
                    npcManager.Draw(spriteBatch, screenManager);
                    player.Draw(spriteBatch, screenManager);
                    gameOver.Draw(spriteBatch);
                    break;
                case GameState.help:
                    help.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Help()
        {
            changeState(GameState.help);
        }

        public void Play()
        {
            changeState(GameState.play);
        }

        public void GameOver()
        {
            changeState(GameState.gameover);
        }

        void changeState(GameState newState)
        {
            if (state != newState)
            {
                switch (state)
                {
                    case GameState.main:
                        teardownMainState();
                        break;
                    case GameState.play:
                        teardownPlayState();
                        break;
                    case GameState.gameover:
                        teardownGameoverState();
                        break;
                    case GameState.help:
                        teardownHelpState();
                        break;
                }

                switch (newState)
                {
                    case GameState.main:
                        setupMainState();
                        break;
                    case GameState.play:
                        setupPlayState();
                        break;
                    case GameState.gameover:
                        setupGameoverState();
                        break;
                    case GameState.help:
                        setupHelpState();
                        break;
                }
                state = newState;
            }
        }

        protected void setupMainState()
        {
            MediaPlayer.Resume();
            mainMenu.Initialize();
        }

        protected void teardownMainState()
        {
            UnbindKeyboardEvent(Keys.Up);
            UnbindKeyboardEvent(Keys.Down);
            UnbindKeyboardEvent(Keys.Enter);
        }

        protected void teardownHelpState()
        {
            UnbindKeyboardEvent(Keys.Space);
        }

        protected void setupHelpState()
        {
            BindKeyboardEventListener(Keys.Space, new keyboardEventListener(() => changeState(GameState.main)));
            help.Initialize();
        }

        protected void setupPlayState()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(bgm2);
            stage = Stage.stageInstance;
            map = Map.createFromStage(stage);
            map.LoadContent(Content);
            screenManager.setSizeFromStage(stage);
            screenManager.applySizeToGraphicsMgr(graphics);
            player = new Player(this);
            player.updateScreenManager(screenManager);
            player.updateMap(map);
            npcManager.Initialize();
            npcManager.map = map;
            player.Initialize(this);
            player.Spawn(Stage.stageInstance.PlayerInitialPosition);

            foreach (Weapon weapon in weaponBox)
            {
                weapon.Initialize();
                weapon.Spawn(new Vector2(rand.Next(-Stage.stageInstance.MapWidth / 2, Stage.stageInstance.MapWidth / 2),
                    rand.Next(-Stage.stageInstance.MapHeight / 2, Stage.stageInstance.MapHeight / 2)));
            }
        }

        protected void teardownPlayState()
        {
            this.UnbindKeyboardEvent(Keys.Up);
            this.UnbindKeyboardEvent(Keys.Down);
            this.UnbindKeyboardEvent(Keys.Right);
            this.UnbindKeyboardEvent(Keys.Left);
            this.UnbindKeyboardEvent(Keys.W);
            this.UnbindKeyboardEvent(Keys.A);
            this.UnbindKeyboardEvent(Keys.S);
            this.UnbindKeyboardEvent(Keys.D);
            this.UnbindKeyboardEvent(Keys.Space);
            MediaPlayer.Stop();
            MediaPlayer.Play(bgm);
        }

        protected void setupGameoverState()
        {
            this.BindKeyboardEventListener(Keys.Space,new keyboardEventListener(() => changeState(GameState.main)));
            GameOver.ResultContext result;

            result.kill = npcManager.kill;
            result.time = npcManager.playTime;
            gameOver.Initialize(result);
        }

        protected void teardownGameoverState()
        {
            this.UnbindKeyboardEvent(Keys.Space);
        }
    }
}
