using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawPath : MonoBehaviour {
    // Creates a line renderer that follows a Sin() function
    // and animates it.
    public onEenterCheckPoint enterPointScript;
    public LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 2;
    private GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("airPlane");
        enterPointScript = player.GetComponent<onEenterCheckPoint>();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.numPositions = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;
    }
	
	// Update is called once per frame
	void Update () {
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, enterPointScript.checkpointsPos[enterPointScript.checkNum]);
    }
}
