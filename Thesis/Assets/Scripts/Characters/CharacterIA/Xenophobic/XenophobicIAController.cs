using System;
using UnityEngine;

public class XenophobicIAController : IAController
{
    public enum State : byte
    {
        Base,
        Alert,
        Alertless,
        Aware,
        Alertful,
        Patrolling,
        Seeking
    }

    public enum Trigger : byte
    {
        CalmDown,
        Unnerve,
        Patrol,
        Seek
    }

    public class XenophobicIAControllerSaveData
    {
        public string slaveGUID;
    }

    [SerializeField]
    [Range(0, 100)]
    private float hiddenDetectionProbabilityAlertless, hiddenDetectionProbabilityAware, hiddenDetectionProbabilityAlertful;

    [SerializeField]
    private float hiddenDetectionDistanceAlertless, hiddenDetectionDistanceAware, hiddenDetectionDistanceAlertful;

    [SerializeField]
    private float awareTime, alertfulTime;

    [SerializeField]
    private float shortRangeAttackDetectionDistance = 0.8f, longRangeAttackDetectionDistance;

    public float HiddenDetectionDistanceAlertless { get => hiddenDetectionDistanceAlertless; }
    public float HiddenDetectionDistanceAware { get => hiddenDetectionDistanceAware; }
    public float HiddenDetectionDistanceAlertful { get => hiddenDetectionDistanceAlertful; }

    public float HiddenDetectionProbabilityAlertless { get => hiddenDetectionProbabilityAlertless; }
    public float HiddenDetectionProbabilityAware { get => hiddenDetectionProbabilityAware; }
    public float HiddenDetectionProbabilityAlertful { get => hiddenDetectionProbabilityAlertful; }

    public float AwareTime { get => awareTime; }
    public float AlertfulTime { get => alertfulTime; }

    public float ShortRangeAttackDetectionDistance { get => shortRangeAttackDetectionDistance; }
    public float LongRangeAttackDetectionDistance { get => longRangeAttackDetectionDistance; }

    
    public float CurrentSeekingTime { get; set; }
    public float CurrentHiddenDetectionDistance { get; set; }
    public float CurrentHiddenDetectionProbability { get; set; }

    [SerializeField]
    private XenophobicIAControllerHSMStateAsset baseHSMAsset;

    protected XenophobicIAState hsm;

    protected override void SJAwake()
    {
        base.SJAwake();

        hsm = XenophobicIAControllerHSMStateAsset.BuildFromAsset<XenophobicIAState>(baseHSMAsset, this);
    }

    protected override void SJStart()
    {
        base.SJStart();

        hsm.Enter();
    }

    protected override void SJUpdate()
    {
        if(Slave != null)
        {
            Control();
        }
    }

    public override void Control()
    {
        hsm.Update();
    }

    protected override object GetSaveData()
    {
        return new XenophobicIAControllerSaveData() { slaveGUID = Slave.InstanceGuid };
    }

    protected override void LoadSaveData(object data)
    {

    }

    protected override void OnPostSave()
    {

    }

    protected override void OnPostLoad(object data)
    {
        XenophobicIAControllerSaveData saveData = (XenophobicIAControllerSaveData)data;

        SetSlave(SJUtil.FindSJMonoBehaviourByInstanceGUID<Character>(saveData.slaveGUID));
    }
}
