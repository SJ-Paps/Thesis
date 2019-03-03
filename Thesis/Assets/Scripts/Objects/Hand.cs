using UnityEngine;
using UnityEngine.Animations;

public class Hand : SJMonoBehaviour, IOwnable<IHandOwner>
{
    public CollectableObject CurrentCollectable { get; private set; }

    public IHandOwner Owner { get; private set; }

    public bool IsFree
    {
        get
        {
            return CurrentCollectable == null;
        }
    }

    public bool CollectObject(CollectableObject collectable)
    {
        if (IsFree && collectable.Collect(Owner))
        {
            CurrentCollectable = collectable;

            OnCollect(collectable);

            return true;
        }

        return false;
    }

    public bool DropObject()
    {
        if (!IsFree && CurrentCollectable.Drop())
        {
            OnDrop();

            CurrentCollectable = null;

            return true;
        }

        return false;
    }

    public bool ThrowObject()
    {
        if (!IsFree && CurrentCollectable is IThrowable throwable && throwable.Throw())
        {
            OnThrow();

            CurrentCollectable = null;

            return true;
        }

        return false;
    }

    public bool ActivateCurrentObject()
    {
        if (!IsFree)
        {
            return CurrentCollectable.Activate(Owner);
        }

        return false;
    }

    public void UseWeapon()
    {
        if(!IsFree && CurrentCollectable is Weapon weapon)
        {
            weapon.Use();
        }
    }

    protected virtual void OnCollect(CollectableObject collectableObject)
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = transform;
        source.weight = 1;

        collectableObject.ParentConstraint.AddSource(source);

        collectableObject.ParentConstraint.constraintActive = true;

        Vector3 offset = new Vector3(collectableObject.HandlePoint.localPosition.x * collectableObject.transform.localScale.x,
                                     collectableObject.HandlePoint.localPosition.y * collectableObject.transform.localScale.y);

        collectableObject.ParentConstraint.SetTranslationOffset(0, -offset);
    }

    protected virtual void OnDrop()
    {
        CurrentCollectable.ParentConstraint.RemoveSource(0);

        CurrentCollectable.ParentConstraint.constraintActive = false;
    }

    protected virtual void OnThrow()
    {
        OnDrop();
    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }
}
