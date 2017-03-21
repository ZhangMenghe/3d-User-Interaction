using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // required when using UI elements in scripts
using UnityEngine.EventSystems;// Required when using Event data.
using UnityEngine;

public class uiController : MonoBehaviour {
    public Transform panel;
    public Transform measureMsgPanel;
    public ItemMovement itemMovementScript;
    public SaveAndLoad saveLoadScript;
    public flying flyingScript;
    public Transform pointerL;
    public Transform pointerR;
    public OVRInput.Controller controller;//LEFT HAND
    private Transform[] buttons;
    private int indexButton;
    private bool[] buttonHighLight;
    private bool lastfiveExist;
    public bool uiActive;
    // Use this for initialization
    void Start () {
        GameObject Controller = GameObject.Find("Controller");
        itemMovementScript = Controller.GetComponent<ItemMovement>();
        saveLoadScript = Controller.GetComponent<SaveAndLoad>();
        flyingScript = Controller.GetComponent<flying>();
        //panel = GameObject.Find("Panel").transform;
        lastfiveExist = false;
        indexButton = 0;
        buttonHighLight = new bool[9];
        for (int i = 0; i < 9 ; i++)
            buttonHighLight[i] = false;
        uiActive = true;
    }
    void RecoverControllerPos()
    {
        this.gameObject.transform.position = new Vector3(-2.6f, -0.4f, -1.24f);
        this.gameObject.transform.rotation = Quaternion.Euler(0,220,0);
    }

    void handleClickButton(string choose)
    {
        if(choose != "flying")
        {
            if (flyingScript.shouldFlying)
            {
                RecoverControllerPos();
                flyingScript.shouldFlying = false;
            }
                
        }
        if (itemMovementScript.isMeasureMsgOn)
        {
                itemMovementScript.isMeasureMsgOn = false;
                measureMsgPanel.gameObject.SetActive(false);
        }
        switch (choose)
        {
            case "restart":
                Debug.Log("restart here");
                itemMovementScript.shouldRestart = true;
                break;
            case "save":
                Debug.Log("save here");
                saveLoadScript.shouldSave = true;
                break;
            case "load":
                Debug.Log("Load here");
                saveLoadScript.shouldLoad = true;
                break;
            case "pointer":
                pointerL.gameObject.SetActive(!lastfiveExist);
                pointerR.gameObject.SetActive(!lastfiveExist);
                lastfiveExist = !lastfiveExist;
                break;
            case "lefthand":
                Debug.Log("two hand pointer");
                itemMovementScript.shouldDoingTwoHands = true;
                break;
            case "righthand":
                Debug.Log("right hand pointer here");
                itemMovementScript.shouldDoingTwoHands =false;
                break;
            case "flying":
                if (flyingScript.shouldFlying)
                    RecoverControllerPos();
                flyingScript.shouldFlying = !flyingScript.shouldFlying;
                break;
            case "ruler":
                itemMovementScript.shouldApplyRuler = !itemMovementScript.shouldApplyRuler;
                break;
            default:
                Debug.Log("right hand pointer here");
                itemMovementScript.shouldDoingTwoHands = false;
                break;
        }

    }
    bool checkNoHighlight()
    {
        for (int i = 0; i < 9; i++)
            if (buttonHighLight[i] == true)
                return false;
        return true;
    }
    // Update is called once per frame
    void Update () {
        if(OVRInput.Get(OVRInput.Button.Start, controller))
        {
            if (uiActive)
            {
                uiActive = false;
                panel.gameObject.SetActive(false);
            }
            else
            {
                uiActive = true;
                panel.gameObject.SetActive(true);
            }
        }
        if (uiActive)
        {
            //panel = GameObject.Find("Panel").transform;
            //OVRInput.Get(OVRInput.Button.Two, controllerLeft)
            buttons = panel.gameObject.GetComponentsInChildren<Transform>();
            //if (Input.GetKey(KeyCode.Space))
            //buttons[2].gameObject.GetComponent<Button>().Select();
            //buttons[2].gameObject.GetComponent<Image>().color = new Color(0.93f, 0.53f, 0.53f);
            //Debug.Log(buttons[2].gameObject.name);
            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller))//if (OVRInput.Get(OVRInput.Button.One, controller))
            {
                indexButton = (indexButton + 1) % buttons.Length;
                if (indexButton == 0)
                    indexButton++;
                for (int i = 0; i < 9; i++)
                {
                    if (buttonHighLight[i] == true)
                    {
                        buttonHighLight[i] = false;
                        buttons[i].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                    }
                }
                //buttons[indexButton].gameObject.GetComponent<Button>().Select();
                buttons[indexButton].gameObject.GetComponent<Image>().color = new Color(0.93f, 0.53f, 0.53f);
                buttonHighLight[indexButton] = true;
            }
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller))
            {
                for (int i = 1; i < buttons.Length; i++)
                {
                    if (buttonHighLight[i] == true)
                    {
                        buttonHighLight[i] = false;
                        buttons[i].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                        handleClickButton(buttons[i].gameObject.name);
                    }
                }
            }
        }

	}

}
