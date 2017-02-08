using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;

public class test : MonoBehaviour {
    public Transform parent;
	void Start () {
        string parentName = parent.gameObject.name;
        char c = parentName[parentName.Length-1];
        this.gameObject.name += c.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(Vector3.right * 3);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.Translate(Vector3.up * 0.5f);
        }
        //transform.Translate(Vector3.right * 0.1f);
    }
}
