using SJ.Management;
using SJ.Tools;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingPullingStateBehaviour : TribalStateBehaviour, IFixedUpdateListener
    {
        [SerializeField]
        private float velocityDeadZone;

        private bool shouldMove;
        private FaceDirection moveDirection;
        private float moveForce;
        private bool isMovingByWill;

        public override void OnEnter()
        {
            moveDirection = Blackboard.GetItem<FaceDirection>(Tribal.BlackboardKeys.PullMoveDirection);

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
            return isMovingByWill == false || (IsOnVelocityDeadZone() && HasWallTooClose(moveDirection));
        }

        public override void OnExit()
        {
            shouldMove = false;
            isMovingByWill = false;

            Owner.UnsubscribeFromFixedUpdate(this);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch (ev.type)
            {
                case Character.OrderType.Move:

                    var nextDirection = ev.weight >= 0 ? FaceDirection.Right : FaceDirection.Left;

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

        public void Move(FaceDirection direction, float extraForceMultiplier = 1)
        {
            ApplyForceOnDirection(direction, Owner.MovementAcceleration * extraForceMultiplier);

            ClampVelocityIfIsOverLimit();
        }

        private void ApplyForceOnDirection(FaceDirection direction, float force)
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

        private bool IsOnVelocityDeadZone()
        {
            return Owner.RigidBody2D.velocity.x > velocityDeadZone * -1 && Owner.RigidBody2D.velocity.x < velocityDeadZone;
        }

        private bool HasWallTooClose(FaceDirection moveDirection)
        {
            int layerMask = Layers.Floor;

            int direction = (int)moveDirection;

            Bounds bounds = Owner.Collider.bounds;
            float widthExtents = 0.02f;
            float heightNegativeOffset = 0.1f;

            var frontPoint = new Vector2(bounds.center.x + ((bounds.extents.x + widthExtents) * direction), bounds.center.y);
            var size = new Vector2(widthExtents * 2, bounds.size.y - heightNegativeOffset);

            return Physics2D.OverlapBox(frontPoint, size, Owner.transform.eulerAngles.z, layerMask) != null;
        }

        private bool IsInPullMode()
        {
            return Blackboard.GetItem<bool>(Tribal.BlackboardKeys.PullMode);
        }
    }
}