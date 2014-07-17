using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

            imageEffect = dataAccess.Content.GetEffect("image effect");
            dropShadowEffect = dataAccess.Content.GetEffect("drop shadow effect");
        }

        public void SetEditData(CollageEditData editData)
        {
            this.editData = editData;
        }

        public void Draw()
        {
            Rectangle boundary = editData.DrawRectangle.Rectangle;
            SetTransformMatrixForEffects();

            dataAccess.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            DrawBackground(boundary);
            foreach(Image image in editData.Collage.Images)
                DrawImageWithDropShadow(image, boundary);
            dataAccess.SpriteBatch.End();

            border.Draw(dataAccess.SpriteBatch, boundary);
        }
        private void DrawBackground(Rectangle boundary)
        {
            dataAccess.SpriteBatch.Draw(tex, boundary, editData.Collage.BackgroundColor);
        }
        private void SetTransformMatrixForEffects()
        {
            Matrix transformMatrix = GetTransformMatrix();
            imageEffect.Parameters["MatrixTransform"].SetValue(transformMatrix);
            dropShadowEffect.Parameters["MatrixTransform"].SetValue(transformMatrix);
        }
        private void DrawImageWithDropShadow(Image image, Rectangle boundary)
        {
            Rectangle imageRectangle = GetImageRectangle(image, boundary);
            Rectangle dropShadowRectangle = GetDropShadowRectangle(imageRectangle, 0.1f);

            SetupAndApplyDropShadowEffect(imageRectangle, dropShadowRectangle, 80f);
            DrawImageSource(image.Source, dropShadowRectangle, image.Rotation);

            Color color = CalculateColorOverlay(image);
            SetupAndApplyImageEffect(image, imageRectangle, color);
            DrawImageSource(image.Source, imageRectangle, image.Rotation);
        }
        private Rectangle GetImageRectangle(Image image, Rectangle boundary) 
        {
            return image.GetRectangleInBoundary(boundary); 
        }
        private Rectangle GetDropShadowRectangle(Rectangle imageRectangle, float shadowSize) 
        { 
            return Utils.ExpandRectangle(imageRectangle, (int)Math.Round(imageRectangle.Width * shadowSize));
        }
        private void SetupAndApplyDropShadowEffect(Rectangle imageRectangle, Rectangle dropShadowRectangle, float intensity)
        {
            dropShadowEffect.Parameters["AspectRatio"].SetValue(dropShadowRectangle.Width / (float)dropShadowRectangle.Height);
            dropShadowEffect.Parameters["Intense"].SetValue(intensity);
            dropShadowEffect.Parameters["BorderRadius"].SetValue((dropShadowRectangle.Width - imageRectangle.Width) / (float)dropShadowRectangle.Width / 2f);
            dropShadowEffect.CurrentTechnique.Passes[0].Apply();
        }
        private Color CalculateColorOverlay(Image image)
        {
            Color color = Color.White;
            if (editData.SelectedImages.Contains(image)) color = Utils.MultiplyColors(color, Color.Red);
            if (editData.ImageUnderMouse == image) color = Utils.MultiplyColors(color, Color.FromNonPremultiplied(220, 220, 220, 255));
            return color;
        }
        private void SetupAndApplyImageEffect(Image image, Rectangle imageRectangle, Color color)
        {
            imageEffect.Parameters["Size"].SetValue((float)imageRectangle.Width);
            imageEffect.Parameters["AspectRatio"].SetValue(image.Source.AspectRatio);
            imageEffect.Parameters["ColorMultiply"].SetValue(Utils.ToVector(color));
            imageEffect.CurrentTechnique.Passes[0].Apply();
        }
        private void DrawImageSource(ImageSource source, Rectangle rectangle, float rotation)
        {
            Vector2 origin = new Vector2(source.Texture.Width / 2f, source.Texture.Height / 2f);
            rectangle.X += rectangle.Width / 2;
            rectangle.Y += rectangle.Height / 2;
            dataAccess.SpriteBatch.Draw(source.Texture, rectangle, null, Color.White, rotation, origin, SpriteEffects.None, 0);
        }

        private Matrix GetTransformMatrix()
        {
            Viewport viewport = dataAccess.GraphicsDevice.Viewport;
            return Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
        }
    }
}
