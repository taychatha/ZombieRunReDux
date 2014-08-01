#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Tao.Sdl;
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
        Random rnd = new Random();
        Controls controls;
        KeyboardState pastKey;
        int x1;
        int y1;
        List<Bullet> bullets = new List<Bullet>();
        List<Zombie> zombies = new List<Zombie>();
         

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
            //zombies spawn
            for (int i = 0; i < rnd.Next(5,9); i++)
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
       
        
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        public void midLevelGenerator()
        {
            x1 = 200;
            //End Barrier, let's make it a 25% chance that a barrier occurs here
            if (rnd.Next(0, 100) < 25)
            {
                block b = new block(this);
                b.LoadContent();
                b.position =new Vector2(200, 600);
                block b2 = new block(this);
                b2.LoadContent();
                b2.position = new Vector2(200, (600 - b2.Height));
                x1 += (int)b.Width;
                allBlocks.Add(b);
                allBlocks.Add(b2);
                


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
                b.LoadContent();
                b.position = new Vector2(x1-b.Width, 600-b.Height);
                x1 += (int)b.Width;
                allBlocks.Add(b);
                midBlocks.Add(b);

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


            x1 = 500;
            foreach (block b in topBlocks)
            {
                b.position = new Vector2(x1, 350);
                x1 += (int)b.Width;

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


            
            //update all blocks in array
            foreach (block b in allBlocks)
                b.Update();
            
            //block1.Update();
            //block2.Update();
            //block3.Update();
            //CheckCollisions();
            player1.Update(controls, gameTime, allBlocks);

            foreach (Zombie z in zombies)
            {
                if (z.isAlive)
                {

                    z.Update(gameTime, midBlocks);

                    z.getPlayerPosition(player1);
                    z.checkBulletCollision(bullets);
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
                b.position.X += 10;
                if (b.position.X > 1020)
                    b.isVisible = false;
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
            newB.position.X += 20;
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

            //draw all blocks in array

            foreach (block b in allBlocks)
            {
                b.Draw(spriteBatch);
            }

            

            foreach (Zombie z in zombies)
            {
               
                if(z.isAlive)
                    z.Draw(spriteBatch);
            }
            
            //block1.Draw(spriteBatch);
            //block2.Draw(spriteBatch);
            //block3.Draw(spriteBatch);
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);
            
            player1.Draw(spriteBatch);
            
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }



        
    }
}
