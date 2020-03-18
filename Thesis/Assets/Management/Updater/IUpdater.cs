using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Updatables
{
    public interface IUpdater
    {
        bool IsActive { get; }

        void Subscribe(IUpdatable updatable);
        void Unsubscribe(IUpdatable updateable);
        void Enable();
        void Disable();
    }
}