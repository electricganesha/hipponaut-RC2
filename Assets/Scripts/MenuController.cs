using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	private Animator animator;
	private bool isAnimationRunning = false;
    private LevelManager levelManager;
    private int currentSelection = 0;
    private AudioSource audioSource;
    private float timeSinceLastAnimation = 0.0f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        audioSource = GetComponent<AudioSource>();
        currentSelection = 0;
    }
	
	// Update is called once per frame
	void Update () {

        bool buttonPressed = Input.GetButton("Fire1");

        timeSinceLastAnimation += Time.deltaTime;

        if (timeSinceLastAnimation >= 0.2f) {
			float direction = Input.GetAxis ("Horizontal");


            if (direction < 0.0f) {
				animator.SetTrigger ("goLeft");
				isAnimationRunning = true;
                audioSource.Play();

			} else if (direction > 0.0f) {
				animator.SetTrigger ("goRight");
				isAnimationRunning = true;
                audioSource.Play();
			}

            if (buttonPressed)
            {
                switch (currentSelection)
                {
                    case (0):
                        Debug.Log("MainScene");
                        levelManager.LoadLevel("MainScene");
                        break;
                    case (1):
                        Debug.Log("Co-Op");
                        levelManager.LoadLevel("MainSceneCoop");
                        break;
                    case (2):
                        Debug.Log("Credits");
                        levelManager.LoadLevel("Credits");
                        break;
                    case (3):
                        Debug.Log("Empty");
                        break;
                    default:
                        break;
                }
            }
            timeSinceLastAnimation = 0.0f;
        }
		
		}

	public void animationFinished()
	{ 
		isAnimationRunning = false;
	}

    public void setCurrentSelection(int selectedMenu)
    {
        currentSelection = selectedMenu;
    }
}
