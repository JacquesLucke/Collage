using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Collage
{
    class SaveCollageOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        Texture2D tex;

        public SaveCollageOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;

            tex = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            tex.SetData<Color>(new Color[] { Color.White });
        }
        public bool CanStart()
        {
            return dataAccess.Keymap["save collage"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            int width = 2000;
            int height = (int)Math.Round(width / editData.Collage.AspectRatio);
            RenderTarget2D rt = new RenderTarget2D(dataAccess.GraphicsDevice, width, height);
            dataAccess.GraphicsDevice.SetRenderTarget(rt);

            Rectangle drawRectangle = new Rectangle(0, 0, width, height);
            dataAccess.SpriteBatch.Begin();
            dataAccess.SpriteBatch.Draw(tex, drawRectangle, editData.Collage.BackgroundColor);

            foreach (Image image in editData.Collage.Images)
            {
                Color color = Color.White;
                if (editData.SelectedImages.Contains(image)) color = Color.Red;
                // calculate rectangle where the image will be drawn
                Rectangle imageRectangle = image.GetRectangleInBoundary(drawRectangle);

                DrawImageSource(image.Source, imageRectangle, image.Rotation, color);
            }

            dataAccess.SpriteBatch.End();

            dataAccess.GraphicsDevice.SetRenderTarget(null);

            SaveFileWindow sf = new SaveFileWindow();
            string fileName = sf.SaveFile(FileTypes.JPG, FileTypes.PNG);
            if(fileName != null)
            {
                System.Drawing.Bitmap bitmap = Utils.Texture2Bitmap(rt);
                bitmap.Save(fileName);
            }
            return false;
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
