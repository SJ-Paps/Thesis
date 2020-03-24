namespace SJ.GameEntities.Characters.Tribals.States
{
    public class ChoiceFromLastTriggerState : TribalState
    {
        protected override void OnEnter()
        {
            var triggerToNextState = Blackboard.GetItemOf<Tribal.Trigger>(Tribal.BlackboardKeys.LastTrigger);

            StateMachine.Trigger(triggerToNextState);
        }
    }
}