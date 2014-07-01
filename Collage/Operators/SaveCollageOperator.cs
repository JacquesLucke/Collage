﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Collage
{
    class SaveCollageOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        SaveFileWindow sfw;
        Texture2D tex;
        ProgressBarWindow progressBar;

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
            dataAccess.GuiThread.Invoke(OpenFileBrowser);
            return true;
        }
        public void OpenFileBrowser()
        {
            sfw = new SaveFileWindow(dataAccess);
            sfw.OpenDialog(FileTypes.Images);
        }
        public void StartProgressBar()
        {
            progressBar = new ProgressBarWindow();
            progressBar.Start();
            progressBar.Name = "Save Collage";
        }

        public bool Update()
        {
            bool isPathChoosed = !dataAccess.GuiThread.IsBlockedByDialog;

            if (isPathChoosed)
            {
                string fileName = sfw.SelectedPath;
                sfw.Destroy();
                if (fileName != null)
                {
                    // calculate final dimensions
                    int width = 4000;
                    int height = (int)Math.Round(width / editData.Collage.AspectRatio);
                    Rectangle dimensions = new Rectangle(0, 0, width, height);

                    Texture2D render = Render(dimensions, width, height);

                    System.Drawing.Bitmap bitmap = Utils.ToBitmap(render);
                    bitmap.Save(fileName);
                    bitmap.Dispose();
                    render.Dispose();
                    GC.Collect();
                }
            }
            return !isPathChoosed;
        }

        public void DrawImageSource(ImageSource source, Rectangle rectangle, float rotation)
        {
            Texture2D texture = source.Texture;
            if (source.FileName != "") texture = source.GetBigVersion(Math.Max(rectangle.Width, rectangle.Height));
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            rectangle.X += rectangle.Width / 2;
            rectangle.Y += rectangle.Height / 2;
            dataAccess.SpriteBatch.Draw(texture, rectangle, null, Color.White, rotation, origin, SpriteEffects.None, 0);
            if (source.FileName != "")
            {
                texture.Dispose();
                texture = null;
                GC.Collect();
            }
        }

        private Texture2D Render(Rectangle part, int totalWidth, int totalHeight)
        {
            dataAccess.GuiThread.Invoke(StartProgressBar);
            while (progressBar == null) ;
            progressBar.TotalSteps = editData.Collage.Images.Count;

            // create and set RenderTarget
            RenderTarget2D rt = new RenderTarget2D(dataAccess.GraphicsDevice, part.Width, part.Height);
            dataAccess.GraphicsDevice.SetRenderTarget(rt);

            Rectangle capureRec = new Rectangle(0, 0, part.Width, part.Height);
            Rectangle totalRec = new Rectangle(-part.X, -part.Y, totalWidth, totalHeight);

            dataAccess.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            // -----------------------------------------------------------------------------------------

            // draw background color
            dataAccess.SpriteBatch.Draw(tex, capureRec, editData.Collage.BackgroundColor);

            // draw each image
            foreach (Image image in editData.Collage.Images)
            {
                // calculate rectangle where the image will be drawn
                Rectangle imageRectangle = image.GetRectangleInBoundary(totalRec);
                DrawImageSource(image.Source, imageRectangle, image.Rotation);
                progressBar.StepUp();
            }

            // -----------------------------------------------------------------------------------------
            dataAccess.SpriteBatch.End();

            // reset RenderTarget
            dataAccess.GraphicsDevice.SetRenderTarget(null);
            progressBar.Destroy();

            return rt;
        }
    }
}
