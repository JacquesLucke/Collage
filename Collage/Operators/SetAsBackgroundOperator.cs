using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Collage
{
    public class SetAsBackgroundOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public SetAsBackgroundOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Keymap["set as background"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            if (editData.SelectedImages.Count > 0)
            {
                Image newBackgroundImage = editData.SelectedImages.First();

                ImageData newImageData = new ImageData();
                newImageData.Center = new Vector2(0.5f);
                newImageData.Rotation = 0f;
                newImageData.Width = 1f;

                int oldIndex = editData.Collage.Images.IndexOf(newBackgroundImage);

                Command command = new Command(ExecuteSetAsBackground, UndoSetAsBackground, new List<object>() { oldIndex, newImageData }, "Set To Front");
                command.SetUndoData(new List<object>() { oldIndex, newBackgroundImage.Data });
                editData.UndoManager.ExecuteAndAddCommand(command);
            }
            return false;
        }

        public object ExecuteSetAsBackground(object data)
        {
            int index = (int)((List<object>)data)[0];
            Image image = editData.Collage.Images[index];
            ImageData imageData = (ImageData)((List<object>)data)[1];

            editData.Collage.Images.Remove(image);
            editData.Collage.Images.Insert(0, image);
            image.Data = imageData;

            return null;
        }
        public object UndoSetAsBackground(object data)
        {
            int index = (int)((List<object>)data)[0];
            ImageData imageData = (ImageData)((List<object>)data)[1];

            Image image = editData.Collage.Images.First();

            editData.Collage.Images.Remove(image);
            editData.Collage.Images.Insert(index, image);
            image.Data = imageData;

            return null;
        }
    }
}
