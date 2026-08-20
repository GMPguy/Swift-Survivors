using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random=UnityEngine.Random;

public class LandScript : MonoBehaviour {

    // References
    //public GameObject LandPrefab;
    public Tilemap TerrainTilemap;
    public GameObject TreesPrefab;
    public GameObject PlantsPrefab;
    public GameObject InteractablePrefab;
    public GameObject LandpartHide;
    public GameObject RadioactivityZone;
    List<Transform> TreeChunks;
    List<Spawner> CachedSpawners;
    public List<GameObject> Lands;
    public GameObject Barriers;
    public GameScript GS;
    public RoundScript RS;
    public GameObject MainPlayer;
    public List<GameObject> Waters;
    public GameObject GrassAnchor;
    public GameObject[] Grasses;
    Color32[] GrassColor;
    public BiomeConfig Biome;
    Vector3 PreviousCampos = new Vector3(1000f, 0f, 1000f);
    Color PrevSkyColor;
    // References

    // Activate stuff
    public bool Generated = false;
    public bool Started = false;
    public int ObjectsToSpawn = 0;
    int orgObjectsToSpawn = 0;
    public bool NavmeshBake = false;

	// Use this for initialization
    public void TheStart(BiomeConfig GotTerrain, float difficulty) {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
        MainPlayer = GameObject.FindGameObjectWithTag("Player");

        TreeChunks = new List<Transform>();
        RoundScript.CachedSpawner = new List<Spawner>();

        // Spawn lands
        // GTODO - This code spawns land objects
        if (GS.GameModePrefab.x == 0) {
            Biome = GotTerrain;
            Lands = new List<GameObject>();

            // Spawn tiles
            int2 tiles = new ((int)(Biome.WorldSize.x / Biome.TileSize.x), (int)(Biome.WorldSize.y / Biome.TileSize.y));
            int2 monument = new (Random.Range(0, tiles.x), Random.Range(0, tiles.y));
            for (int Z = 0; Z < tiles.y; Z++)
                for (int X = 0; X < tiles.x; X++) {
                    float2 pos = new (Z * Biome.WorldSize.x, X * Biome.WorldSize.y);
                    TileBase theTile = Biome.FallbackTile;

                    if (X == monument.x && Z == monument.y && Biome.MonumentTile != null && Biome.MonumentTile.Length > 0f)
                        theTile = Biome.MonumentTile[Random.Range(0, Biome.MonumentTile.Length)];
                    else
                        for (int t = 0; t < Biome.TileBanks.Length; t++)
                            if (Biome.TileBanks[t].GetTile(GS, RS, RS.DifficultySliderA, pos, new (X, Z), out TileBase newTile)) {
                                theTile = newTile;
                                break;
                            }

                    TerrainTilemap.SetTile(new (X, Z, 0), theTile);
                }
            
            // Cache tiles to lands
            for (int gc = 0; gc < TerrainTilemap.transform.childCount; gc++)
                Lands.Add(TerrainTilemap.transform.GetChild(gc).gameObject);

            TerrainTilemap.transform.position -= new Vector3(Biome.WorldSize.x /2f, 0f, Biome.WorldSize.y / 2f);

            if (Biome.BiomeName[0] == "Snowy Area") {
                GrassColor = new Color32[] { new Color32(55, 75, 65, 255), new Color32(135, 155, 145, 255) };
            } else if (Biome.BiomeName[0] == "Sea") {
                GrassColor = new Color32[] { new Color32(0, 55, 0, 255), new Color32(0, 155, 0, 255) };
            } else {
                GrassColor = Biome.GrassColor;
            }

            // Spawn radioactivity zone
            List<Vector4> zones = new();
            for (int rz = 0; rz < 20; rz++) {
                // Pick new zone
                for (int at = 0; at < 10; at++) {
                    float2 zoneSize = new (Random.Range(.05f, .25f) * Biome.WorldSize.x, Random.Range(.05f, .25f) * Biome.WorldSize.y);

                    Vector4 newZone = new (
                        Random.Range(5f, Biome.WorldSize.x / 2f - zoneSize.x),
                        Random.Range(5f, Biome.WorldSize.y / 2f - zoneSize.y),
                        0f, 0f
                    );

                    for (int nz = 0; nz <= 1; nz++)
                        newZone[2 + nz] = newZone[nz] + zoneSize[nz];

                    // Quarter flip
                    float2 flipX = new (newZone[0] * -1, newZone[2] * -1);
                    float2 flipY = new (newZone[1] * -1, newZone[3] * -1);
                    switch (rz / 5) {
                        case 1:
                            newZone[0] = flipX[1];
                            newZone[2] = flipX[0];
                            break;
                        case 2:
                            newZone[1] = flipY[1];
                            newZone[3] = flipY[0];
                            break;
                        case 3:
                            newZone[0] = flipX[1];
                            newZone[2] = flipX[0];
                            newZone[1] = flipY[1];
                            newZone[3] = flipY[0];
                            break;
                    }

                    // Check overlaping
                    for (int o = 0; o < zones.Count; o++) {
                        Vector4 b = newZone;
                        Vector4 a = zones[o];

                        if (!(a.x > b.z || a.z < b.x || a.y > b.w || a.w < b.y))
                            goto Overlapped;
                    }
                    
                    // All correct
                    float power = Mathf.Lerp(
                        Random.Range(Biome.Radioactivity[0], Biome.Radioactivity[1]),
                        Random.Range(Biome.Radioactivity[2], Biome.Radioactivity[3]),
                        RS.DifficultySliderB
                    );

                    if (power > 0f) {
                        zones.Add(newZone);

                        SpecialScript zoneObj = GameObject.Instantiate(RadioactivityZone).GetComponent<SpecialScript>();

                        zoneObj.transform.position = Vector3.Lerp(
                            new Vector3(newZone.x, 0f, newZone.y),
                            new Vector3(newZone.z, 0f, newZone.w),
                            .5f
                        );

                        zoneObj.transform.localScale = new Vector3(
                            newZone.z - newZone.x,
                            1f,
                            newZone.w - newZone.y
                        );

                        zoneObj.ExplosionRange = power;
                    }

                    break;

                    Overlapped:;
                    continue;
                }
            }
        }
        // Spawn lands

        // Set Lands
        // GTODO - This code sets up lands
        foreach (GameObject LandToSet in Lands)
            SetLand(LandToSet);

        // GTODO - This code picks barrier and escape roots
        SetBarrier(GotTerrain.Barrier);

        // Set Escape Roots
        string WhichWall = "NESW";
        for (int AmountOfTunnels = 5 - Mathf.Clamp((int)(difficulty * 3f), 1, 3); AmountOfTunnels > 0; AmountOfTunnels--) {
            int PickedWall = Random.Range(0, (int)(WhichWall.Length - 1f));
            string WhichWallA = WhichWall.Substring(PickedWall, 1);
            WhichWall = WhichWall.Remove(PickedWall, 1);

            GameObject NewTunnel = Instantiate(InteractablePrefab) as GameObject;
            NewTunnel.GetComponent<InteractableScript>().Variables = new JClass(2, JTemplate.JustID);//new Vector3(2f, 0f, 0f);
            if (WhichWallA == "N") {
                NewTunnel.transform.position = new Vector3(Random.Range(-100f, 100f), 0f, 249f);
                NewTunnel.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            } else if (WhichWallA == "E") {
                NewTunnel.transform.position = new Vector3(249f, 0f, Random.Range(-100f, 100f));
                NewTunnel.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            } else if (WhichWallA == "S") {
                NewTunnel.transform.position = new Vector3(Random.Range(-100f, 100f), 0f, -249f);
                NewTunnel.transform.eulerAngles = new Vector3(0f, -90f, 0f);
            } else if (WhichWallA == "W") {
                NewTunnel.transform.position = new Vector3(-249f, 0f, Random.Range(-100f, 100f));
                NewTunnel.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

        // Set plants
        SetPlants(difficulty);

        ObjectsToSpawn = TreeChunks.Count;
        orgObjectsToSpawn = ObjectsToSpawn;

        Started = true;

    }

    void FixedUpdate() {

        if (GS == null || RS == null) {
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
        } else if (GS.GameModePrefab.x == 0) {

            if (Generated == false && Started == true) {

                // Delayed world object spawn

                if (ObjectsToSpawn > 0) {
                    // Step one - spawn objects
                    
                    // Place trees
                    if(TreeChunks.ToArray().Length > 0)
                    for(int pt = Mathf.Clamp(TreeChunks.ToArray().Length-1, 0, 5); pt >= 0; pt--){
                        Growatree(TreeChunks.ToArray()[pt].position, TreeChunks.ToArray()[pt].transform.parent);
                        Destroy(TreeChunks.ToArray()[pt].gameObject);
                        TreeChunks.RemoveAt(pt);
                        TreeChunks.TrimExcess();
                        ObjectsToSpawn--;
                    }

                    if (ObjectsToSpawn > 0f)
                        NewMenuScript.LoadingAdditionalInfo = GS.SetString(
                            $"Spawning world objects: {orgObjectsToSpawn - ObjectsToSpawn} / {orgObjectsToSpawn}",
                            $"Tworzenie obiektów świata: {orgObjectsToSpawn - ObjectsToSpawn} / {orgObjectsToSpawn}"
                        );
                    else
                        NewMenuScript.LoadingAdditionalInfo = GS.SetString("Baking navigation surfaces", "Tworzenie powierzchni do nawigowania SI");
                } else if (NavmeshBake == false) {
                    // Step two - bake navmesh
                    RS.NavigationSurface_Humanoid.BuildNavMesh();
                    NavmeshBake = true;
                } else {
                    Generated = true;
                }

            } else {

                if (!MainPlayer) {
                    MainPlayer = GameObject.FindGameObjectWithTag("Player");
                    return;
                }

                // Regular world update

                GameObject LandUrStandingOn = null;
                foreach (GameObject FoundLand in Lands) {
                    if ((MainPlayer.transform.position.x > (FoundLand.transform.position.x - 25f) && MainPlayer.transform.position.x < (FoundLand.transform.position.x + 25f)) && (MainPlayer.transform.position.z > (FoundLand.transform.position.z - 25f) && MainPlayer.transform.position.z < (FoundLand.transform.position.z + 25f))) {
                        LandUrStandingOn = FoundLand;
                    }
                }
                if (LandUrStandingOn != null) {
                    if (LandUrStandingOn.name.Substring(0, 1) == "0") {
                        LandUrStandingOn.name = "1" + LandUrStandingOn.name.Substring(1);
                        RS.GetComponent<RoundScript>().SetScore(JType.RoundScore_Stats_MapDiscovered, "/+1");
                    }

                    LandUrStandingOn.transform.GetChild(0).GetComponent<MinimapMarker>().MapSize = 0f;
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
        
    }

    public void SetLand(GameObject Land){

        void MeshColor (MeshRenderer mesh, float x, float z) {
            float2 values = GetNoise(x, z, 0);
            foreach (Material Mat in mesh.materials) {
                if (Mat.name == "Grass1 (Instance)" || Mat.name == "Grass2 (Instance)" || Mat.name == "Grass3 (Instance)") {
                    Mat.color = Color32.Lerp(Biome.GrassColor[0], Biome.GrassColor[1], Random.Range(values.x, values.y));
                } else if (Mat.name == "WoodenFence1 (Instance)") {
                    Mat.color = Color32.Lerp(new Color32(100, 75, 55, 255), new Color32(188, 155, 133, 255), Random.Range(values.x, values.y));
                }
            }
        }

        void SetLandInLand (Transform LandInLand) {
            
            if (LandInLand.name == "Tree"){
                TreeChunks.Add(LandInLand);
            } else if (LandInLand.name == "Building") {
                LandInLand.GetComponent<BuildingSpawnerScript>().SpawnBuilding(RS.DifficultySliderB, Land.transform, LandInLand.transform);
            } else if (LandInLand.name == "Fence") {
                LandInLand.GetComponent<FenceSpawner>().SpawnFence(RS.DifficultySliderB, LandInLand.transform);
            } else if (LandInLand.TryGetComponent<MeshRenderer>(out MeshRenderer mesh)) {
                MeshColor(mesh, LandInLand.transform.position.x, LandInLand.transform.position.z);
            } else if (LandInLand.GetComponent<LODGroup>() || LandInLand.GetComponent<InstantLODScript>()) {
                foreach(Transform child in LandInLand)
                    if (child.TryGetComponent<MeshRenderer>(out MeshRenderer meshB))
                        MeshColor(meshB, LandInLand.transform.position.x, LandInLand.transform.position.z);
            } else if (LandInLand.name == "Water" || LandInLand.name == "DeepWater") {
                bool Freeze = false;
                if (Biome != null) {
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

            // Add bottomlands
            if (LandInLand.name == "Flatland" && RS.Map_Biome.Bottomland) {
                Transform newBottom = GameObject.Instantiate(RS.Map_Biome.Bottomland).transform;
                newBottom.SetParent(LandInLand);
                newBottom.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                foreach(Transform child in newBottom)
                    if (child.TryGetComponent<MeshRenderer>(out MeshRenderer meshB))
                        MeshColor(meshB, LandInLand.transform.position.x, LandInLand.transform.position.z);
            }

        }

        if (Land.TryGetComponent<LandpartScript>(out LandpartScript mainPart))
            mainPart.Setup(this);

        foreach (Transform LandInLand in Land.transform)
            SetLandInLand(LandInLand);

        Land.name = "0";

        Transform HideQuad = Instantiate(LandpartHide).transform;
        HideQuad.SetParent(Land.transform);
        HideQuad.localPosition = Vector3.zero;
        HideQuad.eulerAngles = Vector3.zero;
        HideQuad.SetSiblingIndex(0);
        HideQuad.GetComponent<MinimapMarker>().MapSize = 240 / 10f;

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

    public void SetPlants(float diff) {
        int maxPlants = (int) Mathf.Lerp(Biome.AmountOfPlants[0], Biome.AmountOfPlants[1], diff);

        for (int p = 0; p < maxPlants; p++) {
            for (int a = 100; a > 0; a--) {
                Vector3 plantCheck = new Vector3(
                    Random.Range(-Biome.WorldSize.x / 2f, Biome.WorldSize.x / 2f),
                    100f,
                    Random.Range(-Biome.WorldSize.y / 2f, Biome.WorldSize.y / 2f)
                );

                if (Physics.Raycast(plantCheck, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                    if (hit.collider.TryGetComponent<FootstepMaterial>(out FootstepMaterial ground))
                        if (ground.IsTerrain) {
                            PlantScript newPlant = GameObject.Instantiate(PlantsPrefab).GetComponent<PlantScript>();
                            newPlant.transform.position = hit.point;
                            newPlant.transform.Rotate(Vector3.up * Random.Range(0f, 360f));

                            string plantType = Biome.PlantTypes[Random.Range(0, Biome.PlantTypes.Length)];
                            newPlant.PlantType = plantType;
                            break;
                        }
            }
        }
    }

    public void Growatree(Vector3 here, Transform within, string treetype = default){

        string specificTree = "";
        float randA = Random.value;// GS.SeedPerlin2D("5876364858", here.x, here.y);
        float randB = Random.value;//GS.SeedPerlin2D("1340296748", here.x, here.y);
        float randC = Random.value;//GS.SeedPerlin2D("1068794655", here.x, here.y);
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

    public float2 GetNoise(float x, float z, int map) {
        Texture2D minMap = RS.Map_Biome.Noises[RS.Map_BiomeNoiseMap].MinMap[map];
        Texture2D maxMap = RS.Map_Biome.Noises[RS.Map_BiomeNoiseMap].MaxMap[map];

        x *= RS.Map_BiomeNoiseMapRotation.x;
        z *= RS.Map_BiomeNoiseMapRotation.y;

        float Long = (x / RS.Map_Biome.WorldSize.x) + .5f;
        float Latit = (z / RS.Map_Biome.WorldSize.y) + .5f;

        int intLong = (int)Mathf.Lerp(0, minMap.width, Long);
        int intLatit = (int)Mathf.Lerp(0, minMap.height, Latit);

        return new (
            minMap.GetPixel(intLong, intLatit).r,
            maxMap.GetPixel(intLong, intLatit).r
        );
    }

    public void DrawGrass() {

        float Quality = (float)GameObject.Find("_GameScript").GetComponent<GameScript>().GrassQuality / 4f;
        float Distance = Mathf.Lerp(0f, RS.Map_Biome.Atmospheres[RS.Map_BiomeAtmosphere].FogDistance.y / 3f, Quality);
        int GrassQuality = GameObject.Find("_GameScript").GetComponent<GameScript>().GrassQuality switch {
            0 or 1 => 5,
            _ => 5
        };

        if ((Vector3.Distance(new Vector3(GameObject.Find("MainCamera").transform.position.x, 0f, GameObject.Find("MainCamera").transform.position.z), PreviousCampos) > Distance / 2f) && GS.GameModePrefab.x == 0) {// && RS.GetComponent<RoundScript>().GotTerrain != null && RS.GetComponent<RoundScript>().GotTerrain.GetComponent<BiomeInfo>() != null) {
            PrevSkyColor = GameObject.Find("Sun").GetComponent<Light>().color;
            // Set grasses colors
            foreach (GameObject GetGrass in Grasses) {
                if (GetGrass.GetComponent<MeshRenderer>() != null) {
                    if (Quality <= 0.5f && GetGrass.GetComponent<MeshRenderer>().shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.Off) {
                        GetGrass.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    } else if (Quality > 0.5f && GetGrass.GetComponent<MeshRenderer>().shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.TwoSided) {
                        GetGrass.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                    }
                    foreach (Material GetMat in GetGrass.GetComponent<MeshRenderer>().sharedMaterials) {
                        float LerpValue = Mathf.Clamp(Vector3.Distance(new Vector3(-250f, 0f, -250f), PreviousCampos) / 500f, 0f, 1f);
                        switch (GetMat.name) {
                            case "Grass1":
                                Color SunColor1 = PrevSkyColor;
                                GetMat.color = Color32.Lerp(GrassColor[0], GrassColor[1], 0.25f);
                                GetMat.SetColor("_ReflectColor", Color.Lerp(Color.black, SunColor1, 0.5f));
                                break;
                            case "Grass2":
                                Color SunColor2 = PrevSkyColor;
                                GetMat.color = Color32.Lerp(GrassColor[0], GrassColor[1], 0.5f);
                                GetMat.SetColor("_ReflectColor", Color.Lerp(Color.black, SunColor2, 0.5f));
                                break;
                            case "Grass3":
                                Color SunColor3 = PrevSkyColor;
                                GetMat.color = Color32.Lerp(GrassColor[0], GrassColor[1], 0.75f);
                                GetMat.SetColor("_ReflectColor", Color.Lerp(Color.black, SunColor3, 0.5f));
                                break;
                            case "Leaves1":
                                GetMat.color = Color32.Lerp(new Color32(200, 55, 55, 255), new Color32(255, 225, 0, 255), LerpValue);
                                break;
                            case "Leaves2":
                                GetMat.color = Color32.Lerp(new Color32(0, 200, 100, 255), new Color32(55, 100, 55, 255), LerpValue);
                                break;
                            case "Leaves3":
                                GetMat.color = Color32.Lerp(new Color32(55, 255, 55, 255), new Color32(100, 75, 55, 255), LerpValue);
                                break;
                            case "Wall1":
                                GetMat.color = Color.HSVToRGB(LerpValue, 0.25f, 1f);
                                break;
                            case "Wall2":
                                GetMat.color = Color.HSVToRGB(LerpValue / 2f, 0.25f, 1f);
                                break;
                            case "Wall3":
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
                    if (CheckGrassPosition(GrassQuality, new Vector3(PreviousCampos.x - (Distance * 2.5f) + (GrassX * 5f) + 2.4f, 1000f, PreviousCampos.z - (Distance * 2.5f) + (GrassZ * 5f) + 2.4f), out Vector3 point, out Vector3 normal)) {
                        Vector3 PlantedPos = point;
                        float PerlinA = GS.FixedPerlinNoise(PlantedPos.x / 2f, PlantedPos.z / 2f);
                        float PerlinB = GS.FixedPerlinNoise(PlantedPos.x, PlantedPos.z);

                        float2 grassMargins = GetNoise(PlantedPos.x, PlantedPos.z, 1);
                        grassMargins.x *= Biome.Grasses.Length - .01f;
                        grassMargins.y *= Biome.Grasses.Length - .01f;
                        GameObject ToInstantiante = Biome.Grasses[(int)Mathf.Lerp(grassMargins.x, grassMargins.y, PerlinA)];

                        if (ToInstantiante != null) {
                            GameObject PlantGrass = Instantiate(ToInstantiante) as GameObject;
                            PlantGrass.transform.forward = normal;
                            if (Vector3.Distance(PlantGrass.transform.forward, Vector3.up) < 0.5f) {
                                PlantGrass.transform.position = PlantedPos + new Vector3(Mathf.Lerp(-0.5f, 0.5f, PerlinA), 0f, Mathf.Lerp(-0.5f, 0.5f, PerlinB));
                                PlantGrass.transform.forward = normal;
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

    Vector3[] rayOffset = new Vector3[] {
        Vector3.zero,
        new (-2.5f, 0f, -2.5f),
        new (2.5f, 0f, 2.5f),
        new (-2.5f, 0f, 2.5f),
        new (2.5f, 0f, -2.5f)
    };

    bool CheckGrassPosition (int quality, Vector3 rayCenter, out Vector3 point, out Vector3 normal) {

        Vector3[] Verts = new Vector3 [4];
        Vector3[] Norms = new Vector3 [4];

        point = Vector3.zero;
        normal = Vector3.zero;
        
        for (int cr = 0; cr < quality; cr++) {
            Ray CheckForLand = new Ray(rayCenter + rayOffset[cr], Vector3.down );
            RaycastHit CheckForLandHIT;

            if (Physics.Raycast(CheckForLand, out CheckForLandHIT, Mathf.Infinity)) {
                if (Vector3.Angle(CheckForLandHIT.normal, Vector3.up) > 45f)
                    goto UtterFail;

                if (CheckForLandHIT.collider.GetComponent<FootstepMaterial>() != null && CheckForLandHIT.collider.GetComponent<FootstepMaterial>().IsTerrain == true) {
                    if (cr == 0) {
                        normal = CheckForLandHIT.normal;
                        point = CheckForLandHIT.point;
                    } else {
                        Verts[cr - 1] = CheckForLandHIT.point;
                        Norms[cr - 1] = CheckForLandHIT.normal;
                    }
                } else
                    goto UtterFail;
            } else
                goto UtterFail;
        }

        switch (quality) {
            case 5:

                // Calculate centroid
                point = Vector3.zero;
                for (int c = 0; c < Verts.Length; c++)
                    point += Verts[c];
                point /= Verts.Length;

                // Calculate normal
                normal = Vector3.zero;
                for (int c = 0; c < Norms.Length; c++)
                    normal += Norms[c];
                normal = normal.normalized;
                break;
            case 3:
                Vector3 diff =  Verts[0] - Verts[1];

                normal = -Vector3.Cross(diff, Vector3.right + Vector3.back);
                point = Vector3.Lerp(Verts[0], Verts[1], .5f);
                break;
        }

        return true;

        UtterFail:;
        point = normal = Vector3.zero;
        return false;
    }

}
