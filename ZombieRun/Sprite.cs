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
    public class Sprite
    {
        public string textureName = "";
        protected Texture2D texture;
        protected Game game;
        public Vector2 position = Vector2.Zero;
        public Vector2 tright; //x,y
        public Vector2 tleft;//-x,y
        public Vector2 bright;//x,-y
        public Vector2 bleft;//-x,-y
        public bool moving;
        public SpriteEffects flip;
        public bool isRight;
        

        public Sprite(Game myGame)
        {
            game = myGame;
            this.flip = SpriteEffects.None;
            isRight = true;
        }

        public SpriteEffects GetFlip()
        {
            return this.flip;
        }
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }

        }
        public float Width
        {
            get { return texture.Width; }
        }

        public float Height
        {
            get { return texture.Height; }
        }


        public virtual void LoadContent()
        {
            if (textureName != "")
            {
                texture = game.Content.Load<Texture2D>(textureName+".png");
            }

        }

        

        public virtual void Draw(SpriteBatch batch, SpriteEffects flip)
        {
            if (texture != null)
            {
                Vector2 drawPosition = position;
                drawPosition.X -= texture.Width / 2;
                drawPosition.Y -= texture.Height / 2;
                batch.Draw(texture, drawPosition, null, Color.White, 0, new Vector2 (0,0), 1, this.flip, 0);
            }

        }
    }
}
