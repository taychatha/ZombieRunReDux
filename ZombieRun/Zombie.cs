﻿using System;
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
        Random rnd = new Random();

        public Zombie(Game myGame) :
            base(myGame)
        {
            textureName = "Creeper1";
            speed = 1.0f;
            currSpeed = speed;
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

        public void Update(GameTime gameTime, List<block> platforms)
        {

            Move(platforms);
            //Jump(gameTime);



        }


        public void getPlayerPosition(Player player1)
        {
            destination = player1.position.X;
        }



        public void Move(List<block> platforms)
        {

            if (destination == this.position.X) {
                //x_accel = 0;
            }
            if (destination > this.position.X)
            {
                
                position.X += currSpeed;
            }

            //else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
              //  x_accel -= speed;



            if (destination < this.position.X) { 
               //x_accel -= speed;
                //else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
                //  x_accel += speed;
                position.X -= currSpeed;
        }
            //double playerFriction = pushing ? (friction * 3) : friction;
            //x_vel = x_vel * (1 - playerFriction) + x_accel * .10;
            //movedX = Convert.ToInt32(x_accel);
            //position.X += movedX;

            //position.X = MathHelper.Clamp(position.X, 10 + texture.Width / 2, 1020 - texture.Width / 2);
            position.X = MathHelper.Clamp(position.X, -50 + texture.Width / 2, 2000 - texture.Width / 2);

            if (!grounded)
            {
                y_vel += gravity;
                if (y_vel > maxFallSpeed)
                    y_vel = maxFallSpeed;
                position.Y += Convert.ToInt32(y_vel);
            }

            else
            {
                //y_vel = 1;
                position.Y = position.Y;
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
                        myscore = 100;
                        isAlive = false;
                        b.isVisible = false;
                        bullets.Remove(b);
                        break;
                    }

                }
        
        }

        public int getScore()
        {
            return myscore;
        }

        public void setScore(int score)
        {
            myscore = score;
        }


        public void checkYCollisions(List<block> platforms)
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
                //else
                //{
                //    currSpeed = speed;
                //}
            }


            //collisions work for all side of blocks. 
            foreach (block p in collidedBlocks)
            {
                if (p.textureName == "brick 3")
                {
                    if ((BoundingBox.Bottom <
                        (p.BoundingBox.Top + 40/*+ radius*/)))
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
                        position.X -= 2;
                        //x_vel -= speed;
                        //x_vel /= -1;
                        //grounded = false;
                        //player1.direction.X = -1.0f * player1.direction.X;
                    }

                    else if (BoundingBox.Intersects(p.BoundingBox) && (position.X >
                        (p.position.X - p.Width / 1.5 /*+ Xradius*/))) // otherwise, we have to be colliding from the sides
                    {
                        position.X += 2;
                        //x_vel += speed;
                        //x_vel /= -1;
                        //    //grounded = false;
                        //    //player1.direction.X = -1.0f * player1.direction.X;
                    }

                }
            }
        }


        public static float NextFloat(Random random)
        {
            float tempSpeed = 1.0f;
            int caseSwitch = random.Next(1, 6);
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
            return tempSpeed;
        }



    }
}



/**
class Zombie

    {

        public Rectangle Location { get; set; }

        public double XVelocity { get; set; }

        public double YVelocity { get; set; }

        public double Speed { get; set; }

        public bool Grounded { get; set; }

        public Texture2D Image { get; set; }




        public Zombie(Point startPosition, Texture2D image, double speed)

        {

            this.Image = image;

            this.Location = new Rectangle(startPosition.X, startPosition.Y, Image.Width, Image.Height);

            this.Speed = speed;

        }




        void Update(List<Block> blocks)

        {

            CheckForBlockIntersection(blocks);

            UpdateVelocity();

            UpdatePosition();

        }




        void Draw(SpriteBatch sb)

        {

            sb.Begin();

            sb.Draw(Image, Location);

            sb.End();

        }




        private bool CheckForBlockIntersections(List<Block> blocks)

        {            

            for(Block block in blocks){

                if(this.Location.Intersects(block.Location)){

                    Grounded = true;

                    return;

                }

            }

            Grounded = false;

        }




        private void UpdateVelocity()

        {

            YVelocity = Grounded ? 0 : Speed;

        }




        private void UpdatePosition()

        {

            double XPosition = Location.X + XVelocity;

            double YPosition = Location.Y + YVelocity;

            Location = new Rectangle(XPosition, YPosition, Image.Width, Image.Height);

        }

    }
*/