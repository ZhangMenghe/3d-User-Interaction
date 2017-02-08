using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testOutline : MonoBehaviour {
    //private BoxCollider boxCollider;
    // Use this for initialization
    void Start () {
        BoxCollider boxCollider = this.gameObject.GetComponent<BoxCollider>();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = this.transform.position + boxCollider.center;
        cube.transform.localScale = boxCollider.size;
    }
    void OnDrawGizmosSelected()
    {
        BoxCollider boxCollider = this.gameObject.GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(this.transform.position + boxCollider.center, boxCollider.size);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
