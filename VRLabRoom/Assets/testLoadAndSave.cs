using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLoadAndSave : MonoBehaviour {
    public Transform deskPrefab;
	// Use this for initialization
	void Start () {
        //Transform obj = GameObject.Find("deskS").transform;
        //Debug.Log(obj.localPosition);
        int i = 0;
        var deskP = Instantiate(deskPrefab, Vector3.zero, Quaternion.Euler(-90, 0, 180));
        deskP.gameObject.name = "DeskGroup" + i.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
