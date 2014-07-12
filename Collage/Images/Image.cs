using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collage
{
    public class Image
    {
        ImageSource source;
        ImageData data;

        public Image(DataAccess dataAccess, string fileName)
        {
            this.source = new ImageSource(dataAccess, fileName);
            SetDefaultData();
        }
        public Image(ImageSource source)
        {
            this.source = source;
            SetDefaultData();
        }
        public Image(ImageSource source, Vector2 center)
            : this(source)
        {
            data.Center = center;
        }

        public void SetDefaultData()
        {
            data = new ImageData();
            data.Center = new Vector2(0.5f);
            data.Width = 0.2f;
            data.Rotation = 0f;
        }

        public void Unload()
        {
            source.Unload();
        }
        public void Reload()
        {
            source.LoadProxy();
        }

        public float Width
        {
            get { return data.Width; }
            set { data.Width = value; }
        }
        public float Height
        {
            get { return data.Width / source.AspectRatio; }
            set { data.Width = value * source.AspectRatio; }
        }
        public float Rotation
        {
            get { return data.Rotation; }
            set { data.Rotation = value; }
        }
        public Vector2 Center
        {
            get { return data.Center; }
            set { data.Center = value; }
        }
        public ImageSource Source
        {
            get { return source; }
        }
        public ImageData Data
        {
            get { return data; }
            set { data = value; }
        }

        public Rectangle GetRectangleInBoundary(Rectangle boundary)
        {
            Vector2 realCenter = GetCenterInBoundary(boundary);
            float realWidth = boundary.Width * Width;
            float realHeight = boundary.Height * Height * ((float)boundary.Width / (float)boundary.Height);
            FloatRectangle realRectangle = new FloatRectangle(realCenter.X - realWidth / 2f, realCenter.Y - realHeight / 2f, realWidth, realHeight);
            return realRectangle.ToRectangle();
        }
        public Vector2 GetCenterInBoundary(Rectangle boundary)
        {
            return new Vector2(boundary.Left + data.Center.X * boundary.Width, boundary.Top + data.Center.Y * boundary.Height);
        }
        public void SetCenterInBoundary(Rectangle boundary, Vector2 center)
        {
            this.data.Center = new Vector2((center.X - boundary.Left) / (float)boundary.Width, (center.Y - boundary.Top) / (float)boundary.Height);
        }

        public Texture2D Texture { get { return source.Texture; } }
    }
}
