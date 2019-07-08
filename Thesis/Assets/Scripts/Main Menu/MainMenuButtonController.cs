using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonController : SJMonoBehaviour {

    [SerializeField]
    private Button newGame, loadGame, resumeGame, options, exitDesktop, exitMainMenu;

    private Canvas canvas;

    private LocalizedTextLibrary localizedTextLibrary;
    
	protected override void Awake () {

        localizedTextLibrary = LocalizedTextLibrary.GetInstance();

        canvas = GetComponent<Canvas>();

        //exit desktop button
        exitDesktop.onClick.AddListener(ExitToDesktop);

        //exit main menu button
        exitMainMenu.onClick.AddListener(ExitToMainMenu);

        //new game button
        newGame.onClick.AddListener(GoNewGame);

        //load game button
        loadGame.onClick.AddListener(LoadGame);

        //resume game button
        resumeGame.onClick.AddListener(HideMenu);

    }

    private void ExitToDesktop()
    {
        ModalWindowManager.Instance.DisplayConfirmationMenu(localizedTextLibrary.GetLineByTagAttribute("confirmation_menu_message_exit").FirstLetterToUpper(), Application.Quit, null, canvas.rootCanvas);
    }

    private void ExitToMainMenu()
    {
        ModalWindowManager.Instance.DisplayConfirmationMenu(localizedTextLibrary.GetLineByTagAttribute("confirmation_menu_message_exit").FirstLetterToUpper(), GoMenu, null, canvas.rootCanvas);
    }

    private void HideMenu()
    {

    }

    private void GoMenu()
    {

    }

    private void GoNewGame()
    {
        GameManager.GetInstance().LoadGame(new string[] { "MasterSceneLevel1", "Entities_FirstGame" });
    }

    private void LoadGame()
    {
        GameManager.GetInstance().LoadGame(GameManager.SaveFilePath);
    }
}
