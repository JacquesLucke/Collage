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
        CollageEditData editData;
        DataAccess dataAccess;
        Texture2D tex;

        public CollagePreviewRenderer(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            tex = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            tex.SetData<Color>(new Color[] { Color.White });
        }

        public void SetEditData(CollageEditData editData)
        {
            this.editData = editData;
        }

        public void Draw()
        {
            Rectangle drawRectangle = editData.DrawRectangle.Rectangle;
            dataAccess.SpriteBatch.Begin();
            dataAccess.SpriteBatch.Draw(tex, drawRectangle, editData.Collage.BackgroundColor);

            foreach(Image image in editData.Collage.Images)
            {
                Color color = Color.White;
                if (editData.SelectedImages.Contains(image)) color = Color.Red;
                Rectangle imageRectangle = image.GetRectangleInBoundary(drawRectangle);
                dataAccess.SpriteBatch.Draw(image.Texture, imageRectangle, color);
            }

            dataAccess.SpriteBatch.End();
        }
    }
}
