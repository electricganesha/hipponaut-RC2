using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2_WaveManager : MonoBehaviour {

    public Transform posicaoDoPlaneta;
    public float shootSpeed = 0.0f;

    public Vector3 directionVector = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, posicaoDoPlaneta.localPosition, shootSpeed * Time.deltaTime);

        directionVector = Vector3.MoveTowards(posicaoDoPlaneta.localPosition, transform.localPosition, 100);

        float angle = calculateProjectileAngle();

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (transform.localPosition == posicaoDoPlaneta.localPosition)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "earth")
        {
            Destroy(gameObject);
        }
        else if (other.GetComponent<SpriteRenderer>().color == gameObject.GetComponent<SpriteRenderer>().color)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    public float calculateProjectileAngle()
    {
        Vector3 dir = new Vector3(0, 0, 0);

        if (directionVector.x > 0 && directionVector.y >= 0)
        {
            dir = directionVector;
        }
        else if (directionVector.x < 0 && directionVector.y > 0)
        {
            dir = -directionVector;
        }
        else if (directionVector.x > 0 && directionVector.y <= 0)
        {
            dir = -directionVector;
        }
        else if (directionVector.x < 0 && directionVector.y < 0)
        {
            dir = directionVector;
        }


        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        if (angle > 0 && angle < 90)
        {
            angle = angle - 90;
        }
        else if (angle > 90)
        {
            angle = angle + 90;
        }
        else if (angle > -180 && angle < -90)
        {
            angle = angle -90;
        }
        else if (angle < 0 && angle > -90)
        {
            angle = angle + 90;
        }

        return angle;
    }
}
