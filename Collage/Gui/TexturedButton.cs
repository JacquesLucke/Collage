﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class TexturedButton
    {
        DataAccess dataAccess;
        ImageSource source;
        Rectangle rectangle;
        bool isDown = false;
        bool wasDown = false;

        public TexturedButton(DataAccess dataAccess, ImageSource source, Point position)
        {
            this.dataAccess = dataAccess;
            this.source = source;
            rectangle = new Rectangle(position.X, position.Y, source.Width, source.Height);
        }
        public TexturedButton(DataAccess dataAccess, string fileName, Point position)
        {
            this.dataAccess = dataAccess;
            this.source = new ImageSource(dataAccess.GraphicsDevice, fileName);
            this.source.Load();
            rectangle = new Rectangle(position.X, position.Y, source.Width, source.Height);
        }
        public TexturedButton(DataAccess dataAccess, ImageSource source)
        {
            this.dataAccess = dataAccess;
            this.source = source;
            rectangle = new Rectangle(0, 0, 100, 100);
        }

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

        public void Draw()
        {
            dataAccess.SpriteBatch.Draw(source.Texture, rectangle, GetColorOverlay());
        }

        public Color GetColorOverlay()
        {
            Color color = Color.White;
            if (IsMouseOver  && !isDown) color = Color.FromNonPremultiplied(200, 200, 200, 255);
            if (dataAccess.Input.IsLeftButtonDown && !isDown) color = Color.White;
            if (isDown) color = Color.FromNonPremultiplied(160, 160, 160, 255);
            return color;
        }
    }
}
