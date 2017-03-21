using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMaterial : MonoBehaviour {
    public bool isActive;
    public bool alreadyExist;
    public Transform handModel;
    private GameObject Instmodel;
    // Use this for initialization
    void Start () {
        isActive = false;
        alreadyExist = false;
    }

	// Update is called once per frame
	void Update () {
        if (!isActive || alreadyExist)
            return;
        //Instmodel = Instantiate(transform.gameObject, transform.position+new Vector3(0.8f, .0f, .0f), Quaternion.identity);
        //Instmodel.transform.localScale = new Vector3(0.005f, 1.0f, 1.0f);
        //Instmodel.transform.parent = handModel;
        GameObject baseMat = transform.gameObject;
        GameObject cubeNew = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Transform parentTrans = GameObject.Find("DIYobjInteration").transform;
        cubeNew.GetComponent<Renderer>().material = baseMat.GetComponent<Renderer>().material;
        cubeNew.transform.position = parentTrans.position + new Vector3(1.0f, 1.0f, 1.0f);//spawner location
        cubeNew.transform.parent = parentTrans;
        cubeNew.name = "modelCube";
        isActive = false;
        alreadyExist = true;
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.name == "modelCube")
        {
            alreadyExist = false;
            obj.GetComponent<Renderer>().material = transform.gameObject.GetComponent<Renderer>().material;
            Destroy(Instmodel);
        }

    }*/
}
