using UnityEngine;
using System.Collections;

public class ParticleEffect : MonoBehaviour {

    private ParticleSystem myParticle;

	// Use this for initialization

	void Awake () {
        myParticle = GetComponentInChildren<ParticleSystem>();
    }
/*
    void PlayParticle()
    {
        myParticle.Play();
    }
	*/
    void Update()
    {
        myParticle.Play();
    }

}
