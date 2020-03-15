using UnityEngine;

namespace SJ.Localization
{
    //Opciones para mostrar el texto de diferentes maneras


    //Clase que modifica el texto del componente Text del objeto por un texto localizado
    public abstract class LocalizedTextComponent : SJMonoBehaviour
    {

        //nombre del atributo "tag" del XML con texto localizado
        [SerializeField]
        protected string langTag;

        [SerializeField]
        protected TextOptions option;

        protected LocalizedText localizedText;

        public string Text
        {
            get
            {
                return localizedText.ToString();
            }
        }

        protected override void SJAwake()
        {
            localizedText = new LocalizedText(langTag, option);

            localizedText.onTextChanged += OnTextChanged;

            localizedText.UpdateText();
        }

        protected override void SJOnDestroy()
        {
            localizedText.Dispose();
        }

        protected abstract void OnTextChanged(string text);
    }
}


