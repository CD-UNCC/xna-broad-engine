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
        Released
    }

    public static class InputManager
    {
        #region Fields

        static List<Enum> _inputToCheck = new List<Enum>();
        static Dictionary<KeyValuePair<Enum, InputModifier>, Enum> _inputHandlers = new Dictionary<KeyValuePair<Enum, InputModifier>, Enum>();
        static Dictionary<Enum[], Enum> _combinedInputActions = new Dictionary<Enum[], Enum>();

        static MouseState _pastMouse, _lastMouse, _curMouse;
        static KeyboardState _pastKeyboard, _lastKeyboard, _curKeyboard;
        

        #endregion

        #region Public Properties

        public static int ScrollValueChange { get { return _curMouse.ScrollWheelValue - _lastMouse.ScrollWheelValue; } }
        public static Vector2 MousePos { get { return new Vector2(_curMouse.X, _curMouse.Y); } }
        public static InputModifier DefaultInputModifier { get; set; }

        #endregion

        #region Static Constructor

        static InputManager()
        {
            DefaultInputModifier = InputModifier.Clicked;
        }

        #endregion

        #region Private Methods

        static void AddInputToCheck(Enum input) 
        {            
            if (!_inputToCheck.Contains(input))
                _inputToCheck.Add(input);
        }
        
        static string GetEnumName(Enum input) { return Enum.GetName(input.GetType(), input); }

        static int CompareEnums(Enum a, Enum b)
        {
            return GetEnumName(a).CompareTo(GetEnumName(b));
        }
        static int CompareKeyedEnums<T>(KeyValuePair<Enum, T> a, KeyValuePair<Enum, T> b)
        {
            return CompareEnums(a.Key, b.Key);
        }
        static int CompareArrayedKeyLengths<K, V>(KeyValuePair<K[], V> a, KeyValuePair<K[], V> b)
        {
            return a.Key.Length.CompareTo(b.Key.Length);
        }

        static bool IsUsed(Enum input, out InputModifier mod)
        {
            mod = InputModifier.Clicked;
            if (input is Keys)
            {
                if (Clicked((Keys)input))
                    return true;
                if (Held((Keys)input))
                {
                    mod = InputModifier.Held;
                    return true;
                }
                if (Released((Keys)input))
                {
                    mod = InputModifier.Released;
                    return true;
                }
                return false;
            }
            if (input is MouseButtons)
            {
                if (Clicked((MouseButtons)input))
                    return true;
                if (Held((MouseButtons)input))
                {
                    mod = InputModifier.Held;
                    return true;
                }
                if (Released((MouseButtons)input))
                {
                    mod = InputModifier.Released;
                    return true;
                }
                return false;
            }

            // TODO: Add support for gamepad

            return false;
        }

        static bool CheckCombinedInput(Enum[] required, List<KeyValuePair<Enum, InputModifier>> usedInputs)
        {
            int curInput = 0;
            List<int> matchedIndexes = new List<int>();

            for (int i = 0; i < required.Length; i++)
            {
                if (required.Length > usedInputs.Count - curInput + matchedIndexes.Count)
                    return false;
                for (curInput = curInput; curInput < usedInputs.Count; curInput++)
                    if (GetEnumName(usedInputs[curInput].Key) == GetEnumName(required[i]))
                    {
                        matchedIndexes.Add(curInput);
                        break;
                    }
            }

            if (matchedIndexes.Count == required.Length)
            {
                for (int i = matchedIndexes.Count - 1; i >= 0; i--)
                    usedInputs.RemoveAt(matchedIndexes[i]);
                return true;
            }
            return false;
        }

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

            List<KeyValuePair<Enum, InputModifier>> inputs = GetUsedInputs();

            List<KeyValuePair<Enum[], Enum>> combinedInputs = _combinedInputActions.ToList();
            combinedInputs.Sort(new Comparison<KeyValuePair<Enum[], Enum>>(CompareArrayedKeyLengths));
            combinedInputs.Reverse();

            foreach (KeyValuePair<Enum[], Enum> pair in combinedInputs)
                if (CheckCombinedInput(pair.Key, inputs))
                    ActivityManager.CurrentActivity.HandleInput(pair.Value);

            foreach (KeyValuePair<Enum, InputModifier> input in inputs)
            {
                Enum controller;
                if (_inputHandlers.TryGetValue(input, out controller))
                    ActivityManager.CurrentActivity.HandleInput(controller);
            }
        }

        #endregion

        #region Public Methods

        public static void AddInput(Enum inputHandler, Enum input, InputModifier mod)
        {
            KeyValuePair<Enum, InputModifier> keyV = new KeyValuePair<Enum, InputModifier>(input, mod);
            if (_inputHandlers.ContainsKey(keyV))
                throw new ArgumentException("An input with that key and modifier is already specified", "input, mod");
            else
            {
                AddInputToCheck(input);
                _inputHandlers.Add(keyV, inputHandler);
            }
        }
        public static void AddInput(Enum inputHandler, Enum input) { AddInput(inputHandler, input, DefaultInputModifier); }

        public static void AddCombinedInput(Enum inputHandler, params Enum[] inputs)
        {
            if (inputs.Length <= 0)
                throw new ArgumentException("Combined inputs require 2 or more inputs to be passed", "inputs");
            if (inputs.Length == 1)
            {
                AddInput(inputHandler, inputs[0]);
                return;
            }

            List<Enum> keys = new List<Enum>();
            for (int i = 0; i < inputs.Length; i++)
            {
                AddInputToCheck(inputs[i]);
                keys.Add(inputs[i]);
            }
            keys.Sort(new Comparison<Enum>(CompareEnums));
            _combinedInputActions.Add(keys.ToArray(), inputHandler);
        }

        public static List<KeyValuePair<Enum, InputModifier>> GetUsedInputs()
        {
            List<KeyValuePair<Enum, InputModifier>> usedInputs = new List<KeyValuePair<Enum, InputModifier>>();
            foreach (Enum toCheck in _inputToCheck)
            {
                InputModifier mod;
                if (IsUsed(toCheck, out mod))
                    usedInputs.Add(new KeyValuePair<Enum, InputModifier>(toCheck, mod));
            }
            usedInputs.Sort(new Comparison<KeyValuePair<Enum, InputModifier>>(CompareKeyedEnums));

            return usedInputs;
        }

        /*
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
         */

        #region Input States

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
        public static bool IsButtonUp(MouseButtons btn) { return IsButtonUp(btn, _curMouse); }

        #endregion

        #endregion
    }
}
