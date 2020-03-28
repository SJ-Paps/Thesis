using SJ.Management;
using UnityEngine;

namespace SJ.Menu
{
    public class LoadGameScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private MainMenu mainMenu;

        private LoadGameScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new LoadGameScreenViewController(GetComponent<ILoadGameScreenView>(), Repositories.GetProfileRepository(), 
                Repositories.GetGameSettingsRepository(), Management.Application.GameManager, mainMenu);
        }
    }
}