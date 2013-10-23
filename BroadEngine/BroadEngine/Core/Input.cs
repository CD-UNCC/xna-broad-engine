using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BroadEngine.Core
{
    public enum MouseButtons
    {
        MouseLeft,
        MouseRight,
        MouseMiddle,
        MouseScroll,
        MouseScrollUp,
        MouseScrollDown,
        XButton1,
        XButton2
    }

    public enum ControllerButtons
    {

    }

    public enum InputModifier
    {
        Clicked,
        Held,
        Release,
        ClickRelease,
        HoldRelease
    }

    public static class InputManager
    {
        #region Fields

        static List<string> _inputToCheck = new List<string>();
        static Dictionary<string, List<Action>> _inputActions = new Dictionary<string, List<Action>>();
        static Dictionary<string[], List<Action>> _combinedInputActions = new Dictionary<string[], List<Action>>();

        static MouseState _pastMouse, _lastMouse, _curMouse;
        static KeyboardState _pastKeyboard, _lastKeyboard, _curKeyboard;
        

        #endregion

        #region Public Properties

        public static int ScrollValueChange { get { return _curMouse.ScrollWheelValue - _lastMouse.ScrollWheelValue; } }
        public static Vector2 MousePos { get { return new Vector2(_curMouse.X, _curMouse.Y); } }

        #endregion

        #region Private Methods

        static string AddInputToCheck(Enum input) 
        {
            string inputName = GetEnumName(input);
            if (!_inputToCheck.Contains(inputName))
                _inputToCheck.Add(inputName);
            return inputName;
        }
        static string GetEnumName(Enum input) { return Enum.GetName(input.GetType(), input); }

        #endregion

        #region Internal Methods

        internal static void Initialize()
        {
            _pastMouse = _lastMouse = _curMouse = Mouse.GetState();
            _pastKeyboard = _lastKeyboard = _curKeyboard = Keyboard.GetState();
        }

        internal static void Update()
        {
            _pastMouse = _lastMouse;
            _lastMouse = _curMouse;
            _curMouse = Mouse.GetState();

            _pastKeyboard = _lastKeyboard;
            _lastKeyboard = _curKeyboard;
            _curKeyboard = Keyboard.GetState();

            List<string> usedInputs = GetUsedInputs().ToList<string>();
            foreach (string[] inputs in _combinedInputActions.Keys)
            {
                if (usedInputs.Count == 0)
                    break;

                int curInput = -1;
                List<int> matchedIndexes = new List<int>();
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (inputs.Length > usedInputs.Count - curInput + matchedIndexes.Count)
                        break;

                    for (++curInput; curInput < usedInputs.Count; curInput++)
                        if (usedInputs[curInput] == inputs[i])
                        {
                            matchedIndexes.Add(curInput);
                            break;
                        }
                }
                if (matchedIndexes.Count == inputs.Length)
                {
                    for (int i = inputs.Length - 1; i >= 0; i--)
                        usedInputs.RemoveAt(matchedIndexes[i]);
                    foreach (Action action in _combinedInputActions[inputs])
                        action();
                }
            }

            string[] moddedInputs = GetModifiedUsedInputs(usedInputs.ToArray());
            for (int i = 0; i < moddedInputs.Length; i++)
            {
                List<Action> actions;
                if (_inputActions.TryGetValue(moddedInputs[i], out actions))
                    foreach (Action action in actions)
                        action();
            }
        }

        #endregion

        #region Public Methods

        public static void AddInput(Action onInput, Enum input, InputModifier mod)
        {
            string inputName = GetEnumName(mod) + AddInputToCheck(input);

            List<Action> inputActions;
            if (_inputActions.TryGetValue(inputName, out inputActions))
                inputActions.Add(onInput);
            else
            {
                inputActions = new List<Action>();
                inputActions.Add(onInput);
                _inputActions.Add(inputName, inputActions);
            }
        }
        public static void AddInput(Action onInput, Enum input) { AddInput(onInput, input, InputModifier.Clicked); }

        public static void AddCombinedInput(Action onInput, params Enum[] inputs)
        {
            if (inputs.Length <= 0)
                throw new InvalidOperationException("Input requires at least one input from Keys, MouseButtons, or ControllerButtons");
            if (inputs.Length == 1)
            {
                AddInput(onInput, inputs[0]);
                return;
            }

            List<string> inputNames = new List<string>();
            for (int i = 0; i < inputs.Length; i++) inputNames.Add(AddInputToCheck(inputs[i]));
            inputNames.Sort();

            string[] sortedNames = inputNames.ToArray();
            List<Action> inputActions;
            if (_combinedInputActions.TryGetValue(sortedNames, out inputActions))
                inputActions.Add(onInput);
            else
            {
                inputActions = new List<Action>();
                inputActions.Add(onInput);
                _combinedInputActions.Add(sortedNames, inputActions);
            }
        }

        public static string[] GetUsedInputs()
        {
            List<string> usedInputs = new List<string>();
            foreach (string toCheck in _inputToCheck)
            {
                Keys key;
                if (Enum.TryParse(toCheck, out key))
                {
                    if (_curKeyboard.IsKeyDown(key) || (_curKeyboard.IsKeyDown(key) != _lastKeyboard.IsKeyDown(key)))
                    {
                        usedInputs.Add(toCheck);
                        continue;
                    }
                }

                MouseButtons btn;
                if (Enum.TryParse(toCheck, out btn))
                {
                    if (IsButtonDown(btn) || IsButtonUp(btn) != IsButtonUp(btn, _lastMouse))
                    {
                        usedInputs.Add(toCheck);
                        continue;
                    } 
                }

                // TODO: Add gamepad support
            }
            usedInputs.Sort();
            return usedInputs.ToArray();
        }

        public static string[] GetModifiedUsedInputs(string[] usedInputs)
        {
            List<string> moddedInputs = new List<string>();
            foreach (string toCheck in _inputToCheck)
            {
                Keys key;
                if (Enum.TryParse(toCheck, out key))
                {
                    if (Clicked(key))
                        moddedInputs.Add("Clicked" + toCheck);
                    else if (Held(key))
                        moddedInputs.Add("Held" + toCheck);
                    else if (Released(key))
                        moddedInputs.Add("Released" + toCheck);
                    continue;
                }

                MouseButtons btn;
                if (Enum.TryParse(toCheck, out btn))
                {
                    if (Clicked(btn))
                        moddedInputs.Add("Clicked" + toCheck);
                    else if (Held(btn))
                        moddedInputs.Add("Held" + toCheck);
                    else if (Released(btn))
                        moddedInputs.Add("Released" + toCheck);
                    continue;
                }

                // TODO: Add gamepad support
            }
            moddedInputs.Sort();
            return moddedInputs.ToArray();
        }
        public static string[] GetModifiedUsedInputs() { return GetModifiedUsedInputs(GetUsedInputs()); }

        public static bool Clicked(Keys key)
        {
            return _curKeyboard.IsKeyDown(key) && _lastKeyboard.IsKeyUp(key);
        }
        public static bool Clicked(MouseButtons btn)
        {
            return IsButtonDown(btn) && IsButtonUp(btn, _lastMouse);
        }

        public static bool Held(Keys key)
        {
            return _curKeyboard.IsKeyDown(key) && _lastKeyboard.IsKeyDown(key);
        }
        public static bool Held(MouseButtons btn)
        {
            return IsButtonDown(btn) && IsButtonDown(btn, _lastMouse);
        }

        public static bool Released(Keys key)
        {
            return _curKeyboard.IsKeyUp(key) && _lastKeyboard.IsKeyDown(key);
        }
        public static bool Released(MouseButtons btn)
        {
            return IsButtonUp(btn) && IsButtonDown(btn, _lastMouse);
        }

        static bool IsButtonDown(MouseButtons btn, MouseState stateOn)
        {
            switch (btn)
            {
                case MouseButtons.MouseLeft:
                    return stateOn.LeftButton == ButtonState.Pressed;
                case MouseButtons.MouseRight:
                    return stateOn.RightButton == ButtonState.Pressed;
                case MouseButtons.MouseMiddle:
                    return stateOn.MiddleButton == ButtonState.Pressed;
                case MouseButtons.XButton1:
                    return stateOn.XButton1 == ButtonState.Pressed;
                case MouseButtons.XButton2:
                    return stateOn.XButton2 == ButtonState.Pressed;
                case MouseButtons.MouseScroll:
                    return ScrollValueChange != 0;
                case MouseButtons.MouseScrollDown:
                    return ScrollValueChange <= -1;
                case MouseButtons.MouseScrollUp:
                    return ScrollValueChange >= 1;
            }
            return false;
        }
        public static bool IsButtonDown(MouseButtons btn) { return IsButtonDown(btn, _curMouse); }
        static bool IsButtonUp(MouseButtons btn, MouseState stateOn) { return !IsButtonDown(btn, stateOn); }
        public static bool IsButtonUp(MouseButtons btn) { return IsButtonUp(btn, _lastMouse); }

        #endregion
    }
}
