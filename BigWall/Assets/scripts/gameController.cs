using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.VR;

public class gameController : MonoBehaviour
{
    public Vector3 originalPosition;
    public float radius;
    public int wallHeight;
    public Transform brick;
    public Transform ball;
    public int TimeToDispear;//Time to throw a ball or cast laser
    public int TimeToMove;//Time to move person
    public int TimeToTriggerUI;
    public int Selection;
    public Button mybutton;
    public progressCircle progressScript;
    public bool enableMove = false;
    public uicontroller uicontrollerScript;
    public float restartAngle;
    public int restartTime;
    public Vector3 headSetRotationDir;
    public bool hasChangeSelection = false;

    private int timer = 0;
    private RaycastHit lastRayhit;
    private RaycastHit lastHit;

    private Vector3 lastHitPos = new Vector3(0, 0, 0);
    private Vector3 rayCastrot;
    private Vector3 rayCastpos;
    private int countTime = 0;
    private int planCountTime = 0;
    private int uiChangeTimer= 0;
    // Use this for initialization
    [SerializeField] VRNode m_VRNode    = VRNode.Head;
    void Start()
    {
        //Generate a wall
        float brickLength = brick.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        float brickHeight = brickLength / 4;
        //float brickWidth = brickLength / 2;
        float width = brickLength / 2;
        float length = radius - width / 2;
        float plusAngle = Mathf.Atan(width / length) * 2;
        float plus = .0f;
        for (int h = 0; h < wallHeight; h++)
        {
            for (float theta = 0; theta < 2 * Mathf.PI; theta = theta + plusAngle)
            {
                Instantiate(brick, new Vector3(radius * Mathf.Cos(theta + plus), h * brickHeight, radius * Mathf.Sin(theta + plus)) + originalPosition, Quaternion.Euler(0, 90 - theta * 180 / Mathf.PI, 0));
            }
            plus += plusAngle / 2;
            if (h % 3 == 0) plus = 0;
        }
        GameObject Controller = GameObject.Find("ProgressCircle");
        //Controller = GameObject.FindGameObjectsWithTag("Player")[0];
        progressScript = Controller.GetComponent<progressCircle>();
        progressScript.gameObject.SetActive(false);
        GameObject uiController = GameObject.Find("UIController");
        uicontrollerScript = uiController.GetComponent<uicontroller>();
    }

    void LaserOrBallAction(RaycastHit rayHit)
    {
        if (Selection == 1){//laser
            Destroy(rayHit.collider.gameObject);
            //rayHit.collider.gameObject.SetActive(false);
            //rayHit.collider.gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.gameObject.transform.forward * 10000000);
            //Debug.Log(headSetRotationDir);
        }


        if (Selection == 2)
        {//ball
            Vector3 pos = new Vector3(0,2,0);
            //pos = Camera.main.transform.position;
            Instantiate(ball, pos, Quaternion.identity);
        }

    }
    // Update is called once per frame
    bool isClose(Vector3 a, Vector3 b){
      bool x=  Mathf.Abs(a.x-b.x) < 0.005;
      bool y=  Mathf.Abs(a.y-b.y) < 0.005;
      bool z=  Mathf.Abs(a.z-b.z) < 0.005;
      return x&&y&&z;
    }
    void Update()
    {
      //Debug.Log(InputTracking.GetLocalRotation(m_VRNode).x);
      //Debug.Log(Camera.main.transform.forward.y > restartAngle);

      //reload scene
      if (InputTracking.GetLocalRotation(m_VRNode).x> restartAngle)
      {
          timer++;
          if (timer > restartTime)
              Application.LoadLevel("main");
      }
        //rayCastrot = InputTracking.GetLocalRotation(m_VRNode)*Vector3.forward;
        rayCastrot = Camera.main.transform.forward;
        headSetRotationDir = rayCastrot;
        //rayCastpos = InputTracking.GetLocalPosition(m_VRNode);
        //rayCastpos = new Vector3(0,1,0);
        rayCastpos = Camera.main.transform.position;
        //Debug.Log(rot);
        //Debug.Log(pos);
        Ray myRay = new Ray(rayCastpos, rayCastrot);
        RaycastHit rayHit;
        if (Physics.Raycast(myRay, out rayHit, Mathf.Infinity))//hit something
        {
            //Debug.Log(rayHit.transform.gameObject.name);
            if (isClose(rayHit.point, lastRayhit.point)){
                if(rayHit.transform.gameObject.name == "redbrick(Clone)")
                {
                    if(Selection != 0) { //shot laser or ball
                        countTime++;
                        //Debug.Log(countTime);
                        if (countTime >= TimeToDispear)
                        {
                            //Destroy(rayHit.collider.gameObject);
                            LaserOrBallAction(rayHit);
                            countTime = 0;
                        }
                        //progress bar
                        if (!progressScript.isProgressCircleRun && countTime == 1)
                        {
                            progressScript.fillValue = .0f;
                            progressScript.filterImg.fillAmount = .0f;
                            GameObject circle = progressScript.GetComponent<progressCircle>().gameObject;
                            circle.SetActive(true);
                            //Debug.Log("active!");
                            progressScript.isProgressCircleRun = true;
                        }
                    }//end selection=0
                }//end brick hit
                if(rayHit.transform.gameObject.name == "Plane" && enableMove ) {
                    planCountTime++;
                    if (planCountTime >= TimeToMove){
                        //Vector3 currentPos = Camera.main.gameObject.transform.position;
                        Vector3 currentPos = rayCastpos;
                        Vector3 lastHitPos = lastRayhit.point;
                        if ((currentPos.x - lastHitPos.x) * (currentPos.x - lastHitPos.x) + (currentPos.z - lastHitPos.z) * (currentPos.z - lastHitPos.z) < radius * radius - 1)
                            {Camera.main.gameObject.transform.position = new Vector3(lastHitPos.x, 2, lastHitPos.z);
                             Debug.Log(Camera.main.gameObject.transform.position);
                            }
                        //else

                        planCountTime = 0;
                    }//end trigger move
                }//end plane hit
                if (rayHit.transform.gameObject.name == "uiChanger") {//selection menu
                    //Debug.Log(rayHit.transform.gameObject.name);
                    uiChangeTimer++;
                    if(uiChangeTimer >= TimeToTriggerUI) {
                        Selection = (Selection + 1) % 3;//0 for nothing, 1 for laser, 2 for ball
                        //uicontrollerScript.showMsgSwitch = Selection
                        uiChangeTimer = 0;
                        hasChangeSelection = true;
                    }
                }
            }
            lastRayhit = rayHit;
        }
    }//end update


}
