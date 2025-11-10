using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class LandpartScript : MonoBehaviour {

    // Main variables
    public float[] LandRotations;
    public bool SmoothRotation;

    public float[] LandHeight;
    public int LandHeightMap = -1;
    public bool SmoothHeight;

    public Transform[] KeepTransforms;
    public Anchor[] ObjectAnchors;

    Vector3[] KeepPositions;
    Quaternion[] KeepRotations;

    public void Setup (LandScript terrain) {

        // Keep rotations
        KeepPositions = new Vector3[KeepTransforms.Length];
        KeepRotations = new Quaternion[KeepTransforms.Length];

        for (int t = 0; t < KeepTransforms.Length; t++) {
            KeepPositions[t] = KeepTransforms[t].position;
            KeepRotations[t] = KeepTransforms[t].rotation;
        }
        
        // Rotate
        if (LandRotations != null && LandRotations.Length > 0)
            if (!SmoothRotation)
                this.transform.Rotate(Vector3.up * LandRotations[Random.Range(0, LandRotations.Length)]);
            else {
                float randome = Random.Range(0f, LandRotations.Length - .09f);
                int first = (int)randome;
                int second = (first + 1) % LandRotations.Length;
                this.transform.Rotate(Vector3.up * Mathf.Lerp(LandRotations[first], LandRotations[second], randome % 1f));
            }

        // Set height
        if (LandHeight != null && LandHeight.Length > 0)
            if (LandHeightMap >= 0) {
                float2 margins = terrain.GetNoise(transform.position.x, transform.position.z, LandHeightMap);
                float value = Random.Range(margins.x, margins.y);
                this.transform.position += Vector3.up * Mathf.Lerp(LandHeight[0], LandHeight[1], value);
            } else if (!SmoothHeight)
                this.transform.position += Vector3.up * LandHeight[Random.Range(0, LandHeight.Length)];
            else {
                float randome = Random.Range(0f, LandHeight.Length - .09f);
                int first = (int)randome;
                int second = (first + 1) % LandHeight.Length;
                this.transform.position += Vector3.up * Mathf.Lerp(LandHeight[first], LandHeight[second], randome % 1f);
                Debug.Log($"Randome {randome}, first {first} second {second}, mod {randome % 1f}");
            }

        // Set rotations
        for (int t = 0; t < KeepTransforms.Length; t++) {
            KeepTransforms[t].position = KeepPositions[t];
            KeepTransforms[t].rotation = KeepRotations[t];
        }

        // Set anchors
        for (int a = 0; a < ObjectAnchors.Length; a++) {
            Anchor anchor = ObjectAnchors[a];

            for (int at = 0; at < anchor.transforms.Length; at++) {
                Transform anchorT = anchor.transforms[at];
                anchorT.position += Vector3.up * 10f;
                Vector3 pos = anchorT.position;

                if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit)) {
                    float angle = Vector3.Angle(hit.normal, Vector3.up);

                    if (angle > anchor.Angle) {
                        Destroy(anchorT.gameObject);
                        continue;
                    }

                    pos = hit.point;

                    if (anchor.LerpToNormal > 0f)
                        anchorT.up = Vector3.Lerp(anchorT.up, hit.normal, angle / 90f * anchor.LerpToNormal);

                    if (anchor.PullUpAngle > 0f)
                        pos += anchor.PullUpAngle * (angle / 90f) * Vector3.up;

                    anchorT.position = pos;
                } else
                    Destroy(anchorT.gameObject);
            }
        }

    }

    [System.Serializable]
    public class Anchor {
        public Transform[] transforms;
        public float LerpToNormal;
        public float PullUpAngle;
        public float Angle = 10f;
    }

}
