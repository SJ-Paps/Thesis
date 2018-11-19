using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
	private Transform target;
    private float z;

    void Start() {

        z = transform.position.z;

        target = GameManager.GetInstance().FindPlayer().transform;
	}

	void Update () {
        transform.position = new Vector3(target.position.x, target.position.y, z);
	}
}
