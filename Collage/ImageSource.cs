using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Collage
{
    public class ImageSource
    {
        DataAccess dataAccess;
        Texture2D texture, emptyTexture;
        int width = 100;
        int height = 60;

        public ImageSource(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            emptyTexture = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            emptyTexture.SetData<Color>(new Color[] { Color.White });
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
        {
            ImageLoader loader = new ImageLoader(dataAccess, fileName);
            texture = loader.Load();
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
