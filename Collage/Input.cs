using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Collage
{
    public class Input
    {
        MouseState mouse = Mouse.GetState();
        MouseState lastMouse = Mouse.GetState();

        KeyboardState keyboard = Keyboard.GetState();
        KeyboardState lastKeyboard = Keyboard.GetState();

        public void Update()
        {
            lastMouse = mouse;
            lastKeyboard = keyboard;

            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();
        }

        #region Mouse
        public bool IsLeftButtonDown
        {
            get
            {
                return mouse.LeftButton == ButtonState.Pressed;
            }
        }
        public bool IsRightButtonDown
        {
            get
            {
                return mouse.RightButton == ButtonState.Pressed;
            }
        }
        public bool IsMiddleButtonDown
        {
            get
            {
                return mouse.MiddleButton == ButtonState.Pressed;
            }
        }

        public bool IsLeftButtonPressed
        {
            get
            {
                return mouse.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released;
            }
        }
        public bool IsRightButtonPressed
        {
            get
            {
                return mouse.RightButton == ButtonState.Pressed && lastMouse.RightButton == ButtonState.Released;
            }
        }

        public bool IsLeftButtonReleased
        {
            get
            {
                return lastMouse.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released;
            }
        }
        public bool IsRightButtonReleased
        {
            get
            {
                return lastMouse.RightButton == ButtonState.Pressed && mouse.RightButton == ButtonState.Released;
            }
        }

        public Vector2 MousePositionVector
        {
            get
            {
                return new Vector2(mouse.X, mouse.Y);
            }
        }
        public Point MousePositionPoint
        {
            get
            {
                return new Point(Convert.ToInt32(mouse.X), Convert.ToInt32(mouse.Y));
            }
        }

        public Vector2 LastMousePositionVector
        {
            get
            {
                return new Vector2(lastMouse.X, lastMouse.Y);
            }
        }
        public Point LastMousePositionPoint
        {
            get
            {
                return new Point(Convert.ToInt32(lastMouse.X), Convert.ToInt32(lastMouse.Y));
            }
        }

        public void SetMousePosition(Point position)
        {
            Mouse.SetPosition(position.X, position.Y);
        }
        public void SetMousePosition(Vector2 position)
        {
            Mouse.SetPosition((int)Math.Round(position.X), (int)Math.Round(position.Y));
        }

        public int ScrollWheelValue
        {
            get
            {
                return mouse.ScrollWheelValue;
            }
        }
        public int LastScrollWheelValue
        {
            get
            {
                return lastMouse.ScrollWheelValue;
            }
        }
        public int ScrollWheelDifference
        {
            get
            {
                return mouse.ScrollWheelValue - lastMouse.ScrollWheelValue;
            }
        }

        public int MouseX
        {
            get
            {
                return mouse.X;
            }
        }
        public int MouseY
        {
            get
            {
                return mouse.Y;
            }
        }

        public int MouseXDifference
        {
            get
            {
                return mouse.X - lastMouse.X;
            }
        }
        public int MouseYDifference
        {
            get
            {
                return mouse.Y - lastMouse.Y;
            }
        }

        public Vector2 MouseDifferenceVector
        {
            get
            {
                return new Vector2(mouse.X - lastMouse.X, mouse.Y - lastMouse.Y);
            }
        }
        public Point MouseDifferencePoint
        {
            get
            {
                return new Point(mouse.X - lastMouse.X, mouse.Y - lastMouse.Y);
            }
        }
        #endregion

        #region Keyboard
        public bool IsKeyDown(Keys key)
        {
            return keyboard.IsKeyDown(key);
        }
        public bool IsKeyPressed(Keys key)
        {
            return keyboard.IsKeyDown(key) && lastKeyboard.IsKeyUp(key);
        }
        public bool IsKeyReleased(Keys key)
        {
            return lastKeyboard.IsKeyDown(key) && keyboard.IsKeyUp(key);
        }
        public bool IsOnlyKeyPressed(Keys key)
        {
            Keys[] pressedKeys = GetPressedKeys();
            if (pressedKeys.Length == 1)
            {
                return pressedKeys[0] == key;
            }
            return false;
        }

        public bool IsLastKeyDown(Keys key)
        {
            return lastKeyboard.IsKeyDown(key);
        }

        public bool AreKeysDown(params Keys[] keys)
        {

            foreach (Keys k in keys)
            {
                if (!IsKeyDown(k))
                {
                    return false;
                }
            }
            return true;
        }
        public bool AreKeysPressed(params Keys[] keys)
        {
            bool output = AreKeysDown(keys);
            bool isPressed = false;
            for (int i = 0; i < keys.Length; i++)
            {
                if (IsKeyPressed(keys[i]))
                {
                    isPressed = true;
                    break;
                }
            }
            return output && isPressed;
        }

        public bool IsShift
        {
            get
            {
                return keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            }
        }
        public bool IsAlt
        {
            get
            {
                return keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt);
            }
        }
        public bool IsStrg
        {
            get
            {
                return keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
            }
        }

        public bool IsLastShift
        {
            get
            {
                return lastKeyboard.IsKeyDown(Keys.LeftShift) || lastKeyboard.IsKeyDown(Keys.RightShift);
            }
        }
        public bool IsLastAlt
        {
            get
            {
                return lastKeyboard.IsKeyDown(Keys.LeftAlt) || lastKeyboard.IsKeyDown(Keys.RightAlt);
            }
        }
        public bool IsLastStrg
        {
            get
            {
                return lastKeyboard.IsKeyDown(Keys.LeftControl) || lastKeyboard.IsKeyDown(Keys.RightControl);
            }
        }

        public Keys[] GetPressedKeys()
        {
            return keyboard.GetPressedKeys();
        }

        public string GetWrittenNumber()
        {
            string output = "";
            if (IsKeyPressed(Keys.D0)) output += "0";
            if (IsKeyPressed(Keys.D1)) output += "1";
            if (IsKeyPressed(Keys.D2)) output += "2";
            if (IsKeyPressed(Keys.D3)) output += "3";
            if (IsKeyPressed(Keys.D4)) output += "4";
            if (IsKeyPressed(Keys.D5)) output += "5";
            if (IsKeyPressed(Keys.D6)) output += "6";
            if (IsKeyPressed(Keys.D7)) output += "7";
            if (IsKeyPressed(Keys.D8)) output += "8";
            if (IsKeyPressed(Keys.D9)) output += "9";

            return output;
        }

        public bool IsArrowKeyPressed()
        {
            return IsKeyPressed(Keys.Left) || IsKeyPressed(Keys.Right) || IsKeyPressed(Keys.Up) || IsKeyPressed(Keys.Down);
        }
        #endregion
    }
}