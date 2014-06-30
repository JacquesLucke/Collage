#define WITHWINDOWSDIALOG

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
#if WITHWINDOWSDIALOG
            ofd = new OpenFileDialog();
            ofd.Multiselect = multipleFiles;
            ofd.Filter = Utils.FileTypesToFilter(fileTypes);
            result = ofd.ShowDialog();
#else
            fcd = new FileChooserDialog("Choose Files", null, FileChooserAction.Open, ButtonsType.Ok);
            fcd.AddButton("Open", ResponseType.Ok);
            FileFilter filter = new FileFilter();
            filter.AddPattern("*.jpg");
            filter.AddPattern("*.png");
            fcd.AddFilter(filter);
            fcd.SelectMultiple = true;
            fcd.Run();
#endif
        }

        public void Destroy()
        {
#if WITHWINDOWSDIALOG == false
            fcd.Destroy();
#endif
        }

        public string SelectedFile
        {
#if WITHWINDOWSDIALOG
            get
            {
                if (result == DialogResult.OK) return ofd.FileName;
                return null;
            }
#else
            get
            {
                if (fcd != null) return fcd.Filename;
                return null;
            }
#endif
        }
        public string[] SelectedFiles
        {
#if WITHWINDOWSDIALOG
            get
            {
                if (result == DialogResult.OK) return ofd.FileNames;
                return null;
            }
#else
            get
            {
                if (fcd != null) return fcd.Filenames;
                return null;
            }
#endif
        }
    }
}
