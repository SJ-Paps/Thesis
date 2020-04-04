using SJ.Management;
using UnityEngine;

namespace SJ.Menu
{
    public class NewGameScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private MainMenu mainMenu;

        private NewGameScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new NewGameScreenViewController(GetComponent<INewGameScreenView>(), Repositories.GetProfileRepository(),
                Repositories.GetGameSettingsRepository(), Management.Application.Instance.TranslatorService(), Management.Application.Instance.GameManager(), mainMenu);
        }
    }
}
