using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float maxMovementInY;
	public Transform target;
	private Vector3 posDiff = Vector3.zero;

	void Start () {
		posDiff = transform.position - target.position;	
	}

	void Update () {
		transform.position = target.position + posDiff;
	}
}
