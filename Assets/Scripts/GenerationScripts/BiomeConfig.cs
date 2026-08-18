using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random=UnityEngine.Random;

[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Configs/Biome")]
public class BiomeConfig : ScriptableObject {

    // Variables
    public string[] BiomeName;
    public string[] AvailableTerrainTypes;
    public GameObject Bottomland;

    public GameObject[] Grasses;
    public string[] Sponges;
    public Color32[] GrassColor;
    public NoiseMap[] Noises;

    public int[] AmountOfPlants;
    public string[] PlantTypes;

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

    public AtmosphereConfig[] Atmospheres;

    // Generation banks
    public float2 WorldSize = new (500f, 500f);
    public float2 TileSize = new (50f, 50f);

    public TileBank[] TileBanks;
    public TileBase FallbackTile;
    public TileBase[] MonumentTile;

    [System.Serializable]
    public class TileBank {
        public Tile[] Tiles;

        public float2 PerlinMargins = new (0f, 1f);
        public float PerlinShift;
        public float PerlinSize = 1f;

        public int2 AreaMin = new (-1, -1), AreaMax = new (11, 11);

        public bool GetTile (GameScript GS, RoundScript RS, float diff, float2 pos, int2 id, out TileBase tile) {

            tile = null;

            // Check if perlin fits
            float perlin = GS.FixedPerlinNoise(pos.x + PerlinShift, pos.y + PerlinShift);
            if (perlin < PerlinMargins.x || perlin > PerlinMargins.y) {
                return false;
            }
            
            // Check if area fits
            if (id.x < AreaMin.x || id.x > AreaMax.x || id.y < AreaMin.y || id.y > AreaMax.y) {
                return false;
            }
            
            // Prepare tickets
            List<TileBase> tickets = new ();
            for (int b = 0; b < Tiles.Length; b++) {
                int amount = (int)Mathf.Lerp(Tiles[b].Chances[0], Tiles[b].Chances[1], diff);
                for (int t = 0; t < amount; t++)
                    tickets.Add(Tiles[b].TileObject);
            }

            // Pick a ticket
            if (tickets.Count > 0) {
                tile = tickets[Random.Range(0, tickets.Count)];
                return true;
            } else {
                return false;
            }
            
        }
    }

    [System.Serializable]
    public struct Tile {
        public TileBase TileObject;
        public int2 Chances;
    }

    [System.Serializable]
    public struct NoiseMap {
        public Texture2D[] MinMap;
        public Texture2D[] MaxMap;
        public bool FlipLongitude, FlipLatitude;
        public int[] Rotations;
    }

}
