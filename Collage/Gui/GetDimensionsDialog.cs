using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Collage
{
    public class GetDimensionsDialog
    {
        Window window;
        Button okButton, cancelButton;
        Label widthLabel, heightLabel;
        Entry widthInputEntry, heightInputEntry;

        public GetDimensionsDialog() { }

        public void Start()
        {
            window = new Window(WindowType.Toplevel);
            window.SetPosition(WindowPosition.Mouse);
            window.KeepAbove = true;
            window.Resize(200, 150);
            window.Title = "Dimensions";

            Fixed fix = new Fixed();

            // width
            widthLabel = new Label();
            widthLabel.Text = "Width";
            fix.Put(widthLabel, 15, 25);

            widthInputEntry = new Entry();
            widthInputEntry.SetSizeRequest(100, 25);
            widthLabel.Text = "Width";
            fix.Put(widthInputEntry, 80, 20);

            // height
            heightLabel = new Label();
            heightLabel.Text = "Height";
            fix.Put(heightLabel, 15, 75);

            heightInputEntry = new Entry();
            heightInputEntry.SetSizeRequest(100, 25);
            fix.Put(heightInputEntry, 80, 70);

            // Buttons
            okButton = new Button();
            okButton.Label = "OK";
            okButton.SetSizeRequest(80, 30);
            fix.Put(okButton, 10, 110);

            cancelButton = new Button();
            cancelButton.Label = "Cancel";
            cancelButton.SetSizeRequest(80, 30);
            fix.Put(cancelButton, 110, 110);

            window.Add(fix);
            window.ShowAll();
        }

        public void Destroy()
        {
            okButton.Destroy();
            window.Destroy();
        }
    }
}