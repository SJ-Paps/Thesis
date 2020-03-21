using SJ.GameEntities;
using UnityEngine;

public delegate void ChangeControlDelegate<TSlave, TOrder>(TSlave previousSlave, TSlave newSlave) where TSlave : IControllable<TOrder>;

public interface IController<TControllable, TOrder> where TControllable : IControllable<TOrder>
{
    event ChangeControlDelegate<TControllable, TOrder> OnControllableChanged;
    void SetControllable(TControllable controllable);
}

public abstract class UnityController<TControllable, TOrder> : SaveableGameEntity, IController<TControllable, TOrder> where TControllable : IControllable<TOrder>
{
    public event ChangeControlDelegate<TControllable, TOrder> OnControllableChanged;

    [SerializeField]
    protected TControllable controllable;

    public TControllable Controllable => controllable;

    public void SetControllable(TControllable controllable)
    {
        TControllable previous = this.controllable;
        this.controllable = controllable;
        OnControllableChanged?.Invoke(previous, controllable);
    }
}