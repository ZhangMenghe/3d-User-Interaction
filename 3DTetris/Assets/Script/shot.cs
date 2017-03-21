using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shot : MonoBehaviour {
    public static int ammo = 2;

    private LineRenderer laserLineRender;
    private float laserWidth;
    private float laserMaxLength;
    private float lastDeleteTime;
    public int mode = 0;
    private float deleteThreshold;
    public GameObject objToDrag;
    public AudioClip restartAudio;
    public AudioClip destroyAudio;
    private AudioSource audioSound;

    // Use this for initialization
    void Start () {
        deleteThreshold = 1.0f;
        laserWidth = 0.1f;
        laserMaxLength = 50f;
        laserLineRender = GameObject.Find("Controller").GetComponent<LineRenderer>();
        //laserLineRender.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        laserLineRender.startWidth = laserWidth;
        laserLineRender.endWidth = laserWidth;
        laserLineRender.startColor = new Color(1, 165/255, 0);
        laserLineRender.endColor = new Color(245/255, 222/255, 179/255);
        audioSound = GameObject.Find("Controller").GetComponent<AudioSource>();
        lastDeleteTime = Time.time;
    }

	   // Update is called once per frame
	   void Update () {
          //shot the ball
          Ray ray = new Ray(transform.position, transform.forward);
          RaycastHit rayHit;

          if (Physics.Raycast(ray, out rayHit, Mathf.Infinity)) {
              laserLineRender.enabled = true;
              laserLineRender.SetPosition(0, transform.position);
              laserLineRender.SetPosition(1, rayHit.point);

              if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) {


                GameObject hitObj = rayHit.transform.gameObject;
                string lookForName = hitObj.name;
                if (lookForName.IndexOf("color") != -1)
                {
                    hitObj.GetComponent<changeMaterial>().isActive = true;
                }
                else if (lookForName == "modelCube")
                {
                    objToDrag = hitObj;
                    //highlight objToDrag
                } else if (lookForName == "Reset") {
                    audioSound.PlayOneShot(restartAudio);
                    ResetGame();
                } else if (lookForName == "Mode") {
                  ChangeMode();
                } else if(lookForName == "resetCustom"){
                  Transform parentTrans = GameObject.Find("DIYobjInteration").transform;
                  for (int i = 0; i < parentTrans.childCount; i++)
                  {
                      Destroy(parentTrans.GetChild(i).gameObject);
                  }
                  for(int i=1;i<=6;i++){
                    GameObject obj = GameObject.Find("color"+i.ToString());
                    obj.GetComponent<changeMaterial>().alreadyExist = false;
                  }
                }else {
                  Transform parentTransform = rayHit.transform.parent;
                  if (parentTransform != null) {
                    GameObject blockGroup = parentTransform.gameObject;
                    if (ammo > 0 && blockGroup.tag == "destroyable" && blockGroup.name != "model" &&Time.time-lastDeleteTime > deleteThreshold) {
                      if (blockGroup.GetComponent<Group>().isOnGround) {
                        audioSound.PlayOneShot(destroyAudio);
                        Destroy(blockGroup);
                        ammo--;
                        GameObject.Find("AmmoText").GetComponent<TextMesh>().text = "Ammo: " + ammo;
                       lastDeleteTime = Time.time;
                      }
                    }
                  }
                }

              }


          }

    }

    public void AddAmmo() {
      ammo++;
      GameObject.Find("AmmoText").GetComponent<TextMesh>().text = "Ammo: " + ammo;
    }

    void ResetGame() {
      GameObject.Find("Controller").GetComponent<Spawner>().GridObj.setScore(0);
      SceneManager.LoadScene("main", LoadSceneMode.Single);
      ammo = 2;
    }

    void ChangeMode() {
      Spawner sp = GameObject.Find("Controller").GetComponent<Spawner>();

      GameObject cam = GameObject.Find("OVRCameraRig");
      Debug.Log(cam);
      if (mode == 0) {
        mode = 1;
        cam.transform.position = new Vector3(-1.6f, 0.5f, 1.01f);
        cam.transform.rotation = new Quaternion(0.0f, 0.707f, 0.0f, -0.707f);
        sp.paused = true;
      } else {
        mode = 0;
        cam.transform.position = new Vector3(3.0f, 12.0f, -3.0f);
        cam.transform.rotation = new Quaternion(0.383f, 0.0f, 0.0f, 0.924f);
      }
    }
}
