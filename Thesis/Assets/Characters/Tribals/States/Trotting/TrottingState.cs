namespace SJ.GameEntities.Characters.Tribals.States
{
    public class TrottingState : TribalState
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.TrotAnimatorTrigger);
        }

        protected override void OnExit()
        {
            base.OnExit();

            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.TrotAnimatorTrigger);
        }
    }
}