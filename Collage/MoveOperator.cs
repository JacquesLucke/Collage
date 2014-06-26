using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    class MoveOperator : IUpdateableCollageOperator
    {
        DataAccess dataAccess;
        CollageEditData editData;
        Vector2 totalMovement;

        public MoveOperator() { }

        public void SetData(DataAccess dataAccess, CollageEditData editData)
        {
            this.dataAccess = dataAccess;
            this.editData = editData;
        }
        public bool CanStart()
        {
            return dataAccess.Input.IsMiddleButtonDown;
        }

        public bool Start()
        {
            totalMovement = Vector2.Zero;
            return true;
        }

        public bool Update()
        {
            if (dataAccess.Input.IsMiddleButtonDown)
            {
                editData.DrawRectangle.Move(dataAccess.Input.MouseDifferenceVector);
                totalMovement += dataAccess.Input.MouseDifferenceVector;
            }
            else
            {
                Command command = new Command(ExecuteMove, ExecuteMove, totalMovement);
                command.SetUndoData(-totalMovement);
                editData.UndoManager.AddCommand(command);
            }

            return dataAccess.Input.IsMiddleButtonDown;
        }

        public object ExecuteMove(object distance)
        {
            editData.DrawRectangle.Move((Vector2)distance);
            return -(Vector2)distance;
        }
    }
}
