using SAM.FSM;
using UnityEngine;

public class Turret : Robot
{
    /*[SerializeField]
    protected TurretIdleState idleState;

    [SerializeField]
    protected TurretAttackState attackState;

    [SerializeField]
    protected TurretMovingState movingState;

    [SerializeField]
    protected TurretActionIdleState actionIdleState;*/

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

        /*FSM<State, Trigger> movingFSM = new FSM<State, Trigger>();

        idleState.InitializeState(movingFSM, State.Idle, this, orders, blackboard);
        movingState.InitializeState(movingFSM, State.Moving, this, orders, blackboard);

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
        AddStateMachineWhenAlive(actionFSM);*/
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

    public override bool ShouldBeSaved()
    {
        return true;
    }

    protected override void OnSave(SaveData data)
    {
        data.AddValue("x", transform.parent.position.x);
        data.AddValue("y", transform.parent.position.y);
        data.AddValue("limitLeft", leftLimit);
        data.AddValue("limitRight", rightLimit);
        data.AddValue("rotation z", transform.parent.rotation.z);
        data.AddValue("rotation x", transform.parent.rotation.x);
        data.AddValue("rotation y", transform.parent.rotation.y);
        data.AddValue("rotation w", transform.parent.rotation.w);
    }

    protected override void OnLoad(SaveData data)
    {
        transform.parent.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));
        transform.parent.rotation = new Quaternion(data.GetAs<float>("rotation x"), data.GetAs<float>("rotation y"), data.GetAs<float>("rotation z"), data.GetAs<float>("rotation w"));
        leftLimit = data.GetAs<float>("limitLeft");
        rightLimit = data.GetAs<float>("limitRight");
    }
}
