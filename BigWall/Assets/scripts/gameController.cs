using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gameController : MonoBehaviour
{
    public Vector3 originalPosition;
    public float radius;
    public int wallHeight;
    public Transform brick;
    public Transform ball;
    public int TimeToDispear;//Time to throw a ball or cast laser
    public int TimeToMove;//Time to move person
    public int Selection;
    public Button mybutton;
    public progressCircle progressScript;
    public bool enableMove = false;

    private Vector3 lastHitPos = new Vector3(0, 0, 0);
    private int countTime = 0;
    private int planCountTime = 0;
    // Use this for initialization
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
    }

    void LaserOrBallAction(RaycastHit rayHit)
    {
        if (Selection == 1)//laser
            Destroy(rayHit.collider.gameObject);
        if (Selection == -1)
        {//ball
            Instantiate(ball, Camera.main.transform.position, Quaternion.identity);
        }

    }
    // Update is called once per frame
    void Update()
    {
        Ray myRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit rayHit;
        if (Selection != 0)
        {//act laser or ball

            if (Physics.Raycast(myRay, out rayHit, Mathf.Infinity))
            {
                //Debug.LogFormat("you hit {0}", rayHit.collider.name);
                if (rayHit.point == lastHitPos && rayHit.transform.gameObject.name != "Plane")
                {
                    countTime++;
                    if (countTime >= TimeToDispear)
                    {
                        //Destroy(rayHit.collider.gameObject);
                        LaserOrBallAction(rayHit);
                        countTime = 0;
                    }
                    //Debug.Log(countTime);
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


                }
                lastHitPos = rayHit.point;

            }
        }//end selection!=0
         //TEL-trans camera
        if (enableMove && Physics.Raycast(myRay, out rayHit, Mathf.Infinity))
        {
            //Debug.LogFormat("you hit {0}", rayHit.collider.name);
            if (rayHit.point == lastHitPos && rayHit.transform.gameObject.name == "Plane")
            {
                planCountTime++;
                if (planCountTime >= TimeToMove)
                {
                    Vector3 currentPos = Camera.main.gameObject.transform.position;
                    if((currentPos.x - lastHitPos.x)* (currentPos.x - lastHitPos.x) + (currentPos.z - lastHitPos.z) * (currentPos.z - lastHitPos.z) < radius * radius-1)
                        Camera.main.gameObject.transform.position = new Vector3(lastHitPos.x, 2, lastHitPos.z);
                    //else

                    countTime = 0;
                }
            }
            lastHitPos = rayHit.point;
        }
    }//end update
}
