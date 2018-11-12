using SAM.FSM;
using UnityEngine;

public class Turret : Robot
{
    [SerializeField]
    protected TurretIdleState idleState;

    [SerializeField]
    protected TurretMovingState movingState;

    [SerializeField]
    protected TurretActionIdleState actionIdleState;

    [SerializeField]
    protected TurretAttackState attackState;

    [SerializeField]
    private float leftLimit, rightLimit;

    public float LeftLimit
    {
        get
        {
            return leftLimit;
        }
    }

    public float RightLimit
    {
        get
        {
            return rightLimit;
        }
    }

    private float currentRotationReference;

    protected override void Awake()
    {
        
        base.Awake();

        FSM<State, Trigger> movingFSM = new FSM<State, Trigger>();


        movingState.InitializeState(movingFSM, State.Idle, this, orders, blackboard);
        idleState.InitializeState(movingFSM, State.Moving, this, orders, blackboard);

        movingFSM.AddState(idleState);
        movingFSM.AddState(movingState);

        movingFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movingFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movingFSM.StartBy(State.Idle);

        FSM<State, Trigger> actionFSM = new FSM<State, Trigger>();

        actionIdleState.InitializeState(actionFSM, State.Idle, this, orders, blackboard);
        attackState.InitializeState(actionFSM, State.Attacking, this, orders, blackboard);

        actionFSM.AddState(actionIdleState);
        actionFSM.AddState(attackState);
        

        actionFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        actionFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);

        actionFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movingFSM);
        AddStateMachineWhenAlive(actionFSM);
    }

    public override void GetEnslaved()
    {

    }

    protected override void OnFacingChanged(bool facingLeft)
    {
        
    }

    public void Rotate(float rotation)
    {
        if (currentRotationReference + rotation > leftLimit)
        {
            rotation = leftLimit - currentRotationReference;
        }
        else if (currentRotationReference + rotation < rightLimit)
        {
            rotation = rightLimit - currentRotationReference;
        }

        transform.Rotate(Vector3.forward, rotation);
        currentRotationReference += rotation;
    }

    public bool IsOverLimit()
    {
        return currentRotationReference == leftLimit || currentRotationReference == rightLimit;
    }
}
