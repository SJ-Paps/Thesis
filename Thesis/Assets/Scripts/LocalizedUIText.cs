using UnityEngine.UI;

public class LocalizedUIText : LocalizedText {

    protected Text selfText;

    void Start () {

        selfText = GetComponent<Text>();

        UpdateText();
	}

    protected override void UpdateText()
    {
        switch(option)
        {
            case TextOptions.None:

                selfText.text = languageManager.GetLineByTagAttribute(tag);

                break;

            case TextOptions.ToLower:

                selfText.text = languageManager.GetLineByTagAttribute(tag).ToLower();

                break;

            case TextOptions.ToUpper:

                selfText.text = languageManager.GetLineByTagAttribute(tag).ToUpper();

                break;
        }
    }
}
