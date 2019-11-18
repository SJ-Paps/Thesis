using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Updatables
{
    public static class UpdaterFactory
    {
        public static IUpdater Create()
        {
            return new UpdateManager();
        }
    }
}


