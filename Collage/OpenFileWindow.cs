#if WINDOWS
using System.Windows.Forms; 
#endif

namespace Collage
{
    public class OpenFileWindow
    {
        public OpenFileWindow() { }

        public string OpenFile(params FileTypes[] fileTypes)
        {
#if WINDOWS
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = Utils.FileTypesToFilter(fileTypes);
            if (ofd.ShowDialog() == DialogResult.OK) return ofd.FileName;
#endif
            else return null;
        }
        public string[] OpenFiles(params FileTypes[] fileTypes)
        {
#if WINDOWS
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = Utils.FileTypesToFilter(fileTypes);
            if (ofd.ShowDialog() == DialogResult.OK) return ofd.FileNames;
#endif
            else return null;
        }
    }
}
