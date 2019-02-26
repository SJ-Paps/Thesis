using UnityEngine;

public class Hand : SJMonoBehaviour, IOwnable<IHandOwner>
{
    public IActivable<IHandOwner> CurrentActivable { get; private set; }

    public IHandOwner Owner { get; private set; }

    private SpringJoint2D springJoint2d;

    public bool IsFree
    {
        get
        {
            return CurrentActivable == null;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Rigidbody2D rigidbody2D = transform.root.GetComponentInChildren<Rigidbody2D>();

        springJoint2d = rigidbody2D.gameObject.AddComponent<SpringJoint2D>();
        springJoint2d.enabled = false;
        springJoint2d.autoConfigureDistance = false;
        springJoint2d.distance = 0.2f;
        springJoint2d.dampingRatio = 1;
        springJoint2d.enableCollision = true;
    }

    public bool CollectObject(CollectableObject collectable)
    {
        if (IsFree && collectable.Collect(Owner))
        {
            CurrentActivable = collectable;

            return true;
        }

        return false;
    }

    public bool DropObject()
    {
        if (!IsFree && CurrentActivable is ICollectable<IHandOwner> cached && cached.Drop())
        {
            CurrentActivable = null;

            return true;
        }

        return false;
    }

    public bool ThrowObject()
    {
        if (!IsFree && CurrentActivable is IThrowable cached && cached.Throw())
        {
            CurrentActivable = null;

            return true;
        }

        return false;
    }

    public bool ActivateCurrentObject()
    {
        if (!IsFree)
        {
            return CurrentActivable.Activate(Owner);
        }

        return false;
    }

    public bool PushOrPullObject(MovableObject movable)
    {
        if (IsFree)
        {
            if(movable.Activate(Owner))
            {
                springJoint2d.enabled = true;
                springJoint2d.connectedBody = movable.Rigidbody2D;
                CurrentActivable = movable;

                return true;
            }
        }

        return false;
    }

    public void ReleaseMovableObject()
    {
        if(!IsFree && CurrentActivable is MovableObject)
        {
            springJoint2d.enabled = false;
            springJoint2d.connectedBody = null;
            CurrentActivable = null;
        }
    }

    public bool ActivateObject(IActivable<IHandOwner> activable)
    {
        return activable.Activate(Owner);
    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }
}
