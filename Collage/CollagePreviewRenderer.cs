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
                // specify rotation origin
                Vector2 origin = new Vector2(image.Source.Width / 2f, image.Source.Height / 2f);
                // calculate rectangle where the image will be drawn
                Rectangle imageRectangle = image.GetRectangleInBoundary(drawRectangle);
                imageRectangle.X += imageRectangle.Width / 2;
                imageRectangle.Y += imageRectangle.Height / 2;

                dataAccess.SpriteBatch.Draw(image.Texture, imageRectangle, null, color, image.Rotation, origin, SpriteEffects.None, 0);
            }

            dataAccess.SpriteBatch.End();
        }
    }
}
