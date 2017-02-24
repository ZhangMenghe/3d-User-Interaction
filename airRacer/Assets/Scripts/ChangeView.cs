using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeView : MonoBehaviour {
    private bool couldChangeView;
    private int ViewChoice;
    public GameObject Cabin;
    public GameObject Jet;
    private GameObject viewNumText;
    // Use this for initialization
    void Start () {
        couldChangeView = true;
        ViewChoice = 0;//0 for FPS,  2 for third view,3 for cockpit ,
        viewNumText = GameObject.Find("ViewText");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextView();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
            CouldDoNext();

    }
    public void NextView()
    {
        if (couldChangeView)
        {
            ViewChoice = (ViewChoice+1)%2;
            Debug.Log(ViewChoice);
            switch (ViewChoice)
            {
                case 0:
                    if (Cabin != null)
                        Cabin.SetActive(false);
                    Jet.transform.localPosition = new Vector3(Jet.transform.localPosition.x, Jet.transform.localPosition.y, -Jet.transform.localPosition.z);
                    viewNumText.GetComponent<TextMesh>().text = "FPS View";
                    break;

                case 1:
                    Cabin.SetActive(false);
                    Jet.transform.localPosition = new Vector3(Jet.transform.localPosition.x, Jet.transform.localPosition.y, -Jet.transform.localPosition.z);
                    viewNumText.GetComponent<TextMesh>().text = "3rd View";
                    break;
                case 2:
                    Cabin.SetActive(true);
                    viewNumText.GetComponent<TextMesh>().text = "Cockpit View";
                    break;
            }
            couldChangeView = false;
        }

    }
    public void CouldDoNext()
    {
        couldChangeView = true;
    }
}
