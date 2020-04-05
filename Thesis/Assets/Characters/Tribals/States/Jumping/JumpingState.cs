using SJ.Management;
using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class JumpingState : TribalSimpleState, IFixedUpdateListener
    {
        [SerializeField]
        private float jumpForce, velocityDeadZone;

        private bool keepsReceivingJumpOrders;

        private float accumulatedForce;

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);

            keepsReceivingJumpOrders = true;

            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Jump);

            accumulatedForce = 0;
        }

        protected override void OnUpdate()
        {
            if (ShouldStop())
                FinishJump();

            keepsReceivingJumpOrders = false;
        }

        private bool ShouldStop()
        {
            return keepsReceivingJumpOrders == false || IsOverMaxForce() || (IsBelowVelocityDeadZone() && Owner.IsTouchingCeilingWalkable());
        }

        private bool IsOverMaxForce()
        {
            return accumulatedForce >= Owner.JumpMaxForce;
        }

        private void FinishJump()
        {
            Trigger(Tribal.Trigger.Fall);
        }

        protected override void OnExit()
        {
            keepsReceivingJumpOrders = false;

            Owner.UnsubscribeFromFixedUpdate(this);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if(ev.type == Character.OrderType.Jump)
            {
                keepsReceivingJumpOrders = true;

                return true;
            }

            return false;
        }

        public void DoFixedUpdate()
        {
            Jump();
        }

        private void Jump()
        {
            var force = Owner.JumpAcceleration * jumpForce;

            accumulatedForce += force;

            Owner.RigidBody2D.AddForce(force * Vector2.up, ForceMode2D.Impulse);
        }

        private bool IsBelowVelocityDeadZone()
        {
            return Owner.RigidBody2D.Velocity.y < velocityDeadZone;
        }
    }
}