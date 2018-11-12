using SAM.FSM;
using UnityEngine;
using System;
using SAM.Timers;

[Serializable]
public class XenophobicPatrolState : XenophobicIAState
{
    [SerializeField]
    private float minTime = 4f, maxTime = 6f;

    private SyncTimer patrolTimer;

    private Character.Order currentOrder;

    private Action<Vector2> onLastDetectionPositionChangedDelegate;

    public override void InitializeState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, controller, blackboard);

        onLastDetectionPositionChangedDelegate += Seek;

        patrolTimer = new SyncTimer();
        patrolTimer.onTick += TickTurn;
    }


    protected override void OnEnter()
    {
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
    }
}
