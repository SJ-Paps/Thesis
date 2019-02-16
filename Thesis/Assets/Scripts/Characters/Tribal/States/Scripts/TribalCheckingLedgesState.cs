using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TribalCheckingLedgesState : TribalHSMState
{
    private float yCenterOffset = 0.03f;

    private SyncTimer waiterToCheckTimer;
    private float waiterToCheckInterval = 0.2f;

    public TribalCheckingLedgesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activeDebug = true;

        waiterToCheckTimer = new SyncTimer();
        waiterToCheckTimer.Interval = waiterToCheckInterval;
        waiterToCheckTimer.onTick += OnTimerTick;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        waiterToCheckTimer.Start();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        waiterToCheckTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.onCollisionStay2D -= CheckLedges;
    }

    private void OnTimerTick(SyncTimer timer)
    {
        character.onCollisionStay2D += CheckLedges;
    }

    private void CheckLedges(Collision2D collision)
    {
        Log("CHECKING LEDGES");

        int xDirection = 0;

        if (character.transform.right.x >= 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
        }

        Vector2 beginPoint = new Vector2(character.Collider.bounds.center.x + (character.Collider.bounds.extents.x * xDirection),
                                                        character.Collider.bounds.center.y + (character.Collider.bounds.extents.y));

        if (IsInFrontOfLedge(beginPoint, xDirection, out RaycastHit2D hit))
        {
            Log("IM IN FRONT OF A WALKABLE OBJECT");

            if(IsValidForGrapplingAndClimbing(beginPoint, xDirection, ref hit))
            {
                Log("AND IT'S A VALID LEDGE");
                blackboard.ledgeCheckHit = hit;
                SendEvent(Character.Trigger.HangLedge);
            }
            else
            {
                Log("BUT IT'S NOT A LEDGE");
            }
        }
    }

    private bool IsValidForGrapplingAndClimbing(Vector2 beginPoint, int direction, ref RaycastHit2D hit)
    {
        Vector2 checkIsValidLedgeBoxSize = character.Collider.bounds.size;
        Vector2 checkIsValidLedgeBoxCenter = new Vector2(beginPoint.x + (character.Collider.bounds.extents.x * direction), hit.point.y + character.Collider.bounds.extents.y + yCenterOffset);
        
        Collider2D possibleObstacle = Physics2D.OverlapCapsule(checkIsValidLedgeBoxCenter, checkIsValidLedgeBoxSize, CapsuleDirection2D.Vertical, Reg.walkableLayerMask);

        return possibleObstacle == null;
    }

    private bool IsInFrontOfLedge(Vector2 beginPoint, int direction, out RaycastHit2D hit)
    {
        Vector2 endPoint = new Vector2(beginPoint.x + (character.Collider.bounds.size.x * direction), beginPoint.y - character.Collider.bounds.size.y);

        hit = Physics2D.Linecast(beginPoint, endPoint, Reg.walkableLayerMask);

        EditorDebug.DrawLine(beginPoint, endPoint, Color.green);

        return hit;
    }
}