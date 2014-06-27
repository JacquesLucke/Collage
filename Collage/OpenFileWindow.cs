using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Collage
{
    public class OpenFileWindow
    {
        public OpenFileWindow() { }

        public string OpenFile(params FileTypes[] fileTypes)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = Utils.FileTypesToFilter(fileTypes);
            if (ofd.ShowDialog() == DialogResult.OK) return ofd.FileName;
            else return null;
        }
        public string[] OpenFiles(params FileTypes[] fileTypes)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = Utils.FileTypesToFilter(fileTypes);
            if (ofd.ShowDialog() == DialogResult.OK) return ofd.FileNames;
            else return null;
        }
    }
}
