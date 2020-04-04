using SJ.Tools;
using System;

namespace SJ.GameEntities
{
    public interface IMovableObject
    {
        event Action OnLinkBreak;

        bool Connect(IRigidbody2D rigidbody2D);
        void Disconnect();
    }
}
