using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Collage
{
    public class Border
    {
        public Color color;
        private Texture2D background;
        private GraphicsDevice graphicsDevice;

        public Border(GraphicsDevice graphicsDevice, Color color)
        {
            this.graphicsDevice = graphicsDevice;
            this.color = color;

            // creates a 1 pixel texture
            background = new Texture2D(graphicsDevice, 1, 1);
            background.SetData<Color>(new Color[1] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle innerRectangle)
        {
            #region calculate border rectangles
            Viewport viewport = graphicsDevice.Viewport;

            Rectangle top = new Rectangle(0, 0, viewport.Width, Math.Max(0, innerRectangle.Y));
            Rectangle bottom = new Rectangle(0, Math.Min(viewport.Height, innerRectangle.Bottom), viewport.Width, Math.Max(0, viewport.Height - innerRectangle.Bottom));
            Rectangle left = new Rectangle(0, Math.Max(0, innerRectangle.Y), Math.Max(0, innerRectangle.X), Math.Min(innerRectangle.Bottom, innerRectangle.Height));
            Rectangle right = new Rectangle(Math.Min(innerRectangle.Right, viewport.Width), innerRectangle.Y, Math.Max(0, viewport.Width - innerRectangle.Right), innerRectangle.Height);
            #endregion

            #region draw rectangles
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            spriteBatch.Draw(background, top, (Color)color);
            spriteBatch.Draw(background, bottom, (Color)color);
            spriteBatch.Draw(background, left, (Color)color);
            spriteBatch.Draw(background, right, (Color)color);
            spriteBatch.End();
            #endregion
        }
    }
}
