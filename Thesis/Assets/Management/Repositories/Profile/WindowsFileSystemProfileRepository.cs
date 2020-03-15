using SJ.Save;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;

namespace SJ.Profiles
{
    public class WindowsFileSystemProfileRepository : IProfileRepository
    {
        private static readonly string profileDirectory = Path.Combine(UnityEngine.Application.dataPath, "../saves/profiles");
        private static readonly string profileFileName = "profile.sj";
        private static readonly string saveFileName = "save.sj";

        private ISaveSerializer saveSerializer;

        private Dictionary<string, ProfileSaveDataTuple> profiles;

        private bool cached;

        public WindowsFileSystemProfileRepository(ISaveSerializer saveSerializer)
        {
            this.saveSerializer = saveSerializer;
            profiles = new Dictionary<string, ProfileSaveDataTuple>();
        }

        public IObservable<Unit> CreateProfile(string profile, ProfileData profileData, SaveData saveData)
        {
            return LoadProfiles()
                .Do(_ => profiles.Add(profile, new ProfileSaveDataTuple() { profileData = profileData, saveData = saveData }))
                .SelectMany(_ => SaveProfile(profile));
        }

        public IObservable<Unit> UpdateProfileData(string profile, ProfileData profileData)
        {
            return LoadProfiles()
                .Do(_ => profiles[profile].profileData = profileData)
                .SelectMany(_ => SaveProfile(profile));
        }

        public IObservable<Unit> SaveOnProfile(string profile, SaveData saveData)
        {
            return LoadProfiles()
                .Do(_ => profiles[profile].saveData = saveData)
                .SelectMany(_ => SaveProfile(profile));
        }

        public IObservable<ProfileData> GetProfileDataFrom(string profile)
        {
            return LoadProfiles()
                .Select(_ => profiles[profile].profileData);
        }

        public IObservable<SaveData> GetSaveDataFrom(string profile)
        {
            return LoadProfiles()
                .Select(_ => profiles[profile].saveData);
        }

        public IObservable<Unit> DeleteProfile(string profile)
        {
            return LoadProfiles()
                .Do(_ => DeleteProfileAndDirectory(profile));
        }

        public IObservable<Unit> DeleteAllProfiles()
        {
            return LoadProfiles()
                .Do(_ => DeleteAllProfilesAndDirectories());
        }

        public IObservable<string[]> GetAllProfiles()
        {
            return LoadProfiles()
                .Select(_ => profiles.Keys.ToArray());
        }

        public IObservable<bool> Exists(string profile)
        {
            return LoadProfiles()
                .Select(_ => profiles.ContainsKey(profile));
        }

        private IObservable<Unit> LoadProfiles()
        {
            if (cached)
                return Observable.ReturnUnit();

            return Observable.Create<Unit>(observer =>
            {
                var profileData = LoadProfileData();
                var saveData = LoadSaveData();

                for (int i = 0; i < profileData.Count; i++)
                {
                    profiles.Add(profileData[i].name, new ProfileSaveDataTuple() { profileData = profileData[i], saveData = saveData[i] });
                }

                cached = true;

                observer.OnNext(Unit.Default);
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        private List<ProfileData> LoadProfileData()
        {
            string[] profileFilePaths = null;
            List<ProfileData> profileData = new List<ProfileData>();

            try
            {
                profileFilePaths = Directory.GetFiles(profileDirectory, profileFileName, SearchOption.AllDirectories);
            }
            catch { }

            if (profileFilePaths != null)
            {
                foreach (var profileFile in profileFilePaths)
                    profileData.Add(LoadProfileDataFrom(profileFile));
            }

            return profileData;
        }

        private ProfileData LoadProfileDataFrom(string profileFile)
        {
            string serialized = File.ReadAllText(profileFile);

            SaveData[] saves = saveSerializer.Deserialize(serialized);

            return (ProfileData)saves[0].saveObject;
        }

        private List<SaveData> LoadSaveData()
        {
            string[] saveFilePaths = null;
            List<SaveData> saveData = new List<SaveData>();

            try
            {
                saveFilePaths = Directory.GetFiles(profileDirectory, saveFileName, SearchOption.AllDirectories);
            }
            catch { }

            if (saveFilePaths != null)
            {
                foreach (var saveFile in saveFilePaths)
                    saveData.Add(LoadSaveDataFrom(saveFile));
            }

            return saveData;
        }

        private SaveData LoadSaveDataFrom(string saveFile)
        {
            string serialized = File.ReadAllText(saveFile);

            SaveData[] saves = saveSerializer.Deserialize(serialized);

            return saves[0];
        }

        private IObservable<Unit> SaveProfiles()
        {
            return Observable.Create<Unit>(observer =>
            {
                foreach (var profile in profiles)
                {
                    SaveProfileSynchronously(profile.Key);
                }

                observer.OnNext(Unit.Default);
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        private IObservable<Unit> SaveProfile(string profile)
        {
            return Observable.Create<Unit>(observer =>
            {
                SaveProfileSynchronously(profile);

                observer.OnNext(Unit.Default);
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        private void SaveProfileSynchronously(string profile)
        {
            var profileTuple = profiles[profile];

            var profileName = profile;
            var profileData = profileTuple.profileData;
            var saveData = profileTuple.saveData;

            var directory = EnsureProfileHasSaveDirectoryAndReturnIt(profileName);

            SaveData profileSaveData = new SaveData(profileName, profileData);

            string profileFilePath = Path.Combine(directory, profileFileName);
            string saveFilePath = Path.Combine(directory, saveFileName);

            string serializedProfileData = saveSerializer.Serialize(profileSaveData);
            string serializedSaves = saveSerializer.Serialize(saveData);

            File.WriteAllText(profileFilePath, serializedProfileData);
            File.WriteAllText(saveFilePath, serializedSaves);
        }

        private string EnsureProfileHasSaveDirectoryAndReturnIt(string profile)
        {
            string currentProfileDirectory = GetProfileDirectory(profile);

            if (Directory.Exists(currentProfileDirectory) == false)
            {
                Directory.CreateDirectory(currentProfileDirectory);
            }

            return currentProfileDirectory;
        }

        private string GetProfileDirectory(string profileName)
        {
            return Path.Combine(profileDirectory, profileName);
        }

        private void DeleteProfileAndDirectory(string profileName)
        {
            Directory.Delete(GetProfileDirectory(profileName), true);
            profiles.Remove(profileName);
        }

        private void DeleteAllProfilesAndDirectories()
        {
            var keyValues = profiles.ToArray();

            for (int i = 0; i < keyValues.Length; i++)
            {
                DeleteProfileAndDirectory(keyValues[i].Key);
                i--;
            }
        }

        private class ProfileSaveDataTuple
        {
            public ProfileData profileData;
            public SaveData saveData;
        }
    }
}