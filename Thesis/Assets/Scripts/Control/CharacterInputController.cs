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

    protected override void SJUpdate()
    {
        Control();
    }

    public class CharacterInputControllerSaveData
    {
        public string slaveGUID;
    }

    public override void Control()
    {
        for (int i = 0; i < keyOrders.Length; i++)
        {
            if (keyOrders[i].key.WasTriggered())
            {
                foreach(Character.Order order in keyOrders[i].orders)
                {
                    slave.SendOrder(order);
                }
            }
        }
    }

    protected override object GetSaveData()
    {
        return new CharacterInputControllerSaveData() { slaveGUID = Slave.InstanceGUID };
    }

    protected override void LoadSaveData(object data)
    {
        
    }

    public override void PostSaveCallback()
    {
        
    }

    public override void PostLoadCallback(object data)
    {
        CharacterInputControllerSaveData saveData = (CharacterInputControllerSaveData)data;

        SetSlave(SJUtil.FindSJMonoBehaviourByInstanceGUID<Character>(saveData.slaveGUID));
    }
}
