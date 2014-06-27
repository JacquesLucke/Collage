using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class CollageEditData
    {
        CollageObject collage;
        MoveableRectangle drawRectangle;
        UndoManager undoManager;
        List<Image> selectedImages;

        Image imageUnderMouse;

        public CollageEditData(CollageObject collage, MoveableRectangle drawRectangle, UndoManager undoManager)
        {
            this.collage = collage;
            this.drawRectangle = drawRectangle;
            this.undoManager = undoManager;

            selectedImages = new List<Image>();
        }

        public CollageObject Collage
        {
            get { return collage; }
        }
        public MoveableRectangle DrawRectangle
        {
            get { return drawRectangle; }
        }
        public UndoManager UndoManager
        {
            get { return undoManager; }
        }

        public List<Image> SelectedImages
        {
            get { return selectedImages; }
        }

        public void Update(Input input)
        {
            imageUnderMouse = null;
            foreach (Image image in collage.Images)
            {
                Rectangle rec = image.GetRectangleInBoundary(drawRectangle.Rectangle);
                if (Utils.IsVectorInRotatedRectangle(input.MousePositionVector, rec, image.Rotation)) imageUnderMouse = image;
            }
        }

        public Image ImageUnderMouse
        {
            get { return imageUnderMouse; }
        }
    }
}
