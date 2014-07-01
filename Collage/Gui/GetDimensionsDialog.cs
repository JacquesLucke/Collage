using Gtk;
using System;

namespace Collage
{
    public class GetDimensionsDialog
    {
        Window window;
        Button okButton, cancelButton;
        Label widthLabel, heightLabel;
        Entry widthInputEntry, heightInputEntry;
        ResponseType response;

        int width, height;
        int min, max;

        public GetDimensionsDialog() 
        {
            min = 1;
            max = 6000;

            width = 1000;
            height = 1000;

            response = ResponseType.Cancel;
        }

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
            widthInputEntry.Text = "" + width;
            widthInputEntry.TextInserted += OnlyNumber;
            fix.Put(widthInputEntry, 80, 20);

            // height
            heightLabel = new Label();
            heightLabel.Text = "Height";
            fix.Put(heightLabel, 15, 75);

            heightInputEntry = new Entry();
            heightInputEntry.SetSizeRequest(100, 25);
            heightInputEntry.Text = "" + height;
            heightInputEntry.TextInserted += OnlyNumber;
            fix.Put(heightInputEntry, 80, 70);

            // Buttons
            okButton = new Button();
            okButton.Label = "OK";
            okButton.SetSizeRequest(80, 30);
            okButton.Clicked += okButton_Clicked;
            fix.Put(okButton, 10, 110);

            cancelButton = new Button();
            cancelButton.Label = "Cancel";
            cancelButton.SetSizeRequest(80, 30);
            cancelButton.Clicked += cancelButton_Clicked;
            fix.Put(cancelButton, 110, 110);

            window.Add(fix);
            window.ShowAll();
        }
        public void SetInputRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        void okButton_Clicked(object sender, EventArgs e)
        {
            response = ResponseType.Ok;
            Destroy();
        }
        void cancelButton_Clicked(object sender, EventArgs e)
        {
            response = ResponseType.Cancel;
            Destroy();
        }

        public void Destroy()
        {
            if(window != null) window.Destroy();
        }

        public int InputWidth
        {
            get 
            {
                if (widthInputEntry != null) width = Convert.ToInt32(widthInputEntry.Text);
                return width;
            }
            set
            { 
                width = value;
                if (widthInputEntry != null) widthInputEntry.Text = "" + value; 
            }
        }
        public int InputHeight
        {
            get
            {
                if (heightInputEntry != null) height = Convert.ToInt32(heightInputEntry.Text);
                return height;
            }
            set
            {
                height = value;
                if (heightInputEntry != null) heightInputEntry.Text = "" + value;
            }
        }

        public ResponseType Response
        {
            get { return response; }
        }

        private void OnlyNumber(object o, TextInsertedArgs args)
        {
            try
            {
                int number = Convert.ToInt32(((Entry)o).Text);
                if (number < min) number = min;
                if (number > max) number = max;
                ((Entry)o).Text = "" + number;
            }
            catch 
            {
                ((Entry)o).DeleteText(args.Position - args.Text.Length, args.Position);
            }
        }
    }
}