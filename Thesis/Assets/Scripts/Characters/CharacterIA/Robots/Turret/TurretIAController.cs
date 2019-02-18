﻿using System;
using UnityEngine;

public class TurretIAController : IAController<Turret>
{
    public enum State
    {
        Base,
        Alertless,
        Alertful,
        Searching
    }

    public enum Trigger
    {
        CalmDown,
        SetFullAlert,
    }

    public class Blackboard
    {

    }

    [SerializeField]
    private TurretIAControllerHSMStateAsset baseHSMAsset;

    protected TurretIAControllerHSMState hsm;

    protected Blackboard blackboard;

    protected override void Awake()
    {
        base.Awake();

        hsm = TurretIAControllerHSMStateAsset.BuildFromAsset<TurretIAControllerHSMState>(baseHSMAsset, this, blackboard);

        hsm.Enter();
    }

    void Update()
    {
        Control();
    }

    public override void Control()
    {
        hsm.Update();
    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

    private Guid slaveGuid;

    protected override void OnSave(SaveData data)
    {
        data.AddValue("s", Slave.saveGUID);
    }

    protected override void OnLoad(SaveData data)
    {
        slaveGuid = new Guid(data.GetAs<string>("s"));
    }

    public override void PostLoadCallback(SaveData data)
    {
        Turret slave = SJMonoBehaviourSaveable.GetSJMonobehaviourSaveableBySaveGUID<Turret>(slaveGuid);

        if (slave != null)
        {
            SetSlave(slave);
        }
    }
}
