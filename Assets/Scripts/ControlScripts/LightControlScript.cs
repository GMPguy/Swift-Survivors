using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControlScript : MonoBehaviour
{
    Color BackAmbient;
    bool added = false;
    GameScript GS;
    public string ImportanceLevel = "Normal";
    // 0 - Off
    // 1 - Low
    // 2 - Medium
    // 3 - Good
    // 4 - High

    // Start is called before the first frame update
    void Start()
    {
        SetLight();
    }

    // Update is called once per frame
    public void SetLight()
    {
        if (GS == null) {
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(!added) {
                GS.LightCache.Add(this);
                added = true;
            }
        }

        if (GS.LightingQuality == 0)
        {
            // Completely off
            this.GetComponent<Light>().enabled = false;
            if (ImportanceLevel == "Sun") {
                BackAmbient = RenderSettings.ambientSkyColor;
                RenderSettings.ambientSkyColor = this.GetComponent<Light>().color;
                this.transform.GetChild(0).GetComponent<Light>().enabled = false;
            }
        }
        else
        {
            switch (ImportanceLevel)
            {
                case "Normal":
                    // Will always be on, but will be low quality at levels less than medium
                    this.GetComponent<Light>().enabled = true;
                    if (GS.GetComponent<GameScript>().LightingQuality >= 2)
                    {
                        this.GetComponent<Light>().renderMode = LightRenderMode.ForcePixel;
                        this.GetComponent<Light>().shadows = LightShadows.Hard;
                    }
                    else
                    {
                        this.GetComponent<Light>().renderMode = LightRenderMode.ForceVertex;
                        this.GetComponent<Light>().shadows = LightShadows.None;
                    }
                    break;
                case "Important":
                    // Always on and high quality
                    this.GetComponent<Light>().enabled = true;
                    this.GetComponent<Light>().renderMode = LightRenderMode.ForcePixel;
                    this.GetComponent<Light>().shadows = LightShadows.Hard;
                    break;
                case "Optional":
                    // Will be on if quality will be greater than medium, and will be high quality only at high
                    if (GS.LightingQuality == 4)
                    {
                        this.GetComponent<Light>().renderMode = LightRenderMode.ForcePixel;
                        this.GetComponent<Light>().enabled = true;
                        this.GetComponent<Light>().shadows = LightShadows.Hard;
                    }
                    else if (GS.GetComponent<GameScript>().LightingQuality >= 3)
                    {
                        this.GetComponent<Light>().renderMode = LightRenderMode.ForceVertex;
                        this.GetComponent<Light>().enabled = true;
                        this.GetComponent<Light>().shadows = LightShadows.None;
                    }
                    else
                    {
                        this.GetComponent<Light>().enabled = false;
                    }
                    break;
                case "Luxorious":
                    // Will be on only at high quality
                    if (GS.LightingQuality == 4)
                    {
                        this.GetComponent<Light>().renderMode = LightRenderMode.ForcePixel;
                        this.GetComponent<Light>().enabled = true;
                        this.GetComponent<Light>().shadows = LightShadows.Hard;
                    }
                    else
                    {
                        this.GetComponent<Light>().enabled = false;
                    }
                    break;
                case "Sun":
                    // It's sun, duh!
                    this.GetComponent<Light>().enabled = true;
                    this.transform.GetChild(0).GetComponent<Light>().enabled = true;
                    if(RenderSettings.ambientSkyColor.ToString() == this.GetComponent<Light>().color.ToString()) RenderSettings.ambientSkyColor = BackAmbient;
                    //if (this.transform.GetChild(0).GetComponent<Light>().color == RenderSettings.ambientLight) {
                    //    GameObject.Find("_RoundScript").GetComponent<RoundScript>().AmbientSet("Normal");
                    //}
                    break;
                default:
                    // Wrong light type
                    Debug.LogError("Wrong light type!");
                    break;
            }
        }
    }
}
