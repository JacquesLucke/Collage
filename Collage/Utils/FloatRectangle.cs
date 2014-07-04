using Microsoft.Xna.Framework;
using System;

namespace Collage
{
    public struct FloatRectangle
    {
        float x;
        float y;
        float width;
        float height;

        public FloatRectangle(Rectangle rec)
        {
            this.x = rec.X;
            this.y = rec.Y;
            this.width = rec.Width;
            this.height = rec.Height;
        }
        public FloatRectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        public FloatRectangle(Vector2 start, Vector2 end)
        {
            x = Math.Min(start.X, end.X);
            y = Math.Min(start.Y, end.Y);
            width = Math.Abs(end.Y - start.X);
            height = Math.Abs(start.Y - end.Y);
        }
        public static FloatRectangle CreateRectangle(float x1, float y1, float x2, float y2)
        {
            FloatRectangle output = new FloatRectangle();
            output.x = Math.Min(x1, x2);
            output.y = Math.Min(y1, y2);
            output.width = Math.Abs(x2 - x1);
            output.height = Math.Abs(y2 - y1);
            return output;
        }

        /// <summary>
        /// Does a Math.Round method on every Parameter
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(width), (int)Math.Round(height));
        }
        /// <summary>
        /// Does a Math.Ceiling or Math.Floor method on every Parameter
        /// </summary>
        /// <returns></returns>
        public Rectangle ToOuterRectangle()
        {
            return new Rectangle((int)Math.Floor(x), (int)Math.Floor(y), (int)Math.Ceiling(width), (int)Math.Ceiling(height));
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(width, height);
            }
            set
            {
                width = value.X;
                height = value.Y;
            }
        }

        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
                width -= value.X - x;
                height -= value.Y - y;
            }
        }
        public Vector2 TopRight
        {
            get
            {
                return new Vector2(x + width, y);
            }
            set
            {
                y = value.Y;
                width = value.X - x;
                height -= value.Y - y;
            }
        }
        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(x, y + height);
            }
            set
            {
                x = value.X;
                width -= value.X - x;
                height = value.Y - y;
            }
        }
        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(x + width, y + height);
            }
            set
            {
                width = value.X - x;
                height = value.Y - y;
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(x + width / 2f, y + height / 2f);
            }
            set
            {
                x = value.X - width / 2f;
                y = value.Y - height / 2f;
            }
        }

        public float X { get { return x; } }
        public float Y { get { return y; } }
        public float Width { get { return width; } }
        public float Height { get { return height; } }

        public float AspectRatio
        {
            get { return width / height; }
            set { width = height * value; }
        }
    }
}
