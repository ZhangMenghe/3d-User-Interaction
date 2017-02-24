using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onEenterCheckPoint : MonoBehaviour {
    //public loadCheckPoints loadPointsScript;
    public flightMove flightMoveScript;
    public bool finished;
    public bool startCD;    
    public int delayTime;
    public bool ready;
    public AudioClip checkPointAudio;
    public AudioClip finishAudio;
    public LineRenderer lineRenderer;


    public List<Transform> checkpointTrans;
    public List<Vector3> checkpointsPos;
    public int checkNum;
    private float distance;
    private float restartTime;
    private Quaternion lastOrientation;

    private GameObject player;
    private GameObject directionArrow;
    private GameObject checkNumText;
    private GameObject distanceText;
    private GameObject countText;
    private GameObject timer;
    private AudioSource audioSound;
    // Use this for initialization
    void Start () {
        //GameObject Controller = GameObject.Find("Controller");
        //loadPointsScript = Controller.GetComponent<loadCheckPoints>();
        checkNum = 0;
        finished = false;
        startCD = false;
        restartTime = .0f;
        player = GameObject.Find("airPlane");
        flightMoveScript = player.GetComponent<flightMove>();
        directionArrow = GameObject.Find("arrow");
        checkNumText = GameObject.Find("checkNum");
        distanceText = GameObject.Find("distanceText");
        countText = GameObject.Find("countdownText");
        checkNumText.GetComponent<TextMesh>().text = "#0";
        timer = GameObject.Find("Timer");
        lastOrientation = player.transform.rotation;
        GameObject Controller = GameObject.Find("Controller");
        lineRenderer = Controller.GetComponent<LineRenderer>();
        audioSound = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!ready) return;
        
        if (!finished)
        {
            Transform lookatDir = checkpointTrans[checkNum];
            directionArrow.transform.LookAt(lookatDir);
            directionArrow.transform.Rotate(new Vector3(180, 0, 0));
            distance = (player.transform.position - lookatDir.position).magnitude;
            distanceText.GetComponent<TextMesh>().text = "Dist: " + distance.ToString();

            //if (distanceText != null)
            //    distanceText.SetActive(false);
           // if (directionArrow != null)
           //     directionArrow.SetActive(false);

            if (restartTime - Time.time >= 0)
            {
                flightMoveScript.countingDown = true;
                countText.GetComponent<TextMesh>().text = (restartTime - Time.time).ToString();
            }
            else if (startCD)
            {
                countText.GetComponent<TextMesh>().text = "";
                flightMoveScript.countingDown = false;
                startCD = false;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (finished)
        {
            return;
        }

        if (other.gameObject.name == "CheckPoint" + checkNum.ToString())
        {
            audioSound.PlayOneShot(checkPointAudio);
            checkNum++;
            checkNumText.GetComponent<TextMesh>().text = "#" + checkNum.ToString();
            other.gameObject.SetActive(false);
            lastOrientation = player.transform.rotation;
            if (checkNum >= checkpointsPos.Count-1)
            {
                audioSound.PlayOneShot(finishAudio);
                finished = true;
                checkNumText.GetComponent<TextMesh>().text = "Finished!!";
                timer.GetComponent<TextMesh>().color = new Color(.0f, 1.0f, .0f);
                checkNumText.GetComponent<TextMesh>().color = new Color(.0f, 1.0f, .0f);
                distanceText.GetComponent<TextMesh>().text = "";
                countText.GetComponent<TextMesh>().text = "";
                lineRenderer.enabled = false;
                directionArrow.SetActive(false);
            }

        }
        else//set player to last correct checkpoint
        {
            Debug.Log(other.gameObject.name);
            if (checkNum == 0)
            {
                player.transform.position = checkpointsPos[0];
                player.transform.rotation = lastOrientation;
            }
            else
            {
                player.transform.position = checkpointsPos[checkNum - 1];
                player.transform.rotation = lastOrientation;
            }

            //startCD = true;
            //flightMoveScript.countingDown = true;
            //restartTime = Time.time + delayTime;
        }


        /*if (checkNum > 0)
        {
            player.transform.position = checkpointsPos[checkNum - 1];
            player.transform.rotation = lastOrientation;
        }*/
        startCD = true;
        flightMoveScript.countingDown = true;
        restartTime = Time.time + delayTime;

    }

}
