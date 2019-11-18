using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System;
using SJ.Profiles.Exceptions;
using SJ.Save;

namespace SJ.Profiles
{
    public class WindowsFileSystemProfileRepository : IProfileRepository
    {
        private static readonly string profileDirectory = Path.Combine(UnityEngine.Application.dataPath, "../saves/profiles");
        private static readonly string profileFileName = "profile.sj";
        private static readonly string saveFileName = "save.sj";

        private ISaveSerializer saveSerializer;

        public WindowsFileSystemProfileRepository(ISaveSerializer saveSerializer)
        {
            this.saveSerializer = saveSerializer;
        }

        public Task DeleteProfile(string profile)
        {
            return Task.Run(() => InternalDeleteProfile(profile));
        }

        private void InternalDeleteProfile(string profileName)
        {
            if (ProfileExistsAndIsValid(profileName))
            {
                Directory.Delete(GetProfileDirectory(profileName), true);
            }
        }

        public Task DeleteAllProfiles()
        {
            return Task.Run(() => InternalDeleteAllProfiles());
        }

        private void InternalDeleteAllProfiles()
        {
            string[] allProfileDirectories = GetAllProfileDirectories();

            for (int i = 0; i < allProfileDirectories.Length; i++)
            {
                Directory.Delete(allProfileDirectories[i], true);
            }
        }

        private string[] GetAllProfileDirectories()
        {
            string[] profileArray = null;

            try
            {
                profileArray = Directory.GetFiles(profileDirectory, profileFileName, SearchOption.AllDirectories);
            }
            catch { }

            if (profileArray != null)
            {
                for (int i = 0; i < profileArray.Length; i++)
                {
                    profileArray[i] = Path.GetDirectoryName(profileArray[i]);
                }
            }

            return profileArray;
        }

        public bool ProfileExistsAndIsValid(string profileName)
        {
            string profileDirectory = GetProfileDirectory(profileName);

            string currentProfileFilePath = Path.Combine(profileDirectory, profileFileName);
            string currentProfileSaveDataFilePath = Path.Combine(profileDirectory, saveFileName);

            return File.Exists(currentProfileFilePath) && File.Exists(currentProfileSaveDataFilePath);
        }

        public Task<ProfileData[]> GetAllProfileData()
        {
            return Task.Run(() => InternalGetAllProfiles());
        }

        private ProfileData[] InternalGetAllProfiles()
        {
            string[] profileFilePaths = null;

            try
            {
                profileFilePaths = Directory.GetFiles(profileDirectory, profileFileName, SearchOption.AllDirectories);
            }
            catch { }


            List<ProfileData> profiles = null;

            if (profileFilePaths != null)
            {
                for (int i = 0; i < profileFilePaths.Length; i++)
                {
                    string profileName = Path.GetFileName(Path.GetDirectoryName(profileFilePaths[i]));

                    if (ProfileExistsAndIsValid(profileName))
                    {
                        ProfileData data = InternalGetProfileDataFrom(profileName);

                        if (ProfileData.IsValid(data))
                        {
                            if (profiles == null)
                            {
                                profiles = new List<ProfileData>();
                            }

                            profiles.Add(data);
                        }
                    }
                }
            }

            if (profiles == null)
            {
                return null;
            }
            else
            {
                return profiles.ToArray();
            }
        }

        public Task<ProfileData> GetProfileDataFrom(string profile)
        {
            return Task.Run(() => InternalGetProfileDataFrom(profile));
        }

        public ProfileData InternalGetProfileDataFrom(string profileName)
        {
            if (ProfileExistsAndIsValid(profileName))
            {
                string profileFilePath = GetProfileFilePath(profileName);

                string serialized = File.ReadAllText(profileFilePath);

                SaveData[] saves = saveSerializer.Deserialize(serialized);

                if (saves != null && saves.Length > 0)
                {
                    return (ProfileData)saves[0].saveObject;
                }
            }

            throw new NonExistentProfileException();
        }

        public Task<SaveData> GetSaveDataFromProfile(string profile)
        {
            return Task.Run(() => InternalGetSaveDataFromProfile(profile));
        }

        private SaveData InternalGetSaveDataFromProfile(string profileName)
        {
            if (ProfileExistsAndIsValid(profileName))
            {
                string saveDataFilePath = Path.Combine(GetProfileDirectory(profileName), saveFileName);

                if (File.Exists(saveDataFilePath))
                {
                    string serialized = File.ReadAllText(saveDataFilePath);

                    SaveData[] saves = saveSerializer.Deserialize(serialized);

                    if (saves != null && saves.Length > 0)
                    {
                        SaveData profileSaveData = saves[0];

                        return profileSaveData;
                    }
                }
            }

            throw new InvalidOperationException("provided profile name does not exists or is invalid");
        }

        public Task SaveProfile(ProfileData profileData, SaveData saveData)
        {
            return Task.Run(() => InternalSaveProfile(profileData, saveData));
        }

        private void InternalSaveProfile(ProfileData profileData, SaveData saveData)
        {
            string currentProfileDirectory = GetProfileDirectory(profileData.name);

            if (Directory.Exists(currentProfileDirectory) == false)
            {
                Directory.CreateDirectory(currentProfileDirectory);
            }

            SaveData profileSaveData = new SaveData(profileData.name, profileData);

            string profileFilePath = Path.Combine(currentProfileDirectory, profileFileName);
            string saveFilePath = Path.Combine(currentProfileDirectory, saveFileName);

            string serializedProfileData = saveSerializer.Serialize(profileSaveData);
            string serializedSaves = saveSerializer.Serialize(saveData);

            File.WriteAllText(profileFilePath, serializedProfileData);
            File.WriteAllText(saveFilePath, serializedSaves);
        }

        private string GetProfileDirectory(string profileName)
        {
            return Path.Combine(profileDirectory, profileName);
        }

        private string GetProfileFilePath(string profileName)
        {
            return Path.Combine(GetProfileDirectory(profileName), profileFileName);
        }

        public Task<bool> Exists(string profile)
        {
            return Task.Run(() => InternalExists(profile));
        }

        private bool InternalExists(string profile)
        {
            string profileDirectory = GetProfileDirectory(profile);

            string currentProfileFilePath = Path.Combine(profileDirectory, profileFileName);
            string currentProfileSaveDataFilePath = Path.Combine(profileDirectory, saveFileName);

            return File.Exists(currentProfileFilePath) && File.Exists(currentProfileSaveDataFilePath);
        }
    }
}