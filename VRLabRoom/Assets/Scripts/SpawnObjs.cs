using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjs : MonoBehaviour {
    public Transform lockPrefab;
    public Transform paintPrefab;
    public Transform StoragePrefab;
    public Transform tvPrefab;
    public Transform boardPrefab;
    public Transform deskPrefab;
    public bool forceToSpawnAll;
    public bool allExist;
    public OVRInput.Controller controller;

    // public Transform ws2Prefab;
    // public Transform ws3Prefab;

    private int spawnSequence;
    private bool[] existList;
    // Use this for initialization
    void Start () {
        forceToSpawnAll = false;
        spawnSequence = -1;
        existList = new bool[8];
        for (int i = 0; i < 8; i++)
            existList[i] = false;
    }

    bool NeedSpawnSomething()
    {
        bool need = OVRInput.Get(OVRInput.Button.One, controller);

        return need;
    }
    void updateSpawnSelection()
    {
        spawnSequence = (spawnSequence+1)%8;
        if (spawnSequence == 7)
            allExist = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (forceToSpawnAll || NeedSpawnSomething())
        {
            updateSpawnSelection();
            //spawnSequence = 2;
            if (!existList[spawnSequence])//not exist
            {
                existList[spawnSequence] = true;//make it exist
                switch (spawnSequence)
                {
                    case 0://lock
                        var lockP =  Instantiate(lockPrefab, new Vector3(-4.34f, -0.335f, 1.478f), Quaternion.identity);
                        lockP.gameObject.name = "Lock";
                        break;
                    case 1://paint
                        var paintP = Instantiate(paintPrefab, new Vector3(0.3f, -0.4f, 1.13f), Quaternion.Euler(0, 7.843f, 0));
                        paintP.gameObject.name = "paintGroup";
                        //transform.Rotate(new Vector3(.0f, 7.843f, .0f));
                        break;
                    case 2://storage
                        var sP1 = Instantiate(StoragePrefab, new Vector3(1.44f, -0.7f, 0.3f), Quaternion.Euler(-90, 0, 82));
                        var sP2 = Instantiate(StoragePrefab, new Vector3(1.63f, -0.7f, -0.97f), Quaternion.Euler(-90, 0, 82));
                        var sP3 = Instantiate(StoragePrefab, new Vector3(1.8f, -0.7f, -2.24f), Quaternion.Euler(-90, 0, 82));
                        sP1.gameObject.name = "Storage0";
                        sP2.gameObject.name = "Storage1";
                        sP3.gameObject.name = "Storage2";
                        break;
                    case 3://tv
                        var tvP1 = Instantiate(tvPrefab, new Vector3(-6.0f, -0.25f, 0.7f), Quaternion.Euler(-90, 0, -10));
                        var tvP2 = Instantiate(tvPrefab, new Vector3(-3.7f, -0.25f, 1.6f), Quaternion.Euler(-90, 0, 0));
                        tvP1.gameObject.name = "TV0";
                        tvP2.gameObject.name = "TV1";
                        //transform.Rotate(new Vector3(-90.0f, 0f, .0f));
                        break;
                    case 4://board
                        var boardP = Instantiate(boardPrefab, new Vector3(-2.6f, -0.1f, 1.57f), Quaternion.Euler(-90, 0, 7.8f));
                        boardP.gameObject.name = "WhiteBoard";
                        //transform.Rotate(new Vector3(-90.0f, 0f, .0f));
                        break;
                    case 5://ws-1
                        for(int i = 0; i < 5; i++)
                        {
                            var deskP = Instantiate(deskPrefab, new Vector3(-6.0f, -0.4f, -3.8f + i * 0.7f), Quaternion.Euler(-90, 0, 180));
                            deskP.gameObject.name = "DeskGroup" + i.ToString();
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            var deskP2 = Instantiate(deskPrefab, new Vector3(-5.25f, -0.4f, -3.8f + i * 0.7f), Quaternion.Euler(-90, 0, 0));
                            deskP2.gameObject.name = "DeskGroup" + (i+5).ToString();
                        }
                        break;
                    case 6://ws-2
                        for (int i = 0; i < 5; i++)
                        {
                            var deskP3 = Instantiate(deskPrefab, new Vector3(-4.0f, -0.4f, -3.8f + i * 0.7f), Quaternion.Euler(-90, 0, 180));
                            deskP3.gameObject.name = "DeskGroup" + (i + 10).ToString();
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            var deskP4 = Instantiate(deskPrefab, new Vector3(-3.25f, -0.4f, -3.8f + i * 0.7f), Quaternion.Euler(-90, 0, 0));
                            deskP4.gameObject.name = "DeskGroup" + (i + 15).ToString();
                        }
                        break;
                    case 7://ws-3
                        for (int i = 0; i < 5; i++)
                        {
                            var deskP5 = Instantiate(deskPrefab, new Vector3(-2.0f, -0.4f, -3.8f + i * 0.7f), Quaternion.Euler(-90, 0, 180));
                            deskP5.gameObject.name = "DeskGroup" + (i + 20).ToString();
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            var deskP6 = Instantiate(deskPrefab, new Vector3(-1.25f, -0.4f, -3.8f + i * 0.7f), Quaternion.Euler(-90, 0, 0));
                            deskP6.gameObject.name = "DeskGroup" + (i + 25).ToString();
                        }
                        break;

                }
            }
            
        }//end needSpawn
        

    }
}
