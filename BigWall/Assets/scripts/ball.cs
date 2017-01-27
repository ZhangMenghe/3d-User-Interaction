using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    public float volocity;
    private Rigidbody rb;
    private Vector3 basic;
    public gameController gameControllerScript;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        GameObject Controller = GameObject.Find("Controller");
        gameControllerScript = Controller.GetComponent<gameController>();
	}

	// Update is called once per frame
	void Update () {
        basic = gameControllerScript.headSetRotationDir;
        //rb.AddForce(Physics.gravity * rb.mass);
        //transform.position = basic.position + basic.forward * volocity;
        //rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(basic * 1000);
	}
}
