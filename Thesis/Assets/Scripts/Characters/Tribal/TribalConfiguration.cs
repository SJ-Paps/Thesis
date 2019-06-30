using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalConfiguration : CharacterConfiguration
{
    [SerializeField]
    private Transform hand;

    public Transform Hand { get { return hand; } }
}
