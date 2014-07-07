using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Collage
{
    public class SetBackwardOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public SetBackwardOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }

        public bool Start()
        {
            List<Image> newOrder = new List<Image>(editData.Collage.Images);
            Rectangle testBoundary = new Rectangle(0, 0, 1000, 600);

            foreach (Image image in editData.SelectedImages)
            {
                int index = newOrder.IndexOf(image);
                newOrder.Remove(image);
                Rectangle rec1 = image.GetRectangleInBoundary(testBoundary);

                for (int i = index - 1; i >= 0; i--)
                {
                    index = i;
                    Rectangle rec2 = newOrder[i].GetRectangleInBoundary(testBoundary);
                    if (Utils.CouldOverlap(rec1, rec2, image.Rotation, newOrder[i].Rotation)) break;
                }
                newOrder.Insert((int)MathHelper.Clamp(index, 0, newOrder.Count), image);
            }

            Command command = new Command(ExecuteOrderChange, ExecuteOrderChange, newOrder, "Set Backward");
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
