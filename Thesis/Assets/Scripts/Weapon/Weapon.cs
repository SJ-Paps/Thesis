using UnityEngine;

public abstract class Weapon : SJMonoBehaviour {
    
    public Character User { get; protected set; }

    public virtual bool BeingUsed { get; protected set; }
    
    protected virtual void Awake()
    {

    }

    public virtual void SetUser(Character character)
    {
        User = character;

        transform.SetParent(character.HandPoint);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public virtual void Drop()
    {
        User = null;

        transform.SetParent(null);
    }

    public void UseWeapon()
    {
        if(User != null && !BeingUsed)
        {
            OnUseWeapon();
        }
    }

    protected abstract void OnUseWeapon();
}
