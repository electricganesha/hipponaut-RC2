using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool isSplashScreen1 = false;
    public bool isSplashScreen2 = false;
    private float timeToLoadMenu = 0.0f;

    void Start()
    {
      
    }

    void Update()
    {

        if (isSplashScreen1)
        {
            timeToLoadMenu += Time.deltaTime;
            if (timeToLoadMenu > 4.0f)
            {
                LoadLevel("Splash-2");
            }
        }

        if (isSplashScreen2)
        {
            timeToLoadMenu += Time.deltaTime;
            if (timeToLoadMenu > 4.0f)
            {
                LoadLevel("Menu");
            }
        }
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitRequest()
    {
        Application.Quit();
    }
}
