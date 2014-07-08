using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Collage
{
    public class CollageEditState : IState
    {
        DataAccess dataAccess;
        CollagePreviewRenderer previewRenderer;
        CollageEditData editData;

        List<ICollageOperator> collageOperators;
        IUpdateableCollageOperator activeOperator;
        List<IOperatorActivator> activators;

        public CollageEditState(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;

            // create CollageEditData. Inside you find all the information about the collage and how it is drawn
            CollageObject collage = new CollageObject();
            int width = dataAccess.GraphicsDevice.Viewport.Bounds.Width - 100;
            int height = (int)Math.Round(width / collage.AspectRatio);
            MoveableRectangle drawRectangle = new MoveableRectangle(new FloatRectangle(50, 50, width, height));
            UndoManager undoManager = new UndoManager();

            editData = new CollageEditData(collage, drawRectangle, undoManager);

            // create the Preview Renderer
            previewRenderer = new CollagePreviewRenderer(dataAccess);
            previewRenderer.SetEditData(editData);

            RegisterCollageOperators();

            // create activators
            activators = new List<IOperatorActivator>();
            activators.Add(new SpecialOperatorActivator(dataAccess, collageOperators));
            activators.Add(new KeymapActivator(dataAccess, collageOperators));
            activators.Add(new ToolbarActivator(dataAccess, collageOperators));
        }

        public void Start()
        {
        }

        public void Update()
        {
            Input input = dataAccess.Input;
            editData.Update(input);

            // undo
            if(activeOperator == null && dataAccess.Keymap["undo"].IsPressed(dataAccess.Input))
            {
                editData.UndoManager.Undo();
            }
            // redo
            if (activeOperator == null && dataAccess.Keymap["redo"].IsPressed(dataAccess.Input))
            {
                editData.UndoManager.Redo();
            }

            // update or deactivate operators
            if (activeOperator != null)
            {
                if (!activeOperator.Update())
                {
                    activeOperator = null;
                    SetActivatorSensitivity(true);
                }
            }
            if (activeOperator == null)
            {
                List<ICollageOperator> startableOperators = new List<ICollageOperator>();

                // check activators
                foreach(IOperatorActivator activator in activators)
                {
                    startableOperators.AddRange(activator.GetActivatedOperators());
                }

                // start startable operators
                foreach(ICollageOperator op in startableOperators)
                {
                    if(op.Start() && op is IUpdateableCollageOperator)
                    {
                        activeOperator = (IUpdateableCollageOperator)op;
                        SetActivatorSensitivity(false);
                        break;
                    }
                }
            }
        }

        private void SetActivatorSensitivity(bool sensitivity)
        {
            foreach(IOperatorActivator activator in activators)
            {
                if (activator is IParallelOperatorActivator) ((IParallelOperatorActivator)activator).SetSensitivity(sensitivity);
            }
        }

        public void Draw()
        {
            previewRenderer.Draw();
        }

        public void RegisterCollageOperators()
        {
            Keymap keymap = dataAccess.Keymap;

            // create list with operators
            collageOperators = new List<ICollageOperator>();            // every Operator has an index
            collageOperators.Add(new MoveOperator());                   //  0
            collageOperators.Add(new ZoomOperator());                   //  1
            collageOperators.Add(new ChangeBackgroundColorOperator());  //  2
            collageOperators.Add(new OpenImageOperator());              //  3
            collageOperators.Add(new SelectImageOperator());            //  4    
            collageOperators.Add(new GrabOperator());                   //  5
            collageOperators.Add(new ScaleOperator());                  //  6
            collageOperators.Add(new RotateOperator());                 //  7
            collageOperators.Add(new DeleteImageOperator());            //  8
            collageOperators.Add(new SelectAllOperator());              //  9
            collageOperators.Add(new SaveCollageOperator());            // 10
            collageOperators.Add(new AutoPositonOperator());            // 11  
            collageOperators.Add(new ChangeAspectRatioOperator());      // 12
            collageOperators.Add(new SetToFrontOperator());             // 13
            collageOperators.Add(new SetAsBackgroundOperator());        // 14
            collageOperators.Add(new SetForwardOperator());             // 15
            collageOperators.Add(new SetBackwardOperator());            // 16
            collageOperators.Add(new ClearCollageOperator());           // 17

            foreach(ICollageOperator op in collageOperators)
                op.SetData(dataAccess, editData);

            // register keymap for operators
            keymap.Set("change background color", new KeyCombination(false, false, false, Keys.B));
            keymap.Set("open images", new KeyCombination(false, false, false, Keys.O));
            keymap.Set("delete images", new KeyCombination(false, false, false, Keys.X));
            keymap.Set("select all", new KeyCombination(false, false, false, Keys.A));
            keymap.Set("save collage", new KeyCombination(false, false, false, Keys.S));
            keymap.Set("auto position", new KeyCombination(false, false, false, Keys.Q));
            keymap.Set("change aspect ratio", new KeyCombination(false, false, false, Keys.R));
            keymap.Set("set to front", new KeyCombination(false, false, false, Keys.Up));
            keymap.Set("set as background", new KeyCombination(false, false, false, Keys.Down));
            keymap.Set("set forward", new KeyCombination(false, false, false, Keys.Right));
            keymap.Set("set backward", new KeyCombination(false, false, false, Keys.Left));
            keymap.Set("clear collage", new KeyCombination(true, false, false, Keys.N));
        }
    }
}
