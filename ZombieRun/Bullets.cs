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
    class Bullet : Sprite
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

        public override void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("bullet.png");
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
        }


    }
}
