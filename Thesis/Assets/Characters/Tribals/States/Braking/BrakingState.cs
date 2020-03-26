using SJ.Management;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class BrakingState : TribalState, IFixedUpdateListener
    {
        [SerializeField]
        private float brakeForce;

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);
        }

        protected override void OnUpdate()
        {
            if (Owner.CurrentVelocity.x == 0)
                Trigger(Tribal.Trigger.Stop);
        }

        protected override void OnExit()
        {
            Owner.UnsubscribeFromFixedUpdate(this);
        }

        public void DoFixedUpdate()
        {
            Owner.Brake(brakeForce);
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