using UnityEngine;

public class Trident : Gun
{
    [SerializeField]
    new private Collider2D collider;

    protected override void Awake()
    {
        
    }

    public override void Activate()
    {
        collider.enabled = true;
    }

    public override void Deactivate()
    {
        collider.enabled = true;
    }
}
