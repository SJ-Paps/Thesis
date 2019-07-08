using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonController : SJMonoBehaviour {

    [SerializeField]
    private Button newGame, loadGame, resumeGame, options, exitDesktop, exitMainMenu;
    
	protected override void Awake ()
    {
        MainMenu.GetInstance().onShow += OnMainMenuShow;

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

        GameManager.GetInstance().SetOnQuitReturnScene("Menu");

    }

    private void ExitToDesktop()
    {
        MainMenu.GetInstance().DisplayConfirmationMenu(LocalizedTextLibrary.GetInstance().GetLineByTagAttribute("confirmation_menu_message_exit").FirstLetterToUpper(), Application.Quit, null);
    }

    private void ExitToMainMenu()
    {
        MainMenu.GetInstance().DisplayConfirmationMenu(LocalizedTextLibrary.GetInstance().GetLineByTagAttribute("confirmation_menu_message_exit").FirstLetterToUpper(), GoMenu, null);
    }

    private void HideMenu()
    {
        MainMenu.GetInstance().Hide();
    }

    private void GoMenu()
    {
        GameManager.GetInstance().QuitGame();
    }

    private void GoNewGame()
    {
        GameManager.GetInstance().LoadGame(new string[] { "MasterSceneLevel1", "Entities_FirstGame_igld" });
        MainMenu.GetInstance().Hide();
    }

    private void LoadGame()
    {
        GameManager.GetInstance().LoadGame(GameManager.SaveFilePath);
        MainMenu.GetInstance().Hide();
    }

    private void OnMainMenuShow()
    {
        if (GameManager.GetInstance().IsInGame)
        {
            exitDesktop.gameObject.SetActive(true);
            exitMainMenu.gameObject.SetActive(true);
            newGame.gameObject.SetActive(false);
            loadGame.gameObject.SetActive(true);
            resumeGame.gameObject.SetActive(true);
            options.gameObject.SetActive(true);
        }
        else
        {
            exitDesktop.gameObject.SetActive(true);
            exitMainMenu.gameObject.SetActive(false);
            newGame.gameObject.SetActive(true);
            loadGame.gameObject.SetActive(true);
            resumeGame.gameObject.SetActive(false);
            options.gameObject.SetActive(true);
        }
    }
}
