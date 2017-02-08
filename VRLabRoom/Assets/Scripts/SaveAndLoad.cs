using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
class objData
{
    public Vector3 newPos;
    public Quaternion rotation;
}
public class SaveAndLoad : MonoBehaviour {
    public string fileName;
    private string[] findNameList;
    private int[] findItemNum;
    private StreamWriter file;
    private Transform targetObj;
    private Transform[] targetGroup;
    // Use this for initialization
    void Start () {
        initialFind();//findNameList and findItemNum
    }
	void initialFind()
    {
        findNameList = new string[8];//desks, whiteboard, paint, lock, storage, tv 
        findNameList[0] = "Lock";
        findNameList[1] = "WhiteBoard";
        findNameList[2] = "Paint";
        findNameList[3] = "Storage";
        findNameList[4] = "TV";
        findNameList[5] = "DeskGroup";
        findNameList[6] = "Desk";
        findNameList[7] = "Chair";
        findItemNum = new int[6];
        findItemNum[0] = 1;
        findItemNum[1] = 1;
        findItemNum[2] = 1;
        findItemNum[3] = 3;
        findItemNum[4] = 2;
        findItemNum[5] = 30;
        findItemNum[6] = 30;
        findItemNum[7] = 30;
    }
    void Awake()
    {
        fileName = Application.dataPath + "/information.txt";

    }
    private string createRotationString(Quaternion rot)
    {
        string rotationString = "";
        rotationString += rot.x.ToString() + " ";
        rotationString += rot.y.ToString() + " ";
        rotationString += rot.z.ToString() + " ";
        rotationString += rot.w.ToString() + " ";
        //data.rotation.ToString()
        return rotationString;
    }
    private string createPositionString(Vector3 pos)
    {
        string PosString = "";
        PosString += pos.x.ToString() + " ";
        PosString += pos.y.ToString() + " ";
        PosString += pos.z.ToString() + " ";
        //data.rotation.ToString()
        return PosString;
    }

    bool getTargetIsObj(int i)
    {
        if (i < 3)//single object
        {
            targetObj = GameObject.Find(findNameList[i]).transform;
            return true;
        }
        else
        {
            targetGroup = new Transform[findItemNum[i]];
            for (int j = 0; j < findItemNum[i]; j++)
            {
                string name = findNameList[i] + j.ToString();
                targetGroup[j] = GameObject.Find(name).transform;
            }
            return false;
        }
    }
    public void Save()
    {
        file = new StreamWriter(fileName);
        //rotation = GameObject.Find("Cube").transform.rotation;
        for (int i = 0; i < findNameList.Length; i++)
        {
            if (getTargetIsObj(i))//single obj
            {
                objData data = new objData();
                data.newPos = targetObj.position;
                data.rotation = targetObj.rotation;
                file.WriteLine(createPositionString(data.newPos));
                file.WriteLine(createRotationString(data.rotation));
            }
            else//group
            {
                for (int j = 0; j < findItemNum[i]; j++)
                {
                    objData data = new objData();
                    data.newPos = targetGroup[j].position;
                    data.rotation = targetGroup[j].rotation;
                    file.WriteLine(createPositionString(data.newPos));
                    file.WriteLine(createRotationString(data.rotation));
                }
            }
        }


        //file.WriteLine(Time.time + " | " + "Cube");

        Debug.Log("save to " + fileName);
        file.Close();
    }
    private void restoreRotation(GameObject obj, string[] rot)
    {
        obj.transform.rotation = new Quaternion(float.Parse(rot[0]), float.Parse(rot[1]), float.Parse(rot[2]), float.Parse(rot[3]));
    }
    private void restorePosition(GameObject obj, string[] pos)
    {
        obj.transform.position = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
    }
    public void Load()
    {
        string[] lines = System.IO.File.ReadAllLines(fileName);
        int index = 0;
        Transform restoreTarget;
        for (int i = 0; i < findNameList.Length; i++)
        {
            for (int j = 0; j < findItemNum[i]; j++)
            {
                if (j == 0)
                {
                    if (findItemNum[i] == 1)//single
                        restoreTarget = GameObject.Find(findNameList[i]).transform;
                    else
                        restoreTarget = GameObject.Find(findNameList[i] + '0').transform;
                }
                else
                {
                    restoreTarget = GameObject.Find(findNameList[i] + j.ToString()).transform;
                }
                string[] pos = lines[index++].Split();
                string[] rot = lines[index++].Split();
                restorePosition(restoreTarget.gameObject, pos);
                restoreRotation(restoreTarget.gameObject, rot);
            }
        }
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Load();
    }
}
