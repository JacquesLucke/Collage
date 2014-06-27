using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Collage
{
    public class ImageLoader
    {
        string fileName = "";
        DataAccess dataAccess;

        public ImageLoader(DataAccess dataAccess, string fileName)
        {
            this.fileName = fileName;
            this.dataAccess = dataAccess;
        }

        public Texture2D Load()
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(dataAccess.GraphicsDevice, fs);
            fs.Close();
            return texture;
        }

        public Texture2D Load(int maxSize)
        {
            Bitmap bitmap = new Bitmap(fileName);
            Size newSize;

            float aspectRatio = (float)bitmap.Width / (float)bitmap.Height;
            if(aspectRatio > 1) newSize = new Size(maxSize, (int)Math.Round(maxSize / aspectRatio));
            else newSize = new Size((int)Math.Round(maxSize * aspectRatio), maxSize);

            Bitmap smallBitmap = new Bitmap(bitmap, newSize);
            bitmap.Dispose();

            Texture2D texture = ConvertToTexture(smallBitmap);
            return texture;
        }

        public Texture2D ConvertToTexture(Bitmap bitmap)
        {
            Texture2D texture;
            if (bitmap == null)
            {
                return null;
            }
            texture = new Texture2D(dataAccess.GraphicsDevice, bitmap.Width, bitmap.Height);

            // MemoryStream to store the bitmap data.
            MemoryStream ms = new MemoryStream();
            // Save image to MemoryStream
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            //Go to the beginning of the memory stream.
            ms.Seek(0, SeekOrigin.Begin);

            //Fill the texture.
            texture = Texture2D.FromStream(dataAccess.GraphicsDevice, ms);

            ms.Close();
            ms = null;

            return texture;
        }
    }
}
