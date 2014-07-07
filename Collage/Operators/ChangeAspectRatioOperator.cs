namespace Collage
{
    public class ChangeAspectRatioOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        float startAspectRatio;

        public ChangeAspectRatioOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }

        public bool Start()
        {
            startAspectRatio = editData.Collage.AspectRatio;
            return true;
        }

        public bool Update()
        {
            if (dataAccess.Keymap["change aspect ratio"].IsDown(dataAccess.Input))
            {
                editData.Collage.AspectRatio += dataAccess.Input.MouseDifferenceVector.X / 500f;
                return true;
            }
            else
            {
                Command command = new Command(ExecuteAspectRatioChange, ExecuteAspectRatioChange, editData.Collage.AspectRatio, "Change Aspect Ratio");
                command.SetUndoData(startAspectRatio);
                editData.UndoManager.AddCommand(command);
                return false;
            }
        }

        public object ExecuteAspectRatioChange(object newAspectRatio)
        {
            editData.Collage.AspectRatio = (float)newAspectRatio;
            return null;
        }
    }
}
