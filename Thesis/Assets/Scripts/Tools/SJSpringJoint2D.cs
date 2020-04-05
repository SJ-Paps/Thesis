using System;
using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(SpringJoint2D))]
    public class SJSpringJoint2D : SJMonoBehaviour, ISpringJoint2D
    {
        [SerializeField]
        private float breakForce;

        private SpringJoint2D innerJoint;

        private SpringJoint2D InnerJoint => innerJoint ?? (innerJoint = GetComponent<SpringJoint2D>());

        public event Action OnJointBreak;
        public event Action<IRigidbody2D> OnConnectedBodyChanged;
        public event Action OnDisconnected;

        public IRigidbody2D ConnectedBody { get; private set; }

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
                InnerJoint.enabled = value;
            }
        }

        public float BreakForce { get => breakForce; set => breakForce = value; }

        public void Connect(IRigidbody2D rigidbody2D)
        {
            if (rigidbody2D == null)
                throw new ArgumentNullException(nameof(rigidbody2D));

            ConnectedBody = rigidbody2D;
            InnerJoint.connectedBody = rigidbody2D.InnerComponent;
            Enabled = true;

            OnConnectedBodyChanged?.Invoke(ConnectedBody);
        }

        public void Disconnect()
        {
            ConnectedBody = null;
            InnerJoint.connectedBody = null;
            Enabled = false;

            OnDisconnected?.Invoke();
        }

        protected override void SJFixedUpdate()
        {
            var reactionForce = InnerJoint.reactionForce.magnitude;

            if (reactionForce > breakForce)
            {
                Logger.LogConsole("BREAK SPRING JOINT FORCE: " + reactionForce);
                Disconnect();
                OnJointBreak?.Invoke();
            }
        }
    }
}