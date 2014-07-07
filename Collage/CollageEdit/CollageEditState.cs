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

            previewRenderer = new CollagePreviewRenderer(dataAccess);
            previewRenderer.SetEditData(editData);

            RegisterCollageOperators();

            SpecialOperatorActivator specialActivator = new SpecialOperatorActivator(dataAccess, collageOperators);

            activators = new List<IOperatorActivator>();
            activators.Add(specialActivator);
        }

        public void Start()
        {
        }

        public void Update()
        {
            Input input = dataAccess.Input;
            editData.Update(input);

            // undo
            if(activeOperator == null && dataAccess.Keymap["undo"].IsCombinationPressed(dataAccess.Input))
            {
                editData.UndoManager.Undo();
            }
            // redo
            if (activeOperator == null && dataAccess.Keymap["redo"].IsCombinationPressed(dataAccess.Input))
            {
                editData.UndoManager.Redo();
            }

            // update or deactivate operators
            if (activeOperator != null)
            {
                if (!activeOperator.Update()) activeOperator = null;
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
                        break;
                    }
                }
            }
        }

        public void Draw()
        {
            previewRenderer.Draw();
        }

        public void RegisterCollageOperators()
        {
            collageOperators = new List<ICollageOperator>();
            collageOperators.Add(new MoveOperator());
            collageOperators.Add(new ZoomOperator());
            collageOperators.Add(new ChangeBackgroundColorOperator());
            collageOperators.Add(new OpenImageOperator());
            collageOperators.Add(new SelectImageOperator());
            collageOperators.Add(new GrabOperator());
            collageOperators.Add(new ScaleOperator());
            collageOperators.Add(new RotateOperator());
            collageOperators.Add(new DeleteImageOperator());
            collageOperators.Add(new SelectAllOperator());
            collageOperators.Add(new SaveCollageOperator());
            collageOperators.Add(new AutoPositonOperator());
            collageOperators.Add(new ChangeAspectRatioOperator());
            collageOperators.Add(new SetToFrontOperator());
            collageOperators.Add(new SetAsBackgroundOperator());
            collageOperators.Add(new SetForwardOperator());
            collageOperators.Add(new SetBackwardOperator());
            collageOperators.Add(new ClearCollageOperator());

            foreach(ICollageOperator op in collageOperators)
            {
                op.SetData(dataAccess, editData);
            }
        }
    }
}
