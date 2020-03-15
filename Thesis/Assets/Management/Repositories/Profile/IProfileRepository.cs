using SJ.Save;
using System;
using UniRx;

namespace SJ.Profiles
{
    public interface IProfileRepository
    {
        IObservable<Unit> CreateProfile(string profile, ProfileData profileData, SaveData saveData);
        IObservable<Unit> UpdateProfileData(string profile, ProfileData profileData);
        IObservable<Unit> SaveOnProfile(string profile, SaveData saveData);
        IObservable<ProfileData> GetProfileDataFrom(string profile);
        IObservable<SaveData> GetSaveDataFrom(string profile);
        IObservable<Unit> DeleteProfile(string profile);
        IObservable<Unit> DeleteAllProfiles();
        IObservable<string[]> GetAllProfiles();
        IObservable<bool> Exists(string profile);
    }
}