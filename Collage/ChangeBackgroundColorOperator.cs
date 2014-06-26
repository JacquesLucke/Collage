using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    class ChangeBackgroundColorOperator : ICollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;

        public ChangeBackgroundColorOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.IsKeyReleased(Keys.B);
        }

        public bool Start()
        {
            Color color = Color.FromNonPremultiplied(dataAccess.Random.Next(150 ) + 70, dataAccess.Random.Next(150) + 70, dataAccess.Random.Next(150) + 70, 255);
            Command command = new Command(ExecuteColorChange, ExecuteColorChange, color, "Change Background Color");
            editData.UndoManager.ExecuteAndAddCommand(command);
            return false;
        }

        public object ExecuteColorChange(object color)
        {
            Color c = editData.Collage.BackgroundColor;
            editData.Collage.BackgroundColor = (Color)color;
            return c;
        }
       
    }
}
