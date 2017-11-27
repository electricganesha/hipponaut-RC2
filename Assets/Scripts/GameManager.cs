using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private LevelManager levelManager;

    [SerializeField]
    private int numeroOrbitas = 3;

    public GameObject satelitePrefab;

    private int numTotalSatelites = 0;

    [System.Serializable]
    public class orbita
    {
        public int numSatelites;
        public float orbitRotationSpeed;
        public float distanciaATerra;
        public bool goDireita;
        public float timeToShootMin;
        public float timeToShootMax;
        public int minColorShot;
        public int maxColorShot;

    }

    [SerializeField]
    List<orbita> orbitas;

    private int numObjects = 1;

    private int satelitesDestroyed = 0;

    private float timeSinceWin = 0.0f;
    private float timeSinceLose = 0.0f;

    void Start()
    {
        Vector3 center = new Vector3(0, 0, 0);

        levelManager = GameObject.FindObjectOfType<LevelManager>();
        
        foreach(orbita o in orbitas)
        {
            numTotalSatelites += o.numSatelites;

            float anguloRotacaoFixed = 360 / o.numSatelites;

            float anguloRotacao = 0.0f;

            for (int i=0; i<o.numSatelites; i++)
            {
                anguloRotacao = anguloRotacao += anguloRotacaoFixed;

                Vector3 pos = RandomCircle(center, (float)o.distanciaATerra);
                GameObject satelite = Instantiate(satelitePrefab, pos, Quaternion.identity);

                satelite.transform.RotateAround(center,new Vector3(0, 0, 1), anguloRotacao);

                satelite.GetComponent<SateliteManager>().goDireita = o.goDireita;
                satelite.GetComponent<SateliteManager>().orbitRotationSpeed = o.orbitRotationSpeed;
                satelite.GetComponent<SateliteManager>().timeToShoot = Random.Range(o.timeToShootMin, o.timeToShootMax);
                satelite.GetComponent<SateliteManager>().minColorShot = o.minColorShot;
                satelite.GetComponent<SateliteManager>().maxColorShot = o.maxColorShot;
                satelite.GetComponent<SateliteManager>().distanciaATerra = o.distanciaATerra;
            }
        }

    }

    void Update()
    {
        CheckWinCondition();
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        Vector3 pos;
        pos.x = center.x ;
        pos.y = center.y + radius * Mathf.Cos(Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    /*
     *  Ganhou Minino
     */
    private void CheckWinCondition()
    {

        if(numTotalSatelites == satelitesDestroyed)
        {
            
            timeSinceWin += Time.deltaTime;
            if (timeSinceWin >= 5.0f)
            {
                levelManager.LoadLevel("WinScene");
            }    
        }

    }

    /*
     * Perdeu Minino
     */
    public void CheckLoseCondition()
    {
        timeSinceLose += Time.deltaTime;
        if (timeSinceLose >= 3.0f)
        {
            levelManager.LoadLevel("GameOverScene");
        }
    }

    public void destroySatelite()
    {
        satelitesDestroyed++;
    }

 
}
