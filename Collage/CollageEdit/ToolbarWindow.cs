using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Collage
{
    public class ToolbarWindow
    {
        Keymap keymap;

        Window window;
        Button openButton, autoPositionButton;
        CheckButton stayOnTopCheckbutton;
        List<object> interactions;

        public ToolbarWindow(Keymap keymap) 
        {
            this.keymap = keymap;
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
            openButton.SetSizeRequest(100, 30);
            openButton.TooltipText = "Shortcut: " + keymap["open images"].ToString();
            openButton.Name = "open images";
            openButton.Clicked += OperatorButtonClicked;
            fix.Put(openButton, 10, 20);

            autoPositionButton = new Button();
            autoPositionButton.Label = "Auto Position";
            autoPositionButton.SetSizeRequest(100, 30);
            autoPositionButton.TooltipText = "Shortcut: " + keymap["auto position"].ToString();
            autoPositionButton.Name = "auto position";
            autoPositionButton.Clicked += OperatorButtonClicked;
            fix.Put(autoPositionButton, 10, 55);

            stayOnTopCheckbutton = new CheckButton();
            stayOnTopCheckbutton.Label = "Stay on Top";
            stayOnTopCheckbutton.Toggled += StayOnTopToogled;
            fix.Put(stayOnTopCheckbutton, 10, 200);

            window.Add(fix);
            window.ShowAll();
        }

        void StayOnTopToogled(object sender, EventArgs e)
        {
            window.KeepAbove = ((CheckButton)sender).Active;
        }

        void OperatorButtonClicked(object sender, EventArgs e)
        {
            interactions.Add(((Button)sender).Name);
        }

        public List<object> GetInteractions()
        {
            return interactions;
        }

        public void SetSensitivity(bool sensitivity)
        {
            if (window != null) window.Sensitive = sensitivity;
        }
    }
}
