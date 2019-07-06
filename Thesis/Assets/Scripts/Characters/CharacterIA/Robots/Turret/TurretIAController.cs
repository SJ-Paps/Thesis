using System;
using UnityEngine;

public class TurretIAController : IAController
{
    public enum State : byte
    {
        Base,
        Alertless,
        Alertful,
        Searching
    }

    public enum Trigger : byte
    {
        CalmDown,
        SetFullAlert,
    }

    [SerializeField]
    private TurretIAControllerHSMStateAsset baseHSMAsset;

    protected TurretIAControllerHSMState hsm;

    protected override void Awake()
    {
        base.Awake();

        hsm = TurretIAControllerHSMStateAsset.BuildFromAsset<TurretIAControllerHSMState>(baseHSMAsset, this);
    }

    protected override void Start()
    {
        base.Start();

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
