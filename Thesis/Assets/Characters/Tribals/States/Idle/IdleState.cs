using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class IdleState : TribalState
    {
        protected override void OnEnter()
        {
            Owner.Animator.SetTrigger(Tribal.AnimatorTriggers.Idle);
        }

        protected override void OnExit()
        {
            Owner.Animator.ResetTrigger(Tribal.AnimatorTriggers.Idle);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if(ev.type == Character.OrderType.Move)
            {
                FaceDirection desiredDirection = default;

                if (ev.weight > 0)
                    desiredDirection = FaceDirection.Right;
                else
                    desiredDirection = FaceDirection.Left;

                Owner.Face(desiredDirection);

                if (HasWallTooClose(desiredDirection) == false)
                {
                    Blackboard.SetItem(Tribal.BlackboardKeys.MovingInitialDirectionAndForce, ev.weight);
                    Trigger(Tribal.Trigger.Trot);
                    return true;
                }
            }

            return false;
        }

        private bool HasWallTooClose(FaceDirection faceDirection)
        {
            int layerMask = Reg.walkableLayerMask;

            Bounds bounds = Owner.Collider.bounds;
            float width = 0.05f;
            float offset = -0.01f;

            int direction = (int)faceDirection;

            var upperPoint = new Vector2(bounds.center.x + ((bounds.extents.x - offset) * direction), bounds.center.y + bounds.extents.y);
            var lowerPoint = new Vector2(bounds.center.x + ((bounds.extents.x - offset) * direction), bounds.center.y - bounds.extents.y);

            Logger.DrawLine(upperPoint, new Vector2(upperPoint.x + (width * direction), lowerPoint.y), Color.green);
            Logger.DrawLine(lowerPoint, new Vector2(lowerPoint.x + (width * direction), upperPoint.y), Color.green);

            return Physics2D.Linecast(upperPoint, new Vector2(upperPoint.x + (width * direction), lowerPoint.y), layerMask) ||
                Physics2D.Linecast(lowerPoint, new Vector2(lowerPoint.x + (width * direction), upperPoint.y), layerMask);
        }
    }
}
