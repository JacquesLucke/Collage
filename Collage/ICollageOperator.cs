using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public interface ICollageOperator
    {
        void SetData(DataAccess dataAccess, CollageEditData editData);
        bool CanStart();

        // false means that the operator finished
        bool Start();
        bool Update();
    }
}
