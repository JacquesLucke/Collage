using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Collage
{
    class RandomSelectionOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        List<Image> selectionBefore;
        List<Image> randomOrder;
        float selectionFraction;

        public RandomSelectionOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }

        public bool Start()
        {
            selectionFraction = 0.5f;
            selectionBefore = new List<Image>(editData.SelectedImages);

            List<Image> imageListCopy = new List<Image>(editData.Collage.Images);
            randomOrder = new List<Image>();
            for (int i = imageListCopy.Count - 1; i >= 0; i--)
            {
                int index = dataAccess.Random.Next(i);
                randomOrder.Add(imageListCopy[index]);
                imageListCopy.RemoveAt(index);
            }
            
            return true;
        }

        public bool Update()
        {
            if (dataAccess.Input.IsLeftButtonReleased)
            {
                Command command = new Command(ExecuteSelectionChange, ExecuteSelectionChange, new List<Image>(editData.SelectedImages), "Random Selection");
                command.SetUndoData(selectionBefore);
                editData.UndoManager.AddCommand(command);
                return false;
            }
            else
            {
                selectionFraction = dataAccess.Input.MousePositionVector.X / (float)dataAccess.GraphicsDevice.Viewport.Width;
                selectionFraction = MathHelper.Clamp(selectionFraction, 0, 1);

                editData.SelectedImages.Clear();
                editData.SelectedImages.AddRange(randomOrder.GetRange(0, (int)Math.Floor(randomOrder.Count * selectionFraction)));
                return true;
            }
        }

        public object ExecuteSelectionChange(object newSelection)
        {
            editData.SelectedImages.Clear();
            editData.SelectedImages.AddRange((List<Image>)newSelection);
            return null;
        }
    }
}
