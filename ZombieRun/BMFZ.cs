using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace ZombieRun
{
    public class BMFZ : Sprite
    {
        public float speed;
        public bool isAlive;
        public bool grounded;
        private int x_accel;
        private double friction;
        public double x_vel;
        public double y_vel;
        public int movedX;
        private Vector2 direction;
        private bool pushing;
        public double gravity = 1.0;
        public int maxFallSpeed = 10;
        private int jumpPoint = 0;
        private float destination;
        public int myscore;
        public float currSpeed;
        public bool rightCollision;
        Random rnd = new Random();
        private int picNum;
        private Vector2 prevPos;
        private int frame = 0;
        private Texture2D tempText;


        public BMFZ(Game myGame) :
            base(myGame)
        {
            textureName = "w";
            speed = 1.0f;
            currSpeed = speed;
            friction = 2;
            x_accel = 0;
            y_vel = 0;
            x_vel = 0;
            movedX = 0;
            position.X = 50;
            position.Y = 500;
            isAlive = true;
            rightCollision = false;
        }

        public override void LoadContent()
        {
            //game.Content.Load<Texture2D>("BMFZ.png");
            //tempText = game.Content.Load<Texture2D>("BMFZ.png");

            for (int i = 1; i < 4; i++)
            {
                tempText = game.Content.Load<Texture2D>("BMFZ " + i + ".png");
                animation.Add(tempText);
            }
            texture = animation[picNum];
        }

        public float Width
        {
            get { return texture.Width; }
        }
        public float Height
        {
            get { return texture.Height; }
        }

        public Rectangle HitBox
        {
            get { return new Rectangle(((int)position.X-60), ((int)position.Y-80), texture.Width, texture.Height); }

        }

        public void Update(GameTime gameTime)
        {

            //Move();
            //position.X = MathHelper.Clamp(position.X, -50 + texture.Width / 2, 2000 - texture.Width / 2);
            moving = true;
            frame++;
            if (moving)
            {
                if (frame % 8 == 0)
                {
                    texture = animation[picNum];
                    picNum++;
                    //texture = animation[picNum];
                    if (picNum == 3)
                    {
                        picNum = 1;
                    }
                }
                //texture = animation[picNum];
                //Console.WriteLine(x_vel);
                prevPos = position;
            }
        }

        public void Move()
        {

            if (destination > this.position.X)
            {

                position.X += currSpeed;
            }
        }

        public void getPlayerPosition(Player player1, GameTime gameTime)
        {
            destination = player1.position.X;
            if (player1.position.X == 5)
            {
                //Jump(gameTime);
                position.Y -= 100;
            }
        }

        public void Draw(SpriteBatch batch)
        {
             //if (texture != null)
            //{
                Vector2 drawPosition = new Vector2(50,650);
                drawPosition.X -= texture.Width / 2;
                drawPosition.Y -= texture.Height / 2;
                batch.Draw(texture, drawPosition, null, Color.White, 0, new Vector2 (0,0), 1, SpriteEffects.None, 0);
            //}

        }

        public void Jump(GameTime gameTime)
        {
            y_vel = -10;
            jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
            grounded = false;
        }
        
        
    }

}
