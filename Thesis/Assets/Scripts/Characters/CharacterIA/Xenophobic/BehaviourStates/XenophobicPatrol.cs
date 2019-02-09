using SAM.Timers;
using System;
using UnityEngine;

public class XenophobicPatrolState : XenophobicIAState
{

    private float minTime = 4f, maxTime = 6f;

    private SyncTimer patrolTimer;

    private Character.Trigger currentOrder;

    private Action<Vector2> onLastDetectionPositionChangedDelegate;

    public XenophobicPatrolState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        //onLastDetectionPositionChangedDelegate += Seek;

        patrolTimer = new SyncTimer();
        //patrolTimer.onTick += TickTurn;
    }


    /*protected override void OnEnter()
    {
        EditorDebug.Log("XENOPHOBIC PATROL ENTER");

        blackboard.onLastDetectionPositionChanged += onLastDetectionPositionChangedDelegate;

        Turn();
    }

    protected override void OnUpdate()
    {
        patrolTimer.Update(Time.deltaTime);

        if(ShouldTurn())
        {
            Turn();
        }

        controller.Slave.SetOrder(currentOrder);
    }

    protected override void OnExit()
    {
        EditorDebug.Log("XENOPHOBIC PATROL EXIT");
    }

    private float CalculatePatrolTime()
    {
        return UnityEngine.Random.Range(minTime, maxTime);
    }

    private bool ShouldTurn()
    {
        return controller.Slave.CheckWall(Reg.walkableLayerMask) || controller.Slave.CheckFloorAhead(Reg.walkableLayerMask) == false;
    }

    private void Seek(Vector2 position)
    {
        stateMachine.Trigger(XenophobicIAController.Trigger.Seek);
    }

    private void TickTurn(SyncTimer timer)
    {
        Turn();
    }

    private void Turn()
    {
        float xDir = controller.Slave.transform.right.x;

        if (xDir < 0)
        {
            currentOrder = Character.Order.OrderMoveRight;
        }
        else
        {
            currentOrder = Character.Order.OrderMoveLeft;
        }

        patrolTimer.Interval = CalculatePatrolTime();
        patrolTimer.Start();
    }*/
}
