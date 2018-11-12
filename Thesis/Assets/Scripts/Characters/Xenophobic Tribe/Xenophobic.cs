using UnityEngine;

public class Xenophobic : Tribal, IAudibleListener
{
    [SerializeField]
    protected Weapon weapon;

    public Weapon Weapon
    {
        get
        {
            return weapon;
        }

        protected set
        {
            weapon = value;
        }
    }

    public override void GetEnslaved()
    {

    }

    public void Listen(ref AudibleData data)
    {
        
    }
}
