using UnityEngine;
using UnityEngine.UI;

namespace SJ.UI
{

    public class LanguageMenu : SJMonoBehaviour
    {

        [SerializeField]
        private LanguageButton languageButtonPrefab;

        [SerializeField]
        private VerticalLayoutGroup buttonMenu;

        // Use this for initialization
        protected override void SJAwake()
        {
            string[] languages = Application.GetTranslatorService().GetLanguages();

            foreach (string language in languages)
            {
                LanguageButton button = Instantiate<LanguageButton>(languageButtonPrefab, buttonMenu.transform);

                button.SetLanguage(language);
            }
        }
    }
}

