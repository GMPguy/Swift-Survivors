using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandpartScript : MonoBehaviour {

    // Main variables
    public float[] LandRotations;
    public bool SmoothRotation;
    public Transform[] KeepTransforms;
    Vector3[] KeepPositions;
    Quaternion[] KeepRotations;

    public void Setup () {

        // Keep rotations
        for (int t = 0; t < KeepTransforms.Length; t++) {
            KeepPositions[t] = KeepTransforms[t].position;
            KeepRotations[t] = KeepTransforms[t].rotation;
        }
        
        // Rotate
        if (LandRotations != null && LandRotations.Length > 0)
            if (SmoothRotation)
                this.transform.Rotate(Vector3.up * LandRotations[Random.Range(0, LandRotations.Length)]);
            else {
                float randome = Random.Range(0, LandRotations.Length);
                int first = (int)randome;
                int second = (first + 1) % LandRotations.Length;
                this.transform.Rotate(Vector3.up * Mathf.Lerp(LandRotations[first], LandRotations[second], randome % 1f));
            }

        // Set rotations
        for (int t = 0; t < KeepTransforms.Length; t++) {
            KeepTransforms[t].position = KeepPositions[t];
            KeepTransforms[t].rotation = KeepRotations[t];
        }

    }
}
