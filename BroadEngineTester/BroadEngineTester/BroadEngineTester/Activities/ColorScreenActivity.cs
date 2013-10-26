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
        LoopingSprite sprite;
        int count = 0;
        public override void Load()
        {
            sprite = new LoopingSprite("TestAnimation", 33, 100, 100, 5, 5);
            sprite.Position = Screen.Center;
            sprite.Color = Color.BlueViolet;
            AddObject(sprite);
            cTrans = new ReversibleTransition<Color>(Color.BlanchedAlmond, Color.Coral, 1);
            cTrans.OnValueChanged = new Action<Color>(ChangeColor);
            cTrans.Loops = true;
            cTrans.OnLoopReverse = new Action(Reversed);
            cTrans.Running = true;
            AddObject(cTrans);

            InputManager.AddInput(ColorControls.Click, MouseButtons.MouseLeft);
            InputManager.AddInput(ColorControls.Up, Keys.Up, InputModifier.Held);
            InputManager.AddInput(ColorControls.Left, Keys.Left, InputModifier.Held);
            InputManager.AddInput(ColorControls.Right, Keys.Right, InputModifier.Held);
            InputManager.AddInput(ColorControls.Down, Keys.Down, InputModifier.Held);
            InputManager.AddCombinedInput(ColorControls.RotateRight, Keys.Left, Keys.Right);
            InputManager.AddCombinedInput(ColorControls.RotateLeft, Keys.Left, Keys.Right, Keys.RightShift);
        }

        public override void HandleInput(Enum input)
        {
            switch ((ColorControls)input)
            {
                case ColorControls.Click:
                    cTrans.Reset();
                    break;
                case ColorControls.Left:
                    sprite.Position.X -= .5f;
                    break;
                case ColorControls.Right:
                    sprite.Position.X += .5f;
                    break;
                case ColorControls.Up:
                    sprite.Position.Y -= .5f;
                    break;
                case ColorControls.Down:
                    sprite.Position.Y += .5f;
                    break;
                case ColorControls.RotateRight:
                    sprite.Rotation += .05f;
                    break;
                case ColorControls.RotateLeft:
                    sprite.Rotation -= .05f;
                    break;
            }
        }

        public override void Draw(GameTime gameTime, bool isPaused)
        {
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

        enum ColorControls
        {
            Left,
            Right,
            Up,
            Down,
            Click,
            RotateRight,
            RotateLeft
        }
    }
}
