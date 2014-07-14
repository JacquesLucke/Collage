using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Collage
{
    class SaveCollageOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        SaveFileWindow sfw;
        Texture2D tex;
        ProgressBarWindow progressBar;
        DimensionsDialog dimensionsDialog;
        Effect imageEffect, dropShadowEffect;
        int step = 1;
        int width, height;
        string fileName;

        public SaveCollageOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;

            tex = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            tex.SetData<Color>(new Color[] { Color.White });

            // load the image effect
            BinaryReader br = new BinaryReader(new FileStream("Content\\Effects\\ImageEffect.mgfx", FileMode.Open));
            imageEffect = new Effect(dataAccess.GraphicsDevice, br.ReadBytes((int)br.BaseStream.Length));
            br.Close();

            // load the drop shadow effect
            br = new BinaryReader(new FileStream("Content\\Effects\\DropShadow.mgfx", FileMode.Open));
            dropShadowEffect = new Effect(dataAccess.GraphicsDevice, br.ReadBytes((int)br.BaseStream.Length));
            br.Close();
        }

        public bool Start()
        {
            step = 1;
            fileName = null;
            return true;
        }
        public void OpenFileBrowser()
        {
            sfw = new SaveFileWindow();
            sfw.OpenDialog(FileTypes.JPG, FileTypes.PNG, FileTypes.BMP);
        }
        public void OpenDimensionsDialog()
        {
            dimensionsDialog = new DimensionsDialog();
            dimensionsDialog.SetInputRange(500, 6000);
            dimensionsDialog.SetData(3000, editData.Collage.AspectRatio);
            dimensionsDialog.Start();
        }
        public void StartProgressBar()
        {
            progressBar = new ProgressBarWindow();
            progressBar.Start();
            progressBar.Name = "Save Collage";
        }

        public bool Update()
        {
            // adjust the size
            if(step == 1)
            {
                if (dimensionsDialog == null && !dataAccess.GuiThread.WaitsToInvoke) dataAccess.GuiThread.Invoke(OpenDimensionsDialog);

                if (!dataAccess.GuiThread.WaitsToInvoke)
                {
                    if (dimensionsDialog.Response == Gtk.ResponseType.Cancel) { 
                        dimensionsDialog.Destroy(); 
                        dimensionsDialog = null; 
                        return false; 
                    }
                    if (dimensionsDialog.Response == Gtk.ResponseType.Ok)
                    {
                        width = dimensionsDialog.InputWidth;
                        height = dimensionsDialog.InputHeight;
                        dimensionsDialog.Destroy();
                        dimensionsDialog = null;
                        step = 2;
                    }
                }
            }
            // select a path where to save
            if(step == 2)
            {
                if(sfw == null && !dataAccess.GuiThread.WaitsToInvoke) dataAccess.GuiThread.Invoke(OpenFileBrowser);

                if(!dataAccess.GuiThread.IsBlockedByDialog)
                {
                    fileName = sfw.SelectedPath;
                    sfw.Destroy();
                    sfw = null;
                    if (fileName == null) return false;
                    step = 3;
                }
            }
            // render the image and size
            if (step == 3)
            {
                // setup progress bar
                dataAccess.GuiThread.Invoke(StartProgressBar);
                while (progressBar == null) ; // wait until the bar is setup
                progressBar.TotalSteps = editData.Collage.Images.Count + 2;

                Rectangle dimensions = new Rectangle(0, 0, width, height);
                Texture2D render = Render(dimensions, width, height);

                // correct alpha
                progressBar.StepUp("correct alpha");
                Color[] colors = new Color[width * height];
                render.GetData<Color>(colors);
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i].A = 255;
                }
                render.SetData<Color>(colors);
                colors = null;
                GC.Collect();

                System.Drawing.Bitmap bitmap = Utils.ToBitmap(render);
                progressBar.StepUp("Save");
                bitmap.Save(fileName);
                bitmap.Dispose();
                render.Dispose();
                GC.Collect();

                progressBar.Destroy();
                step = 4;
            }
            return step < 4;
        }

        public void DrawImageSource(ImageSource source, Rectangle rectangle, float rotation, bool loadFullSize)
        {
            Texture2D texture = source.Texture;
            if (source.FileName != "" && loadFullSize) texture = source.GetBigVersion(Math.Max(rectangle.Width, rectangle.Height));
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            rectangle.X += rectangle.Width / 2;
            rectangle.Y += rectangle.Height / 2;
            dataAccess.SpriteBatch.Draw(texture, rectangle, null, Color.White, rotation, origin, SpriteEffects.None, 0);
            if (source.FileName != "" && loadFullSize)
            {
                texture.Dispose();
                texture = null;
                GC.Collect();
            }
        }

        private Texture2D Render(Rectangle part, int totalWidth, int totalHeight)
        {
            // create and set RenderTarget
            RenderTarget2D rt = new RenderTarget2D(dataAccess.GraphicsDevice, part.Width, part.Height);
            dataAccess.GraphicsDevice.SetRenderTarget(rt);
            dataAccess.GraphicsDevice.Clear(editData.Collage.BackgroundColor);

            Rectangle capureRec = new Rectangle(0, 0, part.Width, part.Height);
            Rectangle totalRec = new Rectangle(-part.X, -part.Y, totalWidth, totalHeight);

            dataAccess.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            // -----------------------------------------------------------------------------------------

            // setup viewport matrix
            Matrix transformMatrix = GetTransformMatrix();
            imageEffect.Parameters["MatrixTransform"].SetValue(transformMatrix);
            dropShadowEffect.Parameters["MatrixTransform"].SetValue(transformMatrix);
            
            // draw each image
            foreach (Image image in editData.Collage.Images)
            {
                // calculate rectangles where to draw the image and drop shadow
                Rectangle imageRectangle = image.GetRectangleInBoundary(totalRec);
                Rectangle dropShadowRectangle = Utils.ExpandRectangle(imageRectangle, imageRectangle.Width / 10);

                // setup the drop shadow effect
                dropShadowEffect.Parameters["AspectRatio"].SetValue(dropShadowRectangle.Width / (float)dropShadowRectangle.Height);
                dropShadowEffect.Parameters["Intense"].SetValue(80f);
                dropShadowEffect.Parameters["BorderRadius"].SetValue((dropShadowRectangle.Width - imageRectangle.Width) / (float)dropShadowRectangle.Width / 2f);
                dropShadowEffect.CurrentTechnique.Passes[0].Apply();
                DrawImageSource(image.Source, dropShadowRectangle, image.Rotation, false);

                // setup the image effect
                imageEffect.Parameters["Size"].SetValue((float)imageRectangle.Width);
                imageEffect.Parameters["AspectRatio"].SetValue(image.Source.AspectRatio);
                imageEffect.Parameters["ColorMultiply"].SetValue(new Vector4(1));
                imageEffect.CurrentTechnique.Passes[0].Apply();
                DrawImageSource(image.Source, imageRectangle, image.Rotation, true);
               
                progressBar.StepUp();
            }

            // -----------------------------------------------------------------------------------------
            dataAccess.SpriteBatch.End();

            // reset RenderTarget
            dataAccess.GraphicsDevice.SetRenderTarget(null);

            return rt;
        }

        private Matrix GetTransformMatrix()
        {
            Viewport viewport = dataAccess.GraphicsDevice.Viewport;
            return Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
        }
    }
}
