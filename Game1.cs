using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        Rectangle window, startBtnRect, whiteManRect, playerHealthBarRect;
        Vector2 playerSpeed, snakeSpeed;
        Texture2D introScreenText, startBtnText, animationOneOne, animationOneTwo, animationOneThree, 
        animationOneFour, walkingLeftOne, walkingLeftTwo, walkingLeftThree, walkingRightOne, walkingRightTwo, walkingRightThree, stageOneBG, jumpingRight, jumpingLeft, fillerText;
        SoundEffect windOne, windTwo, windThree, windFour, birds;
        Song introSong;
        SoundEffectInstance windOneInstance, windTwoInstance, windThreeInstance, windFourInstance;
        float snakeattackSpeed, gravitationalAcceleration = 0;
        int walkAnimation = 0, windNum = 1, introFade = 0, seconds = 0, walkingAnimationStep = 2;
        bool startAnimation, cobraAnimation, waterAnimation, walking, fade, ptIntro = false;
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
            startBtnRect = new Rectangle(300, 470, 180, 90);
            whiteManRect = new Rectangle(50, 400, 60, 90);
            playerSpeed = new Vector2(3 , 45);
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();
            base.Initialize();
            windOneInstance.Play();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            windOne = Content.Load<SoundEffect>("Wind (1)");
            windTwo = Content.Load<SoundEffect>("Wind (2)");
            windThree = Content.Load<SoundEffect>("Wind (3)");
            windFour = Content.Load<SoundEffect>("Wind (4)");
            birds = Content.Load<SoundEffect>("Birds");
            introSong = Content.Load<Song>("Bill Withers - Ain't No Sunshine");
            introScreenText = Content.Load<Texture2D>("Intro screen");
            startBtnText = Content.Load<Texture2D>("Start button");
            animationOneOne = Content.Load<Texture2D>("Animation One One");
            animationOneTwo = Content.Load<Texture2D>("Animation One Two");
            animationOneThree = Content.Load<Texture2D>("Animation One Three");
            animationOneFour = Content.Load<Texture2D>("Animation One Four");
            walkingRightOne = Content.Load<Texture2D>("White man walking right(1)png");
            walkingRightTwo = Content.Load<Texture2D>("White man walking (2) right");
            fillerText = Content.Load<Texture2D>("White man walking (2) right");
            walkingRightThree = Content.Load<Texture2D>("White man walking right(3)");
            walkingLeftOne = Content.Load<Texture2D>("White man walking left(1)png");
            walkingLeftTwo = Content.Load<Texture2D>("White man walking (2) left");
            walkingLeftThree = Content.Load<Texture2D>("White man walking left(3)");
            //jumpingRight = Content.Load<Texture2D>("White man walking left(1)png");
            //jumpingLeft = Content.Load<Texture2D>("White man walking left(1)png");
            stageOneBG = Content.Load<Texture2D>("Animation One Four");
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
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            Window.Title = mouseState.X + "," + mouseState.Y;
            if (screen == Screen.Start)
            {
                if (windNum == 1 && windOneInstance.State == SoundState.Stopped)
                {
                    windNum = 2;
                    windTwoInstance.Play();
                }
                else if(windNum == 2 && windTwoInstance.State == SoundState.Stopped)
                {
                    windNum = 3;
                    windThreeInstance.Play();
                }
                else if (windNum == 3 && windThreeInstance.State == SoundState.Stopped)
                {
                    windNum = 4;
                    windFourInstance.Play();
                }
                else if (windNum == 4 && windFourInstance.State == SoundState.Stopped)
                {
                    windNum = 1;
                    windOneInstance.Play();
                }
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.X < 480 && mouseState.X > 300 && mouseState.Y > 470 && mouseState.Y < 560)
                {
                    fade = true;
                    introFade++;
                    if (introFade == 50)
                    {
                        
                        screen = Screen.Animation;
                    }
                }
                if (fade == true)
                {
                    introFade++;
                    if (introFade == 50)
                    {
                        introFade = 0;
                        screen = Screen.Animation;
                        seconds = 0;
                    }
                }
            }


            if (screen == Screen.Main)
            {
                //Add to top if all chapters and lower to start as you go down!
                if (ptIntro == false)
                {
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        if (playerSpeed.X > 0)
                        {
                            playerSpeed.X = playerSpeed.X * -1;
                        }

                    }
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        if (playerSpeed.X < 0)
                        {
                            playerSpeed.X = playerSpeed.X * -1;
                        }

                    }
                    if (keyboardState.GetPressedKeyCount() == 0 && whiteManRect.Y == 400)
                    {
                        walkAnimation = 2;
                    }
                    else if (whiteManRect.Y == 400)
                    {
                        seconds++;
                        if ((seconds % 20) == 0)
                        {
                            walkAnimation++;
                            if (walkAnimation == 5)
                            {
                                walkAnimation = 1;
                            }
                        }
                    }
                        if ((whiteManRect.X - window.X) >= 340 && (whiteManRect.X - (window.X + 3200)) <= -400 && keyboardState.GetPressedKeyCount() != 0)
                        {
                            window.X = (window.X + (int)playerSpeed.X);

                        }
                        else if (keyboardState.GetPressedKeyCount() != 0)
                        {
                            whiteManRect.X = whiteManRect.X - (int)playerSpeed.X;
                        }
                    
                    if (whiteManRect.X <= window.X)
                    {
                        whiteManRect.X = whiteManRect.X + (int)(playerSpeed.X);
                    }
                    if (whiteManRect.X >= (window.X + 3200))
                    {
                        whiteManRect.X = whiteManRect.X + (int)(playerSpeed.X);
                    }

                            if (keyboardState.IsKeyDown(Keys.W) && whiteManRect.Y == 400)
                    {
                        whiteManRect.Y = whiteManRect.Y + (int)playerSpeed.Y;
                    }
                    if (whiteManRect.Y != 400 && whiteManRect.Y > 400)
                    {
                        playerSpeed.Y = playerSpeed.Y - 1;
                        whiteManRect.Y = (int)(whiteManRect.Y + playerSpeed.Y);
                    }
                    if (whiteManRect.Y < 400)
                    {
                        whiteManRect.Y = 400;
                    }
                    
                }

               
                
                
            }


            if (screen == Screen.Animation)
            {
                if (ptIntro == false)
                {
                    if (seconds < 901)
                    {
                        ++seconds;
                        if (seconds == 340)
                        {
                            birds.Play();
                        }
                        if (seconds == 450)
                        {
                            MediaPlayer.Play(introSong);
                        }
                        if (seconds > 850)
                        {
                            introFade++;
                            MediaPlayer.Volume = ((100f - (introFade))/100);
                        }
                    }
                    if (seconds == 901)
                    {
                        screen = Screen.Main;
                        window = new Rectangle(0, 0, 3200, 600);
                    }
                }
            }


            if (screen == Screen.Death)
            {

            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (screen == Screen.Start)
            {
                _spriteBatch.Draw(introScreenText , window, Color.White* ((100f - (introFade * 2f)) / 100f));
                _spriteBatch.Draw(startBtnText, new Rectangle (290,460, 200, 110), Color.Peru * ((100f - (introFade * 2f))/100f));
                _spriteBatch.Draw(startBtnText, startBtnRect, Color.White * ((100f - (introFade * 2f))/100f));
            }


            if (screen == Screen.Main)
            {
               if (ptIntro == false)
                {
                    _spriteBatch.Draw(stageOneBG, window, Color.White);

                    /*_spriteBatch.Draw(fillerText, whiteManRect, Color.White);*///alanna

                    if (whiteManRect.Y == 400)
                    {
                        if (playerSpeed.X < 0)
                        {
                            if (walkAnimation == 1)
                            {
                                _spriteBatch.Draw(walkingRightOne, whiteManRect, Color.White);
                            }
                            else if (walkAnimation == 2)
                            {
                                _spriteBatch.Draw(walkingRightTwo, whiteManRect, Color.White);
                            }
                            else if (walkAnimation == 3)
                            {
                                _spriteBatch.Draw(walkingRightThree, whiteManRect, Color.White);
                            }
                            else if (walkAnimation == 4)
                            {
                                _spriteBatch.Draw(walkingRightTwo, whiteManRect, Color.White);
                            }

                        }
                        if (playerSpeed.X > 0)
                        {
                            if (walkAnimation == 1)
                            {
                                _spriteBatch.Draw(walkingLeftOne, whiteManRect, Color.White);
                            }
                            else if (walkAnimation == 2)
                            {
                                _spriteBatch.Draw(walkingLeftTwo, whiteManRect, Color.White);
                            }
                            else if (walkAnimation == 3)
                            {
                                _spriteBatch.Draw(walkingLeftThree, whiteManRect, Color.White);
                            }
                            else if (walkAnimation == 4)
                            {
                                _spriteBatch.Draw(walkingLeftTwo, whiteManRect, Color.White);
                            }

                        }
                    }
                    else
                    {
                        if (playerSpeed.X > 0)
                        {
                            _spriteBatch.Draw(jumpingRight, whiteManRect, Color.White);
                        }
                        else if (playerSpeed.X < 0)
                        {
                            _spriteBatch.Draw(jumpingLeft, whiteManRect, Color.White);
                        }
                    }
                }
            }


            if (screen == Screen.Animation)
            {
                if (seconds < 180)
                {
                    _spriteBatch.Draw(animationOneOne, window, Color.White);
                }
                else if (seconds < 420)
                {
                    _spriteBatch.Draw(animationOneTwo, window, Color.White);
                }
                else if (seconds < 660)
                {
                    _spriteBatch.Draw(animationOneThree, window, Color.White);
                }
                else if (seconds <= 900)
                {
                    _spriteBatch.Draw(animationOneFour, window, Color.White * ((100f - (introFade * 2f)) / 100f));

                }
            }
            if (screen == Screen.Death)
            {

            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}