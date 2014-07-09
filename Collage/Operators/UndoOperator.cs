namespace Collage
{
    class UndoOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public UndoOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }

        public bool Start()
        {
            editData.UndoManager.Undo();
            return false;
        }
    }
}
