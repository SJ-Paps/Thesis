using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterConfiguration : Configuration
{
    [SerializeField]
    private Blackboard blackboard;

    public Blackboard Blackboard { get { return blackboard; } }
}
