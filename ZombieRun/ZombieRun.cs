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
        Player player1;
        block block2;
        block block1;
        block block3;
        block[] blocks;
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
            player1 = new Player(this);
            block1 = new block(this);
            block1.LoadContent();
            block1.position = new Vector2(405, 686);
            block2 = new block(this);
            block2.LoadContent();
            block2.position = new Vector2(300, 686);
            block3 = new block(this);
            block3.LoadContent();
            block3.position = new Vector2(500, 633);
            
            blocks = new block[3];
            blocks[0] = block1;
            blocks[1]= block2;
            blocks[2] = block3;

            player1.LoadContent();
            player1.position = new Vector2(200, 700);


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
            block1.Update();
            block2.Update();
            block3.Update();
            //CheckCollisions();
            base.Update(gameTime);
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
            block1.Draw(spriteBatch);
            block2.Draw(spriteBatch);
            block3.Draw(spriteBatch);
            player1.Draw(spriteBatch);
            
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }



        
    }
}
