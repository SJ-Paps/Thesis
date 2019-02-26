using UnityEngine;

public abstract class ActivableObject<TActivator> : SJMonoBehaviourSaveable, IActivable<TActivator> where TActivator : class
{
    public virtual bool Activate(TActivator user)
    {
        EditorDebug.Log("EL OBJECTO " + name + " FUE ACTIVADO");

        return true;
    }
}
