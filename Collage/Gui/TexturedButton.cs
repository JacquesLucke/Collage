using Microsoft.Xna.Framework;
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
        FloatRectangle position;

        public TexturedButton(DataAccess dataAccess, ImageSource source, Rectangle position)
        {
            this.dataAccess = dataAccess;
            this.source = source;
            this.position = new FloatRectangle(position);
        }

        public FloatRectangle DrawPosition
        {
            get { return position; }
            set { position = value; }
        }
        public FloatRectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool IsOver
        {
            get { return position.ToRectangle().Contains(dataAccess.Input.MousePositionPoint); }
        }
        public bool IsDown
        {
            get { return IsOver && dataAccess.Input.IsLeftButtonDown; }
        }
        public bool IsPressed
        {
            get { return IsOver && dataAccess.Input.IsLeftButtonPressed; }
        }

        public void Draw()
        {
            dataAccess.SpriteBatch.Draw(source.Texture, position.ToRectangle(), Color.White);
        }
    }
}
