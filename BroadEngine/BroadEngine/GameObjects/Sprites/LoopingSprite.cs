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
        public Animation Animation { get; protected set; }

        public LoopingSprite(string animationName) : base()
        {
            Animation = ContentLoader.Get<Animation>(animationName).Copy();
            Origin = new Vector2(Animation.SpriteWidth / 2, Animation.SpriteHeight / 2);
            _texHeight = Animation.SpriteHeight;
            _texWidth = Animation.SpriteWidth;
        }

        public void Update(GameTime gameTime, bool isPaused)
        {
            if (!isPaused)
                Animation.Update();
        }

        public override void Draw(GameTime gameTime, bool isPaused) { Screen.Draw(Animation, Position, Color, Rotation, Origin, Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0); }
    }
}
