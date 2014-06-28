using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class GtkWindow : Window
    {
        public GtkWindow()
            : base(WindowType.Toplevel)
        {
            Resize(300, 500);

            ShowAll();
        }
    }
}
