using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float maxMovementInY;
	private Transform target;
	private Vector3 posDiff = Vector3.zero;

	void Start () {

        target = GameManager.GetInstance().FindPlayer().transform;

        posDiff = transform.position - target.position;	
	}

	void Update () {
		transform.position = target.position + posDiff;
	}
}
