using Gtk;
using System;

namespace Collage
{
    public class DimensionsDialog
    {
        Window window;
        Button okButton, cancelButton;
        Label widthLabel, heightLabel;
        Entry widthInputEntry, heightInputEntry;
        ResponseType response;
        bool keepAspectRatio = true;
        float aspectRatio;

        int width, height;
        int min, max;

        int preventStackOverflow = 0;

        public DimensionsDialog() 
        {
            min = 1;
            max = 6000;

            width = 1000;
            height = 1000;
            aspectRatio = 1;

            response = ResponseType.None;
        }

        public void Start()
        {
            window = new Window(WindowType.Toplevel);
            window.SetPosition(WindowPosition.Mouse);
            window.KeepAbove = true;
            window.Resize(200, 150);
            window.Title = "Dimensions";
            window.Deletable = false;

            Fixed fix = new Fixed();

            // width
            widthLabel = new Label();
            widthLabel.Text = "Width";
            fix.Put(widthLabel, 15, 25);

            widthInputEntry = new Entry();
            widthInputEntry.SetSizeRequest(100, 25);
            widthInputEntry.TextInserted += OnlyNumber;
            widthInputEntry.TextInserted += ChangeWidth;
            widthInputEntry.TextDeleted += ChangeWidth;
            fix.Put(widthInputEntry, 80, 20);

            // height
            heightLabel = new Label();
            heightLabel.Text = "Height";
            fix.Put(heightLabel, 15, 75);

            heightInputEntry = new Entry();
            heightInputEntry.SetSizeRequest(100, 25);
            heightInputEntry.TextInserted += OnlyNumber;
            heightInputEntry.TextInserted += ChangeHeight;
            heightInputEntry.TextDeleted += ChangeHeight;
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

            widthInputEntry.Text = "" + width;
            heightInputEntry.Text = "" + height;
        }

        public void SetInputRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        public void SetData(int width, float aspectRatio)
        {
            this.width = width;
            this.height = (int)Math.Round(3000 / aspectRatio);
            this.aspectRatio = aspectRatio;
        }

        void okButton_Clicked(object sender, EventArgs e)
        {
            if(width > max || height > max)
            {
                Window messageWindow = new Window("Info");
                messageWindow.SetSizeRequest(250, 100);
                messageWindow.SetPosition(WindowPosition.Mouse);

                MessageDialog messageDialog = new MessageDialog(messageWindow, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, "Width and height have to be below 6000");
                messageDialog.Run();
                messageDialog.Destroy();
                return;
            }
            response = ResponseType.Ok;
        }
        void cancelButton_Clicked(object sender, EventArgs e)
        {
            response = ResponseType.Cancel;
        }

        public void Destroy()
        {
            if(window != null) window.Destroy();
        }

        public int InputWidth
        {
            get { return width; }
            set 
            { 
                width = value;
                if (widthInputEntry != null) widthInputEntry.Text = "" + value; 
            }
        }
        public int InputHeight
        {
            get { return height; }
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
            }
            catch
            {
                ((Entry)o).DeleteText(args.Position - args.Text.Length, args.Position);
            }
        }
        private void ChangeWidth(object o, object args)
        {
            if (preventStackOverflow == 0)
            {
                preventStackOverflow = 1;
                width = Convert.ToInt32("0" + widthInputEntry.Text);
                if (keepAspectRatio) InputHeight = (int)Math.Round(width / aspectRatio);
                preventStackOverflow = 0;
            }
        }
        private void ChangeHeight(object o, object args)
        {
            if (preventStackOverflow == 0)
            {
                preventStackOverflow = 1;
                height = Convert.ToInt32("0" + heightInputEntry.Text);
                if (keepAspectRatio) InputWidth = (int)Math.Round(height * aspectRatio);
                preventStackOverflow = 0;
            }
        }
    }
}