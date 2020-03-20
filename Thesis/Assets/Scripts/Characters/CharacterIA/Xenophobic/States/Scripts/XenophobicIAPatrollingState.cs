using SJ;
using UnityEngine;

public class XenophobicIAPatrollingState : XenophobicIAState
{
    private SyncTimer turnTimer;

    private Character.Order currentOrder;

    private float minTime = 4f, maxTime = 6f;

    public XenophobicIAPatrollingState()
    {
        turnTimer = new SyncTimer();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        if(Slave.IsFacingLeft)
        {
            currentOrder = Character.Order.MoveLeft;
        }
        else
        {
            currentOrder = Character.Order.MoveRight;
        }

        turnTimer.Start();

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(ShouldTurn())
        {
            Turn();
        }

        Slave.SendOrder(Character.Order.Walk);
        Slave.SendOrder(currentOrder);
    }

    protected override void OnExit()
    {
        base.OnExit();

        turnTimer.Stop();

        Slave.SendOrder(Character.Order.Trot);
    }

    private bool ShouldTurn()
    {
        return turnTimer.Active == false || Slave.CheckWall() || Slave.CheckFloorAhead() == false;
    }

    private void Turn()
    {
        float xDir = Slave.transform.right.x;

        if (xDir < 0)
        {
            currentOrder = Character.Order.MoveRight;
        }
        else
        {
            currentOrder = Character.Order.MoveLeft;
        }

        turnTimer.Interval = CalculatePatrolTime();
        turnTimer.Start();
    }

    private float CalculatePatrolTime()
    {
        return Random.Range(minTime, maxTime);
    }
}
