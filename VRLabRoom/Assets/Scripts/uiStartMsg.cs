using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiStartMsg : MonoBehaviour {
    public Transform startMsgPanel;
    public Transform measureMsgPanel;
    public Transform uiPanel;
    public ItemMovement itemMovementScript;
    //public uiController uicontroScript;
    // Use this for initialization
    private bool startMsgOn;
    private bool measureMsgOn;
    void Start () {
       // GameObject Controller = GameObject.Find("Controller");
        //uicontroScript = Controller.GetComponent<uiController>();
        startMsgOn = true;
        GameObject Controller = GameObject.Find("Controller");
        itemMovementScript = Controller.GetComponent<ItemMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        measureMsgOn = itemMovementScript.isMeasureMsgOn;
        if (startMsgOn && OVRInput.Get(OVRInput.Button.Any))
        {
            startMsgPanel.gameObject.SetActive(false);
            //uicontroScript.uiActive = true;
            uiPanel.gameObject.SetActive(true);
            startMsgOn = false;
        }
        if(measureMsgOn && OVRInput.Get(OVRInput.Button.Any))
        {
            //measureMsgPanel.gameObject.SetActive(false);
            measureMsgOn = false;
        }
            

    }
}
