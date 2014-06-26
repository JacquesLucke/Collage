using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public delegate object DoCommand(object doData);
    public delegate object UndoCommand(object undoData);

    public class Command : ICommand
    {
        DoCommand doCommand;
        UndoCommand undoCommand;

        object doData, undoData;
        bool receiveUndoData = true;

        string name;

        public Command(DoCommand doCommand, UndoCommand undoCommand, object doData, string name)
        {
            this.doCommand = doCommand;
            this.undoCommand = undoCommand;
            this.doData = doData;
            this.name = name;
        }

        public void Execute()
        {
            object data = doCommand(doData);
            if (receiveUndoData) undoData = data;
        }
        public void Undo()
        {
            undoCommand(undoData);
        }

        public bool ReceiveUndoData
        {
            get { return receiveUndoData; }
            set { receiveUndoData = value; }
        }
        public void SetDoData(object doData)
        {
            this.doData = doData;
        }
        public void SetUndoData(object undoData)
        {
            this.undoData = undoData;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
