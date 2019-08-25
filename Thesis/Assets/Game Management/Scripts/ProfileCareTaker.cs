using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System;

public static class ProfileCareTaker
{
    private static string profileDirectory = Path.Combine(Application.dataPath, "../saves/profiles");
    private static string profileFileName = "profile.sj";
    private static string saveFileName = "save.sj";

    public static Task SaveProfileAsync(string profileName, ProfileData profileData, SaveData saveData)
    {
        return Task.Run(
            delegate()
            {
                SaveProfile(profileName, profileData, saveData);
            }
            );
    }

    private static void SaveProfile(string profileName, ProfileData profileData, SaveData saveData)
    {
        string currentProfileDirectory = GetProfileDirectory(profileName);

        if (Directory.Exists(currentProfileDirectory) == false)
        {
            Directory.CreateDirectory(currentProfileDirectory);
        }

        SaveData profileSaveData = new SaveData(profileName, profileData);

        string profileFilePath = Path.Combine(currentProfileDirectory, profileFileName);
        string saveFilePath = Path.Combine(currentProfileDirectory, saveFileName);

        SaveLoadTool.Serialize(profileFilePath, profileSaveData);
        SaveLoadTool.Serialize(saveFilePath, saveData);
    }

    public static Task<ProfileData[]> GetAllProfilesAsync()
    {
        return Task.Run<ProfileData[]>(GetAllProfiles);
    }

    private static ProfileData[] GetAllProfiles()
    {
        string[] profileFilePaths = null;

        try
        {
            profileFilePaths = Directory.GetFiles(profileDirectory, profileFileName, SearchOption.AllDirectories);
        }
        catch { }
        

        List<ProfileData> profiles = null;

        if(profileFilePaths != null)
        {
            for (int i = 0; i < profileFilePaths.Length; i++)
            {
                string profileName = Path.GetFileName(Path.GetDirectoryName(profileFilePaths[i]));

                if(ProfileExistsAndIsValid(profileName))
                {
                    SaveData[] saves = SaveLoadTool.Deserialize(profileFilePaths[i]);

                    if (saves != null && saves.Length > 0)
                    {
                        if (profiles == null)
                        {
                            profiles = new List<ProfileData>();
                        }

                        profiles.Add((ProfileData)saves[0].saveObject);
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

    private static string[] GetAllProfileDirectories()
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

    public static bool ProfileExistsAndIsValid(string profileName)
    {
        string profileDirectory = GetProfileDirectory(profileName);

        string currentProfileFilePath = Path.Combine(profileDirectory, profileFileName);
        string currentProfileSaveDataFilePath = Path.Combine(profileDirectory, saveFileName);

        return File.Exists(currentProfileFilePath) && File.Exists(currentProfileSaveDataFilePath);
    }

    public static Task<SaveData> GetSaveDataFromProfileAsync(string profileName)
    {
        return Task.Run<SaveData>(
            delegate()
            {
                return GetSaveDataFromProfile(profileName);
            }
            );
    }

    private static SaveData GetSaveDataFromProfile(string profileName)
    {
        if(ProfileExistsAndIsValid(profileName))
        {
            string saveDataFilePath = Path.Combine(GetProfileDirectory(profileName), saveFileName);

            if (File.Exists(saveDataFilePath))
            {
                SaveData[] saves = SaveLoadTool.Deserialize(saveDataFilePath);

                if (saves != null && saves.Length > 0)
                {
                    SaveData profileSaveData = saves[0];

                    return profileSaveData;
                }
            }
        }

        throw new InvalidOperationException("provided profile name does not exists or is invalid");
    }

    private static string GetProfileDirectory(string profileName)
    {
        return Path.Combine(profileDirectory, profileName);
    }

    public static void DeleteProfile(string profileName)
    {
        if(ProfileExistsAndIsValid(profileName))
        {
            Directory.Delete(GetProfileDirectory(profileName), true);
        }
    }

    public static void DeleteAllProfiles()
    {
        string[] allProfileDirectories = GetAllProfileDirectories();

        for(int i = 0; i < allProfileDirectories.Length; i++)
        {
            Directory.Delete(allProfileDirectories[i], true);
        }
    }
}
