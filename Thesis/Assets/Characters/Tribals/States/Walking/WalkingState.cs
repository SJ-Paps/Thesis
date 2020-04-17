using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class WalkingState : TribalSimpleState
    {
        [SerializeField]
        private float velocityConstraintPercentage;

        private int velocityConstraintId;

        protected override void OnEnter()
        {
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Walk);

            velocityConstraintId = Owner.MaxMovementVelocity.AddConstraint(velocityConstraintPercentage);
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Walk);

            Owner.MaxMovementVelocity.RemoveConstraint(velocityConstraintId);
        }
    }
}