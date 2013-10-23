using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroadEngine.Core;
using Microsoft.Xna.Framework;
using BroadEngine.GameObjects.Modules;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BroadEngineTester.Activities
{
    public class ColorScreenActivity : Activity
    {
        ReversibleTransition<Color> cTrans;
        int count = 0;
        public override void Load()
        {

            cTrans = new ReversibleTransition<Color>(Color.BlanchedAlmond, Color.Coral, 1);
            cTrans.OnValueChanged = new Action<Color>(ChangeColor);
            cTrans.Loops = true;
            cTrans.OnLoopReverse = new Action(Reversed);
            cTrans.Running = true;
            _activityObjects.Add(cTrans);

            InputManager.AddInput(new Action(Reset), MouseButtons.MouseLeft, InputModifier.Clicked);
            InputManager.AddInput(new Action(Reset), Keys.Up, InputModifier.Held);
            InputManager.AddInput(new Action(Left), Keys.Left);
            InputManager.AddInput(new Action(Right), Keys.Right);
            InputManager.AddCombinedInput(new Action(Reset), Keys.Left, Keys.Right);
        }

        public override void Draw(GameTime gameTime, bool isPaused)
        {
            string[] inputs = InputManager.GetUsedInputs();
            int i;
            for (i = 0; i < inputs.Length; i++)
                Screen.DrawString(inputs[i], new Vector2(20, 20 + 20 * i), Color.Black);
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
            SetColor(Color.WhiteSmoke, Color.Black);
        }

        public void Right()
        {
            SetColor(Color.BlueViolet, Color.DarkRed);
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
