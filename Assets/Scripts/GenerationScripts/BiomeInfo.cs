using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeInfo : MonoBehaviour {

    // Variables
    public string[] BiomeName;
    public string[] AvailableTerrainTypes;
    public GameObject[] Grasses;
    public string[] Sponges;

    public string MobPHsuggestion = "Default";
    public int[] AmountOfMobs;

    public float[] AmountOfMutants;
    public float[] AmountOfBandits;
    public int[] Radioactivity;

    public string FloraType = "Default";
    public string Barrier = "";
    public string Monument = "";
    public string Ambience = "";
    public string Music = "";
    public Color32[] DaytimeColors;
    // Colors
    public Color32[] SunColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Sun colors
    public Color32[] AmbientColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Ambient colors
    public Color32[] FogColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Fog and background colors
    public Color32[] AtmosphereColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Sky colors
    public Color32[] CloudColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Cloud colors
    public Color32[] PostProcessingColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Post processing colors
    public Vector4[] PostProcessingVariables = new Vector4[]{ Vector4.zero, Vector4.zero, Vector4.zero, Vector4.zero, Vector4.zero, Vector4.zero};
    // PPV Contrast, Saturation, Temperature
    public Color32[] GrassColor;
    // Colors
    // Variables

    void Start(){

        for(int PatchColor = 0; PatchColor < 6; PatchColor ++){

            bool[] Inherit = {true, true, true, true, true, true};
            if(FogColors[PatchColor].a == 0){
                Inherit[PatchColor] = false;
                switch(PatchColor){
                    case 0:
                        FogColors[PatchColor] = new Color32(200, 225, 255, 255);
                        break;
                    case 1:
                        FogColors[PatchColor] = new Color32(225, 125, 25, 255);
                        break;
                    case 2:
                        FogColors[PatchColor] = new Color32(55, 55, 75, 255);
                        break;
                    case 3:
                        //FogColors[PatchColor] = new Color32(75, 75, 125, 255);
                        FogColors[PatchColor] = new Color32(100, 100, 125, 255);
                        break;
                    case 4:
                        //FogColors[PatchColor] = new Color32(155, 55, 75, 255);
                        FogColors[PatchColor] = new Color32(55, 55, 100, 255);
                        break;
                    case 5:
                        FogColors[PatchColor] = new Color32(0, 0, 75, 255);
                        break;
                }
            }

            if(PatchColor <= 2 && CloudColors[PatchColor].a == 0){
                switch(PatchColor){
                    case 0:
                        CloudColors[PatchColor] = new Color32(255, 255, 255, 255);
                        break;
                    case 1:
                        CloudColors[PatchColor] = new Color32(55, 0, 25, 255);
                        break;
                    case 2:
                        CloudColors[PatchColor] = new Color32(0, 0, 0, 255);
                        break;
                }
            }

            if(PatchColor <= 2 && AmbientColors[PatchColor].a == 0){
                switch(PatchColor){
                    case 0:
                        AmbientColors[PatchColor] = new Color32(55, 55, 100, 255);
                        break;
                    case 1:
                        AmbientColors[PatchColor] = new Color32(25, 0, 55, 255);
                        break;
                    case 2:
                        AmbientColors[PatchColor] = new Color32(0, 0, 0, 255);
                        break;
                }
            }

            if(PatchColor <= 2 && SunColors[PatchColor].a == 0){
                switch(PatchColor){
                    case 0:
                        SunColors[PatchColor] = new Color32(255, 255, 200, 255);
                        break;
                    case 1:
                        SunColors[PatchColor] = new Color32(255, 125, 55, 255);
                        break;
                    case 2:
                        SunColors[PatchColor] = new Color32(55, 55, 255, 255);
                        break;
                }
            }

            if(PostProcessingColors[PatchColor].a == 0){
                switch(PatchColor){
                    case 0:
                        PostProcessingColors[PatchColor] = new Color32(255, 255, 255, 255);
                        break;
                    case 1:
                        PostProcessingColors[PatchColor] = new Color32(255, 125, 255, 255);
                        break;
                    case 2:
                        PostProcessingColors[PatchColor] = new Color32(125, 125, 255, 255);
                        break;
                    case 3:
                        PostProcessingColors[PatchColor] = new Color32(255, 255, 255, 255);
                        break;
                    case 4:
                        PostProcessingColors[PatchColor] = new Color32(255, 255, 255, 255);
                        break;
                    case 5:
                        PostProcessingColors[PatchColor] = new Color32(255, 255, 255, 255);
                        break;
                }
            }

            if(AtmosphereColors[PatchColor].a == 0){
                if(Inherit[PatchColor] == true) AtmosphereColors[PatchColor] = FogColors[PatchColor];
                else {
                    switch(PatchColor){
                        case 0:
                            AtmosphereColors[PatchColor] = new Color32(155, 200, 255, 255);
                            break;
                        case 1:
                            AtmosphereColors[PatchColor] = new Color32(100, 125, 155, 255);
                            break;
                        case 2:
                            AtmosphereColors[PatchColor] = new Color32(0, 0, 55, 255);
                            break;
                        default:
                            AtmosphereColors[PatchColor] = FogColors[PatchColor];
                            break;
                    }
                }
            }
        }

    }

}
