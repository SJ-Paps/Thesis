namespace SJ.GameEntities.Characters.Tribals.States
{
    public class AliveState : TribalSimpleState
    {
        private bool isInWalkingMode;

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if(ev.type == Character.OrderType.Walk)
            {
                if (isInWalkingMode)
                {
                    isInWalkingMode = false;
                    Blackboard.SetItem(Tribal.BlackboardKeys.WalkMode, false);
                    Trigger(Tribal.Trigger.Trot);
                }
                else
                {
                    isInWalkingMode = true;
                    Blackboard.SetItem(Tribal.BlackboardKeys.WalkMode, true);
                    Trigger(Tribal.Trigger.Walk);
                }

                return true;
            }

            return false;
        }
    }
}