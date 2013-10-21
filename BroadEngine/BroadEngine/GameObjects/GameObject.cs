using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BroadEngine.GameObjects
{
    public abstract class GameObject
    {
        protected List<GameObject> _children = new List<GameObject>();

        #region Properties

        public GameObject Parent { get; protected set; }

        #endregion

        #region Public Methods

        public IEnumerable<GameObject> GetChildrenOfType<T>() where T : GameObject { return _children.OfType<T>(); }

        #endregion

        #region Protected Methods



        #endregion
    }

    public interface IAttaches
    {
        void OnAttach(IAttachable parent);
    }

    public interface IAttachable
    {
        void Attach(IAttaches child);
    }

    public interface IUpdateable
    {
        void Update(GameTime gameTime);
    }

    public interface IDrawable
    {
        Vector2 Position { get; set; }
        Rectangle Bounds { get; set; }
        void Draw(GameTime gameTime);
    }
}
