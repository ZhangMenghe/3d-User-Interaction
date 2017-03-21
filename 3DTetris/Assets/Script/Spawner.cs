using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public GameObject[] groups;
    private Vector3 startPos;
    private Vector3 nextPos;
    public GameObject boundaryObj;
    public Grid GridObj;
    public bool paused;
    public int width;
    public int length;
    public int height;
    public bool NextUserDesignModel;
    public GameObject userDesignModel;
    private int currentSel;
    private int nextSel;
    private int timeFlag;
    private int diyCount;
    public AudioClip AudioClip;
    private static int numBlocks;

    void Start()
    {
        GridObj = new Grid(width, length, height);
        GridObj.clearAudio = AudioClip;
        startPos = new Vector3((int)GridObj.w / 2, (int )GridObj.h-3, (int)GridObj.l/2);
        nextPos = GameObject.Find("nextBound").transform.position;
        nextPos = nextPos+ new Vector3(-3.5f, -0.5f, 0.5f);
        currentSel = -1;
        nextSel = -1;
        timeFlag = 1;
        diyCount = 0;
        numBlocks = 0;
        paused = false;
        NextUserDesignModel = false;
        BuildBoundary();
        NextSpawner();
    }
    void BuildBoundary()
    {
        Vector3 pos = new Vector3(-0.5f, 2.0f, -0.5f);
        Instantiate(boundaryObj, pos, Quaternion.identity);
        Instantiate(boundaryObj, pos + new Vector3(.0f, .0f, GridObj.l), Quaternion.identity);
        Instantiate(boundaryObj, pos + new Vector3(GridObj.w, .0f, .0f), Quaternion.identity);
        Instantiate(boundaryObj, pos + new Vector3(GridObj.w, .0f, GridObj.l), Quaternion.identity);
    }
    public void NextSpawner()
    {
        numBlocks++;
        if (numBlocks % 10 == 0) {
          shot other = (shot) GameObject.Find("hand_right").GetComponent(typeof(shot));
          other.AddAmmo();
        }

        // Random Index
        if (currentSel == -1)
        {
            currentSel = Random.Range(0, groups.Length);
            nextSel = Random.Range(0, groups.Length);
            //Instantiate(groups[currentSel], startPos, Quaternion.identity);
            nextSel = 0;
        }
        else
        {
            currentSel = nextSel;
            nextSel = Random.Range(0, groups.Length);
            GameObject nextGene = GameObject.Find("model");
            //if(nextGene.tag != "dontDestroy")
                Destroy(nextGene);

        }

        if (!NextUserDesignModel)
        {
            Instantiate(groups[currentSel], startPos, Quaternion.identity);
            var model = Instantiate(groups[nextSel], nextPos, Quaternion.identity);
            model.GetComponent<Group>().enabled = false;
            model.name = "model";
            model.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (timeFlag == 1)
        {
            diyCount++;
            userDesignModel = GameObject.Find("DIYobj");
            Instantiate(groups[currentSel], startPos, Quaternion.identity);
            var model = Instantiate(userDesignModel, nextPos, Quaternion.identity);
            model.GetComponent<Group>().enabled = false;
            model.name = "miao" + diyCount.ToString() ;
            //model.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            Vector3 basePos = model.transform.GetChild(0).localPosition;
            for (int i = 0; i < userDesignModel.transform.childCount; i++)
            {
                 model.transform.GetChild(i).localPosition -= basePos;
                 Destroy(userDesignModel.transform.GetChild(i).gameObject);
             }

            timeFlag++;
        }
        else
        {
            userDesignModel = GameObject.Find("miao" + diyCount.ToString());
            userDesignModel.GetComponent<Group>().enabled = true;
            //userDesignModel.transform.localScale = new Vector3(5f, 5f, 5f);
            userDesignModel.transform.position = startPos;
            var model = Instantiate(groups[nextSel], nextPos, Quaternion.identity);
            model.GetComponent<Group>().enabled = false;
            model.name = "model";
            model.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            timeFlag=1;
            NextUserDesignModel = false;
            //userDesignModel.GetComponent<Group>().enabled = false;
        }

    }
}
