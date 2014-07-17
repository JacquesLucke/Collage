using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
            collage.AspectRatio = drawRectangle.AspectRatio;
            CalculateImageUnderMouse(input);
        }
        private void CalculateImageUnderMouse(Input input)
        {
            imageUnderMouse = null;
            for (int i = collage.Images.Count - 1; i >= 0; i--)
            {
                Rectangle rec = collage.Images[i].GetRectangleInBoundary(drawRectangle.Rectangle);
                if (Utils.IsVectorInRotatedRectangle(input.MousePositionVector, rec, collage.Images[i].Rotation))
                {
                    imageUnderMouse = collage.Images[i];
                    break;
                }
            }
        }

        public Image ImageUnderMouse
        {
            get { return imageUnderMouse; }
        }
    }
}
