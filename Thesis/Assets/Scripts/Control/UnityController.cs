using UnityEngine;

public delegate void ChangeControlDelegate<TSlave, TOrder>(TSlave previousSlave, TSlave newSlave) where TSlave : IControllable<TOrder> where TOrder : struct;

public interface IController<TSlave, TOrder> where TSlave : IControllable<TOrder> where TOrder : struct
{
    void Control();
    void SetSlave(TSlave slave);
}

public abstract class UnityController<TSlave, TOrder> : MonoBehaviour, IController<TSlave, TOrder> where TSlave : IControllable<TOrder> where TOrder : struct
{
    public event ChangeControlDelegate<TSlave, TOrder> onSlaveChanged;

    [SerializeField]
    protected TSlave slave;

    public TSlave Slave
    {
        get
        {
            return slave;
        }
    }

    public abstract void Control();

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

}


