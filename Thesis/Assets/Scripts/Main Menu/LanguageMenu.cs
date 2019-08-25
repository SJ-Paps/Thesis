using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : SJMonoBehaviour {

    [SerializeField]
    private LanguageButton languageButtonPrefab;

    [SerializeField]
    private VerticalLayoutGroup buttonMenu;

	// Use this for initialization
	protected override void Awake ()
    {
        base.Awake();

        string[] languages = LanguageManager.GetLanguages();

        foreach(string language in languages)
        {
            LanguageButton button = Instantiate<LanguageButton>(languageButtonPrefab, buttonMenu.transform);

            button.SetLanguage(language);
        }
	}
}
