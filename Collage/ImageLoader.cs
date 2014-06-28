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

                smallBitmap = new Bitmap(bitmap, newSize);
                bitmap.Dispose();
                bitmap = null;
            }

            if (bitmap == null)
            {
                texture = Utils.ToTexture(smallBitmap, dataAccess.GraphicsDevice); 
                smallBitmap.Dispose();
            }
            else
            {
                texture = Utils.ToTexture(bitmap, dataAccess.GraphicsDevice);
                bitmap.Dispose();
            }

            GC.Collect();
            return texture;
        }
    }
}
