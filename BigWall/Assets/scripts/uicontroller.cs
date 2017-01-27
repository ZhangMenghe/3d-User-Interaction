using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class uicontroller : MonoBehaviour {
    public Transform startMessage;
    public Transform selectMsgLaser;
    public Transform selectMsgBall;
    public Transform selectMsgNo;
    public Transform myParent;
    public Vector3 parentPosition;
    public gameController gameControllerScript;
    public int showMsgSwitch;//what message to show
    public Transform lastActiveObj;
    public int TimeToRemoveMsg;
    private bool showSelMsg = false;
    private bool existStartMsg = true;
    private RectTransform text;
    private RectTransform textL;
    private RectTransform textB;
    private RectTransform textN;
    private int lastSelection;
    private int keepTimer = 0;
    // Use this for initialization
    void Start()
    {
        GameObject Controller = GameObject.Find("Controller");
        gameControllerScript = Controller.GetComponent<gameController>();
        parentPosition = new Vector3(0, 0, 0);
        //Debug.Log(gameControllerScript.Selection);
        showStartMessage();
        //createSelectionTexts();
        lastSelection = -1;
    }
    public void showStartMessage()
    {
        startMessage.gameObject.SetActive(true);
    }
    public void createSelectionTexts() {

    }
    void Update () {
        if (gameControllerScript.hasChangeSelection)
            startMessage.gameObject.SetActive(false);

        int curSel = gameControllerScript.Selection;
        if(curSel == lastSelection)
        {
            keepTimer++;
            if(keepTimer >= TimeToRemoveMsg)
            {
                lastActiveObj.gameObject.SetActive(false);
                keepTimer = 0;
            }
        }
        else if (gameControllerScript.hasChangeSelection && curSel != lastSelection)//choose change
        {
            switch (curSel)
            {
                case 0:
                    selectMsgNo.gameObject.SetActive(true);
                    lastActiveObj = selectMsgNo;
                    break;
                case 1:
                    selectMsgLaser.gameObject.SetActive(true);
                    lastActiveObj = selectMsgLaser;
                    break;
                case 2:
                    selectMsgBall.gameObject.SetActive(true);
                    lastActiveObj = selectMsgBall;
                    break;
            }
            lastSelection = curSel;
        }



    }

}
