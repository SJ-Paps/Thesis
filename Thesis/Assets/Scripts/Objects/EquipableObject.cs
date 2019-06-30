using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public abstract class EquipableObject : CollectableObject, IEquipable
{
    [SerializeField]
    private Transform handlePoint;

    public Transform HandlePoint
    {
        get
        {
            return handlePoint;
        }
    }

    private ParentConstraint parentConstraint;

    public ParentConstraint ParentConstraint
    {
        get
        {
            if (parentConstraint == null)
            {
                parentConstraint = GetComponentInChildren<ParentConstraint>();
            }

            return parentConstraint;
        }
    }
}
