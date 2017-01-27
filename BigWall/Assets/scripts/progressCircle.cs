using UnityEngine;
using UnityEngine.UI;

public class progressCircle : MonoBehaviour
{
    public Image filterImg;
    public bool isProgressCircleRun;
    //public float progressSpeed;
    public gameController gameControllerScript;

    private float FinishValue;
    public float fillValue;//fractional of filling
    //public Text attachedText;
    // Use this for initialization
    void Start()
    {
        fillValue = .0f;
        filterImg.fillAmount = .0f;
        //attachedText.text = "0%";
        isProgressCircleRun = false;
        GameObject Controller = GameObject.Find("Controller");
        gameControllerScript = Controller.GetComponent<gameController>();
        FinishValue = (float)gameControllerScript.TimeToDispear-1;
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isProgressCircleRun && fillValue <= FinishValue)
        {
            float Dvalue = FinishValue-fillValue;
            if (Dvalue > 0)
            {
                fillValue +=1;
                if(fillValue >= FinishValue) {//disappear it
                    this.gameObject.SetActive(false);
                    //Debug.Log(fillValue);
                    isProgressCircleRun = false;
                }
                else
                {
                    filterImg.fillAmount = fillValue / FinishValue;
                    //attachedText.text = Mathf.RoundToInt(fillValue / FinishValue).ToString() + " %";
                }
            }
        }
    }
}
