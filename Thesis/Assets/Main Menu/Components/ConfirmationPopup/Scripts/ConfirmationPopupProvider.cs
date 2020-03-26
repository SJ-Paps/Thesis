using SJ.Management;
using System;
using UnityEngine;

namespace SJ.Menu
{
    public static class ConfirmationPopupProvider
    {
        private const string ConfirmationPopupPrefabName = "ConfirmationPopup";

        public static void ShowWith(string message, Action onAccept, Action onCancel)
        {
            var confirmationPopupInstance = GameObject.Instantiate(SJResources.LoadComponentOfGameObject<ConfirmationPopup>(ConfirmationPopupPrefabName));

            confirmationPopupInstance.Show(message, onAccept, onCancel);
        }
    }
}