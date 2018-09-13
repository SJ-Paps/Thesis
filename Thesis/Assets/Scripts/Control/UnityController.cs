using UnityEngine;
using System;

public delegate void ChangeControlDelegate(IControllable previousSlave, IControllable newSlave);

public abstract class UnityController : MonoBehaviour
{
    public abstract void Control();
}

public abstract class UnityController<TSlave, TOrder> : UnityController where TSlave : IControllable<TOrder> where TOrder : struct
{
    public event ChangeControlDelegate onSlaveChanged;

    [SerializeField]
    protected TSlave slave;

    public TSlave Slave
    {
        get
        {
            return slave;
        }
    }

    public void SetSlave(TSlave slave)
    {
        TSlave previous = this.slave;

        this.slave = slave;

        if(onSlaveChanged != null)
        {
            onSlaveChanged(previous, slave);
        }
    }
}

public abstract class UnityInputController<TSlave, TOrder> : UnityController<TSlave, TOrder> where TSlave : IControllable<TOrder> where TOrder : struct
{
    [SerializeField]
    protected struct KeyOrder
    {
        [SerializeField]
        public MultiKey key;
        [SerializeField]
        public TOrder order;
    }
}


