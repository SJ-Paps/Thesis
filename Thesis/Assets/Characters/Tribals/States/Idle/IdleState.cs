namespace SJ.GameEntities.Characters.Tribals.States
{
    public class IdleState : TribalState
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.IdleAnimatorTrigger);
        }

        protected override void OnExit()
        {
            base.OnExit();

            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.IdleAnimatorTrigger);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch(ev.type)
            {
                case Character.OrderType.Move:
                    Blackboard.SetItem(Tribal.BlackboardKeys.MovingInitialDirectionAndForce, ev.weight);
                    Trigger(Tribal.Trigger.Trot);
                    return true;
            }

            return false;
        }
    }
}
