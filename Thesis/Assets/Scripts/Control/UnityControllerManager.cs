using System.Collections.Generic;
using UnityEngine;

public class UnityControllerManager : MonoBehaviour {

    private List<UnityController> controllers;

	// Use this for initialization
	void Awake () {

        controllers = new List<UnityController>();

        controllers.Add(new CharacterController("PlayerController", FindObjectOfType<MainCharacter>()));
	}
	
	// Update is called once per frame
	void Update () {

        for(int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Control();
        }
		
	}


}
