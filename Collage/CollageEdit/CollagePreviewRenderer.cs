using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Collage
{
    public class CollagePreviewRenderer
    {
        CollageEditData editData;
        DataAccess dataAccess;
        Texture2D tex;
        Border border;
        Effect imageEffect;

        public CollagePreviewRenderer(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            tex = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            tex.SetData<Color>(new Color[] { Color.White });

            border = new Border(dataAccess.GraphicsDevice, Color.FromNonPremultiplied(182, 195, 205, 200));

            // load the effect
            BinaryReader br = new BinaryReader(new FileStream("Content\\ImageEffect.mgfx", FileMode.Open));
            imageEffect = new Effect(dataAccess.GraphicsDevice, br.ReadBytes((int)br.BaseStream.Length));
            br.Close();
        }

        public void SetEditData(CollageEditData editData)
        {
            this.editData = editData;
        }

        public void Draw()
        {
            Rectangle drawRectangle = editData.DrawRectangle.Rectangle;
            dataAccess.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            dataAccess.SpriteBatch.Draw(tex, drawRectangle, editData.Collage.BackgroundColor);

            // setup viewport matrix
            imageEffect.Parameters["MatrixTransform"].SetValue(GetTransformMatrix());

            foreach(Image image in editData.Collage.Images)
            {
                Color color = Color.White;
                if (editData.SelectedImages.Contains(image)) color = Utils.MultiplyColors(color, Color.Red);
                if (editData.ImageUnderMouse == image) color = Utils.MultiplyColors(color, Color.FromNonPremultiplied(220, 220, 220, 255));
                // calculate rectangle where the image will be drawn
                Rectangle imageRectangle = image.GetRectangleInBoundary(drawRectangle);

                // setup the effect
                imageEffect.Parameters["Size"].SetValue((float)imageRectangle.Width);
                imageEffect.Parameters["AspectRatio"].SetValue(image.Source.AspectRatio);
                imageEffect.Parameters["ColorMultiply"].SetValue(Utils.ToVector(color));
                imageEffect.CurrentTechnique.Passes[0].Apply();

                DrawImageSource(image.Source, imageRectangle, image.Rotation, color);
            }

            dataAccess.SpriteBatch.End();

            border.Draw(dataAccess.SpriteBatch, drawRectangle);
        }

        public void DrawImageSource(ImageSource source, Rectangle rectangle, float rotation, Color color)
        {
            Vector2 origin = new Vector2(source.Texture.Width / 2f, source.Texture.Height / 2f);
            rectangle.X += rectangle.Width / 2;
            rectangle.Y += rectangle.Height / 2;
            dataAccess.SpriteBatch.Draw(source.Texture, rectangle, null, color, rotation, origin, SpriteEffects.None, 0);
        }

        private Matrix GetTransformMatrix()
        {
            Viewport viewport = dataAccess.GraphicsDevice.Viewport;
            return Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
        }
    }
}
