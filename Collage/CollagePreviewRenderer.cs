using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class CollagePreviewRenderer
    {
        CollageObject collage;
        DataAccess dataAccess;
        Texture2D tex;

        public CollagePreviewRenderer(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            tex = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            tex.SetData<Color>(new Color[] { Color.White });
        }

        public void SetCollage(CollageObject collage)
        {
            this.collage = collage;
        }

        public void Draw(Rectangle rectangle)
        {
            dataAccess.SpriteBatch.Begin();
            dataAccess.SpriteBatch.Draw(tex, rectangle, collage.BackgroundColor);

            foreach(Image image in collage.Images)
            {
                Rectangle imageRectangle = image.GetRectangleInBoundary(rectangle);
                dataAccess.SpriteBatch.Draw(image.Texture, imageRectangle, Color.White);
            }

            dataAccess.SpriteBatch.End();
        }
    }
}
