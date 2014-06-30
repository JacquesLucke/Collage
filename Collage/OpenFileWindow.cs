using System;
using System.Windows.Forms; 
using Gtk;

namespace Collage
{
    public class OpenFileWindow
    {
        DataAccess dataAccess;
        OpenFileDialog ofd;
        DialogResult result = DialogResult.No;
        FileChooserDialog fcd;


        public OpenFileWindow(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;
        }

        public void OpenDialog(bool multipleFiles, params FileTypes[] fileTypes)
        {
            ofd = new OpenFileDialog();
            ofd.Multiselect = multipleFiles;
            ofd.Filter = Utils.FileTypesToFilter(fileTypes);
            result = ofd.ShowDialog();
        }

        public string SelectedFile
        {
            get
            {
                if (result == DialogResult.OK) return ofd.FileName;
                return null;
            }
        }
        public string[] SelectedFiles
        {
            get
            {
                if (result == DialogResult.OK) return ofd.FileNames;
                return null;
            }
        }
    }
}
