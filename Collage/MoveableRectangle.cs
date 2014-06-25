using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage.Utils
{
    public struct MoveableRectangle
    {
        FloatRectangle current;
        FloatRectangle start;

        public MoveableRectangle(FloatRectangle startRectangle)
        {
            start = startRectangle;
            current = startRectangle;
        }

        public void Reset()
        {
            current = start;
        }
        public void Move(Vector2 distance)
        {
            current.Position += distance;
        }
        public void Zoom(float factor, Vector2 zoomCenter)
        {
            float ratio = current.AspectRatio;
            float leftPercent = (zoomCenter.X - current.X) / current.Width;
            float topPercent = (zoomCenter.Y - current.Y) / current.Height;

            float X1, Y1, X2, Y2;
            X1 = current.X;
            Y1 = current.Y;
            X2 = current.X + current.Width;
            Y2 = current.Y + current.Height;

            X1 += leftPercent * factor * ratio;
            X2 -= (1 - leftPercent) * factor * ratio;

            Y1 += topPercent * factor;
            Y2 -= (1 - topPercent) * factor;

            if (X1 + 50 < X2 && Y1 + 50 < Y2)
            {
                current.Position = new Vector2(X1, Y1);
                current.Size = new Vector2(X2 - X1, Y2 - Y1);
            }
        }

        public Rectangle Rectangle
        {
            get { return current.ToRectangle(); }
        }

        public Vector2 Position { get { return current.Position; } }
        public Vector2 Center { get { return current.Center; } }
    }
}
