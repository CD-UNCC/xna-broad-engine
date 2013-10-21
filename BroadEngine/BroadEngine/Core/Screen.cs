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
        static SpriteBatch _spriteBatch;

        #endregion

        #region Properties

        public static GraphicsDeviceManager Graphics { get; internal set; }

        public static GraphicsDevice GraphicsDevice { get { return Graphics.GraphicsDevice; } }

        public static Color ScreenClearColor;

        public static Color DefaultScreenClearColor
        {
            get { return _defaultScreenClearColor; }
            set
            {
                ScreenClearColor = value;
                _defaultScreenClearColor = value;
            }
        }

        public static int Width
        {
            get { return Graphics.PreferredBackBufferWidth; }
            set { Graphics.PreferredBackBufferWidth = value; Graphics.ApplyChanges(); }
        }

        public static int Height
        {
            get { return Graphics.PreferredBackBufferHeight; }
            set { Graphics.PreferredBackBufferHeight = value; Graphics.ApplyChanges(); }
        }

        public static bool IsMouseVisible
        {
            get { return Game.CurrentGame.IsMouseVisible; }
            set { Game.CurrentGame.IsMouseVisible = value; }
        }

        public static bool IsFullScreen { get { return Graphics.IsFullScreen; } }

        public static bool IsActiveWindow { get { return Game.CurrentGame.IsActive; } }

        public static GameWindow Window { get { return Game.CurrentGame.Window; } }


        #endregion

        #region Public Methods

        public static bool ToggleFullScreen()
        {
            Graphics.ToggleFullScreen();
            return IsFullScreen;
        }

        public static void ClearScreen() { GraphicsDevice.Clear(ScreenClearColor);  }

        #endregion

        #region Internal Methods

        internal static void Load()
        {
            Width = 800;
            Height = 800;
            IsMouseVisible = true;
            DefaultScreenClearColor = Color.Black;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        internal static void BeginDraw() { _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise); }
        internal static void EndDraw() { _spriteBatch.End(); }

        #endregion
    }
}
