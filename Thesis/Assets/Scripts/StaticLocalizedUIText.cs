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
public class StaticLocalizedUIText : MonoBehaviour {

    //nombre del atributo "name" del XML con texto localizado
    [SerializeField]
    private string tagName;

    [SerializeField]
    private TextOptions option;

    private Text selfText;

    void Awake () {
        
        selfText = GetComponent<Text>();

        switch (option)
        {
            case TextOptions.None:

                selfText.text = LanguageManager.GetInstance().GetLineByTagAttribute(tagName);
                break;

            case TextOptions.ToUpper:
                selfText.text = LanguageManager.GetInstance().GetLineByTagAttribute(tagName).ToUpper();
                break;

            case TextOptions.ToLower:
                selfText.text = LanguageManager.GetInstance().GetLineByTagAttribute(tagName).ToLower();
                break;
        }
	}
}
