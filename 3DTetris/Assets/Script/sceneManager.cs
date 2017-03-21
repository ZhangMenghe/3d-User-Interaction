using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour {
    public int restartTime = 10;
    private int restartTimer;
	// Use this for initialization
	void Start () {
        restartTimer = 0;
    }
    bool checkRestartCondition()
    {
        return false;
    }
    // Update is called once per frame
    void Update () {
        if (checkRestartCondition())
        {
            restartTimer++;
            if (restartTimer > restartTime)
                SceneManager.LoadScene("main", LoadSceneMode.Single);
        }
	}
}
