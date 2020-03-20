using System;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    [CreateAssetMenu(menuName = "Load Actions/Queued/Observable immediately")]
    public class ImmediateQueuedCompositeLoadAction : CompositeLoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Observable.Zip(loadActions)
                .Select(_ => Unit.Default)
                .ObserveOn(Scheduler.Immediate);
        }
    }
}