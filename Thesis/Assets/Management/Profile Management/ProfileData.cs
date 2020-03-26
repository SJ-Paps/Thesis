using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Management
{
    public struct ProfileData
    {
        public string name;

        public static bool IsValid(ProfileData data)
        {
            return string.IsNullOrEmpty(data.name) == false;
        }
    }
}