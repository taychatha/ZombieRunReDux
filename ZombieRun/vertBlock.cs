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
    public class vertBlock : block
    {

        public int movedX;
        private Vector2 direction;


        public vertBlock(Game myGame) :
            base(myGame)
        {
            textureName = "longBlock";

        }

        public float Width
        {
            get { return texture.Width; }
        }
        public float Height
        {
            get { return texture.Height; }
        }


        public void Update() { }
        private void checkYCollisions()
        {

        }




    }
}
