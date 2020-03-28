namespace SJ.GameEntities.Characters.Tribals.States
{
    public class ChoiceMovingState : TribalState
    {
        protected override void OnEnter()
        {
            Blackboard.TryGetItemOf(Tribal.BlackboardKeys.WalkMode, out bool shouldWalk);

            if (shouldWalk)
                Trigger(Tribal.Trigger.Walk);
            else
                Trigger(Tribal.Trigger.Trot);
        }
    }
}