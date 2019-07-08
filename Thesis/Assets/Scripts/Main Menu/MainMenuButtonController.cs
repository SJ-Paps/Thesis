using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

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

        //new game button
        newGame.onClick.AddListener(GoNewGame);

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
        GameManager.GetInstance().QuitGame();
        SceneManager.LoadScene("Menu");
    }

    private void GoNewGame()
    {
        CoroutineManager.GetInstance().StartCoroutine(NewGameCoroutine());
    }

    private IEnumerator NewGameCoroutine()
    {
        GameManager.GetInstance().onLoadingSucceeded += CloseLoadingScreen;

        AsyncOperation openLoadingScreenOperation = LoadingScreenManager.Open();

        GameManager.GetInstance().NewGame(Application.persistentDataPath, new string[] { "MasterSceneLevel1", "Entities_FirstGame_igld" });

        while(openLoadingScreenOperation.isDone == false)
        {
            yield return null;
        }
        
        SceneManager.UnloadSceneAsync("Menu");
    }

    private void CloseLoadingScreen()
    {
        LoadingScreenManager.Close();
    }

    private void LoadGame()
    {
        CoroutineManager.GetInstance().StartCoroutine(LoadGameCoroutine());
    }

    private IEnumerator LoadGameCoroutine()
    {
        GameManager.GetInstance().onLoadingSucceeded += CloseLoadingScreen;

        AsyncOperation openLoadingScreenOperation = LoadingScreenManager.Open();

        GameManager.GetInstance().LoadGame(Application.persistentDataPath);

        while (openLoadingScreenOperation.isDone == false)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("Menu");
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
