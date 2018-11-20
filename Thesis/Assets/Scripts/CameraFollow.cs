using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
	private Transform target;
    private float z;

    void Start() {

        z = transform.position.z;
	}

	void Update () {

        if(target == null)
        {
            Character c = GameManager.GetInstance().FindPlayer();

            if(c != null)
            {
                target = c.transform;
            }
        }
        else
        {
            transform.position = new Vector3(target.position.x, target.position.y, z);
        }
	}
}
