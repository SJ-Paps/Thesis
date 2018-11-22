using UnityEngine;

public abstract class ActivableObject : SJMonoBehaviourSaveable, IActivable
{
    public virtual void Activate(Character user)
    {
        Debug.Log("EL OBJECTO " + name + " FUE ACTIVADO");
    }
}
