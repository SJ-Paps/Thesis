using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class WalkingState : TribalState
    {
        [SerializeField]
        private float velocityConstraintPercentage = 60;

        private int velocityConstraintId;

        protected override void OnEnter()
        {
            base.OnEnter();

            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.WalkAnimatorTrigger);

            velocityConstraintId = Owner.MaxMovementVelocity.AddPercentageConstraint(velocityConstraintPercentage);

        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();

            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.WalkAnimatorTrigger);

            Owner.MaxMovementVelocity.RemovePercentageConstraint(velocityConstraintId);
        }
    }
}