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
        Effect dropShadowEffect;

        public CollagePreviewRenderer(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            tex = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            tex.SetData<Color>(new Color[] { Color.White });

            border = new Border(dataAccess.GraphicsDevice, Color.FromNonPremultiplied(182, 195, 205, 200));

            // load the image effect
            BinaryReader br = new BinaryReader(new FileStream("Content\\ImageEffect.mgfx", FileMode.Open));
            imageEffect = new Effect(dataAccess.GraphicsDevice, br.ReadBytes((int)br.BaseStream.Length));
            br.Close();

            // load the drop shadow effect
            br = new BinaryReader(new FileStream("Content\\DropShadow.mgfx", FileMode.Open));
            dropShadowEffect = new Effect(dataAccess.GraphicsDevice, br.ReadBytes((int)br.BaseStream.Length));
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
            Matrix transformMatrix = GetTransformMatrix();
            imageEffect.Parameters["MatrixTransform"].SetValue(transformMatrix);
            dropShadowEffect.Parameters["MatrixTransform"].SetValue(transformMatrix);

            foreach(Image image in editData.Collage.Images)
            {
                // calculate rectangles where to draw the image and drop shadow
                Rectangle imageRectangle = image.GetRectangleInBoundary(drawRectangle);
                Rectangle dropShadowRectangle = Utils.ExpandRectangle(imageRectangle, imageRectangle.Width / 10);
                
                // setup the drop shadow effect
                dropShadowEffect.Parameters["AspectRatio"].SetValue(dropShadowRectangle.Width / (float)dropShadowRectangle.Height);
                dropShadowEffect.Parameters["Intense"].SetValue(30f);
                dropShadowEffect.CurrentTechnique.Passes[0].Apply();
                DrawImageSource(image.Source, dropShadowRectangle, image.Rotation, Color.Black);

                // calculate color overlay for selection or mouse over
                Color color = Color.White;
                if (editData.SelectedImages.Contains(image)) color = Utils.MultiplyColors(color, Color.Red);
                if (editData.ImageUnderMouse == image) color = Utils.MultiplyColors(color, Color.FromNonPremultiplied(220, 220, 220, 255));

                // setup the image effect
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
