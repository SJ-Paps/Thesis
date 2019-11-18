using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Profiles
{
    public static class ProfileRepositoryFactory
    {
        public static IProfileRepository Create()
        {
#if UNITY_STANDALONE_WIN
            return new WindowsFileSystemProfileRepository();
#endif
        }
    }
}


