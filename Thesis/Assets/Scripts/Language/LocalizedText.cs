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

    protected LocalizedTextLibrary localizedTextLibrary;
    protected LanguageManager languageManager;

    void Awake () {

        localizedTextLibrary = LocalizedTextLibrary.GetInstance();
        languageManager = LanguageManager.GetInstance();
        languageManager.onLanguageChanged += OnLanguageChanged;
        
	}

    protected virtual void OnLanguageChanged(Language info)
    {
        UpdateText();
    }

    protected virtual void UpdateText()
    {
        switch (option)
        {
            case TextOptions.None:
                
                Text = localizedTextLibrary.GetLineByTagAttribute(langTag);

                break;

            case TextOptions.ToLower:

                Text = localizedTextLibrary.GetLineByTagAttribute(langTag).ToLower();

                break;

            case TextOptions.ToUpper:

                Text = localizedTextLibrary.GetLineByTagAttribute(langTag).ToUpper();

                break;

            case TextOptions.FirstLetterToUpper:

                Text = localizedTextLibrary.GetLineByTagAttribute(langTag).FirstLetterToUpper();

                break;
        }
    }


}
