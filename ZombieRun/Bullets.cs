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
    public class Bullet : Sprite
    {
        public Vector2 startPosition;
        public Vector2 velocity;
        public Vector2 origin;
        public bool isVisible;
        public int speed;
        private int x_accel;
        public double x_vel;
        public int movedX;
        private Vector2 direction;

        public Bullet(Game myGame) :
            base(myGame)
        {

            isVisible = false;
            speed = 200;
            x_vel = 0;


        }

        public void Update(){
            
            position.X += 3;
        }

        public override void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("bullet.png");
        }

        public void checkForBoxCollision(List<block> blocks)
        {
            
            foreach (block b in blocks)
            {
                if ((b.textureName == "brick 3") &&((BoundingBox.Bottom > b.BoundingBox.Top-20)&&
                    (BoundingBox.Top < b.BoundingBox.Bottom-20)&&
                    (BoundingBox.Left < b.BoundingBox.Right -50)&&
                    (BoundingBox.Right > b.BoundingBox.Left - 50)))
                {
                    isVisible = false;
                }

                else if ((b.textureName == "longBrick3") && ((BoundingBox.Bottom > b.BoundingBox.Top - 40) &&
                    (BoundingBox.Top < b.BoundingBox.Bottom - 50) &&
                    (BoundingBox.Left < b.BoundingBox.Right -50) &&
                    (BoundingBox.Right > b.BoundingBox.Left- 50)))
                {
                    isVisible = false;
                }
            }
        }

    }
}
