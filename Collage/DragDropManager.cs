using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Collage
{
    public class DragDropManager
    {
        Form form;

        bool dragEnter = false;
        bool dragOver = false;
        bool dragDrop = false;
        DragEventArgs dragEventArgs = null;

        public DragDropManager(GameWindow window)
        {
            //form = Form.FromHandle(handle) as Form;
            //form.AllowDrop = true;
            //form.DragEnter += form_DragEnter;
            //form.DragOver += form_DragOver;
            //form.DragDrop += form_DragDrop;
        }

        public void Reset()
        {
            dragEnter = false;
            dragOver = false;
            dragDrop = false;
            dragEventArgs = null;
        }

        private void form_DragEnter(object sender, DragEventArgs e)
        {
            dragEnter = true;
            dragEventArgs = e;
            e.Effect = DragDropEffects.All;
        }
        private void form_DragOver(object sender, DragEventArgs e)
        {
            dragOver = true;
            dragEventArgs = e;
        }
        private void form_DragDrop(object sender, DragEventArgs e)
        {
            dragDrop = true;
            dragEventArgs = e;
        }

        public bool DragEnter { get { return dragEnter; } }
        public bool DragOver { get { return dragOver; } }
        public bool DragDrop { get { return dragDrop; } }
        public DragEventArgs DragEventArgs { get { return dragEventArgs; } }
    }
}
