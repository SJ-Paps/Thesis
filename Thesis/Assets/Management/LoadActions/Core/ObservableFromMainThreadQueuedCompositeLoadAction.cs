using System;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    [CreateAssetMenu(menuName = "SJ/Load Actions/Queued/Observable from coroutine")]
    public class ObservableFromMainThreadQueuedCompositeLoadAction : CompositeLoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Observable.Zip(loadActions)
                .Select(_ => Unit.Default)
                .ObserveOnMainThread();
        }
    }
}