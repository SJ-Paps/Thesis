using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCheckCeiling : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return IsCeiling();
    }

    private bool IsCeiling()
    {
        float rayLength = Configuration.Collider.bounds.extents.y + 0.01f;

        SJCollider2D Collider = Configuration.Collider;

        Vector2 startCheckPointLeft = new Vector2(Collider.bounds.center.x - Collider.bounds.extents.x,
                                                Collider.bounds.center.y + Collider.bounds.extents.y);
        Vector2 startCheckPointRight = new Vector2(Collider.bounds.center.x + Collider.bounds.extents.x,
                                                startCheckPointLeft.y);

        Vector2 endCheckPointLeft = new Vector2(startCheckPointLeft.x, startCheckPointLeft.y + rayLength);
        Vector2 endCheckPointRight = new Vector2(startCheckPointRight.x, startCheckPointRight.y + rayLength);

        return Physics2D.Linecast(startCheckPointLeft, endCheckPointLeft, Reg.walkableLayerMask) || Physics2D.Linecast(startCheckPointRight, endCheckPointRight, Reg.walkableLayerMask);
    }
}
