using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroadEngine.Core;
using Microsoft.Xna.Framework;
using BroadEngine.GameObjects.Modules;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BroadEngine.GameObjects.Sprites;

namespace BroadEngineTester.Activities
{
    public class ColorScreenActivity : Activity
    {
        ReversibleTransition<Color> cTrans;
        SimpleSprite sprite;
        int count = 0;
        public override void Load()
        {
            sprite = new SimpleSprite(ContentLoader.Get<Texture2D>("TestSprite"));
            AddObject(sprite);
            cTrans = new ReversibleTransition<Color>(Color.BlanchedAlmond, Color.Coral, 1);
            cTrans.OnValueChanged = new Action<Color>(ChangeColor);
            cTrans.Loops = true;
            cTrans.OnLoopReverse = new Action(Reversed);
            cTrans.Running = true;
            AddObject(cTrans);

            InputManager.AddInput(new Action(Reset), MouseButtons.MouseLeft, InputModifier.Clicked);
            InputManager.AddInput(new Action(Left), Keys.Left, InputModifier.Held);
            InputManager.AddInput(new Action(Right), Keys.Right, InputModifier.Held);
            InputManager.AddInput(new Action(Down), Keys.Down, InputModifier.Held);
            InputManager.AddInput(new Action(Up), Keys.Up, InputModifier.Held);
            InputManager.AddInput(new Action(Q), Keys.Q, InputModifier.Held);
            InputManager.AddInput(new Action(E), Keys.E, InputModifier.Held);
            InputManager.AddInput(new Action(W), Keys.W, InputModifier.Held);
            InputManager.AddInput(new Action(S), Keys.S, InputModifier.Held);
            InputManager.AddCombinedInput(new Action(Reset), Keys.Left, Keys.Right);
        }

        public override void Draw(GameTime gameTime, bool isPaused)
        {
            Vector2[] corners = sprite.RotatedCorners;
            int i;
            for (i = 0; i < corners.Length; i++)
                Screen.DrawString(corners[i].ToString(), new Vector2(20, 20 + 20 * i), Color.Black);
            Screen.DrawRect(sprite.Bounds, Color.DarkTurquoise * .5f);
            base.Draw(gameTime, isPaused);
        }

        public void Reversed()
        {
            if (++count >= 5)
            {
                count = 0;
                cTrans.Loops = false;
                AddDelayedAction(2, new Action(RestartLoop));
            }
        }

        public void Reset()
        {
            cTrans.Reset();
        }

        public void Left()
        {
            sprite.Position.X -= .3f;
        }

        public void Right()
        {
            sprite.Position.X += .3f;
        }

        public void Up()
        {
            sprite.Position.Y -= .3f;
        }

        public void Down()
        {
            sprite.Position.Y += .3f;
        }

        public void Q()
        {
            sprite.Rotation += .2f;
        }

        public void E()
        {
            sprite.Rotation -= .2f;
        }

        public void W()
        {
            sprite.Scale += .01f;
        }

        public void S()
        {
            sprite.Scale -= .01f;
        }

        public void SetColor(Color start, Color end)
        {
            ReversibleTransition<Color> tmpTrans = new ReversibleTransition<Color>(start, end, 1);
            tmpTrans.OnValueChanged = new Action<Color>(ChangeColor);
            tmpTrans.Loops = true;
            tmpTrans.OnLoopReverse = new Action(Reversed);
            tmpTrans.Running = true;
            ReplaceObject(ref cTrans, tmpTrans);
        }

        public void RestartLoop()
        {
            cTrans.Loops = true;
            cTrans.Running = true;
        }

        public void ChangeColor(Color color)
        {
            Screen.ScreenClearColor = color;
        }
    }
}
