﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage.Undo
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
