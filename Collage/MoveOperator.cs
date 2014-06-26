﻿using Microsoft.Xna.Framework;
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
            return true;
        }

        public bool Update()
        {
            if (dataAccess.Input.IsMiddleButtonDown)
            {
                Command command = new Command(ExecuteMove, ExecuteMove, dataAccess.Input.MouseDifferenceVector);
                dataAccess.UndoManager.ExecuteAndAddCommand(command);
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
