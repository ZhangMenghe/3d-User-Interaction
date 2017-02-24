using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadCheckPoints : MonoBehaviour {
    public onEenterCheckPoint entercheckPointScript;
    public Transform checkPointPrefab;
    public GameObject player;
    public float scale;
    private List<Transform> checkpointTrans;
    private List<Vector3> checkpointsPos;
    private string filename;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("airPlane");
        entercheckPointScript = player.GetComponent<onEenterCheckPoint>();

        filename = "Assets/data/Competition_Track.txt";
        checkpointsPos = new List<Vector3>();
        checkpointTrans = new List<Transform>();
        readCheckPointFromFile();
        setupCheckPointInScene();

        entercheckPointScript.checkpointsPos = checkpointsPos;
        entercheckPointScript.checkpointTrans = checkpointTrans;
        entercheckPointScript.ready = true;
    }

    void readCheckPointFromFile()
    {
        string line;

        System.IO.StreamReader file = new System.IO.StreamReader(filename);

        while ((line = file.ReadLine()) != null)
        {
            char[] delims = { ' ' };
            string[] words = line.Split(delims);
            float[] values = new float[3];

            for (int i = 0; i < words.Length; i++)
            {
                if (i >= 3)
                {
                    print("Error in XYZ file.");
                    break;
                }
                string word = words[i];
                values[i] = float.Parse(word)* scale;
            }

            checkpointsPos.Add(new Vector3(values[0], values[1], values[2]));
        }
    }
    void setupCheckPointInScene()
    {
        for(int i = 0; i < checkpointsPos.Count; i++)
        {
            var obj = Instantiate(checkPointPrefab, checkpointsPos[i], Quaternion.identity);
            obj.gameObject.name = "CheckPoint" + i.ToString();
            checkpointTrans.Add(obj);
        }
        
        Transform check1 = checkpointTrans[0];
        Transform check2 = checkpointTrans[1];
        player.transform.position = check1.position;
        player.transform.LookAt(check2);
        player.transform.Translate(-player.transform.right * 50.0f);
        //player.transform.Rotate(Vector3.up);
    }
    void startCountingDown()
    {

    }
}
