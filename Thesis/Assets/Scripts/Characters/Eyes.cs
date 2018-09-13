using UnityEngine;
using System;


public class Eyes : MonoBehaviour
{
    [SerializeField]
    private TriggerEnterExit2D nearVision, mediumVision, distantVision;

    public TriggerEnterExit2D NearVision { get { return nearVision; } }
    public TriggerEnterExit2D MediumVision { get { return mediumVision; } }
    public TriggerEnterExit2D DistantVision { get { return distantVision; } }

    public event Action<Collider2D> onNearVisionEnter;
    public event Action<Collider2D> onMediumVisionEnter;
    public event Action<Collider2D> onDistantVisionEnter;
    public event Action<Collider2D> onNearVisionExit;
    public event Action<Collider2D> onMediumVisionExit;
    public event Action<Collider2D> onDistantVisionExit;
    public event Action<Collider2D> onNearVisionStay;
    public event Action<Collider2D> onMediumVisionStay;
    public event Action<Collider2D> onDistantVisionStay;

    void Awake()
    {
        if (NearVision != null)
        {
            NearVision.onEntered += OnNearVisionEntered;
            NearVision.onExited += OnNearVisionExited;
            NearVision.onStay += OnNearVisionStay;
        }

        if (MediumVision != null)
        {
            MediumVision.onEntered += OnMediumVisionEntered;
            MediumVision.onExited += OnMediumVisionExited;
            MediumVision.onStay += OnMediumVisionStay;
        }

        if (DistantVision != null)
        {
            DistantVision.onEntered += OnDistantVisionEntered;
            DistantVision.onExited += OnDistantVisionExited;
            DistantVision.onStay += OnDistantVisionStay;
        }
    }

    private void OnNearVisionEntered(Collider2D collider)
    {
        if(onNearVisionEnter != null)
        {
            onNearVisionEnter(collider);
        }
    }

    private void OnMediumVisionEntered(Collider2D collider)
    {
        if(onMediumVisionEnter != null)
        {
            onMediumVisionEnter(collider);
        }
    }

    private void OnDistantVisionEntered(Collider2D collider)
    {
        if(onDistantVisionEnter != null)
        {
            onDistantVisionEnter(collider);
        }
    }

    private void OnNearVisionExited(Collider2D collider)
    {
        if(onNearVisionExit != null)
        {
            onNearVisionExit(collider);
        }
    }

    private void OnMediumVisionExited(Collider2D collider)
    {
        if(onMediumVisionExit != null)
        {
            onMediumVisionExit(collider);
        }
    }

    private void OnDistantVisionExited(Collider2D collider)
    {
        if(onDistantVisionExit != null)
        {
            onDistantVisionExit(collider);
        }
    }


    private void OnNearVisionStay(Collider2D collider)
    {
        if (onNearVisionStay != null)
        {
            onNearVisionStay(collider);
        }
    }

    private void OnMediumVisionStay(Collider2D collider)
    {
        if (onMediumVisionStay != null)
        {
            onMediumVisionStay(collider);
        }
    }

    private void OnDistantVisionStay(Collider2D collider)
    {
        if (onDistantVisionStay != null)
        {
            onDistantVisionStay(collider);
        }
    }
}
