using UnityEngine;

public abstract class ActivableObject : SJMonoBehaviourSaveable, IActivable<Character>
{
    public virtual void Activate(Character user)
    {
        EditorDebug.Log("EL OBJECTO " + name + " FUE ACTIVADO");
    }
}
