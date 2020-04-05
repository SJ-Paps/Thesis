using System;
using UnityEngine;

namespace SJ.GameEntities.Characters
{
    public abstract class Character : SaveableGameEntity, ICharacter
    {
        public readonly struct Order
        {
            public readonly OrderType type;
            public readonly float weight;

            public Order(OrderType type, float weight)
            {
                this.type = type;
                this.weight = weight;
            }
        }

        public enum OrderType
        {
            Jump,
            Hide,
            GetUp,
            GetDown,
            Move,
            Walk,
            Run,
            Activate,
            Pull,
        }

        public event Action<Order> OnOrderReceived;

        public FaceDirection FacingDirection
        {
            get
            {
                if (transform.right.x > 0)
                    return FaceDirection.Right;
                else
                    return FaceDirection.Left;
            }
        }

        public bool BlockFacing { get; set; }

        public void SendOrder(Order order)
        {
            OnOrderReceived?.Invoke(order);

            OnSendOrder(order);
        }

        protected abstract void OnSendOrder(Order order);

        public void Face(FaceDirection direction)
        {
            if (BlockFacing || direction == FacingDirection)
                return;

            transform.Rotate(Vector3.up, 180);
        }
    }
}