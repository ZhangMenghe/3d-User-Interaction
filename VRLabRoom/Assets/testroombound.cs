using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testroombound : MonoBehaviour {
    private Vector3 selectPos;
    public Transform room;
    public Camera cam;
	// Use this for initialization
	void Start () {
		
	}
    void IsSelectingPointInsideRoom()
    {
        //Debug.Log(room.gameObject.GetComponent<Collider>().bounds);
        Bounds roomBound = room.gameObject.GetComponent<Collider>().bounds;
        selectPos.x = Mathf.Min(selectPos.x, roomBound.center.x + roomBound.extents.x);
        selectPos.x = Mathf.Max(selectPos.x, roomBound.center.x - roomBound.extents.x);
        selectPos.z = Mathf.Min(selectPos.z, roomBound.center.z + roomBound.extents.z);
        selectPos.z = Mathf.Max(selectPos.z, roomBound.center.z - roomBound.extents.z);
    }

    // Update is called once per frame
    void Update () {
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                selectPos = hit.point;
                if (IsSelectingPointInsideRoom())
                    Debug.Log("Inside");
            }
        }*/
        Transform cube = GameObject.Find("Cube").transform;
        selectPos = cube.position;
        IsSelectingPointInsideRoom();
        cube.position = new Vector3(selectPos.x, cube.position.y, selectPos.z);

    }
}
