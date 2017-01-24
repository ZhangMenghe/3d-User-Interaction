using UnityEngine;
using System.Collections;

public class sceneController : MonoBehaviour {
    public float restartAngle = 0.9f;
    public int restartTime = 10;
    private int timer = 10;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Camera.main.transform.forward.y > restartAngle)
        {
            timer++;
            if (timer > restartTime)
                Application.LoadLevel("main");
        }
	}
}
