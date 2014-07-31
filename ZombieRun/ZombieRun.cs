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
        //block block2;
        //block block1;
        //block block3;
        block[] blocks;
        Bullet bulletBill;
        List<Bullet> bullets = new List<Bullet>();
        Random rnd = new Random();
        KeyboardState pastKey;
        Controls controls;

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
            bulletBill = new Bullet(this);
            bulletBill.LoadContent();
            bulletBill.position = new Vector2(100, 700);
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
            blocks = new block[rnd.Next(6,15)];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new block(this);
                blocks[i].LoadContent();
            }

            //for loop trial for "randomly" placing tiles
            int x1;
            int y1;

            for (int i = 0; i < blocks.Length/2; i++)
            {
                x1 = rnd.Next(100, 700);
                y1 = 600;
                blocks[i].position = new Vector2(x1, y1);
               
            }

            for (int i = blocks.Length / 2; i < blocks.Length; i++)
            {

                x1 = rnd.Next(100, 700);
                y1 = 400;
                blocks[i].position = new Vector2(x1, y1);
            }

                //refinement of block placement
                for (int i = 1; i < blocks.Length; i++)
                {
                    if (blocks[i - 1].position.X >= (blocks[i].position.X + 50))
                    {
                        blocks[i].position.X = blocks[i - 1].position.X + 100;
                    }
                    if (blocks[i - 1].position.Y >= (blocks[i].position.Y - 25))
                    {
                        blocks[i].position.Y = blocks[i - 1].position.Y - 75;
                    }
                }


            player1.LoadContent();
            player1.position = new Vector2(100, 700);


            // TODO: use this.Content to load your game content here
        }

        
        

        //protected void CheckCollisions()
        //{
        //    float radius = player1.Width / 2;
        //    if ((player1.position.X >= (block1.position.X - block1.Width / 2) && player1.position.X <= (block1.position.X + block1.Width / 2)))
        //        if ((player1.position.Y > (block1.position.Y - block1.Height+10)))
        //            player1.grounded = true;
        //        else
        //            player1.grounded = false;

        //    //check for block
            
        //}
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            controls.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player1.Update(controls, gameTime, blocks);

            //update all blocks in array
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].Update();
            }
            //block1.Update();
            //block2.Update();
            //block3.Update();
            //CheckCollisions();
            if (Keyboard.GetState().IsKeyDown(Keys.A) && pastKey.IsKeyUp(Keys.A)){
                Shoot();
                //if (bulletBill.isVisible == false)
                //    bulletBill.isVisible = true;
                //else
                //    bulletBill.isVisible = false;
        }
            pastKey = Keyboard.GetState();
            bulletBill.Update();
            UpdateBullets();
            
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
            newB.position.Y -= 10;
            newB.position.X += 10;
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

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].Draw(spriteBatch);
            }
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);
            if(bulletBill.isVisible)
                bulletBill.Draw(spriteBatch);
            //block1.Draw(spriteBatch);
            //block2.Draw(spriteBatch);
            //block3.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }



        
    }
}
