using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonController : SJMonoBehaviour {

    [SerializeField]
    private Button newGame, loadGame, resumeGame, options, exitDesktop, exitMainMenu;

    private Canvas canvas;

    private MainMenu mainMenu;

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

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void Start()
    {
        mainMenu = MainMenu.GetInstance();
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
        GameManager.Instance.Pause(false);
    }

    private void GoMenu()
    {
        GameManager.Instance.GoMenu();
    }

    private void GoNewGame()
    {
        SceneLoader.GetInstance().NewGame();
    }

    private void LoadGame()
    {
        SceneLoader.GetInstance().LoadGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                resumeGame.gameObject.SetActive(false);
                exitMainMenu.gameObject.SetActive(false);
                newGame.gameObject.SetActive(true);
                loadGame.gameObject.SetActive(true);
                exitDesktop.gameObject.SetActive(true);
                options.gameObject.SetActive(true);

                break;

            case 1:
                resumeGame.gameObject.SetActive(true);
                exitMainMenu.gameObject.SetActive(true);
                newGame.gameObject.SetActive(false);
                loadGame.gameObject.SetActive(false);
                exitDesktop.gameObject.SetActive(true);
                options.gameObject.SetActive(true);

                break;


        }
    }
}
