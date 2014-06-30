namespace Collage
{
#if WINDOWS && WithWindowsDialogs // this SaveFileDialog uses Windows Forms
    using System.Windows.Forms;
    public class SaveFileWindow
    {
        DataAccess dataAccess;
        SaveFileDialog sfd;
        DialogResult result = DialogResult.No;

        public SaveFileWindow(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void OpenDialog(params FileTypes[] fileTypes)
        {
            sfd = new SaveFileDialog();
            sfd.Filter = Utils.FileTypesToWinFormFilter(fileTypes);
            result = sfd.ShowDialog();
        }
        public void Destroy()
        {
            sfd.Dispose();
            sfd = null;
        }

        public string SelectedPath
        {
            get
            {
                if (result == DialogResult.OK) return sfd.FileName;
                return null;
            }
        }
    }
#else
    using Gtk;
    public class SaveFileWindow
    {
        DataAccess dataAccess;
        FileChooserDialog fcd;

        public SaveFileWindow(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void OpenDialog(params FileTypes[] fileTypes)
        {
            fcd = new FileChooserDialog("Choose Path", null, FileChooserAction.Save, ButtonsType.Ok);
            fcd.AddButton("Save", ResponseType.Ok);
            FileFilter filter = new FileFilter();
            fcd.AddFilter(Utils.FileTypesToGtkFilter(fileTypes));
            fcd.Run();
        }
        public void Destroy()
        {
            fcd.Destroy();
        }

        public string SelectedPath
        {
            get
            {
                if (fcd != null) return fcd.Filename;
                return null;
            }
        }
    }
#endif
}
