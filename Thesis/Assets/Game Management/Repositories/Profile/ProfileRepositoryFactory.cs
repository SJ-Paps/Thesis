﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Profiles
{
    public static class ProfileRepositoryFactory
    {
        public static IProfileRepository Create()
        {
            return new WindowsFileSystemProfileRepository(Serializers.GetSaveSerializer());
        }
    }
}


