using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ
{
    public static class GameSettingsRepositoryFactory
    {
        public static IGameSettingsRepository Create()
        {
            return new WindowsFileSystemGameSettingsRepository(Serializers.GetSaveSerializer());
        }
    }
}