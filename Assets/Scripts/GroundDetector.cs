using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour {

    public int Detected = 0;
    public GameObject DetectedObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Detected > 0) {
            Detected -= 1;
        } else {
            DetectedObject = null;
        }
		
	}

    void OnTriggerStay(Collider other) {

        Detected = 3;
        DetectedObject = other.gameObject;

    }
}
