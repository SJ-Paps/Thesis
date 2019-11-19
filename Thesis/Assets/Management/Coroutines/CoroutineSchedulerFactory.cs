using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Coroutines
{
    public static class CoroutineSchedulerFactory
    {
        public static ICoroutineScheduler Create()
        {
            return new UnityCoroutineScheduler();
        }
    }
}
