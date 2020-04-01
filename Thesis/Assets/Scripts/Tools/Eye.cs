using UnityEngine;
using System;
using SJ;

namespace SJ.Tools
{
    public class Eye : SJMonoBehaviour
    {
        protected new SJCollider2D collider;

        [SerializeField]
        protected Transform eyePoint;

        public SJCollider2D Collider
        {
            get
            {
                return collider;
            }

            set
            {
                bool shouldReBind = collider != value;

                collider = value;

                if (shouldReBind)
                {
                    Bind(collider);
                }
            }
        }

        public Transform EyePoint
        {
            get
            {
                return eyePoint;
            }

            set
            {
                eyePoint = value;
            }
        }

        public event Action<Collider2D, Eye> onEntered;
        public event Action<Collider2D, Eye> onStay;
        public event Action<Collider2D, Eye> onExited;

        private Action<Collider2D> onEnteredTriggerDelegate;
        private Action<Collider2D> onStayTriggerDelegate;
        private Action<Collider2D> onExitedTriggerDelegate;

        protected override void SJAwake()
        {
            collider = GetComponent<SJCollider2D>();

            onEnteredTriggerDelegate = OnEnteredTrigger;
            onStayTriggerDelegate = OnStayTrigger;
            onExitedTriggerDelegate = OnExitedTrigger;

        }

        protected override void SJStart()
        {
            Bind(Collider);
        }

        private void Bind(SJCollider2D collider)
        {
            ClearPrevious();

            if (collider != null)
            {
                this.collider = collider;

                Collider.IsTrigger = true;

                Collider.onEnteredTrigger += onEnteredTriggerDelegate;
                Collider.onStayTrigger += onStayTriggerDelegate;
                Collider.onExitedTrigger += onExitedTriggerDelegate;
            }
        }

        private void ClearPrevious()
        {
            if (Collider != null)
            {
                Collider.onEnteredTrigger -= onEnteredTriggerDelegate;
                Collider.onStayTrigger -= onStayTriggerDelegate;
                Collider.onExitedTrigger -= onExitedTriggerDelegate;
            }
        }

        private void OnEnteredTrigger(Collider2D collider)
        {
            if (onEntered != null)
            {
                onEntered(collider, this);
            }
        }

        private void OnStayTrigger(Collider2D collider)
        {
            if (onStay != null)
            {
                onStay(collider, this);
            }
        }

        private void OnExitedTrigger(Collider2D collider)
        {
            if (onExited != null)
            {
                onExited(collider, this);
            }
        }

        public bool IsVisible(Collider2D collider, int visionBlockingLayerMask, int targetLayerMask)
        {
            if (eyePoint != null && Collider != null)
            {
                if (Collider.OverlapPoint(eyePoint.position) && Collider.IsTouching(collider))
                {
                    int finalLayerMask = visionBlockingLayerMask | targetLayerMask;

                    float distance;

                    if (Collider.bounds.size.x > Collider.bounds.size.y)
                    {
                        distance = Collider.bounds.size.x;
                    }
                    else
                    {
                        distance = Collider.bounds.size.y;
                    }

                    RaycastHit2D hit = Physics2D.Raycast(eyePoint.position, eyePoint.up, distance, finalLayerMask);

                    if (hit && Collider.OverlapPoint(hit.point))
                    {
                        return hit.collider == collider;
                    }
                }
            }

            return false;
        }

        public bool IsVisible(Collider2D collider, int visionBlockingLayerMask, int targetLayerMask, out RaycastHit2D hitInfo)
        {
            hitInfo = default;

            if (eyePoint != null && Collider != null)
            {
                if (Collider.OverlapPoint(eyePoint.position))
                {
                    int finalLayerMask = visionBlockingLayerMask | targetLayerMask;

                    float distance;

                    if (Collider.bounds.size.x > Collider.bounds.size.y)
                    {
                        distance = Collider.bounds.size.x;
                    }
                    else
                    {
                        distance = Collider.bounds.size.y;
                    }

                    hitInfo = Physics2D.Raycast(eyePoint.position, eyePoint.up, distance, finalLayerMask);

                    if (hitInfo && Collider.OverlapPoint(hitInfo.point))
                    {
                        return hitInfo.collider == collider;
                    }
                }
            }

            return false;
        }
    }
}