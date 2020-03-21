using System;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    [CreateAssetMenu(menuName = "SJ/Load Actions/Concurrent/Observable from main thread")]
    public class ObservableFromMainThreadConcurrentCompositeLoadAction : CompositeLoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Observable.Merge(loadActions)
                .ObserveOnMainThread();
        }
    }
}


