using UnityEngine;
using UnityEngine.Animations;

public abstract class CollectableObject : ActivableObject<IHandOwner>, ICollectable<IHandOwner> {

    public IHandOwner Owner { get; protected set; }

    private new Rigidbody2D rigidbody2D;
    private ParentConstraint parentConstraint;

    [SerializeField]
    private Transform handlePoint;

    public Transform HandlePoint
    {
        get
        {
            return handlePoint;
        }
    }

    protected Rigidbody2D Rigidbody2D
    {
        get
        {
            if(rigidbody2D == null)
            {
                rigidbody2D = GetComponentInChildren<Rigidbody2D>();
            }

            return rigidbody2D;
        }
    }

    public ParentConstraint ParentConstraint
    {
        get
        {
            if(parentConstraint == null)
            {
                parentConstraint = GetComponentInChildren<ParentConstraint>();
            }

            return parentConstraint;
        }
    }

    public bool Collect(IHandOwner user)
    {
        if(Owner == null && ValidateCollect(user))
        {
            PropagateOwnerReference(user);
            OnCollect(user);
            
            return true;
        }

        return false;
    }

    public bool Drop()
    {
        if(Owner != null && ValidateDrop())
        {
            OnDrop();
            ClearOwnerReference();

            return true;
        }

        return false;
    }

    public sealed override bool Activate(IHandOwner user)
    {
        if(Owner != null && ValidateActivation(user))
        {
            if(Active)
            {
                Active = false;
            }
            else
            {
                Active = true;
            }

            OnActivation();

            return true;
        }

        return false;
    }

    protected virtual bool ValidateCollect(IHandOwner user)
    {
        return true;
    }

    protected virtual bool ValidateDrop()
    {
        return true;
    }

    protected virtual bool ValidateActivation(IHandOwner user)
    {
        return true;
    }

    protected virtual void OnCollect(IHandOwner user)
    {

    }

    protected virtual void OnDrop()
    {

    }

    protected virtual void OnActivation()
    {

    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }

    private void ClearOwnerReference()
    {
        Owner = null;
    }
}
