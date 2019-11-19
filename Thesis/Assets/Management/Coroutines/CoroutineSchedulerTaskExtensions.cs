using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace SJ.Coroutines
{
    public static class CoroutineSchedulerTaskExtensions
    {
        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Task task, Action onSuccess, Action<Exception> onFail, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess, onFail, onComplete));
        }

        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Task task, Action onSuccess)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess));
        }

        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Task task, Action onSuccess, Action<Exception> onFail)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess, onFail));
        }

        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Task task, Action onSuccess, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess, null, onComplete));
        }

        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Action task, Action onSuccess, Action<Exception> onFail, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess, onFail, onComplete));
        }

        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Action task, Action onSuccess)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess));
        }

        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Action task, Action onSuccess, Action<Exception> onFail)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess, onFail));
        }

        public static void AwaitTask(this ICoroutineScheduler coroutineScheduler, Action task, Action onSuccess, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess, null, onComplete));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Task<TResult> task, Action<TResult> onSuccess, Action<Exception> onFail, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess, onFail, onComplete));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Task<TResult> task, Action<TResult> onSuccess)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Task<TResult> task, Action<TResult> onSuccess, Action<Exception> onFail)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess, onFail));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Task<TResult> task, Action<TResult> onSuccess, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(task, onSuccess, null, onComplete));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Func<TResult> task, Action<TResult> onSuccess, Action<Exception> onFail, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess, onFail, onComplete));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Func<TResult> task, Action<TResult> onSuccess)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Func<TResult> task, Action<TResult> onSuccess, Action<Exception> onFail)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess, onFail));
        }

        public static void AwaitTask<TResult>(this ICoroutineScheduler coroutineScheduler, Func<TResult> task, Action<TResult> onSuccess, Action onComplete)
        {
            coroutineScheduler.StartCoroutine(AwaitTaskCoroutine(Task.Run(task), onSuccess, null, onComplete));
        }

        private static IEnumerator AwaitTaskCoroutine(Task task, Action onSuccess, Action<Exception> onFail = null, Action onComplete = null)
        {
            while(task.IsCompleted == false)
            {
                yield return null;
            }

            if(task.IsFaulted)
            {
                if(onFail != null)
                {
                    onFail(task.Exception);
                }
                else
                {
                    throw task.Exception;
                }
            }
            else
            {
                onSuccess();
            }

            task.Dispose();

            if(onComplete != null)
            {
                onComplete();
            }
        }

        private static IEnumerator AwaitTaskCoroutine<TResult>(Task<TResult> task, Action<TResult> onSuccess, Action<Exception> onFail = null, Action onComplete = null)
        {
            while (task.IsCompleted == false)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                if (onFail != null)
                {
                    onFail(task.Exception);
                }
                else
                {
                    throw task.Exception;
                }
            }
            else
            {
                onSuccess(task.Result);
            }

            task.Dispose();

            if (onComplete != null)
            {
                onComplete();
            }
        }
    }
}