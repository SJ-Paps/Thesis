using Paps.Maybe;
using SJ.GameEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
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

        private ApplicationSettings applicationSettings;

        public event Action OnSaving;
        public event Action OnSaveFailed;
        public event Action OnSaveSucceeded;
        public event Action OnLoading;
        public event Action OnLoadingFailed;
        public event Action OnLoadingSucceeded;
        public event Action OnSessionBegan;
        public event Action OnSessionFinished;

        private HashSet<ISaveableGameEntity> saveables;

        public string CurrentProfile => currentProfileData.name;
        private ProfileData currentProfileData;

        private IProfileRepository profileRepository;

        public GameManager(IProfileRepository profileRepository, ApplicationSettings applicationSettings)
        {
            this.profileRepository = profileRepository;
            this.applicationSettings = applicationSettings;

            saveables = new HashSet<ISaveableGameEntity>();
        }

        public void BeginSessionFor(string profile)
        {
            Debug.Log("Wants to begin session with profile " + profile);

            profileRepository.GetProfileDataFrom(profile)
                .ObserveOnMainThread()
                .Subscribe(maybeProfile =>
                {
                    if (maybeProfile.IsNothing())
                        CreateDefaultProfile(profile)
                            .Do(profileData => currentProfileData = profileData)
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
                loadedScenes = applicationSettings.BeginningScenes
            });
        }

        public void EndSession()
        {
            currentProfileData = default;
            saveables.Clear();

            OnSessionFinished?.Invoke();

            SceneManager.LoadScene(applicationSettings.ReturnSceneOnEndSession);
        }

        public void Save()
        {
            ISaveableGameEntity[] currentSaveables = saveables.ToArray();
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
                .ContinueWith(profileRepository.SaveOnProfile(CurrentProfile, new SaveData(CurrentProfile, sessionData)))
                .ObserveOnMainThread()
                .Subscribe(_ => OnSaveSucceeded?.Invoke(), error => { OnSaveFailed?.Invoke(); Debug.LogError(error.Message); });
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
                            .ContinueWith(LoadEntities(sessionData))
                            .Do(_ => OnLoadingSucceeded?.Invoke())
                            .Do(_ => OnSessionBegan?.Invoke())
                            .Subscribe();
                },
                error => OnLoadingFailed?.Invoke());
        }

        private IObservable<Unit> LoadEntities(GameSessionSaveData sessionData)
        {
            GameplayObjectSave[] saves = sessionData.gameplaySaves;
            IObservable<GameObject>[] loadEntityPrefabObservables = 
                Array.ConvertAll(saves, save => SJResources.LoadAssetAsync<GameObject>(save.PrefabName));

            return Observable.Zip(loadEntityPrefabObservables)
                .ObserveOnMainThread()
                .Do(loadedSaveables =>
                {
                    var saveablesArray = Array.ConvertAll(loadedSaveables.ToArray(), 
                        saveable => GameObject.Instantiate<GameObject>(saveable).GetComponent<ISaveableGameEntity>());

                    int index = 0;
                    foreach (var saveable in saveablesArray)
                    {
                        saveable.Load(saves[index]);
                        index++;
                    }

                    index = 0;
                    foreach (var saveable in saveablesArray)
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

            LoadGameplayScenes(applicationSettings.BeginningScenes)
                .Do(_ => OnLoadingSucceeded?.Invoke())
                .Do(_ => OnSessionBegan?.Invoke())
                .Subscribe();
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
            SceneManager.LoadScene(applicationSettings.BaseScene, LoadSceneMode.Single);
            yield return null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(applicationSettings.BaseScene));
            yield return null;
        }

        private string[] GetSaveableScenes()
        {
            List<string> saveableScenes = new List<string>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                if (IsValidSaveableScene(current.name))
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

        public void SubscribeSaveable(ISaveableGameEntity saveable)
        {
            saveables.Add(saveable);
        }

        public void UnsubscribeSaveable(ISaveableGameEntity saveable)
        {
            saveables.Remove(saveable);
        }

        private bool IsValidSaveableScene(string sceneName)
        {
            return applicationSettings.IgnoreScenesOnSave.Contains(sceneName) == false && sceneName != applicationSettings.BaseScene;
        }
    }
}