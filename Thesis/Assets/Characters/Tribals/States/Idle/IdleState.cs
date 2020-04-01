namespace SJ.GameEntities.Characters.Tribals.States
{
    public class IdleState : TribalState
    {
        protected override void OnEnter()
        {
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Idle);
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Idle);
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
