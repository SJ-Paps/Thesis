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
}
