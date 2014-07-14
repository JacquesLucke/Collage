using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Collage
{
    public class SelectImageOperator : ICollageOperator, ISpecialOperatorStart
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

            bool selectionChanged = false;
            Rectangle drawRectangle = editData.DrawRectangle.Rectangle;

            if (!dataAccess.Input.IsShift)
            {
                // single selection
                Image newSelectedImage = editData.ImageUnderMouse;
                if (!editData.SelectedImages.Contains(newSelectedImage))
                {
                    if (newSelectedImage != null)
                    {
                        newSelection.Add(newSelectedImage);
                        selectionChanged = editData.SelectedImages.Count != 1 || !editData.SelectedImages.Contains(newSelectedImage);
                    }
                    else
                    {
                        selectionChanged = editData.SelectedImages.Count != 0;
                    }
                }
            }
            else
            {
                // multiselect
                newSelection.AddRange(editData.SelectedImages);

                Image newSelectedImage = editData.ImageUnderMouse;
                if(newSelectedImage != null)
                {
                    if (editData.SelectedImages.Contains(newSelectedImage)) newSelection.Remove(newSelectedImage);
                    else newSelection.Add(newSelectedImage);
                    selectionChanged = true;
                }
            }

            if (selectionChanged)
            {
                Command command = new Command(ExecuteSelection, ExecuteSelection, newSelection, "Select Image(s)");
                editData.UndoManager.ExecuteAndAddCommand(command);
            }
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
