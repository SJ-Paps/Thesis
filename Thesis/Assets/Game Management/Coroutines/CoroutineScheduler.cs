using Paps.Unity;
using UnityEngine;
using System.Collections;

namespace SJ.Coroutines
{
    public class UnityCoroutineScheduler : ICoroutineScheduler
    {
        private class UnityCoroutineSchedulerMonobehaviour : MonoBehaviour
        {
            private void Awake()
            {
                name = GetType().Name;
            }
        }

        private UnityCoroutineSchedulerMonobehaviour instance;

        public UnityCoroutineScheduler()
        {
            instance = new GameObject().AddComponent<UnityCoroutineSchedulerMonobehaviour>();

            UnityUtil.DontDestroyOnLoad(instance.gameObject);
        }

        public void StopCoroutine(IEnumerator coroutine)
        {
            try
            {
                instance.StopCoroutine(coroutine);
            }
            catch { }
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            instance.StartCoroutine(coroutine);
        }

        public void StopAllCoroutines()
        {
            instance.StopAllCoroutines();
        }
        
    }

}
