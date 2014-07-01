using Microsoft.Xna.Framework;

namespace Collage
{
    public class GrabOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        Vector2 lastMouseDownPosition;
        Vector2 totalMove;

        public GrabOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;

            lastMouseDownPosition = dataAccess.Input.MousePositionVector;
        }
        public bool CanStart()
        {
            if (!dataAccess.Input.IsLeftButtonDown) lastMouseDownPosition = dataAccess.Input.MousePositionVector;

            return Vector2.Distance(lastMouseDownPosition, dataAccess.Input.MousePositionVector) > 15;
        }

        public bool Start()
        {
            totalMove = Vector2.Zero;
            return editData.SelectedImages.Count > 0;
        }

        public bool Update()
        {
            bool continueMove = dataAccess.Input.IsLeftButtonDown;
            if (continueMove)
            {
                foreach (Image image in editData.SelectedImages)
                {
                    image.SetCenterInBoundary(editData.DrawRectangle.Rectangle, image.GetCenterInBoundary(editData.DrawRectangle.Rectangle) + dataAccess.Input.MouseDifferenceVector);
                }
                totalMove += dataAccess.Input.MouseDifferenceVector;
            }
            else
            {
                Command command = new Command(ExecuteMove, ExecuteMove, totalMove, "Move Selected Images");
                command.SetUndoData(-totalMove);
                editData.UndoManager.AddCommand(command);
            }
            return continueMove;
        }

        public object ExecuteMove(object distance)
        {
            foreach (Image image in editData.SelectedImages)
            {
                image.SetCenterInBoundary(editData.DrawRectangle.Rectangle, image.GetCenterInBoundary(editData.DrawRectangle.Rectangle) + (Vector2)distance);
            }
            return -(Vector2)distance;
        }
    }
}
