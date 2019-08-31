﻿using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : SJMonoBehaviour {

    private string language;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Text text;

    protected override void SJAwake()
    {
        button.onClick.AddListener(ChangeLanguage);
    }

    protected override void SJOnEnable()
    {
        LanguageManager.onLanguageChanged += OnLanguageChanged;
    }

    protected override void SJOnDisable()
    {
        LanguageManager.onLanguageChanged -= OnLanguageChanged;
    }

    public void SetLanguage(string language)
    {
        this.language = language;

        UpdateButtonText();
    }
	
	private void ChangeLanguage()
    {
        LanguageManager.ChangeLanguage(language);

        ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

        gameConfiguration.userLanguage = language;

        GameConfigurationCareTaker.SaveConfiguration();
    }

    private void UpdateButtonText()
    {
        text.text = LanguageManager.GetLocalizedTextLibrary().GetLineByTagOfCurrentLanguage("language_name_" + language).FirstLetterToUpper();
    }

    private void OnLanguageChanged(string language)
    {
        UpdateButtonText();
    }
}
