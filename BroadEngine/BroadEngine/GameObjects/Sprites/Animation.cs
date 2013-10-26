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
        int _spriteWidth;
        int _spriteHeight;
        int _numCols;
        int _framesPerSprite;

        int _curSprite;
        int _curFrame;

        public Texture2D SourceTexture { get; protected set; }
        public Rectangle CurrentSpriteRect { get { return new Rectangle(_spriteWidth * (_curSprite % _numCols), _spriteHeight * (_curSprite / _numCols), _spriteWidth, _spriteHeight); } }

        public Animation(string textureName, int numSprites, int spriteWidth, int spriteHeight, int numCols, int framesPerSprite)
        {
            SourceTexture = ContentLoader.Get<Texture2D>(textureName);
            _numSprites = numSprites;
            _spriteWidth = spriteWidth;
            _spriteHeight = spriteHeight;
            _numCols = numCols;
            _framesPerSprite = framesPerSprite;

            _curSprite = 0;
            _curFrame = 0;
        }

        public void Update()
        {
            _curFrame++;
            if (_curFrame == _framesPerSprite)
            {
                _curFrame = 0;
                _curSprite++;
                if (_curSprite == _numSprites)
                    _curSprite = 0;
            }
        }
    }
}
