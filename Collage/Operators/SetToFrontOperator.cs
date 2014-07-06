using System.Collections.Generic;

namespace Collage
{
    public class SetToFrontOperator : ICollageOperator, ISpecialOperatorStart
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public SetToFrontOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Keymap["set to front"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            List<Image> newOrder = new List<Image>(editData.Collage.Images);
            foreach (Image image in editData.SelectedImages)
            {
                newOrder.Remove(image);
                newOrder.Add(image);
            }
            Command command = new Command(ExecuteOrderChange, ExecuteOrderChange, newOrder, "Set To Front");
            editData.UndoManager.ExecuteAndAddCommand(command);
            return false;
        }

        public object ExecuteOrderChange(object newOrder)
        {
            List<Image> oldOrder = new List<Image>(editData.Collage.Images);
            editData.Collage.Images.Clear();
            editData.Collage.Images.AddRange((List<Image>)newOrder);
            return oldOrder;
        }
    }
}
