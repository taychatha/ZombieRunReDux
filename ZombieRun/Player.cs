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
        private int frame = 0;
        public bool isRight;
        private int picNum;
        private Vector2 prevPos;
        private Texture2D tempText;

        public Player(Game myGame):
            base(myGame)
        {

            textureName = "prep2";
            speed = 7;
            friction = .15;
            x_accel = 0;
            y_vel = 0;
            x_vel = 0;
            movedX = 0;
            isRight = true;
            picNum = 1;
        }
        public override void LoadContent()
        {
            for (int i = 1; i < 7; i++)
            {
                tempText = game.Content.Load<Texture2D>("hero" + i + ".png");
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

        public void Update(Controls controls, GameTime gameTime, List<block> platforms, List<block> platforms2)
        {
            position.X -= 0.5f;  
            Move(controls, platforms, platforms2);
            Jump(controls, gameTime);
             //For sprite sheet animation
            if (moving)
            {
                if ((prevPos.X - 0.2f) == position.X)
                {
                    //frame++;
                    moving = false;
                    picNum = 1;
                }
                else
                {
                    if (grounded == true)
                    {
                        moving = true;
                        frame++;
                    }
                }
                if (frame % 5 == 0)
                {
                    texture = animation[picNum];
                    picNum++;
                    //texture = animation[picNum];
                    if (picNum == 6)
                    {
                        picNum = 1;
                    }

                    //texture = animation[picNum];
                    //Console.WriteLine(x_vel);
                    prevPos = position;
                }
            }

            
        }


        
        public void Move(Controls controls, List<block> platforms, List<block> platforms2)
        {
            if (controls.onPress(Keys.Right, Buttons.DPadRight)){
                x_accel += speed;
                isRight = true;
                this.flip = SpriteEffects.None;
                moving = true;
            }

            else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
            {
                x_accel -= speed;
                moving = false;

            }



            if (controls.onPress(Keys.Left, Buttons.DPadLeft))
            {
                x_accel -= speed;
                isRight = false;
                this.textureName = "prep2-left";
                this.flip = SpriteEffects.FlipHorizontally;
                moving = true;
            }
            else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
            {
                x_accel += speed;
                moving = false;
            }

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
                         position.X -= 8;
                         //x_vel -= speed;
                         //x_vel /= -1;
                         //grounded = false;
                         //player1.direction.X = -1.0f * player1.direction.X;
                     }

                     else if ((position.X >
                         (p.position.X - p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                     {
                         position.X += 8;
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
                        position.X -= 8;
                        //x_vel -= speed;
                        //x_vel /= -1;
                        //grounded = false;
                        //player1.direction.X = -1.0f * player1.direction.X;
                    }

                    else if ((position.X >
                        (p.position.X - p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                    {
                        position.X += 8;
                        //x_vel += speed;
                        //x_vel /= -1;
                        //    //grounded = false;
                        //    //player1.direction.X = -1.0f * player1.direction.X;
                    }

                }
            } 
        }

        public bool checkZombieCollisions(List<Zombie> zombies, BMFZ bmfz)
        {
            bool dead = false;
            foreach (Zombie z in zombies)
            {
                if (!z.isAlive)
                {
                    break;
                }
                else if (BoundingBox.Intersects(z.CollisionBox) || BoundingBox.Intersects(bmfz.HitBox))
                {
                    dead = true;
                }
            }

           
            return dead;
        }

        
    }
}
