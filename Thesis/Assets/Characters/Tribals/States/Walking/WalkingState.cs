﻿using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class WalkingState : TribalState
    {
        [SerializeField]
        private float velocityConstraintPercentage;

        private int velocityConstraintId;

        protected override void OnEnter()
        {
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.WalkAnimatorTrigger);

            velocityConstraintId = Owner.MaxMovementVelocity.AddPercentageConstraint(velocityConstraintPercentage);
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.WalkAnimatorTrigger);

            Owner.MaxMovementVelocity.RemovePercentageConstraint(velocityConstraintId);
        }
    }
}