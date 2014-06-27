using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public bool CanStart()
        {
            return dataAccess.Keymap["delete image"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            CommandCombination commands = new CommandCombination();
            foreach(Image image in editData.SelectedImages)
            {
                Command command = new Command(ExecuteRemoveImage, ExecuteAddImage, image, "Delete Image");
                commands.ExecuteAndAddToCombination(command);
            }
            editData.UndoManager.AddCommand(commands);
            return false;
        }

        private object ExecuteRemoveImage(object image)
        {
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
            if (unloadSource) ((Image)image).Unload();
            return image;
        }
        private object ExecuteAddImage(object image)
        {
            ((Image)image).Reload();
            editData.Collage.Images.Add((Image)image);
            return null;
        }
    }
}