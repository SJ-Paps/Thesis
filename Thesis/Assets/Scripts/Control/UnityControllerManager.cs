using System.Collections.Generic;
using UnityEngine;

public class UnityControllerManager : MonoBehaviour {

    [SerializeField]
    private List<UnityController> controllers;

	// Use this for initialization
	void Awake () {

        controllers = new List<UnityController>();
	}
	
	// Update is called once per frame
	void Update () {

        for(int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Control();
        }
		
	}


}
