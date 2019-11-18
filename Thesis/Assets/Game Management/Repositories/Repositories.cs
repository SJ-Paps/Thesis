using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SJ.Profiles;

namespace SJ
{
    public static class Repositories
    {
        private static IProfileRepository profileRepository;

        public static IProfileRepository GetProfileRepository()
        {
            if(profileRepository == null)
            {
                profileRepository = ProfileRepositoryFactory.Create();
            }

            return profileRepository;
        }
    }
}