using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BroadEngine.Core
{
    public static class Screen
    {
        #region Fields

        static Color _defaultScreenClearColor;
        static GraphicsDevice _graphicsDevice;
        static SpriteBatch _spriteBatch;

        #endregion

        #region Properties

        public static GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
            internal set
            {
                _graphicsDevice = value;
                _spriteBatch = new SpriteBatch(value);
            }
        }

        public static Color CurrentScreenClearColor;

        public static Color DefaultScreenClearColor
        {
            get { return _defaultScreenClearColor; }
            set
            {
                CurrentScreenClearColor = value;
                _defaultScreenClearColor = value;
            }
        }

        #endregion

        #region Public Methods

        public static void ClearScreen() { _graphicsDevice.Clear(CurrentScreenClearColor);  }

        #endregion

        #region Internal Methods

        internal static void BeginDraw() { _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise); }
        internal static void EndDraw() { _spriteBatch.End(); }

        #endregion
    }
}
