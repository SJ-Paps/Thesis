using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TribalCheckingLedgesState : TribalHSMState
{
    private float yCenterOffset = 0.1f;

    public TribalCheckingLedgesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activeDebug = true;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        character.onCollisionStay2D += CheckLedges;
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.onCollisionStay2D -= CheckLedges;
    }

    private void CheckLedges(Collision2D collision)
    {
        Log("CHECKING LEDGES");

        float xDir = 0;

        if(character.transform.right.x >= 0)
        {
            xDir = 1;
        }
        else
        {
            xDir = -1;
        }

        Vector2 checkLedgeBoxSize = new Vector2(0.2f, character.Collider.bounds.extents.y);
        Vector2 checkLedgeBoxCenter = new Vector2(character.Collider.bounds.center.x + ((character.Collider.offset.x + character.Collider.bounds.extents.x) * xDir),
                                            character.Collider.bounds.center.y + character.Collider.offset.y + yCenterOffset);

        if (IsInFrontOfLedge(checkLedgeBoxCenter, checkLedgeBoxSize))
        {
            Log("IM IN FRONT OF A LEDGE");

            Vector2 checkIsValidLedgeBoxSize = character.Collider.bounds.size;
            Vector2 checkIsValidLedgeBoxCenter = new Vector2(character.Collider.bounds.center.x + ((character.Collider.offset.x + character.Collider.bounds.size.x) * xDir),
                                                        character.Collider.bounds.center.y + character.Collider.bounds.size.y + yCenterOffset);

            if(IsValidForGrapplingAndClimbing(checkIsValidLedgeBoxCenter, checkIsValidLedgeBoxSize))
            {
                Log("AND IT'S A VALID LEDGE");
            }
            else
            {
                Log("BUT IT'S NOT A VALID LEDGE");
            }
        }
    }


    private bool IsInFrontOfLedge(Vector2 boxCenter, Vector2 boxSize)
    {
        Collider2D possibleLedge = Physics2D.OverlapBox(boxCenter, boxSize, character.transform.eulerAngles.z, Reg.walkableLayerMask);

        return possibleLedge != null;
    }

    private bool IsValidForGrapplingAndClimbing(Vector2 boxCenter, Vector2 boxSize)
    {
        Collider2D possibleObstacle = Physics2D.OverlapBox(boxCenter, boxSize, character.transform.eulerAngles.z, Reg.walkableLayerMask);

        return possibleObstacle == null;
    }
}