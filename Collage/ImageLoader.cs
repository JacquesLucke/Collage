using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.IO;

namespace Collage
{
    public class ImageLoader
    {
        string fileName = "";
        DataAccess dataAccess;

        Texture2D texture;
        int maxSize = 0;

        public ImageLoader(DataAccess dataAccess, string fileName, int maxSize)
        {
            this.fileName = fileName;
            this.dataAccess = dataAccess;
            this.maxSize = maxSize;
        }

        public Texture2D Load()
        {
            Bitmap bitmap = new Bitmap(fileName);
            Bitmap smallBitmap = null;

            if (maxSize != 0)
            {
                // make the image smaller to need less RAM
                Size newSize;

                float aspectRatio = (float)bitmap.Width / (float)bitmap.Height;
                if (aspectRatio > 1) newSize = new Size(maxSize, (int)Math.Round(maxSize / aspectRatio));
                else newSize = new Size((int)Math.Round(maxSize * aspectRatio), maxSize);

                smallBitmap = new Bitmap(bitmap.GetThumbnailImage(newSize.Width, newSize.Height, null, IntPtr.Zero));
                bitmap.Dispose();
                bitmap = null;
            }

            if (bitmap == null)
            {
                texture = ConvertToTexture(smallBitmap); 
                smallBitmap.Dispose();
            }
            else
            {
                texture = ConvertToTexture(bitmap);
                bitmap.Dispose();
            }

            GC.Collect();
            return texture;
        }

        public Texture2D ConvertToTexture(Bitmap bitmap)
        {
            Texture2D texture;
            if (bitmap == null)
            {
                return null;
            }

            // MemoryStream to store the bitmap data.
            MemoryStream ms = new MemoryStream();
            // Save image to MemoryStream
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            //Go to the beginning of the memory stream.
            ms.Seek(0, SeekOrigin.Begin);

            //Fill the texture.
            texture = Texture2D.FromStream(dataAccess.GraphicsDevice, ms);

            ms.Close();
            ms.Dispose();
            ms = null;

            return texture;
        }
    }
}
