using BroadEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.GameObjects.Sprites
{
    public class Animation
    {
        int _numSprites;
        int _numCols;

        int _curSprite;
        int _curFrame;

        public int SpriteHeight { get; protected set; }
        public int SpriteWidth { get; protected set; }

        int _framesPerSprite;
        public int FramesPerSprite { get { return _framesPerSprite; }
            set { _framesPerSprite = value; _framesPerSprite = Helper.ClampValueMin(_framesPerSprite, 1); } }
        public Texture2D SourceTexture { get; protected set; }
        public Rectangle CurrentSpriteRect { get { return new Rectangle(SpriteWidth * (_curSprite % _numCols), SpriteHeight * (_curSprite / _numCols), SpriteWidth, SpriteHeight); } }

        public Animation(Texture2D texture, int numSprites, int spriteWidth, int spriteHeight, int numCols, int framesPerSprite)
        {
            SourceTexture = texture;
            _numSprites = numSprites;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
            _numCols = numCols;
            _framesPerSprite = framesPerSprite;

            _curSprite = 0;
            _curFrame = 0;
        }

        public void Update()
        {
            _curFrame++;
            if (_curFrame >= _framesPerSprite)
            {
                _curFrame = 0;
                _curSprite++;
                if (_curSprite == _numSprites)
                    _curSprite = 0;
            }
        }

        public Animation Copy() { return new Animation(SourceTexture, _numSprites, SpriteWidth, SpriteHeight, _numCols, _framesPerSprite); }
    }
}
