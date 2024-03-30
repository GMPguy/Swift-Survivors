using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudColorControl : MonoBehaviour {

    // Variables
    public string Specific = "";
    GameScript GS;
    bool added = false;

    public Color32 BlendColor;
    public float BlendForce = 0f;
    public float Alpha = 1f;

	// Use this for initialization
	void Start () {
        SetColor("");
	}
	
	// Update is called once per frame
	public void SetColor (string Type) {

        if(GS == null){
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(!added) {
                GS.HudCache.Add(this);
                added = true;
            }
        }

        if (Specific == "WBText") {

            if (float.Parse(GS.HudColorHUE.Substring(4, 2)) / 99f > 0.75f) {
                this.GetComponent<Text>().color = new Color(0.3f, 0.3f, 0.35f, this.GetComponent<Text>().color.a);
            } else {
                this.GetComponent<Text>().color = new Color(1f, 1f, 1f, this.GetComponent<Text>().color.a);
            }

        } else if (Specific == "WBImage") {

            if (float.Parse(GS.HudColorHUE.Substring(4, 2)) / 99f > 0.75f) {
                this.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.35f, this.GetComponent<Image>().color.a);
            } else {
                this.GetComponent<Image>().color = new Color(1f, 1f, 1f, this.GetComponent<Image>().color.a);
            }

        } else {

            Color32 HudColor = Color.HSVToRGB(float.Parse(GS.HudColorHUE.Substring(0, 2)) / 99f,
            float.Parse(GS.HudColorHUE.Substring(2, 2)) / 99f,
            float.Parse(GS.HudColorHUE.Substring(4, 2)) / 99f);
            this.GetComponent<Image>().color = Color32.Lerp(HudColor, BlendColor, BlendForce);
            this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, Alpha);

        }
        
    }
}
