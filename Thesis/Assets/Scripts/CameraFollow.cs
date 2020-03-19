using SJ;
using UnityEngine;

public class CameraFollow : SJMonoBehaviour {
    
	private Transform target;
    private float z;

    protected override void SJStart() {

        EnableUpdate = true;

        z = transform.position.z;
	}

	protected override void SJUpdate () {

        if(target == null)
        {
            Character c = FindObjectOfType<MainCharacter>();

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
