using UnityEngine;
using System;

public delegate void ChangeControlDelegate<TOrder>(IControllable<TOrder> previousSlave, IControllable<TOrder> newSlave) where TOrder : struct;

public abstract class UnityController : MonoBehaviour
{
    public abstract void Control();
}

public abstract class UnityController<TSlave, TOrder> : UnityController where TSlave : IControllable<TOrder> where TOrder : struct
{
    [SerializeField]
    protected struct KeyOrder
    {
        [SerializeField]
        public MultiKey key;
        [SerializeField]
        public TOrder order;
    }

    public event ChangeControlDelegate<TOrder> onSlaveChanged;

    [SerializeField]
    protected TSlave slave;

    public void SetSlave(TSlave slave)
    {
        this.slave = slave;
    }
}
