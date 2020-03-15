using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ConfirmationPopup : SJMonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI message;

    [SerializeField]
    private Button acceptButton, cancelButton;

    public bool Active
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

    protected override void SJAwake()
    {
        gameObject.SetActive(false);
    }

    public void Show(string message, Action onAccept, Action onCancel)
    {
        this.message.text = message;

        acceptButton.onClick.AddListener(() => 
        {
            ClearListenersAndDismiss();
            onAccept();
        });

        cancelButton.onClick.AddListener(() =>
        {
            ClearListenersAndDismiss();
            onCancel();
        });

        gameObject.SetActive(true);
    }

    private void ClearListenersAndDismiss()
    {
        acceptButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        Destroy(gameObject);
    }
}
