using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flying : MonoBehaviour {
    public float flyingSpeed;
    public float rotationSpeed;
    public bool shouldFlying;
    // Use this for initialization
    void Start () {
        shouldFlying = false;
    }
	Vector3 checkFlyingDirection()
    {
        Vector3 dir = Vector3.zero;
        Vector2 scondAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //if (Input.GetKey(KeyCode.UpArrow))
        if (scondAxis.y > 0)
        {
            dir += Vector3.up;//return flying direction:up, foward, back, right, left
            if (scondAxis.y > 0.8f)
                return dir;

        }
            
        if (scondAxis.y < 0)
        {
            if (this.gameObject.transform.position.y > -1.0f)
                dir+= Vector3.down;
            if (scondAxis.y < -0.8f)
                return dir;
        }
            
        if (scondAxis.x < 0)
            dir= Vector3.left;
        if (scondAxis.x > 0)
            dir= Vector3.right;
        return dir;
    }
    Vector3 checkFlyerRotation()
    {
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (primaryAxis.y > 0)
            return Vector3.up;//return flying direction:up, foward, back, right, left
        if (primaryAxis.y < 0)            
                return Vector3.down;
        if (primaryAxis.x > 0)
            return Vector3.left;
        if (primaryAxis.x < 0)
            return Vector3.right;
        return Vector3.zero;
    }
	// Update is called once per frame
	void Update () {
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        //Debug.Log(primaryAxis);
        if (shouldFlying)
        {
            this.gameObject.transform.Translate(checkFlyingDirection() * flyingSpeed);
            this.gameObject.transform.Rotate(checkFlyerRotation() * rotationSpeed);
        }

	}
}
