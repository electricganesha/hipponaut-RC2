using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevelManager : MonoBehaviour
{

    public GameObject water;
    public bool loseLevel = false;
    
    GameManager gameManager;


    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (water.gameObject.transform.localScale.x < 0.8f)
            loseLevel = true;

        if (loseLevel)
           gameManager.CheckLoseCondition();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "satelliteProjectile")
        {
            water.gameObject.transform.localScale = (water.gameObject.transform.localScale) * 0.98f;
        }
    }

}