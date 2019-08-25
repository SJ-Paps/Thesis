using System;
using UnityEngine;

public enum TextOptions : byte
{
    None,
    ToUpper,
    ToLower,
    FirstLetterToUpper
}

[Serializable]
public class LocalizedText
{
    
    public string tag;

    private string text;

    public string Text
    {
        get
        {
            return text;
        }

        private set
        {
            text = value;
        }
    }
    
    private ILocalizedTextLibrary textLibrary;

    [SerializeField]
    private TextOptions option;

    public event Action<string> onTextChanged;

    public LocalizedText(string tag, TextOptions option)
    {
        this.tag = tag;
        this.option = option;

        textLibrary = LanguageManager.GetLocalizedTextLibrary();

        LanguageManager.onLanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(string language)
    {
        UpdateText();
    }

    public void UpdateText()
    {
        switch (option)
        {
            case TextOptions.None:

                Text = textLibrary.GetLineByTagOfCurrentLanguage(tag);

                break;

            case TextOptions.ToLower:

                Text = textLibrary.GetLineByTagOfCurrentLanguage(tag).ToLower();

                break;

            case TextOptions.ToUpper:

                Text = textLibrary.GetLineByTagOfCurrentLanguage(tag).ToUpper();

                break;

            case TextOptions.FirstLetterToUpper:

                Text = textLibrary.GetLineByTagOfCurrentLanguage(tag).FirstLetterToUpper();

                break;
        }

        onTextChanged(Text);
    }

    public override string ToString()
    {
        return Text;
    }
}
