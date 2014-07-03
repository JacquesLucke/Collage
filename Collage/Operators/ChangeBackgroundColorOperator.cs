using Microsoft.Xna.Framework;

namespace Collage
{
    class ChangeBackgroundColorOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        ColorChooserDialog colorChooser;
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
            return true;
        }
        public bool Update()
        {
            if (colorChooser == null && !dataAccess.GuiThread.WaitsToInvoke) dataAccess.GuiThread.Invoke(OpenColorDialog);
            if (colorChooser != null)
            {
                editData.Collage.BackgroundColor = colorChooser.SelectedColor;
                if (colorChooser.Response != Gtk.ResponseType.None && colorChooser.Response != Gtk.ResponseType.Ok)
                {
                    editData.Collage.BackgroundColor = startColor;
                    colorChooser.Destroy();
                    colorChooser = null;
                    return false;
                }
                if (colorChooser.Response == Gtk.ResponseType.Ok)
                {
                    Command command = new Command(ExecuteColorChange, ExecuteColorChange, colorChooser.SelectedColor, "Change Background Color");
                    command.SetUndoData(startColor);
                    editData.UndoManager.AddCommand(command);

                    colorChooser.Destroy();
                    colorChooser = null;
                    return false;
                }
            }
            return true;
        }

        public void OpenColorDialog()
        {
            colorChooser = new ColorChooserDialog();
            colorChooser.OpenDialog(editData.Collage.BackgroundColor);
        }

        public object ExecuteColorChange(object color)
        {
            Color c = editData.Collage.BackgroundColor;
            editData.Collage.BackgroundColor = (Color)color;
            return c;
        }
    }
}
