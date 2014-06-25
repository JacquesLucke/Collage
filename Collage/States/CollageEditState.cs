using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage.States
{
    public class CollageEditState : IState
    {
        DataAccess dataAccess;

        public CollageEditState(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;
        }

        public void Start()
        {

        }

        public void Update()
        {
        }

        public void Draw()
        {
        }
    }
}
