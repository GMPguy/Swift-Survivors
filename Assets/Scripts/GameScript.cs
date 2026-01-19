using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using System.Globalization;
using Unity.Mathematics;
using Random=UnityEngine.Random;
using UnityEngine.Audio;

public class GameScript : MonoBehaviour {

    // Main Variables
    public string Version = "1.0";
    public int Platform = 2; // 1 Desktop   2 WebGL
    // Main Variables

    // Game Variables
    public string SaveFileName = "";
    public int FileSeed = 0;
    public string RoundSetting = "G0?D0?P1";
    public int2 GameModePrefab = new (0, 0);
    public int Round = 0;
    public int Biome = 0;
    public int Score = 0;
    public int Money = 0;
    public int Ammo = 0;
    public int RoundsSeed = 0;
    public Vector2 HealthSave;
    public Vector2 HungerSave;
    public float PlayerSpeed;
    public JClass PlayerInvEq;
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

    public int DestructionQuality = 1; // Disabled, Standard, High
    public int EffectsQuality = 1; // Disabled, Standard, High
    public int FPScap = 60;

    public float CameraBobbing = 1f;
    public float CameraShifting = .5f;
    public float FOV = 50f;
    public bool Ragdolls = true;
    public Vector2 MainResolution = new Vector2(800f, 600f);
    public Vector2 UIResolution = new Vector2(800f, 600f);
    int PrevSkyboxType = 0;
    public int SkyboxType = 0;

    public static List<InstantLODScript> CachedLODs;
    // Volume
    public AudioMixer mainMixer;
    string[] mixerChannels = {"Master", "Music", "Sounds", "EnvSounds"};
    public float MuteVolume = 0f;
    public float[] Volumes = {1f, 1f, 1f, 1f}; // Master, Music, Sound, EnvSound
    float[] prevVolumes = {-1f, -1f, -1f, -1f};
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
    public string NewProfileName;
    public bool CanSaveStuff = false;
    List<KeyCode> TempDevCommand;
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

    // Profile messages
    public List<string> Messages; // ti_Good;de_Description;vi_BonusVisuals;im_0; - importance levels 0-not 1-save 2-popup 3-popupinstant

    // Improved item classification
    public itemClass[] ItemCache;
    public class itemClass{
        GameScript GS;
        public JClass startVariables;
        public Vector3 ThrowVariables = new(10f, 0f, 0f); // throw speed, change of breaking, damage
        public int imageIndex; // 0 normal, 1 counter, 2 bar
        public string[] Name, Desc;
        public itemClass(GameScript sGS, string[] sName, string[] sDesc, JClass startervariables, Vector3 setThrow = default, int setImage = 0){
            GS = sGS;
            Name = sName; 
            Desc = sDesc;
            startVariables = new (startervariables);
            if(setThrow != default) ThrowVariables = setThrow;
            imageIndex = setImage;
        }
        public itemClass(){}

        public string getName() => GS.SetString(Name[0], Name[1]);
        public string getNameEng() => Name[0];
        public string getDesc() => GS.SetString(Desc[0], Desc[1]);
    }
    // Improved item classification

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

            if (Application.systemLanguage == SystemLanguage.Polish)
                Language = "Polski";
            else
                Language = "English";

            if (PlayerPrefs.HasKey("Options")) {
                SaveSettings(1);
            }

            // Set resolutions
            for (int rs = 0; rs < Screen.resolutions.Length; rs++)
                if(Screen.resolutions[rs].refreshRate == 60 && Screen.resolutions[rs].width >= 640f && Screen.resolutions[rs].height >= 480f) 
                    Resolutions.Add(new Vector2(Screen.resolutions[rs].width, Screen.resolutions[rs].height));

            if (SceneManager.GetActiveScene().name == "NewMenu")
                NewMenuScript.LoadingAdditionalInfo = "";

