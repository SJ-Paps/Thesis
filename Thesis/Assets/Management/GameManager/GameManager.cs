using Paps.Maybe;
using SJ.Profiles;
using SJ.Save;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UnityEngine.SceneManagement;

namespace SJ.Management
{
    public class GameManager : IGameManager
    {
        public class GameSessionSaveData
        {
            public string[] loadedScenes;
            public bool isBeginning;
            public GameplayObjectSave[] gameplaySaves;
        }

        private string ignoreScenesOnSavingSubfix = "_igld";
        private string baseSceneName = "Base";

        private string[] beginScenes;
        private string returnSceneOnEndSession;

        public event Action OnSaving;
        public event Action OnSaveFailed;
        public event Action OnSaveSucceeded;
        public event Action OnLoading;
        public event Action OnLoadingFailed;
        public event Action OnLoadingSucceeded;
        public event Action OnSessionBegan;
        public event Action OnSessionFinished;

        private HashSet<ISaveable> saveables;

        public string CurrentProfile => currentProfileData.name;
        private ProfileData currentProfileData;

        private IProfileRepository profileRepository;

        public GameManager(IProfileRepository profileRepository, ApplicationSettings applicationSettings)
        {
            this.profileRepository = profileRepository;

            beginScenes = applicationSettings.BeginningScenes;
            returnSceneOnEndSession = applicationSettings.ReturnSceneOnEndSession;

            saveables = new HashSet<ISaveable>();
        }

        public void BeginSessionFor(string profile)
        {
            Debug.Log("Wants to begin session with profile " + profile);

            profileRepository.GetProfileDataFrom(profile)
                .Subscribe(maybeProfile =>
                {
                    if (maybeProfile.IsNothing())
                        CreateDefaultProfile(profile)
                            .Do(profileData => currentProfileData = profileData)
                            .Do(_ => OnSessionBegan?.Invoke())
                            .Do(_ => NewGame())
                            .Subscribe();
                    else
                    {
                        currentProfileData = maybeProfile.Value;
                        LoadGame();
                    }
                },
                error => Debug.LogError(error.Message));
        }

        private IObservable<ProfileData> CreateDefaultProfile(string profile)
        {
            var newProfile = new ProfileData() { name = profile };

            return profileRepository.CreateProfile(profile, newProfile,
                DefaultSaveDataFor(profile)
            ).Select(_ => newProfile);
        }

        private SaveData DefaultSaveDataFor(string profile)
        {
            return new SaveData(profile, new GameSessionSaveData()
            {
                gameplaySaves = null,
                isBeginning = true,
                loadedScenes = beginScenes
            });
        }

        public void EndSession()
        {
            currentProfileData = default;
            saveables.Clear();

            OnSessionFinished?.Invoke();

            SceneManager.LoadScene(returnSceneOnEndSession);
        }

        public void Save()
        {
            ISaveable[] currentSaveables = saveables.ToArray();
            GameplayObjectSave[] saves = Array.ConvertAll(currentSaveables, saveable => saveable.Save());

            for (int i = 0; i < currentSaveables.Length; i++)
                currentSaveables[i].PostSaveCallback();

            GameSessionSaveData sessionData = new GameSessionSaveData()
            {
                loadedScenes = GetSaveableScenes(),
                gameplaySaves = saves,
                isBeginning = false
            };

            OnSaving?.Invoke();

            profileRepository.UpdateProfileData(CurrentProfile, currentProfileData)
                .Select(_ => profileRepository.SaveOnProfile(CurrentProfile, new SaveData(CurrentProfile, sessionData)))
                .ObserveOnMainThread()
                .Subscribe(_ => OnSaveSucceeded?.Invoke(), error => OnSaveFailed?.Invoke());
        }

        private void LoadGame()
        {
            OnLoading?.Invoke();

            LoadFromSaveGame();
        }

        private void LoadFromSaveGame()
        {
            profileRepository.GetSaveDataFrom(CurrentProfile)
                .Subscribe(saveData =>
                {
                    var sessionData = (GameSessionSaveData)saveData.Value.saveObject;

                    if (sessionData.isBeginning)
                        NewGame();
                    else
                        LoadGameplayScenes(sessionData.loadedScenes)
                            .Select(_ => LoadEntities(sessionData))
                            .Subscribe(_ => OnLoadingSucceeded?.Invoke());
                },
                error => OnLoadingFailed?.Invoke());
        }

        private IObservable<Unit> LoadEntities(GameSessionSaveData sessionData)
        {
            GameplayObjectSave[] saves = sessionData.gameplaySaves;
            IObservable<ISaveable>[] loadEntityPrefabObservables = 
                Array.ConvertAll(saves, save => SJResources.LoadComponentOfGameObjectAsync<ISaveable>(save.prefabName));

            return Observable.Zip(loadEntityPrefabObservables)
                .ObserveOnMainThread()
                .Do(loadedSaveables =>
                {
                    int index = 0;
                    foreach (var saveable in loadedSaveables)
                    {
                        saveable.Load(saves[index]);
                        index++;
                    }

                    index = 0;
                    foreach (var saveable in loadedSaveables)
                    {
                        saveable.PostLoadCallback(saves[index]);
                        index++;
                    }
                }
                ).Select(_ => Unit.Default);
        }

        private void NewGame()
        {
            OnLoading?.Invoke();

            LoadGameplayScenes(beginScenes)
                .Subscribe(_ => OnLoadingSucceeded?.Invoke());
        }

        private IObservable<Unit> LoadGameplayScenes(string[] sceneNames)
        {
            return Observable.FromCoroutine(() => LoadBaseScene())
                .Select(_ =>
                {
                    var loadSceneObservables = Array.ConvertAll(sceneNames,
                        scene => SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).AsAsyncOperationObservable());

                    return Observable.Zip(loadSceneObservables);
                })
                .Select(_ => Unit.Default);
        }

        private IEnumerator LoadBaseScene()
        {
            SceneManager.LoadScene(baseSceneName, LoadSceneMode.Single);
            yield return null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseSceneName));
            yield return null;
        }

        private string[] GetSaveableScenes()
        {
            List<string> saveableScenes = new List<string>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                if (current.name.HasSubfix(ignoreScenesOnSavingSubfix) == false && current.name != baseSceneName)
                    saveableScenes.Add(current.name);
            }

            return saveableScenes.ToArray();
        }

        public bool IsInGame()
        {
            return string.IsNullOrEmpty(CurrentProfile) == false;
        }

        public void Reload()
        {
            LoadGame();
        }

        public void SubscribeSaveable(ISaveable saveable)
        {
            saveables.Add(saveable);
        }

        public void UnsubscribeSaveable(ISaveable saveable)
        {
            saveables.Remove(saveable);
        }
    }
}