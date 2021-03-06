﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Collage
{
    public class ImageSource
    {
        GraphicsDevice graphicsDevice;
        Texture2D texture, emptyTexture;
        int width = 100;
        int height = 60;
        string fileName = "";

        public ImageSource(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            emptyTexture = new Texture2D(graphicsDevice, 1, 1);
            emptyTexture.SetData<Color>(new Color[] { Color.Tomato });
        }
        public ImageSource(GraphicsDevice graphicsDevice, int width, int height)
            : this(graphicsDevice)
        {
            this.width = width;
            this.height = height;
        }
        public ImageSource(GraphicsDevice graphicsDevice, int width, int height, Color color)
            : this(graphicsDevice, width, height)
        {
            emptyTexture.SetData<Color>(new Color[] { color });
        }

        public ImageSource(GraphicsDevice graphicsDevice, string fileName)
            :this(graphicsDevice)
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
        public void LoadProxy()
        {
            if (texture == null)
            {
                ImageLoader loader = new ImageLoader(graphicsDevice, fileName, 170);
                texture = loader.Load();
            }
        }
        public void Load()
        {
            if (texture == null)
            {
                ImageLoader loader = new ImageLoader(graphicsDevice, fileName, 0);
                texture = loader.Load();
            }
        }

        public Texture2D GetBigVersion(int maxSize)
        {
            ImageLoader loader = new ImageLoader(graphicsDevice, fileName, maxSize);
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
