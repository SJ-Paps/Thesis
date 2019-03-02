using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TribalCheckingLedgesState : TribalHSMState
{
    private SyncTimer waiterToCheckTimer;

    public TribalCheckingLedgesState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
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

        Bounds ownerBounds = Owner.Collider.bounds;

        Vector2 beginPoint = new Vector2(ownerBounds.center.x + (ownerBounds.extents.x * xDirection),
                                                        ownerBounds.center.y + (ownerBounds.extents.y));

        if (IsInFrontOfLedge(beginPoint, xDirection, out RaycastHit2D hit))
        {
            Log("IM IN FRONT OF A WALKABLE OBJECT");

            float yCenterOffset = 0.02f;

            Vector2 checkBox = new Vector2(ownerBounds.extents.x / 2, 0.1f);

            Vector2 validCheckCenter = new Vector2(hit.point.x, hit.point.y + checkBox.y + yCenterOffset);
            
            
            if(IsValidForGrappling(validCheckCenter, checkBox))
            {
                Log("AND IT'S A VALID LEDGE");
                Blackboard.ledgeCheckHit = hit;
                SendEvent(Character.Order.HangLedge);
            }
            else
            {
                Log("BUT IT'S NOT A LEDGE");
            }
        }
    }

    private bool IsValidForGrappling(Vector2 center, Vector2 boxSize)
    {
        Collider2D possibleObstacle = Physics2D.OverlapBox(center, boxSize, Reg.walkableLayerMask);

        return possibleObstacle == null;
    }

    private bool IsInFrontOfLedge(Vector2 beginPoint, int direction, out RaycastHit2D hit)
    {
        float rayYSizeReduction = 0.1f;

        Vector2 endPoint = new Vector2(beginPoint.x + (Owner.Collider.bounds.size.x * direction), beginPoint.y - Owner.Collider.bounds.size.y + rayYSizeReduction);

        hit = Physics2D.Linecast(beginPoint, endPoint, Reg.walkableLayerMask);

        EditorDebug.DrawLine(beginPoint, endPoint, Color.green);

        return hit;
    }
}