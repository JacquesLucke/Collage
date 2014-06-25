using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public interface IState
    {
        void Start();
        void Update();
        void Draw();
    }
}
