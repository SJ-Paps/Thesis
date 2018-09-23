using UnityEngine;
using System;


public class Eyes : MonoBehaviour
{
    public Trigger2D Trigger2D { get; protected set; }

    protected Transform startPoint;

    void Awake()
    {
        Trigger2D = GetComponent<Trigger2D>();
    }

    public void SetEyePoint(Transform eyePoint)
    {
        startPoint = eyePoint;
    }

    public bool IsVisible(Collider2D collider, int layerMask)
    {
        if(startPoint != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(startPoint.position, collider.transform.position, layerMask);

            return hit.collider == collider;
        }

        return false;
    }

    public bool IsNear(Collider2D collider, int layerMask, float distance)
    {
        if (startPoint != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(startPoint.position, collider.transform.position, layerMask);

            return hit.distance <= distance;
        }

        return false;
    }
}
