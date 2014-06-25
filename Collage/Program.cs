#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Collage
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var main = new Main())
                main.Run();
        }
    }
#endif
}
