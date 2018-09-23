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
}
