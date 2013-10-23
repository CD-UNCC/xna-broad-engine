using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroadEngine.Core
{
    public static class ContentLoader
    {
        #region Fields

        static Dictionary<string, object> _contentTable;
        static ContentManager _contentManager;

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public static T Get<T>(string contentName) { return (T)_contentTable[contentName]; }

        #endregion

        #region Internal Methods

        internal static void Load(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _contentTable = new Dictionary<string, object>();

            Screen.Font = _contentManager.Load<SpriteFont>("Fonts/TestFont");
            _contentTable.Add("TestFont", Screen.Font);
        }

        #endregion
    }
}
