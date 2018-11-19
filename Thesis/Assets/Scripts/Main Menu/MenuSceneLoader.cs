using UnityEngine;

public class MenuSceneLoader : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        GameManager.GetInstance();
        MainMenu.GetInstance();
        CoroutineManager.GetInstance();
	}
}
