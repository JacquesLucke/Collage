using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class StateManager
    {
        IState currentState;

        public StateManager() { }

        public void SetCurrentState(IState state)
        {
            currentState = state;
            currentState.Start();
        }

        public void Update(GameTime time)
        {
            currentState.Update(time);
        }
        public void Draw(GameTime time)
        {
            currentState.Draw(time);
        }
    }
}
