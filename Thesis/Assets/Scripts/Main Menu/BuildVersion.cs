using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BuildVersion : MonoBehaviour {
    
	void Awake()
    {
        GetComponent<Text>().text = "v" + Application.version;
    }
}
