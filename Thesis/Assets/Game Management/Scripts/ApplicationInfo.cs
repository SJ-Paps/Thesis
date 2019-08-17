using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationInfoAsset", menuName = "Application Info Asset")]
public class ApplicationInfo : ScriptableObject
{
    [SerializeField]
    private string[] beginningScenes;

    [SerializeField]
    private string returnSceneOnEndSession;

    public string[] BeginningScenes { get { return beginningScenes; } }

    public string ReturnSceneOnEndSession { get { return returnSceneOnEndSession; } }
}
