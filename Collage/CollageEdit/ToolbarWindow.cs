using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Collage
{
    public class ToolbarWindow
    {
        Window window;
        Button openButton;
        List<object> interactions;

        public ToolbarWindow() 
        {
            interactions = new List<object>();
        }

        public void Start()
        {
            window = new Window(WindowType.Toplevel);
            window.Move(10, 60);
            window.Resize(200, 700);
            window.Title = "Toolbar";
            window.Deletable = false;

            Fixed fix = new Fixed();

            openButton = new Button();
            openButton.Label = "Open Images";
            openButton.SetSizeRequest(80, 30);
            openButton.Clicked += openButton_Clicked;
            fix.Put(openButton, 10, 20);

            window.Add(fix);
            window.ShowAll();
        }

        void openButton_Clicked(object sender, EventArgs e)
        {
            interactions.Add("open images");
        }

        public List<object> GetInteractions()
        {
            return interactions;
        }
    }
}
