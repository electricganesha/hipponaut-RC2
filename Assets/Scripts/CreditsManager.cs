using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour {

    LevelManager lm;

	// Use this for initialization
	void Start () {
        lm = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GoToMainMenu()
    {
        lm.LoadLevel("Menu");
    }
}
