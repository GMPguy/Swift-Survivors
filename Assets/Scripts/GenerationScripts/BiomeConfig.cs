using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Configs/Biome")]
public class BiomeConfig : ScriptableObject {

    // Variables
    public string[] BiomeName;
    public string[] AvailableTerrainTypes;
    public GameObject[] Grasses;
    public string[] Sponges;
    public Color32[] GrassColor;

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

    public AtmosphereConfig Atmosphere;

}
