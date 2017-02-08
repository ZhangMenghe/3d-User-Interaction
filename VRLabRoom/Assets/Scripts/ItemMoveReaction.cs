using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoveReaction : MonoBehaviour {
    public ItemMovement ItemMovementScript;
    private BoxCollider boxCollider;
    private bool alreadyReact;
    // Use this for initialization
    void Start () {
        GameObject Controller = GameObject.Find("Controller");
        ItemMovementScript = Controller.GetComponent<ItemMovement>();
        //this.gameObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
    }

	// Update is called once per frame
	void Update () {
        //if (!alreadyReact && ItemMovementScript.isSelecting && CheckBeenSelect())
        //{
         //   ItemMovementScript.reactObj = this.gameObject;
         //   Debug.Log("add to list");
         //   ItemMovementScript.moveList.Add(this.gameObject);
         //   alreadyReact = true;
          //  ItemMovementScript.isSelecting = false;
            //foreach (Transform child in this.transform)
            //{
            //    child.gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Outlined/Silhouetted Diffuse");

            //}
       // }
            
    }
}
