using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAControllerConfiguration : Configuration
{
    [SerializeField]
    private Blackboard blackboard;

    public Blackboard Blackboard { get { return blackboard; } }
}
