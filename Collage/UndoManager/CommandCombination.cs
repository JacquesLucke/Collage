using System.Collections.Generic;

namespace Collage
{
    public class CommandCombination : ICommand
    {
        List<ICommand> commands = new List<ICommand>();

        public CommandCombination() { }

        public void ExecuteAndAddToCombination(ICommand command)
        {
            command.Execute();
            AddToCombination(command);
        }

        public void AddToCombination(ICommand command)
        {
            commands.Add(command);
        }

        public void Execute()
        {
            for(int i = 0; i<commands.Count; i++)
            {
                commands[i].Execute();
            }
        }

        public void Undo()
        {
            for(int i = commands.Count - 1; i >= 0; i--)
            {
                commands[i].Undo();
            }
        }
    }
}
