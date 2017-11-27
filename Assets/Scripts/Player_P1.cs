using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_P1 : MonoBehaviour {

    private Quaternion rotation;
    private Rigidbody rb;
    private Vector3 velocity;
    private Vector3 theScale;

    private bool lastDirection = false;
    private bool playerMoving = false;
    private float playerVelocity = 0.0f;
    private float buttonTimePressed = 0.0f;
    private float timeSinceLastShot = 0.0f;
    private bool shotCooldown = false;
    private bool facingRight = false;

    public float velocityMultiplier = 3.0f;
    public float brakeDrag = 0.7f;
    public float runningDrag = 0.45f;

    public string horizontalAxis = "Horizontal_P1";
    public string shootButton = "Fire1_P1";

    public bool goingLeft = false;
    public bool goingRight = false;
    public bool playerIdle = false;
    public bool orderBrake = false;

    private bool naoVirou = true;

    private Quaternion playerRotation;

    //Ricardo
    [SerializeField]
    private GameObject wave;

    public Transform centerOfEarth;
    private Vector3 directionVector;

    private bool buttonPressed = false;
    private bool buttonReleased = false;
    private bool buttonJustReleased = false;

    private bool shotSoundPlayed = false;


    private Color fireColor;
    private Color cor;

    private GameObject implosion;
    private GameObject derrapagem_esq;
    private GameObject derrapagem_dir;

    //Player Audio
    public AudioClip[] audioClips;
    public AudioClip[] characterVoice;
    private AudioClip currentClip;
    private AudioSource[] audioSources = new AudioSource[3];

    public float[] speakInterval = new float [2];
    
    private float timeCounter = 0.0f;
    private float randomTime = 0.0f;
   

    Animator anim;

    void Awake()
    {
       implosion = GameObject.FindGameObjectWithTag("P1_implosionWave");
       implosion.SetActive(false);
       derrapagem_dir = GameObject.FindGameObjectWithTag("derrapagemDir");
       derrapagem_esq = GameObject.FindGameObjectWithTag("derrapagemEsq");
       derrapagem_dir.SetActive(false);
       derrapagem_esq.SetActive(false);

       //Corrigir bug temporario de disparo inicio de jogo
       Vector3 fixBugPosition = transform.position;
       fixBugPosition.x = -2.0f;
       transform.position = fixBugPosition;

    }

    // Use this for initialization
    void Start () {

       Vector3 currentPosition = transform.position;
       currentPosition.x = -0.2f;
       transform.position = currentPosition;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        audioSources[0] = gameObject.AddComponent<AudioSource>();
        audioSources[1] = gameObject.AddComponent<AudioSource>();
        audioSources[2] = gameObject.AddComponent<AudioSource>();

        //CORRER DERRAPAR ETC
        audioSources[0].clip = null;
        audioSources[0].loop = true;

        //DISPARO CHARGE
        audioSources[1].clip = null;
        audioSources[1].loop = true;

        //VOZ
        audioSources[2].clip = null;
        audioSources[2].loop = false;

       

    }

    // Update is called once per frame
    void Update()
    {
        //Get input from Left Stick
        float x = -Input.GetAxis(horizontalAxis);
        bool pressingButton = Input.GetButton(shootButton);

        Vector3 currentPosition = transform.position;
        currentPosition.z = 0.0f;
        transform.position = currentPosition;


        // if (!buttonPressed)
        //Fire Button pressed - Charge Effect
        if (pressingButton && !shotCooldown)
        {
            buttonTimePressed += Time.deltaTime;
            implosion.SetActive(true);
            buttonPressed = true;
            fireColor = Charge(buttonTimePressed);
            timeSinceLastShot = 0;
            buttonReleased = false;
        }
        else if (buttonPressed)
        {
            timeSinceLastShot += Time.deltaTime;
            Fire(fireColor);
            implosion.SetActive(false);
            buttonPressed = false;
            buttonReleased = true;
            buttonTimePressed = 0.0f;
            shotCooldown = true;
        }
        else if (shotCooldown)
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > 0.4f)
                shotCooldown = false;
        }


        //Tweak velocity and set maxAngularVelocity
        x *= velocityMultiplier;
        rb.maxAngularVelocity = x;

        //Get current vector velocity (float)
        playerVelocity = rb.angularVelocity.magnitude;

        lastDirection = goingLeft;

        // Detect no joystick input or direction change to order brake animation
        if (Mathf.Abs(x) < 0.1f)
        {
            

            if (playerVelocity >= 0.04) // Player Braking
            {
                orderBrake = true;
                rb.drag = brakeDrag;
               
            }
            else // Player Idle
            {
                orderBrake = false;
                playerIdle = true;
                goingLeft = false;
                goingRight = false;
                rb.drag = runningDrag;
                derrapagem_dir.SetActive(false);
                derrapagem_esq.SetActive(false);
            }


        }
        // Detect player joystick input and move player
        else if  (Mathf.Abs(x) >= 0.16f)
        {

            rb.drag = runningDrag;
            rb.angularVelocity = new Vector3(0, 0, x);
            playerMoving = true;
            playerIdle = false;
            orderBrake = false;

            //detecção de direcçao
            if (x > 0)
            {
                derrapagem_dir.SetActive(true);
                derrapagem_esq.SetActive(false);
                goingLeft = true;
                goingRight = false;
                if (!facingRight)
                {
                    flip();
                }
            }
            else
            {
                derrapagem_dir.SetActive(false);
                derrapagem_esq.SetActive(true);
                goingLeft = false;
                goingRight = true;
                if (facingRight)
                {
                    flip();
                }
            }
        }

        anim.SetBool("idle", playerIdle);
        anim.SetBool("goingLeft", goingLeft);
        anim.SetBool("goingRight", goingRight);

        anim.SetBool("running", false);
        if (orderBrake)
        {
            if (goingLeft)
            {
                anim.SetBool("sliding_left", goingLeft);
            }
            else if (goingRight)
            {
                anim.SetBool("sliding_right", goingRight);
            }
            else
            { 
                anim.SetBool("sliding_left", false);
                anim.SetBool("sliding_right", false);
            }
        }
        else
        {
            anim.SetBool("sliding_left", false);
            anim.SetBool("sliding_right", false);
            anim.SetBool("running", false);
            if (!playerIdle)
            {
                anim.SetBool("running", true);
            }
             

        }

        if (anim.GetBool("running"))
        {
            playSound(audioSources[0],audioClips[0],true, false);
        }
        else if (anim.GetBool("sliding_left"))
        {
            playSound(audioSources[0], audioClips[1], false, false);
        } else if (anim.GetBool("sliding_right"))
        {
            playSound(audioSources[0], audioClips[1], false, false);
        }
        else
        {
            audioSources[0].Stop();
        }

        if (buttonPressed)
        {
            shotSoundPlayed = false;
            playSound(audioSources[1], audioClips[2], true, false);
          //  buttonJustReleased = true;
        }
        else if (buttonReleased) //&& buttonJustReleased)
        {
            playSound(audioSources[1], audioClips[3], false, true);
           // buttonJustReleased = false;
        }
        else
        {
            audioSources[1].Stop();
        }

        //Tocar sons de voz
        if (timeCounter > randomTime)
        {
            randomTime = Random.Range(speakInterval[0], speakInterval[1]);

            timeCounter = 0.0f;

            playCharacterVoice();  
        }

        timeCounter += Time.deltaTime;
        directionVector = Vector3.MoveTowards(centerOfEarth.position, transform.localPosition, 100);

    }

    public void TurnRight()
    {
        if(naoVirou)
        {
            transform.Rotate(0, -90, 0);
            naoVirou = false;
        }
        
    }


    void Fire(Color fireColor)
    {
        directionVector = Vector3.MoveTowards(centerOfEarth.position, transform.localPosition, 100);

        GameObject newWave = Instantiate(wave);

        newWave.GetComponent<SpriteRenderer>().color = fireColor;
        newWave.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,transform.localPosition.z) * 1.4f;
        

        newWave.GetComponent<WavePlayerManager>().generalDistanceForShooting = directionVector;

        float angle = calculateProjectileAngle();             

        newWave.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    public Color Charge(float buttonTimePressed)
    {
        
        if (buttonTimePressed > 1.5f)
        {
            implosion.GetComponent<SpriteRenderer>().color = Color.white;
            return Color.white;
        }
        else if (buttonTimePressed > 1.0f)
        {
            implosion.GetComponent<SpriteRenderer>().color = Color.blue;
            return Color.blue;
        }
        else if (buttonTimePressed > 0.5f)
        {
            implosion.GetComponent<SpriteRenderer>().color = Color.green;
            return Color.green;
        }
        else if (buttonTimePressed > 0)
        {
            implosion.GetComponent<SpriteRenderer>().color = Color.red;
            return Color.red;
        }
        return new Color();
    }
  
    public void flip()
    {
        facingRight = !facingRight;
        theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    public void playSound(AudioSource source, AudioClip soundNow, bool isLooping, bool playOnce)
    {

        source.loop = isLooping;

        if (source.clip != soundNow)
        {
            source.Stop();
            source.clip = soundNow;
        }

        if (!source.isPlaying && !playOnce)
        {
            source.Play();
        }
        else if (playOnce && !shotSoundPlayed)
        {
            source.PlayOneShot(soundNow);
            shotSoundPlayed = true;

        }
    }

    public float calculateProjectileAngle()
    {
        Vector3 dir = new Vector3(0, 0, 0);

        if (directionVector.x >= 0 && directionVector.y >= 0)
        {
            dir = directionVector;
        }
        else if (directionVector.x < 0 && directionVector.y > 0)
        {
            dir = -directionVector;
        }
        else if (directionVector.x >= 0 && directionVector.y <= 0)
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
            angle = angle + 90;
        }
        else if (angle > 90)
        {
            angle = angle - 90;
        }
        else if (angle > -180 && angle < -90)
        {
            angle = angle + 90;
        }
        else if (angle < 0 && angle > -90)
        {
            angle = angle - 90;
        }

        return angle;
    }

    public void playCharacterVoice()
    {

        audioSources[2].Stop();

        int randomVoiceClip = Random.Range(0, 7);

        switch (randomVoiceClip)
        {
            case 0:
                audioSources[2].clip = characterVoice[0];
            break;
            case 1:
                audioSources[2].clip = characterVoice[1];
            break;
            case 2:
                audioSources[2].clip = characterVoice[2];
            break;
            case 3:
                audioSources[2].clip = characterVoice[3];
            break;
            case 4:
                audioSources[2].clip = characterVoice[4];
            break;
            case 5:
                audioSources[2].clip = characterVoice[5];
            break;
            case 6:
                audioSources[2].clip = characterVoice[6];
            break;
        }

        audioSources[2].Play();
    }
   

}