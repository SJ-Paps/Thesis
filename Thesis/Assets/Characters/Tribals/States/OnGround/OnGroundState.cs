using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class OnGroundState : TribalSimpleState
    {
        [SerializeField]
        private float velocityDeadZone, intervalBetweenGroundAndNextJump;

        private SyncTimer groundingTimer = new SyncTimer();

        protected override void Initialize()
        {
            groundingTimer.Interval = intervalBetweenGroundAndNextJump;
        }

        protected override void OnEnter()
        {
            groundingTimer.Start();

            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Ground);
        }

        protected override void OnUpdate()
        {
            groundingTimer.Update(Time.deltaTime);

            if (IsFalling())
                Trigger(Tribal.Trigger.Fall);
        }

        private bool IsFalling()
        {
            return IsBelowVelocityDeadZone() && Owner.IsTouchingFloorWalkable() == false;
        }

        private bool IsBelowVelocityDeadZone()
        {
            return Owner.RigidBody2D.velocity.y < velocityDeadZone;
        }

        protected override void OnExit()
        {
            groundingTimer.Stop();

            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Ground);
        }

        protected override bool OnHandleEvent(Character.Order order)
        {
            if (order.type == Character.OrderType.Jump && 
                groundingTimer.Active == false && 
                Owner.IsTouchingCeilingWalkable() == false)
            {
                Trigger(Tribal.Trigger.Jump);
                return true;
            }

            return false;
        }
    }
}