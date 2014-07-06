
namespace Collage
{
    public interface ICollageOperator
    {
        void SetData(DataAccess dataAccess, CollageEditData editData);

        // false means that the operator finished
        bool Start();
    }
}
