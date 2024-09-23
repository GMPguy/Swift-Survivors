using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class RoundScript : MonoBehaviour {

    // Round Variables
    public string RoundState = "Prepeare";
    public float RoundTime = 0f;
    public int[] HordeVariables = new int[] { 8, 2, 0, 0 };   // Total Amount - At Once - Power - Special Mutant Chance
    public int HordeAmount = 0;
    public bool IsCausual = false;
    public string TempStats = "";
    // References
    public GameObject PlayerPrefab;
    public GameObject ItemPrefab;
    public string[] GunRecoilAndSpreadValues;
    public int[] TotalItems;
    public int[] FoodItems;
    public int[] Utilities;
    public int[] HealingItems;
    public int[] Weapons;
    public int[] AmmoItems;
    public int[] AttachmentItems;
    public GameObject InteractablePrefab;
    public GameObject MobPrefab;
    public GameObject MobPHprefab;
    public GameObject SpecialPrefab;
    // Attacks
    int AttackQuerryID = 0;
    GameObject[] AttackQuerry = new GameObject[10];
    public GameObject GeneralAttack;
    public GameObject InvisibleAttack;
    public GameObject GunAttack;
    public AudioClip[] GunfireSounds;

    public List<Vector3> FragElements;
    GameObject FragTransform;
    // Attacks
    // Effects
    int EffectQuerryID = 0;
    GameObject[] EffectQuerry = new GameObject[25];
    public GameObject EffectPrefab;
    // Effects
    public GameObject MainTerrain;
    public PlayerScript MainPlayer;
    public GameScript GS;
    public CanvasScript CS;
    public GameObject BiomeList;
    public GameObject HordeMapList;
    public GameObject GotTerrain;
    public GameObject SkyboxObj;
    public Texture[] SkyboxImages;
    public MobPH[] MobPHeses;
    int currentMobPHscan = 0;
    public List<DestructionScript> ActiveDestructs;
    public List<BuildingScript> ActiveBuildings;
    // Ambiences
    public GameObject[] Ambients;
    public GameObject[] Musics;
    public GameObject NukeAlarm;
    public float DrawDistance = 0f;
    // Ambiences
    // References

    // Nuke variables
    public GameObject NukeObj;
    public float NukeDistance = 0f;
    public Vector3 NukePosition;
    // Nuke variables

    public float DetectionRange = 0f;
    public float DifficultySliderA = 0f;
    public float DifficultySliderB = 0f;
    public float Sunnyness = 1f;
    public float[] TimeOfDay = {0, 0, 0}; // Time of day, hour (is displayed in minutes), dynamic time change
    public int Weather = 0;
    public float[] DefCST = {0f, 0f, 0f};
    public Color32 DefPPC;
    float LightningBang = 5f;
    float AltilleryFire = 5f;
    public int TealState = 0;
    public bool[] IsSwimming = new bool[] { false, false };
    float HordeSpawnCooldown = 0f;
    int Odds = 2;
    bool SetShops = true;
    public float ResetHeight = -5f;
    float SwimDepth = 0f;

    bool DonePrepare = false;
    public bool SunHidden = false;
    bool DynamicDone = false;

    float CountSecond = 1f;
    // public float Sunnyness = 1f;

	// Use this for initialization
	void Start () {

        if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        if(GameObject.Find("MainCanvas")) CS = GameObject.Find("MainCanvas").GetComponent<CanvasScript>();

        if (GS.WhatOnStart == 0 || GS.WhatOnStart == -1) {
            GS.Score = 0;
            GS.Money = 0;
            GS.Ammo = 0;
            if (GS.WhatOnStart != -1) {
                GS.Round = 1;
                GS.Biome = 1;
            }
            GS.RoundSeed = Random.Range(10000000, 99999999).ToString();
            GS.HealthSave = new Vector2(100f, 100f);
            GS.HungerSave = new Vector2(240f, 240f);
            GS.PlayerSpeed = 5f;
            GS.MaxInventory = 4;
            GS.PlayerBuffs = "";
        }

        ActiveDestructs = new List<DestructionScript>();
        ActiveBuildings = new List<BuildingScript>();
        FragElements = new List<Vector3>();
        FragTransform = new ("FragTransform");

        switch (GS.GetSemiClass(GS.RoundSetting, "G", "?")) {
            case "0": 
                GS.GameModePrefab = new (0, 0);
                break;
            case "1": 
                GS.GameModePrefab = new (1, 0);
                break;
            case "2": 
                GS.GameModePrefab = new (0, 1);
                IsCausual = true;
                break;
        }

        SetItemArrays();
        GS.setItemData(IsCausual);
    }
	
	// Update is called once per frame
	void Update () {

        if (GS == null) {
            if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        } else {

            DetectionRange = GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane * 1.25f;

            if (GameObject.FindGameObjectWithTag("Player") != null) {
                IsSwimming[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().IsSwimming;
                if (IsSwimming[0] == true && IsSwimming[1] == false) {
                    IsSwimming[1] = true;
                }
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().IsSwimming == true) {
                    SwimDepth = Mathf.Clamp(GameObject.Find("MainCamera").transform.position.y / -25f, 0f, 1f);
                } else {
                    SwimDepth = 0f;
                }
            } else {
                IsSwimming = new bool[] { false, false };
            }

            // Manage destructors
            if(ActiveDestructs.ToArray().Length >= 1f){
                for(int ad = 0; ad < ActiveDestructs.ToArray().Length; ad++) if(ActiveDestructs[ad]) ActiveDestructs[ad].Do();
            }

            // Manage buildings
            if(ActiveBuildings.ToArray().Length >= 1f){
                for(int ad = 0; ad < ActiveBuildings.ToArray().Length; ad++) if(ActiveBuildings[ad]) ActiveBuildings[ad].Do();
            }

            // Fragment elements
            if(FragElements.ToArray().Length >= 1f && Time.time > 0f){
                for (int shoot = Mathf.Clamp(2, 0, FragElements.ToArray().Length); shoot > 0; shoot--) {
                    int fragID = FragElements.ToArray().Length;
                    string[] Ids = new string[]{"Flintlock", "BakerRifle", "NockGun", "Garand", "Revolver", "M4", "Scar", "AK-47"};
                    int Yturn = fragID % 8;
                    int Xturn = fragID % 32 / 8;
                    FragTransform.transform.position = FragElements[0] + new Vector3(Random.Range(-1f, 1f), Random.Range(0.25f, 1f), Random.Range(-1f, 1f));
                    FragTransform.transform.eulerAngles = new Vector3((Xturn * -11) + Random.Range(-5f, 5f), (Yturn * 45f) + Random.Range(-22f, 22f), 0f);
                    GameObject.Find("_RoundScript").GetComponent<RoundScript>().Attack(new string[]{Ids[(int)Random.Range(0f, 7.9f)], "CanHurtSelf", "Power100;"}, FragTransform.transform.position, FragTransform.transform.forward);
                    FragElements.RemoveAt(0);
                    FragElements.TrimExcess();
                }
            }

            // Count survived time
            if (RoundState == "Normal" || RoundState == "Nuked" || RoundState == "BeforeWave" || RoundState == "HordeWave"){
                if (CountSecond > 0f) CountSecond -= Time.deltaTime;
                else {
                    CountSecond = 1f;
                    SetScore("SurvivedTime_", "/+1");
                }
            }

            // Timeofday[1] is hour, but if it changes, an AmbientSet must be called, for the visual changes to set in
            if(RoundState == "Normal" || RoundState == "Nuked"){
                TimeOfDay[1] += Time.deltaTime/2f;
                if(TimeOfDay[1] > TimeOfDay[2] + 1f){
                    TimeOfDay[2] = TimeOfDay[1] + 1f;
                    if(GS.SkyboxType == 2) AmbientSet("Normal");
                }

                if(MobPHeses.Length > 0) {
                    if (MobPHeses[currentMobPHscan]) MobPHeses[currentMobPHscan].DoAnUpdate();
                    currentMobPHscan = (currentMobPHscan+1) % MobPHeses.Length;
                }
            }
            if(Input.GetKey(KeyCode.T)){
                TimeOfDay[1] += 1;
                if(TimeOfDay[1] > 1440) TimeOfDay[1] = 0; 
                if(GS.SkyboxType == 2) AmbientSet("Normal");
            }

            if (RoundState == "Prepeare" && DonePrepare == false) {

                DonePrepare = true;
                DifficultySliderA = Mathf.Clamp(GS.Round / (30f - (int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?")) * 5f)), 0f, 1f);
                if (IsCausual)
                    DifficultySliderB = GS.Round > 1 ? GS.SeedPerlin(GS.RoundSeed) : .1f;
                else
                    DifficultySliderB = DifficultySliderA;

                // Set Terrain Stuff
                if (GS.GameModePrefab.x == 0) {

                    foreach (Transform FindBiome in BiomeList.transform) {
                        if (FindBiome.transform.GetSiblingIndex() == GS.Biome) {
                            GotTerrain = FindBiome.gameObject;
                        }
                    }
                    if (GotTerrain == null) {
                        GotTerrain = BiomeList.transform.GetChild(1).gameObject;
                    }

                    if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "1") {
                        RoundTime = 300f;
                    } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "2") {
                        RoundTime = Mathf.Clamp(300f - (DifficultySliderA * 180f), 120f, 300f);
                    } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "3") {
                        RoundTime = Mathf.Clamp(240f - (DifficultySliderA * 120f), 120f, 240f);
                    } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "4") {
                        RoundTime = Mathf.Clamp(180f - (DifficultySliderA * 60f), 120f, 240f);
                    } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "5") {
                        RoundTime = 120f;
                    }

                    TimeOfDay[0] = GS.Round;
                    TimeOfDay[0] -= (GS.Round / 4) * 4;
                    //print("The round is " + GS.Round + ", minus the repeat " + (GS.Round / 4) + ", round minus repeat is " + (GS.Round - (GS.Round / 4)) + ", and so the results should be " + TimeOfDay[0]);
                    switch(TimeOfDay[0]){
                        case 0: 
                            TimeOfDay[1] = (int)(60f * Mathf.Lerp(0f, 3f, GS.SeedPerlin(GS.RoundSeed)));
                            TimeOfDay[1] = (int)Mathf.Clamp(TimeOfDay[1], 0f, (3f * 60f) - (RoundTime / 2f));
                            break;
                        case 1: 
                            TimeOfDay[1] = (int)(60f * Mathf.Lerp(7f, 12f, GS.SeedPerlin(GS.RoundSeed)));
                            TimeOfDay[1] = (int)Mathf.Clamp(TimeOfDay[1], 7f * 60f, (12f * 60f) - (RoundTime / 2f));
                            break;
                        case 2: 
                            TimeOfDay[1] = (int)(60f * Mathf.Lerp(12f, 18f, GS.SeedPerlin(GS.RoundSeed))); 
                            TimeOfDay[1] = (int)Mathf.Clamp(TimeOfDay[1], 12f * 60f, (18f * 60f) - (RoundTime / 2f));
                            break;
                        case 3: 
                            TimeOfDay[1] = (int)(60f * Mathf.Lerp(18f, 21f, GS.SeedPerlin(GS.RoundSeed))); 
                            TimeOfDay[1] = (int)Mathf.Clamp(TimeOfDay[1], 18f * 60f, (21f * 60f) - (RoundTime / 2f));
                            break;
                    }

                    if (Weather == -1) {
                        Weather = Mathf.Clamp((int)Mathf.Lerp(-0.5f, 6.5f, GS.SeedPerlin(GS.RoundSeed + "89665776")), 0, 5);
                        if ((GS.Biome == 5 || GS.Round == 1) && (Weather >= 2)) {
                            Weather = 1;
                        }
                    }

                    if (Weather == 0) {
                        Sunnyness = Mathf.Lerp(0.75f, 1f, GS.SeedPerlin(GS.RoundSeed + "23455543"));
                    } else if (Weather == 1) {
                        Sunnyness = Mathf.Lerp(0.5f, 0.75f, GS.SeedPerlin(GS.RoundSeed + "23455543"));
                    } else if (Weather == 2) {
                        Sunnyness = Mathf.Lerp(0.25f, 0.5f, GS.SeedPerlin(GS.RoundSeed + "23455543"));
                    } else if (Weather == 3) {
                        Sunnyness = Mathf.Lerp(0f, 0.25f, GS.SeedPerlin(GS.RoundSeed + "23455543"));
                    } else if (Weather == 4) {
                        Sunnyness = 0f;
                    } else if (Weather == 5) {
                        Sunnyness = 0f;
                    }

                    AmbientSet("Normal");
                    MainTerrain.GetComponent<LandScript>().TheStart();

                    // Set Monuments
                    int MonumentsToSpawn = (int)Mathf.Lerp(0f, 2.9f, GS.SeedPerlin(GS.RoundSeed + "1233"));
                    for (int SetMonuments = MonumentsToSpawn; SetMonuments > 0; SetMonuments--) {
                        int PickMonument = (int)Mathf.Lerp(0f, 9.9f, GS.SeedPerlin2D(GS.RoundSeed, SetMonuments / 3f, SetMonuments / 3f));
                        if ((PickMonument == 9 || PickMonument == 2) && GotTerrain.GetComponent<BiomeInfo>().BiomeName[0] == "Battleground") {
                            PickMonument = 0;
                        }
                        MainTerrain.GetComponent<LandScript>().SetLand(MainTerrain.GetComponent<LandScript>().Lands[PickMonument], PickMonument.ToString(), 0);
                    }

                    // Set Lands
                    foreach (GameObject LandToSet in MainTerrain.GetComponent<LandScript>().Lands) {
                        if (LandToSet.name.Substring(2, 1) != "M") {
                            float PickTerrain = GS.SeedPerlin2D(GS.RoundSeed, LandToSet.transform.position.x + 1000, LandToSet.transform.position.z + 1000);
                            //print(PickTerrain);
                            string PickBiomeAvailableTerrains = GotTerrain.GetComponent<BiomeInfo>().AvailableTerrainTypes[(int)(3f * DifficultySliderB)];
                            Vector2 RadioactivityRange = new Vector2(Mathf.Lerp(GotTerrain.GetComponent<BiomeInfo>().Radioactivity[0], GotTerrain.GetComponent<BiomeInfo>().Radioactivity[2], DifficultySliderB), Mathf.Lerp(GotTerrain.GetComponent<BiomeInfo>().Radioactivity[1], GotTerrain.GetComponent<BiomeInfo>().Radioactivity[3], DifficultySliderB));
                            MainTerrain.GetComponent<LandScript>().SetLand(LandToSet, PickBiomeAvailableTerrains.Substring((int)Mathf.Clamp(PickTerrain * (PickBiomeAvailableTerrains.Length), 0f, PickBiomeAvailableTerrains.Length - 1f), 1), (int)Mathf.Lerp(RadioactivityRange.x, RadioactivityRange.y, PickTerrain));
                        }
                    }
                    MainTerrain.GetComponent<LandScript>().SetBarrier(GotTerrain.GetComponent<BiomeInfo>().Barrier);

                    // Spawn mobs
                    List<MobPH> newMobPHs = new List<MobPH>();
                    for(int spawnMPH = (int)Mathf.Lerp(GotTerrain.GetComponent<BiomeInfo>().AmountOfMobs[0], GotTerrain.GetComponent<BiomeInfo>().AmountOfMobs[1], DifficultySliderB); spawnMPH > 0; spawnMPH--){
                        GameObject SpawnMobPH = Instantiate(MobPHprefab) as GameObject;
                        SpawnMobPH.GetComponent<MobPH>().spawnType = GotTerrain.GetComponent<BiomeInfo>().MobPHsuggestion;
                        SpawnMobPH.GetComponent<MobPH>().DifficultyLevel = DifficultySliderB;
                        newMobPHs.Add(SpawnMobPH.GetComponent<MobPH>());
                    }
                    MobPHeses = newMobPHs.ToArray();
                    // Spawn mobs

                    // Set Escape Roots
                    string WhichWall = "NESW";
                    for (int AmountOfTunnels = 5 - Mathf.Clamp((int)(DifficultySliderB * 3f), 1, 3); AmountOfTunnels > 0; AmountOfTunnels--) {
                        int PickedWall = Random.Range(0, (int)(WhichWall.Length - 1f));
                        string WhichWallA = WhichWall.Substring(PickedWall, 1);
                        WhichWall = WhichWall.Remove(PickedWall, 1);

                        GameObject NewTunnel = Instantiate(InteractablePrefab) as GameObject;
                        NewTunnel.GetComponent<InteractableScript>().Variables = new Vector3(2f, 0f, 0f);
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
                    // Set Escape Roots

                    // Clean from too much
                    if (GameObject.FindGameObjectsWithTag("Item").Length > 150) {
                        print(GameObject.FindGameObjectsWithTag("Item").Length - 150 + " Items removed");
                        for (int TooClean = GameObject.FindGameObjectsWithTag("Item").Length - 150; TooClean > 0; TooClean--) {
                            GameObject ItemToDestroy = GameObject.FindGameObjectsWithTag("Item")[(int)Random.Range(0f, GameObject.FindGameObjectsWithTag("Item").Length - 0.1f)];
                            if (GS.GetSemiClass(ItemToDestroy.GetComponent<ItemScript>().Variables, "id") == "990") {
                                Destroy(ItemToDestroy);
                            }
                        }
                    }
                    if (GameObject.FindGameObjectsWithTag("Interactable").Length > 25) {
                        print(GameObject.FindGameObjectsWithTag("Interactable").Length - 25 + " Interactables removed");
                        for (int TooClean = GameObject.FindGameObjectsWithTag("Interactable").Length - 25; TooClean > 0; TooClean--) {
                            GameObject InteractableToDestroy = GameObject.FindGameObjectsWithTag("Interactable")[(int)Random.Range(0f, GameObject.FindGameObjectsWithTag("Interactable").Length - 0.1f)];
                            if (InteractableToDestroy.GetComponent<InteractableScript>().Variables.x != 2f) {
                                Destroy(InteractableToDestroy);
                            }
                        }
                    }
                    // Clean from too much

                    // Set Terrain Stuff

                    // Set nuke options
                    int PickNukePos = (int)Random.Range(1f, 4.9f);
                    if (PickNukePos == 1) {
                        NukePosition = new Vector3(400f, 0f, 400f);
                    } else if (PickNukePos == 2) {
                        NukePosition = new Vector3(-400f, 0f, 400f);
                    } else if (PickNukePos == 3) {
                        NukePosition = new Vector3(-400f, 0f, -400f);
                    } else if (PickNukePos == 4) {
                        NukePosition = new Vector3(400f, 0f, -400f);
                    }
                    NukeObj.transform.LookAt(new Vector3(0f, NukeObj.transform.position.y, 0f));
                    NukeObj.transform.position = NukePosition;
                    // Set nuke options

                    RoundState = "Normal";

                } else if (GS.GameModePrefab.x == 1) {

                    foreach (Transform SetMap in HordeMapList.transform) {
                        if (int.Parse(SetMap.name.Substring(0, 2)) == int.Parse(GS.GetSemiClass(GS.RoundSetting, "H", "?"))) {
                            SetMap.gameObject.SetActive(true);
                            GotTerrain = SetMap.gameObject;
                        }
                    }

                    //Sunnyness = GotTerrain.GetComponent<MapInfo>().Sunnyness;
                    //SkyColor = GotTerrain.GetComponent<MapInfo>().SkyColors[0];
                    //DrawDistance = GotTerrain.GetComponent<MapInfo>().FogDistances[0];
                    //AmbientColor = GotTerrain.GetComponent<MapInfo>().AmbientColors[0];
                    //SunColor = GotTerrain.GetComponent<MapInfo>().LightColors[0];

                    AmbientSet("Horde");

                    RoundState = "BeforeWave";
                    RoundTime = 60f;

                }


                // Set Player Stuff
                GameObject NewPlayer = Instantiate(PlayerPrefab) as GameObject;
                NewPlayer.transform.position = new Vector3(1f, 3f, 1f);
                MainPlayer = NewPlayer.GetComponent<PlayerScript>();
                switch(GS.GetSemiClass(GS.RoundSetting, "D", "?")){
                    case "1": MainPlayer.FoodLimits = new float[]{0.25f, 0.5f}; break;
                    case "2": MainPlayer.FoodLimits = new float[]{0.33f, 0.66f}; break;
                    case "3": MainPlayer.FoodLimits = new float[]{0.5f, 0.75f}; break;
                    case "4": case "5": MainPlayer.FoodLimits = new float[]{0.5f, 0.99f}; break;
                }
                if (GS.GetComponent<GameScript>().WhatOnStart == 1) {
                    // Load
                    MainPlayer.RS = this;
                    MainPlayer.GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
                    MainPlayer.Health[0] = GS.HealthSave.x;
                    MainPlayer.Health[1] = GS.HealthSave.y;
                    MainPlayer.Food[0] = GS.HungerSave.x;
                    MainPlayer.Food[1] = GS.HungerSave.y;
                    MainPlayer.InventoryFunctions(GS.PlayerInventory);
                    MainPlayer.EquipmentFunctions(GS.PlayerEquipment);
                    MainPlayer.MaxInventorySlots = GS.MaxInventory;
                    MainPlayer.Buffs(GS.PlayerBuffs);
                    MainPlayer.Speed = GS.PlayerSpeed;

                    // Adding hunger
                    if(MainPlayer.Food[0] < MainPlayer.Food[1] * MainPlayer.FoodLimits[0]){
                        MainPlayer.Food[0] = 0;
                        MainPlayer.Food[1] *= 0.5f;
                    } else if(MainPlayer.Food[0] < MainPlayer.Food[1] * MainPlayer.FoodLimits[1]){
                        MainPlayer.Food[0] = 0;
                        MainPlayer.Food[1] *= 1.25f;
                    } else if(MainPlayer.Food[0] <= MainPlayer.Food[1]){
                        MainPlayer.Food[0] = 0;
                        MainPlayer.Food[1] *= 1.5f;
                    } else if(MainPlayer.Food[0] > MainPlayer.Food[1]){
                        MainPlayer.Food[0] -= MainPlayer.Food[1];
                        MainPlayer.Food[1] *= 2f;
                    }

                    if(MainPlayer.Food[0] > MainPlayer.Food[1]) GS.PS.AchProg("Ach_Dinner", "0");

                    MainPlayer.FoodLimits = new float[]{MainPlayer.Food[1] * MainPlayer.FoodLimits[0], MainPlayer.Food[1] * MainPlayer.FoodLimits[1]};

                } else {
                    MainPlayer.Food = new float[]{0f, 100f};
                    MainPlayer.FoodLimits = new float[]{MainPlayer.Food[1] * MainPlayer.FoodLimits[0], MainPlayer.Food[1] * MainPlayer.FoodLimits[1]};

                    if (GS.GameModePrefab.x == 1) {
                        MainPlayer.MaxInventorySlots = 6;
                        GS.Ammo = 100;
                    }
                }
                // Set Player Stuff

            } else if (RoundState == "Normal") {

                if (RoundTime > 0f) {
                    RoundTime -= Time.deltaTime;
                    // Before getting nuked
                    if (RoundTime <= 30f) {
                        if (GS.SkyboxType == 2) {
                            AmbientSet("NormalEnding");
                        }
                    }
                    // Before getting nuked
                } else {
                    RoundState = "Nuked";
                    GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(255, 255, 255, 255), new float[]{1f, 1f});
                    GameObject.Find("MainCanvas").GetComponent<CanvasScript>().PlaySoundBank("S_Nuke");

                    
                    GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = new Color32(255, 255, 255, 255);
                    GameObject.Find("Sun").GetComponent<Light>().intensity = 10f;
                    GameObject.Find("Sun").transform.position = NukeObj.transform.position + Vector3.up * 250f;
                    GameObject.Find("Sun").transform.LookAt(Vector3.zero);
                    AmbientSet("Nuked");

                    MainPlayer.Hurt(Random.Range(0f, 10f), "Nuke", false, NukePosition);
                    foreach (GameObject FleeMob in GameObject.FindGameObjectsWithTag("Mob")) {
                        int Chance = Random.Range(0, 3);
                        if (Chance == 0) {
                            FleeMob.GetComponent<MobScript>().Hurt(9999f, null, true, Vector3.zero, "Nuked");
                        }
                    }

                }

                // Ambient sounds
                foreach (GameObject Ambient in Ambients) {
                    if ((IsSwimming[0] == true && Ambient.name == "UnderwaterAmbience") || (Weather != 4 && Ambient.name == GotTerrain.GetComponent<BiomeInfo>().Ambience && IsSwimming[0] != true) || (Weather == 4 && Ambient.name == "RainingAmbience" && IsSwimming[0] != true)) {
                        if (IsSwimming[0] == true) {
                            Ambient.GetComponent<AudioSource>().volume = 1f;
                        } else {
                            Ambient.GetComponent<AudioSource>().volume = Mathf.Clamp(RoundTime / 60f, 0f, 1f);
                        }
                    } else {
                        Ambient.GetComponent<AudioSource>().volume = 0f;
                    }
                }
                if (RoundTime < 30f && NukeAlarm.GetComponent<AudioSource>().isPlaying == false) {
                    NukeAlarm.GetComponent<AudioSource>().Play();
                    NukeAlarm.transform.position = NukePosition;
                }

                // Mobs fleeing
                if (RoundTime < 30f && RoundTime > 29f) {
                    foreach (GameObject FleeMob in GameObject.FindGameObjectsWithTag("Mob")) {
                        GameObject Exit = null;
                        float NearestExit = Mathf.Infinity;
                        foreach (GameObject AcquireExit in GameObject.FindGameObjectsWithTag("Interactable")) {
                            if (AcquireExit.GetComponent<InteractableScript>().Variables.x == 2f && Vector3.Distance(FleeMob.transform.position, AcquireExit.transform.position) < NearestExit) {
                                Exit = AcquireExit;
                                NearestExit = Vector3.Distance(FleeMob.transform.position, AcquireExit.transform.position);
                            }
                        }
                        if (FleeMob.GetComponent<MobScript>().Curious <= 0f && (FleeMob.GetComponent<MobScript>().TypeOfMob == 2f || FleeMob.GetComponent<MobScript>().TypeOfMob == 3f)) {
                            FleeMob.GetComponent<MobScript>().React("Curious", 30f, Exit.transform.position);
                            if (Vector3.Distance(FleeMob.transform.position, Exit.transform.position) < 5f) {
                                Destroy(FleeMob);
                            }
                        }
                    }
                }

                // Lightning
                if (Weather == 5 && LightningBang > 0f) {
                    LightningBang -= Time.deltaTime;
                } else if (Weather == 5) {
                    LightningBang = Random.Range(5f, 15f);
                    GameObject Bang = Instantiate(SpecialPrefab) as GameObject;
                    Bang.transform.position = new Vector3(Random.Range(-225f, 225f), 100f, Random.Range(-225f, 225f));
                    Bang.GetComponent<SpecialScript>().TypeOfSpecial = "Lightning";
                }

                // Alrillery and Gunfire
                if (AltilleryFire > 0f) {
                    AltilleryFire -= Time.deltaTime;
                } else if (GS.GetComponent<GameScript>().Biome == 10) {
                    AltilleryFire = Random.Range(1f, 3f);
                    //GameObject Alt = Instantiate(AttackPrefab) as GameObject;
                    //Alt.transform.position = new Vector3(Random.Range(-225f, 225f), 50f, Random.Range(-225f, 225f));
                    //Alt.transform.LookAt(this.transform.position + new Vector3(Random.Range(-50f, 50f), -50f, Random.Range(-50f, 50f)));
                    //Alt.GetComponent<AttackScript>().GunName = "Rocket";
                    //Alt.GetComponent<AttackScript>().Slimend = Alt;
                    Attack(new string[]{"Rocket"}, new Vector3(Random.Range(-225f, 225f), 50f, Random.Range(-225f, 225f)), new Vector3(Random.Range(-50f, 50f), -50f, Random.Range(-50f, 50f)));
                }

                // Music
                int TimeOfDay = GS.Round * 6;
                TimeOfDay -= (TimeOfDay / 24) * 24;
                float MusicObscure = Mathf.Clamp(1f - (SwimDepth * 2f), 0f, 1f);
                if (GameObject.Find("MainCanvas").GetComponent<CanvasScript>().DialogedMob != null) {
                    foreach (GameObject Music in Musics) {
                        if (Music.name == "M_Bossa Antigua" && MainPlayer.State == 1) {
                            if (Music.GetComponent<AudioSource>().isPlaying == false) {
                                Music.GetComponent<AudioSource>().Play();
                            }
                        } else {
                            if (Music.GetComponent<AudioSource>().isPlaying == true) {
                                Music.GetComponent<AudioSource>().Stop();
                            }
                        }
                    }
                } else {
                    foreach (GameObject Music in Musics) {
                        if (((Music.name == GotTerrain.GetComponent<BiomeInfo>().Music && TimeOfDay != 0) || (Music.name == "Lightless Dawn" && TimeOfDay == 0)) && MainPlayer.State == 1) {
                            if (Music.GetComponent<AudioSource>().isPlaying == false) {
                                Music.GetComponent<AudioSource>().Play();
                            }
                            Music.GetComponent<AudioSource>().volume = Mathf.Clamp((RoundTime - 30f) / 10f, 0f, 1f) * 0.5f * MusicObscure;
                        } else {
                            if (Music.GetComponent<AudioSource>().isPlaying == true) {
                                Music.GetComponent<AudioSource>().Stop();
                            }
                        }
                    }
                }



            } else if (RoundState == "Nuked") {

                NukeObj.SetActive(true);
                NukeObj.transform.localScale = new Vector3(NukeDistance, 1f, NukeDistance);
                GameObject.Find("Sun").transform.position = NukeObj.transform.position + Vector3.up * 250f;
                GameObject.Find("Sun").transform.LookAt(Vector3.zero);
                NukeDistance += Time.deltaTime * 25f;

                if (MainPlayer.Radioactivity >= -1f && MainPlayer.IsHS == false) {
                    MainPlayer.Radioactivity += Time.deltaTime;
                }

                if (Vector3.Distance(MainPlayer.transform.position, NukePosition) < NukeDistance) {
                    MainPlayer.Hurt(MainPlayer.Health[1], "Nuke", false, NukePosition);
                }
                foreach (GameObject Mob in GameObject.FindGameObjectsWithTag("Mob")) {
                    if (Vector3.Distance(Mob.transform.position, NukePosition) < NukeDistance) {
                        Mob.GetComponent<MobScript>().Hurt(Mob.GetComponent<MobScript>().MobHealth[0], null, true, Vector3.zero, "Nuked");
                    }
                }

                if (GameObject.Find("Sun").GetComponent<Light>().intensity > 1f) {
                    GameObject.Find("Sun").GetComponent<Light>().intensity = Mathf.Lerp(GameObject.Find("Sun").GetComponent<Light>().intensity, 1f, 0.01f);
                    GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = Color32.Lerp(new Color32(75, 25, 0, 255), new Color32(255, 255, 255, 255), (GameObject.Find("Sun").GetComponent<Light>().intensity - 1f) / 9f);
                }

                // Ambient sounds
                foreach (GameObject Ambient in Ambients) {
                    if (Ambient.name == "NukeAmbience") {
                        Ambient.GetComponent<AudioSource>().volume = 1f;
                    } else {
                        Ambient.GetComponent<AudioSource>().volume = 0f;
                    }
                }

                // Music
                foreach (GameObject Music in Musics) {
                    if (Music.name == "Corruption" && MainPlayer.State == 1) {
                        if (Music.GetComponent<AudioSource>().isPlaying == false) {
                            Music.GetComponent<AudioSource>().Play();
                        }
                    } else {
                        if (Music.GetComponent<AudioSource>().isPlaying == true) {
                            Music.GetComponent<AudioSource>().Stop();
                        }
                    }
                }

            } else if (RoundState == "TealState") {

                foreach (GameObject Music in Musics) {
                    if (Music.name == "Almost New" && MainPlayer.State == 1) {
                        if (Music.GetComponent<AudioSource>().isPlaying == false) {
                            Music.GetComponent<AudioSource>().Play();
                        }
                    } else {
                        if (Music.GetComponent<AudioSource>().isPlaying == true) {
                            Music.GetComponent<AudioSource>().Stop();
                        }
                    }
                }

            } else if (RoundState == "BeforeWave") {

                if (RoundTime > 0f) {
                    RoundTime -= Time.deltaTime;
                    if (RoundTime < 5f) {
                        if(GS.SkyboxType == 2) AmbientSet("Horde");
                    } else {
                        CS.MMsafe = 1f;
                    }
                    if (SetShops == true) {
                        SetShops = false;
                        // Clean From Items
                        foreach (GameObject CleanItem in GameObject.FindGameObjectsWithTag("Item")) {
                            Destroy(CleanItem);
                        }
                        foreach (GameObject CleanItem in GameObject.FindGameObjectsWithTag("HordeDrop")) {
                            Destroy(CleanItem);
                        }
                        foreach (RagdollScript CleanItem in GameObject.FindObjectsOfType<RagdollScript>()) {
                            if (CleanItem.GetComponent<RagdollScript>().Freeze <= 0f) {
                                Destroy(CleanItem.gameObject);
                            }
                        }

                        if (GS.GetComponent<GameScript>().Round > 1) {
                            DifficultySliderA = Mathf.Clamp(GS.Round / (55f - (int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?")) * 5f)), 0f, 1f);
                        } else {
                            DifficultySliderA = 0;
                        }
                        float DifficultyDifference = 1f;
                        if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "1") {
                            DifficultyDifference = 0.5f;
                        } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "2") {
                            DifficultyDifference = 1f;
                        } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "3") {
                            DifficultyDifference = 2f;
                        } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "4" || GS.GetSemiClass(GS.RoundSetting, "D", "?") == "5") {
                            DifficultyDifference = 3f;
                        }
                        HordeVariables = new int[] {
                            (int)(Mathf.Lerp(10f, 100f, DifficultySliderA) * DifficultyDifference),
                            (int)(Mathf.Lerp(5f, 30f, DifficultySliderA)),
                            (int)Mathf.Clamp((DifficultySliderA * 3f) * 100f, 0f, 100f),
                            (int)Mathf.Clamp((DifficultySliderA * 3f) * 100f, 0f, 100f)
                        };
                        if (GS.Platform == 2 && HordeVariables[1] > 10f) {
                            HordeVariables[1] = 10;
                        }
                        foreach (GameObject GetInt in GameObject.FindGameObjectsWithTag("Interactable")){
                            if (GetInt.GetComponent<InteractableScript>().Variables.x == 5f) {
                                int pickItem = (int)Mathf.Clamp(GS.SeedPerlin2D(GS.RoundSeed, GetInt.transform.position.x + GS.Round, GetInt.transform.position.y + GS.Round) * GetInt.GetComponent<InteractableScript>().SelectedModel.transform.GetChild(1).childCount, 0f, GetInt.GetComponent<InteractableScript>().SelectedModel.transform.GetChild(1).childCount - 0.1f);
                                if (GS.Round <= 1 && (pickItem == 3 || pickItem == 4 || pickItem == 6 || pickItem == 7 || pickItem == 8)) {
                                    pickItem = 0;
                                }
                                GetInt.GetComponent<InteractableScript>().Interaction("SetItem", pickItem);
                            } else if (GetInt.GetComponent<InteractableScript>().Variables.x == 4f) {
                                for (int AddTradeOptions = 5; AddTradeOptions >= 0; AddTradeOptions--) {
                                    List<int> AvailableOffers = new List<int>();
                                    //int WhichCategory = (int)(Mathf.PerlinNoise(GS.GetComponent<GameScript>().LandSeed.y + GS.GetComponent<GameScript>().Round + (float)AddTradeOptions, GS.GetComponent<GameScript>().LandSeed.y + GS.GetComponent<GameScript>().Round + (float)AddTradeOptions) * 3.9f);
                                    //int WhichCategory = (int)Random.Range(0f, 3.9f);
                                    int WhichCategory =  (int)(Mathf.Clamp(GS.SeedPerlin2D(GS.RoundSeed, (int)GetInt.transform.position.x + GS.Round + AddTradeOptions, (int)GetInt.transform.position.y + GS.Round + AddTradeOptions), 0f, 1f) * 5.9f);
                                    if (WhichCategory <= 2) {
                                        foreach (int GetWeapon in Weapons) {
                                            AvailableOffers.Add(GetWeapon);
                                        }
                                    } else if (WhichCategory == 3) {
                                        foreach (int GetAttachment in AttachmentItems) {
                                            AvailableOffers.Add(GetAttachment);
                                        }
                                    } else if (WhichCategory == 4) {
                                        foreach (int GetHealingItems in HealingItems) {
                                            AvailableOffers.Add(GetHealingItems);
                                        }
                                    } else if (WhichCategory == 5) {
                                        AvailableOffers.Add(80);
                                        AvailableOffers.Add(81);
                                    }
                                    float PickBargain = GS.SeedPerlin2D(GS.RoundSeed, GetInt.transform.position.x + GS.Round + AddTradeOptions, GetInt.transform.position.y + GS.Round + AddTradeOptions);
                                    float PickPrice = GS.SeedPerlin2D(GS.RoundSeed, GetInt.transform.position.y + GS.Round + AddTradeOptions, GetInt.transform.position.x + GS.Round + AddTradeOptions);
                                    GetInt.GetComponent<InteractableScript>().TradeOptions[AddTradeOptions] = AvailableOffers.ToArray()[(int)(PickBargain * AvailableOffers.ToArray().Length - 0.1f)];
                                    GetInt.GetComponent<InteractableScript>().TradePrices[AddTradeOptions] = (int)(Mathf.Lerp(5f, 100f, DifficultySliderA) + (PickPrice * Mathf.Lerp(20f, 750f, DifficultySliderA)));
                                    //GetInt.GetComponent<InteractableScript>().TradeOptions[AddTradeOptions] = AvailableOffers.ToArray()[(int)(Mathf.PerlinNoise(GS.GetComponent<GameScript>().LandSeed.x + GetInt.transform.position.x + GS.GetComponent<GameScript>().Round + AddTradeOptions, GS.GetComponent<GameScript>().LandSeed.y + GetInt.transform.position.y + GS.GetComponent<GameScript>().Round + AddTradeOptions) * AvailableOffers.ToArray().Length - 0.1f)];
                                    //GetInt.GetComponent<InteractableScript>().TradePrices[AddTradeOptions] = (int)(Mathf.PerlinNoise(GS.GetComponent<GameScript>().LandSeed.x + GetInt.transform.position.x + GS.GetComponent<GameScript>().Round + AddTradeOptions, GS.GetComponent<GameScript>().LandSeed.y + GetInt.transform.position.y + GS.GetComponent<GameScript>().Round + AddTradeOptions) * Mathf.Lerp(20f, 750f, DifficultySlider));
                                }
                            }
                        }
                    }
                } else {
                    DifficultySliderA = Mathf.Clamp(GS.Round / (25f - (int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?")) * 2f)), 0f, 1f);
                    HordeAmount = HordeVariables[0];
                    Odds = 2;
                    RoundState = "HordeWave";
                    AmbientSet("Horde");
                }

                // Ambiences
                foreach (GameObject Ambient in Ambients) {
                    if (Ambient.name == GotTerrain.GetComponent<MapInfo>().Ambience) {
                        Ambient.GetComponent<AudioSource>().volume = 1f;
                    } else {
                        Ambient.GetComponent<AudioSource>().volume = 0f;
                    }
                }

                // Music
                foreach (GameObject Music in Musics) {
                    if (Music.name == GotTerrain.GetComponent<MapInfo>().Music && MainPlayer.State == 1) {
                        if (Music.GetComponent<AudioSource>().volume > 0f) {
                            Music.GetComponent<AudioSource>().volume -= Time.deltaTime/10f;
                        }
                    } else {
                        Music.GetComponent<AudioSource>().Stop();
                    }
                }

            } else if (RoundState == "HordeWave") {

                int BAmountOfHorde = 0;
                foreach (GameObject MutantFound in GameObject.FindGameObjectsWithTag("Mob")) {
                    if (MutantFound.GetComponent<MobScript>().State == 0) {
                        BAmountOfHorde += 1;
                    }
                }

                if (HordeSpawnCooldown > 0f) {
                    HordeSpawnCooldown -= Time.deltaTime;
                } else if (HordeAmount > 0 && BAmountOfHorde < HordeVariables[1]) {
                    GameObject NewCustomer = Instantiate(MobPrefab) as GameObject;
                    GameObject CustomerSpawnPoint = GotTerrain.GetComponent<MapInfo>().HordeSpawnPoints[(int)Random.Range(0, GotTerrain.GetComponent<MapInfo>().HordeSpawnPoints.Length - 0.1f)];
                    NewCustomer.transform.position = CustomerSpawnPoint.transform.position;
                    NewCustomer.GetComponent<MobScript>().AiPosition = NewCustomer.transform.position;
                    NewCustomer.GetComponent<MobScript>().CurrentWaypoint = CustomerSpawnPoint;
                    HordeAmount -= 1;
                    if (Odds == 1 && HordeVariables[3] > 0f) {
                        int[] MutantTypes = new int[] { 1, 4, 5, 6, 7, 9, 10, 11, 12, 13 };
                        NewCustomer.GetComponent<MobScript>().TypeOfMob = MutantTypes[(int)Random.Range(0f, MutantTypes.Length - 0.1f)];
                        Odds = (int)Mathf.Round(HordeVariables[0] / (HordeVariables[0] * (HordeVariables[3] / 100f)));
                    } else  {
                        NewCustomer.GetComponent<MobScript>().TypeOfMob = 1;
                        Odds -= 1;
                        
                    }
                    HordeSpawnCooldown = 5.5f - int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?"));
                }

                if (HordeAmount <= 0f && BAmountOfHorde <= 0f) {
                    GS.Round += 1;
                    GS.Money += 10 * GS.Round;
                    GS.AddToScore(10*GS.Round);
                    SetShops = true;
                    RoundTime = 60f;
                    RoundState = "BeforeWave";
                    MainPlayer.InventoryFunctions("");
                    MainPlayer.EquipmentFunctions("");
                    MainPlayer.Buffs("");
                    SpecialEvent("SaveHordeProgress");
                    GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(255, 255, 255, 255), new float[]{4f, 4f});
                    GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                    GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(1).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                    GS.SetText(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(0).GetComponent<Text>(), "Wave survived!", "Fala przeżyta!");
                    GS.SetText(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(1).GetComponent<Text>(), "Money +" + (10 * GS.Round), "Pieniądze +" + (10 * GS.Round));

                    AmbientSet("Horde");

                    for (int RagdollsToClean = (int)Mathf.Clamp(GameObject.FindObjectsOfType<RagdollScript>().Length - 5, 0f, Mathf.Infinity); RagdollsToClean > 0; RagdollsToClean --) {
                        Destroy(GameObject.FindObjectOfType<RagdollScript>().gameObject);
                    }

                }

                // Music
                foreach (GameObject Music in Musics) {
                    if (Music.name == GotTerrain.GetComponent<MapInfo>().Music && MainPlayer.State == 1) {
                        if (Music.GetComponent<AudioSource>().isPlaying == false || Music.GetComponent<AudioSource>().volume <= 0f) {
                            Music.GetComponent<AudioSource>().Play();
                            Music.GetComponent<AudioSource>().time = 0f;
                        }
                    } else {
                        if (Music.GetComponent<AudioSource>().isPlaying == true) {
                            Music.GetComponent<AudioSource>().Stop();
                        }
                    }
                }

            }

            if (TealState == 1) {
                TealState = 2;
                RoundState = "TealState";
                NukeObj.SetActive(false);
                AmbientSet("TealState");
                foreach (GameObject MobRemove in GameObject.FindGameObjectsWithTag("Mob")) {
                    Destroy(MobRemove);
                }
                RoundTime = 0f;
            }

            if (IsSwimming[0] == true && TealState != 2) {

                AmbientSet("Swimming");
                if (QualitySettings.GetQualityLevel() > 0) {
                    MainPlayer.Bubbles.SetActive(true);
                    ParticleSystem.MainModule SetCol = MainPlayer.Bubbles.GetComponent<ParticleSystem>().main;
                    SetCol.startColor = RenderSettings.fogColor;
                }

            } else if (IsSwimming[1] == true && TealState != 2) {

                IsSwimming[1] = false;
                AmbientSet("Normal");
                MainPlayer.Bubbles.SetActive(false);

            }

            // Suns
            Light Sun = null;
            Light SubSun = null;
            if(GameObject.Find("Sun")) {Sun = GameObject.Find("Sun").GetComponent<Light>(); SubSun = GameObject.Find("Sun").transform.GetChild(0).GetComponent<Light>();}
            if (Sun && Sun.transform.GetChild(0) != null) {
                if (QualitySettings.GetQualityLevel() != 0) {
                    bool InShadows = true;
                    if (GameObject.FindGameObjectWithTag("Player") != null) {
                        SubSun.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + (Sun.transform.forward * -1000f);
                        SubSun.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
                    }
                    Ray CheckForShadow = new Ray(SubSun.transform.position, SubSun.transform.forward);
                    RaycastHit CheckForShadowHIT;
                    if (Physics.Raycast(CheckForShadow, out CheckForShadowHIT, Mathf.Infinity)) {
                        if (CheckForShadowHIT.collider.tag == "Player") {
                            InShadows = false;
                        }
                    }
                    SunHidden = InShadows;
                    if (InShadows == false) {
                        if (SubSun.intensity != Sun.intensity || SubSun.color != Sun.color) {
                            SubSun.color = Sun.color;
                            SubSun.intensity = Mathf.MoveTowards(SubSun.intensity, Sun.intensity, 0.06f * (Time.deltaTime * 50f));
                        }
                    } else {
                        if (SubSun.intensity != 0f || SubSun.color != Sun.color) {
                            SubSun.color = Sun.color;
                            SubSun.intensity = Mathf.MoveTowards(SubSun.intensity, 0f, 0.06f * (Time.deltaTime * 50f));
                        }
                    }
                } else if (SubSun.intensity != Sun.intensity || SubSun.color != Sun.color) {
                    SubSun.color = Sun.color;
                    SubSun.intensity = 1f;
                }
            }

        }
		
	}

    void LateUpdate(){

        if(MainPlayer) {
            SkyboxObj.transform.position = MainPlayer.MainCamera.position;
            for(int SC = 0; SC < SkyboxObj.transform.GetChild(0).childCount; SC ++){
                GameObject SkyboxChild = SkyboxObj.transform.GetChild(0).GetChild(SC).gameObject;
                if(SkyboxChild.name == "Fog") {
                    SkyboxChild.transform.eulerAngles = new Vector3(0f, MainPlayer.MainCamera.eulerAngles.y, 0f);
                    SkyboxChild.transform.GetChild(0).GetComponent<SpriteRenderer>().color = RenderSettings.fogColor;
                    SkyboxChild.transform.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.5f, Quaternion.Angle(SkyboxChild.transform.rotation, SkyboxObj.transform.rotation) / 180f), 1f);
                } else if (SkyboxChild.name == "Clouds1" || SkyboxChild.name == "Clouds2"){
                    float CRotation = GS.FixedPerlinNoise(GS.Round + (Time.time / 10000f) + (TimeOfDay[1] / 100f), GS.Round + (Time.time / 10000f)) * 360f;
                    float CAlpha = GS.FixedPerlinNoise(GS.Round + (Time.time / 10000f), GS.Round + (Time.time / 10000f) + (TimeOfDay[1] / 100f)) * 0.75f;
                    if(SkyboxChild.name == "Clouds1"){
                        CRotation = GS.FixedPerlinNoise(GS.Round + (Time.time / 20000f), -GS.Round + (Time.time / 20000f) + (TimeOfDay[1] / 200f)) * 360f;
                        CAlpha = GS.FixedPerlinNoise(-GS.Round + (Time.time / 20000f) + (TimeOfDay[1] / 200f), GS.Round + (Time.time / 20000f)) * 0.75f;
                    }
                    SkyboxChild.transform.localEulerAngles = new Vector3(90f, 0f, CRotation);
                    Color SetAlpha = SkyboxChild.GetComponent<SpriteRenderer>().color;
                    if(SkyboxChild.name == "Clouds2") SetAlpha.a = Mathf.Lerp(1f, 0f, (Sunnyness-0.5f)*4f ) * CAlpha;
                    else if(SkyboxChild.name == "Clouds1") SetAlpha.a = Mathf.Lerp(1f, 0f, Sunnyness * 2f) * CAlpha;
                    SkyboxChild.GetComponent<SpriteRenderer>().color = SetAlpha;
                } else if (SkyboxChild.name == "Sun" || SkyboxChild.name == "Nuke"){
                    if(QualitySettings.GetQualityLevel() != 0){
                        float AltSunnyness = Mathf.Clamp(Sunnyness, 0.25f, 0.5f); 
                        AltSunnyness = Mathf.Clamp(AltSunnyness, 0f, GameObject.Find("Sun").GetComponent<Light>().intensity);
                        float Blinding = 0f;
                        AltSunnyness = (AltSunnyness - 0.25f) / 0.25f;
                        Blinding = Mathf.Clamp((Vector3.Angle(SkyboxChild.transform.forward, GameObject.Find("MainCamera").transform.forward) - 135f) / 45f, 0f, GameObject.Find("Sun").transform.GetChild(0).GetComponent<Light>().intensity);
                        Blinding = Mathf.Clamp(Blinding, 0, (SkyboxChild.transform.eulerAngles.x - 10f) / 20f);
                        Blinding = Mathf.Clamp(Blinding, 0f, SkyboxChild.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a);
                        Color BlindColor = SkyboxChild.transform.GetChild(1).GetComponent<SpriteRenderer>().color;
                        BlindColor.a = AltSunnyness * Blinding;
                        SkyboxChild.transform.GetChild(1).GetComponent<SpriteRenderer>().color = BlindColor;
                        if(SkyboxChild.name == "Nuke") {
                            SkyboxChild.transform.rotation = GameObject.Find("Sun").transform.rotation;
                            SkyboxChild.transform.eulerAngles = new Vector3(25f, SkyboxChild.transform.eulerAngles.y, SkyboxChild.transform.eulerAngles.z);
                        }
                    } else if (SkyboxChild.transform.GetChild(1).GetComponent<SpriteRenderer>().color.a > 0){
                        SkyboxChild.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
                    }
                }
            }
        }

    }

    public void SpecialEvent(string Event){
        if (Event == "Escape") {
            MainPlayer.InventoryFunctions("");
            MainPlayer.EquipmentFunctions("");
            MainPlayer.Buffs("");
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().PauseMenu.LoadingTime = Random.Range(0.5f, 1f);
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().PauseMenu.AfterLoading = "f_EscapeMap";
            //GS.GetComponent<GameScript>().ChangeLevel("EscapeMap");
        } else if (Event == "Reset") {
            MainPlayer.InventoryFunctions("");
            MainPlayer.EquipmentFunctions("");
            MainPlayer.Buffs("");
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().PauseMenu.LoadingTime = Random.Range(0.5f, 1f);
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().PauseMenu.AfterLoading = "f_ResetMap";
            //GS.GetComponent<GameScript>().ChangeLevel("ResetMap");
        } else if (Event == "GameOver") {
            MainPlayer.InventoryFunctions("");
            MainPlayer.EquipmentFunctions("");
            MainPlayer.Buffs("");
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().PauseMenu.LoadingTime = Random.Range(0.5f, 1f);
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().PauseMenu.AfterLoading = "f_GameOver";
        } else if (Event == "SaveHordeProgress") {
            GS.RoundSeed = Random.Range(10000000, 99999999).ToString();
            GS.HealthSave = new Vector2(MainPlayer.Health[0], MainPlayer.Health[1]);
            GS.PlayerSpeed = MainPlayer.Speed;
            GS.PlayerInventory = MainPlayer.InventoryText;
            GS.PlayerEquipment = MainPlayer.EquipmentText;
            GS.MaxInventory = MainPlayer.MaxInventorySlots;
            GS.PlayerBuffs = MainPlayer.BuffsText;
            GS.SaveManipulation(GS.CurrentSave, 0);
        } else if (Event.Substring(0, 3) == "ESC"){

            if(MainPlayer.Food[0] <= 0f && !IsCausual){

                MainPlayer.ReleaseCamera = 0f;
                int SelfEat = (int)Random.Range(1f, MainPlayer.FoodLimits[0]);
                MainPlayer.Hurt(SelfEat, "Canibalism", true, MainPlayer.transform.position);
                MainPlayer.Food[0] += SelfEat;

            } else {

                MainPlayer.State = 0;

                if (!IsCausual) {

                    string[] Punishments = {"0", "01234"};
                    string[] Rewards = {"0", "01234"};

                    if(MainPlayer.Food[0] < MainPlayer.FoodLimits[0]){
                        // Hungry
                        SetScore("Hunger_", "0");
                        Punishments[0] = "1";
                    } else if(MainPlayer.Food[0] < MainPlayer.FoodLimits[1]){
                        // Normal
                        SetScore("Hunger_", "1");
                    } else if(MainPlayer.Food[0] < MainPlayer.Food[1]){
                        // Well fed
                        SetScore("Hunger_", "2");
                        Rewards[0] = "1";
                    } else {
                        // Full
                        SetScore("Hunger_", "3");
                        Rewards[0] = "2";
                    }

                    // Punish
                    for(int Punish = int.Parse(Punishments[0]); Punish > 0; Punish--){
                        int ReceivedPunish = (int)Random.Range(0f, Punishments[1].Length - 0.1f);

                        // Getting punishments
                        switch(Punishments[1].Substring(ReceivedPunish, 1)){
                            case "0":
                                // Lose items
                                SetScore("PItemLost_", "1");
                                int CheckThisOne = Random.Range(0, MainPlayer.MaxInventorySlots);
                                for(int Unlucky = MainPlayer.MaxInventorySlots; Unlucky > 0; Unlucky --){
                                    int offset = CheckThisOne + Unlucky;
                                    if(offset > MainPlayer.MaxInventorySlots) offset -= MainPlayer.MaxInventorySlots;
                                    if(MainPlayer.Inventory[offset] != "id0;") {
                                        MainPlayer.Inventory[offset] = "id0;";
                                        break;
                                    }
                                }
                                break;
                            case "1":
                                // Tired
                                int HowMuch = (int)Random.Range(5f, 25f);
                                MainPlayer.Tiredness = Mathf.Clamp(MainPlayer.Tiredness + HowMuch, 0, 75);
                                SetScore("PTired_", HowMuch.ToString());
                                break;
                            case "2":
                                // Wet
                                MainPlayer.Wet = 100;
                                SetScore("PWet_", "1");
                                break;
                            case "3":
                                // Damaged
                                float HealthToLose = Random.Range(0f, Mathf.Clamp(25f, MainPlayer.Health[0] - 1f, 25f));
                                MainPlayer.Health[0] -= HealthToLose;
                                SetScore("PDamaged_", ((int)HealthToLose).ToString());
                                break;
                            case "4":
                                // NoAmmo
                                for(int LoseAmmo = 0; LoseAmmo < MainPlayer.MaxInventorySlots; LoseAmmo ++){
                                    if(GS.ExistSemiClass(MainPlayer.Inventory[LoseAmmo], "va")) 
                                        MainPlayer.Inventory[LoseAmmo] = GS.SetSemiClass(MainPlayer.Inventory[LoseAmmo], "va", "0");
                                }
                                SetScore("PNoAmmo_", "1");
                                break;
                        }

                        if(ReceivedPunish == Punishments.Length - 1 && Punishments.Length > 1)
                            Punishments[1] = Punishments[1].Substring(0, ReceivedPunish);
                        else if (Punishments.Length > 1)
                            Punishments[1] = Punishments[1].Substring(0, ReceivedPunish) + Punishments[1].Substring(ReceivedPunish + 1);
                        else
                            break;

                    }

                    // Reward
                    for(int Reward = int.Parse(Rewards[0]); Reward > 0; Reward--){
                        int ReceiveReward = (int)Random.Range(0f, Rewards[1].Length - 0.1f);

                        // Getting punishments
                        switch(Rewards[1].Substring(ReceiveReward, 1)){
                            case "0":
                                // Random item
                                int ByThatMuch = (int)Random.Range(1f, 2.9f);
                                SetScore("RItemGot_", ByThatMuch.ToString());
                                break;
                            case "1":
                                // Healed
                                SetScore("RHealed_", "1");
                                MainPlayer.Health[0] = MainPlayer.Health[1];
                                break;
                            case "2":
                                // Adrenalined
                                SetScore("RAdrenalined_", "1");
                                MainPlayer.Adrenaline = 100f;
                                break;
                            case "3":
                                // Treasure
                                SetScore("RTreasure_", "1");
                                break;
                            case "4":
                                // Drunk
                                float DrunkBy = (int)Random.Range(25f, 75f);
                                MainPlayer.Drunkenness = DrunkBy;
                                SetScore("RDrunk_", DrunkBy.ToString());
                                break;
                        }

                        if(ReceiveReward == Rewards.Length - 1 && Rewards.Length > 1)
                            Rewards[1] = Rewards[1].Substring(0, ReceiveReward);
                        else if (Rewards.Length > 1)
                            Rewards[1] = Rewards[1].Substring(0, ReceiveReward) + Rewards[1].Substring(ReceiveReward + 1);
                        else
                            break;

                    }

                }

                MainPlayer.InventoryFunctions("");
                MainPlayer.EquipmentFunctions("");
                MainPlayer.Buffs("");

                RoundState = Event;

            }

        }
    }

    void SetItemArrays() {

        FoodItems = new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 17, 18, 19, 20, 21, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 119, 120, 121, 122, 123 };
        Utilities = new int[] { 2, 11, 12, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 124, 125, 126, 127, 128, 129};
        Weapons = new int[] { 14, 15, 16, 27, 28, 29, 31, 32, 34, 35, 36, 38, 40, 41, 42, 55, 56, 57, 58, 59, 60, 61, 62, 64, 65, 66, 67, 68, 69, 108, 109, 110, 111, 112, 113, 114, 115, 132, 133, 134, 135, 136, 137, 138, 139, 152, 153, 154, 155, 156, 157, 159, 160};
        AmmoItems = new int[] { 30, 33, 37, 39, 63};
        HealingItems = new int[] { 22, 23, 24, 25, 26, 106, 107};
        AttachmentItems = new int[] { 100, 101, 102, 103, 104, 105 };

        List<int> SetTotal = new List<int>();
        foreach (int GiveF in FoodItems) {
            SetTotal.Add(GiveF);
        }
        foreach (int GiveU in Utilities) {
            SetTotal.Add(GiveU);
        }
        foreach (int GiveW in Weapons) {
            SetTotal.Add(GiveW);
        }
        foreach (int GiveA in AmmoItems) {
            SetTotal.Add(GiveA);
        }
        foreach (int GiveH in HealingItems) {
            SetTotal.Add(GiveH);
        }
        foreach (int GiveAT in AttachmentItems) {
            SetTotal.Add(GiveAT);
        }
        TotalItems = SetTotal.ToArray();

        // Gun ID >> WhenStanding (Angle) > WhenWalking (Angle) > WhenwayOff (Angle) >> FiringMax (Angle) > FiringPerShot (Per cent) > RecoilY (Angle) > RecoilX (Angle)
        GunRecoilAndSpreadValues = new string[] {
        "029/010-030-060/040-002/050-050",
        "031/020-040-060/100-002/050-050",
        "032/010-040-060/100-010/450-010",
        "034/005-020-100/020-010/050-025",
        "035/050-060-100/150-010/300-050",
        "036/020-030-040/020-002/015-025",
        "038/010-050-100/100-001/060-060",
        "996/010-050-100/100-001/060-060",
        "040/050-060-100/150-003/300-050",
        "041/030-040-040/020-002/015-025",
        "042/005-030-080/080-001/040-040",
        "055/020-030-040/020-002/015-025",
        "056/005-030-080/040-002/300-010",
        "057/020-060-100/100-001/060-060",
        "058/040-050-050/030-001/020-030",
        "059/002-030-060/060-001/040-040",
        "060/007-040-100/100-001/080-080",
        "061/050-060-100/100-003/450-450",
        "062/010-100-200/020-001/075-100",
        "064/020-100-200/010-001/030-050",
        "065/000-050-100/010-010/300-010",
        "113/015-060-100/000-000/300-050",
        "135/020-050-100/050-001/010-020",
        "137/005-030-080/040-002/020-020",
        "157/030-060-120/040-000/300-050",
        "159/000-030-060/000-000/300-050",
        "160/030-060-120/100-001/060-060",};
    }

    public Vector4 ReceiveGunSpred(int GunID, float MaxSpeed, float Spread) {

        Spread = Mathf.Clamp(Spread, 0f, 1f);
        MaxSpeed = Mathf.Clamp(MaxSpeed, 0f, 2f);

        // x > Received gun spread (Angle)
        // y > gun spread to add (Per cent)
        // z > Recoil Y (Angle.y)
        // w > Recoil X (Angle.x)

        float[] GRASValues = new float[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        foreach (string CheckGRAS in GunRecoilAndSpreadValues) {
            if (int.Parse(CheckGRAS.Substring(0, 3)) == GunID) {
                GRASValues[0] = int.Parse(CheckGRAS.Substring(4, 3)); // When standing
                GRASValues[1] = int.Parse(CheckGRAS.Substring(8, 3)); // When walking
                GRASValues[2] = int.Parse(CheckGRAS.Substring(12, 3)); // When way off
                GRASValues[3] = int.Parse(CheckGRAS.Substring(16, 3)); // Firing max
                GRASValues[4] = int.Parse(CheckGRAS.Substring(20, 3)); // Firing per shot
                GRASValues[5] = int.Parse(CheckGRAS.Substring(24, 3)); // Recoil Y
                GRASValues[6] = int.Parse(CheckGRAS.Substring(28, 3)); // Recoil X
            }
        }

        float SpreadCausedBySpread = 0f;
        float SpreadCausedByMovement = 0f;

        SpreadCausedBySpread = GRASValues[3] * Spread;
        if (MaxSpeed <= 1f) {
            SpreadCausedByMovement = Mathf.Lerp(GRASValues[0], GRASValues[1], MaxSpeed);
        } else {
            SpreadCausedByMovement = Mathf.Lerp(GRASValues[1], GRASValues[2], MaxSpeed - 1f);
        }

        return new Vector4((SpreadCausedBySpread + SpreadCausedByMovement) / 10f, GRASValues[4] / 10f, GRASValues[5] / 10f, GRASValues[6] / 10f);

    }

    public void Effect(string[] Args, Vector3[] Vecs){

        GameObject NewEffect = Instantiate(EffectPrefab) as GameObject;
        NewEffect.transform.position = Vecs[0];
        if(Vecs.Length > 1) NewEffect.transform.eulerAngles = Vecs[1];

    }

    public void Attack(string[] Args, Vector3 From, Vector3 To, GameObject Attacker = null, GameObject Slimend = null, GameObject BulletChamber = null){

        GameObject ThisAttack = GeneralAttack;
        switch(Args[0]){
            case "Flashlight": case "Knife": case  "Bayonet": case  "Crowbar": case  "FireAxe": case  "Machete": case  "BaseballBat": case "SapphireSpear": case "Katana": case "Spear": case "Shovel": case "FryingPan": case "Sledgehammer": case "Plunger": case "Chainsaw": case "MutantBite": case "StrongMutantBite": case "StoneAxe": case "Fokos": case "Sword": case "Pickaxe":
                ThisAttack = InvisibleAttack;
                break;
            case "Colt": case  "Luger": case "Revolver": case "HunterRifle": case "DBShotgun": case "Thompson": case "AK-47": case "Shotgun": case "MP5": case "M4": case "Sten": case "Garand": case "GarandR": case "Famas": case "Uzi": case "G3": case "Scar": case "SPAS": case "SAW": case "Minigun": case "MosinNagant": case "Musket": case "G18": case "M1Carbine": case "Flintlock": case "BakerRifle": case "NockGun":
                ThisAttack = GunAttack;
                break;
            default:
                ThisAttack = GeneralAttack;
                break;
        }
        
        GameObject Attack = Instantiate(ThisAttack) as GameObject;
        Attack.transform.position = From;
        Attack.transform.LookAt(From + To);
        Attack.GetComponent<AttackScript>().GunName = Args[0];
        Attack.GetComponent<AttackScript>().Attacker = Attacker;
        Attack.GetComponent<AttackScript>().Slimend = Slimend;
        Attack.GetComponent<AttackScript>().BulletChamber = BulletChamber;

        // Additional arguments
        bool ClampDP = true;
        for(int ScanArg = 1; ScanArg < Args.Length; ScanArg ++){
            if(GS.ExistSemiClass(Args[ScanArg], "Inventory")) 
                Attack.GetComponent<AttackScript>().WchichItemWasHeld = int.Parse(GS.GetSemiClass(Args[ScanArg], "Inventory"), CultureInfo.InvariantCulture);
            if(GS.ExistSemiClass(Args[ScanArg], "Power")) 
                Attack.GetComponent<AttackScript>().DrunknessPower = float.Parse(GS.GetSemiClass(Args[ScanArg], "Power"), CultureInfo.InvariantCulture);
            if(GS.ExistSemiClass(Args[ScanArg], "GunSpread")) 
                Attack.GetComponent<AttackScript>().SpecialGunSpread = float.Parse(GS.GetSemiClass(Args[ScanArg], "GunSpread"), CultureInfo.InvariantCulture);
            if(GS.ExistSemiClass(Args[ScanArg], "MeleeDurability")) 
                Attack.GetComponent<AttackScript>().MeleeDurability = float.Parse(GS.GetSemiClass(Args[ScanArg], "MeleeDurability"), CultureInfo.InvariantCulture);
            if(Args[ScanArg] == "HideGunFire") 
                Attack.GetComponent<AttackScript>().GunFire = false;
            if(Args[ScanArg] == "Ricochet" || Args[ScanArg] == "Penetration"){
                Attack.GetComponent<AttackScript>().DrunknessPower = Mathf.Clamp(Attack.GetComponent<AttackScript>().DrunknessPower / 2f, 0f, 1f);
                Attack.GetComponent<AttackScript>().PenetrationStatus = Args[ScanArg];
                ClampDP = false;
            }
            if(Args[ScanArg] == "CanHurtSelf") 
                Attack.GetComponent<AttackScript>().CanHurtSelf = true;
            if(Args[ScanArg] == "IsSilenced") 
                Attack.GetComponent<AttackScript>().IsSilenced = true;
        }
        if(ClampDP) Attack.GetComponent<AttackScript>().DrunknessPower = Mathf.Clamp(Attack.GetComponent<AttackScript>().DrunknessPower, 1f, Mathf.Infinity);

        // querrying
        if(AttackQuerry[AttackQuerryID] == null){
            AttackQuerry[AttackQuerryID] = Attack;
        } else {
            Destroy(AttackQuerry[AttackQuerryID]);
            AttackQuerry[AttackQuerryID] = Attack;
        }
        AttackQuerryID = (AttackQuerryID+1)%10;

    }

    public void SetScore(string ScoreType, string Score){

        bool Permission = true;

        if(Permission){
            TempStats = GS.SetSemiClass(TempStats, ScoreType, Score);
            GS.PlaythroughStats = GS.SetSemiClass(GS.PlaythroughStats, ScoreType, Score);
        }

    }

    public void AmbientSet(string WhatSet) {

        bool Default = true;
        bool HasSet = false;
        string WhichSkybox = "";
        if (RoundState == "TealState" || RoundState == "Nuked") {
            Default = false;
        }
        if(WhatSet == "Normal" && GS.GameModePrefab.x == 1) WhatSet = "Horde";

        Color32 FogColor = new Color32(0, 0, 0, 0);
        Color32 SkyColor = new Color32(0, 0, 0, 0);
        Color32 CloudColor = new Color32(0, 0, 0, 0);
        Color32[] SunColors = new Color32[2];
        Color32 AmbientColor = new Color32(0, 0, 0, 0);
        Color32 PostProcessingColor = new Color32(0, 0, 0, 0);
        Vector4 PPV = Vector4.zero;
        float[] ColorLerpValues = new float[]{0f, 0f, 0f};
        Vector3 SunRotation = new Vector3(45f, 45f, 0f);

        float SunPower = Sunnyness;
        SunPower = Mathf.Lerp(0f, 1f, (Sunnyness - 0.2f) / 0.3f);
        if(TimeOfDay[1] > 390 && TimeOfDay[1] < 420f) SunPower *= ((TimeOfDay[1] - 390) / 30f);
        else if(TimeOfDay[1] > 1140 && TimeOfDay[1] < 1200) SunPower *= (1f - ((TimeOfDay[1] - 1140) / 60f));
        else if((TimeOfDay[1] >= 1200 && TimeOfDay[1] <= 1260) || (TimeOfDay[1] >= 360f && TimeOfDay[1] <= 390)) SunPower = 0f;

        if(GotTerrain && (WhatSet == "Normal" || WhatSet == "NormalEnding" || WhatSet == "Swimming")){
            BiomeInfo GitBI = GotTerrain.GetComponent<BiomeInfo>();
            if(Default){
                if(TimeOfDay[1] >= 360f && TimeOfDay[1] <= 480){
                    // Morning NIGHT-DUSK-DAY
                    if(TimeOfDay[1] < 360f) ColorLerpValues = new float[]{2f, 1f, ((float)TimeOfDay[1] - 360f) / 60f};
                    else ColorLerpValues = new float[]{1f, 0f, ((float)TimeOfDay[1] - 360f) / 60f};
                    DrawDistance = 25f + (Sunnyness * 75f);
                    SunRotation = Vector3.Lerp(new Vector3(0, -90, 0f), new Vector3(30f, -45f, 0f), (TimeOfDay[1] - 360f) / 120f);
                } else if (TimeOfDay[1] > 480 && TimeOfDay[1] < 1080){
                    // Day
                    ColorLerpValues = new float[]{0f, 0f, 0f};
                    DrawDistance = 25f + (Sunnyness * 75f);
                    if(TimeOfDay[1] < 750) SunRotation = Vector3.Lerp(new Vector3(30f, -45, 0f), new Vector3(60f, 0, 0f), (TimeOfDay[1] - 480) / 270f);
                    else SunRotation = Vector3.Lerp(new Vector3(60f, 0, 0f), new Vector3(30f, 90f, 0f), (TimeOfDay[1] - 750) / 270f);
                } else if (TimeOfDay[1] >= 1080f && TimeOfDay[1] <= 1320f){
                    // Evening DAY-DUSK-NIGHT
                    if(TimeOfDay[1] < 1200f) ColorLerpValues = new float[]{0f, 1f, ((float)TimeOfDay[1] - 1080f) / 120f};
                    else ColorLerpValues = new float[]{1f, 2f, ((float)TimeOfDay[1] - 1200f) / 120f};
                    SunRotation = Vector3.Lerp(new Vector3(30f, 90f, 0f), new Vector3(0f, 135f, 0f), (TimeOfDay[1] - 1080) / 240f);
                    DrawDistance = 25f + (Sunnyness * 75f);
                } else {
                    // Night
                    ColorLerpValues = new float[]{2f, 2f, 0f};
                    DrawDistance = 20f + (Sunnyness * 50f);
                    if(TimeOfDay[1] > 1260) SunRotation = Vector3.Lerp(new Vector3(0f, -90f, 0f), new Vector3(60f, 0f, 0f), (TimeOfDay[1] - 1260) / 180f);
                    else SunRotation = Vector3.Lerp(new Vector3(60f, 0f, 0f), new Vector3(0f, 135f, 0f), TimeOfDay[1] / 480f);
                }
            }

            if(QualitySettings.GetQualityLevel() == 0)
                DrawDistance = Mathf.Clamp(DrawDistance, 0f, 50f);

            FogColor = Color32.Lerp( 
                Color32.Lerp(GitBI.FogColors[(int)ColorLerpValues[0] + 3], GitBI.FogColors[(int)ColorLerpValues[1] + 3], ColorLerpValues[2]), 
                Color32.Lerp(GitBI.FogColors[(int)ColorLerpValues[0]], GitBI.FogColors[(int)ColorLerpValues[1]], ColorLerpValues[2]), 
                Sunnyness);
            SkyColor = Color32.Lerp( 
                Color32.Lerp(GitBI.AtmosphereColors[(int)ColorLerpValues[0] + 3], GitBI.AtmosphereColors[(int)ColorLerpValues[1] + 3], ColorLerpValues[2]), 
                Color32.Lerp(GitBI.AtmosphereColors[(int)ColorLerpValues[0]], GitBI.AtmosphereColors[(int)ColorLerpValues[1]], ColorLerpValues[2]), 
                Sunnyness);
            CloudColor = Color32.Lerp(
                //Color32.Lerp(GitBI.CloudColors[(int)ColorLerpValues[0] + 3], GitBI.CloudColors[(int)ColorLerpValues[1] + 3], ColorLerpValues[2]),
                Color32.Lerp(GitBI.FogColors[(int)ColorLerpValues[0] + 3], GitBI.FogColors[(int)ColorLerpValues[1] + 3], ColorLerpValues[2]),
                Color32.Lerp(GitBI.CloudColors[(int)ColorLerpValues[0]], GitBI.CloudColors[(int)ColorLerpValues[1]], ColorLerpValues[2]),
                Sunnyness * 2f);
            SunColors[0] = Color32.Lerp(GitBI.SunColors[(int)ColorLerpValues[0]], GitBI.SunColors[(int)ColorLerpValues[1]], ColorLerpValues[2]);
            SunColors[1] = Color32.Lerp( 
                new Color32(0, 0, 0, 255), 
                Color32.Lerp(GitBI.SunColors[(int)ColorLerpValues[0]], GitBI.SunColors[(int)ColorLerpValues[1]], ColorLerpValues[2]), 
                Sunnyness*1.3f);
            AmbientColor = Color.Lerp(GitBI.AmbientColors[(int)ColorLerpValues[0]], GitBI.AmbientColors[(int)ColorLerpValues[1]], ColorLerpValues[2]) / (1f + (Sunnyness / 2f));
            PostProcessingColor = Color32.Lerp( 
                Color32.Lerp(GitBI.PostProcessingColors[(int)ColorLerpValues[0] + 3], GitBI.PostProcessingColors[(int)ColorLerpValues[1] + 3], ColorLerpValues[2]), 
                Color32.Lerp(GitBI.PostProcessingColors[(int)ColorLerpValues[0]], GitBI.PostProcessingColors[(int)ColorLerpValues[1]], ColorLerpValues[2]), 
                Sunnyness * 2f);
            PPV = Vector4.Lerp(
                Vector4.Lerp(GitBI.PostProcessingVariables[(int)ColorLerpValues[0] + 3], GitBI.PostProcessingVariables[(int)ColorLerpValues[1] + 3], ColorLerpValues[2]),
                Vector4.Lerp(GitBI.PostProcessingVariables[(int)ColorLerpValues[0]], GitBI.PostProcessingVariables[(int)ColorLerpValues[1]], ColorLerpValues[2]),
                Sunnyness * 2f);
        }

        if (WhatSet == "Normal" && Default == true && IsSwimming[0] == false) {
            GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = FogColor;
            RenderSettings.fogColor = FogColor;
            RenderSettings.fogEndDistance = DrawDistance;
            GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane = DrawDistance;
            RenderSettings.ambientLight = AmbientColor;
            GameObject.Find("Sun").transform.eulerAngles = SunRotation;
            GameObject.Find("Sun").GetComponent<Light>().color = SunColors[1];
            GameObject.Find("Sun").GetComponent<Light>().intensity = 1f;
            GameObject.Find("Sun").GetComponent<LightControlScript>().SetLight();
            DefPPC = PostProcessingColor;
            DefCST = new float[]{PPV.x, PPV.y, PPV.z, PPV.w};
            WhichSkybox = "Standard";
            HasSet = true;
        } else if (WhatSet == "NormalEnding" && Default == true) {
            float LerpValue = RoundTime / 30f;
            Sunnyness = Mathf.Clamp(Sunnyness, 0f, LerpValue);
            SkyColor = Color32.Lerp(new Color32(0, 0, 0, 0), SkyColor, (LerpValue + 0.25f) / 1.25f);
            GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = Color32.Lerp(new Color32(0, 0, 0, 0), FogColor, (LerpValue + 0.25f) / 1.25f);
            RenderSettings.fogColor = Color32.Lerp(new Color32(0, 0, 0, 0), FogColor, (LerpValue + 0.25f) / 1.25f);
            RenderSettings.fogEndDistance = DrawDistance;
            GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane = DrawDistance;
            RenderSettings.ambientLight = AmbientColor;
            GameObject.Find("Sun").transform.eulerAngles = SunRotation;
            GameObject.Find("Sun").GetComponent<Light>().color = Color32.Lerp(new Color32(0, 0, 0, 0), SunColors[1], LerpValue);
            GameObject.Find("Sun").GetComponent<Light>().intensity = Mathf.Lerp(0f, 1f, LerpValue);
            GameObject.Find("Sun").GetComponent<LightControlScript>().SetLight();
            DefPPC = PostProcessingColor;
            DefCST = new float[]{PPV.x, PPV.y, PPV.z, PPV.w};
            WhichSkybox = "Standard";
            HasSet = true;
        } else if (WhatSet == "Horde"){
            float LerpValue = RoundTime / 5f;
            if(GS.SkyboxType < 2) LerpValue = 0f;
            GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = Color32.Lerp(GotTerrain.GetComponent<MapInfo>().SkyColors[1], GotTerrain.GetComponent<MapInfo>().SkyColors[0], RoundTime / 5f);
            RenderSettings.fogColor = GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor;
            RenderSettings.fogEndDistance = Mathf.Lerp(GotTerrain.GetComponent<MapInfo>().FogDistances[1], GotTerrain.GetComponent<MapInfo>().FogDistances[0], RoundTime / 5f);
            RenderSettings.ambientLight = Color32.Lerp(GotTerrain.GetComponent<MapInfo>().AmbientColors[1], GotTerrain.GetComponent<MapInfo>().AmbientColors[0], RoundTime / 5f);
            GameObject.Find("Sun").GetComponent<Light>().color = Color32.Lerp(GotTerrain.GetComponent<MapInfo>().LightColors[1], GotTerrain.GetComponent<MapInfo>().LightColors[0], RoundTime / 5f);
            GameObject.Find("Sun").GetComponent<LightControlScript>().SetLight();
            DefPPC = Color.white;
            DefCST = new float[]{0f, 0f, 0f, 0f};
            GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane = RenderSettings.fogEndDistance;
            WhichSkybox = "Plain";
            HasSet = true;
        } else if (WhatSet == "Swimming" && Default == true) {
            float SwimDepthA = SwimDepth;
            if (TimeOfDay[1] == 3) {
                SwimDepthA = Mathf.Lerp(SwimDepth, 1f, 0.5f);
            } else if (TimeOfDay[1] == 0) {
                SwimDepthA = 1f;
            }
            RenderSettings.fogColor = Color32.Lerp(FogColor, new Color32(0, 0, 25, 255), SwimDepthA);
            RenderSettings.ambientLight = Color32.Lerp(FogColor, new Color32(0, 0, 0, 255), SwimDepthA);
            GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = RenderSettings.fogColor;
            GameObject.Find("Sun").transform.eulerAngles = SunRotation;
            GameObject.Find("Sun").GetComponent<Light>().intensity = Mathf.Lerp(1f, 0f, SwimDepthA);
            GameObject.Find("Sun").GetComponent<Light>().color = Color.black;//Color32.Lerp(FogColor, new Color32(0, 0, 25, 255), SwimDepthA);
            GameObject.Find("Sun").GetComponent<LightControlScript>().SetLight();
            DefPPC = Color32.Lerp(FogColor, new Color32(125, 200, 255, 255), SwimDepthA);
            DefCST = new float[]{Mathf.Lerp(0f, 25f, SwimDepthA), Mathf.Lerp(0f, 100f, SwimDepthA), 0f, 0.5f};
            GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane = Mathf.Lerp(DrawDistance * 0.75f, 10f, SwimDepthA);
            RenderSettings.fogEndDistance = GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane;
            WhichSkybox = "Plain";
            HasSet = true;
        } else if (WhatSet == "TealState") {
            GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = new Color32(100, 175, 255, 255);
            RenderSettings.fogColor = GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor;
            RenderSettings.fogEndDistance = 75f;
            GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane = 75f;
            RenderSettings.ambientLight = new Color32(0, 0, 55, 255);
            GameObject.Find("Sun").transform.eulerAngles = SunRotation;
            GameObject.Find("Sun").GetComponent<Light>().color = new Color32(0, 0, 255, 255);
            GameObject.Find("Sun").GetComponent<Light>().intensity = 0.5f;
            GameObject.Find("Sun").GetComponent<LightControlScript>().SetLight();
            DefPPC = new Color32(0, 125, 255, 255);
            DefCST = new float[]{50f, -50f, 0f, 0f};
            WhichSkybox = "Plain";
            HasSet = true;
        } else if (WhatSet == "Nuked" || (WhatSet == "Normal" && RoundState == "Nuked")) {
            GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = new Color32(125, 75, 0, 255);
            RenderSettings.fogColor = GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor;
            RenderSettings.fogEndDistance = 250f;
            GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane = 250f;
            RenderSettings.ambientLight = new Color32(255, 75, 0, 255);
            //GameObject.Find("Sun").transform.eulerAngles = SunRotation;
            GameObject.Find("Sun").GetComponent<Light>().color = new Color32(255, 155, 55, 255);
            GameObject.Find("Sun").GetComponent<Light>().intensity = 1f;
            GameObject.Find("Sun").GetComponent<LightControlScript>().SetLight();
            DefPPC = new Color32(255, 125, 125, 255);
            DefCST = new float[]{50f, 50f, 0f, 0.5f};
            WhichSkybox = "Nuked";
            HasSet = true;
        }
        if (GS.SkyboxType == 0) WhichSkybox = "Plain";

        if(HasSet){

            SkyboxObj.transform.eulerAngles = new Vector3(0f, GameObject.Find("Sun").transform.eulerAngles.y - 180f, 0f);
            for(int GetSB = 0; GetSB < SkyboxObj.transform.childCount; GetSB ++){
                GameObject GotSkybox = SkyboxObj.transform.GetChild(GetSB).gameObject;
                if(GotSkybox.name == WhichSkybox){
                    GotSkybox.SetActive(true);
                    GotSkybox.transform.SetSiblingIndex(0);
                    string SkyboxImage = "Plain";

                    switch(WhichSkybox){
                        case "Standard":
                            GotSkybox.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = SkyColor;
                            switch(Weather){
                                case 0: SkyboxImage = "ClearSky"; break;
                                case 1: SkyboxImage = "MostlyClearSky"; break;
                                case 2: SkyboxImage = "PartlyCloudy"; break;
                                case 3: case 4: case 5: SkyboxImage = "Cloudy"; break;
                            }
                            break;
                        case "Plain":
                            GotSkybox.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = RenderSettings.fogColor;
                            SkyboxImage = "Plain";
                            break;
                        case "Nuked":
                            GotSkybox.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                            SkyboxImage = "Nuked";
                            break;
                    }

                    foreach(Texture SetSkybox in SkyboxImages){
                        if(SetSkybox.name == SkyboxImage) {
                            GotSkybox.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = SetSkybox; 
                        }
                    }

                    for(int GC = 1; GC < GotSkybox.transform.childCount; GC ++){
                        GameObject GetCloud = GotSkybox.transform.GetChild(GC).gameObject;
                        switch(GetCloud.name){
                            case "Clouds1": case "Clouds2":
                                GetCloud.transform.localEulerAngles = new Vector3(0f, 0f, GS.SeedPerlin(GS.RoundSeed) * 360f);
                                Color CloudColorToSet = CloudColor;
                                GetCloud.GetComponent<SpriteRenderer>().color = CloudColorToSet;
                                break;
                            case "Sun":
                                if(TimeOfDay[1] > 360 && TimeOfDay[1] < 1260) {
                                    GetCloud.transform.rotation = GameObject.Find("Sun").transform.rotation; 
                                    GetCloud.transform.localScale = Vector3.one;
                                    GetCloud.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GetCloud.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color32.Lerp( 
                                        new Color32( SunColors[0].r, SunColors[0].g, SunColors[0].b, (byte)(SunPower * 255f)),
                                        new Color32(255, 255, 255, (byte)(SunPower * 255f)),
                                        (SunRotation.x - 20f) / 45f
                                    );
                                } else GetCloud.transform.localScale = Vector3.zero;
                                break;
                            case "Moon":
                                if(TimeOfDay[1] <= 480 || TimeOfDay[1] >= 1020) {
                                    if(TimeOfDay[1] >= 1020) GetCloud.transform.eulerAngles = Vector3.Lerp(new Vector3(0f, -90f, 0f), new Vector3(60f, 0f, 0f), (TimeOfDay[1] - 1020) / 420f);
                                    else GetCloud.transform.eulerAngles = Vector3.Lerp(new Vector3(60f, 0f, 0f), new Vector3(0f, 135f, 0f), TimeOfDay[1] / 480f);
                                    GetCloud.transform.localScale = Vector3.one;
                                } else GetCloud.transform.localScale = Vector3.zero;
                                break;
                            case "Nuke":
                                GetCloud.transform.rotation = GameObject.Find("Sun").transform.rotation; 
                                GetCloud.transform.eulerAngles = new Vector3(15f, GetCloud.transform.eulerAngles.y, GetCloud.transform.eulerAngles.z);
                                GetCloud.transform.localScale = Vector3.one;
                                GetCloud.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                                break;
                        }
                    }

                } else {
                    GotSkybox.SetActive(false);
                }
            }
        }

    }

}
