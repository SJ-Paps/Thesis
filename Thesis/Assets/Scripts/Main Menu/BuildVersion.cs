using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BuildVersion : SJMonoBehaviour {
    
	protected override void SJAwake()
    {
        GetComponent<Text>().text = "v" + Application.version;
    }
}
