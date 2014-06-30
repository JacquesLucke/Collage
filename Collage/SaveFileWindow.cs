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
            sfd.Filter = Utils.FileTypesToFilter(fileTypes);
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
#endif
}
