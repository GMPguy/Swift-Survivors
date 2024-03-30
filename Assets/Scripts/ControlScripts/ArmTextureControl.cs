using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmTextureControl : MonoBehaviour {

    // variables
    public bool Needed = false;
    bool WasSet = false;

    // References
    PlayerScript Player;
    GameObject Hand;

    // For Arms
    public List<string> ArmMaterialNames;
    public List<Color> ColorsDefault;
    public List<Color> ColorsWet;

    void Start() {
        //Set(0f);
    }

    public void Set(float Wet){

        if(!WasSet){

            ArmMaterialNames = new List<string>();
            ColorsDefault = new List<Color>();
            ColorsWet = new List<Color>();

            WasSet = true;
            Player = this.transform.root.GetComponent<PlayerScript>();
            Hand = this.transform.parent.parent.GetChild(0).gameObject;

            // Setting up variables
            for(int GetMat = 0; GetMat < this.GetComponent<MeshRenderer>().materials.Length; GetMat ++){
                Material Mat = this.GetComponent<MeshRenderer>().materials[GetMat];

                ArmMaterialNames.Add(Mat.name);

                switch(Mat.name){
                    case "ConscriptJacket3 (Instance)":
                        ColorsDefault.Add(new Color32(125, 163, 134, 255));
                        ColorsWet.Add(ColorsDefault.ToArray()[GetMat] * 0.25f);
                        break;
                    case "ConscriptJacket2 (Instance)":
                        ColorsDefault.Add(new Color32(225, 225, 225, 255));
                        ColorsWet.Add(ColorsDefault.ToArray()[GetMat] * 0.5f);
                        break;
                    case "ConscriptJacket1 (Instance)":
                        ColorsDefault.Add(new Color32(145, 188, 156, 255));
                        ColorsWet.Add(ColorsDefault.ToArray()[GetMat] * 0.25f);
                        break;
                }

            }

        }

        // Actually setting
        for(int Set = 0; Set < this.GetComponent<MeshRenderer>().materials.Length; Set ++){
            Material SetMat = this.GetComponent<MeshRenderer>().materials[Set];
            SetMat.color = Color.Lerp(ColorsDefault.ToArray()[Set], ColorsWet.ToArray()[Set], Wet);
        }

    }

}
