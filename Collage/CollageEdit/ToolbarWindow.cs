using Gtk;
using System;
using System.Collections.Generic;

namespace Collage
{
    public class ToolbarWindow
    {
        Keymap keymap;

        Window window;
        Button openButton, saveButton, deleteButton, selectAllButton, autoPositionButton;
        Button changeAspectRatioButton;
        Button setBackwardButton, setForwardButton;
        Button setAsBackgroundButton, setToFrontButton;
        Button clearButton, changeBackgroundColorButton;
        Button undoButton, redoButton;
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
            window.Resize(230, 700);
            window.Title = "Toolbar";
            window.Deletable = false;
            window.ModifyBg(StateType.Normal, new Gdk.Color(182, 195, 205));

            Fixed fix = new Fixed();

            openButton = new Button();
            openButton.Label = "Open Images";
            openButton.SetSizeRequest(100, 30);
            openButton.TooltipText = "Shortcut: " + keymap["open images"].ToString();
            openButton.Name = "open images";
            openButton.Clicked += OperatorButtonClicked;

            saveButton = new Button();
            saveButton.Label = "Save Collage";
            saveButton.SetSizeRequest(100, 30);
            saveButton.TooltipText = "Shortcut: " + keymap["save collage"].ToString();
            saveButton.Name = "save collage";
            saveButton.Clicked += OperatorButtonClicked;

            deleteButton = new Button();
            deleteButton.Label = "Delete";
            deleteButton.SetSizeRequest(100, 30);
            deleteButton.TooltipText = "Shortcut: " + keymap["delete images"].ToString();
            deleteButton.Name = "delete images";
            deleteButton.Clicked += OperatorButtonClicked;

            changeAspectRatioButton = new Button();
            changeAspectRatioButton.Label = "Aspect Ratio";
            changeAspectRatioButton.SetSizeRequest(100, 30);
            changeAspectRatioButton.TooltipText = "Shortcut: " + keymap["change aspect ratio"].ToString();
            changeAspectRatioButton.Name = "change aspect ratio";
            changeAspectRatioButton.Clicked += OperatorButtonClicked;
            
            autoPositionButton = new Button();
            autoPositionButton.Label = "Auto Position";
            autoPositionButton.SetSizeRequest(100, 30);
            autoPositionButton.TooltipText = "Shortcut: " + keymap["auto position"].ToString();
            autoPositionButton.Name = "auto position";
            autoPositionButton.Clicked += OperatorButtonClicked;

            changeBackgroundColorButton = new Button();
            changeBackgroundColorButton.Label = "Background";
            changeBackgroundColorButton.SetSizeRequest(100, 30);
            changeBackgroundColorButton.TooltipText = "Shortcut: " + keymap["change background color"].ToString();
            changeBackgroundColorButton.Name = "change background color";
            changeBackgroundColorButton.Clicked += OperatorButtonClicked;

            setBackwardButton = new Button();
            setBackwardButton.Label = "Set Backward";
            setBackwardButton.SetSizeRequest(100, 30);
            setBackwardButton.TooltipText = "Shortcut: " + keymap["set backward"].ToString();
            setBackwardButton.Name = "set backward";
            setBackwardButton.Clicked += OperatorButtonClicked;

            setForwardButton = new Button();
            setForwardButton.Label = "Set Forward";
            setForwardButton.SetSizeRequest(100, 30);
            setForwardButton.TooltipText = "Shortcut: " + keymap["set forward"].ToString();
            setForwardButton.Name = "set forward";
            setForwardButton.Clicked += OperatorButtonClicked;

            setAsBackgroundButton = new Button();
            setAsBackgroundButton.Label = "Set Background";
            setAsBackgroundButton.SetSizeRequest(100, 30);
            setAsBackgroundButton.TooltipText = "Shortcut: " + keymap["set as background"].ToString();
            setAsBackgroundButton.Name = "set as background";
            setAsBackgroundButton.Clicked += OperatorButtonClicked;

            setToFrontButton = new Button();
            setToFrontButton.Label = "Set to Front";
            setToFrontButton.SetSizeRequest(100, 30);
            setToFrontButton.TooltipText = "Shortcut: " + keymap["set to front"].ToString();
            setToFrontButton.Name = "set to front";
            setToFrontButton.Clicked += OperatorButtonClicked;

            clearButton = new Button();
            clearButton.Label = "Clear Collage";
            clearButton.SetSizeRequest(100, 30);
            clearButton.TooltipText = "Shortcut: " + keymap["clear collage"].ToString();
            clearButton.Name = "clear collage";
            clearButton.Clicked += OperatorButtonClicked;

            selectAllButton = new Button();
            selectAllButton.Label = "Select All";
            selectAllButton.SetSizeRequest(100, 30);
            selectAllButton.TooltipText = "Shortcut: " + keymap["select all"].ToString();
            selectAllButton.Name = "select all";
            selectAllButton.Clicked += OperatorButtonClicked;

            undoButton = new Button();
            undoButton.Label = "Undo";
            undoButton.SetSizeRequest(100, 30);
            undoButton.TooltipText = "Shortcut: " + keymap["undo"].ToString();
            undoButton.Name = "undo";
            undoButton.Clicked += OperatorButtonClicked;

            redoButton = new Button();
            redoButton.Label = "Redo";
            redoButton.SetSizeRequest(100, 30);
            redoButton.TooltipText = "Shortcut: " + keymap["redo"].ToString();
            redoButton.Name = "redo";
            redoButton.Clicked += OperatorButtonClicked;


            stayOnTopCheckbutton = new CheckButton();
            stayOnTopCheckbutton.Label = "Stay on Top";
            stayOnTopCheckbutton.Toggled += StayOnTopToogled;

            // place objects in window
            fix.Put(openButton, 10, 20); fix.Put(saveButton, 120, 20);
            fix.Put(deleteButton, 10, 55); fix.Put(changeAspectRatioButton, 120, 55);
            fix.Put(autoPositionButton, 10, 90); fix.Put(changeBackgroundColorButton, 120, 90);

            fix.Put(setBackwardButton, 10, 140); fix.Put(setForwardButton, 120, 140);
            fix.Put(setAsBackgroundButton, 10, 175); fix.Put(setToFrontButton, 120, 175);
            fix.Put(clearButton, 10, 210); fix.Put(selectAllButton, 120, 210);

            fix.Put(undoButton, 10, 260); fix.Put(redoButton, 120, 260);

            fix.Put(stayOnTopCheckbutton, 10, 300);

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
