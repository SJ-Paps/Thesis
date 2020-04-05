using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class StandingState : TribalSimpleState
    {
        private bool hasReceivedPullOrder;

        protected override void Initialize()
        {
            Blackboard.SetItem(Tribal.BlackboardKeys.PullMode, false);
        }

        protected override void OnUpdate()
        {
            if(hasReceivedPullOrder == false)
                Blackboard.SetItem(Tribal.BlackboardKeys.PullMode, false);

            hasReceivedPullOrder = false;
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if(ev.type == Character.OrderType.Pull)
            {
                if (IsInPullMode() == false)
                    SearchMovableObjects();

                hasReceivedPullOrder = true;
                return true;
            }

            return false;
        }

        protected override void OnExit()
        {
            Blackboard.SetItem(Tribal.BlackboardKeys.PullMode, false);
        }

        private void SearchMovableObjects()
        {
            var movableObject = Owner.SearchMovableObjects();

            if (movableObject != null)
            {
                Blackboard.SetItem(Tribal.BlackboardKeys.MovableObject, movableObject);
                Blackboard.SetItem(Tribal.BlackboardKeys.PullMode, true);
                Trigger(Tribal.Trigger.Pull);
            }
        }

        private bool IsInPullMode()
        {
            return Blackboard.GetItem<bool>(Tribal.BlackboardKeys.PullMode);
        }
    }
}
