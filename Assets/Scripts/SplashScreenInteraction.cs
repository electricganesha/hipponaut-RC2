using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenInteraction : MonoBehaviour {

    private LevelManager levelManager;

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
    }
	
	// Update is called once per frame
	void Update () {
        bool pressingButton = Input.GetButton("Fire1");

        if(pressingButton)
        {
            levelManager.LoadLevel("Menu");
        }
    }
}
