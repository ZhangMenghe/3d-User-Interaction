using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmPlay : MonoBehaviour {
    private AudioSource audioSound;
    public AudioClip bgm;
    private flightMove flightMoveScript;
    private bool started;
    // Use this for initialization
    void Start () {
        audioSound = gameObject.GetComponent<AudioSource>();
        GameObject player = GameObject.Find("airPlane");
        flightMoveScript = player.GetComponent<flightMove>();
        started = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (flightMoveScript.bgmStarted && started == false)
        {
            audioSound.Play();
            started = true;
        }
            
	}
}