            CachedLODs = new List<InstantLODScript>();
        }
		
	}

    void Update() {

        SettingsInAction();

        // Temp dev comand
        if (Input.GetKey(KeyCode.C)) {
            if (TempDevCommand == null)
                TempDevCommand = new ();

            if (TempDevCommand.Count < 10)
                foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
                    if (Input.GetKeyDown(kcode) && kcode != KeyCode.C)
                        TempDevCommand.Add(kcode);
        } else if (TempDevCommand != null) {

            string readValue = "";
            for (int c = 0; c < TempDevCommand.Count; c++)
                readValue += TempDevCommand[c].ToString();
            
            switch (readValue) {
                case "DELETEALL":
                    CanSaveStuff = false;
                    PlayerPrefs.DeleteAll();
                    Mess("Please exit the game");
                    Mess("All data has been wiped - new data will not be saved");
                    break;
            }

            TempDevCommand = null;
        }

        if (SaveSettingsTime > 0f) {
            SaveSettingsTime -= Time.unscaledDeltaTime;
        } else {
            SaveSettingsTime = 60f;
            SaveSettings(0);
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

        float multiplier = GetSemiClass(RoundSetting, "G") switch {
            "0" => .25f,
            _ => 1f
        };

        multiplier *= GetSemiClass(RoundSetting, "D") switch {
            "0" => .5f,
            "2" => 1.5f,
            "3" => 2f,
            "4" => 4f,
            _ => 1f
        };


        PS.GitExp(Mathf.RoundToInt(ScoreToAdd * multiplier));
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
            
            
            // Get new round seed
            if (WayOfChange == "EscapeMap") {
                Random.InitState(FileSeed);
                for (int r = 0; r < Round; r++)
                    for (int q = 4; q > 0; q--)
                        _ = Random.value;
            }
            
            RoundsSeed = Random.Range(int.MinValue / 2, int.MaxValue / 2);
            Biome = AvailableBiomes.ToArray()[(int)Random.Range(0f, AvailableBiomes.ToArray().Length - 0.1f)];
            
            HealthSave = new Vector2(FoundPlayer.GetComponent<PlayerScript>().Health[0], FoundPlayer.GetComponent<PlayerScript>().Health[1]);
            HungerSave = new Vector2(FoundPlayer.GetComponent<PlayerScript>().Food[0], FoundPlayer.GetComponent<PlayerScript>().Food[1]);
            PlayerSpeed = FoundPlayer.GetComponent<PlayerScript>().Speed;
            PlayerInvEq = FoundPlayer.GetComponent<PlayerScript>().InvEqCache;

            MaxInventory = FoundPlayer.GetComponent<PlayerScript>().MaxInventorySlots;
            PlayerBuffs = FoundPlayer.GetComponent<PlayerScript>().BuffsText;

            //Saves(CurrentSave, 0);
            SaveManipulation(CurrentSave, 0);
            WhatOnStart = 1;

            SceneManager.LoadScene("MainGame");
            WindowToBootUp = "MainMenu";

        } else if (WayOfChange == "NewGame") {

            //Saves(CurrentSave, 2);
            SaveManipulation(CurrentSave, 2);
            PlaythroughStats = "";
            WhatOnStart = 0;

            SceneManager.LoadScene("MainGame");
            WindowToBootUp = "MainMenu";

        } else if (WayOfChange == "LoadGame") {

            //Saves(CurrentSave, 1);
            SaveManipulation(CurrentSave, 1);
            WhatOnStart = 1;

            SceneManager.LoadScene("MainGame");
            WindowToBootUp = "MainMenu";

        } else if (WayOfChange == "GameOver") {

            SceneManager.LoadScene("NewMenu");
            WindowToBootUp = "GameOver";

        } else if (WayOfChange == "BackToMenu") {

            SceneManager.LoadScene("NewMenu");
            WindowToBootUp = "MainMenu";

        } else if (WayOfChange == "BootMenu") {

            SceneManager.LoadScene("NewMenu");
            WindowToBootUp = "BootMenu";

        }

    }

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
                + "®am" + Ammo.ToString()

                + "®fs" + FileSeed.ToString()
                + "®sr" + RoundsSeed.ToString()

                + "®l0" + HealthSave.x.ToString()
                + "®l1" + HealthSave.y.ToString()

                + "®h0" + HungerSave.x.ToString()
                + "®h1" + HungerSave.y.ToString()

                + "®ps" + PlayerSpeed.ToString()
                + "®pi" + JsonUtility.ToJson(PlayerInvEq)
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
                    Ammo = int.Parse(GetSemiClass(Receiver, "am", "®"));

                    FileSeed = int.Parse(GetSemiClass(Receiver, "fs", "®"));
                    RoundsSeed = int.Parse(GetSemiClass(Receiver, "sr", "®"));

                    HealthSave.x = float.Parse(GetSemiClass(Receiver, "l0", "®"));
                    HealthSave.y = (int)float.Parse(GetSemiClass(Receiver, "l1", "®"));

                    HungerSave.x = (int)float.Parse(GetSemiClass(Receiver, "h0", "®"));
                    HungerSave.y = (int)float.Parse(GetSemiClass(Receiver, "h1", "®"));

                    PlayerSpeed = float.Parse(GetSemiClass(Receiver, "ps", "®"));
                    PlayerInvEq = JsonUtility.FromJson<JClass>(GetSemiClass(Receiver, "pi", "®"));//GetSemiClass(Receiver, "pi", "®");
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
            case 4: // DeleteAll
                PlayerPrefs.DeleteKey("Saves");
                break;
        }
        return "";

    }

    public void UpdateRecord(){

        NeueScore[0] = SetSemiClass(NeueScore[0], "R", "/+-1");

        // Remove rewards and whatnot
        if(GetSemiClass(NeueScore[0], "P") != "1"){
            if(GetSemiClass(NeueScore[0], "G") == "0") PS.Statistics = SetSemiClass(PS.Statistics, "TotalRounds_", "/+" + GetSemiClass(NeueScore[0], "R"));
            else if(GetSemiClass(NeueScore[0], "G") == "2") PS.Statistics = SetSemiClass(PS.Statistics, "TotalCasualRounds_", "/+" + GetSemiClass(NeueScore[0], "R"));
            else PS.Statistics = SetSemiClass(PS.Statistics, "TotalWaves_", "/+" + GetSemiClass(NeueScore[0], "R"));
            PS.Statistics = SetSemiClass(PS.Statistics, "TotalScore_", "/+" + GetSemiClass(NeueScore[0], "S"));

            if ((ExistSemiClass(NeueScore[0], "S") && int.Parse(GetSemiClass(NeueScore[0], "S")) > 0) && (!ExistSemiClass(PS.Statistics, "HighestScore_") || (float.Parse(GetSemiClass(NeueScore[0], "S")) > float.Parse(GetSemiClass(PS.Statistics, "HighestScore_"))))) {
                PS.Statistics = SetSemiClass(PS.Statistics, "HighestScore_", GetSemiClass(NeueScore[0], "S"));
            }

            if ((ExistSemiClass(NeueScore[0], "R") && int.Parse(GetSemiClass(NeueScore[0], "R")) > 0 && GetSemiClass(NeueScore[0], "G") == "0") && (!ExistSemiClass(PS.Statistics, "MostRounds_") || (float.Parse(GetSemiClass(NeueScore[0], "R")) > float.Parse(GetSemiClass(PS.Statistics, "MostRounds_"))))) {
                PS.Statistics = SetSemiClass(PS.Statistics, "MostRounds_", GetSemiClass(NeueScore[0], "R"));
            } else if ((ExistSemiClass(NeueScore[0], "R") && int.Parse(GetSemiClass(NeueScore[0], "R")) > 0 && GetSemiClass(NeueScore[0], "G") == "2") && (!ExistSemiClass(PS.Statistics, "MostCasualRounds_") || (float.Parse(GetSemiClass(NeueScore[0], "R")) > float.Parse(GetSemiClass(PS.Statistics, "MostCasualRounds_"))))) {
                PS.Statistics = SetSemiClass(PS.Statistics, "MostCasualRounds_", GetSemiClass(NeueScore[0], "R"));
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

        Application.targetFrameRate = FPScap > 0 ? FPScap : 999;
        bool trimS = false; bool trimP = false; bool trimL = false;

        // Volumes
        float Deafening = Mathf.Clamp(Earpiercing[0] / Earpiercing[1], 0f, 1f);
        if (EarpiercingAllowed) {
            if (GameObject.Find("Earpiercing") == null) {
                Earpiercing[0] = 0f;
            } else {
                GameObject.Find("Earpiercing").GetComponent<AudioSource>().volume = Deafening * Volumes[2] * Volumes[0];
            }
            if (Earpiercing[0] > 0f) {
                Earpiercing[0] -= 0.02f * (Time.deltaTime * 50f);
            } else if (Earpiercing[1] != 0.01f) {
                Earpiercing[1] = 0.01f;
            }
        } else {
            Deafening = 0f;
            GameObject.Find("Earpiercing").GetComponent<AudioSource>().volume = 0f;
        }

        Volumes[3] = Mathf.Clamp(Volumes[2] - (Deafening * 2f), 0f, 1f);
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().IsSwimming == true) {
                Volumes[3] /= 2f;
            }
        }

        for(int sv = 0; sv < 4; sv++) if (prevVolumes[sv] != Volumes[sv]){
            prevVolumes[sv] = Volumes[sv];
            if(Volumes[sv] > 0.01f)
                mainMixer.SetFloat(mixerChannels[sv], Mathf.Log10(Volumes[sv])*20f);
            else
                mainMixer.SetFloat(mixerChannels[sv], -80f);
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
            } else if (QualitySettings.GetQualityLevel() == 1) {
                // Low
                PPBloom.enabled.value = false;
                PPAmbient.enabled.value = false;
                PPBlur.enabled.value = false;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = false;
            } else if (QualitySettings.GetQualityLevel() == 2) {
                // Medium
                PPBloom.enabled.value = true;
                PPAmbient.enabled.value = false;
                PPBlur.enabled.value = false;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = false;
            } else if (QualitySettings.GetQualityLevel() == 3) {
                // Good
                PPBloom.enabled.value = true;
                PPAmbient.enabled.value = true;
                PPBlur.enabled.value = false;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = false;
            } else if (QualitySettings.GetQualityLevel() == 4) {
                // High
                PPBloom.enabled.value = true;
                PPAmbient.enabled.value = true;
                PPBlur.enabled.value = true;
                PPColorGrading.enabled.value = true;
                PPVignette.enabled.value = true;
                PPDepth.enabled.value = true;
            }

            // Update LODs
            for (int ul = CachedLODs.Count - 1; ul >= 0; ul--)
                if (CachedLODs[ul])
                    CachedLODs[ul].SetLevel(false);
                else
                    CachedLODs.RemoveAt(ul);
            
            PrevGraphSet = GraphicsQuality;
        }

        // Update dynamic graphic settings
        if (QualitySettings.GetQualityLevel() == 1) {
            // Low
            PPColorGrading.contrast.value = ContSaturTempInvi[0];
            PPColorGrading.saturation.value = ContSaturTempInvi[1];
            PPColorGrading.colorFilter.value = PPColor;
            PPVignette.intensity.value = ContSaturTempInvi[3];
        } else if (QualitySettings.GetQualityLevel() == 2) {
            // Medium
            PPColorGrading.contrast.value = ContSaturTempInvi[0];
            PPColorGrading.saturation.value = ContSaturTempInvi[1];
            PPColorGrading.temperature.value = ContSaturTempInvi[2];
            PPColorGrading.colorFilter.value = PPColor;
            PPVignette.intensity.value = ContSaturTempInvi[3];
        } else if (QualitySettings.GetQualityLevel() == 3) {
            // Good
            PPColorGrading.contrast.value = ContSaturTempInvi[0];
            PPColorGrading.saturation.value = ContSaturTempInvi[1];
            PPColorGrading.temperature.value = ContSaturTempInvi[2];
            PPColorGrading.colorFilter.value = PPColor;
            PPVignette.intensity.value = ContSaturTempInvi[3];
        } else if (QualitySettings.GetQualityLevel() == 4) {
            // High
            PPColorGrading.contrast.value = ContSaturTempInvi[0];
            PPColorGrading.saturation.value = ContSaturTempInvi[1];
            PPColorGrading.temperature.value = ContSaturTempInvi[2];
            PPColorGrading.colorFilter.value = PPColor;
            PPVignette.intensity.value = ContSaturTempInvi[3];
            PPDepth.focusDistance.value = CameraFocus[0];
            PPDepth.aperture.value = CameraFocus[1];
            PPDepth.focalLength.value = CameraFocus[2];
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
                ";EarPier_" + EarpiercingAllowed.ToString(CultureInfo.InvariantCulture) +
                ";RagDoll_" + Ragdolls.ToString(CultureInfo.InvariantCulture) +
                ";CamBob_" + CameraBobbing.ToString(CultureInfo.InvariantCulture) +
                ";CamShi_" + CameraShifting.ToString(CultureInfo.InvariantCulture) +
                ";MasterV_" + Volumes[0].ToString(CultureInfo.InvariantCulture) +
                ";SoundV_" + Volumes[2].ToString(CultureInfo.InvariantCulture) +
                ";MusicV_" + Volumes[1].ToString(CultureInfo.InvariantCulture) +
                ";FOV_" + FOV.ToString(CultureInfo.InvariantCulture) +
                ";LaserColor_" + LaserColor.ToString(CultureInfo.InvariantCulture) +
                ";HoloSight_" + HoloSightImage.ToString(CultureInfo.InvariantCulture) +
                ";GrapQ_" + GraphicsQuality.ToString(CultureInfo.InvariantCulture) +
                ";FoliQ_" + GrassQuality.ToString(CultureInfo.InvariantCulture) +
                ";PartQ_" + ParticlesQuality.ToString(CultureInfo.InvariantCulture) +
                ";LiteQ_" + LightingQuality.ToString(CultureInfo.InvariantCulture) +
                ";FPScap_" + FPScap.ToString(CultureInfo.InvariantCulture) +
                ";DestQ_" + DestructionQuality.ToString(CultureInfo.InvariantCulture) +
                ";EffeQ_" + EffectsQuality.ToString(CultureInfo.InvariantCulture) +
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

                string MessagesToSave = "";
                for (int getMess = 0; getMess < Messages.Count; getMess++)
                    MessagesToSave += Messages[getMess] + "^";
                PlayerPrefs.SetString("Messages", MessagesToSave);

                break;
            case 1: // LoadEverything

                if(PlayerPrefs.HasKey("Options")){
                    Language = GetSemiClass(PlayerPrefs.GetString("Options"), "Lang_" );
                    MouseSensitivity = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "Msens_" ), CultureInfo.InvariantCulture);
                    MouseSmoothness = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "Msmooth_" ), CultureInfo.InvariantCulture);
                    InvertedMouse = bool.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "Minv_" ));
                    EarpiercingAllowed = bool.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "EarPier_" ));
                    Ragdolls = bool.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "RagDoll_" ));
                    CameraBobbing = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "CamBob_" ), CultureInfo.InvariantCulture);
                    CameraShifting = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "CamShi_" ), CultureInfo.InvariantCulture);
                    Volumes[0] = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "MasterV_" ), CultureInfo.InvariantCulture);
                    Volumes[2] = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "SoundV_" ), CultureInfo.InvariantCulture);
                    Volumes[1] = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "MusicV_" ), CultureInfo.InvariantCulture);
                    FOV = float.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "FOV_" ), CultureInfo.InvariantCulture);
                    LaserColor = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "LaserColor_" ), CultureInfo.InvariantCulture);
                    HoloSightImage = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "HoloSight_" ), CultureInfo.InvariantCulture);
                    GraphicsQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "GrapQ_" ), CultureInfo.InvariantCulture);
                    GrassQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "FoliQ_" ), CultureInfo.InvariantCulture);
                    ParticlesQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "PartQ_" ), CultureInfo.InvariantCulture);
                    LightingQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "LiteQ_" ), CultureInfo.InvariantCulture);
                    FPScap = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "FPScap_" ), CultureInfo.InvariantCulture);
                    DestructionQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "DestQ_" ), CultureInfo.InvariantCulture);
                    EffectsQuality = int.Parse( GetSemiClass(PlayerPrefs.GetString("Options"), "EffeQ_" ), CultureInfo.InvariantCulture);
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

                if(PlayerPrefs.HasKey("Messages")){
                    foreach(string loadMessage in ListSemiClass(PlayerPrefs.GetString("Messages"), "^"))
                        Messages.Add(loadMessage);
                }

                break;
            case 2: // DeleteEverything
                PlayerPrefs.DeleteKey("Saves");
                PlayerPrefs.DeleteKey("Options");
                PlayerPrefs.DeleteKey("Messages");
                PlayerPrefs.DeleteKey("PrevProfile");
                PlayerPrefs.DeleteKey("ProfileSaves");
                break;
        }

    }

    // Item functions
    public void setItemData(bool isCausal){
        //JType casualAmmo = isCausal ? JType.CasualAmmo : JType.AmmoStack;
        JEntry casualAmmo = isCausal ? new JString(JType.Cacheable, "Ammo") : new JTag(JType.AmmoStack);

        ItemCache = new itemClass[]{
            new(),
            new(this, new string[]{"Apple", "Jabłko"},
                new string[]{"A red and round edible fruit.", "Okrągły i czerwony owoc"},
                new JClass (1, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Flashlight", "Latarka"},
                new string[]{"It shines light in front of you for 5 minutes. Can also used be as a weak weapon.", "Oświeca teren przed tobą przez 5 minut. Działa również jako prosta broń biała."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 2),
                    new JFloat (JType.VariableA, 100f)
                }),
                new (10f, 100f, 2f)
            ),
            new(this, new string[]{"Bread", "Chleb"},
                new string[]{"A big loaf of bread.", "Duży bochenek chleba."},
                new JClass (3, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Can of soup", "Zupa w puszce"},
                new string[]{"A can containing a tomato soup with noodles. Can be eaten cold.", "Puszka z zupą pomidorową i makaronem. Można jeść na zimno."},
                new JClass (4, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Canned mackerel", "Makrela w puszce"},
                new string[]{"A smoked mackerel, soaked in oil.", "Wędzona makrela w oleju."},
                new JClass (5, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Chocolate", "Czekolada"},
                new string[]{"A bar of milk chocolate.", "Tabliczka mlecznej czekolady."},
                new JClass (6, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Sausage", "Kiełbasa"},
                new string[]{"A big piece of smoked kielbasa.", "Duży kawałek wędzonej kiełbasy."},
                new JClass (7, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Jam", "Dżem"},
                new string[]{"A jar containing strawberry jam.", "Słoik z dżemem truskawkowym."},
                new JClass (8, JTemplate.BasicItemStackable),
                new (10f, 100f, 0f)
            ),
            new(this, new string[]{"Chips", "Czipsy"},
                new string[]{"A bag of potato chips.", "Paczka z czipsami."},
                new JClass (9, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Cheese", "Ser"},
                new string[]{"A piece of gouda cheese.", "Kawałek sera gouda."},
                new JClass (10, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Glowstick", "Światło chemiczne"},
                new string[]{"A dim light source with infinite lifetime.", "Słabe źródło światła, które się nigdy nie wyczerpie."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 11),
                    new JFloat (JType.VariableA, 0f),
                    new JFloat(JType.Color, Random.Range(0, 10))
                })
            ),
            new(this, new string[]{"Flare", "Flara"},
                new string[]{"It shines a bright light, and warms you. When thrown, it can set foes on fire.", "Flara będzie świecić jasnym światłem, i będzie cię ocieplać. Gdy rzucisz nią w kogoś, ten ktoś zostanie podpalony."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 12),
                    new JFloat (JType.Color, Random.Range(0, 10))
                }),
                new (20f, 0f, 0f)
            ),
            new(this, new string[]{"Lit Flare", "Odpalona Flara"},
                new string[]{"It shines a bright light, and warms you. When thrown, it can set foes on fire.", "Flara będzie świecić jasnym światłem, i będzie cię ocieplać. Gdy rzucisz nią w kogoś, ten ktoś zostanie podpalony."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 13),
                    new JFloat (JType.VariableA, 100f),
                    new JFloat (JType.Color, Random.Range(0, 10))
                }),
                new (20f, 0f, 0f)
            ),
            new(this, new string[]{"Knife", "Nóż"},
                new string[]{"A weak melee weapon, it's pretty quick though.", "Słaba broń biała, jest jednak dość szybka."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 14),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (15f, 0f, 10f)
            ),
            new(this, new string[]{"Crowbar", "Łom"},
                new string[]{"A slow melee weapon. Doesn't do much damage to living things, but does a lot to objects.", "Powolna broń biała. Nie zadaje dużo obrażeń istotom żywym, ale dużo przedmiotom."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 15),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (5f, 0f, 5f)
            ),
            new(this, new string[]{"Fire axe", "Siekera strażacka"},
                new string[]{"A slow, yet powerful melee weapon. Can chop down trees.", "Powolna, lecz silna broń biała. Można nią ścinać drzewa."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 16),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (5f, 0f, 25f)
            ),
            new(this, new string[]{"Bottle of water", "Butelka z wodą"},
                new string[]{"You can drink from it, to regydrate yourself. Also good for when you're on fire.", "Można się z niej napić, w celu nawodnienia się. Przydatna, gdy ktoś cię podpalił."},
                new JClass (17, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Energy drink", "Energetyk"},
                new string[]{"Despite being very unhealthy, it can rehydrate you, and it reduces tiredness.", "Mimo iż są niezdrowe, mogą cię nawodnić, i obniżyć poziom zmęczenia."},
                new JClass (18, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Candy bar", "Batonik"},
                new string[]{"A small chocolate candy bar, with some nuts.", "Mały batonik czekoladowy, z orzechami."},
                new JClass (19, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Can of beans", "Fasolki w puszce"},
                new string[]{"A can of beans in tomato sauce. Can be eaten cold.", "Puszka z fasolkami w sosie pomidorowym. Można jeść na zimno."},
                new JClass (20, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"MRE", "MRE"},
                new string[]{"Military grade, meal ready to eat. It brings your food points to full.", "Wojskowa racja żywnościowa. Napełnia punkty jedzenia do pełna."},
                new JClass (21, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Bandage", "Bandaż"},
                new string[]{"It stops bleeding, and brings back 25hp.", "Zatrzymuje krwotok, i przywraca 25pz."},
                new JClass (22, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Antibiotics", "Antybiotyki"},
                new string[]{"Consuming it will reduce a small portion of infection and radiation sickness.", "Użycie spowoduje obniżenie małej porcji nabytej infekcji oraz choroby popromiennej."},
                new JClass (23, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Vaccine", "Szczepionka"},
                new string[]{"Injecting this will cure infection.", "Szczepionka leczy z infekcji."},
                new JClass (24, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Lugol's solution", "Płyn lugola"},
                new string[]{"Drinking this cures radiation sickness, and makes you invulnerable to it for 60 seconds.", "Wypicie leczy chorobę popromienną, i chroni przed jej ponownym nabyciem przez 60 sekund."},
                new JClass (25, JTemplate.BasicItemStackable),
                new (10f, 50f, 0f)
            ),
            new(this, new string[]{"First aid kit", "Apteczka pierwszej pomocy"},
                new string[]{"Heals you by 50hp.", "Leczy 50pz."},
                new JClass (26, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Machete", "Maczeta"},
                new string[]{"A direct upgrade to knife; deals more damage, and is more durable. Can be used to chop trees.", "Jest to ulepszona wersja noża; zadaje więcej obrażeń, i jest bardziej wytrzymała. Może ścinać drzewa."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 27),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (15f, 0f, 20f)
            ),
            new(this, new string[]{"Baseball bat", "Kij baseballowy"},
                new string[]{"A blunt and quick weapon. Does decent damage to living things, but is very fragile.", "Jest to kij, którym w miarę szybko i w miarę dobrze można posyłać ludzi do piachu. Broń ta się dość łatwo niszczy."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 28),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (15f, 0f, 25f)
            ),
            new(this, new string[]{"Colt", "Colt"},
                new string[]{"A simple pistol, nothing special.", "Prosty pistolet, nic specialnego."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 29),
                    new JFloat (JType.VariableA, 7),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"Pistol magazine", "Magazynek do pistoletu"},
                new string[]{"It contains ammo which can be used to reload pistols. You can reload it using ammo packs.", "To zawiera amunicję, którą można przeładowywać pistolety. Można go naładować paczkami z amunicją."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 30),
                    new JFloat (JType.VariableA, 4 * Random.Range(1, 5)),
                    casualAmmo
                })
            ),
            new(this, new string[]{"Luger", "Luger"},
                new string[]{"A fast firing pistol. Not very accurate, but does decent damage.", "Szybko strzelający pistolet. Niezbyt celny, ale zadaje dużo obrażeń."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 31),
                    new JFloat (JType.VariableA, 8f),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"Revolver", "Rewolwer"},
                new string[]{"A very powerful yet small gun. Deals a lot of damage, is very accurate, but fires very slow.", "Bardzo silna, a zarazem mała broń palna. Zadaje dużo obrażeń, jest bardzo dokładna, jednak strzela powoli."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 32),
                    new JFloat (JType.VariableA, 6f),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"Ammo pack", "Paczka z amunicją"},
                new string[]{"It contains ammo which can be used to reload guns that don't have magazines. It also can be used to reload other magazines.", "To zawiera amunicję, którą można przeładowywać bronie które nie mają magazynków. Można nią także naładowywać inne magazynki."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID,33),
                    new JFloat (JType.VariableA, 5 * Random.Range(1, 10)),
                    casualAmmo
                })
            ),
            new(this, new string[]{"Hunting rifle", "Karabin myśliwski"},
                new string[]{"A slow bolt-action rifle. Has a decent range, good accuracy, but doesn't do much damage.", "Powolny karabin powtarzalny. Posiada dobry dystans, niski rozrzut broni, ale nie zadaje dużo obrażeń."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 34),
                    new JFloat (JType.VariableA, 5f),
                    new JInt (JType.Attachment, 0)
                }),
                new (7f, 25f, 5f)
            ),
            new(this, new string[]{"DB Shotgun", "Dubeltówka"},
                new string[]{"A very powerful gun that fires many bullets per shot, but can only contain 2 rounds at once.", "Potężna broń która wystrzeliwuje wiele kul na jeden strzał, jednak może pomieścić tylko 2 naboje."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 35),
                    new JFloat (JType.VariableA, 2f),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"Thompson", "Thompson"},
                new string[]{"Cult classic SMG. Doesn't do much damage, but fires really fast, and contains a lot of ammo.", "Bardzo znany i doceniany pistolet maszynowy. Nie zadaje sporej ilości obrażeń, ale strzela bardzo szybko, i posiada dużą pojemność na amunicję."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 36),
                    new JFloat (JType.VariableA, 30f),
                    new JInt (JType.Attachment, 0)
                }),
                new (7f, 25f, 5f)
            ),
            new(this, new string[]{"SMG Magazine", "Magazynek do PM"},
                new string[]{"It contains ammo which can be used to reload SMGs. You can reload it using ammo packs.", "To zawiera amunicję, którą można przeładowywać pistolety maszynowe. Można go naładować paczkami z amunicją."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 37),
                    new JFloat (JType.VariableA, 15 * Random.Range(1, 5)),
                    casualAmmo
                })
            ),
            new(this, new string[]{"AK47", "AK47"},
                new string[]{"Popular, reliable, and very deadly. This is one of the best assault rifle you can get.", "Popularny, niezawodny, i niosiący śmierć. To jest jeden z najlepszych karabinów szturmowych"},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 38),
                    new JFloat (JType.VariableA, 30f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"Rifle magazine", "Magazynek do karabinów"},
                new string[]{"It can be used to reload rifles. You can reload it using ammo packs.", "To zawiera amunicję, którą można przeładowywać karabiny. Można go naładować paczkami z amunicją."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 39),
                    new JFloat (JType.VariableA, 15 * Random.Range(1, 5)),
                    casualAmmo
                })
            ),
            new(this, new string[]{"Shotgun", "Strzelba"},
                new string[]{"A very powerful gun that fires many bullets per shot, but doesn't do as much damage as the double-barrelled variant.", "Potężna broń która wystrzeliwuje wiele kul na jeden strzał, jednak nie zadaje tylu obrażeń co dubeltówka."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 40),
                    new JFloat (JType.VariableA, 5f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"MP5", "MP5"},
                new string[]{"A very fast SMG.", "Bardzo szybki pistolet maszynowy."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 41),
                    new JFloat (JType.VariableA, 40f),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"M4", "M4"},
                new string[]{"This carbine is similar to AK-47. It is slower, but more accurate and deals more damage.", "Karabinek ten jest podobny do AK-47. Jest wolniejszy, ale zadaje więcej obrażeń, i jest bardziej celny."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 42),
                    new JFloat (JType.VariableA, 30f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"Heat pack", "Ogrzewacz"},
                new string[]{"Upon using it, it'll instantly warm you, and you will gain 60 seconds of immunity over coldness.", "Po użyciu tego przedmiotu, natychmiastowo cię ociepli, oraz zdobędziesz odporność na zimno na 60 sekund."},
                new JClass (43, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Adrenaline", "Adrenalina"},
                new string[]{"This substance will make your stamina infinite, for 15 seconds. It will damage you a bit though!", "Ta substancja spowoduje że twoja energia będzie nieskończona, przez 15 sekund. Stracisz jednak trochę zdrowia!"},
                new JClass (44, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Fanny pack", "Torba na pas"},
                new string[]{"Upon wearing it, you'll have +1 inventory slot.", "Po jej założeniu, dostaniesz +1 miejsce w inwentarzu."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 45),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 1)
                })
            ),
            new(this, new string[]{"Backpack", "Plecak"},
                new string[]{"Upon wearing it, you'll have +2 inventory slots.", "Po jego założeniu, dostaniesz +2 miejsc w inwentarzu."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 46),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 1)
                })
            ),
            new(this, new string[]{"Military backpack", "Plecak wojskowy"},
                new string[]{"Upon wearing it, you'll have +4 inventory slots.", "Po jego założeniu, dostaniesz +4 miejsc w inwentarzu."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 47),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 1)
                })
            ),
            new(this, new string[]{"Bulletproof vest", "Kamizelka kuloodporna"},
                new string[]{"Upon wearing it, you'll have 150 max health.", "Po jej założeniu, będziesz miał maksymalnie 150pz."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 48),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 2)
                })
            ),
            new(this, new string[]{"Military vest", "Kamizelka wojskowa"},
                new string[]{"Upon wearing it, you'll have 250 max health, +4 inventory slots, but your walking speed decrease by 3m/s.", "Po jej założeniu, będziesz miał maksymalnie 250pz, +4 miejsc w inwentarzu, jednak twoja prędkość spadnie o 3m/s."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 49),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 2)
                }),
                new (5f, 0f, 0f)
            ),
            new(this, new string[]{"Sleeping bag", "Śpiwór"},
                new string[]{"Upon escaping, all of the tiredness will be removed.", "Po opuszczeniu mapy, całe twoje zmęczenie zniknie."},
                new JClass (50, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Sport shoes", "Buty sportowe"},
                new string[]{"Upon wearing it, your speed will increase by 3m/s.", "Po ich ubraniu, twoja prędkość wzrośnie o 3m/s."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 51),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 3)
                })
            ),
            new(this, new string[]{"Money", "Pieniądze"},
                new string[]{"It can be used to trade, when you don't have the item the trader wants.", "Możesz tym handlować, jeśli nie posiadasz przedmiotu, którego chce handlujący."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 52),
                    new JFloat (JType.VariableA, Random.Range(1, 10) * 5),
                    new JString (JType.Cacheable, "Money")
                })
            ),
            new(this, new string[]{"Night vision goggles", "Noktowizor"},
                new string[]{"Upon using these goggles, you'll be able to see anything in the dark. They last for 180 seconds, you can turn them on/off them at any time, and you don't need to constantly hold them.", "Po założeniu tych gogli, będziesz widzieć w ciemności. Starczają na 180 sekund, możesz je włączyć/wyłączyć w każdej chwili, i nie musisz ich cały czas trzymać."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 53),
                    new JFloat (JType.VariableA, 100f),
                    new JInt (JType.ClothingCategory, 4),
                    new JInt (JType.NightVision, 1)
                }),
                new (15f, 50f, 0f)
            ),
            new(this, new string[]{"Grappling hook", "Hak mocujący"},
                new string[]{"If you throw it while standing, you may create a rope bridge from the point where you're standing, and where the hook lands. Might break when throwing.", "Gdy tym rzucisz stojąc w miejscu, stworzysz most liniowy, znajdujący pomiędzy sobą, a miejscem gdzie uderzy hak. Może się zniszczyć."},
                new JClass (54, JTemplate.BasicItemStackable),
                new (20f, 25f, 0f)
            ),
            new(this, new string[]{"Sten", "Sten"},
                new string[]{"A mass produced smg. Similar to thompson, but does less damage.", "Masowo produkowany pistolet maszynowy. Podobny do thompsona, tylko że zadaje mniej obrażeń."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 55),
                    new JFloat (JType.VariableA, 32f),
                    new JInt (JType.Attachment, 0)
                }),
                new (7f, 25f, 5f)
            ),
            new(this, new string[]{"Garand", "Garand"},
                new string[]{"Classic american rifle. It is very powerful, very accurate, but isn't that fast. It also can be reloaded, only if it's empty.", "Klasyczny amerykański karabin. Zadaje dużo obrażeń, jest dokładny, jednak nie jest taki szybki. Nie można go przeładowywać, jeżeli nie jest pusty."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 56),
                    new JFloat (JType.VariableA, 8f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"Famas", "Famas"},
                new string[]{"A french assault rifle. It's fast, deadly, but not very accurate.", "Francuski karabin szturmowy. Jest szybki, śmiercionośny, jednak niezbyt dokładny."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 57),
                    new JFloat (JType.VariableA, 25f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"Uzi", "Uzi"},
                new string[]{"A smg, that you can hold in one hand.", "Pistolet maszynowy, który można trzymać w jednej ręce."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 58),
                    new JFloat (JType.VariableA, 25f),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"G3", "G3"},
                new string[]{"A german assault rifle. Similar to M4, but a bit more accurate.", "Niemiecki karabin szturmowy. Podobny do karabinku M4, tylko że jest bardziej dokładny."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 59),
                    new JFloat (JType.VariableA, 20f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"SCAR", "SCAR"},
                new string[]{"A fast firing assault rifle. It does a lot of damage, but the recoil is high.", "Szybko strzelny karabin szturmowy. Zadaje dużo obrażeń, jednak ma spory odrzut."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 60),
                    new JFloat (JType.VariableA, 28f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"SPAS", "SPAS"},
                new string[]{"A semi-automatic shotgun. It's very lethal, if you can handle the recoil.", "Pół automatyczna strzelba. Bardzo śmiercionośna, jednak z ogromnym odrzutem."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 61),
                    new JFloat (JType.VariableA, 8f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"SAW", "SAW"},
                new string[]{"Very fast firing machine gun. Can mown down enemies pretty easily", "Bardzo szybki karabin maszynowy. Z łatwością może przemieniać przeciwników w ser szwajcarski."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 62),
                    new JFloat (JType.VariableA, 100f),
                    new JInt (JType.Attachment, 0)
                }),
                new (2f, 25f, 0f)
            ),
            new(this, new string[]{"Ammo chain", "Taśma z nabojami"},
                new string[]{"A bunch of bullets, on a chain. This can be used to reload machine guns.", "Łańcuch z kulami. Można tego użyć do przeładowywania karabinów maszynowych."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 63),
                    new JFloat (JType.VariableA, 250 * Random.Range(1, 4)),
                    casualAmmo
                })
            ),
            new(this, new string[]{"Minigun", "Minigun"},
                new string[]{"A very powerful gun. Fires really fast, does a lot of damage, and can contain a lot of ammo. Although, mobility is decreased when firing.", "Bardzo potężna broń. Strzela bardzo szybko, zadaje dużo obrażeń, jednak mobilność podczas strzelania jest dość niska."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 64),
                    new JFloat (JType.VariableA, 500f),
                    new JInt (JType.Attachment, 0)
                }),
                new (2f, 25f, 0f)
            ),
            new(this, new string[]{"Mosin Nagant", "Karabin Mosina"},
                new string[]{"A bolt-action rifle of soviet construction. It is a very accurate weapon, and is perfect for taking out targets from long distances.", "Karabin powtarzalny sowjeckiej konstrukcji. Jest bardzo dokładny, i sprawdza się idealnie na dalekie dystanse."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 65),
                    new JFloat (JType.VariableA, 5f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"Grenade", "Granat"},
                new string[]{"A throwable explosive. After unpining it, you'll have 5 seconds to throw or drop it before it explodes.", "Materiał wybuchowy do rzucania. Po odbezpieczeniu, masz 5 sekund żeby rzucić, albo upuścić go, zanim wybuchnie."},
                new JClass (66, JTemplate.BasicItemStackable),
                new (15f, 0f, 0f)
            ),
            new(this, new string[]{"Panzerfaust", "Panzerfaust"},
                new string[]{"Single-use grenade launcher.", "Jednorazowy granatnik."},
                new JClass (67, JTemplate.BasicItemStackable),
                new (7f, 25f, 0f)
            ),
            new(this, new string[]{"Chainsaw", "Piła łańcuchowa"},
                new string[]{"A very handy machine, that can mown down any living and/or not living thing. When held, fuel can last for only 60 seconds.", "Bardzo przydatna maszyna, która potrafi rozwalić wszystko co żywe lub nie. Gdy trzymana, paliwo starcza na 60 sekund."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 68),
                    new JFloat (JType.VariableA, 100f)
                }),
                new (7f, 25f, 5f)
            ),
            new(this, new string[]{"Bow", "Łuk"},
                new string[]{"You can shoot arrows with it.", "Można z niego strzelać strzałami."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 69),
                    new JFloat (JType.VariableA, 10f)
                }),
                new (15f, 75f, 0f)
            ),
            new(this, new string[]{"Baguette", "Bagieta"},
                new string[]{"A very long bread.", "Bardzo długi kawałek chleba."},
                new JClass (70, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Pickles", "Ogórki kiszone"},
                new string[]{"Pickled cucumbers. Sour, salty, some even might find them tasty.", "Kwaśne, słone, niektórzy mogliby nawet powiedzieć, że są smaczne."},
                new JClass (71, JTemplate.BasicItemStackable),
                new (10f, 100f, 0f)
            ),
            new(this, new string[]{"Meat", "Mięso"},
                new string[]{"A large piece of dead animal. It is delicious and nutritious!", "Spory kawałek martwego zwierzęcia. Jest pyszne i pożywne!"},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 72),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Pretzel", "Precel"},
                new string[]{"Crusty outside, soft inside.", "Kruche z zewnątrz, miękkie w środku."},
                new JClass (73, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Cheeseburger", "Burger"},
                new string[]{"The cornerstone of any nutritious breakfast.", "Podstawa każdego pożywnego śniadania."},
                new JClass (74, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Waffle", "Gofr"},
                new string[]{"A soft and sweet waffer.", "Słodki i miękki wafel."},
                new JClass (75, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Donut", "Donut"},
                new string[]{"A bagel, shaped like a donut.", "Pączek, w kształcie donuta."},
                new JClass (76, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Pâté", "Pasztet"},
                new string[]{"A forcemeat without pastery.", "Jednolita masa z gotowanego mięsa."},
                new JClass (77, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Crackers", "Krakersy"},
                new string[]{"A bunch of small, salty crackers.", "Paczka z małymi, słonymi krakersami."},
                new JClass (78, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Cola", "Kola"},
                new string[]{"A can of refreshing and energizing soft drink.", "Puszka nawadniającego i energetyzującego napoju gazowanego."},
                new JClass (79, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Beer", "Piwo"},
                new string[]{"A bottle of fermented wheat water. Drinking will result in 15 seconds of drunkeness", "Butleka z fermentowanym sokiem z przenicy. Wypicie spowoduje 15 sekund upicia."},
                new JClass (80, JTemplate.BasicItemStackable),
                new (10f, 100f, 0f)
            ),
            new(this, new string[]{"Vodka", "Wódka"},
                new string[]{"A bottle of fermented potato water. Drinking will result in 45 seconds of drunkeness", "Butelka z fermentowaną wodą z ziemniaków. Wypicie spowoduje 45 sekund upicia."},
                new JClass (81, JTemplate.BasicItemStackable),
                new (10f, 100f, 0f)
            ),
            new(this, new string[]{"Potato", "Ziemniak"},
                new string[]{"A baked potato. Should be safe to eat.", "Upieczony ziemniak. Chyba bezpieczny do spożycia."},
                new JClass (82, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Milk", "Mleko"},
                new string[]{"A carton of cow milk. Will rehydrate you, and will make your bones grow strong.", "Karton krowiego mleka. Wypicie nawodni, i wzmocni twoje kości."},
                new JClass (83, JTemplate.BasicItemStackable),
                new (10f, 25f, 0f)
            ),
            new(this, new string[]{"Biscuits", "Herbatniki"},
                new string[]{"A bunch of small and flat cookies.", "Paczka z małymi, płaskimi ciasteczkami."},
                new JClass (84, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Umbrella", "Parasol"},
                new string[]{"This tool can protect you from rain, and falling down.", "Ten przedmiot chroni przed deszczem, i spadaniem z wysokości."},
                new JClass (85, JTemplate.BasicItemStackable),
                new (15f, 0f, 0f)
            ),
            new(this, new string[]{"Hazmat suit", "Kombinezon materiałów niebezpiecznych"},
                new string[]{"Upon wearing it, you'll be protected from any radiation and fire, though you'll move very slow. Prolonged exposure might damage this suit.", "Po założeniu, nie będziesz otrzymywał obrażeń od promieniowania i ognia, jednak będziesz się powoli poruszał. Nadmierne promieniowanie może jednak uszkodzić ten kombinezon."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 86),
                    new JFloat (JType.VariableA, 100f),
                    new JInt (JType.ClothingCategory, 5),
                    new JEntry (JType.Repairable)
                })
            ),
            new(this, new string[]{"Lifebuoy", "Koło ratunkowe"},
                new string[]{"When held, it allows you to move normally in water, and you won't get wet from it.", "Gdy go trzymasz, pozwala ci normalnie chodzić w wodzie, bez moknięcia."},
                new JClass (87, JTemplate.BasicItemStackable),
                new (15f, 0f, 0f)
            ),
            new(this, new string[]{"Ducktape", "Taśma klejąca"},
                new string[]{"It can be used on some items, in order to repair them by 50%. Has only one use.", "Może zostać użyta na niektórych przedmiotach, aby je naprawić o 50%. Ma tylko jedno użycie."},
                new JClass (88, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Blowtorch", "Palnik"},
                new string[]{"It can be used on some items, in order to repair them to 100%. Has three uses.", "Może zostać użyty na niektórych przedmiotach, aby je naprawić do 100%. Można go użyć trzy razy."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 89),
                    new JFloat (JType.VariableA, 100f)
                }),
                new (10f, 75f, 0f)
            ),
            new(this, new string[]{"Wrench", "Klucz"},
                new string[]{"It can be used on some items, in order to repair them by 5%. Has infinite uses, but reparing takes long time.", "Może zostać użyty na niektórych przedmiotach, aby je naprawić o 5%. Można go używać bez końca, jednak czas naprawy jest dość długi."},
                new JClass (90, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Camera", "Aparat"},
                new string[]{"This device has a really bright flash. When taking a photo, anyone that is close to you, will get stunned for 2 seconds.", "To urządzenie posiada bardzo jasną lampę błyskową. Po strzeleniu zdjęcia, każdy kto jest blisko ciebie, zostanie oślepiony na 2 sekundy."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 91),
                    new JFloat (JType.VariableA, 10f)
                })
            ),
            new(this, new string[]{"Binoculars", "Lornetki"},
                new string[]{"It allows you to zoom really far.", "Pozwalają ci popatrzeć z bliska na dalsze tereny."},
                new JClass (92, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Cowbell", "Dzwon"},
                new string[]{"An old rusty cowbell. You can ring it, to alert anyone in the radius of 100m.", "Stary zardzewiały dzwonek. Możesz nim zadzwonić; każdy w promieniu 100m to usłyszy."},
                new JClass (93, JTemplate.BasicItemStackable),
                new (15f, 0f, 0f)
            ),
            new(this, new string[]{"Scarf", "Szalik"},
                new string[]{"Upon using it, you'll be protected from coldness. Will cause overheating when not cold.", "Po użyciu tego, będziesz chroniony od zimna. Możesz się przegrzać jeśli nie będzie ci zimno"},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 94),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 94)
                })
            ),
            new(this, new string[]{"Riot shield", "Tarcza policyjna"},
                new string[]{"A tactical shield, that can block bullets, melee weapons, mutant attacks, and explosions. It may break after few blocks.", "Taktyczna tarcza, która blokuje kule, bronie białe, ataki mutantów, i eksplozje. Może się popsuć po długim użytku."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 95),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (5f, 0f, 0f)
            ),
            new(this, new string[]{"Cardboard box", "Karton"},
                new string[]{"Upon using it, no one will detect you, although you can only crouch in it. It is small, so after some usage it'll eventually break.", "Po założeniu tego kartonu na siebie, nikt cię nie zobaczy, jednak będziesz musiał kucać. To pudło jest małe, i po długim czasie użytkowania może się popsuć."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 96),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                })
            ),
            new(this, new string[]{"Lockpick", "Wytrych"},
                new string[]{"Upon using it on a locked door or a barrel, it may either: open it, break itself, or nothing will happen.", "Po użyciu tego na zamkniętych drzwiach lub beczkach: zostaną otwarte, wytrych się złamie, albo nic."},
                new JClass (97, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Towel", "Ręcznik"},
                new string[]{"You can use it to wipe yourself, and reduce wetness, bleeding, and radioactivity.", "Możesz nim się wytrzeć, by pozbyć się trochę mokra, krwotoku, i radioaktywności."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 98),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Map", "Mapa"},
                new string[]{"Upon using it, it'll mark some interesting locations on your map.", "Po użyciu tego, na twojej mapie zostaną zaznaczone interesujące lokacje."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 99),
                    new JFloat (JType.VariableA, Random.Range(1, 4))
                })
            ),
            new(this, new string[]{"Suppressor", "Tłumik"},
                new string[]{"A gun attachment. When attached, it'll make your gun fire very quiet, and won't alert anyone.", "Dodatek do broni. Gdy założony, twoja broń będzie strzelać ciszej, i nikt jej nie usłyszy."},
                new JClass (100, JTemplate.BasicItem)
            ),
            new(this, new string[]{"Grip", "Uchwyt"},
                new string[]{"A gun attachment. When attached, the gun recoil will be significantly reduced.", "Dodatek do broni. Gdy założony, rozrzut broni zostanie znacznie zmniejszony."},
                new JClass (101, JTemplate.BasicItem)
            ),
            new(this, new string[]{"Grenade launcher attachment", "Montowalny granatnik"},
                new string[]{"A gun attachment. When attached, you'll be able to fire grenades for 50% of maximum gun's ammo.", "Dodatek do broni. Gdy założony, będziesz mógł wystrzeliwywać granaty za 50% maksymalnej amunicji."},
                new JClass (102, JTemplate.BasicItem)
            ),
            new(this, new string[]{"Sniper scope", "Luneta snajperska"},
                new string[]{"A gun attachment. When attached to some kind of rifle, it reduces gun spread and firing speed, but increases damage and aiming zoom.", "Dodatek do broni. Gdy założona na jakiegoś rodzaju karabin, obniży rozrzut i prędkość strzelania, ale podwyższy obrażenia i zoom."},
                new JClass (103, JTemplate.BasicItem),
                new (10f, 100f, 0f)
            ),
            new(this, new string[]{"Holo sight", "Holograficzny celownik"},
                new string[]{"A gun attachment. When attached, it'll increase your aiming zoom.", "Dodatek do broni. Po założeniu, będziesz mógł bliżej celować."},
                new JClass (104, JTemplate.BasicItem),
                new (10f, 100f, 0f)
            ),
            new(this, new string[]{"Bipod", "Dwójnóg"},
                new string[]{"A gun attachment. When attached, it'll completely remove gun spread and recoil when crouching.", "Dodatek do broni. Po założeniu, pozbędzie się rozrzutu i odrzutu broni, podczas kucania."},
                new JClass (105, JTemplate.BasicItem)
            ),
            new(this, new string[]{"Med Kit", "Apteczka"},
                new string[]{"You can use it to add +75hp. It also has a chance to remove specific wounds, like bleeding or broken bones.", "Możesz tego użyć, by otrzymać +75pz. Użycie tego, także ma szanse pozbycia się specyficznych ran, takich jak krwotok, albo połamane kości."},
                new JClass (106, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Splint", "Szyna"},
                new string[]{"Upon using it, it'll fix your bones if they're broken.", "Po użyciu tego przedmiotu, twoje kości zostaną uleczone, jeśli miałeś je połamane."},
                new JClass (107, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Plunger", "Przepychacz"},
                new string[]{"It's a tool, that can be used as a weapon. It isn't very effective, but you can throw it at a foe, to blind them for 5 seconds.", "Jest to narzędzie, którego możesz użyć jako broni. Nie jest to efektowne, ale możesz rzucić tym w przeciwnika, by go oślepić na 5 sekund."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 108),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                })
            ),
            new(this, new string[]{"Flamethrower", "Miotacz ognia"},
                new string[]{"An industrial flamethrower.", "Przemysłowy miotacz ognia."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 109),
                    new JFloat (JType.VariableA, 100f)
                }),
                new (5f, 50f, 5f)
            ),
            new(this, new string[]{"Frag grenade", "Granat odłamkowy"},
                new string[]{"A throwable explosive. It has smaller explosion radius that normal grenade, albeit, upon exploding it can shoot out multiple deadly fragments.", "Materiał wybuchowy do rzucania. Wybuch ma mniejszy zasięg niż zwykły granat, ale, po detonacji wystrzeliwuje wiele śmiercionośnych odłamków."},
                new JClass (110, JTemplate.BasicItemStackable),
                new (15f, 0f, 0f)
            ),
            new(this, new string[]{"Grenade launcher", "Granatnik"},
                new string[]{"A weapon, that can fire grenades, which explode on impact.", "Broń która wystrzeliwuje granaty, wybuchające po uderzeniu."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 111),
                    new JFloat (JType.VariableA, 6f)
                }),
                new (7f, 50f, 5f)
            ),
            new(this, new string[]{"Crossbow", "Kusza"},
                new string[]{"It's like a bow, except you don't need to load it before shooting, but rather after.", "Jest to podobne do łuku, z takim wyjątkiem, że ładuje się po wystrzale, a nie przed."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 112),
                    new JFloat (JType.VariableA, 10f)
                }),
                new (15f, 75f, 0f)
            ),
            new(this, new string[]{"Musket", "Muszkiet"},
                new string[]{"An old trapdoor musket. Can hold only one bullet at a time, and isn't very accurate.", "Stary muszkiet. Posiada tylko jeden pocisk, i nie jest zbytnio celny."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 113),
                    new JFloat (JType.VariableA, 1f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 2f)
            ),
            new(this, new string[]{"Taser", "Paralizator"},
                new string[]{"A slightly modified taser. It's modification, in fact, makes it is very deadly!", "Zmodyfikowany paralizator. Jego modyfikacja, robi go bardzo niebezpiecznym!"},
                new JClass (114, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Shovel", "Łopata"},
                new string[]{"This tool, besides being a decent yet fragile weapon, has one special use. If you keep hitting dirt with it, you have 10% chance of getting a random item.", "To narzędzie, poza byciem bronią, posiada jedną ciekawą cechę. Jeśli będziesz nią bił o ziemie, będziesz miał 10% szans na zdobycie losowego przedmiotu."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 115),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (7f, 25f, 10f)
            ),
            new(this, new string[]{"Herring", "Śledź"},
                new string[]{"A small fish. Tasty, but not very filling.", "Mała rybka. Smaczna, ale nie napełni do syta."},
                new JClass (116, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Salmon", "Łosoś"},
                new string[]{"A decent catch. Tasty, and filling.", "Przyzwoity połów. Smaczna i napełniająca."},
                new JClass (117, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Carp", "Krap"},
                new string[]{"A very big fish. Tastes like wet newspaper covered in mud, but at least it is very filling.", "Bardzo duża ryba. Smakuje jak mokra gazeta zanurzona w mule, ale za to napełnia do syta."},
                new JClass (118, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Coconut", "Kokos"},
                new string[]{"A coco fruit, that not only is very filling, but also can rehydrate you!", "Owoc kokosowy, który nie dość że napełnia do syta, to również nawadnia!"},
                new JClass (119, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Banana", "Banan"},
                new string[]{"Potassium.", "Potas."},
                new JClass (120, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Sandwich", "Kanapka"},
                new string[]{"Bologna sandwich, a perfect fuel.", "Kanapka, najbardziej przydatny przedmiot w życiu."},
                new JClass (121, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Coffee", "Kawa"},
                new string[]{"A cup of coffee. Can rehydrate you, and instantly recharge stamina.", "Filiżanka kawy. Nawadnia, i przywraca do pełnia wytrzymałość."},
                new JClass (122, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Popsicle", "Lód na patyku"},
                new string[]{"A sweet and sour treat. Be careful tho, it can give you 25% of coldness.", "Słodki i kwaśny deser. Ostrożnie, spożycie spowoduje nabycie 25% zimna."},
                new JClass (123, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Puffer", "Inhalator"},
                new string[]{"Can be used to regain stamina instantly.", "Natychmiastowo regeneruje wytrzymałość."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 124),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Scuba tank", "Butla do nurkowania"},
                new string[]{"Upon equipping it, you'll be breathing oxygen from this tank. It can provide a maximum of 5 minutes of oxygen.", "Po jej założeniu, będziesz oddychał tlenem z tej butli. Może zapewnić maksimum 5 minut tlenu."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 125),
                    new JFloat (JType.VariableA, 100f),
                    new JInt (JType.ClothingCategory, 4)
                }),
                new (2f, 25f, 0f)
            ),
            new(this, new string[]{"Flippers", "Płetwy"},
                new string[]{"Upon wearing those, your swimming speed will increase by 7m/s.", "Po ich założeniu, twoja prędkość pływania wzrośnie o 7m/s."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 126),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 3)
                })
            ),
            new(this, new string[]{"Crank flashlight", "Latarka na korbkę"},
                new string[]{"It shines light in front of you. Battery lasts for a short time, but it can be recharged by cranking it.", "Oświeca teren przed tobą. Bateria starcza na krótką chwilę, ale można ją ręcznie naładować."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 127),
                    new JFloat (JType.VariableA, 0f)
                }),
                new (10f, 50f, 0f)
            ),
            new(this, new string[]{"Fire extinguisher", "Gaśnica"},
                new string[]{"Aside from extinguishing fires, it can also cause a temporary propulsion opposite of your looking direction.", "Poza gaszeniem pożarów, powoduje chwilowy napęd w przeciwnym kierunku twojego wzroku."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 128),
                    new JFloat (JType.VariableA, 100f)
                }),
                new (2f, 75f, 0f)
            ),
            new(this, new string[]{"Fishing rod", "Wędka"},
                new string[]{"This tool allows you to catch fish and other stuff from water.", "To narzędzie pozwala ci łowić ryby i śmieci z wody."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 129),
                    new JFloat (JType.VariableA, 0f)
                }),
                new (10f, 50f, 0f)
            ),
            new(this, new string[]{"Scanner", "Skaner"},
                new string[]{"This advanced electronic device allows you to scan the entire map for items, and other stuff.", "To zaawansowane elektroniczne urządzenie pozwala skanować mapę w poszukiwaniu przedmiotów i innych rzeczy."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 130),
                    new JFloat (JType.VariableA, 100f),
                    new JInt (JType.ScanOption, 0)
                }),
                new (10f, 100f, 0f)
            ),
            new(this, new string[]{"Flashbang", "Granat hukowy"},
                new string[]{"A throwable explosive. It doesn't deal any damage, but it'll stun anyone nearby. It's stun effect decreases with distance.", "Materiał wybuchowy do rzucania. Nie zadaje żadnych obrażeń, ale potrafi ogłuszyć każdego w pobliżu. Efekt ogłuszenia jest silniejszy im bliżej wybuchu."},
                new JClass (131, JTemplate.BasicItemStackable),
                new (15f, 0f, 0f)
            ),
            new(this, new string[]{"Katana", "Katana"},
                new string[]{"A swift and deadly japanese sword. It is very fragile though, and it breaks easily!", "Szybka i śmiertelna broń japońska. Bądź ostrożny, łatwo można ją uszkodzić!"},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 132),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (10f, 100f, 25f)
            ),
            new(this, new string[]{"Molotov cocktail", "Koktajl mołotowa"},
                new string[]{"A throwable bottle with flammable substance in it. Upon throwing and hitting something, everyone within 6 meter radius will be set on fire.", "Butelka z substancją łatwopalną. Gdy nią rzucisz i coś uderzysz, wszyscy w promieniu 6 metrów zostaną podpaleni."},
                new JClass (133, JTemplate.BasicItemStackable),
                new (20f, 0f, 0f)
            ),
            new(this, new string[]{"Spear", "Włócznia"},
                new string[]{"This melee weapon has long attack distance. When thrown, it flies far, and deals more damage.", "Ta broń biała może atakować na dłuższy dystans. Rzucona, zadaje więcej obrażeń, oraz leci na bardzo długi dystans."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 134),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (20f, 25f, 20f)
            ),
            new(this, new string[]{"G18", "G18"},
                new string[]{"This pistol is pretty weak, but it's fully automatic!", "Ten pistolet jest dość słaby, ale jest automatyczny!"},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 135),
                    new JFloat (JType.VariableA, 17f),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"Frying pan", "Patelnia"},
                new string[]{"A kitchen utility made from cast iron. Can be used as a weapon, but it's pretty loud. Some hits may stun foes.", "Narzędzie kuchenne stworzone z żeliwa. Może być użyte za broń, ale jest dość głośna. Niektóre uderzenia mogą ogłuszyć nieprzyjaciół."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 136),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (10f, 15f, 10f)
            ),
            new(this, new string[]{"M1 Carbine", "Karabinek M1"},
                new string[]{"This carbine is an upgrade of the Garand rifle. It has more ammo, fires faster, but deals less damage.", "Ten karabinek jest następcą karabinu Garand. Ma więcej amunicji, strzela szybciej, ale zadaje mniej obrażeń."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 137),
                    new JFloat (JType.VariableA, 15f),
                    new JInt (JType.Attachment, 0)
                }),
                new (7f, 25f, 5f)
            ),
            new(this, new string[]{"Sledgehammer", "Młot wyburzeniowy"},
                new string[]{"This tool is very powerful. It deals a lot of damage, can push foes far, but it's very slow and drains a lot of stamina.", "Jest to potężne narzędzie. Zadaje bardzo dużo obrażeń przedmiotom, potrafi wyrzucić przeciwników w powietrze, jednak jest bardzo powolna i wymaga dużo wytrzymałości."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 138),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (2f, 0f, 0f)
            ),
            new(this, new string[]{"Bazooka", "Bazooka"},
                new string[]{"This is a powerful rocket launcher.", "Jest to potężna wyrzutnia rakiet."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 139),
                    new JFloat (JType.VariableA, 6f)
                }),
                new (5f, 25f, 0f)
            ),
            new(this, new string[]{"Wood", "Drewno"},
                new string[]{"A piece of wood. Used mainly for crafting.", "Kawałek drewna. Głównie używany w tworzeniu."},
                new JClass (140, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Thread", "Nić"},
                new string[]{"A bunch of strings tied together. Used mainly for crafting.", "Kawał nici zawiniętych razem. Głównie używane w tworzeniu."},
                new JClass (141, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Stone", "Kamień"},
                new string[]{"A small stone. Used mainly for crafting, but can also be used as a throwable weapon.", "Mały kamień. Głównie używany w tworzeniu, ale można również nim rzucać w nieprzyjaciół."},
                new JClass (142, JTemplate.BasicItemStackable),
                new (20f, 0f, 5f)
            ),
            new(this, new string[]{"Paper", "Papier"},
                new string[]{"A piece of paper. Used mainly for crafting.", "Kawałek kartki. Głównie używany w tworzeniu."},
                new JClass (143, JTemplate.BasicItemStackable),
                new (5f, 0f, 0f)
            ),
            new(this, new string[]{"Cloth", "Tkanina"},
                new string[]{"A piece of cloth. Used mainly for crafting.", "Kawałek tkaniny. Głównie używany w tworzeniu."},
                new JClass (144, JTemplate.BasicItemStackable),
                new (5f, 0f, 0f)
            ),
            new(this, new string[]{"Metal", "Metal"},
                new string[]{"A bunch of scraps of various metals. Used mainly for crafting.", "Kawał zezłomowanego metalu. Głównie używany w tworzeniu."},
                new JClass (145, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Clay", "Ziemia"},
                new string[]{"A pile of dirt, with some traces of clay, gravel, and whatnot. Used mainly for crafting.", "Sterta ziemi, z domieszką gliny, żwiru, i czegoś tam jeszcze. Głównie używana w tworzeniu."},
                new JClass (146, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Coal", "Węgiel"},
                new string[]{"A piece of charcoal. Used mainly for crafting.", "Kawałek węgla drzewnego. Głównie używany w tworzeniu."},
                new JClass (147, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Campfire", "Ognisko"},
                new string[]{"Upon placing, you'll have a campfire. Standing near it will warm you, and will allow you to craft items that require heat.", "Po zbudowaniu, będziesz miał ognisko. Stojąc blisko ogniska, ogrzeje cię, i będziesz mógł tworzyć przedmioty wymagające ciepła."},
                new JClass (148, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Wooden wall", "Drewniana ściana"},
                new string[]{"You can place it either as: a wall, a floor, or stairs.", "Można postawić jako: ściana, podłoga, lub schody."},
                new JClass (149, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Barbed wire", "Drut kolczasty"},
                new string[]{"Upon placing, anything that enters it, will get cut and stuck. Yes, even the one who placed it.", "Po zbudowaniu, wszystko co wejdzie, się potnie i utknie. Tak, nawet osoba która to położyła."},
                new JClass (150, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Metal wall", "Metalowa ściana"},
                new string[]{"You can place it either as: a wall, a floor, or stairs.", "Można postawić jako: ściana, podłoga, lub schody."},
                new JClass (151, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Metal axe", "Metalowa siekiera"},
                new string[]{"A slow, yet powerful melee weapon. Can chop down trees.", "Powolna, lecz silna broń biała. Można nią ścinać drzewa."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 152),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                })
            ),
            new(this, new string[]{"Celt", "Celt"},
                new string[]{"A makeshift stone axe. It's slow and blunt, but can chop down trees pretty well.", "Domowej roboty kamienna siekiera. Jest powolna i tępa, ale dobrze ścina drzewa."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 153),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (10f, 25f, 10f)
            ),
            new(this, new string[]{"Fokos", "Ciupaga"},
                new string[]{"An axe, with long handle and small head. It's bad at chopping stuff, but you can parry attacks with it pretty well.", "Siekierka z długą rękojeścią i małą główką. Nie nadaje się do siekania, ale można nią dobrze parować ataki."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 154),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (15f, 25f, 10f)
            ),
            new(this, new string[]{"Sword", "Miecz"},
                new string[]{"A great two-handed sword. It's slow, but does great damage.", "Potężny miecz oburęczny. Jest powolny, ale zadaje potężne obrażenia."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 155),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (5f, 0f, 100f)
            ),
            new(this, new string[]{"Pickaxe", "Kilof"},
                new string[]{"The pointy head allows to break solid construction down quite well. It can also be used to climb up walls.", "Szpiczasta główka pozwala na niszczenie solidnych konstrukcji. Można również tego użyć do wspinaczki po ścianach."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 156),
                    new JFloat (JType.VariableA, 100f),
                    new JEntry (JType.Repairable)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"Flintlock", "Pistolet skałkowy"},
                new string[]{"A single shot blackpowder pistol. It's not very accurate, and takes long to reload, but deals a lot of damage.", "Jednostrzałowy pistolet czarnoprochowy. Jest niedokładny, i długo się go przeładowuje, ale zadaje duże obrażenia."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 157),
                    new JFloat (JType.VariableA, 100f),
                    new JInt (JType.Attachment, 0)
                }),
                new (10f, 25f, 5f)
            ),
            new(this, new string[]{"Blackpowder", "Czarny proch"},
                new string[]{"A wooden box with blackpowder cartridges. Can be used to reload blackpowder guns.", "Drewniane pudełko z czarnoprochowymi nabojami. Można je użyć do przeładowania czarnoprochowców."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 158),
                    new JFloat (JType.VariableA, Random.Range(1, 4) * 5),
                    casualAmmo
                })
            ),
            new(this, new string[]{"Baker rifle", "Karabin Bakera"},
                new string[]{"A single shot blackpowder rifle. It takes very long to reload, but the rifled barrel allows for very accurate - and deadly - shots.", "Jednostrzałowy karabin czarnoprochowy. Bardzo długo się go przeładowuje, ale gwintowana lufa gwarantuje bardzo dokładne - i zabójcze - strzały."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 159),
                    new JFloat (JType.VariableA, 1),
                    new JInt (JType.Attachment, 0)
                }),
                new (7f, 25f, 5f)
            ),
            new(this, new string[]{"Nock gun", "Karabin Nocka"},
                new string[]{"Blackpowder musket, that can fire 7 shots at once! The price for obliterating everything near your crosshair, is low accuracy, and VERY long reload time.", "Karabin czarnoprochowy, będący w stanie wystrzelić 7 pocisków na raz! Ceną za anihilację wszystkiego w pobliżu celownika, jest BARDZO długi czas przeładowania, i niska celność."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 160),
                    new JFloat (JType.VariableA, 7f),
                    new JInt (JType.Attachment, 0)
                }),
                new (5f, 25f, 5f)
            ),
            new(this, new string[]{"Fish fillet", "Filet z ryby"},
                new string[]{"A slice of fish, obtained by cutting and roasting fish.", "Kawałek ryby, pozyskany po przez cięcie i smażenie ryby."},
                new JClass (161, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Frozen pizza", "Mrożona pizza"},
                new string[]{"Once heated up, it'll provide you a few slices of pizza.", "Po obróbce termicznej, otrzymasz parę kawałków pizzy."},
                new JClass (162, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Pizza slice", "Kawałek pizzy"},
                new string[]{"A fancy italian dish for the masses.", "Pyszne włoskie danie dla mas."},
                new JClass (163, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Cake", "Tort"},
                new string[]{"A big cake sliced into a few pieces. Will take a while before you eat it all.", "Bardzo duże ciasto, pocięte na parę kawałków. Sporzycie całości trochę zajmie."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 164),
                    new JFloat (JType.VariableA, 4f)
                }),
                new (5f, 0f, 0f)
            ),
            new(this, new string[]{"Cheap wine", "Tanie wino"},
                new string[]{"Château de Jabol, year 2005. Drinking will result in 30 seconds of drunkeness. Contains sulfites.", "Château de Jabol, rocznik 2005. Wypicie spowoduje 30 sekund upicia. Zawiera siarczyny."},
                new JClass (165, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Raw meat", "Surowe mięso"},
                new string[]{"A large piece of dead animal. You should probably cook it first.", "Spory kawałek martwego zwierzęcia. Lepiej będzie je poddać obróbce termicznej."},
                new JClass (166, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Smokes", "Papierosy"},
                new string[]{"They temporarily suppress your hunger, at the cost of your health. Just like in real life!", "Chwilowo wstrzymuje głód, kosztem twojego zdrowia. Dokładnie jak w prawdziwym życiu!"},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 167),
                    new JFloat (JType.VariableA, 5f)
                })
            ),
            new(this, new string[]{"Watermelon", "Arbuz"},
                new string[]{"A very big fruit. Should be smashed/sliced into smaller pieces first.", "Bardzo duży owoc. Przed spożyciem potnij/rozłup na mniejsze kawałki."},
                new JClass (168, JTemplate.BasicItemStackable),
                new (5f, 75f, 0f)
            ),
            new(this, new string[]{"Watermelon slice", "Kawałek arbuza"},
                new string[]{"Delicious yet watery slice of melon. It has ability to rehydrate you, but you can also get stained with water.", "Pyszny kawałek arbuza. Spożycie go cię nawodni, ale również można się przy tym poplamić."},
                new JClass (169, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Carrot", "Marchew"},
                new string[]{"An orange taproot vegetable.", "Pomarańczowy palowy system korzenny."},
                new JClass (170, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Plastic", "Plastik"},
                new string[]{"A piece of plastic. Used mainly for crafting.", "Kawałek plastiku. Głównie używany w tworzeniu."},
                new JClass (171, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Web", "Sieć"},
                new string[]{"A spider's web. Used mainly for crafting.", "Sieć pajęcza. Głównie używany w tworzeniu."},
                new JClass (172, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Empty bottle", "Pusta butelka"},
                new string[]{"An empty plastic bottle. Can be recycled into plastic, which can be used in crafting.", "Pusta plastikowa butelka. Może być zrecyklingowana w zwykły plastik, którego później można użyć w tworzeniu."},
                new JClass (173, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Empty can", "Pusta puszka"},
                new string[]{"An emtpy metal can. Can be smelted into metal fragments, which can be used in crafting.", "Pusta metalowa puszka. Może być przetopiona w metalowe fragmenty, które później można użyć w tworzeniu."},
                new JClass (174, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Sulfur", "Siarka"},
                new string[]{"A yellow chemical. Used mainly for crafting explosives.", "Żółty materiał chemiczny. Głównie używany w tworzeniu materiałów wybuchowych."},
                new JClass (175, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Water filter", "Filtr do wody"},
                new string[]{"A bottle that can be used to purify water from any sources.", "Butelka mogąca filtrować wodę z różnych źródeł."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 176),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Sea scooter", "Skuter podwodny"},
                new string[]{"A handheld scooter, which emits light, and can be used to swim faster. Works only underwater.", "Butelka mogąca filtrować wodę z różnych źródeł."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 177),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Soap", "Mydełko"},
                new string[]{"You can use it, to get rid of dirtiness.", "Można nim zmyć z siebie brud."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 178),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Torch", "Pochodnia"},
                new string[]{"Lights small area for a short time.", "Oświetla mały obszar na krótki czas."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 179),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Lunar shoes", "Kosmiczne buty"},
                new string[]{"These shoes increase your jumping height, and slightly reduce fall damage velocity.", "Te buty zwiększają twoją wysokość skakania, i lekko obniżają prędkość otrzymywania obrażeń od upadków."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 180),
                    new JFloat (JType.VariableA, 0f),
                    new JInt(JType.ClothingCategory, 3)
                })
            )
        };

        // filler
        List<itemClass> filIC = new();
        filIC.AddRange(ItemCache);
        for (int fil = ItemCache.Length; fil < 990; fil++){
            filIC.Add(null);
        }

        // treasures
        filIC.AddRange( new itemClass[]{
            new(this, new string[]{"Teal", "Cyjan"},
                new string[]{"Treasure found in silos. Upon using it, you'll enter a teal state, where you'll be free from any dangers.", "Skarb znajdywalny w silosach. Po jego użyciu, wejdziesz w stan cyjanowy, gdzie będziesz bezpieczny od wszelakich niebezpieczeństw."},
                new JClass(990, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Grail", "Gral"},
                new string[]{"Treasure found in churches. You can drink from it, to regain all of the lost health and hunger, and to get rid of any negative buffs. Has 3 uses.", "Skarb znajdywalny w kościołach. Możesz z niego pić, by odnowić swoje zdrowie oraz głód, i by pozbyć się wszelkich złych efektów. Ma 3 użycia."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 991),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Travelstone", "Kamieńpodróży"},
                new string[]{"Treasure found in lighthouses. You can throw it, and teleport to the place where it lands.", "Skarb znajdywalny w latarniach morskich. Możesz nim rzucić, i teleportować się tam gdzie spadnie."},
                new JClass(992, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Sapphire spear", "Włócznia szafirowa"},
                new string[]{"Treasure found in hangars. It is a weapon, that can kill anything in one hit, but only can be used 4 times.", "Skarb znajdywalny w hangarach. Jest to broń, która zabije każdą żywą istotę, jednym uderzeniem, jednak można z niej skorzystać tylko 4 razy."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 993),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"White gold armor", "Zbroja z białego złota"},
                new string[]{"Treasure found in quarries. It is an armor, that gives you: 300 max health, +6 inventory slots, and +3m/s walking speed.", "Skarb znajdywalny w kamieniołomach. Jest to zbroja, po której założeniu otrzymujesz: 300 maksymalnej ilości zdrowia, +6 miejsc w inwentarzu, oraz +3m/s prędkości chodzenia."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 994),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.ClothingCategory, 2)
                })
            ),
            new(this, new string[]{"Spark", "Iskra"},
                new string[]{"Treasure found in castles. A blue, bright light source that has infinite lifetime.", "Skarb znajdywalny w zamkach. Jasne niebieskie źródło światła, z nieskończoną żywotnością."},
                new JClass(995, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Golden AK47", "Złoty AK47"},
                new string[]{"Treasure found in bunkers. It's basically an AK47 rifle, but it doesn't need any magazines to reload.", "Skarb znajdywalny w bunkrach. Jest to zwykły karabin AK47, tylko że nie potrzebuje żadnych magazynków do przeładowywania."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 996),
                    new JFloat (JType.VariableA, 0f),
                    new JInt (JType.Attachment, 0)
                })
            ),
            new(this, new string[]{"Tesla rifle", "Karabin Tesli"},
                new string[]{"Treasure found in radar stations. It can fire short lightning bolts. Recharges itself slowly overtime.", "Skarb znajdywalny w stacjach radarowych. Może strzelać prądem. Swoją energie regeneruje z czasem powoli."},
                new JClass(new JEntry[]{
                    new JInt (JType.ID, 997),
                    new JFloat (JType.VariableA, 100f)
                })
            ),
            new(this, new string[]{"Ruby ring", "Rubinowy pierścien"},
                new string[]{"Treasure found in mazes. When held, it'll protect you from any damage, but it'll slow you down by a lot.", "Skarb znajdywalny w labiryntach. Trzymany, obroni cię przed wszelkimi obrażeniami, aczkolwiek będziesz się poruszać wolniej."},
                new JClass(998, JTemplate.BasicItemStackable)
            ),
            new(this, new string[]{"Present", "Prezent"},
                new string[]{"Treasure found in ships. Upon using it, it'll drop a lot of random items.", "Skarb znajdywalny w statkach. Po jego użyciu, upuści sporo losowych przedmiotów."},
                new JClass(999, JTemplate.BasicItemStackable)
            )
        });

        ItemCache = filIC.ToArray();

    }

    public float FixedPerlinNoise(float X, float Y) {

        float ToSet = Mathf.Clamp((Mathf.PerlinNoise(X, Y) * 2f) - 0.5f, 0f, 1f);
        return ToSet;

    }

    public void SetDrawDistance (Camera camera, float distance) {
        distance = Mathf.Clamp(distance, 0f, GraphicsQuality == 0 ? 50 : Mathf.Infinity);

        RenderSettings.fogEndDistance = distance;
        camera.farClipPlane = distance;
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

        string Result = "null";

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
                case "TreasuresFound":
                    Result = SetString("Treasures found: ", "Znalezione skarby: ") + TypeScore[1];
                    break;
                case "ItemsUnderwaterFound":
                    Result = SetString("Items found underwater: ", "Znalezione pod wodą przedmioty: ") + TypeScore[1];
                    break;
                case "ObjectsDestroyed":
                    Result = SetString("Objects destroyed: ", "Zniszczono obiektów: ") + TypeScore[1];
                    break;
                case "ChestsDestroyed":
                    Result = SetString("Chests destroyed: ", "Zniszczono skrzyń: ") + TypeScore[1];
                    break;
                case "ChestsOpened":
                    Result = SetString("Chests opened: ", "Otwarto skrzyń: ") + TypeScore[1];
                    break;
                case "PickedLocks":
                    Result = SetString("Picked locks: ", "Otwarto wytrychem: ") + TypeScore[1];
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
                case "TotalCasualRounds":
                    Result = SetString("Total amount of rounds in casual mode: ", "Całkowita liczba przetrwanych rund w trybie swobodnym: ") + TypeScore[1];
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
                case "MostCasualRounds":
                    Result = SetString("Most rounds in a row, in casual mode: ", "Najwięcej przeżytych rund, w trybie swobodnym: ") + TypeScore[1];
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
                case "TradeBuy":
                    Result = SetString("Trades (items bought): ", "Handle (kupiono): ") + TypeScore[1];
                    break;
                case "TradeSell":
                    Result = SetString("Trades (items sold): ", "Handle (sprzedano): ") + TypeScore[1];
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
        string TimeToDisp;
        if(Second <= 60){
            TimeToDisp = Second.ToString();
        } else if (Second <= 3600){
            string Sex = "0" + (Second%60).ToString();
            Sex = Sex.Substring(Sex.Length-2, 2);
            TimeToDisp = ((int)(Second/60f)).ToString() + ":" + Sex;
        } else {
            string Minor = "0" + ((int)((Second/60f)%60f)).ToString();
            Minor = Minor.Substring(Minor.Length-2, 2);
            string Sex = "0" + (Second%60).ToString();
            Sex = Sex.Substring(Sex.Length-2, 2);
            TimeToDisp = ((int)(Second/3600f)).ToString() + ":" + Minor + ":" + Sex;
        }
        return TimeToDisp;
    }

    void OnApplicationQuit(){
        if(CanSaveStuff)
            SaveSettings(0);
        else
            PlayerPrefs.DeleteAll();
    }

}


