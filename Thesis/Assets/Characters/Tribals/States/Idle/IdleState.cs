namespace SJ.GameEntities.Characters.Tribals.States
{
    public class IdleState : TribalState
    {
        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch(ev.type)
            {
                case Character.OrderType.Move:
                    Trigger(Tribal.Trigger.Trot);
                    return true;
            }

            return false;
        }
    }
}
