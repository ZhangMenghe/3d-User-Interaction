using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform cube2;
    // Use this for initialization
    void Start()
    {
        this.gameObject.transform.LookAt(cube2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            this.gameObject.transform.LookAt(cube2);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collider");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("madan");
    }
}
