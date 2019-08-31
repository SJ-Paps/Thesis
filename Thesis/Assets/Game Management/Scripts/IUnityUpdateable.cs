using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnityUpdateable
{
    void DoUpdate();
    void DoLateUpdate();
    void DoFixedUpdate();
}
