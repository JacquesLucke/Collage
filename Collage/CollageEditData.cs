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

        public CollageEditData(CollageObject collage, MoveableRectangle drawRectangle, UndoManager undoManager)
        {
            this.collage = collage;
            this.drawRectangle = drawRectangle;
            this.undoManager = undoManager;
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
    }
}
