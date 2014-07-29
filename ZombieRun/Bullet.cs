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
    class Bullet: Sprite
    {
        const int MAX_Distance = 500;
        public bool Visible = false;

        public Vector2 startPosition;
        public Vector2 bulletTarget;
        private Vector2 bulletVelcoity;
        private float speed;
        private Vector2 direction;
        public Rectangle bulletBox;

        

        public Bullet(Game myGame):
            base(myGame)
        {
            textureName = "bullet";


            
        }

       public void bulletShot(Vector2 center){

           position = center;
           startPosition = center;
           speed = 200;
           Visible = true;
       }

        
        public override void Draw(SpriteBatch batch)
        {
            if(Visible == true)
                base.Draw(batch);
        }
        public void Update(GameTime gameTime, Vector2 theDirection){

            if (Vector2.Distance(startPosition, position) > MAX_Distance)
            {
                Visible = false;
            }

            else
            {
                if (Visible)
                {
                    position += theDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            }
        }

        public void Hit(){
            Visible = false;
        }
    }


}
