using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControlScript : MonoBehaviour {

    GameScript GS;
    bool added = false;

	// Use this for initialization
	void Start () {

        SetVolume();
		
	}
	
	// Update is called once per frame
	public void SetVolume () {

        if (GS == null) {
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(!added) {
                GS.SoundCache.Add(this);
                added = true;
            }
        }

        if (GS != null) {
            if (this.name.Substring(0, 2) == "S_") {
                this.GetComponent<AudioSource>().volume = GS.SoundVolume * GS.MasterVolumeA;
            } else if (this.name.Substring(0, 2) == "M_") {
                this.GetComponent<AudioSource>().volume = GS.MusicVolume * GS.MasterVolumeA;
            } 
        }   

	}
}
