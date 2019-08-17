using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonController : SJMonoBehaviour {

    [SerializeField]
    private Button newGame, loadGame, resumeGame, options, exitDesktop, exitMainMenu;
    
	protected override void Awake ()
    {
        MainMenu.GetInstance().onShow += UpdateButtonStates;

        //exit desktop button
        exitDesktop.onClick.AddListener(ExitToDesktop);

        //exit main menu button
        exitMainMenu.onClick.AddListener(ExitToMainMenu);

        //load game button
        loadGame.onClick.AddListener(LoadGame);

        //resume game button
        resumeGame.onClick.AddListener(HideMenu);

        UpdateButtonStates();

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
        GameManager.GetInstance().EndSession();
    }

    private void LoadGame()
    {
        GameManager.GetInstance().BeginSessionWithProfile(new ProfileData() { name = "DEFAULTPROFILE" });
    }

    private void UpdateButtonStates()
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
