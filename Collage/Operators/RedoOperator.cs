namespace Collage
{
    public class RedoOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public RedoOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }

        public bool Start()
        {
            editData.UndoManager.Redo();
            return false;
        }
    }
}
