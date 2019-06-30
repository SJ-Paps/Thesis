using System;
using UnityEngine;

public class XenophobicIAController : IAController<Xenophobic>
{
    [Serializable]
    public sealed class ConfigurationData
    {
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
    }

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

    /*public class Blackboard : global::Blackboard
    {
        public event Action<Vector2> onLastDetectedPositionUpdated;

        private Vector2 lastDetectedPosition;

        public Vector2 LastDetectedPosition
        {
            get
            {
                return lastDetectedPosition;
            }

            set
            {
                lastDetectedPosition = value;

                if (onLastDetectedPositionUpdated != null)
                {
                    onLastDetectedPositionUpdated(lastDetectedPosition);
                }
            }
        }
    }*/

    [SerializeField]
    private ConfigurationData configuration;

    public ConfigurationData Configuration
    {
        get
        {
            return configuration;
        }
    }

    public float CurrentSeekingTime { get; set; }
    public float CurrentHiddenDetectionDistance { get; set; }
    public float CurrentHiddenDetectionProbability { get; set; }

    [SerializeField]
    private XenophobicIAControllerHSMStateAsset baseHSMAsset;

    protected XenophobicIAState hsm;

    protected override void Awake()
    {
        base.Awake();

        hsm = XenophobicIAControllerHSMStateAsset.BuildFromAsset<XenophobicIAState>(baseHSMAsset, this, GetConfiguration());
    }

    protected override void Start()
    {
        base.Start();

        hsm.Enter();
    }

    protected void Update()
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
        Xenophobic slave = SJMonoBehaviourSaveable.GetSJMonobehaviourSaveableBySaveGUID<Xenophobic>(slaveGuid);

        if(slave != null)
        {
            SetSlave(slave);
        }
    }
}
