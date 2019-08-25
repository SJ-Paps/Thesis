using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : SJMonoBehaviour
{
    [SerializeField]
    private ScriptableLoadRoutine[] loadRoutines;

    protected override void Awake()
    {
        base.Awake();

        CoroutineManager.GetInstance().StartCoroutine(WaitLoad());
    }

    private IEnumerator WaitLoad()
    {
        for (int i = 0; i < loadRoutines.Length; i++)
        {
            var current = loadRoutines[i];

            current.Load();

            while (current.IsCompleted == false)
            {
                yield return null;
            }

            if (current.IsFaulted)
            {
                if (current.ShouldRetry())
                {
                    i--;
                }
                else
                {
                    current.ThrowExceptionIfNeeded();
                }
            }
        }
    }
}
