using UnityEngine.UI;

public class LocalizedUIText : LocalizedText {

    protected Text selfText;

    public override string Text
    {
        get
        {
            return selfText.text;
        }

        protected set
        {
            selfText.text = value;
        }
    }

    void Start () {

        selfText = GetComponent<Text>();

        UpdateText();
	}
}
