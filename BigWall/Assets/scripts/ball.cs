using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    public float volocity;
    private Rigidbody rb;
    private Transform basic;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
        basic = Camera.main.transform;
        //rb.AddForce(Physics.gravity * rb.mass);
        //transform.position = basic.position + basic.forward * volocity;
        //rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(basic.forward * 100);
	}
}
