using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum options {
    right,
    left
}

public class SateliteManager : MonoBehaviour {

    public GameManager gameManager;

    public Vector3 axis;
    public GameObject center;

    private GameObject newWave;

    [SerializeField]
    private GameObject wave;
    [SerializeField]
    private GameObject spriteAviso;

    private float selfRotationSpeed;

    [HideInInspector]
    public bool goDireita;
    [HideInInspector]
    public float orbitRotationSpeed;
    [HideInInspector]
    public float timeToShoot;
    [HideInInspector]
    public int minColorShot = 0;
    [HideInInspector]
    public int maxColorShot = 3;
    [HideInInspector]
    public float distanciaATerra;

    //Animation Variables
    Animator anim;
    
    //[SerializeField]
    private Color[] cores = new Color[3];

    public AudioClip[] audioClips;
    private AudioClip currentClip;
    private AudioSource audioSource;

    private float attackCounter;
    private bool isAttacking = false;
    private float time;
    private float timeToWarn;
    
    private bool stopShooting = false;

    private Color cor;

    private GameObject aviso;

    private bool sateliteDestruido = false;

    // Use this for initialization
    void Start()
    {
        selfRotationSpeed = Random.Range(50f, 100f);

        transform.Rotate(-90, 0, 0);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = null;
        audioSource.loop = false;

        cores[0] = Color.red;
        cores[1] = Color.green;
        cores[2] = Color.blue;

        gameManager = GameObject.FindObjectOfType<GameManager>();
        
        //Get destroy Animation
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (!goDireita) {
            axis = Vector3.forward;
            transform.RotateAround(center.transform.position, axis, orbitRotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward, selfRotationSpeed * Time.deltaTime);
        } else {
            axis = Vector3.back;
            transform.RotateAround(center.transform.position, axis, orbitRotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.back, selfRotationSpeed * Time.deltaTime);
        }

        time += Time.deltaTime;
        timeToWarn += Time.deltaTime;

        if(timeToWarn >= (timeToShoot - 1.0f) && !stopShooting)
        {
            cor = selectColor();

            if (cor == cores[0])
            {
                playSound(audioClips[0]);
            }
            else if (cor == cores[1])
            {
                playSound(audioClips[1]);
            }
            else if (cor == cores[2])
            {
                playSound(audioClips[2]);
            }

            aviso = Instantiate(spriteAviso);
            aviso.GetComponent<SpriteRenderer>().color = cor;
            aviso.transform.position = transform.position;

            timeToWarn = 0.0f;
        }

        

        if(time >= timeToShoot && !stopShooting)
        {

            newWave = Instantiate(wave);

            newWave.GetComponent<SpriteRenderer>().color = cor;

            newWave.transform.localPosition = transform.localPosition;      

            if(!goDireita)
            {
                newWave.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.x + 90);
            }
            else
            {
                newWave.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.x - 90);
            }

            time = 0.0f;
        }

        //Destruir Satelite
        if (sateliteDestruido && Mathf.Abs(transform.position.y) > (distanciaATerra - 1.0f))
        {
            Destroy(gameObject);
        }

    }

    public Color selectColor()
    {
        int randomCor = Random.Range(minColorShot, maxColorShot + 1);
        return cores[randomCor] ;
    }

    public void playSound(AudioClip soundNow)
    {
        if (audioSource.clip != soundNow)
        {
            audioSource.Stop();
            audioSource.clip = soundNow;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy Satellite on collision with Gama Ray (white)
        if (other.GetComponent<SpriteRenderer>().color == Color.white)
        {
            anim.SetTrigger("destroySatelite");

            BoxCollider col = gameObject.GetComponent<BoxCollider>();
            col.enabled = false;

            stopShooting = true;
            Destroy(other.gameObject);
        }
    }

    public void SateliteDestruido()
    {
        sateliteDestruido = true;
        gameManager.destroySatelite();
    }
}
