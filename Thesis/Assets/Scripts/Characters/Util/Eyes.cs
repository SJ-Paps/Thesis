using UnityEngine;
using System;


public class Eyes : MonoBehaviour
{
    private Trigger2D trigger2D;

    public Trigger2D Trigger2D
    {
        get
        {
            if(trigger2D == null)
            {
                trigger2D = GetComponent<Trigger2D>();
            }

            return trigger2D;
        }

        protected set
        {
            trigger2D = value;
        }
    }

    [SerializeField]
    protected Transform eyePoint;


    public event Action<Collider2D> onEntered
    {
        add
        {
            Trigger2D.onEntered += value;
        }

        remove
        {
            Trigger2D.onEntered -= value;
        }
    }

    public event Action<Collider2D> onStay
    {
        add
        {
            Trigger2D.onStay += value;
        }

        remove
        {
            Trigger2D.onStay -= value;
        }
    }

    public event Action<Collider2D> onExited
    {
        add
        {
            Trigger2D.onExited += value;
        }

        remove
        {
            Trigger2D.onExited -= value;
        }
    }

    public void SetEyePoint(Transform eyePoint)
    {
        this.eyePoint = eyePoint;
    }

    public bool IsVisible(Collider2D collider, int blockingLayers, int targetLayer)
    {
        if(eyePoint != null)
        {
            int finalLayerMask = blockingLayers | targetLayer;

            RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, collider.transform.position, finalLayerMask);

            if(hit)
            {
                return hit.collider == collider;
            }
        }

        return false;
    }

    public bool IsNear(Collider2D collider, int blockingLayers, int targetLayer, float distance)
    {
        if (eyePoint != null)
        {
            int finalLayerMask = blockingLayers | targetLayer;

            RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, collider.transform.position, finalLayerMask);

            if(hit)
            {
                return hit.distance <= distance;
            }
        }

        return false;
    }

    public bool IsVisibleAndNear(Collider2D collider, int blockingLayers, int targetLayer, float distance)
    {
        if (eyePoint != null)
        {
            int finalLayerMask = blockingLayers | targetLayer;

            RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, collider.transform.position, finalLayerMask);

            if(hit)
            {
                return hit.collider == collider && hit.distance <= distance;
            }
        }

        return false;
    }
}
