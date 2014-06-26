﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class CollageEditState : IState
    {
        DataAccess dataAccess;
        CollagePreviewRenderer previewRenderer;
        CollageEditData editData;

        List<ICollageOperator> collageOperators;
        IUpdateableCollageOperator activeOperator;

        public CollageEditState(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;

            CollageObject collage = new CollageObject();
            previewRenderer = new CollagePreviewRenderer(dataAccess);
            previewRenderer.SetCollage(collage);

            int width = dataAccess.GraphicsDevice.Viewport.Bounds.Width - 100;
            int height = (int)Math.Round(width / collage.AspectRatio);
            MoveableRectangle drawRectangle = new MoveableRectangle(new FloatRectangle(50, 50, width, height));

            UndoManager undoManager = new UndoManager();

            editData = new CollageEditData(collage, drawRectangle, undoManager);

            RegisterCollageOperators();
        }

        public void Start()
        {

        }

        public void Update()
        {
            Input input = dataAccess.Input;

            // undo
            if(activeOperator == null && input.IsStrg && input.IsKeyReleased(Keys.Z))
            {
                editData.UndoManager.Undo();
            }
            // redo
            if (activeOperator == null && input.IsStrg && input.IsKeyReleased(Keys.Y))
            {
                editData.UndoManager.Redo();
            }

            // update or deactivate operators
            if(activeOperator != null)
            {
                if (!activeOperator.Update()) activeOperator = null;
            }
            // activate operators
            if(activeOperator == null)
            {
                foreach(ICollageOperator op in collageOperators)
                {
                    if(op.CanStart())
                    {
                        if(op.Start() && op is IUpdateableCollageOperator)
                        {
                            activeOperator = (IUpdateableCollageOperator)op;
                            break;
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            previewRenderer.Draw(editData.DrawRectangle.Rectangle);
        }

        public void RegisterCollageOperators()
        {
            collageOperators = new List<ICollageOperator>();
            collageOperators.Add(new MoveOperator());
            collageOperators.Add(new ChangeBackgroundColorOperator());

            foreach(ICollageOperator op in collageOperators)
            {
                op.SetData(dataAccess, editData);
            }
        }
    }
}
