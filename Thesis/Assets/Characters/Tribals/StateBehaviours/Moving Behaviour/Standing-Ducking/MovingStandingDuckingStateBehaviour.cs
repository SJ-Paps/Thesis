using SJ.Management;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingStandingDuckingStateBehaviour : TribalStateBehaviour, IFixedUpdateListener
    {
        [SerializeField]
        private float velocityDeadZone;

        private HorizontalDirection moveDirection;
        private float moveForce;
        private bool isMovingByWill;

        public override void OnEnter()
        {
            moveDirection = Owner.FacingDirection;

            Owner.SubscribeToFixedUpdate(this);

            isMovingByWill = true;
        }

        public override void OnUpdate()
        {
            if (ShouldStop())
                Trigger(Tribal.Trigger.Stop);

            isMovingByWill = false;
        }

        private bool ShouldStop()
        {
            return isMovingByWill == false || (Owner.IsInsideVelocityDeadZoneOnHorizontalAxis(velocityDeadZone) && Owner.IsTouchingWall(moveDirection));
        }

        public override void OnExit()
        {
            isMovingByWill = false;

            Owner.UnsubscribeFromFixedUpdate(this);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch (ev.type)
            {
                case Character.OrderType.Move:

                    moveDirection = ev.weight >= 0 ? HorizontalDirection.Right : HorizontalDirection.Left;
                    moveForce = Math.Abs(ev.weight);

                    isMovingByWill = true;
                    return true;

                case Character.OrderType.Run:

                    Trigger(Tribal.Trigger.Run);
                    return true;
            }

            return false;
        }

        public void DoFixedUpdate()
        {
            Owner.Face(moveDirection);
            Move();
        }

        private void Move()
        {
            Move(moveDirection, moveForce);
        }

        public void Move(HorizontalDirection direction, float extraForceMultiplier = 1)
        {
            ApplyForceOnDirection(direction, Owner.MovementAcceleration * extraForceMultiplier);

            ClampVelocityIfIsOverLimit();
        }

        private void ApplyForceOnDirection(HorizontalDirection direction, float force)
        {
            Owner.RigidBody2D.AddForce(new Vector2((int)direction * force, 0), ForceMode2D.Impulse);
        }

        private void ClampVelocityIfIsOverLimit()
        {
            var velocity = Owner.RigidBody2D.velocity;
            float maxMovementVelocity = Owner.MaxMovementVelocity;

            if (velocity.x > maxMovementVelocity)
                Owner.RigidBody2D.velocity = new Vector2(maxMovementVelocity, velocity.y);
            else if (velocity.x < maxMovementVelocity * -1)
                Owner.RigidBody2D.velocity = new Vector2(maxMovementVelocity * -1, velocity.y);
        }
    }
}