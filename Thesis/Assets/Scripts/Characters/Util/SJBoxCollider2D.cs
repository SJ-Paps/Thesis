using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SJBoxCollider2D : SJCollider2D
{
    public BoxCollider2D InnerBoxCollider2D { get; private set; }

    protected override void OnAwake()
    {
        InnerBoxCollider2D = (BoxCollider2D)InnerCollider;
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
