#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Tao.Sdl;
using System.Media;
using System.Timers;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace ZombieRun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZombieRun : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bgTexture;
        Texture2D line;
        Player player1;
        bool playerShooting;
        private bool paused = false; 
        private bool pauseKeyDown = false;
        private bool pausedForGuide = false;
        BMFZ bmfz;
        float timeSinceLastSpawn = 0F;
        float timeSinceLastShot = 0F;
        bool roundStarted = true;
        bool alive = true;
        //block block2;
        //block block1;
        //block block3;
        List<block> allBlocks = new List<block>();
        List<block> midBlocks = new List<block>();
        List<block> topBlocks = new List<block>();
        List<block> lowBlocks = new List<block>();
        Random rnd = new Random();
        Controls controls;
        KeyboardState pastKey;
        int x1;
        int y1;
        List<Bullet> bullets = new List<Bullet>();
        List<Zombie> zombies = new List<Zombie>();
                        

        //Score trial
        SpriteFont font;
        double myscore;
        int currentScore;
        int highScore = 0;
        double timer;
        float fade = 1;
        public static List<int> HighScoreList { get; set; }

        int blockcount;
        int initBlocks;

        //add new blocks trial
        List<block> allBlocksAdd = new List<block>();
        List<block> midBlocksAdd = new List<block>();
        List<block> topBlocksAdd = new List<block>();
        List<block> lowBlocksAdd = new List<block>();
        bool newBlocks = false;
        bool renewBlocks = true;

        //for song audio
        Song song;
        Song gunShot;
        Song beep;
        SoundEffect death;
        SoundEffect zombieKill;
        



        public ZombieRun()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
        }

        private void BeginPause(bool UserInitiated)
        {
            paused = true;
            pausedForGuide = !UserInitiated;
            //TODO: Pause Audio playback
            //Todo: pause controller vibration
        }

        private void EndPause()
        {
            paused = false;
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

            base.Initialize();

            Joystick.Init();
            Console.WriteLine("Number of Joysticks: " + Sdl.SDL_NumJoysticks());
            controls = new Controls();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// All loadings should be done in LoadContent -tai
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            bgTexture = Content.Load<Texture2D>("background");
            line = Content.Load<Texture2D>("markerline");
            player1 = new Player(this);
            bmfz = new BMFZ(this);
            font = Content.Load<SpriteFont>("main_font");
            zombieKill = Content.Load<SoundEffect>("zdeath4");
            
            death = Content.Load<SoundEffect>("death3");
                        
            SoundPlayer player = new SoundPlayer("Content/Zander Noriega - Abelian.wav");
             
            player.PlayLooping();
            MediaPlayer.Volume = 1.0f;
            MediaPlayer.Play(song);


            //block1 = new block(this);
            //block1.LoadContent();
            //block1.position = new Vector2(470, 600);//position.y = 660, the character doesn't get underneath. I need to adjust the ratio in the class.
            //block2 = new block(this);
            //block2.LoadContent();
            //block2.position = new Vector2(300, 650);
            //block3 = new block(this);
            //block3.LoadContent();
           // block3.position = new Vector2(250, 650);
            
            //set up block array, load all block content
            
            //for (int i = 0; i < rnd.Next(6,15); i++)
            //{
            //    block b = new block(this);
            //    b.LoadContent();
            //    blocks.Add(b);
            //}

            ////for loop trial for "randomly" placing tiles
            
            
            //foreach(block b in blocks){
            //    x1 = rnd.Next(100, 700);
            //    y1 = 600;
            //    b.position = new Vector2(x1, y1);

            //}

            midLevelGenerator();
            topLevelGenerator();
            lowLevelGenerator();
            offscreenZombieGenerator();
           
            //refinement of block placement
            //for (int i = 1; i < blocks.Count; i++)
            //{
            //    if (blocks[i - 1].position.X >= (blocks[i].position.X + 50))
            //    {
            //        blocks[i].position.X = blocks[i-1].position.X + 100;
            //    }
            //    if (blocks[i - 1].position.Y >= (blocks[i].position.Y - 25))
            //    {
            //        blocks[i].position.Y = blocks[i-1].position.Y - 75;
            //    }
            //}
            //offscreenZombieGenerator();

            
            
            player1.LoadContent();
            player1.position = new Vector2(300, 700);
            bmfz.LoadContent();
            bmfz.position = new Vector2(50, 700);

            // TODO: use this.Content to load your game content here
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// 
        //I AM ALL THAT IS MAN TRIAL ZOMBIES
        public void zombieGenerator()
        {
            //zombies spawn
            for (int i = 0; i < rnd.Next(5, 9); i++)
            {
                Zombie Z = new Zombie(this);
                Z.LoadContent();
                zombies.Add(Z);
            }

            int flip = 0;
            foreach (Zombie z in zombies)
            {
                flip = rnd.Next(0, 1);
                x1 = rnd.Next(400, 1000);
                if (flip == 0)
                {
                    z.position = new Vector2(x1, 700);
                }
                else
                    z.position = new Vector2(x1, 400);
            }
        }

        public void lowLevelGenerator()
        {
            x1 = 1900;
            //End Barrier, let's make it a 25% chance that a barrier occurs here
            if (rnd.Next(0, 100) < 10)
            {
                block b = new block(this);
                b.setTexture("longBrick3");
                b.LoadContent();
                b.position = new Vector2(1900, 700-(b.Height/4));
                x1 += (int)b.Width;
                allBlocks.Add(b);
            }

            for (int i = 0; i < rnd.Next(2, 4); i++)
            {
                block b = new block(this);
                b.LoadContent();
                lowBlocks.Add(b);
                allBlocks.Add(b);
            }

            //for loop trial for "randomly" placing tiles


            foreach (block b in lowBlocks)
            {
                b.position = new Vector2(x1, 700);
                x1 += (int)b.Width;

            }

            //make last block a barrier too, make this 50/50?
            if (rnd.Next(0, 100) < 50)
            {
                block b = new block(this);
                b.setTexture("longBrick3");
                
                b.LoadContent();
                b.position = new Vector2(x1 + b.Width, 700 - (b.Height / 4)); 
                x1 += (int)b.Width;
                allBlocks.Add(b);
                lowBlocks.Add(b);
            }

           
        }

        public void lowLevelGeneratorAdd()
        {
            x1 = 1900;
            //End Barrier, let's make it a 25% chance that a barrier occurs here
            if (rnd.Next(0, 100) < 10)
            {
                block b = new block(this);
                b.setTexture("longBrick3");
                b.LoadContent();
                x1 += (int)b.Width;
                b.position = new Vector2(1900, 700 - (b.Height / 4));
               
                
                allBlocksAdd.Add(b);
            }

            for (int i = 0; i < rnd.Next(2, 4); i++)
            {
                block b = new block(this);
                b.LoadContent();
                lowBlocksAdd.Add(b);
                allBlocksAdd.Add(b);
            }

            //for loop trial for "randomly" placing tilesfoff


            foreach (block b in lowBlocksAdd)
            {
                b.position = new Vector2(x1, 700);
                x1 += (int)b.Width;

            }

            //make last block a barrier too, make this 50/50?
            if (rnd.Next(0, 100) < 50)
            {
                block b = new block(this);
                b.setTexture("longBrick3");
                b.LoadContent();
                b.position = new Vector2(x1, 700 - (b.Height/4));
                x1 += (int)b.Width;
                allBlocksAdd.Add(b);
                lowBlocksAdd.Add(b);

            }

            //ZOMBIE GENERATION
            /*for (int i = 0; i < rnd.Next(3, 6); i++)
            {
                Zombie Z = new Zombie(this);
                Z.LoadContent();
                zombies.Add(Z);
            }

            int flip = 0;
            foreach (Zombie z in zombies)
            {
                flip = rnd.Next(0, 1);
                int x2 = rnd.Next(1900, x1);
                /*if (flip == 0)
                {
                    z.position = new Vector2(x1, 700);
                }
                else
                    z.position = new Vector2(x1, 400);
                 
                z.position = new Vector2(x2, 400);
            } */
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        public void midLevelGenerator()
        {
            x1 = 1100;
            //End Barrier, let's make it a 25% chance that a barrier occurs here
            if (rnd.Next(0, 100) < 10)
            {
                block b = new block(this);
                b.setTexture("longBrick3");
                
                b.LoadContent();
                b.position = new Vector2(x1, 600 - (b.Height / 4));
                x1 += (int)b.Width;
                allBlocks.Add(b);
                


            }

            for (int i = 0; i < rnd.Next(2,4); i++)
            {
                block b = new block(this);
                b.LoadContent();
                midBlocks.Add(b);
                allBlocks.Add(b);
            }

            //for loop trial for "randomly" placing tiles


            foreach (block b in midBlocks)
            {
                b.position = new Vector2(x1, 600 - (b.Height/4));
                x1 += (int)b.Width;

            }

            //make last block a barrier too, make this 50/50?
            if (rnd.Next(0, 100) < 50)
            {
                block b = new block(this);
                b.setTexture("longBrick3");

                b.LoadContent();
                b.position = new Vector2(x1, 600 - (b.Height / 4));
                x1 += (int)b.Width;
                allBlocksAdd.Add(b);

            }
        }

        public void midLevelGeneratorAdd()
        {
            x1 = 1100;
            //End Barrier, let's make it a 25% chance that a barrier occurs here
            if (rnd.Next(0, 100) < 10)
            {
                block b = new block(this);
                b.LoadContent();
                b.setTexture("longBrick3");

                b.position = new Vector2(x1, 600 - (b.Height / 4));
                x1 += (int)b.Width;
                allBlocksAdd.Add(b);



            }

            for (int i = 0; i < rnd.Next(2, 4); i++)
            {
                block b = new block(this);
                b.LoadContent();
                midBlocksAdd.Add(b);
                allBlocksAdd.Add(b);
            }

            //for loop trial for "randomly" placing tiles


            foreach (block b in midBlocksAdd)
            {
                b.position = new Vector2(x1, 600 - (b.Height / 4));
                x1 += (int)b.Width;

            }

            //make last block a barrier too, make this 50/50?
            if (rnd.Next(0, 100) < 50)
            {
                block b = new block(this);
                b.setTexture("longBrick3");
                
                b.LoadContent();
                b.position = new Vector2(x1, 600 - (b.Height / 4));
                x1 += (int)b.Width;
                allBlocksAdd.Add(b);
                
            }
        }

        public void topLevelGeneratorAdd()
        {
            for (int i = 0; i < rnd.Next(2, 8); i++)
            {
                block b = new block(this);
                b.LoadContent();
                topBlocksAdd.Add(b);
                allBlocksAdd.Add(b);
            }

            //for loop trial for "randomly" placing tiles


            x1 = 1400;
            foreach (block b in topBlocksAdd)
            {
                b.position = new Vector2(x1, 350);
                x1 += (int)b.Width;

            }
        }

        public void topLevelGenerator()
        {
            for (int i = 0; i < rnd.Next(2, 8); i++)
            {
                block b = new block(this);
                b.LoadContent();
                topBlocks.Add(b);
                allBlocks.Add(b);
            }

            //for loop trial for "randomly" placing tiles
            Zombie z = new Zombie(this);
            z.LoadContent();
            z.position.Y = 350 - z.Height;
            z.position.X = 1500;

            x1 = 1400;
            foreach (block b in topBlocks)
            {
                b.position = new Vector2(x1, 350);
                x1 += (int)b.Width;

            }
        }


        public void offscreenZombieGenerator()
        {
            int count = rnd.Next(12, 20);
            for (int i = 0; i <= count; i++)
            {
                x1 = rnd.Next(1050, 2400);
                Zombie z = new Zombie(this);
                //z.speed = (float)rnd.Next(5, 15) / 10;
                float tempSpeed = 1.0f;
                int caseSwitch = rnd.Next(0, 7);
                switch (caseSwitch)
                {
                    case 1:
                        tempSpeed = 3.2f;
                        break;
                    case 2:
                        tempSpeed = 3.3f;
                        break;
                    case 3:
                        tempSpeed = 3.4f;
                        break;
                    case 4:
                        tempSpeed = 3.5f;
                        break;
                    case 5:
                        tempSpeed = 3.6f;
                        break;
                    case 6:
                        tempSpeed = 3.7f;
                        break;
                }
                z.speed = tempSpeed;
                z.position.X = x1;
                z.LoadContent();
                List<block> temp = new List<block>();
                temp.AddRange(allBlocks);
                temp.AddRange(allBlocksAdd);
                foreach (block b in temp)
                    {
                        if (z.position.X < (b.position.X + b.Width) && z.position.X > b.position.X  && (rnd.Next(1,6) <= 4))
                        {
                            if (z.position.Y == (700 - z.Height))
                            {
                                z.position.Y = (700 - b.Height - z.Height);
                            } 
                            z.position.Y = b.position.Y - z.Height;
                            z.grounded = true;
                            z.speed = 3.4f;
                            //z.currSpeed = 2.5f;
                        }
                    }
                List<block> newTemp = new List<block>();
                newTemp.AddRange(lowBlocks);
                newTemp.AddRange(lowBlocksAdd);
                foreach (block b in newTemp)
                {
                    if (z.position.X < (b.position.X + b.Width) && z.position.X > b.position.X)
                    {
                        //.WriteLine("ZOMBIE IS IN THE FUCKING BLOCK");
                        z.position.Y = 700f - b.Height - z.Height;
                        //Console.WriteLine(z.position.Y);
                    }
                }
                zombies.Add(z);
            }
        }

    
        public void storeHighScore(int score)
        {
            HighScoreList = new List<int>();
            HighScoreList.Add(score);
            //HighScoreList.Add(100);

            //creates or opens the file (Path) if it already exists
            string path = Path.GetPathRoot(Environment.SystemDirectory) + "Users\\" + Environment.UserName + "\\Documents\\New Folder";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter sw = File.CreateText(path + "\\high_score_list.txt"))
            {
            //  for (int i = 0; i < HighScoreList.Count; i++)
            //  {
                    //sw.WriteLine("highscore");
                    //sw.WriteLine();
                    sw.WriteLine(score);
               // }
                Process.Start(path + "\\high_score_list.txt");
            }
        }

        public int retrieveHighScore()
        {
            string[] getHighScore;
            string path = Path.GetPathRoot(Environment.SystemDirectory) + "Users\\" + Environment.UserName + "\\Documents\\New Folder";
            if (!Directory.Exists(path))
            {
                return 0;
            }
            else
            {
                //using (StreamReader sr = File.ReadAllLines(path + "\\high_score_list.txt"))
                //{
                getHighScore = File.ReadAllLines(path + "\\high_score_list.txt");
                return Convert.ToInt32(getHighScore[0]);
                //}        
            }
           
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (controls.onPress(Keys.P, Buttons.Start))
            {
                if (paused == true)
                    paused = false;
                else
                    paused = true;
            }

            if (paused && alive)
            {
                if (!roundStarted)
                {
                    //ToDo: title splash screen
                }
                else if(roundStarted)
                {
                    if (controls.onPress(Keys.Q, Buttons.B))
                    {
                        highScore = retrieveHighScore();
                        if (highScore <= (int)Math.Floor(myscore))
                        {
                            highScore = (int)Math.Floor(myscore);
                            storeHighScore(highScore);
                        }
                        death.Play();
                        Exit();
                    }
                }
            }
            if (paused && !alive)
            {
               

                if (controls.onPress(Keys.Q, Buttons.B))
                {
                    highScore = retrieveHighScore();
                    if (highScore <= (int)Math.Floor(myscore))
                    {
                        highScore = (int)Math.Floor(myscore);
                        storeHighScore(highScore);
                    }
                    Exit();
                }

                else if (controls.onPress(Keys.R, Buttons.A))
                 {
                     highScore = retrieveHighScore();
                     if (highScore <= (int)Math.Floor(myscore))
                     {
                         highScore = (int)Math.Floor(myscore);
                         storeHighScore(highScore);
                     }
                    GameTime gt = new GameTime(); 
                    alive = true;
                    player1 = new Player(this); 
                    player1.LoadContent(); 
                    player1.position = new Vector2(300, 700); 
                    for (int i = 0; i < allBlocks.Count; i++) 
                    { 
                        allBlocks.RemoveAt(i);
                        i--;
                    } 
                    for (int i = 0; i < allBlocksAdd.Count; i++) 
                    {
                        allBlocksAdd.RemoveAt(i);
                        i--;
                    } 
                    for (int i = 0; i < zombies.Count; i++) 
                    { 
                        zombies.RemoveAt(i);
                        i--;
                    } 
                    midLevelGenerator(); 
                    topLevelGenerator(); 
                    lowLevelGenerator();
                    //trial
                    midLevelGeneratorAdd();
                    topLevelGeneratorAdd();
                    lowLevelGeneratorAdd();
                    SoundEffect death = Content.Load<SoundEffect>("death3");
                    death.Play();
                    offscreenZombieGenerator(); 
                    myscore = 0; 
                    paused = false; 
                    Update(gt); 
                }
                
            }
            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            controls.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!paused)
            {
                timeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
                timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (Zombie z in zombies)
                {
                    currentScore = z.getScore();
                    myscore += currentScore;
                    z.setScore(0);
                }

                //this provides a continuous score increase over time.
                timer = (double)gameTime.ElapsedGameTime.TotalSeconds;
                double scoreModifier = timer * 10;
                myscore += scoreModifier;

                fade = fade - .002f;

                blockcount = allBlocks.Count;
                //initBlocks = allBlocks.Count;
                //update all blocks in array
                if (renewBlocks || allBlocks.Count >= 1)
                {
                    for (int i = 0; i < allBlocks.Count; i++)
                    {
                        allBlocks[i].Update();
                        if (allBlocks[i].position.X <= -50)
                        {
                            allBlocks.RemoveAt(i);
                            i--;
                        }

                    }
                }


                //I AM ALL THAT IS MAN
                if (renewBlocks && allBlocks[allBlocks.Count - 1].position.X <= 500)
                {
                    //BLOCKS ADD TRIAL
                    // for (int i = 1; i <= 4; i++)
                    // {
                    //     allBlocksAdd.Add(allBlocks[allBlocks.Count - i]);
                    // }
                    midLevelGeneratorAdd();
                    topLevelGeneratorAdd();
                    lowLevelGeneratorAdd();
                    offscreenZombieGenerator();

                    newBlocks = true;
                    renewBlocks = false;


                }

                initBlocks = allBlocksAdd.Count;
                if (newBlocks || allBlocksAdd.Count >= 1)
                {
                    for (int i = 0; i < allBlocksAdd.Count; i++)
                    {
                        allBlocksAdd[i].Update();
                        if (allBlocksAdd[i].position.X <= -50)
                        {
                            allBlocksAdd.RemoveAt(i);
                            i--;
                        }
                    }
                }

                //if (allBlocksAdd.Count >=1 ) {
                    if (newBlocks && allBlocksAdd[allBlocksAdd.Count - 1].position.X <= 500)
                    {
                        midLevelGenerator();
                        topLevelGenerator();
                        lowLevelGenerator();
                        offscreenZombieGenerator();
                        renewBlocks = true;
                        newBlocks = false;
                    //}
                }

                if (timeSinceLastSpawn > 20F )
                {
                    offscreenZombieGenerator();
                    timeSinceLastSpawn = 0f;
                }









                //I AM ALL THAT IS MAN ZOMBIES


                for (int i = 0; i < zombies.Count; i++)
                {
                    if (!zombies[i].isAlive)
                    {
                        zombies.RemoveAt(i);
                    }
                }



                if (zombies.Count <= 6)
                {
                    //zombieGenerator();
                    //offscreenZombieGenerator();
                }

                player1.Update(controls, gameTime, allBlocks, allBlocksAdd);

                bmfz.getPlayerPosition(player1, gameTime);
                bmfz.Update(gameTime);

                float prevX = 0;
                float prevY = 0;
                float currX = 0;
                float currY = 0;
                foreach (Zombie z in zombies)
                {
                    if (z.isAlive)
                    {
                        currX = z.position.X;
                        currY = z.position.Y;

                        List<block> tempList = new List<block>();
                        tempList.AddRange(allBlocks);
                        tempList.AddRange(allBlocksAdd);
                        z.Update(gameTime, tempList);
                        /*if (prevX != 0 && (Math.Abs(prevY - currY) < 5))
                        {
                            if ((currX - prevX) < 5 && !z.rightCollision)
                            {
                                z.position.X += 5;
                            }
                        }*/
                        z.getPlayerPosition(player1);
                        z.checkBulletCollision(bullets);
                        if(!z.isAlive)
                        {
                            zombieKill.Play();
                        }
                        
                        if (z.position.X < -15)
                        {
                            z.isAlive = false;
                        }
                    }
                    prevX = currX;
                    prevY = currY;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && pastKey.IsKeyUp(Keys.Space))
                {
                    if (timeSinceLastShot> 0.1F)
                    {
                        Shoot();
                        timeSinceLastShot = 0;
                        //SoundPlayer player2 = new SoundPlayer("Content/Gunshot.wav");
                        SoundEffect shot = Content.Load<SoundEffect>("Gunshot");
                        shot.Play(.3f, 0f, 0f);
                    }
                }

               

                UpdateBullets();

                pastKey = Keyboard.GetState();
            }
            //score trial
            //this adds 100 points for each dead zombie, then resets its value
            


            
            base.Update(gameTime);
        }

        
        public void UpdateBullets()
        {

            

            foreach (Bullet b in bullets)
            {
                List<block> temp = new List<block>();
                temp.AddRange(allBlocks);
                temp.AddRange(allBlocksAdd);
                b.checkForBoxCollision(temp);
                if (b.isRight)
                {
                    b.position.X += 10;
                
                    if (b.position.X > 1020)
                        b.isVisible = false;
                }
                else
                {
                    b.position.X -= 10;
                
                    if (b.position.X < -10)
                        b.isVisible = false;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }

            




        }

        public void Shoot()
        {

            Bullet newB = new Bullet(this);
            newB.LoadContent();
            newB.position = player1.position;

            if (player1.isRight)
            {
                newB.isRight = true;
                newB.position.X += 20;
            }
            else
            {
                newB.isRight = false;
                newB.position.X -= 20;
                newB.speed = -200;
            }
            //newB.position.Y -= 13;
            newB.isVisible = true;

            if (bullets.Count < 1000)
                bullets.Add(newB);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);
            spriteBatch.Begin();
            ////draw all sprites here
            spriteBatch.Draw(bgTexture, new Vector2(0, 300), Color.White);
            spriteBatch.Draw(line, new Vector2(0, 700), Color.White);
            string spacebar = "press spacebar to shoot";
            string movement = "up to jump, arrow keys to move";

            string pause = "Press 'P' to Pause";
            spriteBatch.DrawString(font, spacebar, new Vector2(50, 200), Color.WhiteSmoke * fade);
            spriteBatch.DrawString(font, movement, new Vector2(50, 225), Color.WhiteSmoke * fade);
            spriteBatch.DrawString(font, pause, new Vector2(50, 250), Color.WhiteSmoke * fade);
            
            string gameover = "Game Over";
            string pausedState = "Paused!";
            string retry = "Press 'R' to Retry";
            string resume = "Press 'P' to Resume";
            string quit = "Press 'Q' to Quit";
            
            //draw score
            //myscore += currentScore;
            string score = "SCORE: " + Math.Floor(myscore);
            spriteBatch.DrawString(font, score, new Vector2(800, 50), Color.WhiteSmoke);

            //draw high score
            string showHighScore = "HIGH SCORE: " + retrieveHighScore();
            spriteBatch.DrawString(font, showHighScore, new Vector2(15, 50), Color.WhiteSmoke);
            

            //draw Welcome to zombie run
            string nothing = "";
            string zombierun = "ZOMBIE RUN";
            //float fade = (float)timer;
            //double timer2 = 0;
            //timer2++;
            
            
            spriteBatch.DrawString(font, zombierun, new Vector2(412, 50), Color.Crimson);
            
                
            //spriteBatch.DrawString(font, nothing, new Vector2(412, 50), Color.Crimson);
       
            
          
            
            //draw instructions
           
            //draw all blocks in array

            foreach (block b in allBlocks)
            {
                b.Draw(spriteBatch, b.GetFlip());
            }

            foreach (block b in allBlocksAdd)
            {
                b.Draw(spriteBatch, b.GetFlip());
            }

            //draw number of blocks in allBlo cks
            //string numBlocks = "Number of blocks remaining: " + blockcount;
            //spriteBatch.DrawString(font, numBlocks, new Vector2(50, 150), Color.WhiteSmoke);
            //string numAddBlocks = "Number of added blocks remaining: " + initBlocks;
            //spriteBatch.DrawString(font, numAddBlocks, new Vector2(50, 175), Color.WhiteSmoke);

            //draw ammo
            string ammo = "AMMO: UNLTD";
            spriteBatch.DrawString(font, ammo, new Vector2(50, 750), Color.WhiteSmoke);

            foreach (Zombie z in zombies)
            {
               
                if(z.isAlive)
                    z.Draw(spriteBatch, z.GetFlip());
            }
            
            //block1.Draw(spriteBatch);
            //block2.Draw(spriteBatch);
            //block3.Draw(spriteBatch);
            foreach (Bullet b in bullets)
            {
                b.position.Y -= 13;
                b.Draw(spriteBatch, b.GetFlip());
                b.position.Y += 13;
            }

            //CHANGE SPRITE TRIAL
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                player1.textureName = "prep2-left";
            }

            player1.Draw(spriteBatch, player1.GetFlip());

            if (bmfz.isAlive)
            {
                bmfz.Draw(spriteBatch);
            }

            if (paused && alive)
            {
                spriteBatch.DrawString(font, pausedState, new Vector2(420, bgTexture.Height / 2), Color.WhiteSmoke);
                spriteBatch.DrawString(font, resume, new Vector2(350, bgTexture.Height / 2 + 50), Color.WhiteSmoke);
                spriteBatch.DrawString(font, quit, new Vector2(350, bgTexture.Height / 2 + 100), Color.WhiteSmoke);

            }
            else if (paused && !alive)
            {
                spriteBatch.DrawString(font, gameover, new Vector2(420, bgTexture.Height / 2), Color.WhiteSmoke);
                //spriteBatch.DrawString(font, retry, new Vector2(350, bgTexture.Height / 2 + 50), Color.WhiteSmoke);
                spriteBatch.DrawString(font, quit, new Vector2(350, bgTexture.Height / 2 + 100), Color.WhiteSmoke);

            }
            
            if (player1.checkZombieCollisions(zombies, bmfz))
            {
                
                alive = false;
                paused = true;
                allBlocks.Clear();
                allBlocksAdd.Clear();
                zombies.Clear();
                
                

                

            }
           
          
                spriteBatch.End();


                // TODO: Add your drawing code here
            
                base.Draw(gameTime);

 
        }





        
    }
}



