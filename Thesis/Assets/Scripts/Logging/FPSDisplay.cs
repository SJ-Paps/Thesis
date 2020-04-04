using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [RuntimeInitializeOnLoadMethod]
    private static void InstantiateOnLoad()
    {
        new GameObject("FSPDisplay")
            .AddComponent<FPSDisplay>()
            .gameObject.DontDestroyOnLoad();
    }

    float deltaTime = 0.0f;

    private GUIStyle style = new GUIStyle();

    private void Awake()
    {
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int screenWidth = Screen.width, screenHeight = Screen.height;

        Rect rect = new Rect(0, 0, screenWidth, screenHeight * 2 / 100);

        style.fontSize = screenHeight * 2 / 100;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
#endif
}
