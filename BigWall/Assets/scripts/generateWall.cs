using UnityEngine;
using System.Collections;

public class generateWall : MonoBehaviour {
    public Vector3 originPos;
    public float radius;
    public int wallHeight;
    public int TimeToDispear;
    public int TimeToThrowABall;
    public Transform brick0;
    public Transform brick1;
    public Transform brick2;
    public Transform ball;
    public Transform cam;
    private Vector3 lookPos;
    private Vector3 lastHitPos = new Vector3(0,0,0);
    private float lastAngle = .0f;
    private int countTime = 0;
    private int ballTime = 0;
    // Use this for initialization
    void Start () {
        Transform[] bricks = new Transform[3];
        bricks[0] = brick0;
        bricks[1] = brick1;
        bricks[2] = brick2;
        float wallLength = 2 * Mathf.PI * radius;
        float brickWidth = bricks[0].GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        int brickNum = (int)Mathf.Floor(wallLength / brickWidth);
        float height = .0f;
        float plus = .0f;
        for(int h=0; h< wallHeight; h++)
        {
            for (float theta = 0; theta < 2 * Mathf.PI; theta += brickWidth / ( 2* radius))
            {
                //int index = (int)Mathf.Floor(Random.value * 3);
                //Instantiate(bricks[h%3], new Vector3(radius * Mathf.Cos(theta), height, radius * Mathf.Sin(theta)) + originPos, new Quaternion(0, -theta * 90 / Mathf.PI, 0, 0));
                Instantiate(bricks[(h / 3)%3], new Vector3(radius * Mathf.Cos(theta + plus), height, radius * Mathf.Sin(theta +plus)) + originPos, Quaternion.identity);
            }
            height += brickWidth/2;
            plus += brickWidth / (4 * radius);
        }
        lookPos = cam.GetComponent<Transform>().localPosition;
            
	}

    // Update is called once per frame
    void Update () {
        Ray myRay = new Ray(lookPos, Camera.main.transform.forward);
        RaycastHit rayHit;
        if(Physics.Raycast(myRay, out rayHit, Mathf.Infinity))
        {
            Debug.LogFormat("you hit {0}", rayHit.collider.name);
            //rayHit.collider.bounds;?
            if (rayHit.point == lastHitPos && rayHit.collider.name != "Plane")
            {
                countTime++;
                if (countTime >= TimeToDispear)
                {
                    Destroy(rayHit.collider.gameObject);
                    countTime = 0;
                }

            }
            
            else if (Mathf.Abs(lastAngle - Camera.main.transform.forward.y) > 0.01)// && Mathf.Abs(rayHit.point.x - lastHitPos.x) < 0.5)
            {
                ballTime++;
                if (ballTime >= TimeToThrowABall)
                {
                    Instantiate(ball, Camera.main.transform.position, Quaternion.identity);
                    ballTime = 0;
                }
            }
            //Instantiate(ball, Camera.main.transform.position, Quaternion.identity);
            lastHitPos = rayHit.point;
            lastAngle = Camera.main.transform.forward.y;
            Debug.Log(lastAngle);
        }
    }
}
