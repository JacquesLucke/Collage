using System.Collections.Generic;

namespace Collage
{
    class DeleteImageOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public DeleteImageOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }

        public bool Start()
        {
            List<Image> images = new List<Image>();
            foreach(Image image in editData.SelectedImages)
            {
                images.Add(image);
            }
            Command command = new Command(ExecuteRemoveImages, ExecuteAddImages, images, "Remove images");
            editData.UndoManager.ExecuteAndAddCommand(command);
            return false;
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
                if (unloadSource) image.Unload();
            }
            // clear selection because all selected images were deleted
            editData.SelectedImages.Clear();
            return images;
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
            // set images as selection
            editData.SelectedImages.Clear();
            editData.SelectedImages.AddRange((List<Image>)images);
            return images;
        }
    }
}