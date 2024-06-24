using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing;
//Was saying it was ambiguous because this and microsoft xna had same shit?

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

        Rectangle window, startBtnRect, whiteManRect, playerHealthBarRect, wButtonRect, tarpRect, bridgeRect,
        crateRect, goblinRect, dart;

        Vector2 playerSpeed, snakeSpeed;

        Texture2D introScreenText, startBtnText, animationOneOne, animationOneTwo, animationOneThree, 
        animationOneFour, walkingLeftOne, walkingLeftTwo, walkingLeftThree, walkingRightOne, walkingRightTwo,
        walkingRightThree, stageOneBGone, stageOneBGtwo, jumpingRight, jumpingLeft, wButtonText, tarpText,
        bridgeText, crateText, goblinText, dartText;
       
        SoundEffect windOne, windTwo, windThree, windFour, birds, doorOpen, sickInk, goblinShoot;

        Song introSong;

        SoundEffectInstance windOneInstance, windTwoInstance, windThreeInstance, windFourInstance, doorOpenInstance, sickInkInstance, goblinShootInstance;

        String stageOneBGstate;

        float snakeattackSpeed, gravitationalAcceleration = 0;

        int walkAnimation = 0, windNum = 1, introFade = 0, seconds = 0, gameTimer = 0, walkingAnimationStep = 2,
        groundLevel = 590, previousGroundLevel = 590, goblinStartTime = 0, dartsX;

        bool startAnimation, cobraAnimation, waterAnimation, walking, fade, ptIntro = false, wButton = false,
            goblinActive = false;

        //List <int> enemyHealth = new List<int>();
        //List <Rectangle> enemies = new List<Rectangle>();
        //List<Rectangle> enemiesHealthBar = new List<Rectangle>();
        List<Rectangle> darts = new List<Rectangle>();
        List<int> dartStartTimes = new List<int>();

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
            //enemyHealth.Clear();
            window = new Rectangle(0, 0, 800, 600);
            startBtnRect = new Rectangle(300, 470, 180, 90);
            whiteManRect = new Rectangle(50, 470, 80, 120);
            wButtonRect = new Rectangle(750, 200, 100, 100);
            tarpRect = new Rectangle(800,400,250,200);
            bridgeRect = new Rectangle(1090,230,175,350);
            crateRect = new Rectangle(1050,490,90,90);
            goblinRect = new Rectangle((bridgeRect.Right - 20), 530, 60, 60);
            dartsX = goblinRect.Right;
            playerSpeed = new Vector2(3 , 13);
            dartStartTimes.Add(generator.Next(30, 300));
            dartStartTimes.Add(generator.Next(30, 300));
            dartStartTimes.Add(generator.Next(30, 300));
            dartStartTimes.Sort();
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
            doorOpen = Content.Load<SoundEffect>("Door open");
            sickInk = Content.Load<SoundEffect>("nice ink (lewis)");
            goblinShoot = Content.Load<SoundEffect>("Dart goblin 2");
            introSong = Content.Load<Song>("Bill Withers - Ain't No Sunshine");
            introScreenText = Content.Load<Texture2D>("Intro screen");
            startBtnText = Content.Load<Texture2D>("Start button");
            animationOneOne = Content.Load<Texture2D>("Animation One One");
            animationOneTwo = Content.Load<Texture2D>("Animation One Two");
            animationOneThree = Content.Load<Texture2D>("Animation One Three");
            animationOneFour = Content.Load<Texture2D>("Animation One Four");
            walkingRightOne = Content.Load<Texture2D>("White man walking right(1)png");
            walkingRightTwo = Content.Load<Texture2D>("White man walking (2) right");
            walkingRightThree = Content.Load<Texture2D>("White man walking right(3)");
            walkingLeftOne = Content.Load<Texture2D>("White man walking left(1)png");
            walkingLeftTwo = Content.Load<Texture2D>("White man walking (2) left");
            walkingLeftThree = Content.Load<Texture2D>("White man walking left(3)");
            jumpingRight = Content.Load<Texture2D>("White man jumping right");
            jumpingLeft = Content.Load<Texture2D>("White man jumping left");
            stageOneBGone = Content.Load<Texture2D>("Stage one BG one");
            stageOneBGtwo = Content.Load<Texture2D>("Stage one BG two");
            wButtonText = Content.Load<Texture2D>("w button");
            tarpText = Content.Load<Texture2D>("tarp");
            crateText = Content.Load<Texture2D>("crate");
            bridgeText = Content.Load<Texture2D>("bridge");
            goblinText = Content.Load<Texture2D>("Goblin door");
            dartText = Content.Load<Texture2D>("dart");
            windOneInstance = windOne.CreateInstance();
            windTwoInstance = windTwo.CreateInstance();
            windThreeInstance = windThree.CreateInstance();
            windFourInstance = windFour.CreateInstance();
            goblinShootInstance = goblinShoot.CreateInstance();
            doorOpenInstance = doorOpen.CreateInstance();
            sickInkInstance = sickInk.CreateInstance();
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
                gameTimer++;
                //Add to top if all chapters and lower to start as you go down!
                if (whiteManRect.X > 750 || window.X <= -390)
                {
                    wButton = true;
                }
                    if (gameTimer % 15 == 0)
                {
                    if (stageOneBGstate == "one")
                    {
                        stageOneBGstate = "two";
                    }
               else if (stageOneBGstate == "two")
                    {
                        stageOneBGstate = "one";
                    }
                }
                if (ptIntro == false)
                {
                    if (darts.Count != 0)
                    {
                        dartsX = dartsX + 4;
                    }
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
                    if (keyboardState.GetPressedKeyCount() == 0 && whiteManRect.Bottom == groundLevel)
                    {
                        walkAnimation = 2;
                    }
                    else if (whiteManRect.Bottom == groundLevel && (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.A)))
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
                        if ((whiteManRect.X - window.X) >= 360 && (whiteManRect.X - (window.X + 3930)) <= -400 && (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.A)))
                        {
                        
                            window.X = (window.X + (int)playerSpeed.X);
                            wButtonRect.X = (wButtonRect.X + (int)playerSpeed.X);
                            tarpRect.X = (tarpRect.X + (int)playerSpeed.X);
                            crateRect.X = (crateRect.X + (int)playerSpeed.X);
                            bridgeRect.X = (bridgeRect.X + (int)playerSpeed.X);
                            goblinRect.X = (goblinRect.X + (int)playerSpeed.X);
                        

                        }
                        else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.A))
                        {
                            whiteManRect.X = whiteManRect.X - (int)playerSpeed.X;
                        }
                    
                    if (whiteManRect.X <= window.X)
                    {
                        whiteManRect.X = whiteManRect.X + (int)(playerSpeed.X);
                    }
                    if (whiteManRect.X >= (window.X + 3920))
                    {
                        whiteManRect.X = whiteManRect.X + (int)(playerSpeed.X);
                    }
                    if (((whiteManRect.Right <= crateRect.Right && playerSpeed.X < 0 && whiteManRect.Right > crateRect.Left) || (whiteManRect.Left >= crateRect.Left && playerSpeed.X > 0 && whiteManRect.Left < crateRect.Right)) && (whiteManRect.Y + 120) > 490)
                    {
                        window.X = (window.X - (int)playerSpeed.X);
                        wButtonRect.X = (wButtonRect.X - (int)playerSpeed.X);
                        tarpRect.X = (tarpRect.X - (int)playerSpeed.X);
                        crateRect.X = (crateRect.X - (int)playerSpeed.X);
                        bridgeRect.X = (bridgeRect.X - (int)playerSpeed.X);
                        goblinRect.X = (goblinRect.X - (int)playerSpeed.X);
                    }
                    if (whiteManRect.Right <= (bridgeRect.Left + 100) && playerSpeed.X < 0 && whiteManRect.Right > (bridgeRect.Left + 95) && whiteManRect.Bottom > (bridgeRect.Top + 75))
                    {
                        window.X = (window.X - (int)playerSpeed.X);
                        wButtonRect.X = (wButtonRect.X - (int)playerSpeed.X);
                        tarpRect.X = (tarpRect.X - (int)playerSpeed.X);
                        crateRect.X = (crateRect.X - (int)playerSpeed.X);
                        bridgeRect.X = (bridgeRect.X - (int)playerSpeed.X);
                        goblinRect.X = (goblinRect.X - (int)playerSpeed.X);
                    }
                    if (whiteManRect.Left >= (bridgeRect.Left + 95) && playerSpeed.X > 0 && whiteManRect.Left <= (bridgeRect.Left + 97) && whiteManRect.Bottom > (bridgeRect.Top + 75))
                    {
                        window.X = (window.X - (int)playerSpeed.X);
                        wButtonRect.X = (wButtonRect.X - (int)playerSpeed.X);
                        tarpRect.X = (tarpRect.X - (int)playerSpeed.X);
                        crateRect.X = (crateRect.X - (int)playerSpeed.X);
                        bridgeRect.X = (bridgeRect.X - (int)playerSpeed.X);
                        goblinRect.X = (goblinRect.X - (int)playerSpeed.X);
                    }
                    if ((whiteManRect.Left + 50) >= bridgeRect.Right && whiteManRect.Bottom > 486 && (whiteManRect.X + 50) <= (bridgeRect.Right + 3))
                    {
                        window.X = (window.X - (int)playerSpeed.X);
                        wButtonRect.X = (wButtonRect.X - (int)playerSpeed.X);
                        tarpRect.X = (tarpRect.X - (int)playerSpeed.X);
                        crateRect.X = (crateRect.X - (int)playerSpeed.X);
                        bridgeRect.X = (bridgeRect.X - (int)playerSpeed.X);
                        goblinRect.X = (goblinRect.X - (int)playerSpeed.X);
                    }
                    if (whiteManRect.Top <= (bridgeRect.Top + 75 ) && whiteManRect.Bottom >= (bridgeRect.Top + 75) && playerSpeed.Y>0 && whiteManRect.Right >= bridgeRect.Left && whiteManRect.Right <= (bridgeRect.Left + 115))
                    {
                        playerSpeed.Y = 0;
                    }
                    else if (whiteManRect.Bottom <= crateRect.Top && whiteManRect.Bottom > (bridgeRect.Top + 75) && (whiteManRect.X + 50) >= crateRect.Left && (whiteManRect.X + 30) < crateRect.Right)
                    {
                        previousGroundLevel = groundLevel;
                        groundLevel = crateRect.Top;
                    }
                    else if (whiteManRect.Bottom <= tarpRect.Top && (whiteManRect.X + 50) >= tarpRect.Left && (whiteManRect.X + 30) < tarpRect.Right)
                    {
                        previousGroundLevel = groundLevel;
                        groundLevel = tarpRect.Top;
                    }
                    else if (whiteManRect.Bottom <= (bridgeRect.Top + 75) && (whiteManRect.X + 50) >= bridgeRect.Left && (whiteManRect.X) < (bridgeRect.Left + 95))
                    {
                        previousGroundLevel = groundLevel;
                        groundLevel = (bridgeRect.Top + 75);
                    }
                    else if ((whiteManRect.X) >= (bridgeRect.Left + 95) && (whiteManRect.X + 50) <= bridgeRect.Right)
                    {
                        previousGroundLevel = groundLevel;
                        groundLevel = 486;
                    }
                    else
                    {
                        previousGroundLevel = groundLevel;
                        groundLevel = 590;
                    }
                    if (previousGroundLevel < groundLevel && whiteManRect.Bottom == previousGroundLevel)
                    {
                        playerSpeed.Y = 0;
                    }
                        
                        if (keyboardState.IsKeyDown(Keys.W) && whiteManRect.Bottom == groundLevel)
                        {
                             whiteManRect.Y = whiteManRect.Y - (int)playerSpeed.Y;
                             gameTimer = 0;
                        }
                    if (whiteManRect.Bottom != groundLevel && whiteManRect.Bottom < groundLevel)
                    {
                        if ((gameTimer % 2) == 0)
                        {
                            playerSpeed.Y = playerSpeed.Y - 1;
                        }
                        whiteManRect.Y = (int)(whiteManRect.Y - playerSpeed.Y);
                    }
                    if (whiteManRect.Bottom > groundLevel)
                    {
                        whiteManRect.Y = groundLevel - 120;
                        playerSpeed.Y = 13;
                    }
                    if ((whiteManRect.X - goblinRect.X) >= 80 && goblinActive == false)
                    {
                        doorOpenInstance.Play();
                        goblinActive = true;

                    }
                    if (doorOpenInstance.State == SoundState.Stopped && goblinActive == true && goblinStartTime == 0)
                    {
                        goblinStartTime = gameTimer;
                    }
                    if (goblinActive == true && darts != 0)
                    {

                        for (int e = 0; e < darts; e++;)
                        {
                            _spriteBatch.Draw(dartText, darts[e], Color.White);
                        }
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
                        stageOneBGstate = "one";
                        screen = Screen.Main;
                        window = new Rectangle(0, 0, 4000, 600);
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
                    if (stageOneBGstate == "one")
                    {
                        _spriteBatch.Draw(stageOneBGone, window, Color.White);
                    }
                    else if (stageOneBGstate == "two")
                    {
                        _spriteBatch.Draw(stageOneBGtwo, window, Color.White);
                    }
                    if (wButton == true)
                    {
                        _spriteBatch.Draw(wButtonText, wButtonRect, Color.White);
                    }
                    _spriteBatch.Draw(tarpText, tarpRect, Color.White);
                    if (whiteManRect.Bottom == groundLevel)
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
                        if (playerSpeed.X < 0)
                        {
                            _spriteBatch.Draw(jumpingRight, whiteManRect, Color.White);
                        }
                        else if (playerSpeed.X > 0)
                        {
                            _spriteBatch.Draw(jumpingLeft, whiteManRect, Color.White);
                        }
                    }
                    if (goblinActive == true)
                    {
                        _spriteBatch.Draw(goblinText, goblinRect, Color.White);
                    }
                    _spriteBatch.Draw(crateText, crateRect, Color.White);
                    _spriteBatch.Draw(bridgeText, bridgeRect, Color.White);
                    if (darts.Count != 0)
                    {
                        for (int i = 0; i < darts.Count; i++)
                        {
                            _spriteBatch.Draw(dartText, darts[i], Color.White);
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