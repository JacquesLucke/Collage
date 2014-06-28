using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using drawing = System.Drawing;

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

        // Convert Colors and Vectors
        public static Vector4 ToVector(Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

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
    }
}
