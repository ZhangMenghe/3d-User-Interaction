using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leapMove : MonoBehaviour {
    public float climbSpeed;
    private Transform playerT;
    private Vector3 dir;
    // Use this for initialization
    void Start () {
        //climbSpeed = 10.0f;
        playerT = GameObject.Find("airPlane").transform;
        dir = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        //playerT.position += playerT.forward * climbSpeed * Time.deltaTime;
        playerT.position += dir * climbSpeed * Time.deltaTime;
        if(dir!=Vector3.zero)
            Debug.Log(dir);

    }

    public void FlightUp()
    {
        //playerT.position += playerT.up * climbSpeed * Time.deltaTime; 
        dir += playerT.up;
        Debug.Log("on active up");
    }

    public void FlightStopMoveUntilNextGesture()
    {
        dir = Vector3.zero;
    }

    public void FlightUp_d()
    {
        //playerT.position += playerT.up * climbSpeed * Time.deltaTime;
        dir = Vector3.zero;
        Debug.Log("on detect up");
    }

    public void FlightDown()
    {
        //playerT.position += playerT.up * climbSpeed * Time.deltaTime; 
        if (playerT.position.y < 5.0f)
            dir = Vector3.zero;
        else
            dir += -playerT.up;
        Debug.Log("on active down");
    }
    public void FlightDown_d()
    {
        //playerT.position += playerT.up * climbSpeed * Time.deltaTime;
        dir = Vector3.zero;
        Debug.Log("on detect down");
    }

    public void FlyForward()
    {
        //playerT.position += playerT.forward * climbSpeed * Time.deltaTime;
        dir += playerT.forward;
        Debug.Log("on active forward");
    }

    public void FlyForward_d()
    {
        //dir = Vector3.zero;
        //dir = playerT.forward;
        dir -= playerT.forward;
        Debug.Log("on detect forward");
    }

    public void FlyRight()
    {
        dir += playerT.right;
        Debug.Log("on active FlyRight");
    }

    public void FlyRight_d()
    {
        //dir = playerT.right;
        dir = Vector3.zero;
        Debug.Log("on detect FlyRight");
    }

    public void FlyLeft()
    {
        dir += -playerT.right;
        Debug.Log("on active FlyLeft");
    }

    public void FlyLeft_d()
    {
        //dir = playerT.right;
        dir = Vector3.zero;
        Debug.Log("on detect FlyLeft");
    }
}
