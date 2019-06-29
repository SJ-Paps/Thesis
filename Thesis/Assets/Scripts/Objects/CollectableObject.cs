using UnityEngine;
using UnityEngine.Animations;

public abstract class CollectableObject : ActivableObject<Character>, ICollectable {
    
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

    public bool Collect()
    {
        if(ValidateCollect())
        {
            OnCollect();
            
            return true;
        }

        return false;
    }

    public bool Drop()
    {
        if(ValidateDrop())
        {
            OnDrop();

            return true;
        }

        return false;
    }

    public sealed override bool Activate(Character user)
    {
        if(ValidateActivation(user))
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

    protected virtual bool ValidateCollect()
    {
        return true;
    }

    protected virtual bool ValidateDrop()
    {
        return true;
    }

    protected virtual bool ValidateActivation(Character user)
    {
        return true;
    }

    protected virtual void OnCollect()
    {

    }

    protected virtual void OnDrop()
    {

    }

    protected virtual void OnActivation()
    {

    }
}
