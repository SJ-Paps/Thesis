using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class RunningState : TribalState
    {
        [SerializeField]
        private float velocityConstraintPercentage;

        private int velocityConstraintId;

        protected override void OnEnter()
        {
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.RunAnimatorTrigger);

            velocityConstraintId = Owner.MaxMovementVelocity.AddPercentageConstraint(velocityConstraintPercentage);
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.TrotAnimatorTrigger);

            Owner.MaxMovementVelocity.RemovePercentageConstraint(velocityConstraintId);
        }
    }
}