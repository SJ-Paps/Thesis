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

    protected override void SJAwake()
    {
        base.SJAwake();

        hsm = TurretIAControllerHSMStateAsset.BuildFromAsset<TurretIAControllerHSMState>(baseHSMAsset, this);
    }

    protected override void SJStart()
    {
        base.SJStart();

        hsm.Enter();
    }

    protected override void SJUpdate()
    {
        Control();
    }

    public override void Control()
    {
        hsm.Update();
    }

    protected override object GetSaveData()
    {
        return new TurretIAControllerSaveData() { slaveGUID = Slave.EntityGUID };
    }

    protected override void LoadSaveData(object data)
    {

    }

    protected override void OnPostSave()
    {

    }

    protected override void OnPostLoad(object data)
    {
        TurretIAControllerSaveData saveData = (TurretIAControllerSaveData)data;

        SetSlave(SJUtil.FindGameEntityByEntityGUID<Character>(saveData.slaveGUID));
    }
}
