using UnityEngine;
using System;

public class CharController : UnityController<Character, Character.Order> {

    [Serializable]
    new protected struct KeyOrder
    {
        [SerializeField]
        public MultiKey key;
        [SerializeField]
        public Character.Order order;
    }

    [SerializeField]
    private KeyOrder[] keyOrders;

    void Update()
    {
        Control();
    }

    public override void Control()
    {
        for (int i = 0; i < keyOrders.Length; i++)
        {
            if (keyOrders[i].key.WasTriggered())
            {
                slave.SetOrder(keyOrders[i].order);
            }
        }
    }
}
