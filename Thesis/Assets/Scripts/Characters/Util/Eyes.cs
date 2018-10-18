using UnityEngine;
using System;


public class Eyes : MonoBehaviour
{
    public Trigger2D Trigger2D { get; protected set; }

    [SerializeField]
    protected Transform eyePoint;

    void Awake()
    {
        Trigger2D = GetComponent<Trigger2D>();
    }

    public void SetEyePoint(Transform eyePoint)
    {
        this.eyePoint = eyePoint;
    }

    public bool IsVisible(Collider2D collider, int layerMask)
    {
        if(eyePoint != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, collider.transform.position, layerMask);

            Debug.Log(hit.collider.name);

            return hit.collider == collider;
        }

        return false;
    }

    public bool IsNear(Collider2D collider, int layerMask, float distance)
    {
        if (eyePoint != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, collider.transform.position, layerMask);

            return hit.distance <= distance;
        }

        return false;
    }

    public bool IsVisibleAndNear(Collider2D collider, int layerMask, float distance)
    {
        if (eyePoint != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, collider.transform.position, layerMask);

            return hit.collider == collider && hit.distance <= distance;
        }

        return false;
    }
}
