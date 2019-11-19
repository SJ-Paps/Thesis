using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace SJ
{
    public static class GameSettingsRepositoryExtensions
    {
        public static GameSettings GetSettingsSynchronously(this IGameSettingsRepository gameSettingsRepository)
        {
            Task<GameSettings> task = gameSettingsRepository.GetSettings();

            task.Wait();

            if(task.IsFaulted)
            {
                throw task.Exception;
            }
            else
            {
                return task.Result;
            }
        }

        public static void SaveSettingsSynchronously(this IGameSettingsRepository gameSettingsRepository)
        {
            Task task = gameSettingsRepository.SaveSettings();

            task.Wait();

            if(task.IsFaulted)
            {
                throw task.Exception;
            }
        }
    }
}


