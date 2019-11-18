using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Coroutines
{
    public static class CoroutineSchedulerUnityExtensions
    {
        public static AwaitCoroutine AwaitCoroutine(this ICoroutineScheduler coroutineScheduler, IEnumerator coroutine)
        {
            return new AwaitCoroutine(coroutine);
        }
    }
}