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

            if (IsBelowVelocityDeadZone() && IsTouchingFloor() == false)
                Trigger(Tribal.Trigger.Fall);
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
            if (order.type == Character.OrderType.Jump && groundingTimer.Active == false && HasCeilingTooClose() == false)
            {
                Trigger(Tribal.Trigger.Jump);
                return true;
            }

            return false;
        }

        private bool IsTouchingFloor()
        {
            int layerMask = Layers.Walkable;

            Bounds bounds = Owner.Collider.bounds;
            float heightExtents = 0.05f;
            float widthNegativeOffset = 0.2f;

            var lowerPoint = new Vector2(bounds.center.x, bounds.center.y - bounds.extents.y - heightExtents);
            var size = new Vector2(bounds.size.x - widthNegativeOffset, heightExtents * 2);

            return Physics2D.OverlapBox(lowerPoint, size, Owner.transform.eulerAngles.z, layerMask) != null;
        }

        private bool HasCeilingTooClose()
        {
            int layerMask = Layers.Walkable;

            Bounds bounds = Owner.Collider.bounds;
            float heightExtents = 0.05f;
            float widthNegativeOffset = 0.1f;

            var upperPoint = new Vector2(bounds.center.x, bounds.center.y + bounds.extents.y + heightExtents);
            var size = new Vector2(bounds.size.x - widthNegativeOffset, heightExtents * 2);

            return Physics2D.OverlapBox(upperPoint, size, Owner.transform.eulerAngles.z, layerMask) != null;
        }
    }
}