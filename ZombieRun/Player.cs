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
    public class Player: Sprite
    {
        public int speed;
        private bool moving;
        public bool grounded;
        private int x_accel;
        private double friction;
        public double x_vel;
        public double y_vel;
        public int movedX;
        private Vector2 direction;
        private bool pushing;
        public double gravity = .5;
        public int maxFallSpeed = 10;
        private int jumpPoint = 0;

        public Player(Game myGame):
            base(myGame)
        {
            textureName = "prep2";
            speed = 5;
            friction = .15;
            x_accel = 0;
            y_vel = 0;
            x_vel = 0;
            movedX = 0;
        }

        public float Width
        {
            get { return texture.Width; }
        }
        public float Height
        {
            get { return texture.Height; }
        }

        public void Update(Controls controls, GameTime gameTime, block[] platforms)
        {

            Move(controls, platforms);
            Jump(controls, gameTime);
            

            
        }


        
        public void Move(Controls controls, block[] platforms)
        {
            if (controls.onPress(Keys.Right, Buttons.DPadRight)){
                x_accel += speed;
            }
                
            else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
                x_accel -= speed;



            if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                x_accel -= speed;
            else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
                x_accel += speed;

            double playerFriction = pushing ? (friction * 3) : friction;
            x_vel = x_vel * (1 - playerFriction) + x_accel * .10;
            movedX = Convert.ToInt32(x_vel);
            position.X += movedX;

            position.X = MathHelper.Clamp(position.X, 10 + texture.Width / 2, 1020 - texture.Width / 2);
            if(!grounded)
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

            grounded = false;


            
            checkYCollisions(platforms);
            //checkPlatformCollisions(platform);
        }

        private void Jump(Controls controls, GameTime gameTime)
        {

            if (controls.onPress(Keys.Space, Buttons.A) && grounded)
            {
                y_vel = -15;
                jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
                grounded = false;
            }

            else if (controls.onRelease(Keys.Space, Buttons.A) && y_vel < 0)
            {
                y_vel /= 2;
            }
        }

        public void checkYCollisions(block[] platforms)
        {

           
            if (position.Y >= 700)
                grounded = true;
            else
                grounded = false;

            float Xradius = Width / 2;
            float Yradius = Height / 2;
            block collidedPlatform = null;

            foreach (block p in platforms)
            {
                if ((position.X > (p.position.X - p.Width / 2 - Xradius + 10)) &&
                    (position.X < (p.position.X + p.Width / 2 + Xradius - 10)) &&
                   (position.Y > (p.position.Y - p.Height / 2 - Yradius )) &&
                    (position.Y < (p.position.Y + p.Height / 2 + Yradius)))

                {
                    collidedPlatform = p;
                    break;
                }
            }
            //collisions work for all side of blocks. 
            if (collidedPlatform != null)
            {
                if ((position.Y <
                    (collidedPlatform.position.Y - collidedPlatform.Height / 1.5 /*+ radius*/)))
                {

                    grounded = true;

                }

                else if ((position.Y   >
                    (collidedPlatform.position.Y + collidedPlatform.Height / 2 )))
                {
                    if(y_vel < 0)
                        y_vel *= -1;
                    //player1.direction.Y = -1.0f * player1.direction.Y;
                }

                 else if ((position.X <
                (collidedPlatform.position.X + collidedPlatform.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                 {
                     x_vel *= -2;
                     //x_vel -= speed;
                     //x_vel /= -1;
                     //grounded = false;
                     //player1.direction.X = -1.0f * player1.direction.X;
                 }

                else if ((position.X >
                    (collidedPlatform.position.X - collidedPlatform.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                {
                    x_vel *= -2;
                    //x_vel += speed;
                    //x_vel /= -1;
                    //    //grounded = false;
                    //    //player1.direction.X = -1.0f * player1.direction.X;
                }
                else
                {
                    if (grounded)
                    {
                        position.Y -= 10;
                    }
                }
                ///summary
                ///this one works for just one of them, but not the other side. This is interesting.
                ///end summary
                

               // else if ((position.X <
               //(collidedPlatform.position.X + collidedPlatform.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
               // {
               //     position.X += 3;
               //     //x_vel -= speed;
               //     //x_vel /= -1;
               //     //grounded = false;
               //     //player1.direction.X = -1.0f * player1.direction.X;
               // }

               // else if ((position.X >
               //     (collidedPlatform.position.X - collidedPlatform.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
               // {
               //     position.X -= 3;
               //     //x_vel += speed;
               //     //x_vel /= -1;
               //     //    //grounded = false;
               //     //    //player1.direction.X = -1.0f * player1.direction.X;
               // }
               

                

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
                x_vel -= speed;
                //x_vel /= -1;
                //grounded = false;
                //player1.direction.X = -1.0f * player1.direction.X;
            }

            else if ((position.X >
                (collidedPlatform.position.X - collidedPlatform.Width / 2 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
            {
                x_vel += speed;
                //x_vel /= -1;
            //    //grounded = false;
            //    //player1.direction.X = -1.0f * player1.direction.X;
            }


        }

        

        //protected void CheckCollisions(Platform[] platforms)
        //{
        //    float radius = Width / 2;
        //    Platform collidedPlatform = null;
        //    foreach (Platform p in platforms)
        //    {
        //        if ((position.X > (p.position.X - p.Width / 2 - radius)) &&
        //            (position.X < (p.position.X + p.Width / 2 + radius)) &&
        //            (position.Y > (p.position.Y - p.Height / 2 - radius)) &&
        //            (position.Y < (p.position.Y + p.Height / 2 + radius)))
        //        {
        //            collidedPlatform = p;
        //            break;
        //        }
        //    }

        //    if (collidedPlatform != null)
        //    {
        //        if ((position.Y <
        //            (collidedPlatform.position.Y - collidedPlatform.Height / 2)))
        //        {

        //            checkYCollisions(collidedPlatform);

        //        }

        //        else if ((position.Y >
        //            (collidedPlatform.position.Y + collidedPlatform.Height / 2)))
        //        {
        //            y_vel /= -1;
        //            //player1.direction.Y = -1.0f * player1.direction.Y;
        //        }

        //        else // otherwise, we have to be colliding from the sides
        //        {
        //            x_vel = 0;
        //            //player1.direction.X = -1.0f * player1.direction.X;
        //        }

        //    }
        //}

        
    }
}
