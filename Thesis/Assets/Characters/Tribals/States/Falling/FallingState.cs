using SJ.Tools;
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
            if (IsOverVelocityDeadZone() && IsTouchingFloor())
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
    }
}