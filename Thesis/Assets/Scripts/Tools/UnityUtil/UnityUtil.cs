using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SJ.Tools
{
    public static class UnityUtil
    {
        private static List<Object> dontDestroyOnLoadObjects = new List<Object>();

        public static void DontDestroyOnLoad(Object obj)
        {
            if(dontDestroyOnLoadObjects.Contains(obj) == false)
            {
                dontDestroyOnLoadObjects.Add(obj);
                Object.DontDestroyOnLoad(obj);
            }
        }

        public static T FindDontDestroyOnLoadObjectOfType<T>() where T : class
        {
            for (int i = 0; i < dontDestroyOnLoadObjects.Count; i++)
            {
                if (dontDestroyOnLoadObjects[i] is T obj)
                {
                    return obj;
                }
            }

            return null;
        }

        public static void DestroyAllDontDestroyOnLoadObjects()
        {
            for (int i = 0; i < dontDestroyOnLoadObjects.Count; i++)
            {
                Object.Destroy(dontDestroyOnLoadObjects[i]);
            }

            dontDestroyOnLoadObjects.Clear();
        }

        public static void DestroyDontDestroyOnLoadObject(Object obj)
        {
            if(dontDestroyOnLoadObjects.Remove(obj))
            {
                Object.Destroy(obj);
            }
        }

        public static T FindObjectOfTypeInSceneIncludingInactive<T>(Scene scene) where T : class
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();

            for (int j = 0; j < rootObjects.Length; j++)
            {
                T obj = rootObjects[j].GetComponentInChildren<T>(true);

                if (obj != null)
                {
                    return obj;
                }
            }

            return default;
        }

        public static T FindObjectOfType<T>() where T : class
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                GameObject[] rootObjects = current.GetRootGameObjects();

                for (int j = 0; j < rootObjects.Length; j++)
                {
                    T obj = rootObjects[j].GetComponentInChildren<T>(false);

                    if (obj != null)
                    {
                        return obj;
                    }
                }
            }

            return FindDontDestroyOnLoadObjectOfType<T>();
        }

        public static T[] FindObjectsOfType<T>() where T : class
        {
            List<T> objectList = null;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                GameObject[] rootObjects = current.GetRootGameObjects();

                for (int j = 0; j < rootObjects.Length; j++)
                {
                    T[] objects = rootObjects[j].GetComponentsInChildren<T>(false);

                    if (objects != null)
                    {
                        if (objectList == null)
                        {
                            objectList = new List<T>();
                        }

                        objectList.AddRange(objects);
                    }
                }
            }

            if (objectList != null)
            {
                return objectList.ToArray();
            }
            else
            {
                return null;
            }
        }

        public static T FindObjectOfTypeIncludingInactive<T>() where T : class
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                GameObject[] rootObjects = current.GetRootGameObjects();

                for (int j = 0; j < rootObjects.Length; j++)
                {
                    T obj = rootObjects[j].GetComponentInChildren<T>(true);

                    if (obj != null)
                    {
                        return obj;
                    }
                }
            }

            return FindDontDestroyOnLoadObjectOfType<T>();
        }

        public static T[] FindObjectsOfTypeIncludingInactive<T>() where T : class
        {
            List<T> objectList = null;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                GameObject[] rootObjects = current.GetRootGameObjects();

                for (int j = 0; j < rootObjects.Length; j++)
                {
                    T[] objects = rootObjects[j].GetComponentsInChildren<T>(true);

                    if (objects != null)
                    {
                        if (objectList == null)
                        {
                            objectList = new List<T>();
                        }

                        objectList.AddRange(objects);
                    }
                }
            }

            if (objectList != null)
            {
                return objectList.ToArray();
            }
            else
            {
                return null;
            }
        }

        public static GameObject FindGameObjectWithTagIncludingInactive(string tag)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                GameObject[] rootObjects = current.GetRootGameObjects();

                for (int j = 0; j < rootObjects.Length; j++)
                {
                    foreach (Transform child in rootObjects[j].transform)
                    {
                        GameObject currentGameObject = child.gameObject;

                        if (currentGameObject.tag == tag)
                        {
                            return currentGameObject;
                        }
                    }
                }
            }

            return null;
        }

        public static GameObject[] FindGameObjectsWithTagIncludingInactive(string tag)
        {
            List<GameObject> objectList = null;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene current = SceneManager.GetSceneAt(i);

                GameObject[] rootObjects = current.GetRootGameObjects();

                for (int j = 0; j < rootObjects.Length; j++)
                {
                    foreach (Transform child in rootObjects[j].transform)
                    {
                        GameObject currentGameObject = child.gameObject;

                        if (currentGameObject.tag == tag)
                        {
                            if (objectList == null)
                            {
                                objectList = new List<GameObject>();
                            }

                            objectList.Add(currentGameObject);
                        }
                    }
                }
            }

            if (objectList != null)
            {
                return objectList.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}
