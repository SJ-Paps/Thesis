using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MovableObject {

    private BoxCollider2D boxBoxCollider2D;
    private Rigidbody2D boxRigidBody2D;

    private void Awake() 
    {
        boxBoxCollider2D = GetComponent<BoxCollider2D>();
        boxRigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Start () 
    {

	}
	
	void Update () 
    {
		
	}

    void OnCollisionEnter2D(Collision2D collision) 
    {

    }

    void OnCollisionExit2D(Collision2D collision) 
    {
    }
}
