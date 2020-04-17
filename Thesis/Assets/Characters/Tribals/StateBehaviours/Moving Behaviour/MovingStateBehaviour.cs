using SJ.Management;
using SJ.Tools;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingStateBehaviour : TribalStateBehaviour, IFixedUpdateListener
    {
        [SerializeField]
        private float velocityDeadZone;

        private HorizontalDirection moveDirection;
        private float moveForce;
        private bool isMovingByWill;

        public override void OnEnter()
        {
            moveDirection = Blackboard.GetItem<HorizontalDirection>(Tribal.BlackboardKeys.MoveDirection);

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

                    var desiredDirection = ev.weight >= 0 ? HorizontalDirection.Right : HorizontalDirection.Left;
                    
                    if (desiredDirection != moveDirection)
                    {
                        Trigger(Tribal.Trigger.Stop);
                    }
                    else
                    {
                        moveForce = Math.Abs(ev.weight);

                        isMovingByWill = true;
                    }
                    
                    return true;
            }

            return false;
        }

        public void DoFixedUpdate()
        {
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
            var velocity = Owner.RigidBody2D.Velocity;
            float maxMovementVelocity = Owner.MaxMovementVelocity;

            if (velocity.x > maxMovementVelocity)
                Owner.RigidBody2D.Velocity = new Vector2(maxMovementVelocity, velocity.y);
            else if (velocity.x < maxMovementVelocity * -1)
                Owner.RigidBody2D.Velocity = new Vector2(maxMovementVelocity * -1, velocity.y);
        }
    }
}