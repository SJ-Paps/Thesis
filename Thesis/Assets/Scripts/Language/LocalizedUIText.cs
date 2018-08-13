using UnityEngine.UI;

public class LocalizedUIText : LocalizedTextComponent {

    protected Text selfText;

    new protected void Awake () {

        selfText = GetComponent<Text>();

        base.Awake();
    }

    protected override void OnTextChanged(string text)
    {
        selfText.text = text;
    }
}
