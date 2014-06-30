﻿using System;

namespace Collage
{
#if WINDOWS && WithWindowsDialogs // this OpenFileDialog uses Windows Forms
    using System.Windows.Forms; 
    public class OpenFileWindow
    {
        DataAccess dataAccess;
        OpenFileDialog ofd;
        DialogResult result = DialogResult.No;

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

        public void Destroy()
        {
            ofd.Dispose();
            ofd = null;
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
#else // here is the OpenFileDialog that uses GTK#
    using Gtk;
    public class OpenFileWindow
    {
        DataAccess dataAccess;
        FileChooserDialog fcd;

        public OpenFileWindow(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;
        }

        public void OpenDialog(bool multipleFiles, params FileTypes[] fileTypes)
        {
            fcd = new FileChooserDialog("Choose Files", null, FileChooserAction.Open, ButtonsType.Ok);
            fcd.AddButton("Open", ResponseType.Ok);
            FileFilter filter = new FileFilter();
            filter.AddPattern("*.jpg");
            filter.AddPattern("*.png");
            fcd.AddFilter(filter);
            fcd.SelectMultiple = true;
            fcd.Run();
        }

        public void Destroy()
        {
            fcd.Destroy();
        }

        public string SelectedFile
        {
            get
            {
                if (fcd != null) return fcd.Filename;
                return null;
            }
        }
        public string[] SelectedFiles
        {
            get
            {
                if (fcd != null) return fcd.Filenames;
                return null;
            }
        }
    }
#endif
}
