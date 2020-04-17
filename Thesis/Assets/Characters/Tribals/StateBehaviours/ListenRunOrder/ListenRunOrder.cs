namespace SJ.GameEntities.Characters.Tribals.States
{
    public class ListenRunOrder : TribalStateBehaviour
    {
        protected override bool OnHandleEvent(Character.Order ev)
        {
            if (ev.type == Character.OrderType.Run)
            {
                Trigger(Tribal.Trigger.Run);
                return true;
            }

            return false;
        }
    }
}


