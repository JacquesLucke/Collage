using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public abstract class Utils
    {
        public static string FileTypesToFilter(params FileTypes[] types)
        {
            string filter = "";
            for(int i = 0; i< types.Length; i++)
            {
                FileTypes type = types[i];
                switch (type)
                {
                    case FileTypes.Images:
                        filter += "Images|*.jpg;*.png;*.bmp"; break;
                    case FileTypes.JPG:
                        filter += "Jpg|*.jpg"; break;
                    case FileTypes.PNG:
                        filter += "Png|*.png"; break;
                    case FileTypes.BMP:
                        filter += "Bmp|*.bmp"; break;
                }
                if (i < types.Length - 1) filter += "|";
            }
            return filter;
        }

        public static Vector2 RotateAroundOrigin(Vector2 position, Vector2 origin, float rotation)
        {
            return Vector2.Transform((position - origin), Matrix.CreateRotationZ(-rotation)) + origin;
        }
        public static bool IsVectorInRotatedRectangle(Vector2 vector, Rectangle rectangle, float rotation)
        {
            Vector2 rotatedVector = RotateAroundOrigin(vector, ToVector(rectangle.Center), rotation);
            return rectangle.Contains(rotatedVector);
        }

        // convert Vectors and Points
        public static Point ToPoint(Vector2 vector)
        {
            return new Point((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
        }
        public static Vector2 ToVector(Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
