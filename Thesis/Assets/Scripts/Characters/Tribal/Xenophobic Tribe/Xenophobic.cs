using System;
using UnityEngine;

public class Xenophobic : Tribal
{
    

    /*[SerializeField]
protected XenophobicAttackState attackState;

protected override void Awake()
{
base.Awake();

attackState.InitializeState(actionFSM, State.Attacking, this, orders, blackboard);

actionFSM.AddState(attackState);

actionFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
actionFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);

}*/

    public override void GetEnslaved()
    {

    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void PostLoadCallback(SaveData data)
    {
        base.PostLoadCallback(data);
    }

    public override void PostSaveCallback()
    {
        base.PostSaveCallback();
    }

    public override void SetOrder(Trigger order)
    {
        base.SetOrder(order);
    }

    public override bool ShouldBeSaved()
    {
        return base.ShouldBeSaved();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void OnDetected()
    {
        base.OnDetected();
    }

    protected override void OnFacingChanged(bool facingLeft)
    {
        base.OnFacingChanged(facingLeft);
    }

    protected override void OnLoad(SaveData data)
    {
        base.OnLoad(data);
    }

    protected override void OnSave(SaveData data)
    {
        base.OnSave(data);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
