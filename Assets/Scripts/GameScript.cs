using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using System.Globalization;
using System.IO;
using Unity.Mathematics;
using Random=UnityEngine.Random;

public class GameScript : MonoBehaviour {

    // Main Variables
    public string Version = "1.0";
    public int Platform = 2; // 1 Desktop   2 WebGL
    // Main Variables

    // Game Variables
    public string SaveFileName = "";

    public string RoundSetting = "G0?D0?P1";
    public int Round = 0;
    public int Biome = 0;
    public int Score = 0;
    public int Money = 0;
    public Vector2 LandSeed;
    public Vector2 HealthSave;
    public Vector2 HungerSave;
    public float PlayerSpeed;
    public string PlayerInventory;
    public string PlayerEquipment;
    public int MaxInventory = 4;
    public string PlayerBuffs = "";

    public int CurrentSave = 0;
    public string PlaythroughStats = "";
    public string[] NeueScore;
    public int WhatOnStart = 0; // 0 New 1 Load
    // Game Variables

    // Game Options
    // Graphics
    public int GraphicsQuality = 2;
    int PrevLightingQuality = 0;
    public int LightingQuality = 1;
    int PrevParticlesQuality = 0;
    public int ParticlesQuality = 1;
    public int GrassQuality = 0;
    public float CameraBobbing = 1f;
    public float CameraShifting = 1f;
    public float FOV = 50f;
    public bool Ragdolls = true;
    public Vector2 MainResolution = new Vector2(800f, 600f);
    public Vector2 UIResolution = new Vector2(800f, 600f);
    int PrevSkyboxType = 0;
    public int SkyboxType = 0;
    // Volume
    public float MuteVolume = 0f;
    public float MasterVolume = 1f;
    public float MasterVolumeA = 0f;
    float PrevMasterVolume = 0f;
    public float MusicVolume = 1f;
    float PrevMusicVolume = 0f;
    public float SoundVolume = 1f;
    float PrevSoundVolume = 0f;
    public bool EarpiercingAllowed = true;
    // Mouse
    public float MouseSensitivity = 1f;
    public float MouseSmoothness = 0f;
    public bool InvertedMouse = false;
    // Controlls
    public struct KeyBind{
        public KeyBind(string A, KeyCode B){ Name = A; Key = B; }
        public string Name;
        public KeyCode Key;
    }
    public KeyBind[] Controlls = new KeyBind[]{
        new KeyBind("MoveFoward", KeyCode.W)
    };
    // Misc
    public string Language = "English";
    public int LaserColor = 0;
    public int HoloSightImage = 0;
    public string HudColorMain = "White";
    public string HudColorHUE = "000000";
    string PrevHudColorHUE = "000000";
    // Game Options

    // Profile data
    public ProfileScript PS;
    // Profile data

    // Misc
    public string WindowToBootUp = "";
    bool CanSaveSettings = true;
    public int MaxFPS = 999;
    public float[] Earpiercing = new float[] { 0f, 0f };

    public List<SoundControlScript> SoundCache;
    public List<ParticleControlScript> ParticleCache;
    public List<LightControlScript> LightCache;
    public List<HudColorControl> HudCache;

    public LayerMask IgnoreMaks1;
    public LayerMask IngoreMaskWP;
    public LayerMask IgnoreMaskEnemy;
    public LayerMask IngoreMaskAttack;
    public float SaveSettingsTime = 1f;
    int PrevGraphSet = -1;
    float ASetOffset = 0f;
    public string ToldHints = "";
    float RaindowHudChange = 100f;

    public PostProcessProfile MainPP;
    public PostProcessProfile GunsPP;
    Bloom PPBloom;
    AmbientOcclusion PPAmbient;
    MotionBlur PPBlur;
    ColorGrading PPColorGrading;
    Vignette PPVignette;
    DepthOfField PPDepth;
    public float[] ContSaturTempInvi;
    public Color32 PPColor;
    public float[] CameraFocus;

    public List<Vector2> Resolutions;
    // Misc

    // Use this for initialization
    void Start () {

        if (GameObject.Find("_GameScript") != null) {
            Destroy(this.gameObject);
        } else {
            MainPP.TryGetSettings(out PPBloom);
            GunsPP.TryGetSettings(out PPAmbient);
            MainPP.TryGetSettings(out PPBlur);
            MainPP.TryGetSettings(out PPColorGrading);
            MainPP.TryGetSettings(out PPVignette);
            MainPP.TryGetSettings(out PPDepth);
            ContSaturTempInvi = new float[] { 0f, 0f, 0f, 0f };
            PPColor = Color.black;
            CameraFocus = new float[]{10, 2, 4};
            Earpiercing = new float[] { 0f, 0.01f };

            SoundCache = new List<SoundControlScript>();
            ParticleCache = new List<ParticleControlScript>();
            LightCache = new List<LightControlScript>();
            HudCache = new List<HudColorControl>();

            DontDestroyOnLoad(this.gameObject);
            this.gameObject.name = "_GameScript";
            PS = this.GetComponent<ProfileScript>();

            
            Physics.IgnoreLayerCollision(9, 10); // Ignore collision between PLAYER and ITEM

            Physics.IgnoreLayerCollision(9, 13); // Ignore collision between PLAYER and DEAD
            Physics.IgnoreLayerCollision(12, 13); // Ignore collision between MOB and DEAD
            //Physics.IgnoreLayerCollision(13, 13); // Ignore collision between DEAD and DEAD

            Physics.IgnoreLayerCollision(0, 14); // Ignore collision between DEFAULT and IGNOREAI
            Physics.IgnoreLayerCollision(2, 14); // Ignore collision between IGNORE RAYCAST (aka, ground detector) and IGNOREAI

            Physics.IgnoreLayerCollision(10, 16); // Ignore collision between ITEM and DEEPWATER
            Physics.IgnoreLayerCollision(13, 16); // Ignore collision between DEAD and DEEPWATER

            Controlls = new KeyBind[]{
                new KeyBind("MoveForward", KeyCode.W),
                new KeyBind("MoveBackwards", KeyCode.S),
                new KeyBind("MoveRight", KeyCode.D),
                new KeyBind("MoveLeft", KeyCode.A),
                new KeyBind("Sprint", KeyCode.LeftShift),
                new KeyBind("Jump", KeyCode.Space),
                new KeyBind("Crouch", KeyCode.C),
                new KeyBind("Action", KeyCode.Mouse0),
                new KeyBind("AltAction", KeyCode.Mouse1),
                new KeyBind("Interaction", KeyCode.E),
                new KeyBind("DropItem", KeyCode.Q),
                new KeyBind("InformationTab", KeyCode.Tab),
                new KeyBind("Reload", KeyCode.R),
                new KeyBind("CraftingTab", KeyCode.B)
            };

            if (PlayerPrefs.HasKey("Options")) {
                SaveSettings(1);
            }

            // Set resolutions
            for (int rs = 0; rs < Screen.resolutions.Length; rs++)
                if(Screen.resolutions[rs].refreshRate == 60 && Screen.resolutions[rs].width >= 640f && Screen.resolutions[rs].height >= 480f) 
                    Resolutions.Add(new Vector2(Screen.resolutions[rs].width, Screen.resolutions[rs].height));

        }
		
	}

