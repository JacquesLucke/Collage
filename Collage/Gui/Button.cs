using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage.Gui
{
    public abstract class Button
    {
        public Button(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            rectangle = new Rectangle(0, 0, 100, 100);
        }

        protected DataAccess dataAccess;
        protected Rectangle rectangle;
        protected bool isDown = false;
        protected bool wasDown = false;

        public Rectangle DrawPosition
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        public int X
        {
            get { return rectangle.X; }
            set { rectangle.X = value; }
        }
        public int Y
        {
            get { return rectangle.Y; }
            set { rectangle.Y = value; }
        }
        public int Width
        {
            get { return rectangle.Width; }
            set { rectangle.Width = value; }
        }
        public int Height
        {
            get { return rectangle.Height; }
            set { rectangle.Height = value; }
        }

        public bool IsMouseOver
        {
            get { return rectangle.Contains(dataAccess.Input.MousePositionPoint); }
        }
        public bool IsMouseOverAndDown
        {
            get { return IsMouseOver && dataAccess.Input.IsLeftButtonDown; }
        }
        public bool IsMouseOverAndPressed
        {
            get { return IsMouseOver && dataAccess.Input.IsLeftButtonPressed; }
        }

        public bool IsDown
        {
            get { return isDown; }
        }
        public bool IsReleased
        {
            get { return wasDown && !isDown; }
        }

        public void Update()
        {
            wasDown = isDown;
            if (isDown)
                isDown = dataAccess.Input.IsLeftButtonDown;
            else
                isDown = IsMouseOverAndPressed;
        }

        protected Color GetColorStatus()
        {
            Color color = Color.White;
            if (IsMouseOver  && !dataAccess.Input.IsLeftButtonDown) color = Color.FromNonPremultiplied(200, 200, 200, 255);
            if (isDown) color = Color.FromNonPremultiplied(160, 160, 160, 255);
            return color;
        }
    }
}