using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UniRx;

namespace SJ
{
    public interface IGameSettingsRepository
    {
        IObservable<GameSettings> GetSettings();
        IObservable<Unit> SaveSettings();
    }
}