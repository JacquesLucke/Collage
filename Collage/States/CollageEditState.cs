using Collage.Undo;
using Microsoft.Xna.Framework;
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
        Color lastColor = Color.CornflowerBlue;
        Color drawColor = Color.CornflowerBlue;

        public CollageEditState(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;
        }

        object SetColor(object color)
        {
            drawColor = (Color)color;
            return lastColor;
        }
        object UndoSetColor(object color)
        {
            drawColor = (Color)color;
            lastColor = drawColor;
            return null;
        }

        public void Start()
        {

        }

        public void Update()
        {
            Input input = dataAccess.Input;
            if(input.IsKeyReleased(Keys.Right))
            {
                Color color = Color.FromNonPremultiplied(dataAccess.Random.Next(255), dataAccess.Random.Next(255), dataAccess.Random.Next(255), 255);
                Command command = new Command(SetColor, UndoSetColor, color);
                dataAccess.UndoManager.ExecuteAndAddCommand(command);
                lastColor = color;
            }
            if (input.IsKeyReleased(Keys.Left))
            {
                dataAccess.UndoManager.Undo();
            }
        }

        public void Draw()
        {
            dataAccess.GraphicsDevice.Clear(drawColor);
        }
    }
}
