using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private MonobehaviourTest[] tests;

    private void Start()
    {
        tests = new MonobehaviourTest[10];

        for(int i = 0; i < tests.Length; i++)
        {
            tests[i] = new GameObject("TEST " + (i + 1)).AddComponent<MonobehaviourTest>();
            tests[i].SetNumber(i + 1);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            tests[Random.Range(0, tests.Length)].EnableUpdate = false;
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < tests.Length; i++)
            {
                if(tests[i].EnableUpdate == false)
                {
                    tests[i].EnableUpdate = true;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            if(UpdateManager.GetInstance().IsActive)
            {
                UpdateManager.GetInstance().Disable();
            }
            else
            {
                UpdateManager.GetInstance().Enable();
            }
            
        }
    }

    private class MonobehaviourTest : SJMonoBehaviour
    {
        private int number;

        public void SetNumber(int number)
        {
            this.number = number;
        }

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

        protected override void SJUpdate()
        {
            Debug.Log(number);
        }
    }
}

