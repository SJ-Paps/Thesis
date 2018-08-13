using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : SJMonoBehaviour {

    [SerializeField]
    private LanguageButton languageButtonPrefab;

    [SerializeField]
    private VerticalLayoutGroup buttonMenu;

	// Use this for initialization
	void Awake ()
    {
        Language[] languages = LanguageManager.GetInstance().GetLanguages();

        foreach(Language language in languages)
        {
            LanguageButton button = Instantiate<LanguageButton>(languageButtonPrefab, buttonMenu.transform);

            button.SetLanguage(language);
        }
	}
}
