using Microsoft.Xna.Framework;
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
        CollageObject collage;
        CollagePreviewRenderer previewRenderer;
        MoveableRectangle drawRectangle;

        public CollageEditState(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;
            collage = new CollageObject();
            previewRenderer = new CollagePreviewRenderer(dataAccess);
            previewRenderer.SetCollage(collage);

            int width = dataAccess.GraphicsDevice.Viewport.Bounds.Width - 100;
            int height = (int)Math.Round(width / collage.AspectRatio);
            drawRectangle = new MoveableRectangle(new FloatRectangle(50, 50, width, height));
        }

        public void Start()
        {

        }

        public void Update()
        {
            Input input = dataAccess.Input;

            if(input.IsMiddleButtonDown)
            {
                drawRectangle.Move(input.MouseDifferenceVector);
            }
            drawRectangle.Zoom(-input.ScrollWheelDifference / 10f, input.MousePositionVector);
        }

        public void Draw()
        {
            previewRenderer.Draw(drawRectangle.Rectangle);
        }
    }
}
