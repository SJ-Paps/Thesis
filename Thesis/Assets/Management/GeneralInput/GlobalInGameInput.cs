﻿using Paps.Unity;
using SJ.Menu;
using UnityEngine;

namespace SJ.Management
{
    public class GlobalInGameInput : SJMonoBehaviour
    {
        [SerializeField]
        private MainMenu mainMenuPrefab;

        private MainMenu mainMenuInstance;

        protected override void SJAwake()
        {
            UnityUtil.DontDestroyOnLoad(gameObject);

            Application.GameManager.OnSessionFinished += () => { Destroy(mainMenuInstance); mainMenuInstance = null; };
        }

        protected override void SJUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && Application.GameManager.IsInGame())
            {
                if (mainMenuInstance == null)
                    InstantiateMenu();

                if (mainMenuInstance.gameObject.activeSelf)
                    mainMenuInstance.Hide();
                else
                    mainMenuInstance.Show();
            }
#if UNITY_EDITOR
            else if (Input.GetKeyDown(KeyCode.F5))
                Application.GameManager.Save();
            else if (Input.GetKeyDown(KeyCode.F6))
                Application.GameManager.Reload();
#endif
        }

        private void InstantiateMenu()
        {
            mainMenuInstance = Instantiate(mainMenuPrefab);
            mainMenuInstance.gameObject.SetActive(false);
        }
    }

}

