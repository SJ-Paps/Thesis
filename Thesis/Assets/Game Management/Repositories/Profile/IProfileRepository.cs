using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SJ.Save;

namespace SJ.Profiles
{
    public interface IProfileRepository
    {
        Task SaveProfile(ProfileData profileData, SaveData saveData);
        Task<SaveData> GetSaveDataFromProfile(string profile);
        Task DeleteProfile(string profile);
        Task DeleteAllProfiles();
        Task<ProfileData[]> GetAllProfileData();
        Task<ProfileData> GetProfileDataFrom(string profile);
        Task<bool> Exists(string profile);
    }
}