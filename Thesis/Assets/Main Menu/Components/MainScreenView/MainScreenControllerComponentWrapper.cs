﻿using SJ.Management;
using UnityEngine;
using Application = SJ.Management.Application;

namespace SJ.Menu
{
    public class MainScreenControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private MainMenu mainMenu;

        private MainScreenViewController mainScreenController;

        protected override void SJAwake()
        {
            base.SJAwake();

            mainScreenController = new MainScreenViewController(GetComponent<IMainScreenView>(), Application.Instance.GameManager(), 
                Repositories.GetGameSettingsRepository(), mainMenu, Application.Instance.TranslatorService());
        }
    }
}