using UnityEngine;
using System;

public class CharacterInputController : UnityInputController<Character, Character.Trigger> {

    [Serializable]
    protected struct KeyOrder
    {
        [SerializeField]
        public MultiKey key;
        [SerializeField]
        public Character.Trigger[] orders;
    }

    [SerializeField]
    private KeyOrder[] keyOrders;

    void Update()
    {
        Control();
    }

    private Guid slaveGuid;

    public override void Control()
    {
        for (int i = 0; i < keyOrders.Length; i++)
        {
            if (keyOrders[i].key.WasTriggered())
            {
                foreach(Character.Trigger order in keyOrders[i].orders)
                {
                    slave.SetOrder(order);
                }
            }
        }
    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

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
        Character slave = SJMonoBehaviourSaveable.GetSJMonobehaviourSaveableBySaveGUID<Character>(slaveGuid);

        if(slave != null)
        {
            SetSlave(slave);
        }
    }
}
