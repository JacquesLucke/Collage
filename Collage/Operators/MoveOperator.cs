
namespace Collage
{
    class MoveOperator : ICollageOperator, ISpecialOperatorStart
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public MoveOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.IsMiddleButtonDown;
        }

        public bool Start()
        {
            editData.DrawRectangle.Move(dataAccess.Input.MouseDifferenceVector);
            return false;
        }
    }
}
