using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

	private int lastDir = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		bool trigger = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
		shot s = (shot) GameObject.Find("hand_right").GetComponent(typeof(shot));

		if (s.mode == 0) {
			if (trigger) {
				Vector3 contRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles;

				Debug.Log(transform.position.x);
				if (contRot.x < 300.0f && contRot.x > 240.0f) {
					if (transform.position.y < 25.0f) {
						transform.position += Vector3.up * 0.1f;
					}
				} else if (contRot.x > 20.0f && contRot.x < 80.0f) {
					if (transform.position.y > 7.0f) {
						transform.position += Vector3.down * 0.1f;
					}
				} else if (contRot.z < 320.0f && contRot.z > 260.0f) {
					if (transform.position.x < 6.0f) {
						transform.position += Vector3.right * 0.1f;
					}
				} else if (contRot.z > 40.0f && contRot.z < 100.0f) {
					if (transform.position.x > -2.0f) {
						transform.position += Vector3.left * 0.1f;
					}
				}

			}
		} else {
			if (s.objToDrag && trigger) {
				Vector3 contRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles;
				Debug.Log(s.objToDrag.transform.position);

				if (contRot.x < 300.0f && contRot.x > 240.0f) {
					if (lastDir != 1 && s.objToDrag.transform.position.y < 3.0f) {
						s.objToDrag.transform.position += Vector3.up;
					}
					lastDir = 1;
				} else if (contRot.x > 20.0f && contRot.x < 80.0f) {
					if (lastDir != 2 && s.objToDrag.transform.position.y > 0.0f) {
						s.objToDrag.transform.position += Vector3.down;
					}
					lastDir = 2;
				} else if (contRot.z > 40.0f && contRot.z < 100.0f) {
					if (lastDir != 3 && s.objToDrag.transform.position.z > 0.0f) {
						s.objToDrag.transform.position += Vector3.back;
					}
					lastDir = 3;
				} else if (contRot.z < 320.0f && contRot.z > 260.0f) {
					if (lastDir != 4 && s.objToDrag.transform.position.z < 3.0f) {
						s.objToDrag.transform.position += Vector3.forward;
					}
					lastDir = 4;
				} else {
					lastDir = 0;
				}

			}
		}


	}
}
