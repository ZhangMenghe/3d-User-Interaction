using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMap : MonoBehaviour {
    private Transform player;
    // Update is called once per frame
    private void Start()
    {
        player = GameObject.Find("airPlane").transform;
    }
	void Update () {
        transform.position = new Vector3(player.position.x-15.0f, player.position.y+30.0f, player.position.z);
        	
	}
}
