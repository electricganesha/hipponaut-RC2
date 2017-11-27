using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1_WavePlayerManager : MonoBehaviour
{

    //public GameObject player;
    public float shootSpeed = 0.0f;
    public Vector3 generalDistanceForShooting;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // local up
        transform.localPosition = Vector3.MoveTowards(transform.localPosition,generalDistanceForShooting*10, shootSpeed * Time.deltaTime);

    }

}
