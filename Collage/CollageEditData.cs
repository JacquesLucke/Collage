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

        public CollageEditData(CollageObject collage, MoveableRectangle drawRectangle)
        {
            this.collage = collage;
            this.drawRectangle = drawRectangle;
        }

        public CollageObject Collage
        {
            get { return collage; }
        }
        public MoveableRectangle DrawRectangle
        {
            get { return drawRectangle; }
        }
    }
}
