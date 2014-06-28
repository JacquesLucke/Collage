using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class ZoomOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public ZoomOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.ScrollWheelDifference != 0 && (dataAccess.Input.IsStrg || editData.SelectedImages.Count == 0);
        }

        public bool Start()
        {
            editData.DrawRectangle.Zoom(-dataAccess.Input.ScrollWheelDifference / 10f, dataAccess.Input.MousePositionVector);
            return false;
        }
    }
}
