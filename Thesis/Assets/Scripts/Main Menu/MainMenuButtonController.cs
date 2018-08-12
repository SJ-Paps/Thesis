using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour {

    [SerializeField]
    private Button exit;

    private Canvas canvas;

    private LocalizedTextLibrary localizedTextLibrary;

	void Awake () {

        localizedTextLibrary = LocalizedTextLibrary.GetInstance();

        canvas = GetComponent<Canvas>();

        //exit button
        exit.onClick.AddListener(ExitAction);

    }

    private void ExitAction()
    {
        ModalWindowManager.Instance.DisplayConfirmationMenu(localizedTextLibrary.GetLineByTagAttribute("confirmation_menu_message_exit").FirstLetterToUpper(), Application.Quit, null, canvas.rootCanvas);
    }
}
