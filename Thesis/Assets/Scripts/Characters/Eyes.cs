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

    public bool IsVisible(Collider2D collider, int[] layers)
    {
        int layerMask;

        if(layers.Length != 0)
        {
            layerMask = 1 << layers[0];

            for(int i = 1; i < layers.Length; i++)
            {
                layerMask = layerMask | (1 << layers[i]);
            }

            RaycastHit2D hit = Physics2D.Linecast(startPoint.position, collider.transform.position, layerMask);

            return hit.collider == collider;
        }

        return false;
    }
}
