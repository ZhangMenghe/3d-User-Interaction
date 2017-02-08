using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    private float speed;
    private Vector3 dir;
    void Start () {
        speed = 0.3f;
        dir = new Vector3(1.0f, .0f, .0f);

        transform.localPosition = new Vector3(.0f, 0.2f, -6f);
        transform.rotation = Quaternion.identity;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.localPosition = new Vector3(.0f, 0.2f, -6f);
            transform.rotation = Quaternion.identity;
        }
            
	}

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Camera.main.transform.forward * speed); //dir = Camera.main.transform.forward;
        if(Input.GetKey(KeyCode.DownArrow))
            transform.Translate(-Camera.main.transform.forward * speed); //dir = -Camera.main.transform.forward;
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(-Camera.main.transform.right * speed);//dir = -Camera.main.transform.right;
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Camera.main.transform.right * speed);//dir = Camera.main.transform.right;
    }
}
