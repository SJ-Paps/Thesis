using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class IdlePullingState : TribalSimpleState
    {
        private bool isAboutToMove;

        protected override void OnEnter()
        {
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
                FaceDirection desiredDirection = default;

                if (ev.weight > 0)
                    desiredDirection = FaceDirection.Right;
                else
                    desiredDirection = FaceDirection.Left;

                if (HasWallTooClose(desiredDirection) == false)
                {
                    isAboutToMove = true;

                    Blackboard.SetItem(Tribal.BlackboardKeys.PullMoveDirection, ev.weight > 0 ? FaceDirection.Right : FaceDirection.Left);

                    Trigger(Tribal.Trigger.Move);
                    return true;
                }
            }

            return false;
        }

        private bool HasWallTooClose(FaceDirection faceDirection)
        {
            int layerMask = Layers.Floor;

            int direction = (int)faceDirection;

            Bounds bounds = Owner.Collider.bounds;
            float widthExtents = 0.02f;
            float heightNegativeOffset = 0.1f;

            var frontPoint = new Vector2(bounds.center.x + ((bounds.extents.x + widthExtents) * direction), bounds.center.y);
            var size = new Vector2(widthExtents * 2, bounds.size.y - heightNegativeOffset);

            return Physics2D.OverlapBox(frontPoint, size, Owner.transform.eulerAngles.z, layerMask) != null;
        }

        private bool IsInPullMode()
        {
            return Blackboard.GetItem<bool>(Tribal.BlackboardKeys.PullMode);
        }
    }
}