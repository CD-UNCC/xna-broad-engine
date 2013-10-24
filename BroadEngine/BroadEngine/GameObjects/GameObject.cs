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

        public IEnumerable<T> GetChildrenOfType<T>() { return _children.OfType<T>(); }

        #endregion

        #region Protected Methods

        protected bool IsChildOf(GameObject toCheck)
        {
            GameObject parent = this.Parent;

            while (parent != null)
            {
                if (toCheck.Equals(parent))
                    return true;
                else
                    parent = parent.Parent;
            }

            return false;
        }

        protected virtual void Attach(IAttaches child)
        {
            (child as GameObject).Parent = this;
            child.OnAttach(this as IAttachable);
        }

        protected void UpdateChildren(GameTime gameTime, bool isPaused)
        {
            var toUpdate = GetChildrenOfType<IUpdateable>();
            foreach (IUpdateable child in toUpdate)
                child.Update(gameTime, isPaused);
        }

        protected void DrawChildren(GameTime gameTime, bool isPaused)
        {
            var toDraw = GetChildrenOfType<IDrawable>();
            foreach (IDrawable child in toDraw)
                child.Draw(gameTime, isPaused);
        }

        #endregion
    }

    /// <summary>
    /// An interface for game objects that attach to another object
    /// </summary>
    public interface IAttaches
    {
        void OnAttach(IAttachable parent);
    }

    /// <summary>
    /// An interface for game objects that can have other objects attached to them
    /// </summary>
    public interface IAttachable
    {
        void Attach(IAttaches child);
    }

    /// <summary>
    /// An interface for game objects that can be updated
    /// </summary>
    public interface IUpdateable
    {
        void Update(GameTime gameTime, bool isPaused);
    }

    /// <summary>
    /// An interface for game objects that can be drawn
    /// </summary>
    public interface IDrawable
    {
        void Draw(GameTime gameTime, bool isPaused);
    }
}
