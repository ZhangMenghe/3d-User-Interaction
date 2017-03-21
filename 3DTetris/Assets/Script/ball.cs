using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    public float volocity;
    private Rigidbody rb;
    private Transform basic;
	// Use this for initialization
	void Start () {
        basic = GameObject.Find("FakeCamera").transform;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(basic.forward * 1000);
    }

	// Update is called once per frame
	void Update () {
        Time.timeScale = 1;////?????????where did I change it????
        //rb.AddForce(Physics.gravity * rb.mass);
        //transform.position = basic.position + basic.forward * volocity;
        //rb.velocity = Vector3.zero;
       // rb.angularVelocity = Vector3.zero;
       // rb.AddForce(basic.forward * 1000);
	}
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(transform.gameObject);
    }
}
