using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalWindowManager : MonoBehaviour {

    private static ModalWindowManager instance;

    public static ModalWindowManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ModalWindowManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ConfirmationMenu confirmationMenuPrefab;

    private List<ConfirmationMenu> confirmationMenuPool;

    void Awake()
    {
        instance = this;
        confirmationMenuPool = new List<ConfirmationMenu>();
        DontDestroyOnLoad(this);
    }

    public void DisplayConfirmationMenu(string message, UnityAction onSubmit, UnityAction onCancel, Canvas root)
    {
        ConfirmationMenu menu = GetFirstAvailable();

        if(menu == null)
        {
            menu = Instantiate<ConfirmationMenu>(confirmationMenuPrefab);

            confirmationMenuPool.Add(menu);
        }

        menu.Display(message, onSubmit, onCancel, root);
    }

    private ConfirmationMenu GetFirstAvailable()
    {
        foreach(ConfirmationMenu menu in confirmationMenuPool)
        {
            if(menu.Active == false)
            {
                return menu;
            }
        }

        return null;
    }
}
