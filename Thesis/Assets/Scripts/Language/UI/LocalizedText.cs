using UnityEngine;

//Opciones para mostrar el texto de diferentes maneras
public enum TextOptions
{
    None,
    ToUpper,
    ToLower,
    FirstLetterToUpper
}

//Clase que modifica el texto del componente Text del objeto por un texto localizado
public abstract class LocalizedText : SJMonoBehaviour {

    //nombre del atributo "tag" del XML con texto localizado
    [SerializeField]
    protected string langTag;

    [SerializeField]
    protected TextOptions option;

    public abstract string Text
    {
        get;
        protected set;
    }

    protected LanguageManager languageManager;

    void Awake () {

        languageManager = LanguageManager.GetInstance();

        languageManager.onLanguageChanged += OnLanguageChanged;
        
	}

    protected virtual void OnLanguageChanged(LanguageInfo info)
    {
        UpdateText();
    }

    protected virtual void UpdateText()
    {
        switch (option)
        {
            case TextOptions.None:
                
                Text = languageManager.GetLineByTagAttribute(langTag);

                break;

            case TextOptions.ToLower:

                Text = languageManager.GetLineByTagAttribute(langTag).ToLower();

                break;

            case TextOptions.ToUpper:

                Text = languageManager.GetLineByTagAttribute(langTag).ToUpper();

                break;

            case TextOptions.FirstLetterToUpper:

                Text = languageManager.GetLineByTagAttribute(langTag).FirstLetterToUpper();

                break;
        }
    }


}
