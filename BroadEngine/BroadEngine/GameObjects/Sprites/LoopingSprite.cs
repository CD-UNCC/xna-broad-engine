using BroadEngine.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.GameObjects.Sprites
{
    public class LoopingSprite : SimpleSprite, IUpdateable
    {
        protected Animation _animation;

        public LoopingSprite(string textureName, int numSprites, int spriteWidth, int spriteHeight, int numCols, int framesPerSprite) : base()
        {
            _animation = new Animation(textureName, numSprites, spriteWidth, spriteHeight, numCols, framesPerSprite);
            Origin = new Vector2(spriteWidth / 2, spriteHeight / 2);
            _texHeight = spriteHeight;
            _texWidth = spriteWidth;
        }

        public void Update(GameTime gameTime, bool isPaused)
        {
            if (!isPaused)
                _animation.Update();
        }

        public override void Draw(GameTime gameTime, bool isPaused) { Screen.Draw(_animation, Position, Color, Rotation, Origin, Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0); }
    }
}
