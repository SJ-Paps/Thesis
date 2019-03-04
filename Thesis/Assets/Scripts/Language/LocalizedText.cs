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

    private LanguageManager languageManager;
    private LocalizedTextLibrary textLibrary;

    [SerializeField]
    private TextOptions option;

    public event Action<string> onTextChanged;

    public LocalizedText(string tag, TextOptions option)
    {
        this.tag = tag;
        this.option = option;

        languageManager = LanguageManager.GetInstance();
        textLibrary = LocalizedTextLibrary.GetInstance();

        languageManager.onLanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(Language language)
    {
        UpdateText();
    }

    public void UpdateText()
    {
        switch (option)
        {
            case TextOptions.None:

                Text = textLibrary.GetLineByTagAttribute(tag);

                break;

            case TextOptions.ToLower:

                Text = textLibrary.GetLineByTagAttribute(tag).ToLower();

                break;

            case TextOptions.ToUpper:

                Text = textLibrary.GetLineByTagAttribute(tag).ToUpper();

                break;

            case TextOptions.FirstLetterToUpper:

                Text = textLibrary.GetLineByTagAttribute(tag).FirstLetterToUpper();

                break;
        }

        onTextChanged(Text);
    }

    public override string ToString()
    {
        return Text;
    }
}
