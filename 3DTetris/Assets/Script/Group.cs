using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Group : MonoBehaviour {
    public AudioClip moveAudio;
    private AudioSource audioSound;

    public int gameDifficulty;
    // Time since last gravity tick
    private float fallSpeed;
    private float addSpeed;
    private Grid GridObj;
    public bool isOnGround;
    private int lastRot = 0;
    private int frames = 0;
    // Use this for initialization
    void Start()
    {
        audioSound = GameObject.Find("Controller").GetComponent<AudioSource>();
        isOnGround = false;
        GridObj = GameObject.Find("Controller").GetComponent<Spawner>().GridObj;
        fallSpeed = 0.01f * Mathf.Pow(1.27f, GridObj.clearLineNum);
        Debug.Log(GridObj.clearLineNum);
        Debug.Log(fallSpeed);
        addSpeed = 0.01f;
        //Time.timeScale = gameDifficulty;
        if (!isValidGridPos()) {
            if(gameObject.name.IndexOf("miao")!=-1){
              Destroy(gameObject);
              FindObjectOfType<Spawner>().NextSpawner();
            }
            else{
              Destroy(gameObject);
              GameObject.Find("PauseText").GetComponent<TextMesh>().text = "GAME OVER!!";
            }
        }

    }
    public static Vector3 getIntegerGrid(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }
    //check if position inside border
    bool isValidGridPos()
    {
        foreach (Transform child in transform)// should be 4
        {
            Vector3 v = getIntegerGrid(child.position);
            if (!GridObj.insideBorder(v))//check if inside border
                return false;
            var currentGrid = GridObj.grid[(int)v.x, (int)v.y, (int)v.z];
            if (currentGrid != null && currentGrid.parent != transform)
                return false;
        }
        return true;
    }
    void updateGrid()
    {
        //Remove old children from Grid
        for (int y = 0; y < GridObj.h; ++y) {
            for (int x = 0; x < GridObj.w; ++x) {
                for (int z = 0; z < GridObj.l; ++z) {
                    if (GridObj.grid[x, y, z] != null) {
                        if (GridObj.grid[x, y, z].parent == transform) {
                            GridObj.grid[x, y, z] = null;
                        }
                    }
                }
            }
        }

        //add new children to grid
        foreach (Transform child in transform)
        {
            Vector3 v = getIntegerGrid(child.position);
            GridObj.grid[(int)v.x, (int)v.y, (int)v.z] = child;
        }
    }

    void GetRotation()
    {
        Vector3 rot = Vector3.zero;
        Vector3 dir = Vector3.down;

        Vector3 contRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch).eulerAngles;

        bool trigger = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);

        int c = 60;

        // Rotate Right
        if (contRot.z > 40.0f && contRot.z < 100.0f) {
            if (lastRot == 1) {
              frames++;
            } else {
              frames = 0;
            }

            if (frames % c == 0) {
              if (trigger) {
                rot = new Vector3(0, 0, 90);
              } else {
                dir = Vector3.left;
              }
            }

            lastRot = 1;
        }
        // Rotate left
        else if (contRot.z < 320.0f && contRot.z > 260.0f) {
            if (lastRot == 2) {
              frames++;
            } else {
              frames = 0;
            }

            if (frames % c == 0) {
              if (trigger) {
                rot = new Vector3(0, 0, -90);
              } else {
                dir = Vector3.right;
              }
            }
            lastRot = 2;
        }
        // Rotate Back
        else if (contRot.x < 300.0f && contRot.x > 240.0f) {
            if (lastRot == 3) {
              frames++;
            } else {
              frames = 0;
            }

            if (frames % c == 0) {
              if (trigger) {
                rot = new Vector3(-90, 0, 0);
              } else {
                dir = Vector3.back;
              }
            }
            lastRot = 3;
        }
        // Rotate Forward
        else if (contRot.x > 20.0f && contRot.x < 80.0f) {
            if (lastRot == 4) {
              frames++;
            } else {
              frames = 0;
            }

            if (frames % c == 0) {
              if (trigger) {
                rot = new Vector3(90, 0, 0);
              } else {
                dir = Vector3.forward;
              }
            }
            lastRot = 4;
        }
        // Twist right
        else if (contRot.y < 290.0f && contRot.y > 220.0f) {
            if (lastRot == 5) {
              frames++;
            } else {
              frames = 0;
            }

          if (frames % c == 0) {
            if (trigger) {
              rot = new Vector3(0, -90, 0);
            } else {
              dir = Vector3.right;
            }
          }
          lastRot = 5;
        }
        // Twist left
        else if (contRot.y > 90.0f && contRot.y < 160.0f) {
            if (lastRot == 6) {
              frames++;
            } else {
              frames = 0;
            }

          if (frames % c == 0) {
            if (trigger) {
              rot = new Vector3(0, 90, 0);
            } else {
              dir = Vector3.left;
            }
          }
          lastRot = 6;
        } else {
          lastRot = 0;
        }

        if (rot != Vector3.zero) {
          transform.Rotate(rot, Space.World);
          if (isValidGridPos()) {
            updateGrid();
          } else {
            transform.Rotate(-rot, Space.World);
          }
        }

        float speed = GetFallingSpeed(dir);
        transform.position += dir * speed;
        if(dir != -Vector3.up)
            audioSound.PlayOneShot(moveAudio);
        if (isValidGridPos()) {
          updateGrid();
        }
        else {
            GetRoundY(dir * speed);
            if (dir == -Vector3.up) {
                //falling on the ground
                //Clear filled horizontal lines
                GridObj.deleteFullPlane();
                GridObj.addToScore(50);
                isOnGround = true;
                //Spawn next Group
                FindObjectOfType<Spawner>().NextSpawner();

                //Disable script
                enabled = false;
            }
        }

    }

    float GetFallingSpeed(Vector3 dir)
    {

        Vector3 contRotL = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.LTouch);
        Vector3 contRotR = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);

        if (contRotL.y < -9.5f && contRotR.y < -9.5f) {
          fallSpeed += addSpeed;
          return fallSpeed;
        }

        if (dir != -Vector3.up) {
          return 1.0f;//must move one block
        }
        return fallSpeed;
    }

    void GetRoundY(Vector3 floatY)
    {
        Vector3 floatPos = transform.position - floatY;
        int floorY = (int)Mathf.Floor(floatPos.y);
        if (floatPos.y - floorY > 0.5f) {
          transform.position = new Vector3(floatPos.x, 0.5f + floorY, floatPos.z);
        } else {
          transform.position = new Vector3(floatPos.x, (int)floorY, floatPos.z);
        }
        transform.position = floatPos;
    }



    bool GetPauseStatus()
    {
        Spawner sp = GameObject.Find("Controller").GetComponent<Spawner>();
        if (OVRInput.GetDown(OVRInput.RawButton.Start)) {
          sp.paused = !sp.paused;
        }

        if (sp.paused) {
          GameObject.Find("PauseText").GetComponent<TextMesh>().text = "Paused";
        } else {
          GameObject.Find("PauseText").GetComponent<TextMesh>().text = "";
        }

        return sp.paused;
    }

    // Update is called once per frame
    void Update () {
        if (GetPauseStatus()) {
          return;
        }
        GetRotation();
    }
}
