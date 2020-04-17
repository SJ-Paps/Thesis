using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class IdlePullingState : TribalSimpleState
    {
        private bool isAboutToMove;

        protected override void OnEnter()
        {
            Debug.Log("ENTER IDLE PULLING");
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Idle);
        }

        protected override void OnUpdate()
        {
            if (isAboutToMove == false && IsInPullMode() == false)
                Trigger(Tribal.Trigger.Release);

            isAboutToMove = false;
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Idle);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if (ev.type == Character.OrderType.Move)
            {
                HorizontalDirection desiredDirection = default;

                if (ev.weight > 0)
                    desiredDirection = HorizontalDirection.Right;
                else
                    desiredDirection = HorizontalDirection.Left;

                if (Owner.IsTouchingWall(desiredDirection) == false)
                {
                    isAboutToMove = true;

                    Blackboard.SetItem(Tribal.BlackboardKeys.MoveDirection, ev.weight > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);

                    Trigger(Tribal.Trigger.Move);
                    return true;
                }
            }

            return false;
        }

        private bool IsInPullMode()
        {
            return Blackboard.GetItem<bool>(Tribal.BlackboardKeys.PullMode);
        }
    }
}