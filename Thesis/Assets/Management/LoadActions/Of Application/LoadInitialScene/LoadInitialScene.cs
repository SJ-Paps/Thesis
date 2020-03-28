using System;
using UniRx;
using UnityEngine.SceneManagement;

namespace SJ.Management
{
    public class LoadInitialScene : LoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Observable.Start(() => SceneManager.LoadScene("Menu"), Scheduler.Immediate);
        }
    }
}