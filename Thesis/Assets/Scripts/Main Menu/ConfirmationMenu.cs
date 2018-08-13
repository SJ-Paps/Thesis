using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationMenu : SJMonoBehaviour {

    [SerializeField]
    private Text message;

    [SerializeField]
    private Button submitButton, cancelButton;

    public bool Active
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

    void Awake()
    {
        gameObject.SetActive(false);
    }
	
	public void Display(string message, UnityAction onSubmit, UnityAction onCancel, Canvas root)
    {
        this.message.text = message;

        if(onSubmit != null)
        {
            submitButton.onClick.AddListener(onSubmit);
        }

        submitButton.onClick.AddListener(OnConfirmation);

        if(onCancel != null)
        {
            cancelButton.onClick.AddListener(onCancel);
        }
        
        cancelButton.onClick.AddListener(OnConfirmation);

        transform.SetParent(root.transform, false);

        gameObject.SetActive(true);
    }

    private void OnConfirmation()
    {
        submitButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
