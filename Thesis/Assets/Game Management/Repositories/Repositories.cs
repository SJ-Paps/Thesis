using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SJ.Profiles;

namespace SJ
{
    public static class Repositories
    {
        private static IProfileRepository profileRepository;
        private static IGameSettingsRepository gameSettingsRepository;

        public static IProfileRepository GetProfileRepository()
        {
            if(profileRepository == null)
            {
                profileRepository = ProfileRepositoryFactory.Create();
            }

            return profileRepository;
        }

        public static IGameSettingsRepository GetGameSettingsRepository()
        {
            if(gameSettingsRepository == null)
            {
                gameSettingsRepository = GameSettingsRepositoryFactory.Create();
            }

            return gameSettingsRepository;
        }
    }
}