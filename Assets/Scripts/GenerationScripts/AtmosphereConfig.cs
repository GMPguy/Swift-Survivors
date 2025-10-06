using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtmosphereConfig", menuName = "Configs/Atmosphere")]
public class AtmosphereConfig : ScriptableObject {

    public Color32[] DaytimeColors;
    public Color32[] SunColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Sun colors
    public Color32[] AmbientColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Ambient colors
    public Color32[] FogColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Fog and background colors
    public Color32[] AtmosphereColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Sky colors
    public Color32[] CloudColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Cloud colors
    public Color32[] PostProcessingColors = new Color32[]{ new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0), new Color32(0, 0, 0, 0)}; // Post processing colors
    public Vector4[] PostProcessingVariables = new Vector4[]{ Vector4.zero, Vector4.zero, Vector4.zero, Vector4.zero, Vector4.zero, Vector4.zero};
    public Vector4 FogDistance = new Vector4(25f, 75f, 25f, 50f);

}
