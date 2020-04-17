using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class IdleStandingDuckingState : TribalSimpleState
    {
        private RigidbodyConstraints2D previousConstraints;

        protected override void OnEnter()
        {
            previousConstraints = Owner.RigidBody2D.Constraints;
            Owner.RigidBody2D.Constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Idle);
        }

        protected override void OnExit()
        {
            Owner.RigidBody2D.Constraints = previousConstraints;
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Idle);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if(ev.type == Character.OrderType.Move)
            {
                HorizontalDirection desiredDirection = default;

                if (ev.weight > 0)
                    desiredDirection = HorizontalDirection.Right;
                else
                    desiredDirection = HorizontalDirection.Left;

                Owner.Face(desiredDirection);

                if (Owner.IsTouchingWall(desiredDirection) == false)
                {
                    Blackboard.SetItem(Tribal.BlackboardKeys.MoveDirection, ev.weight > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
                    Trigger(Tribal.Trigger.Move);
                    return true;
                }
            }

            return false;
        }
    }
}
