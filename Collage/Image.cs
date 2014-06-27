using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class Image
    {
        Vector2 center = new Vector2(0.5f);
        ImageSource source;
        float width = 0.2f;

        public Image(DataAccess dataAccess, string fileName)
        {
            this.source = new ImageSource(dataAccess, fileName);
        }

        public Image(ImageSource source)
        {
            this.source = source;
        }
        public Image(ImageSource source, Vector2 center)
            : this(source)
        {
            this.center = center;
        }

        public void Unload()
        {
            source.Unload();
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        public float Height
        {
            get { return width / source.AspectRatio; }
            set { width = value * source.AspectRatio; }
        }

        public Rectangle GetRectangleInBoundary(Rectangle boundary)
        {
            Vector2 realCenter = new Vector2(boundary.Left + center.X * boundary.Width, boundary.Top + center.Y * boundary.Height);
            float realWidth = boundary.Width * Width;
            float realHeight = boundary.Height * Height * ((float)boundary.Width / (float)boundary.Height);
            FloatRectangle realRectangle = new FloatRectangle(realCenter.X - realWidth / 2f, realCenter.Y - realHeight / 2f, realWidth, realHeight);
            return realRectangle.ToRectangle();
        }

        public Texture2D Texture { get { return source.Texture; } }
    }
}
