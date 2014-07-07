using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public interface IParallelOperatorActivator : IOperatorActivator
    {
        void SetSensitivity(bool sensitivity);
    }
}
