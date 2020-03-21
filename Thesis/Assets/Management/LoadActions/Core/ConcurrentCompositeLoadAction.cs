using System;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    [CreateAssetMenu(menuName = "SJ/Load Actions/Concurrent/Observable from thread pool")]
    public class ConcurrentCompositeLoadAction : CompositeLoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Observable.Merge(loadActions);
        }
    }
}