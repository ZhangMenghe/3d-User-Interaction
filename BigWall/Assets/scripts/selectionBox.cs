using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionBox : MonoBehaviour {
    public float rotSpeed = 100;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = new Vector3(.0f, 1.0f, .0f);
        rot.y = rot.y * rotSpeed * Time.deltaTime;
        transform.Rotate(rot);
	}
}
