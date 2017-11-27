using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteCor : MonoBehaviour {

    public SpriteRenderer sr;


	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        Color tmp = sr.color;
        tmp = Color.green;
        tmp.a = 0.5f;
        sr.color = tmp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
