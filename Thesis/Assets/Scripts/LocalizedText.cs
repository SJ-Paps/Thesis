using UnityEngine;
using UnityEngine.UI;

//Opciones para mostrar el texto de diferentes maneras
public enum TextOptions
{
    None,
    ToUpper,
    ToLower
}

//Clase que modifica el texto del componente Text del objeto por un texto localizado
public abstract class LocalizedText : SJMonoBehaviour {

    //nombre del atributo "tag" del XML con texto localizado
    [SerializeField]
    protected string tag;

    [SerializeField]
    protected TextOptions option;

    protected LanguageManager languageManager;

    void Awake () {

        languageManager = LanguageManager.GetInstance();

        languageManager.onLanguageChanged += OnLanguageChanged;
        
	}

    protected virtual void OnLanguageChanged(LanguageInfo info)
    {
        UpdateText();
    }

    protected abstract void UpdateText();
}
