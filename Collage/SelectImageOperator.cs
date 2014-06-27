using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class SelectImageOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public SelectImageOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.IsLeftButtonPressed;
        }

        public bool Start()
        {
            List<Image> newSelection = new List<Image>();

            Rectangle drawRectangle = editData.DrawRectangle.Rectangle;
            foreach(Image image in editData.Collage.Images)
            {
                Rectangle rec = image.GetRectangleInBoundary(drawRectangle);
                if(rec.Contains(dataAccess.Input.MousePositionVector))
                {
                    newSelection.Clear();
                    newSelection.Add(image);
                }
            }

            Command command = new Command(ExecuteSelection, ExecuteSelection, newSelection, "Select Image(s)");
            editData.UndoManager.ExecuteAndAddCommand(command);
            return false;
        }

        private object ExecuteSelection(object newSelection)
        {
            List<Image> oldSelection = new List<Image>();
            oldSelection.AddRange(editData.SelectedImages);

            editData.SelectedImages.Clear();
            editData.SelectedImages.AddRange((List<Image>)newSelection);

            return oldSelection;
        }
    }
}
