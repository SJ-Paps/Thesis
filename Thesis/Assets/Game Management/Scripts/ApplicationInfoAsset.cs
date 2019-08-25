using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationInfoAsset", menuName = "Application Info Asset")]
public class ApplicationInfoAsset : ScriptableObject
{
    [SerializeField]
    private string[] beginningScenes;

    [SerializeField]
    private string returnSceneOnEndSession;

    [SerializeField]
    private GameConfiguration defaultGameConfiguration;

    public string[] BeginningScenes { get { return beginningScenes; } }

    public string ReturnSceneOnEndSession { get { return returnSceneOnEndSession; } }

    public GameConfiguration DefaultGameConfiguration { get { return defaultGameConfiguration; } }
}
