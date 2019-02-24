using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class EyeCollection : IList<Eyes>
{
    public event Action<Collider2D, Eyes> onAnyEntered;
    public event Action<Collider2D, Eyes> onAnyStay;
    public event Action<Collider2D, Eyes> onAnyExited;

    private Action<Collider2D, Eyes> onEnteredTriggerDelegate;
    private Action<Collider2D, Eyes> onStayTriggerDelegate;
    private Action<Collider2D, Eyes> onExitedTriggerDelegate;


    private List<Eyes> eyes;

    public int Count
    {
        get
        {
            return eyes.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public Eyes this[int index]
    {
        get
        {
            return eyes[index];
        }

        set
        {
            eyes[index] = value;
        }
    }

    public EyeCollection()
    {
        onEnteredTriggerDelegate = OnEnteredTrigger;
        onStayTriggerDelegate = OnStayTrigger;
        onExitedTriggerDelegate = OnExitedTrigger;

        eyes = new List<Eyes>();
    }

    public EyeCollection(IEnumerable<Eyes> collection) : this()
    {
        AddRange(collection);
    }

    private void OnEnteredTrigger(Collider2D collider, Eyes eye)
    {
        if(onAnyEntered != null)
        {
            onAnyEntered(collider, eye);
        }
    }

    private void OnStayTrigger(Collider2D collider, Eyes eye)
    {
        if(onAnyStay != null)
        {
            onAnyStay(collider, eye);
        }
    }

    private void OnExitedTrigger(Collider2D collider, Eyes eye)
    {
        if(onAnyExited != null)
        {
            onAnyExited(collider, eye);
        }
    }

    private void Bind(Eyes eye)
    {
        if(eye == null)
        {
            throw new ArgumentNullException();
        }

        eye.onEntered += onEnteredTriggerDelegate;
        eye.onStay += onStayTriggerDelegate;
        eye.onExited += onExitedTriggerDelegate;
    }

    private void Clear(Eyes eye)
    {
        if (eye == null)
        {
            throw new ArgumentNullException();
        }

        eye.onEntered -= onEnteredTriggerDelegate;
        eye.onStay -= onStayTriggerDelegate;
        eye.onExited -= onExitedTriggerDelegate;
    }

    public IEnumerator<Eyes> GetEnumerator()
    {
        return eyes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Eyes item)
    {
        Bind(item);

        eyes.Add(item);
    }

    public void AddRange(IEnumerable<Eyes> collection)
    {
        foreach(Eyes eye in collection)
        {
            Bind(eye);
        }

        eyes.AddRange(collection);
    }

    public void Clear()
    {
        for(int i = 0; i < eyes.Count; i++)
        {
            Clear(eyes[i]);
        }

        eyes.Clear();
    }

    public bool Contains(Eyes item)
    {
        return eyes.Contains(item);
    }

    public void CopyTo(Eyes[] array, int arrayIndex)
    {
        eyes.CopyTo(array, arrayIndex);
    }

    public bool Remove(Eyes item)
    {
        if(eyes.Remove(item))
        {
            Clear(item);
            return true;
        }

        return false;
    }

    public int IndexOf(Eyes item)
    {
        return eyes.IndexOf(item);
    }

    public void Insert(int index, Eyes item)
    {
        Bind(item);

        eyes.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        Remove(eyes[index]);
    }
}

public class Eyes : SJMonoBehaviour
{
    protected new SJCollider2D collider;

    [SerializeField]
    protected Transform eyePoint;

    public SJCollider2D Collider
    {
        get
        {
            return collider;
        }

        set
        {
            bool shouldReBind = collider != value;

            collider = value;

            if(shouldReBind)
            {
                Bind(collider);
            }
        }
    }

    public Transform EyePoint
    {
        get
        {
            return eyePoint;
        }

        set
        {
            eyePoint = value;
        }
    }

    public event Action<Collider2D, Eyes> onEntered;
    public event Action<Collider2D, Eyes> onStay;
    public event Action<Collider2D, Eyes> onExited;

    private Action<Collider2D> onEnteredTriggerDelegate;
    private Action<Collider2D> onStayTriggerDelegate;
    private Action<Collider2D> onExitedTriggerDelegate;

    protected override void Awake()
    {
        base.Awake();

        collider = GetComponent<SJCollider2D>();

        onEnteredTriggerDelegate = OnEnteredTrigger;
        onStayTriggerDelegate = OnStayTrigger;
        onExitedTriggerDelegate = OnExitedTrigger;
        
    }

    protected override void Start()
    {
        base.Start();

        Bind(Collider);
    }

    private void Bind(SJCollider2D collider)
    {
        ClearPrevious();

        if (collider != null)
        {
            this.collider = collider;

            Collider.IsTrigger = true;

            Collider.onEnteredTrigger += onEnteredTriggerDelegate;
            Collider.onStayTrigger += onStayTriggerDelegate;
            Collider.onExitedTrigger += onExitedTriggerDelegate;
        }
    }

    private void ClearPrevious()
    {
        if(Collider != null)
        {
            Collider.onEnteredTrigger -= onEnteredTriggerDelegate;
            Collider.onStayTrigger -= onStayTriggerDelegate;
            Collider.onExitedTrigger -= onExitedTriggerDelegate;
        }
    }

    private void OnEnteredTrigger(Collider2D collider)
    {
        if(onEntered != null)
        {
            onEntered(collider, this);
        }
    }

    private void OnStayTrigger(Collider2D collider)
    {
        if (onStay != null)
        {
            onStay(collider, this);
        }
    }

    private void OnExitedTrigger(Collider2D collider)
    {
        if(onExited != null)
        {
            onExited(collider, this);
        }
    }

    public bool IsVisible(Collider2D collider, int visionBlockingLayerMask, int targetLayerMask)
    {
        if (eyePoint != null && Collider != null)
        {
            if(Collider.OverlapPoint(eyePoint.position) && Collider.IsTouching(collider))
            {
                int finalLayerMask = visionBlockingLayerMask | targetLayerMask;

                float distance;

                if (Collider.bounds.size.x > Collider.bounds.size.y)
                {
                    distance = Collider.bounds.size.x;
                }
                else
                {
                    distance = Collider.bounds.size.y;
                }

                RaycastHit2D hit = Physics2D.Raycast(eyePoint.position, eyePoint.up, distance, finalLayerMask);

                if (hit && Collider.OverlapPoint(hit.point))
                {
                    return hit.collider == collider;
                }
            }
        }

        return false;
    }

    public bool IsVisible(Collider2D collider, int visionBlockingLayerMask, int targetLayerMask, out RaycastHit2D hitInfo)
    {
        hitInfo = default;

        if (eyePoint != null && Collider != null)
        {
            if (Collider.OverlapPoint(eyePoint.position))
            {
                int finalLayerMask = visionBlockingLayerMask | targetLayerMask;

                float distance;

                if (Collider.bounds.size.x > Collider.bounds.size.y)
                {
                    distance = Collider.bounds.size.x;
                }
                else
                {
                    distance = Collider.bounds.size.y;
                }

                hitInfo = Physics2D.Raycast(eyePoint.position, eyePoint.up, distance, finalLayerMask);

                if (hitInfo && Collider.OverlapPoint(hitInfo.point))
                {
                    return hitInfo.collider == collider;
                }
            }
        }

        return false;
    }
}