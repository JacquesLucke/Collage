using Microsoft.Xna.Framework;

namespace Collage
{
    public class MoveableRectangle
    {
        FloatRectangle current;

        public MoveableRectangle(FloatRectangle startRectangle)
        {
            current = startRectangle;
        }
        public void SetRectangle(Rectangle rectangle)
        {
            current = new FloatRectangle(rectangle);
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

            if (X1 + 50 < X2 && Y1 + 50 < Y2 || factor < 1)
            {
                current.Position = new Vector2(X1, Y1);
                current.Size = new Vector2(X2 - X1, Y2 - Y1);
            }
        }

        public float AspectRatio
        {
            get { return current.AspectRatio; }
            set { current.AspectRatio = value; }
        }

        public Rectangle Rectangle
        {
            get { return current.ToRectangle(); }
        }

        public Vector2 Position { get { return current.Position; } }
        public Vector2 Center { get { return current.Center; } }
    }
}
