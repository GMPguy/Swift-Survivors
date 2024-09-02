using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class LandScript : MonoBehaviour {

    // References
    public GameObject LandPrefab;
    public GameObject TreesPrefab;
    List<Transform> TreeChunks;
    public GameObject[] Lands;
    public GameObject Barriers;
    public GameScript GS;
    public RoundScript RS;
    public GameObject MainPlayer;
    public List<GameObject> Waters;
    public GameObject GrassAnchor;
    public GameObject[] Grasses;
    Color32[] GrassColor;
    public BiomeInfo Biome;
    Vector3 PreviousCampos = new Vector3(1000f, 0f, 1000f);
    public bool Activated = false;
    Color PrevSkyColor;
    // References

	// Use this for initialization

    public void TheStart() {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
        MainPlayer = GameObject.FindGameObjectWithTag("Player");

        TreeChunks = new List<Transform>();

        // Spawn lands
        if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0") {
            Biome = RS.GetComponent<RoundScript>().GotTerrain.GetComponent<BiomeInfo>();
            List<GameObject> LandsToGet = new List<GameObject>();
            for (int SpawnX = 0; SpawnX <= 9; SpawnX++) {
                for (int SpawnZ = 0; SpawnZ <= 9; SpawnZ++) {
                    GameObject SpawnLand = Instantiate(LandPrefab) as GameObject;
                    SpawnLand.transform.SetParent(this.transform);
                    SpawnLand.transform.localPosition = new Vector3(SpawnX * 50f, 0f, SpawnZ * 50f) - new Vector3(225f, 0f, 225f);
                    LandsToGet.Add(SpawnLand.gameObject);
                }
            }
            Lands = LandsToGet.ToArray();
            if (Biome.transform.GetSiblingIndex() == 5) {
                GrassColor = new Color32[] { new Color32(55, 75, 65, 255), new Color32(135, 155, 145, 255) };
            } else if (Biome.transform.GetSiblingIndex() == 8) {
                GrassColor = new Color32[] { new Color32(0, 55, 0, 255), new Color32(0, 155, 0, 255) };
            } else {
                GrassColor = Biome.GrassColor;
            }
        }
        // Spawn lands

        Activated = true;

    }

    void FixedUpdate() {

        if (GS == null || RS == null || MainPlayer == null) {
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
            MainPlayer = GameObject.FindGameObjectWithTag("Player");
        } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0" && Activated == true) {

            GameObject LandUrStandingOn = null;
            foreach (GameObject FoundLand in Lands) {
                if ((MainPlayer.transform.position.x > (FoundLand.transform.position.x - 25f) && MainPlayer.transform.position.x < (FoundLand.transform.position.x + 25f)) && (MainPlayer.transform.position.z > (FoundLand.transform.position.z - 25f) && MainPlayer.transform.position.z < (FoundLand.transform.position.z + 25f))) {
                    LandUrStandingOn = FoundLand;
                }
                if (FoundLand.name.Substring(0, 1) == "0" && FoundLand.name.Substring(2, 1) == "M" && Vector3.Distance(MainPlayer.transform.position, FoundLand.transform.position) < RS.GetComponent<RoundScript>().DetectionRange) {
                    FoundLand.name = "1" + FoundLand.name.Substring(1);
                    GS.Mess(GS.SetString("Monument found!", "Znaleziono monument!"), "Draw");
                    GS.AddToScore(50);
                    // Hint
                    if (!GameObject.Find("MainCanvas").GetComponent<CanvasScript>().HintsTold.Contains("Monument")) {
                        GameObject.Find("MainCanvas").GetComponent<CanvasScript>().HintsCooldown.Add("Monument");
                    }
                }
            }
            if (LandUrStandingOn != null) {
                if (LandUrStandingOn.name.Substring(0, 1) == "0") {
                    LandUrStandingOn.name = "1" + LandUrStandingOn.name.Substring(1);
                    RS.GetComponent<RoundScript>().SetScore("MapDiscovered_", "/+1");
                }
                MainPlayer.GetComponent<PlayerScript>().MicroSiverts[1] = float.Parse(LandUrStandingOn.name.Substring(1, 1));
            }

            // Place trees
            if(TreeChunks.ToArray().Length > 0)
            for(int pt = Mathf.Clamp(TreeChunks.ToArray().Length-1, 0, 25); pt > 0; pt--){
                Growatree(TreeChunks.ToArray()[pt].position, TreeChunks.ToArray()[pt].transform.parent);
                Destroy(TreeChunks.ToArray()[pt].gameObject);
                TreeChunks.RemoveAt(pt);
                TreeChunks.TrimExcess();
            }

            // WaterStuff
            float IsSwimming = 1f;
            if (RS.GetComponent<RoundScript>().IsSwimming[0] == true) {
                IsSwimming = -1f;
            }

            foreach (GameObject Water in Waters) {
                float AoN = Water.transform.GetChild(0).localScale.z / Water.transform.GetChild(0).localScale.z;
                if (Water.GetComponent<BoxCollider>().size.y == 0.01f) {
                    Water.transform.GetChild(0).localScale += new Vector3(0f, 0f, 0.001f * AoN);
                    if(Water.transform.GetChild(0).localScale.z > 1f){
                        Water.GetComponent<BoxCollider>().size = new Vector3(50f, 0.02f, 50f);
                    }
                } else {
                    Water.transform.GetChild(0).localScale -= new Vector3(0f, 0f, 0.001f * AoN);
                    if(Water.transform.GetChild(0).localScale.z <= 0.1f){
                        Water.GetComponent<BoxCollider>().size = new Vector3(50f, 0.01f, 50f);
                    }
                }
                if (IsSwimming == 1f) {
                    Water.transform.GetChild(0).transform.localPosition = new Vector3(0f, 1f, 0f);
                } else if (IsSwimming == -1f) {
                    Water.transform.GetChild(0).transform.localPosition = new Vector3(0f, 0f, 0f);
                }
                if ((Water.transform.GetChild(0).localScale.z > 0f && IsSwimming == -1f) || (Water.transform.GetChild(0).localScale.z < 0f && IsSwimming == 1f)) {
                    Water.transform.GetChild(0).localScale *= -1f;
                }
            }

            DrawGrass();

        }
        
    }

    public void SetLand(GameObject Land, string LandName, int Radioactivity){

        Land.transform.eulerAngles = new Vector3(0f, (int)(GS.GetComponent<GameScript>().FixedPerlinNoise(Land.transform.position.x * GS.GetComponent<GameScript>().LandSeed.x * 0.33f, Land.transform.position.z * GS.GetComponent<GameScript>().LandSeed.y * 0.33f) * 4.9f) * 90f, 0f);

        foreach (Transform FoundLand in Land.transform) {
            if (FoundLand.name.Substring(0, 1) == LandName) {
                FoundLand.gameObject.SetActive(true);

                foreach (Transform LandInLand in FoundLand.transform) {
                    float randomFactor = GS.FixedPerlinNoise(LandInLand.position.x, LandInLand.position.z);
                    if (LandInLand.name == "Tree"){
                        TreeChunks.Add(LandInLand);
                    } else if (LandInLand.GetComponent<MeshRenderer>() != null) {
                        Color WallColor = Color.HSVToRGB(randomFactor, 0.5f, 1f);
                        foreach (Material Mat in LandInLand.GetComponent<MeshRenderer>().materials) {
                            if (Mat.name == "Grass1 (Instance)" || Mat.name == "Grass2 (Instance)" || Mat.name == "Grass3 (Instance)") {
                                Mat.color = Color32.Lerp(Biome.GrassColor[0], Biome.GrassColor[1], Random.Range(0f, 1f));
                            } else if (Mat.name == "HouseOuter1 (Instance)") {
                                Mat.color = WallColor;
                            } else if (Mat.name == "HouseOuter2 (Instance)") {
                                Mat.color = WallColor / 2f;
                            } else if (Mat.name == "HouseInner (Instance)") {
                                Mat.color = Color.HSVToRGB((randomFactor + .5f) % 1f, 0.5f, 0.5f);
                            } else if (Mat.name == "HouseRoof (Instance)") {
                                Mat.color = Color32.Lerp(new Color32(100, 75, 55, 255), new Color32(255, 225, 155, 255), randomFactor);
                            } else if (Mat.name == "WoodenFence1 (Instance)") {
                                Mat.color = Color32.Lerp(new Color32(100, 75, 55, 255), new Color32(188, 155, 133, 255), randomFactor);
                            }
                        }
                    } else if (LandInLand.GetComponent<Spawner>() != null) {
                        LandInLand.GetComponent<Spawner>().Spawn();
                    } else if (LandInLand.name == "Water" || LandInLand.name == "DeepWater") {
                        bool Freeze = false;
                        if (RS.GetComponent<RoundScript>().GotTerrain != null) {
                            if (Biome.BiomeName[0] == "Snowy Area") {
                                Freeze = true;
                            }
                        }
                        if (Freeze == true) {
                            LandInLand.gameObject.layer = 0;
                            LandInLand.gameObject.name = "Ice";
                            LandInLand.localScale = new Vector3(1f, 0.1f, 1f);
                            LandInLand.GetComponent<FootstepMaterial>().WhatToPlay = "Block";
                            LandInLand.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor;
                        } else {
                            Waters.Add(LandInLand.gameObject);
                            LandInLand.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor.r, GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor.g, GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor.b, 0.75f);
                        }
                    }
                }


            } else {
                Destroy(FoundLand.gameObject);
            }
        }

        Land.name = "0" + ((int)Mathf.Clamp(Radioactivity, 0f, 9f)).ToString();
        if (LandName == "1" || LandName == "2" || LandName == "3" || LandName == "4" || LandName == "5" || LandName == "6" || LandName == "7" || LandName == "8" || LandName == "9" || LandName == "0") {
            Land.name += "M";
            RS.GetComponent<RoundScript>().ResetHeight = -75f;
        } else {
            Land.name += "0";
        }

    }

    public void SetBarrier(string BarrierToSet){

        foreach (Transform FoundBarrier in Barriers.transform) {
            if (FoundBarrier.name.Substring(0, 1) == BarrierToSet) {
                FoundBarrier.gameObject.SetActive(true);
                foreach (Material Mat in FoundBarrier.GetComponent<MeshRenderer>().materials) {
                    if (Mat.name == "Grass1 (Instance)" || Mat.name == "Grass2 (Instance)" || Mat.name == "Grass3 (Instance)") {
                        Mat.color = Color32.Lerp(Biome.GrassColor[0], Biome.GrassColor[1], Random.Range(0f, 1f));
                    }
                }
            } else {
                FoundBarrier.gameObject.SetActive(false);
            }
        }

    }

    public void Growatree(Vector3 here, Transform within, string treetype = default){

        string specificTree = "";
        float randA = GS.SeedPerlin("5876364858", here);
        float randB = GS.SeedPerlin("1340296748", here);
        float randC = GS.SeedPerlin("1068794655", here);
        if (treetype == default) {
            List<string> oneofthese = new List<string>();
            switch(Biome.FloraType){
                case "Default":
                    oneofthese.Add("TreeSpruce"); oneofthese.Add("TreeLarch"); oneofthese.Add("TreePine");
                    oneofthese.Add("TreeApple"); oneofthese.Add("TreeOak"); oneofthese.Add("TreeBirch");
                    break;
                case "Conifer":
                    oneofthese.Add("TreeSpruce"); oneofthese.Add("TreeLarch"); oneofthese.Add("TreePine");
                    oneofthese.Add("TreeSpruce"); oneofthese.Add("TreeLarch"); oneofthese.Add("TreeDeadPine");
                    break;
                case "Snow":
                    oneofthese.Add("TreeSpruce"); oneofthese.Add("TreeLarch"); oneofthese.Add("TreePine");
                    oneofthese.Add("TreeDead"); oneofthese.Add("TreeDeadPine");
                    break;
                case "Wasteland":
                    oneofthese.Add("TreeDead"); oneofthese.Add("TreeDeadPine");
                    break;
                case "Palm":
                    oneofthese.Add("TreePalm"); oneofthese.Add("TreeTallPalm"); oneofthese.Add("TreeDead");
                    break;
                case "Swamp":
                    oneofthese.Add("TreeApple"); oneofthese.Add("TreeLarch");
                    oneofthese.Add("TreeDead"); oneofthese.Add("TreeDeadPine");
                    break;
            }
            specificTree = oneofthese.ToArray()[(int)(randA*(oneofthese.ToArray().Length-0.1f))];
        } else {
            specificTree = treetype;
        }

        GameObject NewTree = Instantiate(TreesPrefab) as GameObject;
        NewTree.transform.Rotate(0f, randC * 360f, randC*10f);
        NewTree.transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 2f, Mathf.PerlinNoise(randA, randB));
        if(within != null) NewTree.transform.SetParent(within);
        NewTree.transform.position = here;

        for(int fu = 0; fu < NewTree.transform.childCount; fu++) if (NewTree.transform.GetChild(fu).name == specificTree) {
            NewTree.transform.GetChild(fu).gameObject.SetActive(true);
            Color32[] LeavesColor = new Color32[]{};
            Color32[] BarkColor = new Color32[]{};
            Color32 fLeave, fBark = new Color32(0,0,0,0);
            switch(specificTree){
                case "TreeSpruce": case "TreeLarch":
                    LeavesColor = new Color32[]{
                        new Color32(100, 155, 125, 255),
                        new Color32(55, 133, 44, 255),
                        new Color32(33, 77, 55, 255)
                    };
                    BarkColor = new Color32[]{
                        new Color32(188, 177, 188, 255),
                        new Color32(88, 55, 33, 255),
                        new Color32(155, 111, 88, 255)
                    };
                    break;
                case "TreePine":
                    LeavesColor = new Color32[]{
                        new Color32(100, 200, 100, 255),
                        new Color32(55, 133, 44, 255),
                        new Color32(33, 77, 55, 255)
                    };
                    BarkColor = new Color32[]{
                        new Color32(88, 75, 55, 255),
                        new Color32(200, 177, 188, 255),
                        new Color32(200, 100, 0, 255)
                    };
                    break;
                case "TreeApple": case "TreeOak": case "TreeBirch":
                    LeavesColor = new Color32[]{
                        new Color32(217, 197, 137, 255),
                        new Color32(133, 199, 66, 255),
                        new Color32(0, 100, 0, 255)
                    };
                    BarkColor = new Color32[]{
                        new Color32(183, 133, 133, 255),
                        new Color32(175, 50, 0, 255),
                        new Color32(94, 69, 23, 255)
                    };
                    break;
                case "TreeDead": case "TreeDeadPine":
                    LeavesColor = new Color32[]{
                        new Color32(0, 0, 0, 255),
                        new Color32(0, 0, 0, 255),
                        new Color32(0, 0, 0, 255)
                    };
                    BarkColor = new Color32[]{
                        new Color32(168, 158, 148, 255),
                        new Color32(140, 115, 51, 255),
                        new Color32(88, 88, 33, 255)
                    };
                    break;
                case "TreePalm": case "TreeTallPalm":
                    LeavesColor = new Color32[]{
                        new Color32(55, 100, 55, 255),
                        new Color32(0, 155, 0, 255),
                        new Color32(155, 175, 0, 255)
                    };
                    BarkColor = new Color32[]{
                        new Color32(200, 175, 155, 255),
                        new Color32(181, 118, 18, 255),
                        new Color32(200, 125, 125, 255)
                    };
                    break;
            }

            if(randB > 0.5f) fLeave = Color32.Lerp(LeavesColor[1], LeavesColor[2], (randB-0.5f)*2f);
            else fLeave = Color32.Lerp(LeavesColor[0], LeavesColor[1], (randB)*2f);

            if(randB > 0.5f) fBark = Color32.Lerp(BarkColor[1], BarkColor[2], (randB-0.5f)*2f);
            else fBark = Color32.Lerp(BarkColor[0], BarkColor[1], (randB)*2f);

            foreach(Material pm in NewTree.transform.GetChild(fu).GetComponent<MeshRenderer>().materials){
                switch(pm.name){
                    case "Leaves1 (Instance)": pm.color = Color.Lerp(Color.black, fLeave, 0.9f); break; 
                    case "Leaves2 (Instance)": pm.color = fLeave; break;
                    case "Leaves3 (Instance)": 
                        if(Biome.FloraType != "Snow") pm.color = Color.Lerp(Color.white, fLeave, 0.9f); 
                        else pm.color = Color.white; break;
                    case "Bark1 (Instance)": pm.color = Color.Lerp(Color.black, fBark, 0.5f); break; 
                    case "Bark2 (Instance)": pm.color = fBark; break;
                }
            }

            NewTree.transform.GetChild(fu).SetParent(NewTree.transform.parent);
            Destroy(NewTree.gameObject);
            break;
        }

    }

    public void DrawGrass() {

        float Quality = (float)GameObject.Find("_GameScript").GetComponent<GameScript>().GrassQuality / 4f;
        float Distance = Mathf.Lerp(0f, 25f, Quality);
        if ((PrevSkyColor != GameObject.Find("Sun").GetComponent<Light>().color && Time.time - (int)Time.time >= 0.9f) || Vector3.Distance(new Vector3(GameObject.Find("MainCamera").transform.position.x, 0f, GameObject.Find("MainCamera").transform.position.z), PreviousCampos) > Distance / 2f && RS != null && RS.GetComponent<RoundScript>().GotTerrain != null && RS.GetComponent<RoundScript>().GotTerrain.GetComponent<BiomeInfo>() != null) {
            PrevSkyColor = GameObject.Find("Sun").GetComponent<Light>().color;
            // Set grasses colors
            foreach (GameObject GetGrass in Grasses) {
                if (GetGrass.GetComponent<MeshRenderer>() != null) {
                    if (Quality <= 0.5f && GetGrass.GetComponent<MeshRenderer>().shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.Off) {
                        GetGrass.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    } else if (Quality > 0.5f && GetGrass.GetComponent<MeshRenderer>().shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.TwoSided) {
                        GetGrass.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                    }
                    foreach (Material GetMat in GetGrass.GetComponent<MeshRenderer>().materials) {
                        float LerpValue = Mathf.Clamp(Vector3.Distance(new Vector3(-250f, 0f, -250f), PreviousCampos) / 500f, 0f, 1f);
                        switch (GetMat.name) {
                            case "Grass1 (Instance)":
                                Color SunColor1 = PrevSkyColor;
                                GetMat.color = Color32.Lerp(GrassColor[0], GrassColor[1], 0.25f);
                                GetMat.SetColor("_ReflectColor", Color.Lerp(Color.black, SunColor1, 0.5f));
                                break;
                            case "Grass2 (Instance)":
                                Color SunColor2 = PrevSkyColor;
                                GetMat.color = Color32.Lerp(GrassColor[0], GrassColor[1], 0.5f);
                                GetMat.SetColor("_ReflectColor", Color.Lerp(Color.black, SunColor2, 0.5f));
                                break;
                            case "Grass3 (Instance)":
                                Color SunColor3 = PrevSkyColor;
                                GetMat.color = Color32.Lerp(GrassColor[0], GrassColor[1], 0.75f);
                                GetMat.SetColor("_ReflectColor", Color.Lerp(Color.black, SunColor3, 0.5f));
                                break;
                            case "Leaves1 (Instance)":
                                GetMat.color = Color32.Lerp(new Color32(200, 55, 55, 255), new Color32(255, 225, 0, 255), LerpValue);
                                break;
                            case "Leaves2 (Instance)":
                                GetMat.color = Color32.Lerp(new Color32(0, 200, 100, 255), new Color32(55, 100, 55, 255), LerpValue);
                                break;
                            case "Leaves3 (Instance)":
                                GetMat.color = Color32.Lerp(new Color32(55, 255, 55, 255), new Color32(100, 75, 55, 255), LerpValue);
                                break;
                            case "Wall1 (Instance)":
                                GetMat.color = Color.HSVToRGB(LerpValue, 0.25f, 1f);
                                break;
                            case "Wall2 (Instance)":
                                GetMat.color = Color.HSVToRGB(LerpValue / 2f, 0.25f, 1f);
                                break;
                            case "Wall3 (Instance)":
                                GetMat.color = Color.HSVToRGB(LerpValue / 3f, 0.25f, 1f);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            PreviousCampos = new Vector3((int)(GameObject.Find("MainCamera").transform.position.x / 5f) * 5f, 0f, (int)(GameObject.Find("MainCamera").transform.position.z / 5f) * 5f);
            GrassAnchor.transform.position = PreviousCampos;
            foreach (Transform cleanGrass in GrassAnchor.transform) {
                Destroy(cleanGrass.gameObject);
            }
            for (int GrassX = 0; GrassX < Distance; GrassX ++) {
                for (int GrassZ = 0; GrassZ < Distance; GrassZ ++) {
                    Ray CheckForLand = new Ray( new Vector3(PreviousCampos.x - (Distance * 2.5f) + (GrassX * 5f) + 2.4f, 1000f, PreviousCampos.z - (Distance * 2.5f) + (GrassZ * 5f) + 2.4f), Vector3.down );
                    RaycastHit CheckForLandHIT;
                    if (Physics.Raycast(CheckForLand, out CheckForLandHIT, Mathf.Infinity)) {
                        if (CheckForLandHIT.collider.GetComponent<FootstepMaterial>() != null && CheckForLandHIT.collider.GetComponent<FootstepMaterial>().IsTerrain == true) {
                            Vector3 PlantedPos = CheckForLandHIT.point;
                            float PerlinA = GS.GetComponent<GameScript>().FixedPerlinNoise(PlantedPos.x * 9.33f * GS.GetComponent<GameScript>().LandSeed.x, PlantedPos.z * 9.33f * GS.GetComponent<GameScript>().LandSeed.y);
                            float PerlinB = GS.GetComponent<GameScript>().FixedPerlinNoise(PlantedPos.x * 9.33f * GS.GetComponent<GameScript>().LandSeed.x, PlantedPos.z * -9.33f * GS.GetComponent<GameScript>().LandSeed.y);
                            GameObject ToInstantiante = RS.GetComponent<RoundScript>().GotTerrain.GetComponent<BiomeInfo>().Grasses[ (int)(PerlinA * (RS.GetComponent<RoundScript>().GotTerrain.GetComponent<BiomeInfo>().Grasses.Length - 0.5f)) ];
                            if (ToInstantiante != null) {
                                GameObject PlantGrass = Instantiate(ToInstantiante) as GameObject;
                                PlantGrass.transform.forward = CheckForLandHIT.normal;
                                if (Vector3.Distance(PlantGrass.transform.forward, Vector3.up) < 0.5f) {
                                    PlantGrass.transform.position = PlantedPos + new Vector3(Mathf.Lerp(-0.5f, 0.5f, PerlinA), 0f, Mathf.Lerp(-0.5f, 0.5f, PerlinB));
                                    PlantGrass.transform.forward = CheckForLandHIT.normal;
                                    PlantGrass.transform.SetParent(GrassAnchor.transform);
                                    PlantGrass.transform.Rotate(new Vector3(0f, 0f, PerlinA * 90f));
                                    PlantGrass.transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.5f, PerlinB);
                                } else {
                                    Destroy(PlantGrass.gameObject);
                                }
                            }
                        }
                    }
                }
            }

        }

    }

}
