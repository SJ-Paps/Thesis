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
            if (IsOverVelocityDeadZone() && Owner.IsTouchingFloorWalkable())
                Trigger(Tribal.Trigger.Ground);
        }

        private bool IsOverVelocityDeadZone()
        {
            return Owner.RigidBody2D.Velocity.y > velocityDeadZone;
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Fall);
        }
    }
}