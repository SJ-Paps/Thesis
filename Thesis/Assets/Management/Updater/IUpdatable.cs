using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Updatables
{
    public interface IUpdatable
    {
        void DoUpdate();
        void DoLateUpdate();
        void DoFixedUpdate();
    }
}


