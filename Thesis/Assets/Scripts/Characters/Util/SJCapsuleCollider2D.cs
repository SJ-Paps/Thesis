using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class SJCapsuleCollider2D : SJCollider2D
{
    public CapsuleCollider2D InnerBoxCollider2D { get; private set; }

    protected override void OnAwake()
    {
        InnerBoxCollider2D = (CapsuleCollider2D)InnerCollider;
    }

    public override void ChangeSize(Vector3 size)
    {
        InnerBoxCollider2D.size = size;
    }

    public override Vector3 GetSize()
    {
        return InnerBoxCollider2D.size;
    }
}
