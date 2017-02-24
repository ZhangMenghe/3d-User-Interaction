using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deskGroupItem : MonoBehaviour {
    public Transform parent;
    // Use this for initialization
    void Start () {
        string parentName = parent.gameObject.name;
        char c = parentName[parentName.Length - 1];
        char d = parentName[parentName.Length - 2];
        if(d<='9' && d >= '0')
        {
            this.gameObject.name += d.ToString();
            
        }
        this.gameObject.name += c.ToString();


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
