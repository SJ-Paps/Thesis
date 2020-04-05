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
        private bool jumpOnce;

        private SyncTimer jumpTimer = new SyncTimer();

        protected override void Initialize()
        {
            jumpTimer.OnTick += _ => FinishJump();
        }

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);

            keepsReceivingJumpOrders = true;

            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Jump);

            jumpTimer.Start();
        }

        protected override void OnUpdate()
        {
            jumpTimer.Interval = Owner.JumpMaxTime;
            jumpTimer.Update(Time.deltaTime);

            if (keepsReceivingJumpOrders == false || (IsBelowVelocityDeadZone() && Owner.IsTouchingCeilingWalkable()))
                FinishJump();

            keepsReceivingJumpOrders = false;
        }

        private void FinishJump()
        {
            Trigger(Tribal.Trigger.Fall);
        }

        protected override void OnExit()
        {
            keepsReceivingJumpOrders = false;
            jumpOnce = false;

            Owner.UnsubscribeFromFixedUpdate(this);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if(ev.type == Character.OrderType.Jump)
            {
                jumpOnce = true;
                keepsReceivingJumpOrders = true;

                return true;
            }

            return false;
        }

        public void DoFixedUpdate()
        {
            if (jumpOnce)
            {
                Jump();
            }

            jumpOnce = false;
        }

        private void Jump()
        {
            Owner.RigidBody2D.AddForce(Owner.JumpAcceleration * jumpForce * Vector2.up, ForceMode2D.Impulse);
        }

        private bool IsBelowVelocityDeadZone()
        {
            return Owner.RigidBody2D.velocity.y < velocityDeadZone;
        }
    }
}