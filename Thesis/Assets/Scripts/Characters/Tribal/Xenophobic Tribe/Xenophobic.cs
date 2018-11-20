using UnityEngine;

public class Xenophobic : Tribal
{
    [SerializeField]
    protected XenophobicAttackState attackState;

    protected override void Awake()
    {
        base.Awake();

        attackState.InitializeState(actionFSM, State.Attacking, this, orders, blackboard);

        actionFSM.AddState(attackState);

        actionFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        actionFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);
        
    }

    public override void GetEnslaved()
    {

    }

    protected override void OnSave(SaveData data)
    {
        data.AddValue("x", transform.position.x);
        data.AddValue("y", transform.position.y);
    }

    protected override void OnLoad(SaveData data)
    {
        transform.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));
    }

}
