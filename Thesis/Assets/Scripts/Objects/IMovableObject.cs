using SJ.Tools;

namespace SJ.GameEntities
{
    public interface IMovableObject
    {
        void Connect(IRigidbody2D rigidbody2D);
        void Disconnect(IRigidbody2D rigidbody2D);
    }
}
