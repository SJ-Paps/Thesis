using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalWindowManager {

    private static ModalWindowManager instance;

    public static ModalWindowManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ModalWindowManager();
            }

            return instance;
        }
    }
    
    private ConfirmationMenu confirmationMenuPrefab;

    private List<ConfirmationMenu> confirmationMenuPool;

    private ModalWindowManager()
    {
        confirmationMenuPool = new List<ConfirmationMenu>();

        //es necesario hacerlo usando <GameObject> porque al ser un prefab el assetbundle no contempla ninguno de sus componentes
        //asique se debe pedir el gameobject primero y luego obtener su componente
        confirmationMenuPrefab = SJResources.LoadAsset<GameObject>("ConfirmationMenuPrefab").GetComponent<ConfirmationMenu>();
    }

    public void DisplayConfirmationMenu(string message, UnityAction onSubmit, UnityAction onCancel, Canvas root)
    {
        ConfirmationMenu menu = GetFirstAvailable();

        if(menu == null)
        {
            menu = GameObject.Instantiate<ConfirmationMenu>(confirmationMenuPrefab);

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
