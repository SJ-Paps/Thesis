namespace SJ.Menu
{
    public class LanguageMenuViewControllerComponentWrapper : SJMonoBehaviour
    {
        private LanguageMenuViewController controller;

        protected override void SJAwake()
        {
            controller = new LanguageMenuViewController(GetComponent<ILanguageMenuView>(), Application.TranslatorService, Repositories.GetGameSettingsRepository());
        }
    }
}

