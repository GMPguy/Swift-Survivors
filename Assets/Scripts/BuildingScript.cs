using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {

    public string Type = "";
    public AudioClip[] audioClips;
    PlayerScript PS;
    GameScript GS;
    RoundScript RS;
    public Light mainLight;
    public AudioSource mainAudio;
    DestructionScript baseDestruct;

    // Fireplace
    float CampLife = 120f;
    // Fireplace

    void Start(){
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
        PS = GameObject.FindObjectOfType<PlayerScript>();
        baseDestruct = this.GetComponent<DestructionScript>();
    }

    void Update() {
        switch(Type){
            case "Campfire": Campfire(); break;
        }
    }

    void Campfire(){
        CampLife -= Time.deltaTime;
        mainLight.intensity = CampLife/120f;
        if(CampLife <= 0f || baseDestruct.Health <= 0f) {
            mainAudio.clip = audioClips[0];
            mainAudio.loop = false;
            mainAudio.Play();
            Destroy(mainLight.gameObject);
            Destroy(this);
        }
    }

}
