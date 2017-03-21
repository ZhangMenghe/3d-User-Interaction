using UnityEngine;
using System.Collections;

public class YAxisSpin : MonoBehaviour {

    public float spinSpeed;
	
	// Update is called once per frame
	void Update () {
        this.transform.localRotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, Vector3.up);
	}
}
