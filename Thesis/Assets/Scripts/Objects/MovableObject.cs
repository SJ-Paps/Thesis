using SJ.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.GameEntities
{
    [RequireComponent(typeof(SJSpringJoint2D))]
    [DisallowMultipleComponent]
    [SelectionBase]
    public class MovableObject : SJMonoBehaviour, IMovableObject
    {
        public event Action OnLinkBreak;

        private ISpringJoint2D joint;

        protected override void SJAwake()
        {
            base.SJAwake();

            joint = GetComponent<ISpringJoint2D>();
            joint.OnJointBreak += NotifyJointBreak;
        }

        public bool Connect(IRigidbody2D rigidbody2D)
        {
            if (joint.ConnectedBody == null)
            {
                joint.Connect(rigidbody2D);
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            joint.Disconnect();
        }

        private void NotifyJointBreak()
        {
            OnLinkBreak?.Invoke();
        }
    }
}
