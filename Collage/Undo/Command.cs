using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage.Undo
{
    delegate object DoCommand(object doData);
    delegate void UndoCommand(object undoData);

    public class Command
    {
        DoCommand doCommand;
        UndoCommand undoCommand;

        object doData, undoData;

        public Command(DoCommand doCommand, UndoCommand undoCommand, object doData)
        {
            this.doCommand = doCommand;
            this.undoCommand = undoCommand;
            this.doData = doData;
        }

        public void Execute()
        {
            undoData = doCommand(doData);
        }
        public void Undo()
        {
            undoCommand(undoData);
        }
    }
}
