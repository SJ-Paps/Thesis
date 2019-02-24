using SAM.Timers;
using UnityEngine;

public class XenophobicIAPatrollingState : XenophobicIAState
{
    private SyncTimer turnTimer;

    private Character.Trigger currentOrder;

    private float minTime = 4f, maxTime = 6f;

    public XenophobicIAPatrollingState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        turnTimer = new SyncTimer();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(ShouldTurn())
        {
            Turn();
        }

        Owner.Slave.SetOrder(currentOrder);
    }

    protected override void OnExit()
    {
        base.OnExit();

        turnTimer.Stop();
    }

    private bool ShouldTurn()
    {
        return turnTimer.Active == false || Owner.Slave.CheckWall() || Owner.Slave.CheckFloorAhead() == false;
    }

    private void Turn()
    {
        float xDir = Owner.Slave.transform.right.x;

        if (xDir < 0)
        {
            currentOrder = Character.Trigger.MoveRight;
        }
        else
        {
            currentOrder = Character.Trigger.MoveLeft;
        }

        turnTimer.Interval = CalculatePatrolTime();
        turnTimer.Start();
    }

    private float CalculatePatrolTime()
    {
        return Random.Range(minTime, maxTime);
    }
}
