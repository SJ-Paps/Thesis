using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : SJMonoBehaviour {

    protected virtual void Awake()
    {

    }

    public abstract void Activate();

    public abstract void Deactivate();
}
