using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class ClearCollageOperator : ICollageOperator, ISpecialOperatorStart
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public ClearCollageOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Keymap["clear collage"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            Command command = new Command(ExecuteClearCollage, UndoClearCollage, null, "Clear Collage");
            editData.UndoManager.ExecuteAndAddCommand(command);
            return false;
        }

        public object ExecuteClearCollage(object empty)
        {
            List<Image> oldImages = new List<Image>(editData.Collage.Images);
            CollageData oldData = editData.Collage.Data.GetCopy();

            editData.Collage.Images.Clear();
            editData.Collage.SetDefaultData();

            // free data
            foreach (Image image in oldImages) image.Unload();

            return new List<object>() { oldImages, oldData };
        }

        public object UndoClearCollage(object data)
        {
            List<Image> images = (List<Image>)((List<object>)data)[0];
            CollageData collageData = (CollageData)((List<object>)data)[1];

            editData.Collage.Images.AddRange(images);
            editData.Collage.Data = collageData;

            // get all needed sources
            List<ImageSource> imageSources = new List<ImageSource>();
            foreach (Image image in (List<Image>)images)
            {
                imageSources.Add(image.Source);
            }
            // reverse the list because the top images should be loaded at first for a better user experience
            imageSources.Reverse();
            AsyncImageSourcesLoader loader = new AsyncImageSourcesLoader(dataAccess, imageSources);
            loader.LoadAll();

            return null;
        }
    }
}
