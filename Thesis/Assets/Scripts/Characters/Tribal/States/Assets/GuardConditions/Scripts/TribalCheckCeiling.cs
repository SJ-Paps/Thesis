using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCheckCeiling : TribalGuardCondition
{
    protected override bool Validate()
    {
        return IsCeiling();
    }

    private bool IsCeiling()
    {
        float rayLength = character.Collider.bounds.extents.y + 0.01f;

        Vector2 startCheckPointLeft = new Vector2(character.Collider.bounds.center.x - character.Collider.bounds.extents.x, 
                                                character.Collider.bounds.center.y + character.Collider.bounds.extents.y);
        Vector2 startCheckPointRight = new Vector2(character.Collider.bounds.center.x + character.Collider.bounds.extents.x,
                                                startCheckPointLeft.y);

        Vector2 endCheckPointLeft = new Vector2(startCheckPointLeft.x, startCheckPointLeft.y + rayLength);
        Vector2 endCheckPointRight = new Vector2(startCheckPointRight.x, startCheckPointRight.y + rayLength);

        return Physics2D.Linecast(startCheckPointLeft, endCheckPointLeft, Reg.walkableLayerMask) || Physics2D.Linecast(startCheckPointRight, endCheckPointRight, Reg.walkableLayerMask);
    }
}
