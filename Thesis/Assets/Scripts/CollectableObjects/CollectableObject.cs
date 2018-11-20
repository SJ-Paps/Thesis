﻿public abstract class CollectableObject : SJMonoBehaviour, ICollectable {

    public Character User { get; protected set; }

    public virtual bool Collect(Character user)
    {
        User = user;

        return true;
    }

    public virtual bool Drop()
    {
        User = null;

        return true;
    }
	
}