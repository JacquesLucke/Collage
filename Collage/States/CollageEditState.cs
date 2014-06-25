using Collage.Undo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            Input input = dataAccess.Input;
        }

        public void Draw()
        {
        }
    }
}
