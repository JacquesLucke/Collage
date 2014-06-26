using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    class MoveOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public MoveOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.IsMiddleButtonDown;
        }

        public bool Start()
        {
            editData.DrawRectangle.Move(dataAccess.Input.MouseDifferenceVector);
            return false;
        }

    }
}
