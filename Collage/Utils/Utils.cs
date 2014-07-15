using Gtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Runtime.InteropServices;
using drawing = System.Drawing;

namespace Collage
{
    public abstract class Utils
    {
        #region convert file types tp filter
        public static string FileTypesToWinFormFilter(params FileTypes[] types)
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
        public static FileFilter FileTypesToGtkFilter(params FileTypes[] types)
        {
            FileFilter filter = new FileFilter();
            for (int i = 0; i < types.Length; i++)
            {
                FileTypes type = types[i];
                switch (type)
                {
                    case FileTypes.Images:
                        filter.AddPattern("*.jpg");
                        filter.AddPattern("*.png");
                        filter.AddPattern("*.bmp");
                        filter.Name += "Images ";
                        break;
                    case FileTypes.JPG:
                        filter.AddPattern("*.jpg");
                        filter.Name += "Jpg ";
                        break;
                    case FileTypes.PNG:
                        filter.AddPattern("*.png");
                        filter.Name += "Png ";
                        break;
                    case FileTypes.BMP:
                        filter.AddPattern("*.bmp");
                        filter.Name += "Bmp ";
                        break;
                }
            }
            return filter;
        }
        #endregion

        #region convert Vectors and Points
        public static Point ToPoint(Vector2 vector)
        {
            return new Point((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
        }
        public static Vector2 ToVector(Point point)
        {
            return new Vector2(point.X, point.Y);
        }
        #endregion

        #region Convert Colors and Vectors
        public static Vector4 ToVector(Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        public static Color ToColor(Gdk.Color color)
        {
            return Color.FromNonPremultiplied(color.Red / 256, color.Green / 256, color.Blue / 256, 255);
        }
        public static Gdk.Color ToColor(Color color)
        {
            return new Gdk.Color(color.R, color.G, color.B);
        }
        public static Color MultiplyColors(params Color[] colors)
        {
            Color output = Color.White;
            for(int i= 0; i< colors.Length;i++)
            {
                output.R = (byte)Math.Round(output.R * colors[i].R / 255f);
                output.G = (byte)Math.Round(output.G * colors[i].G / 255f);
                output.B = (byte)Math.Round(output.B * colors[i].B / 255f);
                output.A = (byte)Math.Round(output.A * colors[i].A / 255f);
            }
            return output;
        }
        #endregion

        #region Convert Textures and Bitmaps
        public static drawing.Bitmap ToBitmap(Texture2D texture)
        {
            drawing.Bitmap bitmap = new drawing.Bitmap(texture.Width, texture.Height, drawing.Imaging.PixelFormat.Format32bppArgb);

            byte blue;
            IntPtr safePtr;
            drawing.Imaging.BitmapData bitmapData;
            drawing.Rectangle rect = new drawing.Rectangle(0, 0, texture.Width, texture.Height);
            byte[] textureData = new byte[4 * texture.Width * texture.Height];

            texture.GetData<byte>(textureData);
            for (int i = 0; i < textureData.Length; i += 4)
            {
                blue = textureData[i];
                textureData[i] = textureData[i + 2];
                textureData[i + 2] = blue;
            }
            bitmapData = bitmap.LockBits(rect, drawing.Imaging.ImageLockMode.WriteOnly, drawing.Imaging.PixelFormat.Format32bppArgb);
            safePtr = bitmapData.Scan0;
            Marshal.Copy(textureData, 0, safePtr, textureData.Length);
            bitmap.UnlockBits(bitmapData);

            textureData = null;
            texture.Dispose();

            return bitmap;
        }
        public static Texture2D ToTexture(drawing.Bitmap bitmap, GraphicsDevice graphicsDevice)
        {
            Texture2D texture;
            if (bitmap == null)
            {
                return null;
            }
            texture = new Texture2D(graphicsDevice, bitmap.Width, bitmap.Height);

            // MemoryStream to store the bitmap data.
            MemoryStream ms = new MemoryStream();
            // Save image to MemoryStream
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            //Go to the beginning of the memory stream.
            ms.Seek(0, SeekOrigin.Begin);

            //Fill the texture.
            texture = Texture2D.FromStream(graphicsDevice, ms);

            ms.Close();
            ms.Dispose();
            ms = null;

            return texture;
        }
        #endregion

        #region Calculations and Tests on Points and Rectangles
        // this will calculate a vector by rotating another one around an origin
        public static Vector2 RotateAroundOrigin(Vector2 position, Vector2 origin, float rotation)
        {
            return Vector2.Transform((position - origin), Matrix.CreateRotationZ(-rotation)) + origin;
        }
        // a better contains method that allows rotated rectangles. This works by rotating the vector and use then the normal Contains method
        public static bool IsVectorInRotatedRectangle(Vector2 vector, Rectangle rectangle, float rotation)
        {
            Vector2 rotatedVector = RotateAroundOrigin(vector, ToVector(rectangle.Center), rotation);
            return rectangle.Contains(rotatedVector);
        }
        // creates a bigger bounding box of a rotated rectangle
        public static Rectangle GetBoundingBox(Rectangle rectangle, float rotation)
        {
            Vector2 topLeft = RotateAroundOrigin(new Vector2(rectangle.Left, rectangle.Top), ToVector(rectangle.Center), rotation);
            Vector2 topRight = RotateAroundOrigin(new Vector2(rectangle.Right, rectangle.Top), ToVector(rectangle.Center), rotation);
            Vector2 bottomLeft = RotateAroundOrigin(new Vector2(rectangle.Left, rectangle.Bottom), ToVector(rectangle.Center), rotation);
            Vector2 bottomRight = RotateAroundOrigin(new Vector2(rectangle.Right, rectangle.Bottom), ToVector(rectangle.Center), rotation);

            int left = (int)Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            int right = (int)Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            int top = (int)Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            int bottom = (int)Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            Rectangle boundingBox = new Rectangle();
            boundingBox.X = left;
            boundingBox.Y = top;
            boundingBox.Width = right - left;
            boundingBox.Height = bottom - top;

            return boundingBox;
        }
        // checks if the distance between to rectangles is lower than the sum of the diagonals (the half of this)
        public static bool DiagonalCollisionTest(Rectangle rectangle1, Rectangle rectangle2)
        {
            float distance = Vector2.Distance(ToVector(rectangle1.Center), ToVector(rectangle2.Center));
            float diagonal1 = (float)Math.Sqrt(rectangle1.Width * rectangle1.Width + rectangle1.Height * rectangle1.Height);
            float diagonal2 = (float)Math.Sqrt(rectangle2.Width * rectangle2.Width + rectangle2.Height * rectangle2.Height);
            return distance <= (diagonal1 + diagonal2) / 2;
        }
        // checks if the bounding boxes of both rotated rectangles are intersecting
        public static bool BoundingBoxCollisionTest(Rectangle rectangle1, Rectangle rectangle2, float rotation1, float rotation2)
        {
            Rectangle rec1 = GetBoundingBox(rectangle1, rotation1);
            Rectangle rec2 = GetBoundingBox(rectangle2, rotation2);
            return rec1.Intersects(rec2);
        }
        // not-exact test if 2 rotated rectangles are intersecting
        public static bool CouldOverlap(Rectangle rectangle1, Rectangle rectangle2, float rotation1, float rotation2)
        {
            if (DiagonalCollisionTest(rectangle1, rectangle2))
            {
                return BoundingBoxCollisionTest(rectangle1, rectangle2, rotation1, rotation2);
            }
            return false;
        }
        #endregion

        public static Rectangle ExpandRectangle(Rectangle source, int margin)
        {
            source.X -= margin;
            source.Y -= margin;
            source.Width += margin * 2;
            source.Height += margin * 2;
            return source;
        }

        public static Vector2 CenterText(string text, SpriteFont font, Rectangle boundary)
        {
            Vector2 size = font.MeasureString(text);
            return new Vector2((boundary.Width - (int)Math.Round(size.X)) / 2 + boundary.X,
                (boundary.Height - (int)Math.Round(size.Y)) / 2 + boundary.Y - 1);
        }
        public static int CenterTextHorizontal(string text, SpriteFont font, Rectangle boundary)
        {
            Vector2 size = font.MeasureString(text);
            return (boundary.Width - (int)Math.Round(size.X)) / 2 + boundary.X;
        }

        public static int CenterTextVertical(string text, SpriteFont font, Rectangle boundary)
        {
            Vector2 size = font.MeasureString(text);
            return (boundary.Height - (int)Math.Round(size.Y)) / 2 + boundary.Y - 1;
        }
    }
}
