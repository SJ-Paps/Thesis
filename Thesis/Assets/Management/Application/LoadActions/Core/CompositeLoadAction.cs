using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    public abstract class CompositeLoadAction : LoadAction
    {
        [SerializeField]
        protected LoadAction[] loadActions;
    }
}