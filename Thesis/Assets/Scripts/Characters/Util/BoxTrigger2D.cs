using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger2D : Trigger2D
{
    void Awake()
    {
        InnerCollider = GetComponent<BoxCollider2D>();
    }

    public override void ChangeSize(Vector2 size)
    {
        ((BoxCollider2D)InnerCollider).size = size;
    }
}
