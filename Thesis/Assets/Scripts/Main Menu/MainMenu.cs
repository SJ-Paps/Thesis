using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class MainMenu : SJMonoBehaviour {

    private static MainMenu instance;

    public static MainMenu GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<MainMenu>();

            if(instance == null)
            {
                GameObject gameObjectInstance = Instantiate(SJResources.LoadAsset<GameObject>("MainMenu"));

                instance = gameObjectInstance.GetComponent<MainMenu>();
            }
            
            instance.Init();
        }

        return instance;
    }

    public bool Shown
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

    public event Action onShow;
    public event Action onHide;

    private Canvas canvas;

    private void Init()
    {
        canvas = GetComponent<Canvas>();

        confirmationMenuPool = new List<ConfirmationMenu>();

        //es necesario hacerlo usando <GameObject> porque al ser un prefab el assetbundle no contempla ninguno de sus componentes
        //asique se debe pedir el gameobject primero y luego obtener su componente
        confirmationMenuPrefab = SJResources.LoadAsset<GameObject>("ConfirmationMenuPrefab").GetComponent<ConfirmationMenu>();
    }

    private void OnEnable()
    {
        if (onShow != null)
        {
            onShow();
        }
    }

    private void OnDisable()
    {
        if (onHide != null)
        {
            onHide();
        }
    }

    public void Show()
    {
        if(!Shown)
        {
            gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if(Shown)
        {
            gameObject.SetActive(false);
        }
    }

    private ConfirmationMenu confirmationMenuPrefab;

    private List<ConfirmationMenu> confirmationMenuPool;

    public void DisplayConfirmationMenu(string message, UnityAction onSubmit, UnityAction onCancel)
    {
        ConfirmationMenu menu = GetFirstAvailable();

        if (menu == null)
        {
            menu = GameObject.Instantiate<ConfirmationMenu>(confirmationMenuPrefab);

            confirmationMenuPool.Add(menu);
        }

        menu.Display(message, onSubmit, onCancel, canvas);
    }

    private ConfirmationMenu GetFirstAvailable()
    {
        foreach (ConfirmationMenu menu in confirmationMenuPool)
        {
            if (menu.Active == false)
            {
                return menu;
            }
        }

        return null;
    }
}
