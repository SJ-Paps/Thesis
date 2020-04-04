using System;

namespace SJ.Tools
{
    public interface ISpringJoint2D
    {
        event Action OnJointBreak;
        event Action<IRigidbody2D> OnConnectedBodyChanged;
        event Action OnDisconnected;

        IRigidbody2D ConnectedBody { get; }
        bool Enabled { get; set; }

        void Connect(IRigidbody2D rigidbody2D);
        void Disconnect();
    }
}