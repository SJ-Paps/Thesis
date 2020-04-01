using SJ.Management;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingState : TribalState, IFixedUpdateListener
    {
        private bool shouldMove;
        private FaceDirection moveDirection;
        private float moveForce;
        private bool isMovingByWill;

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);

            isMovingByWill = true;
        }

        protected override void OnUpdate()
        {
            if (isMovingByWill == false)
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
    }
}