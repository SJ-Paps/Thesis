using SAM.FSM;
using System;
using UnityEngine;

public abstract class Tribal : Character
{
    public override bool CheckIsOnFloor(int layerMask)
    {
        BoxCollider2D boxCollider = ((BoxCollider2D)collider);

        float xRay = transform.position.x + boxCollider.offset.x;
        float yRay = transform.position.y + boxCollider.offset.y - (boxCollider.size.y / 2);

        Vector2 origin = new Vector2(xRay, yRay);

        RaycastHit2D groundDetection = Physics2D.Linecast(origin, origin + (Vector2.down * groundDetectionDistance), layerMask);

        EditorDebug.DrawLine(origin, origin + (Vector2.down * groundDetectionDistance), Color.green);

        return groundDetection.transform != null;
    }
}
