using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                if (editData.SelectedImages.Contains(image)) color = Utils.MultiplyColors(color, Color.Red);
                if (editData.ImageUnderMouse == image) color = Utils.MultiplyColors(color, Color.FromNonPremultiplied(220, 220, 220, 255));
                // calculate rectangle where the image will be drawn
                Rectangle imageRectangle = image.GetRectangleInBoundary(drawRectangle);

                DrawImageSource(image.Source, imageRectangle, image.Rotation, color);
            }

            dataAccess.SpriteBatch.End();
        }

        public void DrawImageSource(ImageSource source, Rectangle rectangle, float rotation, Color color)
        {
            Vector2 origin = new Vector2(source.Texture.Width / 2f, source.Texture.Height / 2f);
            rectangle.X += rectangle.Width / 2;
            rectangle.Y += rectangle.Height / 2;
            dataAccess.SpriteBatch.Draw(source.Texture, rectangle, null, color, rotation, origin, SpriteEffects.None, 0);
        }
    }
}
