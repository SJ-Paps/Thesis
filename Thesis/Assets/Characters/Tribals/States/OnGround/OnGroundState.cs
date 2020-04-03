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

            if (IsBelowVelocityDeadZone() && IsOnFloor() == false)
            {
                Trigger(Tribal.Trigger.Fall);
            }
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

        private bool IsOnFloor()
        {
            int layerMask = Reg.walkableLayerMask;

            Bounds bounds = Owner.Collider.bounds;
            float height = 0.05f;
            float checkFloorNegativeOffsetX = -0.1f;

            Vector2 leftPoint = new Vector2(bounds.center.x - bounds.extents.x - checkFloorNegativeOffsetX, bounds.center.y - bounds.extents.y);
            Vector2 rightPoint = new Vector2(bounds.center.x + bounds.extents.x + checkFloorNegativeOffsetX, bounds.center.y - bounds.extents.y);

            Logger.DrawLine(leftPoint, new Vector3(rightPoint.x, rightPoint.y - height), Color.green);
            Logger.DrawLine(rightPoint, new Vector3(leftPoint.x, leftPoint.y - height), Color.green);

            return Physics2D.Linecast(leftPoint, new Vector2(rightPoint.x, rightPoint.y - height), layerMask) ||
                Physics2D.Linecast(rightPoint, new Vector2(leftPoint.x, leftPoint.y - height), layerMask);
        }

        private bool HasCeilingTooClose()
        {
            int layerMask = Reg.walkableLayerMask;

            Bounds bounds = Owner.Collider.bounds;
            float height = 0.05f;
            float checkFloorNegativeOffsetX = -0.1f;

            Vector2 leftPoint = new Vector2(bounds.center.x - bounds.extents.x - checkFloorNegativeOffsetX, bounds.center.y + bounds.extents.y);
            Vector2 rightPoint = new Vector2(bounds.center.x + bounds.extents.x + checkFloorNegativeOffsetX, bounds.center.y + bounds.extents.y);

            Logger.DrawLine(leftPoint, new Vector3(rightPoint.x, rightPoint.y - height), Color.green);
            Logger.DrawLine(rightPoint, new Vector3(leftPoint.x, leftPoint.y - height), Color.green);

            return Physics2D.Linecast(leftPoint, new Vector2(rightPoint.x, rightPoint.y + height), layerMask) ||
                Physics2D.Linecast(rightPoint, new Vector2(leftPoint.x, leftPoint.y + height), layerMask);
        }
    }
}