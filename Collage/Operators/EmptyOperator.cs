
namespace Collage
{
    public class EmptyOperator : ICollageOperator, ISpecialOperatorStart
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public EmptyOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return false;
        }

        public bool Start()
        {
            return false;
        }
    }
}
