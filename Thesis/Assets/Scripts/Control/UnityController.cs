using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeControlDelegate(IControllable previousSlave, IControllable newSlave);

public abstract class UnityController
{
    public string Name { get; protected set; }

    protected InputAxis[] axes;

    protected UnityController(string name)
    {
        Name = name;
    }

    public abstract void Control();

    protected InputAxis GetInputAxisByName(string name)
    {
        for(int i = 0; i < axes.Length; i++)
        {
            if(axes[i].Name == name)
            {
                return axes[i];
            }
        }

        return null;
    }
}

public abstract class UnityController<T> : UnityController where T : IControllable
{
    public event ChangeControlDelegate onSlaveChanged;

    protected T slave;

    protected UnityController(string name) : base(name)
    {

    }

    public void SetSlave(T slave)
    {
        this.slave = slave;
    }
}
