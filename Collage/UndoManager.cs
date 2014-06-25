using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage.Undo
{
    public class UndoManager
    {
        List<ICommand> commands = new List<ICommand>();
        int currentIndex = -1;

        public UndoManager() { }

        public void ExecuteAndAddCommand(ICommand command)
        {
            command.Execute();
            AddCommand(command);
        }

        public void AddCommand(ICommand command)
        {
            // overwrite old commands
            if (currentIndex < commands.Count - 1)
            {
                int removeCount = commands.Count - currentIndex - 1;
                for (int i = 0; i < removeCount; i++) commands.RemoveAt(commands.Count - 1);
            }
            commands.Add(command);
            currentIndex++;
        }

        public void Undo()
        {
            if (currentIndex >= 0)
            {
                commands[currentIndex].Undo();
                currentIndex--;
            }
        }
    }
}
