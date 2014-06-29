
namespace Collage
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
