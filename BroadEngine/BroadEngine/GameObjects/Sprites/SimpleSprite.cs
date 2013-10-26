using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BroadEngine.Core;

namespace BroadEngine.GameObjects.Sprites
{
    public class SimpleSprite : GameObject, IDrawable
    {
        protected float _texWidth;
        protected float _texHeight;

        #region Properties

        public Vector2 Position;
        public Vector2 Origin;
        public float Rotation;
        public float Scale;
        public Color Color;
        public float Alpha;
        public Texture2D Texture { get; protected set; }
        public Vector2 ScaledOrigin { get { return Origin * Scale; } }
        public float Width { get { return _texWidth * Scale; } }
        public float Height { get { return _texHeight * Scale; } }
        public Vector2[] Corners
        {
            get
            {
                return new Vector2[] {
                    Position - ScaledOrigin,
                    Position + new Vector2(Width - ScaledOrigin.X, -ScaledOrigin.Y),
                    Position + new Vector2(-ScaledOrigin.X, Height - ScaledOrigin.Y),
                    Position + ScaledOrigin
                };
            }
        }
        public Vector2[] RotatedCorners
        {
            get
            {
                Vector2[] corners = Corners;
                Matrix rotation = Matrix.CreateRotationZ(Rotation);
                for (int i = 0; i < 4; i++)
                    corners[i] = Vector2.Transform(corners[i] - Position, rotation) + Position;
                return corners;
            }
        }
        public Rectangle Bounds
        {
            get
            {
                Vector2[] rotatedCorners = RotatedCorners;
                float top, right, bot, left;
                bot = top = rotatedCorners[0].Y;
                left = right = rotatedCorners[0].X;
                for (int i = 1; i < 4; i++)
                {
                    if (rotatedCorners[i].X < left)
                        left = rotatedCorners[i].X;
                    if (rotatedCorners[i].X > right)
                        right = rotatedCorners[i].X;
                    if (rotatedCorners[i].Y < top)
                        top = rotatedCorners[i].Y;
                    if (rotatedCorners[i].Y > bot)
                        bot = rotatedCorners[i].Y;
                }

                int width = (int)Math.Ceiling((right - left));
                int height = (int)Math.Ceiling((bot - top));

                return new Rectangle((int)left, (int)top, width, height);
            }
        }

        #endregion

        #region Constructors

        public SimpleSprite()
        {
            _texWidth = 0;
            _texHeight = 0;
            Origin = Vector2.Zero;
            Position = Vector2.Zero;
            Rotation = 0;
            Scale = 1;
            Alpha = 1;
        }

        public SimpleSprite(Texture2D texture) : this()
        {
            Texture = texture;
            Origin = Texture.Center();
            _texHeight = texture.Height;
            _texWidth = texture.Width;
        }
        public SimpleSprite(string textureName) : this(ContentLoader.Get<Texture2D>(textureName)) { }

        #endregion

        #region Public Methods

        public virtual void Draw(GameTime gameTime, bool isPaused) { Screen.Draw(Texture, Position, Color * Alpha, Rotation, Origin, Scale); }

        #endregion
    }
}
