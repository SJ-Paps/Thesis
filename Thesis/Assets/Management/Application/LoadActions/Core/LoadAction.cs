using System;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    public abstract class LoadAction : ScriptableObject, IObservable<Unit>
    {
        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            return Load().Subscribe(observer);
        }

        protected abstract IObservable<Unit> Load();
    }
}