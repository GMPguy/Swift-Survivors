using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    // Variables
    public string CamType = "";
    public float CamReturn = 0f;
    public float FOV = 60f;
    public GameScript GS;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        // Return static camera
        if(CamReturn > 0f){
            CamReturn -= 0.01f * (Time.deltaTime * 50f);
        } else {
            CamType = "";
        }
        // Player camera

        // Note to self: there are too much camera related references in scripts... better if we do it later
        
    }
}
