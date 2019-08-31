﻿using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonController : SJMonoBehaviour {

    [SerializeField]
    private Button newGame, loadGame, resumeGame, options, exitDesktop, exitMainMenu, continueLastProfile;
    
	protected override void SJAwake ()
    {
        MainMenu.GetInstance().onShow += UpdateButtonStates;

        //exit desktop button
        exitDesktop.onClick.AddListener(ExitToDesktop);

        //exit main menu button
        exitMainMenu.onClick.AddListener(ExitToMainMenu);

        //resume game button
        resumeGame.onClick.AddListener(HideMenu);

        //continue button
        continueLastProfile.onClick.AddListener(Continue);
        
    }

    protected override void SJOnEnable()
    {
        UpdateButtonStates();
    }

    private void ExitToDesktop()
    {
        MainMenu.GetInstance().DisplayConfirmationMenu(LanguageManager.GetLocalizedTextLibrary().GetLineByTagOfCurrentLanguage("confirmation_menu_message_exit").FirstLetterToUpper(), Application.Quit, null);
    }

    private void ExitToMainMenu()
    {
        MainMenu.GetInstance().DisplayConfirmationMenu(LanguageManager.GetLocalizedTextLibrary().GetLineByTagOfCurrentLanguage("confirmation_menu_message_exit").FirstLetterToUpper(), GoMenu, null);
    }

    private void HideMenu()
    {
        MainMenu.GetInstance().Hide();
    }

    private void GoMenu()
    {
        GameManager.GetInstance().EndSession();
    }

    private void Continue()
    {
        string lastProfile = GameConfigurationCareTaker.GetConfiguration().lastProfile;

        if (ProfileCareTaker.ProfileExistsAndIsValid(lastProfile))
        {
            Debug.Log(lastProfile);
            GameManager.GetInstance().BeginSessionWithProfile(ProfileCareTaker.GetProfileDataFromProfile(lastProfile));
        }
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
            continueLastProfile.gameObject.SetActive(false);
        }
        else
        {
            exitDesktop.gameObject.SetActive(true);
            exitMainMenu.gameObject.SetActive(false);
            newGame.gameObject.SetActive(true);
            loadGame.gameObject.SetActive(true);
            resumeGame.gameObject.SetActive(false);
            options.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(GameConfigurationCareTaker.GetConfiguration().lastProfile) == false)
            {
                continueLastProfile.gameObject.SetActive(true);
            }
            else
            {
                continueLastProfile.gameObject.SetActive(false);
            }
        }

        
    }
}
