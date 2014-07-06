using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using Microsoft.Xna.Framework;

namespace Collage
{
    public class ColorChooserDialog
    {
        ColorSelectionDialog dialog;
        ResponseType response = ResponseType.None;
        Color selectedColor;

        public ColorChooserDialog() { }

        public void OpenDialog(Color color)
        {
            Clipboard c = Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));
            c.RequestText(new ClipboardTextReceivedFunc(ClipboardReceiveText));
            selectedColor = color;

            dialog = new ColorSelectionDialog("Choose Color");
            dialog.ColorSelection.CurrentColor = Utils.ToColor(color);
            dialog.Response += DialogResponse;
            dialog.Run();
        }
        public void ClipboardReceiveText(Clipboard c, string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
        public void Destroy()
        {
            dialog.Destroy();
        }

        private void DialogResponse(object o, ResponseArgs args)
        {
            response = args.ResponseId;
        }

        public Microsoft.Xna.Framework.Color SelectedColor
        {
            get
            {
                if (dialog != null) selectedColor = Utils.ToColor(dialog.ColorSelection.CurrentColor);
                return selectedColor;
            }
        }

        public ResponseType Response
        {
            get { return response; }
        }
    }
}
