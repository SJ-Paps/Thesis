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

    public class TurretIAControllerSaveData
    {
        public string slaveGUID;
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

    protected override object GetSaveData()
    {
        return new TurretIAControllerSaveData() { slaveGUID = Slave.InstanceGUID };
    }

    protected override void LoadSaveData(object data)
    {

    }

    public override void PostSaveCallback()
    {

    }

    public override void PostLoadCallback(object data)
    {
        TurretIAControllerSaveData saveData = (TurretIAControllerSaveData)data;

        SetSlave(SJUtil.FindSJMonoBehaviourByInstanceGUID<Character>(saveData.slaveGUID));
    }
}
