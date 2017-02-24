using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flightMove : MonoBehaviour {
    public onEenterCheckPoint enterCheckpointScript;
    public bool countingDown;
    public float startTime;
    public int delayTime;
    public float flyingSpeed;
    public bool gameStarted;
    public AudioClip countAudio;
    
    private GameObject countText;
    private GameObject player;
    private GameObject timer;
    private Rigidbody playerRb;
    private Vector3 direction;
    private AudioSource audioSound;
    public bool bgmStarted;
    // Use this for initialization
    void Start () {
        flyingSpeed = 20.0f;
        countingDown = true;
		startTime = Time.time+delayTime;
        countText = GameObject.Find("countdownText");
        player = GameObject.Find("airPlane");
        timer = GameObject.Find("Timer");
        enterCheckpointScript = player.GetComponent<onEenterCheckPoint>();
        playerRb = player.GetComponent<Rigidbody>();
        gameStarted = false;
        audioSound = gameObject.GetComponent<AudioSource>();
        audioSound.PlayOneShot(countAudio);
        //countAudio.Play();
        bgmStarted = false;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > 5.0f)
        {
            gameStarted = true;
            if (Time.time < 6.0f)
            {
                countText.GetComponent<TextMesh>().text = "GO!!!";
                bgmStarted = true;                    
                return;
            }
        }
        else
        {
            if(Time.time >= 1.0f)
            {
                countText.GetComponent<TextMesh>().text = "Starting in " + (6.0f - Time.time).ToString() + "...";
                return;
            }

        }

        //player.transform.Translate(getDirection() * flyingSpeed);
        if (startTime - Time.time >= 0)
        {
            countText.GetComponent<TextMesh>().text = (startTime - Time.time - 5.0f).ToString();
            timer.GetComponent<TextMesh>().text = "";
        }
        else
        {
            
            if (!enterCheckpointScript.startCD)
            {
                countText.GetComponent<TextMesh>().text = "";
                countingDown = false;
            }
            if (!enterCheckpointScript.finished && Time.time>6.0f)
                timer.GetComponent<TextMesh>().text = (Time.time - startTime - 6.0f).ToString() + "s";

        }

    }
}
