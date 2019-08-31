using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : SJMonoBehaviour
{
    private MonobehaviourTest test;

    protected override void SJStart()
    {
        test = new GameObject("LOLO").AddComponent<MonobehaviourTest>();
    }

    protected override void SJUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            test.EnableUpdate = false;
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            test.EnableUpdate = true;
        }
    }

    private class MonobehaviourTest : SJMonoBehaviour
    {
        protected override void SJOnEnable()
        {
            Debug.Log(gameObject.activeSelf);
            Debug.Log(enabled);
        }

        protected override void SJOnDisable()
        {
            Debug.Log(gameObject.activeSelf);
            Debug.Log(enabled);
        }
    }
}

