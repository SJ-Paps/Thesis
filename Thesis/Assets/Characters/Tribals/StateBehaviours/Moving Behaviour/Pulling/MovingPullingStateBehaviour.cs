using SJ.Management;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingPullingStateBehaviour : TribalStateBehaviour, IFixedUpdateListener
    {
        [SerializeField]
        private float velocityDeadZone, maxMovementVelocityPercentageModifier;

        private bool shouldMove;
        private HorizontalDirection moveDirection;
        private float moveForce;
        private bool isMovingByWill;

        private int constraintId;

        public override void OnEnter()
        {
            moveDirection = Blackboard.GetItem<HorizontalDirection>(Tribal.BlackboardKeys.PullMoveDirection);

            Owner.SubscribeToFixedUpdate(this);

            isMovingByWill = true;

            constraintId = Owner.MaxMovementVelocity.AddPercentageConstraint(maxMovementVelocityPercentageModifier);
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
            shouldMove = false;
            isMovingByWill = false;

            Owner.MaxMovementVelocity.RemovePercentageConstraint(constraintId);

            Owner.UnsubscribeFromFixedUpdate(this);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch (ev.type)
            {
                case Character.OrderType.Move:

                    var nextDirection = ev.weight >= 0 ? HorizontalDirection.Right : HorizontalDirection.Left;

                    if (nextDirection != Owner.FacingDirection && IsInPullMode() == false)
                    {
                        Trigger(Tribal.Trigger.Stop);
                    }
                    else
                    {
                        moveDirection = nextDirection;
                        moveForce = Math.Abs(ev.weight);

                        shouldMove = true;
                        isMovingByWill = true;
                    }
                    return true;
            }

            return false;
        }

        public void DoFixedUpdate()
        {
            if (shouldMove)
                Move();

            shouldMove = false;
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

        private bool IsInPullMode()
        {
            return Blackboard.GetItem<bool>(Tribal.BlackboardKeys.PullMode);
        }
    }
}