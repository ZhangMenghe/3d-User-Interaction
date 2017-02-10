using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ItemMovement : MonoBehaviour {
    public OVRInput.Controller controller;
    public OVRInput.Controller controllerLeft;
    public Vector3 selectPos;
    public bool isSelectInsideRoom;
    public Transform room;
    public Transform playerEye;
    public Transform playerHand;
    public Transform playerLeftHand;
    //private GameObject[] moveList;
    public ArrayList moveList;
    public int movementTimeSheldhold;
    public int itemMoveTimeThreshold;
    private bool isSelectHitObj;
    public LineRenderer laserLineRender;
    public float constrainRayDistance;
    public int restartTime;
    public bool shouldDoingTwoHands;
    public bool shouldRestart;
    public bool shouldApplyRuler;
    public Transform measurePanel;
    public bool isMeasureMsgOn;
    private float laserWidth;
    private float laserMaxLength;
    private Vector3 rayCastrot;
    private Vector3 rayCastpos;
    private int movementTimeCount;
    private int itemMoveTimeCount;
    private GameObject rotateObj;
    private bool isUsingLeftHand;
    private int Reloadtimer;
    private Vector3 paintRight;
    private Vector3 m_startPos;
    private Vector3 m_endPos;
    private int RulercountTime;
    // Use this for initialization
    void Start () {
        m_startPos = new Vector3(.0f, -100.0f, .0f);
        m_endPos = new Vector3(.0f, -100.0f, .0f);
        selectPos = new Vector3(.0f, .0f, .0f);
        moveList = new ArrayList();
        isSelectInsideRoom = true;
        isSelectHitObj = false;
        laserWidth = 0.01f;
        laserMaxLength = 5f;
        laserLineRender = GetComponent<LineRenderer>();
        laserLineRender.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        laserLineRender.startWidth = laserWidth;
        laserLineRender.endWidth = laserWidth;
        laserLineRender.startColor = new Color(.0f, 1.0f, .0f);
        laserLineRender.endColor = new Color(.0f, 1.0f, .0f);
        movementTimeCount = 0;
        itemMoveTimeCount = 0;
        Reloadtimer = 0;
        RulercountTime = 0;
        isUsingLeftHand = false;
        paintRight = new Vector3(1.0f,.0f,-0.1f) ;
        shouldDoingTwoHands = false;
        shouldRestart = false;
        shouldApplyRuler = false;
        isMeasureMsgOn = false;

    }
    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserLineRender.SetPosition(0, targetPosition);
        laserLineRender.SetPosition(1, endPosition);
    }
    bool objAlreadyInMoveList(GameObject checkObj)
    {
        foreach (GameObject obj in moveList)
        {
            if (obj.name == checkObj.name && obj.GetComponent<BoxCollider>() == checkObj.GetComponent<BoxCollider>())
                return true;
        }
        return false;
    }
    bool isDoingSelectionTwoHands()
    {
        if (shouldDoingTwoHands && OVRInput.Get(OVRInput.Button.Two, controllerLeft) &&  (OVRInput.Get(OVRInput.Button.Two, controller)))
        {
            isUsingLeftHand = true;
            rayCastpos = new Vector3(playerLeftHand.position.x, playerLeftHand.position.y - 0.1f, playerLeftHand.position.z);
            rayCastrot = playerHand.position -  playerLeftHand.position;
            Ray ray = new Ray(rayCastpos, rayCastrot);
            RaycastHit hit;
            ShootLaserFromTargetPosition(rayCastpos, rayCastrot, laserMaxLength);
            laserLineRender.enabled = true;
            if (Physics.Raycast(ray, out hit) && hit.distance > constrainRayDistance)
            {
                Debug.Log(hit.distance);
                selectPos = hit.point;
                //Debug.Log("Hit " + hit.transform.gameObject.name);
                if (hit.transform.gameObject.name == "Plane" || hit.transform.gameObject.name == "RoomMesh")
                {
                    isSelectHitObj = false;
                    return true;//selecting something but don't move it
                }
                hit.transform.gameObject.GetComponent<BoundBoxes_BoundBox>().enabled = true;

                if (!objAlreadyInMoveList(hit.transform.gameObject))
                {
                    //Add to moveList
                    moveList.Add(hit.transform.gameObject);
                    //避免多次加入
                    isSelectHitObj = true;
                    return true;
                }
                else
                {
                    isSelectHitObj = false;//already in list
                    return false;
                }


            }
            else
            {

                isSelectHitObj = false;//assume all hit inside room
                return false;
            }
        }
        return false;
    }
    bool IsDoingSelection(){
        if (isUsingLeftHand)
            return false;
        //if (Input.GetMouseButtonDown(0))//TODO:now use mouse, change another way
        if (OVRInput.Get(OVRInput.Button.Two,controller))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            rayCastpos = new Vector3(playerHand.position.x , playerHand.position.y-0.1f, playerHand.position.z);
            rayCastrot = playerHand.forward;
            //Debug.Log(rayCastrot);
            Ray ray = new Ray(rayCastpos, rayCastrot);
            //Debug.Log(InputTracking.GetLocalPosition(mVRNode));
            RaycastHit hit;
            //draw the rayline
            ShootLaserFromTargetPosition(rayCastpos, rayCastrot, laserMaxLength);
            laserLineRender.enabled = true;
            //check if hit something
            if (Physics.Raycast(ray, out hit))
            {
                
                selectPos = hit.point;
                //Debug.Log("Hit " + hit.transform.gameObject.name);
                if (hit.transform.gameObject.name == "Plane" || hit.transform.gameObject.name == "RoomMesh")
                {
                    isSelectHitObj = false;
                    return true;//selecting something but don't move it
                }
                

                if (!objAlreadyInMoveList(hit.transform.gameObject))
                {
                    hit.transform.gameObject.GetComponent<BoundBoxes_BoundBox>().enabled = true;
                    //Add to moveList
                    moveList.Add(hit.transform.gameObject);
                    isSelectHitObj = true;
                    return true;
                }
                else
                {
                    isSelectHitObj = false;//already in list
                    return false;
                }
                    

            }
            else
            {
                
                isSelectHitObj = false;//assume all hit inside room
                return false;
            }
               
        }//end getmousebuttondown
        //didn't press button , don't render line
        //laserLineRender.enabled = false;
        return false;
    }

    void BoundedSelectPointByRoom()
    {
        //Debug.Log(room.gameObject.GetComponent<Collider>().bounds);
        Bounds roomBound = room.gameObject.GetComponent<Collider>().bounds;
        /*bool cxl = selectPos.x <= roomBound.center.x + roomBound.extents.x;
        bool cxr = selectPos.x >= roomBound.center.x - roomBound.extents.x;
        //bool cy = (selectPos.y <= roomBound.center.y + roomBound.extents.y) && (selectPos.y >= roomBound.center.y - roomBound.extents.y);
        bool czf = selectPos.z <= roomBound.center.z + roomBound.extents.z;
        bool czb = selectPos.z >= roomBound.center.z - roomBound.extents.z;
        //Debug.Log(roomBound.center+ roomBound.extents);
        //Debug.Log(roomBound.center - roomBound.extents);*/

        selectPos.x = Mathf.Min(selectPos.x, roomBound.center.x + roomBound.extents.x);
        selectPos.x = Mathf.Max(selectPos.x, roomBound.center.x - roomBound.extents.x);
        selectPos.z = Mathf.Min(selectPos.z, roomBound.center.z + roomBound.extents.z);
        selectPos.z = Mathf.Max(selectPos.z, roomBound.center.z - roomBound.extents.z);
    }

    bool IsDoingMovement()
    {
        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger)){
            rayCastpos = new Vector3(playerHand.position.x, playerHand.position.y - 0.1f, playerHand.position.z);
            rayCastrot = playerHand.forward;
            Ray ray = new Ray(rayCastpos, rayCastrot);
            RaycastHit hit;
            //draw the rayline
            ShootLaserFromTargetPosition(rayCastpos, rayCastrot, laserMaxLength);
            laserLineRender.enabled = true;
            //check if hit something
            if (Physics.Raycast(ray, out hit))
            {
                selectPos = hit.point;
                if (hit.transform.gameObject.name == "Plane")
                    return true;
                else
                    return false;
            }
        }
        //laserLineRender.enabled = false;

        return false;
    }
    bool IsDoingRotation()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            rayCastpos = new Vector3(playerHand.position.x, playerHand.position.y - 0.1f, playerHand.position.z);
            rayCastrot = playerHand.forward;
            Ray ray = new Ray(rayCastpos, rayCastrot);
            RaycastHit hit;
            //draw the rayline
            ShootLaserFromTargetPosition(rayCastpos, rayCastrot, laserMaxLength);
            laserLineRender.enabled = true;
            //check if hit something
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.name == "Plane")
                    return false;
                selectPos = hit.point;
                rotateObj = hit.transform.gameObject;
                return true;
            }
            return false;
        }
        else
            return false;
    }
    void checkReloadTimer()
    {
        /*if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick) && OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            Reloadtimer++;
            if (Reloadtimer > restartTime)
                SceneManager.LoadScene("main", LoadSceneMode.Single);
        }*/
        if (shouldRestart)
        {
            shouldRestart = false;
            SceneManager.LoadScene("main", LoadSceneMode.Single);
        }
    }
    void DoingMeasurement()
    {
       if (OVRInput.Get(OVRInput.Button.Two, controller))
        {
            rayCastpos = new Vector3(playerHand.position.x, playerHand.position.y - 0.1f, playerHand.position.z);
            rayCastrot = playerHand.forward;
            Ray ray = new Ray(rayCastpos, rayCastrot);
            RaycastHit hit;
            //draw the rayline
            ShootLaserFromTargetPosition(rayCastpos, rayCastrot, laserMaxLength);
            laserLineRender.enabled = true;
            //check if hit something
            if (Physics.Raycast(ray, out hit))
            {
                if (m_startPos.y == -100.0f)
                {
                    m_startPos = hit.transform.position;

                }
                else if (m_endPos.y == -100.0f && hit.transform.position!= m_startPos)
                {
                    m_endPos = hit.transform.position;
                    RulercountTime = 0;
                }
                if(m_endPos.y != -100.0f && m_startPos.y != -100.0f)
                {
                    float dist = Vector3.Distance(m_startPos, m_endPos);
                    Transform[] texts;
                    measurePanel.gameObject.SetActive(true);
                    isMeasureMsgOn = true;
                    texts = measurePanel.gameObject.GetComponentsInChildren<Transform>();
                    foreach (Transform text in texts)
                    {
                        if (text.gameObject.name == "m_n1")
                            text.gameObject.GetComponent<TextMesh>().text = m_startPos.ToString();
                        if (text.gameObject.name == "m_n2")
                            text.gameObject.GetComponent<TextMesh>().text = m_endPos.ToString();
                        if (text.gameObject.name == "m_n3")
                            text.gameObject.GetComponent<TextMesh>().text = dist.ToString();
                    }

                }
            }
        }
            //return shouldApplyRuler;
    }
    // Update is called once per frame
    void Update() {
        checkReloadTimer();
        if (IsDoingMovement())
        {
            if (movementTimeCount++ > movementTimeSheldhold)
            {
                BoundedSelectPointByRoom();
                transform.position = new Vector3(selectPos.x, transform.position.y, selectPos.z);
                movementTimeCount = 0;
            }

        }
        else if (IsDoingRotation())
        {
            Debug.Log(rotateObj.name);
            if (rotateObj.name == "Lock" || rotateObj.name == "WhiteBoard")
                rotateObj.transform.Rotate(Vector3.up);
            else
                rotateObj.transform.Rotate(Vector3.forward);

        }
        else if (shouldApplyRuler)
        {
            if (m_startPos.y == -100.0f || m_endPos.y == -100.0f)
                DoingMeasurement();
            else
            {
                m_startPos = new Vector3(.0f, -100.0f, .0f);
                m_endPos = new Vector3(.0f, -100.0f, .0f);
            }
        }
        else if (isDoingSelectionTwoHands() ||IsDoingSelection())
        {
            itemMoveTimeCount++;
            if (itemMoveTimeCount > itemMoveTimeThreshold && moveList.Count!=0 && !isSelectHitObj)
            {
                itemMoveTimeCount = 0;
                GameObject firstObj = moveList[0] as GameObject;
                Vector3 basePos = firstObj.transform.position;
                foreach (GameObject obj in moveList)
                {
                    BoundedSelectPointByRoom();
                    obj.GetComponent<BoundBoxes_BoundBox>().enabled = false;
                    float moveX = obj.transform.position.x - basePos.x;
                    float moveZ = obj.transform.position.z - basePos.z;

                    if (obj.name == "WhiteBoard" || obj.name == "paintGroup")
                    {
                        if (moveList.Count != 1)
                            continue;//don't move
                        if (obj.name == "paintGroup")
                        {
                            //obj.transform.position = new Vector3(selectPos.x, obj.transform.position.y, obj.transform.position.z + (selectPos.x - obj.transform.position.x) * (-0.15f));//try not to hardcode
                            if (selectPos.x < obj.transform.position.x)
                                obj.transform.Translate(-paintRight * 0.1f);
                            else
                                obj.transform.Translate(paintRight * 0.1f);
                            //Debug.Log(obj.transform.right);
                        }
                        else
                        {
                            //obj.transform.position = new Vector3(selectPos.x, obj.transform.position.y, obj.transform.position.z + (selectPos.x - obj.transform.position.x) * (-0.15f));//try not to hardcode
                            if (selectPos.x < obj.transform.position.x)
                                obj.transform.Translate(paintRight * 0.1f);
                            else
                                obj.transform.Translate(-paintRight * 0.1f);
                            //Debug.Log(obj.transform.right);
                        }

                    }
                    else
                        //obj.transform.position = new Vector3(selectPos.x+(obj.transform.position.x-basePos.x), basePos.y, selectPos.z+(obj.transform.position.z - basePos.z));
                        obj.transform.position = new Vector3(selectPos.x +moveX, obj.transform.position.y, selectPos.z+moveZ);
                }
                moveList.Clear();//currently only one item once!!
            }//end hit a destination point:[Movement]
        }//end doingselection
        if(!OVRInput.Get(OVRInput.Button.Any))
            laserLineRender.enabled = false;
        isUsingLeftHand = false;

    }
    public void onClickStatement()
    {
        Debug.Log("show statement");
    }
}
