using Microsoft.Xna.Framework;

namespace Collage
{
    class ChangeBackgroundColorOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        Gtk.ColorSelectionDialog colorDialog;
        Color startColor;

        public ChangeBackgroundColorOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Keymap["change collage background color"].IsCombinationPressed(dataAccess.Input);
        }

        public bool Start()
        {
            startColor = editData.Collage.BackgroundColor;
            dataAccess.GtkThread.Invoke(OpenColorDialog);
            return true;
        }
        public bool Update()
        {
            bool isColorChoosed = !dataAccess.GtkThread.IsBlockedByDialog;
            // update displayed color
            if(colorDialog != null) editData.Collage.BackgroundColor = Utils.ToColor(colorDialog.ColorSelection.CurrentColor);

            if(isColorChoosed)
            {
                Color color = Utils.ToColor(colorDialog.ColorSelection.CurrentColor);
                colorDialog.Destroy();

            Command command = new Command(ExecuteColorChange, ExecuteColorChange, color, "Change Background Color");
                command.SetUndoData(startColor);
            editData.UndoManager.ExecuteAndAddCommand(command);
            }
            return !isColorChoosed;
        }

        public void OpenColorDialog()
        {
            colorDialog = new Gtk.ColorSelectionDialog("Choose color");
            colorDialog.ColorSelection.CurrentColor = Utils.ToColor(editData.Collage.BackgroundColor);
            colorDialog.Run();
        }

        public object ExecuteColorChange(object color)
        {
            Color c = editData.Collage.BackgroundColor;
            editData.Collage.BackgroundColor = (Color)color;
            return c;
        }
    }
}
