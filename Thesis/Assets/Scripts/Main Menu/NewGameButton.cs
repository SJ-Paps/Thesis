using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour {

    private Button button;

	// Use this for initialization
	void Start () {

        button = GetComponent<Button>();

        button.onClick.AddListener(GoNewGame);
		
	}
	
	private void GoNewGame()
    {
        SceneManager.LoadScene(1);
    }
}
