using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class FallingState : TribalSimpleState
    {
        [SerializeField]
        private float velocityDeadZone;

        protected override void OnEnter()
        {
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Fall);
        }

        protected override void OnUpdate()
        {
            if (IsOverVelocityDeadZone() && IsOnFloor())
                Trigger(Tribal.Trigger.Ground);
        }

        private bool IsOverVelocityDeadZone()
        {
            return Owner.RigidBody2D.velocity.y > velocityDeadZone;
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Fall);
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
    }
}