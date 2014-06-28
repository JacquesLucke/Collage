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
            Load();
        }

        public void Unload()
        {
            texture.Dispose();
            texture = null;
            GC.Collect();
        }
        public void Load()
        {
            if (texture == null)
            {
                ImageLoader loader = new ImageLoader(dataAccess, fileName, 170);
                loader.LoadAsync(LoadCallback);
            }
        }

        public void LoadCallback(Texture2D tex) { texture = tex; }

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
