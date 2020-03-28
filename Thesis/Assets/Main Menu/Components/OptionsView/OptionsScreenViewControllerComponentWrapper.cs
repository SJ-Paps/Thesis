using UnityEngine;

namespace SJ.Menu
{
    public class OptionsScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private MainMenu mainMenu;

        private OptionsScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new OptionsScreenViewController(GetComponent<IOptionsScreenView>(), mainMenu);
        }
    }
}