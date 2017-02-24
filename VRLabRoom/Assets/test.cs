using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;

public class test : MonoBehaviour {

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(Vector3.right * 3);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.Translate(Vector3.up * 3);
        }
    }
}
