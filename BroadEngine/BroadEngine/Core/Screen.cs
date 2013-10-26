using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BroadEngine.GameObjects.Sprites;

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

        public static SpriteFont Font;
        public static Texture2D Pixel { get; private set; }

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

        public static Vector2 Center { get { return new Vector2(Width / 2, Height / 2); } }

        #endregion

        #region Public Methods

        public static bool ToggleFullScreen()
        {
            Graphics.ToggleFullScreen();
            return IsFullScreen;
        }

        public static void ClearScreen() { GraphicsDevice.Clear(ScreenClearColor);  }

        public static void DrawString(string text, Vector2 position, Color color) { _spriteBatch.DrawString(Font, text, position, color); }
        public static void Draw(Animation animation, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float drawLayer) { _spriteBatch.Draw(animation.SourceTexture, position, animation.CurrentSpriteRect, color, rotation, origin, scale, effects, drawLayer); }
        public static void Draw(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float drawLayer) { _spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, effects, drawLayer); }
        public static void Draw(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, float scale) { Draw(texture, position, color, rotation, origin, scale, SpriteEffects.None, 0); }
        public static void DrawRect(Rectangle rect, Color color) { _spriteBatch.Draw(Pixel, rect, color); }

        #endregion

        #region Internal Methods

        internal static void Load()
        {
            Width = 800;
            Height = 640;
            IsMouseVisible = true;
            DefaultScreenClearColor = Color.Black;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData<Color>(new Color[] { Color.White });
        }

        internal static void BeginDraw() { _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend); }
        internal static void EndDraw() { _spriteBatch.End(); }

        #endregion
    }
}
