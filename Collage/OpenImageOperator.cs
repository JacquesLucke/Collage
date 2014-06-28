using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class OpenImageOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public OpenImageOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Keymap["open image"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            // open a dialog to choose the files
            OpenFileWindow of = new OpenFileWindow();
            string[] fileNames = of.OpenFiles(FileTypes.Images);
            if(fileNames != null)
            {
                // make a list of all new images
                List<Image> images = new List<Image>();
                for (int i = 0; i < fileNames.Length; i++)
                {
                    // check if the image needs a new Source or if another one can be reused
                    ImageSource imageSource = null;
                    foreach(Image img in editData.Collage.Images)
                    {
                        if (img.Source.FileName == fileNames[i])
                        {
                            imageSource = img.Source;
                            break;
                        }
                    }
                    Image image;
                    if (imageSource == null) image = new Image(dataAccess, fileNames[i]);
                    else image = new Image(imageSource);

                    // set a random position
                    image.Center = new Vector2((float)dataAccess.Random.NextDouble(), (float)dataAccess.Random.NextDouble());

                    images.Add(image);
                }
                // make a command that can load all images at once
                Command command = new Command(ExecuteAddImages, ExecuteRemoveImages, images, "Add new images");
                editData.UndoManager.ExecuteAndAddCommand(command);
            }
            return false;
        }

        private object ExecuteAddImages(object images)
        {
            // get all needed sources and add the image to the collage
            List<ImageSource> imageSources = new List<ImageSource>();
            foreach (Image image in (List<Image>)images)
            {
                imageSources.Add(image.Source);
                editData.Collage.Images.Add(image);
            }
            // reverse the list because the top images should be loaded at first for a better user experience
            imageSources.Reverse();
            AsyncImageSourcesLoader loader = new AsyncImageSourcesLoader(dataAccess, imageSources);
            loader.LoadAll();
            return images;
        }
        private object ExecuteRemoveImages(object images)
        {
            List<Image> imageList = (List<Image>)images;
            foreach (Image image in imageList)
            {
                // remove the image from the collage
                editData.Collage.Images.Remove((Image)image);
                // check if there are other images with the same source
                bool unloadSource = true;
                foreach (Image img in editData.Collage.Images)
                {
                    if (((Image)image).Source == img.Source)
                    {
                        unloadSource = false;
                        break;
                    }
                }
                // unload the texture to free memory
                if (unloadSource) ((Image)image).Unload();
            }
            return images;
        }
    }
}
