using System;
using UnityEngine;

namespace SJ.UI
{
    public static class ConfirmationPopupProvider
    {
        private const string ConfirmationPopupPrefabName = "ConfirmationPopup";

        private static ConfirmationPopup ConfirmationPopupPrefab;

        public static void ShowWith(string message, Action onAccept, Action onCancel)
        {
            if (ConfirmationPopupPrefab == null)
                ConfirmationPopupPrefab = SJResources.LoadComponentOfGameObject<ConfirmationPopup>(ConfirmationPopupPrefabName);

            var confirmationPopupInstance = GameObject.Instantiate(ConfirmationPopupPrefab);

            confirmationPopupInstance.Show(message, onAccept, onCancel);
        }
    }
}