    void Update() {

        SettingsInAction();

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.L)) {
            CanSaveSettings = false;
            PlayerPrefs.DeleteAll();
            print("AutoDestructionIniciated");
        }

        if (SaveSettingsTime > 0f) {
            SaveSettingsTime -= Time.unscaledDeltaTime;
        } else {
            SaveSettingsTime = 1f;
            if (CanSaveSettings == true) {
                SaveSettings(0);
            }
        }

    }

    public void SetText(Text TextToSet, string Eng, string Pl) {

        if (Language == "English") {
            TextToSet.text = Eng;
        } else if (Language == "Polski") {
            TextToSet.text = Pl;
        }

    }

    public string SetString(string Eng, string Pl){

        string FinalString = "";
        if (Language == "English") {
            FinalString = Eng;
        } else {
            FinalString = Pl;
        }

        return FinalString;

    }

    public string DisplayTime(float TimeToSet) {

        string TimeA = "0" + (int)(TimeToSet - (Mathf.Floor(TimeToSet / 60f) * 60f));
        TimeA = TimeA.Substring(TimeA.Length - 2, 2);
        string TimeB = (int)(TimeToSet / 60f) + ":" + TimeA;

        return TimeB;

    }

    public void AddToScore(int ScoreToAdd){
        Score += ScoreToAdd;
        PS.GitExp(ScoreToAdd);
    }

    public void ChangeLevel(string WayOfChange) {

        if (WayOfChange == "EscapeMap" || WayOfChange == "ResetMap") {

            GameObject FoundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (WayOfChange == "EscapeMap") {
                Round += 1;
                AddToScore(Round * 10);
            }

            List<int> AvailableBiomes = new List<int>();
            int[] AllBiomes = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            foreach (int ReceiveBiome in AllBiomes) {
                if (ReceiveBiome != Biome) {
                    AvailableBiomes.Add(ReceiveBiome);
                }
            }
            Biome = AvailableBiomes.ToArray()[(int)Random.Range(0f, AvailableBiomes.ToArray().Length - 0.1f)];
            LandSeed = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
            HealthSave = new Vector2(FoundPlayer.GetComponent<PlayerScript>().Health[0], FoundPlayer.GetComponent<PlayerScript>().Health[1]);
            HungerSave = new Vector2(FoundPlayer.GetComponent<PlayerScript>().Food[0], FoundPlayer.GetComponent<PlayerScript>().Food[1]);
            PlayerSpeed = FoundPlayer.GetComponent<PlayerScript>().Speed;
            PlayerInventory = FoundPlayer.GetComponent<PlayerScript>().InventoryText;

            MaxInventory = FoundPlayer.GetComponent<PlayerScript>().MaxInventorySlots;
            PlayerEquipment = FoundPlayer.GetComponent<PlayerScript>().EquipmentText;
            PlayerBuffs = FoundPlayer.GetComponent<PlayerScript>().BuffsText;

            //Saves(CurrentSave, 0);
            SaveManipulation(CurrentSave, 0);
            WhatOnStart = 1;

            SceneManager.LoadScene("MainGame");

        } else if (WayOfChange == "NewGame") {

            //Saves(CurrentSave, 2);
            SaveManipulation(CurrentSave, 2);
            PlaythroughStats = "";
            WhatOnStart = 0;

            SceneManager.LoadScene("MainGame");

        } else if (WayOfChange == "LoadGame") {

            //Saves(CurrentSave, 1);
            SaveManipulation(CurrentSave, 1);
            print("Loading a file of " + CurrentSave.ToString() + ". That file data is " + SaveManipulation(CurrentSave, 3));
            WhatOnStart = 1;

            SceneManager.LoadScene("MainGame");

        } else if (WayOfChange == "GameOver") {

            SceneManager.LoadScene("NewMenu");
            WindowToBootUp = "GameOver";

        } else if (WayOfChange == "BackToMenu") {

            SceneManager.LoadScene("NewMenu");
            WindowToBootUp = "MainMenu";

        }

    }

    /*public void Saves(int CurrentSave, int WhatToDo){

        if (WhatToDo == 0) {

            PlayerPrefs.SetString("S" + CurrentSave + "RoundSettings", RoundSetting);
            PlayerPrefs.SetInt("S" + CurrentSave + "Round", Round);
            PlayerPrefs.SetInt("S" + CurrentSave + "Score", Score);
            PlayerPrefs.SetInt("S" + CurrentSave + "Biome", Biome);
            PlayerPrefs.SetInt("S" + CurrentSave + "Money", Money);

            PlayerPrefs.SetFloat("S" + CurrentSave + "LandSeed-0", LandSeed.x);
            PlayerPrefs.SetFloat("S" + CurrentSave + "LandSeed-1", LandSeed.y);

            PlayerPrefs.SetFloat("S" + CurrentSave + "Health-0", HealthSave.x);
            PlayerPrefs.SetFloat("S" + CurrentSave + "Health-1", HealthSave.y);

            PlayerPrefs.SetFloat("S" + CurrentSave + "Hunger-0", HungerSave.x);
            PlayerPrefs.SetFloat("S" + CurrentSave + "Hunger-1", HungerSave.y);

            PlayerPrefs.SetFloat("S" + CurrentSave + "PlayerSpeed", PlayerSpeed);
            PlayerPrefs.SetString("S" + CurrentSave + "PlayerInventory", PlayerInventory);
            PlayerPrefs.SetString("S" + CurrentSave + "PlayerEquipment", PlayerEquipment);
            PlayerPrefs.SetInt("S" + CurrentSave + "MaxInventory", MaxInventory);
            PlayerPrefs.SetString("S" + CurrentSave + "Buffs", PlayerBuffs);

        } else if (WhatToDo == 1) {

            RoundSetting = PlayerPrefs.GetString("S" + CurrentSave + "RoundSettings");
            Round = PlayerPrefs.GetInt("S" + CurrentSave + "Round");
            Score = PlayerPrefs.GetInt("S" + CurrentSave + "Score");
            Biome = PlayerPrefs.GetInt("S" + CurrentSave + "Biome");
            Money = PlayerPrefs.GetInt("S" + CurrentSave + "Money");

            LandSeed.x = PlayerPrefs.GetFloat("S" + CurrentSave + "LandSeed-0");
            LandSeed.y = PlayerPrefs.GetFloat("S" + CurrentSave + "LandSeed-1");

            HealthSave.x = PlayerPrefs.GetFloat("S" + CurrentSave + "Health-0");
            HealthSave.y = PlayerPrefs.GetFloat("S" + CurrentSave + "Health-1");

            HungerSave.x = PlayerPrefs.GetFloat("S" + CurrentSave + "Hunger-0");
            HungerSave.y = PlayerPrefs.GetFloat("S" + CurrentSave + "Hunger-1");

            PlayerSpeed = PlayerPrefs.GetFloat("S" + CurrentSave + "PlayerSpeed");
            PlayerInventory = PlayerPrefs.GetString("S" + CurrentSave + "PlayerInventory");
            PlayerEquipment = PlayerPrefs.GetString("S" + CurrentSave + "PlayerEquipment");
            MaxInventory = PlayerPrefs.GetInt("S" + CurrentSave + "MaxInventory");
            PlayerBuffs = PlayerPrefs.GetString("S" + CurrentSave + "Buffs");

        } else if (WhatToDo == 2) {
            
            PlayerPrefs.DeleteKey("S" + CurrentSave + "DifficultyLevel");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "Round");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "Score");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "Biome");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "Money");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "ReserveAmmo");

            PlayerPrefs.DeleteKey("S" + CurrentSave + "LandSeed-0");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "LandSeed-1");

            PlayerPrefs.DeleteKey("S" + CurrentSave + "Health-0");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "Health-1");

            PlayerPrefs.DeleteKey("S" + CurrentSave + "Hunger-0");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "Hunger-1");

            PlayerPrefs.DeleteKey("S" + CurrentSave + "PlayerSpeed");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "PlayerInventory");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "PlayerEquipment");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "MaxInventory");
            PlayerPrefs.DeleteKey("S" + CurrentSave + "Buffs");

        }

    }*/

    public string SaveManipulation(int SaveID, int WhatToDo){

        switch (WhatToDo){
            case 0: // Save

                // Create injection of file
                string Filer = "id" + SaveID.ToString() + "®sn" + SaveFileName
                + "®rs" + RoundSetting
                + "®pts" + PlaythroughStats
                + "®ro" + Round.ToString()
                + "®so" + Score.ToString()
                + "®bo" + Biome.ToString()
                + "®mo" + Money.ToString()

                + "®Lx" + LandSeed.x.ToString()
                + "®Ly" + LandSeed.y.ToString()

                + "®l0" + HealthSave.x.ToString()
                + "®l1" + HealthSave.y.ToString()

                + "®h0" + HungerSave.x.ToString()
                + "®h1" + HungerSave.y.ToString()

                + "®ps" + PlayerSpeed.ToString()
                + "®pi" + PlayerInventory
                + "®pe" + PlayerEquipment
                + "®mi" + MaxInventory.ToString()
                + "®bs" + PlayerBuffs + "®"
                ;

                // Inject the file into string
                bool updateit = false;
                string newies = "";
                string[] oldies = ListSemiClass(PlayerPrefs.GetString("Saves"), "©");
                for(int co = 0; co < oldies.Length; co++)
                    if (GetSemiClass(oldies[co], "id", "®") == SaveID.ToString()){
                        updateit = true;
                        newies += Filer + "©";
                    } else newies += oldies[co] + "©";
                
                if (!updateit) newies += Filer + "©";

                PlayerPrefs.SetString("Saves", newies);
                break;
            case 1: // Load

                string Receiver = "";
                string[] WellFindIt = ListSemiClass(PlayerPrefs.GetString("Saves"), "©");
                for (int fr = 0; fr < WellFindIt.Length; fr++) if (GetSemiClass(WellFindIt[fr], "id", "®") == SaveID.ToString()){
                    Receiver = WellFindIt[fr];
                    break;
                }

                if(Receiver != ""){
                    SaveFileName = GetSemiClass(Receiver, "sn", "®");

                    RoundSetting = GetSemiClass(Receiver, "rs", "®");
                    PlaythroughStats = GetSemiClass(Receiver, "pts", "®");
                    Round = int.Parse(GetSemiClass(Receiver, "ro", "®"));
                    Score = int.Parse(GetSemiClass(Receiver, "so", "®"));
                    Biome = int.Parse(GetSemiClass(Receiver, "bo", "®"));
                    Money = int.Parse(GetSemiClass(Receiver, "mo", "®"));

                    LandSeed.x = float.Parse(GetSemiClass(Receiver, "Lx", "®"));
                    LandSeed.y = float.Parse(GetSemiClass(Receiver, "Ly", "®"));

                    HealthSave.x = float.Parse(GetSemiClass(Receiver, "l0", "®"));
                    HealthSave.y = (int)float.Parse(GetSemiClass(Receiver, "l1", "®"));

                    HungerSave.x = (int)float.Parse(GetSemiClass(Receiver, "h0", "®"));
                    HungerSave.y = (int)float.Parse(GetSemiClass(Receiver, "h1", "®"));

                    PlayerSpeed = float.Parse(GetSemiClass(Receiver, "ps", "®"));
                    PlayerInventory = GetSemiClass(Receiver, "pi", "®");
                    PlayerEquipment = GetSemiClass(Receiver, "pe", "®");
                    MaxInventory = int.Parse(GetSemiClass(Receiver, "mi", "®"));
                    PlayerBuffs = GetSemiClass(Receiver, "bs", "®");

                } else {
                    Debug.LogError("There is no save file with an id of " + SaveID.ToString());
                }
                break;
            case 2: // Delete
                string[] FullList = ListSemiClass(PlayerPrefs.GetString("Saves"), "©");
                string Utilization = "";
                for(int to = 0; to < FullList.Length; to++){
                    if (GetSemiClass(FullList[to], "id", "®") != SaveID.ToString()) Utilization += FullList[to] + "©";
                }

                PlayerPrefs.SetString("Saves", Utilization);
                break;
            case 3: // ReceiveInstance
                string[] Checkers = ListSemiClass(PlayerPrefs.GetString("Saves"), "©");
                for(int to = 0; to <= Checkers.Length; to++){
                    if (GetSemiClass(Checkers[to], "id", "®") == SaveID.ToString()) return Checkers[to];
                }
                break;
        }
        return "";

    }

    public void UpdateRecord(){

        NeueScore[0] = SetSemiClass(NeueScore[0], "R", "/+-1");

        // Remove rewards and whatnot
        if(GetSemiClass(NeueScore[0], "P") != "1"){
            if(GetSemiClass(NeueScore[0], "G") == "0") PS.Statistics = SetSemiClass(PS.Statistics, "TotalRounds_", "/+" + GetSemiClass(NeueScore[0], "R"));
            else PS.Statistics = SetSemiClass(PS.Statistics, "TotalWaves_", "/+" + GetSemiClass(NeueScore[0], "R"));
            PS.Statistics = SetSemiClass(PS.Statistics, "TotalScore_", "/+" + GetSemiClass(NeueScore[0], "S"));

            if ((ExistSemiClass(NeueScore[0], "S") && int.Parse(GetSemiClass(NeueScore[0], "S")) > 0) && (!ExistSemiClass(PS.Statistics, "HighestScore_") || (float.Parse(GetSemiClass(NeueScore[0], "S")) > float.Parse(GetSemiClass(PS.Statistics, "HighestScore_"))))) {
                PS.Statistics = SetSemiClass(PS.Statistics, "HighestScore_", GetSemiClass(NeueScore[0], "S"));
            }
            if ((ExistSemiClass(NeueScore[0], "R") && int.Parse(GetSemiClass(NeueScore[0], "R")) > 0 && GetSemiClass(NeueScore[0], "G") == "0") && (!ExistSemiClass(PS.Statistics, "MostRounds_") || (float.Parse(GetSemiClass(NeueScore[0], "R")) > float.Parse(GetSemiClass(PS.Statistics, "MostRounds_"))))) {
                PS.Statistics = SetSemiClass(PS.Statistics, "MostRounds_", GetSemiClass(NeueScore[0], "R"));
            } else if ((ExistSemiClass(NeueScore[0], "R") && int.Parse(GetSemiClass(NeueScore[0], "R")) > 0 && GetSemiClass(NeueScore[0], "G") == "1") && (!ExistSemiClass(PS.Statistics, "MostWaves_") || (float.Parse(GetSemiClass(NeueScore[0], "R")) > float.Parse(GetSemiClass(PS.Statistics, "MostWaves_"))))) {
                PS.Statistics = SetSemiClass(PS.Statistics, "MostWaves_", GetSemiClass(NeueScore[0], "R"));
            }
            if (!ExistSemiClass(PS.Statistics, "LongestSurvivedTime_") || (int.Parse(GetSemiClass(NeueScore[1], "SurvivedTime_")) > int.Parse(GetSemiClass(PS.Statistics, "LongestSurvivedTime_")))) {
                PS.Statistics = SetSemiClass(PS.Statistics, "LongestSurvivedTime_", GetSemiClass(NeueScore[1], "SurvivedTime_"));
            }
        }

        string[] TempStati = ListSemiClass(NeueScore[1]);
        string NewTemps = "";
        for (int RU = 0; RU < TempStati.Length; RU++) {
            string[] ScoreType = new string[]{ GetStatName(TempStati[RU], 1), GetStatName(TempStati[RU], 2)};
            if(ScoreType[0] != "misc."){
                if(GetSemiClass(NeueScore[0], "P") != "1") PS.Statistics = SetSemiClass(PS.Statistics, ScoreType[0] + "_", "/+" + ScoreType[1]);
                NewTemps += TempStati[RU] + ";";
            }
        }
        NeueScore[1] = NewTemps;

        // Add records
        if(GetSemiClass(NeueScore[0], "P") != "1"){
            string NewRecord = NeueScore[0] + ";T" + PS.RecordDate.ToString() + "/";
            PS.Records += NewRecord;
            PS.RecordDate ++;

            string[] SortedRecords = ListSemiClass(PS.Records, "/");
            if (SortedRecords.Length > 100){

                // sortowanie bombelkowe
                bool RepeatBubble = false;
                for(int bubcek = 0; bubcek <= SortedRecords.Length; bubcek++){
                    if(bubcek < SortedRecords.Length){
                        if(int.Parse(GetSemiClass(SortedRecords[bubcek], "S")) > int.Parse(GetSemiClass(SortedRecords[(int)Mathf.Clamp(bubcek, 0, SortedRecords.Length-1)], "S")) ){
                            string heyu = SortedRecords[(int)Mathf.Clamp(bubcek, 0, SortedRecords.Length-1)];
                            SortedRecords[(int)Mathf.Clamp(bubcek, 0, SortedRecords.Length-1)] = SortedRecords[bubcek];
                            SortedRecords[bubcek] = heyu;
                            RepeatBubble = true;
                        }
                    } else if (RepeatBubble) {
                        bubcek = 0;
                    }
                }

                PS.Records = "";
                for(int no = 1; no < SortedRecords.Length; no++) PS.Records += SortedRecords[no] + "/";
            }
        } else {
            NeueScore[1] += ";ProfileIndependent_0;";
        }

    }

    void SettingsInAction(){

        Application.targetFrameRate = MaxFPS;
        bool trimS = false; bool trimP = false; bool trimL = false;

        // Volumes
        bool SetVolumes = false;
        float Deafening = Mathf.Clamp(Earpiercing[0] / Earpiercing[1], 0f, 1f);
        if (EarpiercingAllowed) {
            if (GameObject.Find("Earpiercing") == null) {
                Earpiercing[0] = 0f;
            } else {
                GameObject.Find("Earpiercing").GetComponent<AudioSource>().volume = Deafening * SoundVolume * MasterVolume;
            }
            if (Earpiercing[0] > 0f) {
                Earpiercing[0] -= 0.02f * (Time.deltaTime * 50f);
                SetVolumes = true;
            } else if (Earpiercing[1] != 0.01f) {
                Earpiercing[1] = 0.01f;
                SetVolumes = true;
            }
        } else {
            Deafening = 0f;
            GameObject.Find("Earpiercing").GetComponent<AudioSource>().volume = 0f;
        }

        MasterVolumeA = Mathf.Clamp(MasterVolume - (Deafening * 2f), 0f, 1f);
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().IsSwimming == true) {
                MasterVolumeA /= 2f;
            }
        }

        
        if (PrevSoundVolume != SoundVolume) {
            PrevSoundVolume = SoundVolume;
            SetVolumes = true;
        } if (PrevMusicVolume != MusicVolume) {
            PrevMusicVolume = MusicVolume;
            SetVolumes = true;
        } if (PrevMasterVolume != MasterVolumeA) {
            PrevMasterVolume = MasterVolumeA;
            SetVolumes = true;
        }

        if (SetVolumes == true && ASetOffset <= 0f) {
            ASetOffset = 0.2f;
            foreach (SoundControlScript A in SoundCache) {
                if(A) A.SetVolume();
                else trimS = true;
            }
        } else if (ASetOffset > 0f) {
            ASetOffset -= 0.02f * (Time.deltaTime * 50f);
        }
        // Volumes

        // GraphicalStuff
        if (QualitySettings.GetQualityLevel() != GraphicsQuality) {
            QualitySettings.SetQualityLevel(GraphicsQuality);
        }
        if (PrevGraphSet != QualitySettings.GetQualityLevel()) {
            if (QualitySettings.GetQualityLevel() == 0) {
                // Minimal
                PPBloom.enabled.value = false;
                PPAmbient.enabled.value = false;
                PPBlur.enabled.value = false;
                PPColorGrading.enabled.value = false;
                PPVignette.enabled.value = false;
                PPDepth.enabled.value = false;
                GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane = 50f;
                RenderSettings.fogEndDistance = 50f;
            } else if (QualitySettings.GetQualityLevel() == 1) {
                // Low
                PPBloom.enabled.value = false;
                PPAmbient.enabled.value = false;
                PPBlur.enabled.value = false;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = false;
                PPColorGrading.contrast.value = ContSaturTempInvi[0];
                PPColorGrading.saturation.value = ContSaturTempInvi[1];
                PPColorGrading.colorFilter.value = PPColor;
                PPVignette.intensity.value = ContSaturTempInvi[3];
            } else if (QualitySettings.GetQualityLevel() == 2) {
                // Medium
                PPBloom.enabled.value = true;
                PPAmbient.enabled.value = false;
                PPBlur.enabled.value = false;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = false;
                PPColorGrading.contrast.value = ContSaturTempInvi[0];
                PPColorGrading.saturation.value = ContSaturTempInvi[1];
                PPColorGrading.temperature.value = ContSaturTempInvi[2];
                PPColorGrading.colorFilter.value = PPColor;
                PPVignette.intensity.value = ContSaturTempInvi[3];
            } else if (QualitySettings.GetQualityLevel() == 3) {
                // Good
                PPBloom.enabled.value = true;
                PPAmbient.enabled.value = true;
                PPBlur.enabled.value = false;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = false;
                PPColorGrading.contrast.value = ContSaturTempInvi[0];
                PPColorGrading.saturation.value = ContSaturTempInvi[1];
                PPColorGrading.temperature.value = ContSaturTempInvi[2];
                PPColorGrading.colorFilter.value = PPColor;
                PPVignette.intensity.value = ContSaturTempInvi[3];
            } else if (QualitySettings.GetQualityLevel() == 4) {
                // High
                PPBloom.enabled.value = true;
                PPAmbient.enabled.value = true;
                PPBlur.enabled.value = true;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = true;
                PPColorGrading.contrast.value = ContSaturTempInvi[0];
                PPColorGrading.saturation.value = ContSaturTempInvi[1];
                PPColorGrading.temperature.value = ContSaturTempInvi[2];
                PPColorGrading.colorFilter.value = PPColor;
                PPVignette.intensity.value = ContSaturTempInvi[3];
                PPDepth.focusDistance.value = CameraFocus[0];
                PPDepth.aperture.value = CameraFocus[1];
                PPDepth.focalLength.value = CameraFocus[2];
            }
        }

        // Lighting quality control
        if (PrevLightingQuality != LightingQuality) {
            PrevLightingQuality = LightingQuality;
            foreach (LightControlScript L in LightCache) {
                if(L)L.SetLight();
                else trimL = true;
            }
        }
        // Particle quality control
        if (PrevParticlesQuality != ParticlesQuality) {
            PrevParticlesQuality = ParticlesQuality;
            foreach (ParticleControlScript P in ParticleCache) {
                if(P)P.SetEmission();
                else trimP = true;
            }
        }

        // cleanup caches
        if(trimS) SoundCache.TrimExcess();
        if(trimP) ParticleCache.TrimExcess();
        if(trimL) LightCache.TrimExcess();

        // Skybox change
        if(PrevSkyboxType != SkyboxType){
            PrevSkyboxType = SkyboxType;
            if(GameObject.Find("_RoundScript"))
                GameObject.Find("_RoundScript").GetComponent<RoundScript>().AmbientSet("Normal");
        }

        // Resolutions
        if(SceneManager.GetActiveScene().name == "MainGame")
            GameObject.Find("MainCanvas").GetComponent<CanvasScaler>().referenceResolution = UIResolution;

        if(!Screen.fullScreen && (Screen.width != MainResolution.x || Screen.height != MainResolution.y))
            Screen.SetResolution((int)MainResolution.x, (int)MainResolution.y, FullScreenMode.Windowed);
        // GraphicalStuff

        // Hud Color
        if (HudColorMain == "Black" && HudColorHUE != "000010") {
            HudColorHUE = "000010";
        } else if (HudColorMain == "White" && HudColorHUE != "000080") {
            HudColorHUE = "000080";
        } else if (HudColorMain == "Rainbow") {
            RaindowHudChange = (RaindowHudChange + 0.02f * (Time.deltaTime * 50f));
            if(RaindowHudChange >= 200f) RaindowHudChange = 100f;
            HudColorHUE = ((int)RaindowHudChange).ToString().Substring(1) + "8050";
        }
        if (PrevHudColorHUE != HudColorHUE) {
            PrevHudColorHUE = HudColorHUE;
            bool trimH = false;
            foreach (HudColorControl HCC in HudCache) 
                if(HCC) HCC.SetColor("");
                else trimH = true;
            if (trimH) HudCache.TrimExcess();
        }
        // Hud Color

    }

    void SaveSettings(int WhatToDo) {

        switch(WhatToDo){
            case 0: // Save everything
                string OptionToSave = 
                "Lang_" + Language +
                ";Msens_" + MouseSensitivity.ToString(CultureInfo.InvariantCulture) +
                ";Msmooth_" + MouseSmoothness.ToString(CultureInfo.InvariantCulture) +
                ";Minv_" + InvertedMouse.ToString(CultureInfo.InvariantCulture) +
                // add inverted X axis
                // add earpiercing
                // add ragdolls
                ";CamBob_" + CameraBobbing.ToString(CultureInfo.InvariantCulture) +
                ";CamShi_" + CameraShifting.ToString(CultureInfo.InvariantCulture) +
                ";MasterV_" + MasterVolume.ToString(CultureInfo.InvariantCulture) +
                ";SoundV_" + SoundVolume.ToString(CultureInfo.InvariantCulture) +
                ";MusicV_" + MusicVolume.ToString(CultureInfo.InvariantCulture) +
                ";FOV_" + FOV.ToString(CultureInfo.InvariantCulture) +
                ";LaserColor_" + LaserColor.ToString(CultureInfo.InvariantCulture) +
                ";HoloSight_" + HoloSightImage.ToString(CultureInfo.InvariantCulture) +
                ";GrapQ_" + GraphicsQuality.ToString(CultureInfo.InvariantCulture) +
                ";FoliQ_" + GrassQuality.ToString(CultureInfo.InvariantCulture) +
                ";PartQ_" + ParticlesQuality.ToString(CultureInfo.InvariantCulture) +
                ";LiteQ_" + LightingQuality.ToString(CultureInfo.InvariantCulture) +
                ";Hints_" + ToldHints + //CHUJ
                ";SkyType_" + SkyboxType.ToString() +
                ";HudColor_" + HudColorMain +
                ";MainResX_" + MainResolution.x.ToString(CultureInfo.InvariantCulture) +
                ";MainResY_" + MainResolution.y.ToString(CultureInfo.InvariantCulture) +
                ";UiResX_" + UIResolution.x.ToString(CultureInfo.InvariantCulture) +
                ";UiResY_" + UIResolution.y.ToString(CultureInfo.InvariantCulture) +
                ";";
                PlayerPrefs.SetString("Options", OptionToSave);
                PlayerPrefs.SetInt("PrevProfile", PS.ProfileID);
                PS.SaveProfile(-1);

                break;
            case 1: // LoadEverything
                if(PlayerPrefs.HasKey("Options")){
                    Language = GetSemiClass(PlayerPrefs.GetString("Options"), "Lang_" );
                    MouseSensitivity = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "Msens_" ), CultureInfo.InvariantCulture);
                    MouseSmoothness = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "Msmooth_" ), CultureInfo.InvariantCulture);
                    InvertedMouse = bool.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "Minv_" ));
                    CameraBobbing = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "CamBob_" ), CultureInfo.InvariantCulture);
                    CameraShifting = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "CamShi_" ), CultureInfo.InvariantCulture);
                    MasterVolume = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "MasterV_" ), CultureInfo.InvariantCulture);
                    SoundVolume = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "SoundV_" ), CultureInfo.InvariantCulture);
                    MusicVolume = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "MusicV_" ), CultureInfo.InvariantCulture);
                    FOV = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "FOV_" ), CultureInfo.InvariantCulture);
                    LaserColor = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "LaserColor_" ), CultureInfo.InvariantCulture);
                    HoloSightImage = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "HoloSight_" ), CultureInfo.InvariantCulture);
                    GraphicsQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "GrapQ_" ), CultureInfo.InvariantCulture);
                    GrassQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "FoliQ_" ), CultureInfo.InvariantCulture);
                    ParticlesQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "PartQ_" ), CultureInfo.InvariantCulture);
                    LightingQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "LiteQ_" ), CultureInfo.InvariantCulture);
                    ToldHints = GetSemiClass(PlayerPrefs.GetString("Options"), "Hints_" );
                    SkyboxType = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "SkyType_" ), CultureInfo.InvariantCulture);
                    HudColorMain = GetSemiClass(PlayerPrefs.GetString("Options"), "HudColor_");
                    MainResolution = new Vector2 (
                        float.Parse(GetSemiClass(PlayerPrefs.GetString("Options"), "MainResX_"), CultureInfo.InvariantCulture),
                        float.Parse(GetSemiClass(PlayerPrefs.GetString("Options"), "MainResY_"), CultureInfo.InvariantCulture)
                    );
                    UIResolution = new Vector2 (
                        float.Parse(GetSemiClass(PlayerPrefs.GetString("Options"), "UiResX_"), CultureInfo.InvariantCulture),
                        float.Parse(GetSemiClass(PlayerPrefs.GetString("Options"), "UiResY_"), CultureInfo.InvariantCulture)
                    );
                }
                if(PlayerPrefs.HasKey("PrevProfile")) PS.SaveProfile( PlayerPrefs.GetInt("PrevProfile") );
                break;
            case 2: // DeleteEverything
                PlayerPrefs.DeleteKey("Options");
                PlayerPrefs.DeleteKey("PrevProfile");
                PlayerPrefs.DeleteKey("ProfileSaves");
                break;
        }

        /*if (WhatToDo == 0) {
            PlayerPrefs.SetString("Language", Language);
            PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
            PlayerPrefs.SetFloat("MouseSmoothness", MouseSmoothness);
            PlayerPrefs.SetString("InvertedMouse", InvertedMouse.ToString());
            PlayerPrefs.SetFloat("CameraBobbing", CameraBobbing);
            PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
            PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
            PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
            PlayerPrefs.SetFloat("FOV", FOV);
            PlayerPrefs.SetInt("LaserColor", LaserColor);
            PlayerPrefs.SetInt("HoloSightImage", HoloSightImage);
            PlayerPrefs.SetInt("Graphics", QualitySettings.GetQualityLevel());
            PlayerPrefs.SetString("HintsTold", ToldHints);
        } else if (WhatToDo == 1) {
            Language = PlayerPrefs.GetString("Language");
            MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
            MouseSmoothness = PlayerPrefs.GetFloat("MouseSmoothness");
            InvertedMouse = bool.Parse(PlayerPrefs.GetString("InvertedMouse"));
            CameraBobbing = PlayerPrefs.GetFloat("CameraBobbing");
            MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            FOV = PlayerPrefs.GetFloat("FOV");
            LaserColor = PlayerPrefs.GetInt("LaserColor");
            HoloSightImage = PlayerPrefs.GetInt("HoloSightImage");
            if (Platform == 2) {
                if (PlayerPrefs.HasKey("Graphics")) {
                    QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Graphics"));
                } else {
                    QualitySettings.SetQualityLevel(2);
                }
            }
            ToldHints = PlayerPrefs.GetString("HintsTold");
        } else if (WhatToDo == 2) {
            PlayerPrefs.DeleteKey("Language");
            PlayerPrefs.DeleteKey("MouseSensitivity");
            PlayerPrefs.DeleteKey("MouseSmoothness");
            PlayerPrefs.DeleteKey("InvertedMouse");
            PlayerPrefs.DeleteKey("CameraBobbing");
            PlayerPrefs.DeleteKey("MasterVolume");
            PlayerPrefs.DeleteKey("SoundVolume");
            PlayerPrefs.DeleteKey("MusicVolume");
            PlayerPrefs.DeleteKey("LaserColor");
            PlayerPrefs.DeleteKey("HoloSightImage");
            PlayerPrefs.DeleteKey("Graphics");
            PlayerPrefs.DeleteKey("FOV");
            PlayerPrefs.DeleteKey("HintsTold");
        }*/

    }

    // Item functions

    public string ReceiveItemName(float ItemID) {

        ItemID = Mathf.Clamp(ItemID, 0, 999);
        string ItemName = "";

        switch ((int)ItemID) {
            case 1:
                ItemName = SetString("Apple", "Jabłko");
                break;
            case 2:
                ItemName = SetString("Flashlight", "Latarka");
                break;
            case 3:
                ItemName = SetString("Bread", "Chleb");
                break;
            case 4:
                ItemName = SetString("Soup Can", "Zupa w Puszce");
                break;
            case 5:
                ItemName = SetString("Canned Mackerel", "Makrela w Puszce");
                break;
            case 6:
                ItemName = SetString("Chocolate Bar", "Czekolada");
                break;
            case 7:
                ItemName = SetString("Sausage", "Kiełbasa");
                break;
            case 8:
                ItemName = SetString("Jam", "Dżem");
                break;
            case 9:
                ItemName = SetString("Chips", "Czipsy");
                break;
            case 10:
                ItemName = SetString("Cheese", "Ser");
                break;
            case 11:
                ItemName = SetString("Glow Stick", "Światło Chemiczne");
                break;
            case 12:
                ItemName = SetString("Flare", "Flara");
                break;
            case 13:
                ItemName = SetString("Lit Flare", "Odpalona Flara");
                break;
            case 14:
                ItemName = SetString("Knife", "Nóż");
                break;
            case 15:
                ItemName = SetString("Crowbar", "Łom");
                break;
            case 16:
                ItemName = SetString("Fire Axe", "Siekiera Strażacka");
                break;
            case 17:
                ItemName = SetString("Bottle of Water", "Butelka z Wodą");
                break;
            case 18:
                ItemName = SetString("Energy Drink", "Energetyk");
                break;
            case 19:
                ItemName = SetString("Candy Bar", "Batonik");
                break;
            case 20:
                ItemName = SetString("Bean Can", "Fasolki w Puszcze");
                break;
            case 21:
                ItemName = SetString("MRE", "MRE");
                break;
            case 22:
                ItemName = SetString("Bandage", "Bandaż");
                break;
            case 23:
                ItemName = SetString("Antibiotics", "Antybiotyk");
                break;
            case 24:
                ItemName = SetString("Vaccine", "Szczepionka");
                break;
            case 25:
                ItemName = SetString("Lugol's Solution", "Płyn Lugola");
                break;
            case 26:
                ItemName = SetString("First Aid Kit", "Apteczka Pierwszej Pomocy");
                break;
            case 27:
                ItemName = SetString("Machete", "Maczeta");
                break;
            case 28:
                ItemName = SetString("Baseball Bat", "Kij Baseball'owy");
                break;
            case 29:
                ItemName = SetString("Colt", "Colt");
                break;
            case 30:
                ItemName = SetString("Pistol Magazine", "Magazynek do Pistoleta");
                break;
            case 31:
                ItemName = SetString("Luger", "Luger");
                break;
            case 32:
                ItemName = SetString("Revolver", "Rewolwer");
                break;
            case 33:
                ItemName = SetString("Ammo Pack", "Paczka z Amunicją");
                break;
            case 34:
                ItemName = SetString("Hunter Rilfe", "Karabin Myśliwski");
                break;
            case 35:
                ItemName = SetString("DB Shotgun", "Dubeltówka");
                break;
            case 36:
                ItemName = SetString("Thompson", "Thompson");
                break;
            case 37:
                ItemName = SetString("SMG Magazine", "Magazynek do Pistoletów Maszynowych");
                break;
            case 38:
                ItemName = SetString("AK-47", "AK-47");
                break;
            case 39:
                ItemName = SetString("Rifle Magazine", "Magazynek do Karabinów");
                break;
            case 40:
                ItemName = SetString("Shotgun", "Strzelba");
                break;
            case 41:
                ItemName = SetString("MP5", "MP5");
                break;
            case 42:
                ItemName = SetString("M4", "M4");
                break;
            case 43:
                ItemName = SetString("Heat Pack", "Ogrzewacz");
                break;
            case 44:
                ItemName = SetString("Adrenaline", "Adrenalina");
                break;
            case 45:
                ItemName = SetString("Fanny Pack", "Torba na Pas");
                break;
            case 46:
                ItemName = SetString("Backpack", "Plecak");
                break;
            case 47:
                ItemName = SetString("Military Backpack", "Plecak Wojskowy");
                break;
            case 48:
                ItemName = SetString("Bulletproof Vest", "Kamizelka Kuloodporna");
                break;
            case 49:
                ItemName = SetString("Military Vest", "Kamizelka Wojskowa");
                break;
            case 990:
                ItemName = SetString("Teal", "Cyjan");
                break;
            case 991:
                ItemName = SetString("Grail", "Graal");
                break;
            case 992:
                ItemName = SetString("Travelstone", "Kamień Teleportacji");
                break;
            case 993:
                ItemName = SetString("Sapphire Spear", "Szafirowa Włócznia");
                break;
            case 994:
                ItemName = SetString("White Gold Armor", "Zbroja z Białego Złota");
                break;
            case 995:
                ItemName = SetString("Spark", "Iskra");
                break;
            case 996:
                ItemName = SetString("Golden AK-47", "Złoty AK-47");
                break;
            case 997:
                ItemName = SetString("Tesla Rifle", "Karabin Tesli");
                break;
            case 998:
                ItemName = SetString("Ruby Ring", "Rubinowy Pierścień");
                break;
            case 999:
                ItemName = SetString("Present", "Prezent");
                break;
            case 50:
                ItemName = SetString("Sleeping Bag", "Śpiwór");
                break;
            case 51:
                ItemName = SetString("Sport Shoes", "Buty Sportowe");
                break;
            case 52:
                ItemName = SetString("Money", "Pieniądze");
                break;
            case 53:
                ItemName = SetString("Night Vision Goggles", "Noktowizor");
                break;
            case 54:
                ItemName = SetString("Grappling Hook", "Hak Mocujący");
                break;
            case 55:
                ItemName = SetString("Sten", "Sten");
                break;
            case 56:
                ItemName = SetString("Garand", "Garand");
                break;
            case 57:
                ItemName = SetString("Famas", "Famas");
                break;
            case 58:
                ItemName = SetString("Uzi", "Uzi");
                break;
            case 59:
                ItemName = SetString("G3", "G3");
                break;
            case 60:
                ItemName = SetString("SCAR", "SCAR");
                break;
            case 61:
                ItemName = SetString("SPAS", "SPAS");
                break;
            case 62:
                ItemName = SetString("SAW", "SAW");
                break;
            case 63:
                ItemName = SetString("Ammo Chain", "Taśma Nabojowa");
                break;
            case 64:
                ItemName = SetString("Minigun", "Minigun");
                break;
            case 65:
                ItemName = SetString("Mosin Nagant", "Mosin Nagant");
                break;
            case 66:
                ItemName = SetString("Grenade", "Granat");
                break;
            case 67:
                ItemName = SetString("Panzerfaust", "Panzerfaust");
                break;
            case 68:
                ItemName = SetString("Chainsaw", "Piła Łańcuchowa");
                break;
            case 69:
                ItemName = SetString("Bow", "Łuk");
                break;
            case 70:
                ItemName = SetString("Baguette", "Bagieta");
                break;
            case 71:
                ItemName = SetString("Pickles", "Ogórki Kiszone");
                break;
            case 72:
                ItemName = SetString("Meat", "Mięso");
                break;
            case 73:
                ItemName = SetString("Pretzel", "Precel");
                break;
            case 74:
                ItemName = SetString("Cheeseburger", "Cheesburger");
                break;
            case 75:
                ItemName = SetString("Waffle", "Gofr");
                break;
            case 76:
                ItemName = SetString("Donut", "Donut");
                break;
            case 77:
                ItemName = SetString("Pâté", "Pasztet");
                break;
            case 78:
                ItemName = SetString("Crackers", "Krakersy");
                break;
            case 79:
                ItemName = SetString("Cola", "Kola");
                break;
            case 80:
                ItemName = SetString("Beer", "Piwo");
                break;
            case 81:
                ItemName = SetString("Vodka", "Wódka");
                break;
            case 82:
                ItemName = SetString("Potato", "Ziemniak");
                break;
            case 83:
                ItemName = SetString("Milk", "Mleko");
                break;
            case 84:
                ItemName = SetString("Biscuits", "Herbatniki");
                break;
            case 85:
                ItemName = SetString("Umbrella", "Parasol");
                break;
            case 86:
                ItemName = SetString("Hazmat Suit", "Kombinezon Materiałów Niebezpiecznych");
                break;
            case 87:
                ItemName = SetString("Lifebuoy", "Koło Ratunkowe");
                break;
            case 88:
                ItemName = SetString("Ducktape", "Taśma Klejąca");
                break;
            case 89:
                ItemName = SetString("Blowtorch", "Palnik");
                break;
            case 90:
                ItemName = SetString("Wrench", "Kluch");
                break;
            case 91:
                ItemName = SetString("Camera", "Aparat");
                break;
            case 92:
                ItemName = SetString("Binoculars", "Lornetki");
                break;
            case 93:
                ItemName = SetString("Cowbell", "Dzwon");
                break;
            case 94:
                ItemName = SetString("Scarf", "Szalik");
                break;
            case 95:
                ItemName = SetString("Riot Shield", "Tarcza Policyjna");
                break;
            case 96:
                ItemName = SetString("Cardboard Box", "Karton");
                break;
            case 97:
                ItemName = SetString("Lockpick", "Wytrych");
                break;
            case 98:
                ItemName = SetString("Towel", "Ręcznik");
                break;
            case 99:
                ItemName = SetString("Map", "Mapa");
                break;
            case 100:
                ItemName = SetString("Suppressor", "Tłumik");
                break;
            case 101:
                ItemName = SetString("Grip", "Uchwyt");
                break;
            case 102:
                ItemName = SetString("Grenade Launcher Attachment", "Montowalny Granatnik");
                break;
            case 103:
                ItemName = SetString("Sniper Scope", "Luneta Snajperska");
                break;
            case 104:
                ItemName = SetString("Holo Sight", "Holograficzny Celownik");
                break;
            case 105:
                ItemName = SetString("Bipod", "Dwójnóg");
                break;
            case 106:
                ItemName = SetString("Med Kit", "Apteczka");
                break;
            case 107:
                ItemName = SetString("Splint", "Szyna");
                break;
            case 108:
                ItemName = SetString("Plunger", "Przepychacz");
                break;
            case 109:
                ItemName = SetString("Flamethrower", "Miotacz Ognia");
                break;
            case 110:
                ItemName = SetString("Frag Grenade", "Granat Odłamkowy");
                break;
            case 111:
                ItemName = SetString("Grenade Launcher", "Granatnik");
                break;
            case 112:
                ItemName = SetString("Crossbow", "Kusza");
                break;
            case 113:
                ItemName = SetString("Musket", "Muszkiet");
                break;
            case 114:
                ItemName = SetString("Taser", "Paralizator");
                break;
            case 115:
                ItemName = SetString("Shovel", "Łopata");
                break;
            case 116:
                ItemName = SetString("Herring", "Śledź");
                break;
            case 117:
                ItemName = SetString("Salmon", "Łosoś");
                break;
            case 118:
                ItemName = SetString("Carp", "Karp");
                break;
            case 119:
                ItemName = SetString("Coconut", "Kokos");
                break;
            case 120:
                ItemName = SetString("Banana", "Banan");
                break;
            case 121:
                ItemName = SetString("Sandwich", "Kanapka");
                break;
            case 122:
                ItemName = SetString("Coffee", "Kawa");
                break;
            case 123:
                ItemName = SetString("Popsicle", "Lód na patyku");
                break;
            case 124:
                ItemName = SetString("Puffer", "Inhalator");
                break;
            case 125:
                ItemName = SetString("Scuba Tank", "Butla do Nurkowania");
                break;
            case 126:
                ItemName = SetString("Flippers", "Płetwy");
                break;
            case 127:
                ItemName = SetString("Crank Flashlight", "Latarka na Korbkę");
                break;
            case 128:
                ItemName = SetString("Fire Extinguisher", "Gaśnica");
                break;
            case 129:
                ItemName = SetString("Fishing Rod", "Wędka");
                break;
            case 130:
                ItemName = SetString("Scanner", "Skaner");
                break;
            case 131:
                ItemName = SetString("Flashbang", "Granat Hukowy");
                break;
            case 132:
                ItemName = SetString("Katana", "Katana");
                break;
            case 133:
                ItemName = SetString("Molotov Cocktail", "Koktajl Mołotowa");
                break;
            case 134:
                ItemName = SetString("Spear", "Włócznia");
                break;
            case 135:
                ItemName = SetString("G18", "G18");
                break;
            case 136:
                ItemName = SetString("Frying Pan", "Patelnia");
                break;
            case 137:
                ItemName = SetString("M1 Carbine", "Karabinek M1");
                break;
            case 138:
                ItemName = SetString("Sledgehammer", "Młot Wyburzeniowy");
                break;
            case 139:
                ItemName = SetString("Bazooka", "Bazooka");
                break;
            case 140:
                ItemName = SetString("Wood", "Drewno");
                break;
            case 141:
                ItemName = SetString("Thread", "Nić");
                break;
            case 142:
                ItemName = SetString("Stone", "Kamień");
                break;
            case 143:
                ItemName = SetString("Paper", "Papier");
                break;
            case 144:
                ItemName = SetString("Cloth", "Tkanina");
                break;
            case 145:
                ItemName = SetString("Metal", "Metal");
                break;
            case 146:
                ItemName = SetString("Clay", "Ziemia");
                break;
            case 147:
                ItemName = SetString("Coal", "Węgiel");
                break;
            default:
                ItemName = "";
                break;
            }

        return ItemName;

    }

    public string ReceiveItemVariables(float ItemID) {

        int ItemIDA = (int)ItemID;
        string VariablesToSet = "id" + ItemIDA.ToString() + ";";//new Vector3(ItemIDA, 0f, 0f);

        switch ((int)ItemIDA) {
            case 2: case  14: case  15: case  16: case  27: case  28: case  991: case  993: case 53: case 68: case 86: case  89: case 88: case 95: case 96: case 98: case 124: case 125: case 128: case 130: case 132: case 134: case 136: case 138:
                VariablesToSet += "va100;"; //new Vector3(ItemIDA, 100f, 0f);
                break;
           case 11: case 12:
                VariablesToSet += "cl" + ((int)Random.Range(0f, 10f)).ToString() + ";";//new Vector3(ItemIDA, 0f, (int)Random.Range(0f, 10f));
                break;
            case 13:
                VariablesToSet = "id12;va100;cl0;";
                break;
            case 29:
                VariablesToSet += "va7;";//new Vector3(ItemIDA, 7f, 0f);
                break;
            case 30:
                VariablesToSet += "va" + (4 * (int)Random.Range(1f, 4.9f)).ToString() + ";";//new Vector3(ItemIDA, (4 * (int)Random.Range(1f, 4.9f)), 0f);
                break;
            case 31:
                VariablesToSet += "va8;";//new Vector3(ItemIDA, 8f, 0f);
                break;
            case 32:
                VariablesToSet += "va6;";//new Vector3(ItemIDA, 6f, 0f);
                break;
            case 33:
                VariablesToSet += "va" + (5 * (int)Random.Range(1f, 10.9f)).ToString() + ";";//new Vector3(ItemIDA, (5 * (int)Random.Range(1f, 10.9f)), 0f);
                break;
            case 34:
                VariablesToSet += "va5;";//new Vector3(ItemIDA, 5f, 0f);
                break;
            case 35:
                VariablesToSet += "va2;";//new Vector3(ItemIDA, 2f, 0f);
                break;
            case 36:
                VariablesToSet += "va30;";//new Vector3(ItemIDA, 30f, 0f);
                break;
            case 37:
                VariablesToSet += "va" + (15 * (int)Random.Range(1f, 4.9f)).ToString() + ";";//new Vector3(ItemIDA, (15 * (int)Random.Range(1f, 4.9f)), 0f);
                break;
            case 38: case 996:
                VariablesToSet += "va30;";//new Vector3(ItemIDA, 30f, 0f);
                break;
            case 39:
                VariablesToSet += "va" + (15 * (int)Random.Range(1f, 4.9f)).ToString() + ";";//new Vector3(ItemIDA, (15 * (int)Random.Range(1f, 4.9f)), 0f);
                break;
            case 40:
                VariablesToSet += "va5;";//new Vector3(ItemIDA, 5f, 0f);
                break;
            case 41:
                VariablesToSet += "va40;";//new Vector3(ItemIDA, 40f, 0f);
                break;
            case 42:
                VariablesToSet += "va30;";//new Vector3(ItemIDA, 30f, 0f);
                break;
            case 55:
                VariablesToSet += "va32;";//new Vector3(ItemIDA, 32f, 0f);
                break;
            case 56:
                VariablesToSet += "va8;";//new Vector3(ItemIDA, 8f, 0f);
                break;
            case 57:
                VariablesToSet += "va25;";//new Vector3(ItemIDA, 25f, 0f);
                break;
            case 58:
                VariablesToSet += "va25;";//new Vector3(ItemIDA, 25f, 0f);
                break;
            case 59:
                VariablesToSet += "va20;";//new Vector3(ItemIDA, 20f, 0f);
                break;
            case 60:
                VariablesToSet += "va28;";//new Vector3(ItemIDA, 28f, 0f);
                break;
            case 61:
                VariablesToSet += "va8;";//new Vector3(ItemIDA, 8f, 0f);
                break;
            case 62:
                VariablesToSet += "va100;";//new Vector3(ItemIDA, 100f, 0f);
                break;
            case 63:
                VariablesToSet += "va" + (250 * (int)Random.Range(1f, 3.9f)).ToString() + ";";//new Vector3(ItemIDA, (250 * (int)Random.Range(1f, 3.9f)), 0f);
                break;
            case 64:
                VariablesToSet += "va500;";//new Vector3(ItemIDA, 500f, 0f);
                break;
            case 65:
                VariablesToSet += "va5;";//new Vector3(ItemIDA, 5f, 0f);
                break;
            case 66: case 110: case 127: case 131:
                VariablesToSet += "va0;sq1;";//new Vector3(ItemIDA, 5f, 0f);
                break;
            case 69:
                VariablesToSet += "va10;";//new Vector3(ItemIDA, 10f, 0f);
                break;
            case 91:
                VariablesToSet += "va10;";//new Vector3(ItemIDA, 10f, 0f);
                break;
            case 99:
                VariablesToSet += "va" + ((int)Random.Range(1f, 3f)).ToString() + ";";//new Vector3(ItemIDA, (int)Random.Range(1f, 3f), 0f);
                break;
            case 108:
                VariablesToSet += "va100;";//new Vector3(ItemIDA, 100f, 0f);
                break;
            case 109:
                VariablesToSet += "va100;";//new Vector3(ItemIDA, 100f, 0f);
                break;
            case 111:
                VariablesToSet += "va6;";//new Vector3(ItemIDA, 6f, 0f);
                break;
            case 112:
                VariablesToSet += "va10;";//new Vector3(ItemIDA, 10f, 0f);
                break;
            case 113:
                VariablesToSet += "va1;";//new Vector3(ItemIDA, 1f, 0f);
                break;
            case 115:
                VariablesToSet += "va100;";//new Vector3(ItemIDA, 100f, 0f);
                break;
            case 135:
                VariablesToSet += "va17;";//new Vector3(ItemIDA, 17f, 0f);
                break;
            case 137:
                VariablesToSet += "va15;";//new Vector3(ItemIDA, 15f, 0f);
                break;
            case 139:
                VariablesToSet += "va6;";//new Vector3(ItemIDA, 6f, 0f);
                break;
            default:
                VariablesToSet += "sq1;";//new Vector3(ItemIDA, 0f, 0f);
                break;
        }

        if(!ExistSemiClass(VariablesToSet, "va")) VariablesToSet += "va0;";
        return VariablesToSet;

    }

    public float SeedPerlin(string Seed, Vector2 Offset = default){
        if(Offset == default) Offset = Vector2.one/2f;
        string[] sh = new string[]{ Seed.Substring(0, (int)(Seed.Length/2f)), Seed.Substring((int)(Seed.Length/2f), Seed.Length - (int)(Seed.Length/2f) ) };
        return Mathf.Clamp((Mathf.PerlinNoise(   
            Offset.x * float.Parse(sh[0]) / Mathf.Pow(10, (int)Mathf.Clamp(sh[0].Length/2f, 1, Mathf.Infinity)),   
            Offset.y * float.Parse(sh[1]) / Mathf.Pow(10, (int)Mathf.Clamp(sh[1].Length/2f, 1, Mathf.Infinity))
        ) * 2f)-0.5f, 0f, 1f);
    }

    public float FixedPerlinNoise(float X, float Y) {

        float ToSet = Mathf.Clamp((Mathf.PerlinNoise(X, Y) * 2f) - 0.5f, 0f, 1f);
        return ToSet;

    }

    public float ReceiveButtonPress(string ButtonName, string ButtonFunc) {

        float Returned = 0f;
        bool Exist = false;
        foreach (KeyBind CheckControll in Controlls) {
            if (CheckControll.Name == ButtonName) {
                Exist = true;
                if (Input.GetKey(CheckControll.Key) && ButtonFunc == "Hold") {
                    Returned = 1f;
                } else if (Input.GetKeyDown(CheckControll.Key) && ButtonFunc == "Press") {
                    Returned = 1f;
                } else if (Input.GetKeyUp(CheckControll.Key) && ButtonFunc == "Release") {
                    Returned = 1f;
                }
            }
        }
        if (Exist == false) {
            Debug.LogError("The button '" + ButtonName + "' was not assigned.");
        }
        return Returned;

    }

    // The Semi-Class functions

    public string GetSemiClass(string From, string What, string Separator = ";"){

        // Bugtest semiclasses
        //print("GetSemiClass( from: " + From + " - what: " + What + ")");
        //Debug.Break();

        string Result = "null";//"null";

        if (From != "" && What != "") {
            int SepLeng = Separator.Length;
            if(From.Substring(From.Length - SepLeng) != Separator) From += Separator;

            string Parm = "";
            for(int Scan = 0; Scan < From.Length; Scan ++){
                if(From.Substring(Scan, SepLeng) == Separator && Result != "null") break;
                else if(Parm == What) Result += From.Substring(Scan, 1);
                else if(From.Substring(Scan, SepLeng) == Separator) {
                    Parm = "";
                    //Scan += SepLeng;
                }
                else Parm += From.Substring(Scan, 1);

                if(Parm == What && Result == "null") Result = "";
            }
        }
        if(Result == "null") Result = "";

        return Result;

    }

    public bool ExistSemiClass(string From, string What, string Separator = ";"){

        bool Result = false;

        if (From != "" && What != "") {
            int SepLeng = Separator.Length;
            if(From.Substring( (int)Mathf.Clamp(From.Length - SepLeng, 0, Mathf.Infinity) ) != Separator) From += Separator;

            string Parm = "";
            for(int Scan = 0; Scan < From.Length; Scan ++){
                if(Parm == What) {Result = true; break;}
                else if(From.Substring(Scan, SepLeng) == Separator) {
                    Parm = "";
                    //Scan += SepLeng;
                }
                else Parm += From.Substring(Scan, 1);

            }
        }

        return Result;

    }

    public string SetSemiClass(string From, string What, string To, string Separator = ";"){

        string Result = From;
        string Prev = "";

        if (What != "") {
            int SepLeng = Separator.Length;
            if(From.Length < SepLeng || From.Substring(From.Length - SepLeng) != Separator) From += Separator;
            

            int[] Has = {0, 0}; // index of where it is
            string IfHas = "";
            bool GotWhat = false;
            for(int Scan = 0; Scan < From.Length; Scan ++){
                if(From.Substring(Scan, SepLeng) == Separator && GotWhat) {Has[1] = Scan; break;}
                else if(From.Substring(Scan, SepLeng) == Separator) {
                    IfHas = ""; 
                    //Scan += SepLeng;
                    Has[0] = Scan + 1;
                }
                else IfHas += From.Substring(Scan, 1);

                if(IfHas == What) GotWhat = true;
            }

            if(GotWhat) Prev = From.Substring( Has[0] + What.Length, Has[1] - (Has[0] + What.Length) );

            // Math
            if(To.Substring(0, 1) == "/"){
                if(To.Substring(0, 2) == "/+"){
                    if(Prev == "") Prev = "0";
                    To = (float.Parse(Prev, CultureInfo.InvariantCulture) + float.Parse(To.Substring(2), CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture);
                } else if(To.Substring(0, 2) == "/*"){
                    if(Prev == "") Prev = "0";
                    To = (float.Parse(Prev, CultureInfo.InvariantCulture) * float.Parse(To.Substring(2), CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture);
                }
            }

            if(!GotWhat){
                Result += What + To + Separator;
            } else {
                string inject = What + To + Separator;
                string Before = From.Substring(0, Has[0]);
                string After = From.Substring(Has[1] + SepLeng);
                Result = Before + inject + After;
            }
        }

        return Result;

    }

    public string RemoveSemiClass(string From, string What, string Separator = ";"){

    // Bugtest semiclasses
    //print("RemoveSemiClass( from: " + From + " - what: " + What + ")");
    //Debug.Break();

        string Result = "";
        int SepLeng = Separator.Length;
        if(From.Substring(From.Length - SepLeng) != Separator) From += Separator;
        int[] Has = {0, 0}; // index of where it is

        string IfHas = "";
        bool GotIt = false;
        for(int Scan = 0; Scan < From.Length; Scan ++){
            if(From.Substring(Scan, SepLeng) == Separator && IfHas == What) {Has[1] = Scan + 1; GotIt = true; break;}
            else if(From.Substring(Scan, SepLeng) == Separator) {
                IfHas = ""; 
                Has[0] = Scan + 1;
            }
            else if (IfHas != What) IfHas += From.Substring(Scan, 1);
        }

        //print(From + " Has{" + Has[0] + " - " + Has[1] + "} " + GotIt);
        if(GotIt){
            string Before = From.Substring(0, Has[0]);
            string After = From.Substring(Has[1]);
            Result = Before + After;
        } else Result = From;
        //print(Result);

        return Result;
    }

    public string[] ListSemiClass(string From, string Separator = ";"){

        // Bugtest semiclasses
        //print("ListSemiClass( from: " + From + ")");
        //Debug.Break();

        string[] Results = new string[]{};
        if(From != ""){
            int SepLeng = Separator.Length;
            if(From.Substring(From.Length - SepLeng) != Separator) From += Separator;

            List<string> GetList = new List<string>();
            string Receive = "";
            for(int Scan = 0; Scan < From.Length; Scan ++){
                if(From.Substring(Scan, SepLeng) == Separator) {
                    GetList.Add(Receive); 
                    Receive = "";
                } 
                else Receive += From.Substring(Scan, 1);
            }
            Results = GetList.ToArray();
        }
        return Results;

    }

    public string GetStatName(string stat, int what = 0){

        string Result = "null";

        string[] TypeScore = {"", "?"};
        for(int ss = 0; ss < stat.Length; ss++)
            if(stat.Substring(ss, 1) == "_") TypeScore[1] = "";
            else if (TypeScore[1] == "?") TypeScore[0] += stat.Substring(ss, 1);
            else TypeScore[1] += stat.Substring(ss, 1);
                        
            switch(TypeScore[0]){
                case "ItemsFound":
                    Result = SetString("Items found: ", "Znalezione przedmioty: ") + TypeScore[1];
                    break;
                case "Killed":
                    Result = SetString("Killed: ", "Zabitych: ") + TypeScore[1];
                    break;
                case "KillMutant":
                    Result = SetString("Mutants killed: ", "Zabitych mutantów: ") + TypeScore[1];
                    break;
                case "KillBandit":
                    Result = SetString("Bandits killed: ", "Zabitych bandytów: ") + TypeScore[1];
                    break;
                case "KillGuard":
                    Result = SetString("Guards killed: ", "Zabitych strażników: ") + TypeScore[1];
                    break;
                case "KillSurvivor":
                    Result = SetString("Survivors killed: ", "Zabitych niedobitków: ") + TypeScore[1];
                    break;
                case "Damage":
                    Result = SetString("Damage dealt: ", "Zadanych obrażeń: ") + TypeScore[1];
                    break;
                case "MapDiscovered":
                    Result = SetString("Map discovered: ", "Odkrycie mapy: ") + TypeScore[1];
                    break;
                case "SurvivedTime":
                    Result = SetString("Survived time: ", "Przetrwany czas:") + SecondsToTime(int.Parse(TypeScore[1]));
                    break;
                case "TotalRounds":
                    Result = SetString("Total amount of rounds: ", "Całkowita liczba przetrwanych rund: ") + TypeScore[1];
                    break;
                case "TotalWaves":
                    Result = SetString("Total amount of horde waves: ", "Całkowita liczba przetrwanych fal hordy: ") + TypeScore[1];
                    break;
                case "TotalScore":
                    Result = SetString("Total amount of gained score: ", "Suma uzbieranych wyników: ") + TypeScore[1];
                    break;
                case "HighestScore":
                    Result = SetString("The highest score: ", "Najwyższy wynik: ") + TypeScore[1];
                    break;
                case "MostRounds":
                    Result = SetString("Most rounds in a row: ", "Najwięcej przeżytych rund: ") + TypeScore[1];
                    break;
                case "MostWaves":
                    Result = SetString("Most horde waves in a row: ", "Najwięcej przeżytych fal hordy: ") + TypeScore[1];
                    break;
                case "LongestSurvivedTime":
                    Result = SetString("Longest time survived: ", "Najdłuższy czas przetrwany:") + SecondsToTime(int.Parse(TypeScore[1]));
                    break;
                case "ProfileIndependent":
                    Result = SetString("Profile independent - those stats won't be saved", "Niezależne od profilu - te statystyki nie zostaną zapisane");
                    break;
                case "TreasuresSold":
                    Result = SetString("Treasures sold: ", "Sprzedane skarby: ") + TypeScore[1];
                    break;
                case "Trade":
                    Result = SetString("Trades: ", "Handle: ") + TypeScore[1];
                    break;
                case "":
                    Result = "";
                    break;
                default:
                    Debug.LogError("Uknown stat type: " + stat);
                    TypeScore[0] = "misc.";
                    break;
            }

        if(what == 2) return TypeScore[1];
        else if(what == 1) return TypeScore[0];
        else return Result;

    }

    public void Mess(string Message, string TypeOfMessage = ""){
        GameObject.Find("_NewMenu").GetComponent<NewMenuScript>().Pop(Message, TypeOfMessage);
    }

    string SecondsToTime(int Second){
        string TimeToDisp = "";
        if(Second <= 60){
            TimeToDisp = Second.ToString();
        } else if (Second <= 3600){
            string Sex = "0" + (Second%60).ToString();
            Sex = Sex.Substring(Sex.Length-2, 2);
            TimeToDisp = ((int)(Second/60f)).ToString() + ":" + Sex;
        } else {
            string Minor = "0" + ((int)((Second/60f)%60f)).ToString();
            string Sex = "0" + (Second%60).ToString();
            Sex = Sex.Substring(Sex.Length-2, 2);
            TimeToDisp = ((int)(Second/3600f)).ToString() + ":" + Minor + ":" + Sex;
        }
        return TimeToDisp;
    }

}


