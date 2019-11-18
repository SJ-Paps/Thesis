using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Audio
{
    public static class SoundServiceFactory
    {
        public static ISoundService Create()
        {
            return new SoundService();
        }
    }

}

