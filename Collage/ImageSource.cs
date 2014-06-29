using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Collage
{
    public class ImageSource
    {
        DataAccess dataAccess;
        Texture2D texture, emptyTexture;
        int width = 100;
        int height = 60;
        string fileName = "";

        public ImageSource(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            emptyTexture = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            emptyTexture.SetData<Color>(new Color[] { Color.Tomato });
        }
        public ImageSource(DataAccess dataAccess, int width, int height)
            : this(dataAccess)
        {
            this.width = width;
            this.height = height;
        }
        public ImageSource(DataAccess dataAccess, int width, int height, Color color)
            : this(dataAccess, width, height)
        {
            emptyTexture.SetData<Color>(new Color[] { color });
        }

        public ImageSource(DataAccess dataAccess, string fileName)
            :this(dataAccess)
        {
            this.fileName = fileName;
        }

        public void Unload()
        {
            if (texture != null)
            {
                texture.Dispose();
                texture = null;
                GC.Collect();
            }
        }
        public void Load()
        {
            if (texture == null)
            {
                ImageLoader loader = new ImageLoader(dataAccess, fileName, 170);
                texture = loader.Load();
            }
        }

        public Texture2D GetBigVersion(int maxSize)
        {
            ImageLoader loader = new ImageLoader(dataAccess, fileName, maxSize);
            return loader.Load();
        }

        public int Width
        {
            get
            {
                if (texture == null) return width;
                else return texture.Width;
            }
        }
        public int Height
        {
            get
            {
                if (texture == null) return height;
                else return texture.Height;
            }
        }
        public float AspectRatio
        {
            get
            {
                return (float)Width / (float)Height;
            }
        }
        public string FileName
        {
            get { return fileName; }
        }

        public Texture2D Texture
        {
            get
            {
                if (texture == null) return emptyTexture;
                else return texture;
            }
        }
    }
}
