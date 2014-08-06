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



        public ZombieRun()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
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

            font = Content.Load<SpriteFont>("main_font");

            SoundPlayer player = new SoundPlayer("Content/Zander Noriega - Abelian.wav");
             
            player.Play();
            MediaPlayer.Volume = 0.5f;
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
            offscreenZombieGenerator();

            

            player1.LoadContent();
            player1.position = new Vector2(100, 700);
            


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
                b.position = new Vector2(x1 - b.Width, 700 - b.Height);
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

            //for loop trial for "randomly" placing tiles


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
                b.position = new Vector2(1100, 600 - (b.Height / 4));
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
                b.position = new Vector2(x1, 600);
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
                allBlocks.Add(b);
                midBlocks.Add(b);

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

                b.position = new Vector2(1100, 600 - (b.Height / 4));
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
                b.position = new Vector2(x1, 600);
                x1 += (int)b.Width;

            }

            //make last block a barrier too, make this 50/50?
            if (rnd.Next(0, 100) < 50)
            {
                block b = new block(this);
                b.setTexture("longBrick3");
                
                b.LoadContent();
                b.position = new Vector2(x1 - b.Width, 600 - (b.Height / 4));
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
            int count = rnd.Next(7, 12);
            for (int i = 0; i <= count; i++)
            {
                x1 = rnd.Next(1100, 1900);
                Zombie z = new Zombie(this);
                //z.speed = (float)rnd.Next(5, 15) / 10;
                float tempSpeed = 1.0f;
                int caseSwitch = rnd.Next(1, 6);
                switch (caseSwitch)
                {
                    case 1:
                        tempSpeed = 0.6f;
                        break;
                    case 2:
                        tempSpeed = 0.8f;
                        break;
                    case 3:
                        tempSpeed = 1.0f;
                        break;
                    case 4:
                        tempSpeed = 1.2f;
                        break;
                    case 5:
                        tempSpeed = 1.4f;
                        break;
                    case 6:
                        tempSpeed = 1.6f;
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
                        if (z.position.X < (b.position.X + b.Width) && z.position.X > b.position.X  && (rnd.Next(1,4) <= 2))
                        {
                            z.position.Y = b.position.Y - z.Height;
                            z.grounded = true;
                            z.speed = 0.7f;
                        }
                    }
                
                zombies.Add(z);
            }
        }

    /*    public void addBlocks()
        {
            initBlocks = allBlocksAdd.Count;
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
     */
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
                for (int i = 0; i < HighScoreList.Count; i++)
                {
                    //sw.WriteLine("highscore");
                    //sw.WriteLine();
                    sw.WriteLine(HighScoreList[i]);
                }
                Process.Start(path + "\\high_score_list.txt");
            }
        }

        public int retrieveHighScore()
        {
            string[] getHighScore;
            string path = Path.GetPathRoot(Environment.SystemDirectory) + "Users\\" + Environment.UserName + "\\Documents\\New Folder";
            if (!Directory.Exists(path))
            {
                return -1;
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


            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            controls.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //score trial
            //this adds 100 points for each dead zombie, then resets its value
            foreach (Zombie z in zombies)
            {
                currentScore = z.getScore();
                myscore += currentScore;
                z.setScore(0);
            }

            //this provides a continuous score increase over time.
            timer = (double) gameTime.ElapsedGameTime.TotalSeconds;
            double scoreModifier = timer * 10;
            myscore += scoreModifier;

            fade = fade - .005f;

            blockcount = allBlocks.Count;
            //initBlocks = allBlocks.Count;
            //update all blocks in array
            if (renewBlocks || allBlocks.Count>=1)
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
                    
                    newBlocks = true;
                    renewBlocks = false;
                    
                   
                }

                initBlocks = allBlocksAdd.Count;
                if (newBlocks || allBlocksAdd.Count>=1)
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

            
                if (newBlocks && allBlocksAdd[allBlocksAdd.Count - 1].position.X <= 500)
                {
                    midLevelGenerator();
                    topLevelGenerator();
                    lowLevelGenerator();
                    renewBlocks = true;
                    newBlocks = false;
                    
                }

                
            

           
            
           
            

            //I AM ALL THAT IS MAN ZOMBIES

            
            for (int i = 0; i < zombies.Count; i++ )
            {
                if (!zombies[i].isAlive)
                {
                    zombies.RemoveAt(i);
                }
            }



            if (zombies.Count <=6 )
            {
                //zombieGenerator();
                offscreenZombieGenerator();
            }
           // if (blockcount <= 4)
           // {
           //    midLevelGenerator();
           //     topLevelGenerator();
           // }
           
            //foreach (block b in allBlocks)
            //{
              //  b.Update();
                //if (!b.visible)
                //{
                  //  allBlocks.Remove(b);
                //}
            //}
            //block1.Update();
            //block2.Update();
            //block3.Update();
            //CheckCollisions();

           

            player1.Update(controls, gameTime, allBlocks, allBlocksAdd);

            foreach (Zombie z in zombies)
            {
                if (z.isAlive)
                {

                    List<block> tempList = new List<block>();
                    tempList.AddRange(allBlocks);
                    tempList.AddRange(allBlocksAdd);
                    z.Update(gameTime, tempList);
                    z.getPlayerPosition(player1);
                    z.checkBulletCollision(bullets);
                    if (z.position.X < -15)
                    {
                        z.isAlive = false;
                    }
                }

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && pastKey.IsKeyUp(Keys.Space))
            {
                Shoot();

            }

            

            UpdateBullets();
            
            pastKey = Keyboard.GetState();

            
            
            
            base.Update(gameTime);
        }

        
        public void UpdateBullets()
        {

            

            foreach (Bullet b in bullets)
            {
                b.checkForBoxCollision(allBlocks);
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
            string spacebar = "press spacebar to shoot";
            string movement = "up to jump, arrow keys to move";
            spriteBatch.DrawString(font, spacebar, new Vector2(50, 200),Color.WhiteSmoke*fade);
            spriteBatch.DrawString(font, movement, new Vector2(50, 225), Color.WhiteSmoke * fade);

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

            bool alive = true;
            if (player1.checkZombieCollisions(zombies))
            {
                string gameOver = "GAME OVER";
                alive = false;
                spriteBatch.DrawString(font, gameOver, new Vector2(412, 300), Color.Crimson);
                highScore = retrieveHighScore();
                if (highScore <= (int)Math.Floor(myscore))
                {
                    highScore = (int)Math.Floor(myscore);
                    storeHighScore(highScore);
                }
                Exit();

            }
           
          
                spriteBatch.End();


                // TODO: Add your drawing code here
            
                base.Draw(gameTime);

 
        }



        
    }
}
