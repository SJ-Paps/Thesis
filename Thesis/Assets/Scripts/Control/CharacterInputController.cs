using UnityEngine;
using System;

public class CharacterInputController : UnityInputController<Character, Character.Order> {

    [Serializable]
    protected struct KeyOrder
    {
        [SerializeField]
        public MultiKey key;
        [SerializeField]
        public Character.Order[] orders;
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
                foreach(Character.Order order in keyOrders[i].orders)
                {
                    slave.SetOrder(order);
                }
            }
        }
    }

    protected override void OnSave(SaveData data)
    {
        data.AddValue("s", Slave.saveGUID);
    }

    protected override void OnLoad(SaveData data)
    {
        slaveGuid = new Guid(data.GetAs<string>("s"));
    }

    public override void PostLoadCallback()
    {
        Character slave = SJMonoBehaviourSaveable.GetSJMonobehaviourSaveableBySaveGUID<Character>(slaveGuid);

        if(slave != null)
        {
            SetSlave(slave);
        }
    }
}
