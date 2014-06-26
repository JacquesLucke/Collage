using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    interface IUpdateableCollageOperator : ICollageOperator
    {
        bool Update();
    }
}
