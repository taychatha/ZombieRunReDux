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
        public double gravity = 0.4;
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

        public void Update(Controls controls, GameTime gameTime, List<block> platforms, List<block> platforms2)
        {
            position.X -= 0.5f;  
            Move(controls, platforms, platforms2);
            Jump(controls, gameTime);
            

            
        }


        
        public void Move(Controls controls, List<block> platforms, List<block> platforms2)
        {
            if (controls.onPress(Keys.Right, Buttons.DPadRight)){
                x_accel += speed;
                this.textureName = "prep2";
            }
                
            else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
                x_accel -= speed;



            if (controls.onPress(Keys.Left, Buttons.DPadLeft))
            {
                x_accel -= speed;
                this.textureName = "prep2-left";
            }
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

            //grounded = false;


            
            checkYCollisions(platforms, platforms2);
            //checkYCollisions(platforms2);
            //checkPlatformCollisions(platform);
        }

        private void Jump(Controls controls, GameTime gameTime)
        {

            if (controls.onPress(Keys.Up, Buttons.A) && grounded)
            {
                y_vel = -15;
                jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
                grounded = false;
            }

            else if (controls.onRelease(Keys.Up, Buttons.A) && y_vel < 0)
            {
                y_vel /= 2;
            }
        }

        
        public void checkYCollisions(List<block> platforms, List<block> platforms2)
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

                if ((position.X > (p.position.X - p.Width / 2 - Xradius )) &&
                    (position.X < (p.position.X + p.Width / 2 + Xradius )) &&
                   (position.Y > (p.position.Y - p.Height / 2 - Yradius  )) &&//on top
                    ((position.Y < (p.position.Y + p.Height / 2 + Yradius)))
                    )//below

                {
                    collidedBlocks.Add(p);
                    
                }
            }

            foreach (block p in platforms2)
            {
                if ((position.X > (p.position.X - p.Width / 2 - Xradius - 5)) &&
                    (position.X < (p.position.X + p.Width / 2 + Xradius + 5)) &&
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

                if (p.textureName == "brick 3")
                {
                    if ((BoundingBox.Bottom <
                        (p.BoundingBox.Top + 4/*+ radius*/)))
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


                    else if ( (position.X <
                    (p.position.X + p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                     {
                         position.X -= 3;
                         //x_vel -= speed;
                         //x_vel /= -1;
                         //grounded = false;
                         //player1.direction.X = -1.0f * player1.direction.X;
                     }

                     else if (BoundingBox.Intersects(p.BoundingBox) && (position.X >
                         (p.position.X - p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                     {
                         position.X += 3;
                         //x_vel += speed;
                         //x_vel /= -1;
                         //    //grounded = false;
                         //    //player1.direction.X = -1.0f * player1.direction.X;
                     }
                  




                }
                else
                {
                    if ((BoundingBox.Bottom <
                        (p.BoundingBox.Top - 40/*40 is the number we adjust to make it look better. but be careful because if the number is higher, then our guy
                                                falls through the block. it's annoying...*/)))
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
                        position.X -= 3;
                        //x_vel -= speed;
                        //x_vel /= -1;
                        //grounded = false;
                        //player1.direction.X = -1.0f * player1.direction.X;
                    }

                    else if (BoundingBox.Intersects(p.BoundingBox) && (position.X >
                        (p.position.X - p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                    {
                        position.X += 3;
                        //x_vel += speed;
                        //x_vel /= -1;
                        //    //grounded = false;
                        //    //player1.direction.X = -1.0f * player1.direction.X;
                    }

                }
            } 
        }

        public bool checkZombieCollisions(List<Zombie> zombies)
        {
            bool dead = false;
            foreach (Zombie z in zombies)
            {
                if (!z.isAlive)
                {
                    break;
                }
                else if (BoundingBox.Intersects(z.BoundingBox))
                {
                    dead = true;
                }
            }
            return dead;
        }

        
    }
}
