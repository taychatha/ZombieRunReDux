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
    public class block : Sprite
    {
        
        public int movedX;
        private Vector2 direction;
        public bool visible;
        

        public block(Game myGame) :
            base(myGame)
        {
            visible = true;
            textureName = "brick 3";
            
        }
        public void setTexture(string newText)
        {
            textureName = newText;
        }

        public float Width
        {
            get { return texture.Width; }
        }
        public float Height
        {
            get { return texture.Height; }
        }


        public void Update() {
            position.X -= 0.5F;
        }

        private void checkYCollisions()
        {

        }

        public bool checkVisibility()
        {
            if (position.X < 0) {
                visible = false;
            }
            else
            {
                visible = true;
            }
            return visible;
        }

        


    }
}
