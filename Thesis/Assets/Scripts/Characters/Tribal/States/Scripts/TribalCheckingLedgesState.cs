﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TribalCheckingLedgesState : TribalHSMState
{
    private SyncTimer waiterToCheckTimer;

    public TribalCheckingLedgesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        waiterToCheckTimer = new SyncTimer();
        waiterToCheckTimer.Interval = 0.2f;
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

        Owner.onCollisionStay2D -= CheckLedges;
    }

    private void OnTimerTick(SyncTimer timer)
    {
        Owner.onCollisionStay2D += CheckLedges;
    }

    private void CheckLedges(Collision2D collision)
    {
        Log("CHECKING LEDGES");

        int xDirection = 0;

        if (Owner.transform.right.x >= 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
        }

        Vector2 beginPoint = new Vector2(Owner.Collider.bounds.center.x + (Owner.Collider.bounds.extents.x * xDirection),
                                                        Owner.Collider.bounds.center.y + (Owner.Collider.bounds.extents.y));

        if (IsInFrontOfLedge(beginPoint, xDirection, out RaycastHit2D hit))
        {
            Log("IM IN FRONT OF A WALKABLE OBJECT");
            
            if(IsValidForGrappling(beginPoint, new Vector2(Owner.Collider.bounds.extents.x / 2, Owner.Collider.bounds.extents.y / 6), xDirection, ref hit))
            {
                Log("AND IT'S A VALID LEDGE");
                Blackboard.ledgeCheckHit = hit;
                SendEvent(Character.Trigger.HangLedge);
            }
            else
            {
                Log("BUT IT'S NOT A LEDGE");
            }
        }
    }

    private bool IsValidForGrappling(Vector2 beginPoint, Vector2 checkBoxSize, int direction, ref RaycastHit2D hit)
    {
        float yCenterOffset = 0.03f;

        Vector2 checkIsValidLedgeBoxCenter = new Vector2(beginPoint.x + (Owner.Collider.bounds.extents.x * direction), hit.point.y + checkBoxSize.y + yCenterOffset);
        
        Collider2D possibleObstacle = Physics2D.OverlapBox(checkIsValidLedgeBoxCenter, checkBoxSize, Reg.walkableLayerMask);

        return possibleObstacle == null;
    }

    private bool IsInFrontOfLedge(Vector2 beginPoint, int direction, out RaycastHit2D hit)
    {
        Vector2 endPoint = new Vector2(beginPoint.x + (Owner.Collider.bounds.size.x * direction), beginPoint.y - Owner.Collider.bounds.extents.y);

        hit = Physics2D.Linecast(beginPoint, endPoint, Reg.walkableLayerMask);

        EditorDebug.DrawLine(beginPoint, endPoint, Color.green);

        return hit;
    }
}