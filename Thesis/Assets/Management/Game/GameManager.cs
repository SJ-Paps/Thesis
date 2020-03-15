using SJ.Coroutines;
using SJ.Profiles;
using SJ.Save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SJ.Game
{
    public class GameManager
    {
        public class GameSessionSaveData
        {
            public int[] loadedScenesIndexes;
            public bool isBeginning;
            public object[] gameplaySaves;
        }

        private string ignoreScenesOnSavingSubfix = "_igld";
        private string baseSceneName = "Base";

        private string[] beginScenes;
        private string returnSceneOnEndSession;

        public event Action onSavingBegan;
        public event Action onSavingFailed;
        public event Action onSavingSucceeded;

        public event Action onLoadingBegan;
        public event Action onLoadingFailed;
        public event Action onLoadingSucceeded;

        public event Action onQuitting;

        private HashSet<SJMonoBehaviourSaveable> saveables;

        public bool IsInGame { get; private set; }

        public ProfileData CurrentProfile { get; private set; }

        private ICoroutineScheduler coroutineScheduler;
        private IProfileRepository profileRepository;

        public GameManager()
        {
            coroutineScheduler = SJ.Application.CoroutineScheduler;
            profileRepository = Repositories.GetProfileRepository();

            beginScenes = Application.ApplicationSettings.BeginningScenes;
            returnSceneOnEndSession = Application.ApplicationSettings.ReturnSceneOnEndSession;

            saveables = new HashSet<SJMonoBehaviourSaveable>();

            onLoadingSucceeded += SetIsInGameAfterLoading;
            onQuitting += SetIsNotInGameOnQuitting;
        }

        public void SubscribeForSave(SJMonoBehaviourSaveable saveable)
        {
            saveables.Add(saveable);
        }

        public void DesubscribeForSave(SJMonoBehaviourSaveable saveable)
        {
            saveables.Remove(saveable);
        }

        public void BeginSessionWithProfile(ProfileData profileData)
        {
            profileRepository.Exists(profileData.name)
                .Subscribe(exists =>
                {
                    if (exists == false)
                    {
                        CreateDefaultProfile(profileData)
                            .Subscribe(_ =>
                            {
                                CurrentProfile = profileData;
                                LoadGame();
                            });
                    }
                    else
                    {
                        CurrentProfile = profileData;
                        LoadGame();
                    }
                    
                });
        }

        private IObservable<Unit> CreateDefaultProfile(ProfileData profileData)
        {
            return profileRepository.CreateProfile(profileData.name, profileData, 
                new SaveData(profileData.name, new GameSessionSaveData() 
                { 
                    gameplaySaves = null, 
                    isBeginning = true,
                    loadedScenesIndexes = GetScenesIndex(beginScenes)
                })
                );
        }

        private int[] GetScenesIndex(string[] scenes)
        {
            int[] indexArray = new int[scenes.Length];

            for (int i = 0, j = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneByBuildIndex(i).name == scenes[j])
                {
                    indexArray[j] = i;
                    j++;
                }
            }

            return indexArray;
        }

        public void EndSession()
        {
            CallOnQuittingEvent();

            CurrentProfile = default;

            SceneManager.LoadScene(returnSceneOnEndSession);
        }

        public void SaveGame()
        {
            CallOnSavingBeganEvent();

            coroutineScheduler.StartCoroutine(GetAllSavesAndAWaitSerializationCoroutine());
        }

        private IEnumerator GetAllSavesAndAWaitSerializationCoroutine()
        {
            List<SJMonoBehaviourSaveable> currentSaveables = new List<SJMonoBehaviourSaveable>();
            List<object> saves = new List<object>();

            currentSaveables.AddRange(saveables);

            for (int i = 0; i < currentSaveables.Count; i++)
            {
                saves.Add(currentSaveables[i].Save());
                yield return null;
            }

            for (int i = 0; i < currentSaveables.Count; i++)
            {
                if (saveables.Contains(currentSaveables[i]) == false)
                {
                    currentSaveables.RemoveAt(i);
                    saves.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < currentSaveables.Count; i++)
            {
                currentSaveables[i].PostSaveCallback();
            }

            GameSessionSaveData sessionData = new GameSessionSaveData()
            {
                loadedScenesIndexes = GetSaveableScenes(),
                gameplaySaves = saves.ToArray(),
                isBeginning = false
            };

            profileRepository.UpdateProfileData(CurrentProfile.name, CurrentProfile)
                .Select(_ => profileRepository.SaveOnProfile(CurrentProfile.name, new SaveData(CurrentProfile.name, sessionData)))
                .Subscribe(_ => CallOnSavingSucceededEvent(), error => CallOnSavingFailedEvent());
        }

        public void LoadGame()
        {
            CallOnLoadingBeganEvent();

            LoadFromSaveGame();
        }

        private void LoadFromSaveGame()
        {
            profileRepository.GetSaveDataFrom(CurrentProfile.name)
                .Subscribe(saveData =>
                {
                    var sessionData = (GameSessionSaveData)saveData.saveObject;

                    if (sessionData.isBeginning)
                        NewGame(beginScenes);
                    else
                        Observable.FromCoroutine(() => PrepareScene(sessionData))
                            .Subscribe(_ => CallOnLoadingSucceededEvent());
                },
                error => CallOnLoadingFailedEvent());
        }

        private IEnumerator PrepareScene(GameSessionSaveData sessionData)
        {
            yield return coroutineScheduler.AwaitCoroutine(LoadGameplayScenes(sessionData.loadedScenesIndexes));

            object[] saves = sessionData.gameplaySaves;

            List<SJMonoBehaviourSaveable> currentSaveables = new List<SJMonoBehaviourSaveable>();
            List<GameplayObjectSave> gameplayObjectSaves = new List<GameplayObjectSave>();

            for (int i = 0; i < saves.Length; i++)
            {
                GameplayObjectSave gameplayObjectSave = saves[i] as GameplayObjectSave;

                if (gameplayObjectSave == null)
                {
                    continue;
                }

                GameObject gameplayGameObject = SJResources.LoadAsset<GameObject>(gameplayObjectSave.prefabName);

                gameplayGameObject = GameObject.Instantiate(gameplayGameObject);

                SJMonoBehaviourSaveable monoBehaviourSaveable = gameplayGameObject.GetComponent<SJMonoBehaviourSaveable>();

                Type type = typeof(SJMonoBehaviour);

                PropertyInfo propertyInfo = type.GetProperty(nameof(monoBehaviourSaveable.InstanceGUID), BindingFlags.Instance | BindingFlags.Public);

                propertyInfo.SetValue(monoBehaviourSaveable, gameplayObjectSave.instanceGUID);

                monoBehaviourSaveable.Load(gameplayObjectSave.save);

                currentSaveables.Add(monoBehaviourSaveable);
                gameplayObjectSaves.Add(gameplayObjectSave);

                yield return null;
            }

            for (int i = 0; i < currentSaveables.Count; i++)
            {
                currentSaveables[i].PostLoadCallback(gameplayObjectSaves[i].save);

                yield return null;
            }
        }


        public void NewGame(string[] sceneNames)
        {
            coroutineScheduler.StartCoroutine(LoadGameplayScenes(sceneNames, CallOnLoadingSucceededEvent));
        }

        private IEnumerator LoadGameplayScenes(string[] sceneNames, Action onCompletion = null)
        {
            SceneManager.LoadScene(baseSceneName, LoadSceneMode.Single);

            yield return null;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseSceneName));

            yield return null;

            for (int i = 0; i < sceneNames.Length; i++)
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);

                while (loadOperation.isDone == false)
                {
                    yield return null;
                }
            }

            if (onCompletion != null)
            {
                onCompletion();
            }
        }

        private IEnumerator LoadGameplayScenes(int[] sceneIndexes, Action onCompletion = null)
        {
            SceneManager.LoadScene(baseSceneName, LoadSceneMode.Single);

            yield return null;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseSceneName));

            yield return null;

            for (int i = 0; i < sceneIndexes.Length; i++)
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndexes[i], LoadSceneMode.Additive);

                while (loadOperation.isDone == false)
                {
                    yield return null;
                }
            }

            if (onCompletion != null)
            {
                onCompletion();
            }
        }

        private void SetIsInGameAfterLoading()
        {
            IsInGame = true;
        }

        private void SetIsNotInGameOnQuitting()
        {
            IsInGame = false;
        }

        private int[] GetSaveableScenes()
        {
            List<int> saveableScenes = new List<int>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                if (current.name.HasSubfix(ignoreScenesOnSavingSubfix) == false && current.name != baseSceneName)
                {
                    saveableScenes.Add(current.buildIndex);
                }
            }

            return saveableScenes.ToArray();
        }

        private void CallOnQuittingEvent()
        {
            if (onQuitting != null)
            {
                onQuitting();
            }
        }

        private void CallOnSavingSucceededEvent()
        {
            if (onSavingSucceeded != null)
            {
                onSavingSucceeded();
            }
        }

        private void CallOnSavingFailedEvent()
        {
            if (onSavingFailed != null)
            {
                onSavingFailed();
            }
        }

        private void CallOnSavingBeganEvent()
        {
            if (onSavingBegan != null)
            {
                onSavingBegan();
            }
        }

        private void CallOnLoadingBeganEvent()
        {
            if (onLoadingBegan != null)
            {
                onLoadingBegan();
            }
        }

        private void CallOnLoadingFailedEvent()
        {
            if (onLoadingFailed != null)
            {
                onLoadingFailed();
            }
        }

        private void CallOnLoadingSucceededEvent()
        {
            if (onLoadingSucceeded != null)
            {
                onLoadingSucceeded();
            }
        }

    }

}


