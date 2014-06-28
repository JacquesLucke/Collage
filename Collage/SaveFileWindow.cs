using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Collage
{
    public class SaveFileWindow
    {
        public SaveFileWindow() { }

        public string SaveFile(params FileTypes[] fileTypes)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = Utils.FileTypesToFilter(fileTypes);
            if (sfd.ShowDialog() == DialogResult.OK) return sfd.FileName;
            else return null;
        }
    }
}
