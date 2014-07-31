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
    public class Zombie : Sprite
    {
        public int speed;
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


        public Zombie(Game myGame) :
            base(myGame)
        {
            textureName = "Creeper1";
            speed = 1;
            friction = 2;
            x_accel = 0;
            y_vel = 0;
            x_vel = 0;
            movedX = 0;
            position.X = 800;
            position.Y = 700;
            isAlive = true;
        }

        public float Width
        {
            get { return texture.Width; }
        }
        public float Height
        {
            get { return texture.Height; }
        }

        public void Update(GameTime gameTime, block[] platforms)
        {

            Move(platforms);
            //Jump(gameTime);



        }


        public void getPlayerPosition(Player player1)
        {
            destination = player1.position.X;
        }



        public void Move(block[] platforms)
        {

            if (destination == this.position.X) {
                //x_accel = 0;
            }
            if (destination > this.position.X)
            {
                
                position.X += 1;
            }

            //else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
              //  x_accel -= speed;



            if (destination < this.position.X) { 
               //x_accel -= speed;
                //else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
                //  x_accel += speed;
                position.X -= 1;
        }
            double playerFriction = pushing ? (friction * 3) : friction;
            x_vel = x_vel * (1 - playerFriction) + x_accel * .10;
            movedX = Convert.ToInt32(x_accel);
            position.X += movedX;

            position.X = MathHelper.Clamp(position.X, 10 + texture.Width / 2, 1020 - texture.Width / 2);

            if (!grounded)
            {
                y_vel += gravity;
                if (y_vel > maxFallSpeed)
                    y_vel = maxFallSpeed;
                position.Y += Convert.ToInt32(y_vel);
            }

            else
            {
                y_vel = 1;
            }

            //grounded = false;



            checkYCollisions(platforms);
            //checkPlatformCollisions(platform);
        }


        public void checkBulletCollision(List<Bullet> bullets)
        {

           foreach (Bullet b in bullets)
                {
                    if (BoundingBox.Intersects(b.BoundingBox))
                    {
                        isAlive = false;
                        b.isVisible = false;
                        bullets.Remove(b);
                        break;
                    }

                }
                

            
        
        }


        public void checkYCollisions(block[] platforms)
        {


            if (position.Y > 700)
                grounded = true;
            else
                grounded = false;

            float Xradius = Width / 2;
            float Yradius = Height / 2;
            List<block> collidedBlocks = new List<block>();

            foreach (block p in platforms)
            {
                if ((position.X > (p.position.X - p.Width / 2 - Xradius)) &&
                    (position.X < (p.position.X + p.Width / 2 + Xradius)) &&
                   (position.Y > (p.position.Y - p.Height / 2 - Yradius)) &&//on top
                    ((position.Y < (p.position.Y + p.Height / 2 + Yradius)))
                    )//below
                {
                    collidedBlocks.Add(p);

                }
            }


            //collisions work for all side of blocks. 
            foreach (block p in collidedBlocks)
            {
                if (p != null)
                {
                    if ((BoundingBox.Bottom <
                        (p.BoundingBox.Top/*+ radius*/)))
                    {

                        grounded = true;

                    }

                    else if ((BoundingBox.Top >
                        (p.position.Y + p.Height / 2 /*- Yradius*/)))
                    {
                        if (y_vel < 0)
                            y_vel *= -1;

                        else
                        {
                            x_vel *= -2;
                        }
                        //player1.direction.Y = -1.0f * player1.direction.Y;
                    }

                    
                    

                    else if ((position.X <
                    (p.position.X + p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                    {
                        position.X -= 1;
                        //x_vel -= speed;
                        //x_vel /= -1;
                        //grounded = false;
                        //player1.direction.X = -1.0f * player1.direction.X;
                        y_vel = 0;
                        position.Y -= 1;
                       
                    }

                    else if (BoundingBox.Intersects(p.BoundingBox) && (position.X >
                        (p.position.X - p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                    {
                        position.X += 1;
                        //x_vel += speed;
                        //x_vel /= -1;
                        //    //grounded = false;
                        //    //player1.direction.X = -1.0f * player1.direction.X;
                        y_vel = 0;
                        position.Y -= 1;
                    }
          

                }
            }
        }

        public void checkXCollisions(block[] platforms)
        {

            float Xradius = Width / 2;
            float Yradius = Height / 2;
            block collidedPlatform = null;

            foreach (block p in platforms)
            {
                if ((position.X > (p.position.X - p.Width / 2 - Xradius)) &&
                    (position.X < (p.position.X + p.Width / 2 + Xradius)) &&
                    (position.Y > (p.position.Y - p.Height / 2 - Yradius)) &&
                    (position.Y < (p.position.Y + p.Height / 2 + Yradius)))
                {
                    collidedPlatform = p;
                    break;
                }
            }

            if ((position.X >
                (collidedPlatform.position.X + collidedPlatform.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
            {
                //x_vel -= speed;
                //x_vel /= -1;
                //grounded = false;
                //player1.direction.X = -1.0f * player1.direction.X;
                position.X -= 1;
                position.Y += 1;
            }

            else if ((position.X >
                (collidedPlatform.position.X - collidedPlatform.Width / 2 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
            {
                //x_vel += speed;
                //x_vel /= -1;
                //    //grounded = false;
                //    //player1.direction.X = -1.0f * player1.direction.X;
                position.X += 1;
                position.Y += 1;
            }


        }




    }
}
