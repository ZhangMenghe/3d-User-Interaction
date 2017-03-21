using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createNewModel : MonoBehaviour {
    public Transform RightHand;
    private Spawner SpawnerScript;
    // Use this for initialization
    void Start () {
        SpawnerScript = GameObject.Find("Controller").GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update () {
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
          Transform userDesignModel = GameObject.Find("DIYobj").transform;
          Transform parentTrans = GameObject.Find("DIYobjInteration").transform;
            if(parentTrans.childCount != 0){
             SpawnerScript.NextUserDesignModel = true;

             for (int i = 0; i < parentTrans.childCount; i++)
             {
                 GameObject basedOnCube = parentTrans.GetChild(i).gameObject;
                 GameObject cubeNew = GameObject.CreatePrimitive(PrimitiveType.Cube);
                 cubeNew.transform.localScale = basedOnCube.transform.localScale;
                 cubeNew.GetComponent<Renderer>().material = basedOnCube.GetComponent<Renderer>().material;
                 cubeNew.transform.localPosition = new Vector3(Mathf.Round(basedOnCube.transform.localPosition.x), Mathf.Round(basedOnCube.transform.localPosition.y), Mathf.Round(basedOnCube.transform.localPosition.z));
                 cubeNew.transform.position = basedOnCube.transform.position;
                 cubeNew.transform.parent = userDesignModel;
                 //Destroy(basedOnCube);
             }
           }

        }
    }
}
