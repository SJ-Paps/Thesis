using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace SJ
{
    public interface IGameSettingsRepository
    {
        Task<GameSettings> GetSettings();
        Task SaveSettings();
        Task SaveSettings(GameSettings settings);
    }
}