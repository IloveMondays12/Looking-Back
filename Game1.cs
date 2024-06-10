using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Looking_Back
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        enum Screen
        {
            Start, 
            Animation,
            Main,
            Death
        }
        Random generator = new Random();
        Screen screen;
        MouseState mouseState;
        KeyboardState keyboardState;
        Rectangle window, start, whiteMan, playerHealthBar;
        Vector2 playerSpeed, snakeSpeed;
        SoundEffect windOne, windTwo, windThree, windFour;
        SoundEffectInstance windOneInstance, windTwoInstance, windThreeInstance, windFourInstance;
        float snakeattackSpeed;
        int walkAnimation, windNum = 1;
        List <int> enemyHealth = new List<int>();
        List <Rectangle> enemies = new List<Rectangle>();
        List<Rectangle> enemiesHealthBar = new List<Rectangle>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.Start;
            enemyHealth.Clear();
            window = new Rectangle(0, 0, 800, 600);
            start = new Rectangle(350, 400, 100, 50);
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            windOne = Content.Load<SoundEffect>("Wind (1)");
            windTwo = Content.Load<SoundEffect>("Wind (2)");
            windThree = Content.Load<SoundEffect>("Wind (3)");
            windFour = Content.Load<SoundEffect>("Wind (4)");
            windOneInstance = windOne.CreateInstance();
            windTwoInstance = windTwo.CreateInstance();
            windThreeInstance = windThree.CreateInstance();
            windFourInstance = windFour.CreateInstance();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (screen == Screen.Start)
            {
                if (windFourInstance.State != SoundState.Playing && windNum == 1)
                {
                    windOne.Play();
                    windNum = 2;
                }
                else if (windOneInstance.State != SoundState.Playing && windNum == 2)
                {
                    windTwo.Play();
                    windNum = 3;
                }
                 else if (windTwoInstance.State != SoundState.Playing && windNum == 3)
                {
                    windThree.Play();
                    windNum = 4;
                }
                else if (windThreeInstance.State != SoundState.Playing && windNum == 4)
                {
                    windFour.Play();
                    windNum = 1;
                }
            }
            if (screen == Screen.Main)
            {

            }
            if (screen == Screen.Animation)
            {

            }
            if (screen == Screen.Death)
            {

            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (screen == Screen.Start)
            {
                //_spriteBatch.Draw(window, Color.AliceBlue);
            }
            if (screen == Screen.Main)
            {

            }
            if (screen == Screen.Animation)
            {

            }
            if (screen == Screen.Death)
            {

            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}