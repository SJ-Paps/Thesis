using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class ChoiceFromLastTriggerState : TribalSimpleState
    {
        protected override void OnEnter()
        {
            var triggerToNextState = Blackboard.GetItem<Tribal.Trigger>(Tribal.BlackboardKeys.LastTrigger);

            Trigger(triggerToNextState);
        }
    }
}