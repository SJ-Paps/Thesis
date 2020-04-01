using SJ.Management;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingState : TribalState, IFixedUpdateListener
    {
        [SerializeField]
        private float velocityDeadZone;

        private bool shouldMove;
        private FaceDirection moveDirection;
        private float moveForce;
        private bool isMovingByWill;

        protected override void OnEnter()
        {
            moveDirection = Owner.FacingDirection;

            Owner.SubscribeToFixedUpdate(this);

            isMovingByWill = true;
        }

        protected override void OnUpdate()
        {
            if (isMovingByWill == false || (IsOnVelocityDeadZone() && HasWallTooClose(moveDirection)))
                Trigger(Tribal.Trigger.Stop);
                
            isMovingByWill = false;
        }

        protected override void OnExit()
        {
            shouldMove = false;
            isMovingByWill = false;

            Owner.UnsubscribeFromFixedUpdate(this);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch(ev.type)
            {
                case Character.OrderType.Move:
                    moveDirection = ev.weight >= 0 ? FaceDirection.Right : FaceDirection.Left;
                    moveForce = Math.Abs(ev.weight);

                    shouldMove = true;
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
            if (shouldMove)
            {
                Owner.Face(moveDirection);
                Move();
            }

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

        private bool HasWallTooClose(FaceDirection faceDirection)
        {
            int layerMask = Reg.walkableLayerMask;

            Bounds bounds = Owner.Collider.bounds;
            float width = 0.05f;
            float offset = -0.01f;

            int direction = (int)faceDirection;

            var upperPoint = new Vector2(bounds.center.x + ((bounds.extents.x - offset) * direction), bounds.center.y + bounds.extents.y);
            var lowerPoint = new Vector2(bounds.center.x + ((bounds.extents.x - offset) * direction), bounds.center.y - bounds.extents.y);

            Logger.DrawLine(upperPoint, new Vector2(upperPoint.x + (width * direction), lowerPoint.y), Color.green);
            Logger.DrawLine(lowerPoint, new Vector2(lowerPoint.x + (width * direction), upperPoint.y), Color.green);

            return Physics2D.Linecast(upperPoint, new Vector2(upperPoint.x + (width * direction), lowerPoint.y), layerMask) ||
                Physics2D.Linecast(lowerPoint, new Vector2(lowerPoint.x + (width * direction), upperPoint.y), layerMask);
        }
    }
}