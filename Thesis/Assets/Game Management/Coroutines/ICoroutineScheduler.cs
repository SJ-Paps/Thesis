using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Coroutines
{
    public interface ICoroutineScheduler
    {
        void StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(IEnumerator coroutine);
        void StopAllCoroutines();
    }
}


