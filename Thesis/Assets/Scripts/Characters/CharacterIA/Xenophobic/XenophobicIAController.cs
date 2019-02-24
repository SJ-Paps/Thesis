using System;
using UnityEngine;

public class XenophobicIAController : IAController<Xenophobic>
{

    public enum State
    {
        Base,
        Alertless,
        Aware,
        Alertful,
        Patrolling,
        Seeking
    }

    public enum Trigger
    {
        CalmDown,
        GetAware,
        SetFullAlert,
        Patrol,
        Seek
    }

    public class Blackboard
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
    }

    [SerializeField]
    private XenophobicIAControllerHSMStateAsset baseHSMAsset;

    protected XenophobicIAState hsm;

    protected Blackboard blackboard;

    protected override void Awake()
    {
        base.Awake();

        blackboard = new Blackboard();

        hsm = XenophobicIAControllerHSMStateAsset.BuildFromAsset<XenophobicIAState>(baseHSMAsset, this, blackboard);
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
