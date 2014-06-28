using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace Collage
{
    public delegate void ImageLoaded(Texture2D texture);
    public class ImageLoader
    {
        string fileName = "";
        DataAccess dataAccess;

        Texture2D texture;
        int maxSize = 0;
        ImageLoaded callBack;

        private static object lockLoading = new object();

        public ImageLoader(DataAccess dataAccess, string fileName, int maxSize)
        {
            this.fileName = fileName;
            this.dataAccess = dataAccess;
            this.maxSize = maxSize;
        }

        public Texture2D Load()
        {
            LoadingThread();
            return texture;
        }

        public void LoadingThread()
        {
            lock (lockLoading)
            {
                Bitmap bitmap = new Bitmap(fileName);
                Bitmap smallBitmap = null;

                if (maxSize != 0)
                {
                    Size newSize;

                    float aspectRatio = (float)bitmap.Width / (float)bitmap.Height;
                    if (aspectRatio > 1) newSize = new Size(maxSize, (int)Math.Round(maxSize / aspectRatio));
                    else newSize = new Size((int)Math.Round(maxSize * aspectRatio), maxSize);

                    smallBitmap = new Bitmap(bitmap, newSize);
                    bitmap.Dispose();
                    bitmap = null;
                }

                if (bitmap == null) { texture = ConvertToTexture(smallBitmap); smallBitmap.Dispose(); }
                else { texture = ConvertToTexture(bitmap); bitmap.Dispose(); }
                callBack(texture);
                GC.Collect();
            }
        }

        public void LoadAsync(ImageLoaded callBack)
        {
            this.callBack = callBack;
            Thread thread = new Thread(LoadingThread);
            thread.Start();
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
            ms.Dispose();
            ms = null;

            return texture;
        }
    }
}
