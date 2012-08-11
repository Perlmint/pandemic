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
    class MainMenu
    {
        Texture2D startButton;
        Texture2D startButtonSelected;
        Texture2D helpButton;
        Texture2D helpButtonSelected;
        Texture2D exitButton;
        Texture2D exitButtonSelected;
        Texture2D background;

        float selectTimer;
        const float SelectTimeOut = 0.2f;

        Game1 game;

        enum Selected
        {
            start,
            help,
            exit
        };

        Selected selected;

        public MainMenu(Game1 paramGame)
        {
            game = paramGame;
        }

        public void Initialize()
        {
            game.BindKeyboardEventListener(Keys.Up, this.MoveUp);
            game.BindKeyboardEventListener(Keys.Down, this.MoveDown);
            game.BindKeyboardEventListener(Keys.Enter, this.Select);

            selected = Selected.start;
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>(Constants.MenuFolder + "\\" + Constants.MenuBackground);
            helpButton = Content.Load<Texture2D>(Constants.MenuFolder + "\\" + Constants.HelpButton);
            startButton = Content.Load<Texture2D>(Constants.MenuFolder + "\\" + Constants.StartButton);
            exitButton = Content.Load<Texture2D>(Constants.MenuFolder + "\\" + Constants.ExitButton);
            helpButtonSelected = Content.Load<Texture2D>(Constants.MenuFolder + "\\" + Constants.HelpButton + "Selected");
            startButtonSelected = Content.Load<Texture2D>(Constants.MenuFolder + "\\" + Constants.StartButton + "Selected");
            exitButtonSelected = Content.Load<Texture2D>(Constants.MenuFolder + "\\" + Constants.ExitButton + "Selected");
        }

        public void Update(float elapsedGameTime)
        {
            if(selectTimer <= SelectTimeOut)
                selectTimer += elapsedGameTime;
        }

        void MoveDown()
        {
            if (selectTimer > SelectTimeOut)
            {
                selectTimer = 0;
                switch (selected)
                {
                    case Selected.start:
                        selected = Selected.help;
                        break;
                    case Selected.help:
                        selected = Selected.exit;
                        break;
                    case Selected.exit:
                        selected = Selected.start;
                        break;
                }
            }
        }

        void MoveUp()
        {
            if (selectTimer > SelectTimeOut)
            {
                selectTimer = 0;
                switch (selected)
                {
                    case Selected.start:
                        selected = Selected.exit;
                        break;
                    case Selected.help:
                        selected = Selected.start;
                        break;
                    case Selected.exit:
                        selected = Selected.help;
                        break;
                }
            }
        }

        void Select()
        {
            switch (selected)
            {
                case Selected.start:
                    game.Play();
                    break;
                case Selected.help:
                    game.Help();
                    break;
                case Selected.exit:
                    game.Exit();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.Draw(startButton, new Rectangle(300, 300, 200, 70), Color.White);
            spriteBatch.Draw(helpButton, new Rectangle(300, 400, 200, 70), Color.White);
            spriteBatch.Draw(exitButton, new Rectangle(300, 500, 200, 70), Color.White);

            switch (selected)
            {
                case Selected.start:
                    spriteBatch.Draw(startButtonSelected, new Rectangle(300, 300, 200, 70), Color.White);
                    break;
                case Selected.help:
                    spriteBatch.Draw(helpButtonSelected, new Rectangle(300, 400, 200, 70), Color.White);
                    break;
                case Selected.exit:
                    spriteBatch.Draw(exitButtonSelected, new Rectangle(300, 500, 200, 70), Color.White);
                    break;
            }
        }
    }
}