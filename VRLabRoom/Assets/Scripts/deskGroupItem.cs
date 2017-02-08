using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deskGroupItem : MonoBehaviour {
    public Transform parent;
    // Use this for initialization
    void Start () {
        string parentName = parent.gameObject.name;
        char c = parentName[parentName.Length - 1];
        this.gameObject.name += c.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
