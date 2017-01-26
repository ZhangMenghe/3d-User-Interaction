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
    public gameController gameControllerScript;
    public int buttonDimTime = 5;

    private bool showSelMsg = false;
    private bool existStartMsg = true;
    private int timeSinceLastTouch = 0;
    private bool IsButtonDim = false;
    // Use this for initialization
    void Start()
    {
        GameObject Controller = GameObject.Find("Controller");
        gameControllerScript = Controller.GetComponent<gameController>();
        //Debug.Log(gameControllerScript.Selection);
        showStartMessage();
        showSelectionTexts();

    }
    public void showStartMessage()
    {
        Vector3 offset = new Vector3(0, -30, 0);
        RectTransform text = (RectTransform)Instantiate(startMessage, myParent.position+ offset, transform.rotation);
        //text.transform.parent = myParent.transform;
        text.SetParent(myParent);
    }
    public void showSelectionTexts() {
        Vector3 offsetL = new Vector3(-500, -100, 0);
        Vector3 offsetN = new Vector3(0, -100, 0);
        Vector3 offsetB = new Vector3(500, -100, 0);
        RectTransform textL = (RectTransform)Instantiate(selectMsgLaser, myParent.position+ offsetL, Quaternion.identity);
        RectTransform textB = (RectTransform)Instantiate(selectMsgBall, myParent.position + offsetB, Quaternion.identity);
        RectTransform textN = (RectTransform)Instantiate(selectMsgNo, myParent.position + offsetN, Quaternion.identity);
        //textL.transform.parent = myParent.transform;
        //textB.transform.parent = myParent.transform;
        //textN.transform.parent = myParent.transform;
        textL.SetParent(myParent);
        textB.SetParent(myParent);
        textN.SetParent(myParent);
        //Inactive
        textL.gameObject.SetActive(false);
        textB.gameObject.SetActive(false);
        textN.gameObject.SetActive(false);
        showSelMsg = false;
    }
    void ActiveSelectionTexts()
    {
        int count = 0;
        while(count < myParent.childCount){
          myParent.GetChild(count).gameObject.SetActive(true);
          count++;
        }
        showSelMsg = true;
    }
    void InActiveSelTexts()
    {
        int count = 0;
        while (count < myParent.childCount)
        {
            myParent.GetChild(count).gameObject.SetActive(false);
            count++;
        }
        showSelMsg = false;
        
    }
    void DimButtonColor()
    {
        Button b = myParent.gameObject.GetComponent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        b.colors = cb;
    }
    void RecoverButtonColor()
    {
        Button b = myParent.gameObject.GetComponent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = new Color(255, 255, 255, 215);
        b.colors = cb;
    }
    // Update is called once per frame
    void Update () {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.
        //pointerData.position = Camera.main.transform.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count == 0) {
            timeSinceLastTouch++;
            if(timeSinceLastTouch >= buttonDimTime) {//button become dim
                
                if (!IsButtonDim)
                {
                    DimButtonColor();
                    IsButtonDim = true;
                }
            }

        }
        if (results.Count > 0)
        {
            //WorldUI is my layer name
            if (results[0].gameObject.name == "text(Clone)") ;
            else if (results[0].gameObject.name == "Button")
            {
                if (showSelMsg == false)
                    ActiveSelectionTexts();
                //Destroy(myParent.GetChild(0).gameObject);
                //Destroy startMessage
                if (existStartMsg == true) { Destroy(myParent.FindChild("text(Clone)").gameObject); existStartMsg = false; }
                timeSinceLastTouch = 0;
                if (IsButtonDim)
                    RecoverButtonColor();
                    IsButtonDim = false;
            }
            else
            {
                if (results[0].gameObject.name == "textL(Clone)") {
                    gameControllerScript.Selection = 1;
                }
                if (results[0].gameObject.name == "textB(Clone)")
                {
                    gameControllerScript.Selection = -1;
                }
                if (results[0].gameObject.name == "textN(Clone)")
                {
                    gameControllerScript.Selection = 0;
                }
                InActiveSelTexts();
            }
            
            results.Clear();
        }
    }

}
