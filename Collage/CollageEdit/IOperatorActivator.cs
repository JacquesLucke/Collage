using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public interface IOperatorActivator
    {
        List<ICollageOperator> GetActivatedOperators();
    }
}
