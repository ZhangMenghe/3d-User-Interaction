using UnityEngine;
using System.Collections.Generic;
using Leap;
using Leap.Unity;

public class LeapBehavior : MonoBehaviour
{
    public bool isSpeedUp;
    LeapProvider provider;
    private Transform playerT;
    private Transform jetFighter;
    public flightMove flightMoveScript;
    private float speed;
    public bool lastFrameSpeedUp;
    public AudioClip speedUpAudio;
    private AudioSource audioSound;
    private Vector3 oldRot;
    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        playerT = GameObject.Find("airPlane").transform;
        jetFighter = GameObject.Find("Jet Fighter").transform;
        flightMoveScript = playerT.GetComponent<flightMove>();
        speed = .35f;
        isSpeedUp= false;
        lastFrameSpeedUp = false;
        audioSound = gameObject.GetComponent<AudioSource>();
        oldRot = jetFighter.rotation.eulerAngles;
    }
    void Update()
    {
        isSpeedUp = false;
        if (flightMoveScript.gameStarted)
        {
            Frame frame = provider.CurrentFrame;
            foreach (Hand hand in frame.Hands)
            {
                if (hand.IsRight)
                {
                    Vector3 oldPosition = playerT.position;
                    Vector3 dir = Vector3.Normalize(hand.PalmNormal.ToVector3());
                    Vector3 newPosition = oldPosition + dir * speed;
                    if (newPosition.y < 5.0f && dir.y < .0f)
                        playerT.position = new Vector3(newPosition.x, oldPosition.y, newPosition.z);
                    else
                        playerT.position = newPosition;
                    // * (playerT.localScale.y * .5f + .02f);
                    Quaternion oldRot = jetFighter.rotation;
                    LeapQuaternion leapRot = hand.Rotation;
                    Quaternion rot = new Quaternion(leapRot.x, leapRot.y, leapRot.z, leapRot.w);
                    //Vector3 newRot = rot.eulerAngles;
                    jetFighter.rotation = rot;
                    //jetFighter.Rotate(new Vector3(0.001f, .0f,.0f));
                    //Debug.Log(oldRot);
                    //Debug.Log(rot);
                    // oldRot = newRot;

                }
                if (hand.IsLeft)
                {
                    Vector3 dir = Vector3.Normalize(hand.PalmNormal.ToVector3());
                    if (dir.y > 0.8)
                    {
                        Debug.Log("++++"+speed);
                        audioSound.PlayOneShot(speedUpAudio);
                        speed += 0.001f;
                        isSpeedUp = true;
                        lastFrameSpeedUp = true;
                    }                  

                    if (dir.y < -0.8)
                    {
                        speed -= 0.01f;
                        Debug.Log("----" + speed);
                    }

                }
            }
        }   
    }
}
