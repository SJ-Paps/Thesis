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
                {
                    Blackboard.SetItem(Tribal.BlackboardKeys.PullMode, true);
                    SearchMovableObjects();
                }

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
            float checkMovableObjectDistanceX = 0.2f;

            int facingDirection = (int)Owner.FacingDirection;

            var bounds = Owner.Collider.bounds;

            var center = new Vector2(bounds.center.x + (bounds.extents.x * facingDirection), bounds.center.y - bounds.extents.y / 3);
            var size = new Vector2(checkMovableObjectDistanceX, bounds.extents.y);

            var movableObject = SJUtil.FindMovableObject(center, size, Owner.transform.eulerAngles.z);

            if (movableObject != null)
            {
                Blackboard.SetItem(Tribal.BlackboardKeys.MovableObject, movableObject);
                
                Trigger(Tribal.Trigger.Pull);
            }
        }

        private bool IsInPullMode()
        {
            return Blackboard.GetItem<bool>(Tribal.BlackboardKeys.PullMode);
        }
    }
}
