using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : SJMonoBehaviour {

    private Language language;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Text text;

    void Awake()
    {
        button.onClick.AddListener(ChangeLanguage);
    }

    public void SetLanguage(Language language)
    {
        this.language = language;

        text.text = language.name;
    }
	
	private void ChangeLanguage()
    {
        LanguageManager.GetInstance().ChangeLanguage(language.name);
    }
}
