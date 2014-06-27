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
            OpenFileWindow of = new OpenFileWindow();
            string[] fileNames = of.OpenFiles(FileTypes.Images);
            if(fileNames != null)
            {
                CommandCombination commands = new CommandCombination();
                for (int i = 0; i < fileNames.Length; i++)
                {
                    Image image = new Image(dataAccess, fileNames[i]);
                    image.Center = new Vector2((float)dataAccess.Random.NextDouble(), (float)dataAccess.Random.NextDouble());

                    Command command = new Command(ExecuteAddImage, ExecuteRemoveImage, image, "Add new image");
                    commands.ExecuteAndAddToCombination(command);
                }
                editData.UndoManager.AddCommand(commands);
            }
            return false;
        }

        private object ExecuteAddImage(object image)
        {
            ((Image)image).Reload();
            editData.Collage.Images.Add((Image)image);
            return image;
        }
        private object ExecuteRemoveImage(object image)
        {
            editData.Collage.Images.Remove((Image)image);

            // check if there are other images with the same source
            bool unloadSource = true;
            foreach(Image img in editData.Collage.Images)
            {
                if (((Image)image).Source == img.Source)
                {
                    unloadSource = false;
                    break;
                }
            }
            if (unloadSource) ((Image)image).Unload();
            return null;
        }
    }
}
