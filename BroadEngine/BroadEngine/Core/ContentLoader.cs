using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BroadEngine.GameObjects.Sprites;

namespace BroadEngine.Core
{
    public static class ContentLoader
    {
        #region Fields

        #region TextLoading Variables

        static char ContentSeparator = ' ';
        static char CommentDelimiter = '#';

        static string TextureFolder = "";
        static string AnimationFolder = "";
        static string FontFolder = "";

        static string DefaultFontName = "";

        #endregion

        static Dictionary<string, object> _contentTable;
        static ContentManager _contentManager;

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public static T Get<T>(string contentName) { return (T)_contentTable[contentName]; }

        #endregion

        #region Internal Methods

        internal static void Load(ContentManager contentManager, string contentFileName)
        {
            _contentManager = contentManager;
            _contentTable = new Dictionary<string, object>();

            using (StreamReader reader = new StreamReader(_contentManager.RootDirectory + "/" + contentFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line.Trim();
                    if (line != String.Empty && line[0] != CommentDelimiter)
                    {
                        line = line.Replace('\t', ContentSeparator);
                        string[] info = line.Split(new char[] { ContentSeparator }, StringSplitOptions.RemoveEmptyEntries);

                        switch (info[0])
                        {
                            case "Texture":
                                _contentTable.Add(info[1], _contentManager.Load<Texture2D>(TextureFolder + info[2]));
                                break;
                            case "Font":
                                if (DefaultFontName == "")
                                    DefaultFontName = info[1];
                                _contentTable.Add(info[1], _contentManager.Load<SpriteFont>(FontFolder + info[2]));
                                break;
                            case "Animation":
                                _contentTable.Add(info[1], new Animation(_contentManager.Load<Texture2D>(AnimationFolder + info[2]), Int32.Parse(info[3]), Int32.Parse(info[4]), Int32.Parse(info[5]), Int32.Parse(info[6]), Int32.Parse(info[7])));
                                break;
                            case "Set":
                                switch (info[1])
                                {
                                    case "Folder":
                                        switch (info[2])
                                        {
                                            case "Texture":
                                                TextureFolder = info[3] + "/";
                                                break;
                                            case "Animation":
                                                AnimationFolder = info[3] + "/";
                                                break;
                                            case "Font":
                                                FontFolder = info[3] + "/";
                                                break;
                                        }
                                        break;
                                    case "Default":
                                        switch (info[2])
                                        {
                                            case "Font":
                                                DefaultFontName = info[3];
                                            break;
                                        }
                                        break;
                                }
                                break;
                        }
                    }                
                }
            }

            Screen.Font = Get<SpriteFont>(DefaultFontName);            
        }

        #endregion
    }
}
