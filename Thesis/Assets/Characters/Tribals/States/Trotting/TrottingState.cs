namespace SJ.GameEntities.Characters.Tribals.States
{
    public class TrottingState : TribalSimpleState
    {
        protected override void OnEnter()
        {
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Trot);
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Trot);
        }
    }
}