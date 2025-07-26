using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    // Main Variables
    public int State = 0; // 0 InActive   1 Alive   2 Dead
    public bool IsCasual = false;
    // Stats
    public float[] Health = new float[] { 100f, 100f };
    public float[] Food = new float[] { 0f, 30f };
    public float[] FoodLimits = new float[]{0f, 0f}; // below hungry, below normal
    public float[] Energy = new float[] { 100f, 100f };
    public float[] Oxygen = new float[] { 20f, 20f };
    public float Speed = 5f; // meters per second
    public float SwimmingSpeed = 5f;
    public float IsCrouching = 0f;
    public string ArmType = "ConscriptJacket";
    // Stats
    // Inventory
    public string InventoryText;
    public string[] Inventory; // id for ids, va for variable a, at for attachment, cl for color, sw scan what
    public string EquipmentText;
    public string[] Equipment; // ct for category, tr for turn on/off
    public int CurrentItemHeld = 0;
    public int MaxInventorySlots = 4;
    float Ach_Umbrella = 0f;
    // Inventory
    // Building
    public string currBuild = "";
    int styleBuild = 0;
    float maxBuildAngle = 0f;
    bool plant = false;
    public GameObject[] Buildings;
    public Transform objBuild;
    public Vector3 posBuild;
    public float rotBuild;
    public Material buildMat;
    // Building
    // Buffs
    public string BuffsText = "";
    public float Bleeding = 0f;
    public float Hydration = 0f;
    public float Infection = 0f;
    public float Tiredness = 0f;
    public float Radioactivity = 0f;
    public float Coldness = 0f;
    public float Adrenaline = 0f;
    public float Drunkenness = 0f;
    public int BrokenBone = 0;
    public float Wet = 0f;
    float PrevWet = -10f;
    public float Hot = 0f;
    public float Fire = 0f;
    public GameObject FireObj;
    public float Campfire = 0f;
    // Buffs
    // Look
    public int HairColor = 0;
    public int SkinColor = 0;
    public string GasMask = "Standard";
    public string Shirt = "ConscriptJacket";
    string PrevShirt = "";
    public string Hat = "";
    public string Bonus = "";
    // Look
    // Main Variables

    // References
    public GameScript GS;
    public RoundScript RS;
    public Transform MainCamera;
    public Transform MinimapCamera;
    public CanvasScript MainCanvas;
    public Transform LookDir;
    public Transform AngleCheck;
    public Light NightVisionLight;
    public GameObject Raining;
    public GameObject Bubbles;
    public GameObject ItemsShown;
    public float[] TempItemShown = new float[]{ 0f, 0f };
    public GameObject GroundDetectorObj;
    public Transform Soundbank;
    public AudioClip[] SoundBankAudios;
    public AudioSource[] GeigerCounter;
    public GameObject ItemPrefab;
    public GameObject EffectPrefab;
    //public GameObject AttackPrefab;
    public GameObject SpecialPrefab;
    public GameObject[] Gibs;
    // References

    // Misc
    public GameObject InteractedGameobject;
    public bool IsGrounded = false;
    float lastGroundY = 0f;
    Vector3 MoveDirNorm, MoveDirSpeed;
    bool InWater = false;
    public bool IsSwimming = false;
    public bool IsNV = false;
    public bool IsHS = false;
    public bool IsST = false;
    public bool IsAiming = false;
    int BulletsLoaded = 0;
    public bool InBox = false;
    int[] previousItem;
    float FootstepCooldown = 1f;
    float JumpCooldown = 0f;
    string WhatWalkingOn = "";
    public float[] CameraShakeForce;
    public GameObject DamageIndicator;
    public GameObject SlimEnd;
    public Vector3 HoloSight;
    public Vector3 AimPart;
    public GameObject BulletChamber;
    public GameObject Scanner;
    public GameObject ScannersScan;
    public string KilledBy;
    public float[] MicroSiverts;
    public float[] ZoomValues;
    public float GunSpreadPC;
    float[] DrunkFluctuations;   // Cooldown FOV Rotation
    float ParryingDamage = 0f;
    bool PermissionToFireBow = false;
    bool MapRead = false;
    public Color32 LaserColor;
    public Vector3 PushbackForce;
    public float ReturnPushback;
    float DropWater = 1f;
    int BulletLoad = 0;
    float DropOrThrow = 0f;
    bool DropButtonHeld = false;
    public GameObject FishingRodBait;
    public bool IsFishing = false;
    public Vector2 FishingStatus;
    float CheckForInteractables = 1f;
    public List<GameObject> ScannedInteractables;
    public string[] ReloadInfo = {"0", "None"};
    // Camera position variables
    public string POV = "FPP";
    public float ReleaseCamera = 0f;
    float LookX = 0f;
    float LookY = 0f;
    float CBAxis = 0f;
    float BonusZ = 0f;
    public Vector3 ItemShakePos;
    int CBDir = 1;
    public Vector3 CameraRecoil;                                // CHANGE THESE TWO FOR IMPROVED RECOIL MECHANICS
    public float[] CameraRecoilVars = new float[] { 0f, 1f };   // CHANGE THESE TWO FOR IMPROVED RECOIL MECHANICS
    public float[] FOVoffset = {0f, 0f};
    public Vector3[] CAMvectors;
    public float[] CAMvariables = {1f, 1f, 60f};
    // Camera position variables
    List<GameObject> scans;
    float scanBuffer = 0f;

    // Cants
    public float CantMove = 0f;
    public float CantLook = 0f;
    public float CantUseItem = 0f;
    public float CantSwitchItem = 0f;
    //public float CantPickOrDrop = 0f;
    public float CantInteract = 0f;
    public float CantCraft = 0f;
    float IsReloading = 0f;
    float EnergyRegen = 0f;
    float GunSpreadRegain = 0f;
    float[] UseDelay = {0f,0f};
    // Cants
    GameObject NullTester = null;
    // Misc

    // Use this for initialization
    void Start() {

        if (GS == null || RS == null || MainCamera == null) {
            if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(GameObject.Find("_RoundScript")) RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();

            if(GameObject.Find("MainCamera")) MainCamera = GameObject.Find("MainCamera").transform;
            MainCamera.SetParent(LookDir);
            MainCamera.localPosition = Vector3.zero;
            MainCamera.rotation = LookDir.rotation;
            ItemsShown.transform.SetParent(MainCamera.transform);
            MinimapCamera = MainCamera.GetChild(2);

            if(GameObject.Find("MainCanvas")) MainCanvas = GameObject.Find("MainCanvas").GetComponent<CanvasScript>();
        }

        State = 1;
        IsCasual = RS.IsCausual;
        CameraShakeForce = new float[] { 0f, 0f, 0f };
        previousItem = new int[] { 0, 0 };
        MicroSiverts = new float[] { 0f, 0f, 0f };
        ZoomValues = new float[] { 0f, 0f, 0f, 0f };  // ZoomedFOV CurrentFOV NormalFOV AimCooldown
        DrunkFluctuations = new float[] { 10f, 0f, 0f };
        CameraRecoilVars = new float[] { 0f, 1f };
        CAMvectors = new Vector3[]{Vector3.zero, Vector3.zero};

        // Sleeping Bag
        if (Tiredness > 0f) {
            for (int CheckINV = 0; CheckINV <= 9; CheckINV++) {
                if (GS.GetSemiClass(Inventory[CheckINV], "id") == "50") {
                    InvGet(CheckINV.ToString(), 1); //Inventory[CheckINV] = "id";
                    Tiredness = 0f;
                    GS.Mess(GS.SetString("Sleeping Bag used", "Użyto śpiworu"));
                    break;
                }
            }
        }
        // Sleeping Bag

        foreach (Interactions GetInt in GameObject.FindObjectsOfType<Interactions>()) {
            ScannedInteractables.Add(GetInt.gameObject);
        }

        // Wear clothes
        Shirt = GS.PS.cl_Clothes;
        Hat = GS.PS.cl_Hat;
        Bonus = GS.PS.cl_Misc;
        SkinColor = GS.PS.cl_Skin;
        HairColor = GS.PS.cl_Hair;
        SetArmModel(Shirt, true);

        // Prepare buildings
        Material[] setMats(MeshRenderer sm){
            Material[] res = new Material[sm.materials.Length];
            for(int repMat = 0; repMat < res.Length; repMat++) res[repMat] = buildMat;
            return res;
        }

        foreach(Transform getObj in objBuild){
            if(getObj.GetComponent<MeshRenderer>()) getObj.GetComponent<MeshRenderer>().materials = setMats(getObj.GetComponent<MeshRenderer>());
            else foreach(Transform subMesh in getObj) if (subMesh.GetComponent<MeshRenderer>()) {
                subMesh.GetComponent<MeshRenderer>().materials = setMats(subMesh.GetComponent<MeshRenderer>());
            }
            getObj.gameObject.SetActive(false);
        }
        // Prepare buildings

        // Get rewards and punishments
        if (GS.ExistSemiClass(GS.PlaythroughStats, "RItemGot_")){
            for(int ByThatMuch = int.Parse(GS.GetSemiClass(GS.PlaythroughStats, "RItemGot_")); ByThatMuch > 0; ByThatMuch--){
                int GivenID = RS.TotalItems[(int)Random.Range(0f, RS.TotalItems.Length - 0.1f)];
                InvGet(GS.itemCache[GivenID].startVariables, 0);
            }
            GS.PlaythroughStats = GS.RemoveSemiClass(GS.PlaythroughStats, "RItemGot_");
        }

        if (GS.ExistSemiClass(GS.PlaythroughStats, "RTreasure_")){
            for(int ByThatMuch = int.Parse(GS.GetSemiClass(GS.PlaythroughStats, "RTreasure_")); ByThatMuch > 0; ByThatMuch--){
                int GivenID = (int)Random.Range(990f, 999.9f);
                InvGet(GS.itemCache[GivenID].startVariables, 0);
            }
            GS.PlaythroughStats = GS.RemoveSemiClass(GS.PlaythroughStats, "RTreasure_");
        }
        // Get rewards and punishments

    }

    void Update(){

        Looking();
        BuildingFunctions();
        LaserColor = Color.HSVToRGB((float)GS.LaserColor / 10f, 1f, 1f);

    }
    

    void FixedUpdate () {

        // Null tester
        if (Input.GetKey(KeyCode.RightBracket)) {
            NullTester.transform.position += Vector3.up;
        }
        // Null tester

        // Prevent Leaving map
        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x, -250f, 250f),
            this.transform.position.y,
            Mathf.Clamp(this.transform.position.z, -250f, 250f));
        // Prevent Leaving map

        // Reset map if not loaded
        if (this.transform.position.y < GameObject.Find("_RoundScript").GetComponent<RoundScript>().ResetHeight && IsSwimming == false && GS.GameModePrefab.x == 0) {
            GameObject.Find("_RoundScript").GetComponent<RoundScript>().SpecialEvent("Reset");
        }
        // Reset map if not loaded

        if (GS == null || RS == null || MainCamera == null) {
            if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(GameObject.Find("_RoundScript")) RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
            if(GameObject.Find("MainCamera")) MainCamera = GameObject.Find("MainCamera").transform;
            MainCamera.transform.SetParent(LookDir);
            MainCamera.transform.localPosition = Vector3.zero;
            MainCamera.transform.rotation = LookDir.rotation;
            ItemsShown.transform.SetParent(MainCamera.transform);
            SetArmModel(Shirt, true);
            if(GameObject.Find("MainCanvas")) MainCanvas = GameObject.Find("MainCanvas").GetComponent<CanvasScript>();
        } else {

            // Cants
            if (CantMove > 0f) CantMove -= 0.02f;
            if (CantLook > 0f) CantLook -= 0.02f;
            if (CantUseItem > 0f) CantUseItem -= 0.02f;
            if (CantSwitchItem > 0f) CantSwitchItem -= 0.02f;
            if (CantCraft > 0f)  CantCraft -= 0.02f;
            if (CantInteract > 0f) CantInteract -= 0.02f;
            if (EnergyRegen > 0f) EnergyRegen -= 0.02f;
            if (IsReloading > 0f) IsReloading -= 0.02f;
            if (GunSpreadRegain > 0f) GunSpreadRegain -= 0.02f;
            if (GunSpreadPC > 0f && GunSpreadRegain <= 0f) GunSpreadPC -= 0.02f;
            if (JumpCooldown > 0f) JumpCooldown -= 0.2f;
            // Cants

            // Night Vision
            if (IsNV == true && State == 1) {
                NightVisionLight.enabled = true;
            } else {
                NightVisionLight.enabled = false;
            }
            // Night Vision

            // Fishing rod
            if (IsFishing == true && GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "129") {
                FishingRodBait.SetActive(true);
                FishingRodBait.transform.SetParent(null);
                FishingRodBait.transform.GetChild(0).LookAt(SlimEnd.transform.position + (SlimEnd.transform.forward / 6f));
                FishingRodBait.transform.GetChild(0).localScale = new Vector3(1f, 1f, Vector3.Distance(FishingRodBait.transform.position, SlimEnd.transform.position + (SlimEnd.transform.forward / 8f)));
                if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "129" || Vector3.Distance(this.transform.position, FishingRodBait.transform.position) > 25f) {
                    IsFishing = false;
                }
                if (FishingStatus.x == 0f) {
                    Ray CheckWhatHit = new Ray(FishingRodBait.transform.position, FishingRodBait.GetComponent<Rigidbody>().velocity.normalized);
                    RaycastHit CheckWhatHitHIT;
                    if (Physics.Raycast(CheckWhatHit, out CheckWhatHitHIT, 1f)) {
                        if (CheckWhatHitHIT.collider.gameObject.layer == 4 || CheckWhatHitHIT.collider.gameObject.layer == 16) {
                            // Water
                            FishingStatus = new Vector2(2f, Random.Range(5f, 30f));
                            GameObject Splash = Instantiate(EffectPrefab) as GameObject;
                            Splash.GetComponent<EffectScript>().EffectName = "BullethitWater";
                            Splash.transform.position = FishingRodBait.transform.position;
                        } else if (CheckWhatHitHIT.collider.transform.root.tag == "Mob") {
                            // Mob
                            FishingStatus = new Vector2(3f, Random.Range(0f, 10f));
                            CheckWhatHitHIT.collider.transform.root.GetComponent<MobScript>().PushbackForce = FishingRodBait.transform.GetChild(0).forward * 15f;
                            CheckWhatHitHIT.collider.transform.root.GetComponent<MobScript>().ReturnPushBack = 1f;
                            if(CheckWhatHitHIT.collider.transform.root.GetComponent<MobScript>().Panic > 0f) GS.PS.AchProg("Ach_Fisherman", "", new string[]{"vnpc_", "1"});
                        } else {
                            // Nothing
                            FishingStatus = new Vector2(1f, Random.Range(0f, 10f));
                            GameObject Splash = Instantiate(EffectPrefab) as GameObject;
                            Splash.GetComponent<EffectScript>().EffectName = "BullethitBlock";
                            Splash.transform.position = FishingRodBait.transform.position;
                        }
                    }
                } else if (FishingStatus.x == 2f) {
                    FishingRodBait.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    if (FishingStatus.y > 0f) {
                        FishingStatus.y -= 0.02f;
                    } else {
                        IsFishing = false;
                        ItemsShown.GetComponent<Animator>().Play("FishingRod-Pull", 0, 0f);
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Crank";
                        int Chance = (int)Random.Range(0f, 6.9f);
                        if (Chance == 0 || Chance == 1) {
                            GS.Mess(GS.SetString("You caught nothing", "Nic nie złowiłeś"), "Error");
                        } else if (Chance == 2) {
                            GS.Mess(GS.SetString("Your fishing rod broke!", "Twoja wędka się popsuła!"), "ItemBroke");
                            InvGet(CurrentItemHeld.ToString(), 1);//Inventory[CurrentItemHeld] = "";
                        } else if (Chance == 3 || Chance == 4 || Chance == 5) {
                            int[] FishesChance = new int[] { 116, 116, 116, 116, 117, 117, 118 };
                            GS.Mess(GS.SetString("You caught a fish!", "Złowiłeś rybę!"), "Good");
                            GameObject SpawnFish = Instantiate(ItemPrefab) as GameObject;
                            SpawnFish.transform.position = FishingRodBait.transform.position;
                            int pickedFish = (int)Random.Range(0f, FishesChance.Length - 0.1f);
                            SpawnFish.GetComponent<ItemScript>().Variables = GS.itemCache[FishesChance[pickedFish]].startVariables;
                            SpawnFish.GetComponent<ItemScript>().State = 2;
                            SpawnFish.GetComponent<ItemScript>().ThrownDirection = FishingRodBait.transform.GetChild(0).forward;
                            if(pickedFish == 6) GS.PS.AchProg("Ach_Fisherman", "", new string[]{"vcarp_", "1"});
                        } else if (Chance == 6) {
                            GS.Mess(GS.SetString("You caught some trash!", "Złowiłeś jakiś przedmiot!"), "Good");
                            GameObject SpawnFish = Instantiate(ItemPrefab) as GameObject;
                            SpawnFish.transform.position = FishingRodBait.transform.position;
                            SpawnFish.GetComponent<ItemScript>().Variables = GS.itemCache[RS.TotalItems[(int)Random.Range(0f, RS.TotalItems.Length - 0.1f)]].startVariables;
                            SpawnFish.GetComponent<ItemScript>().State = 2;
                            SpawnFish.GetComponent<ItemScript>().ThrownDirection = FishingRodBait.transform.GetChild(0).forward + FishingRodBait.transform.GetChild(0).up;
                            GS.PS.AchProg("Ach_Fisherman", "", new string[]{"vgarbage_", "1"});
                        }
                    }
                } else if (FishingStatus.x == 1f) {
                    if (FishingStatus.y <= 1f) {
                        GS.Mess(GS.SetString("Your fishing rod broke!", "Twoja wędka się popsuła!"), "ItemBroke");
                        InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                        ItemsShown.GetComponent<Animator>().Play("Fishing-Catch", 0, 0f);
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Crank";
                    }
                    FishingRodBait.GetComponent<Rigidbody>().velocity = new Vector3(0f, FishingRodBait.GetComponent<Rigidbody>().velocity.y, 0f);
                } else if (FishingStatus.x == 3f) {
                    if (FishingStatus.y <= 1f) {
                        GS.Mess(GS.SetString("Your fishing rod broke!", "Twoja wędka się popsuła!"), "ItemBroke");
                        InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                        ItemsShown.GetComponent<Animator>().Play("Fishing-Catch", 0, 0f);
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Crank";
                    } else {
                        IsFishing = false;
                        ItemsShown.GetComponent<Animator>().Play("Fishing-Catch", 0, 0f);
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Crank";
                    }
                    FishingRodBait.GetComponent<Rigidbody>().velocity = new Vector3(0f, FishingRodBait.GetComponent<Rigidbody>().velocity.y, 0f);
                }
            } else {
                if (IsFishing == true) {
                    IsFishing = false;
                }
                FishingStatus = Vector2.zero;
                if (Vector3.Distance(FishingRodBait.transform.position, this.transform.position) < 1f || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "129") {
                    FishingRodBait.transform.position = this.transform.position;
                    FishingRodBait.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    FishingRodBait.SetActive(false);
                    FishingRodBait.transform.SetParent(this.transform);
                } else {
                    FishingRodBait.transform.position = Vector3.Lerp(FishingRodBait.transform.position, this.transform.position, 0.2f);
                    FishingRodBait.transform.GetChild(0).LookAt(SlimEnd.transform.position + (SlimEnd.transform.forward / 6f));
                    FishingRodBait.transform.GetChild(0).localScale = new Vector3(1f, 1f, Vector3.Distance(FishingRodBait.transform.position, SlimEnd.transform.position + (SlimEnd.transform.forward / 8f)));
                    FishingRodBait.SetActive(true);
                    FishingRodBait.transform.SetParent(null);
                }
            }
            // Fishing rod

            // Grounded State
            if (GroundDetectorObj.GetComponent<GroundDetector>().DetectedObject != null) {
                lastGroundY = this.transform.position.y;
                // Check for water
                if (GroundDetectorObj.GetComponent<GroundDetector>().DetectedObject.GetComponent<FootstepMaterial>() != null) {
                    WhatWalkingOn = GroundDetectorObj.GetComponent<GroundDetector>().DetectedObject.GetComponent<FootstepMaterial>().WhatToPlay;
                    if (GroundDetectorObj.GetComponent<GroundDetector>().DetectedObject.GetComponent<FootstepMaterial>().WhatToPlay == "Water") {
                        InWater = true;
                        if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "87" && GS.GameModePrefab.x != 1) {
                            Wet += 0.04f;
                        }
                    } else {
                        InWater = false;
                    }
                } else {
                    InWater = false;
                }

                // Set grounded
                if (IsGrounded == false) {
                    if (this.GetComponent<Rigidbody>().velocity.y < -3f) {
                        GameObject Footstep = Instantiate(EffectPrefab) as GameObject;
                        Footstep.transform.position = this.transform.position;
                        if (InWater == true) {
                            Footstep.GetComponent<EffectScript>().EffectName = "BullethitWater";
                        } else {
                            Footstep.GetComponent<EffectScript>().EffectName = "BodyDrop";
                        }
                        RecoilCam(new Vector3(Mathf.Lerp(1f, 30f, (this.GetComponent<Rigidbody>().velocity.y + 3f) / -7f) * GS.CameraBobbing, 0f, 0f), 1f, 0.1f);
                    }
                    IsGrounded = true;
                }

            } else {
                IsGrounded = false;
                InWater = false;
            }

            // Check for falling damage
            if (this.GetComponent<Rigidbody>().velocity.y < -10f) {
                Ray CheckForFD = new Ray(this.transform.position, Vector3.up * -1f);
                RaycastHit CheckForFDHIT;
                if (Physics.Raycast(CheckForFD, out CheckForFDHIT, 1.5f, GS.IngoreMaskWP)) {
                    if (CheckForFDHIT.collider.GetComponent<FootstepMaterial>() != null) {
                        if (CheckForFDHIT.collider.GetComponent<FootstepMaterial>().WhatToPlay != "Water") {
                            Hurt(Mathf.Abs(this.GetComponent<Rigidbody>().velocity.y), "Falling", true, this.transform.position);
                            this.GetComponent<Rigidbody>().velocity = new Vector3(0f, -9f, 0f);
                        } else {
                            Ray CheckForGround = new Ray(new Vector3(this.transform.position.x, CheckForFDHIT.collider.gameObject.transform.position.y, this.transform.position.z), Vector3.down);
                            RaycastHit CheckForGroundHIT;
                            if (!Physics.Raycast(CheckForGround, out CheckForGroundHIT, 2.5f, GS.IngoreMaskWP)) {
                                SwimmingStance(true);
                            }
                        }
                    } else {
                        Hurt(Mathf.Abs(this.GetComponent<Rigidbody>().velocity.y), "Falling", true, this.transform.position);
                        this.GetComponent<Rigidbody>().velocity = new Vector3(0f, -9f, 0f);
                    }
                }
            }

            if (IsGrounded == true) {
                this.GetComponent<Rigidbody>().drag = 10f;
                if (FootstepCooldown <= 0f) {
                    FootstepCooldown = 1f;
                    GameObject Footstep = Instantiate(EffectPrefab) as GameObject;
                    Footstep.transform.position = this.transform.position;
                    if (IsSwimming == true) {
                        Footstep.GetComponent<EffectScript>().EffectName = "Swimming";
                    } else {
                        Footstep.GetComponent<EffectScript>().EffectName = "Footstep" + WhatWalkingOn;
                    }
                } else {
                    FootstepCooldown -= (this.GetComponent<Rigidbody>().velocity.magnitude / Speed) / 30f;
                }
            } else {
                this.GetComponent<Rigidbody>().drag = 0f;
            }
            // Grounded State

            if (CheckForInteractables <= 0f) {
                if (InteractedGameobject != null) {
                    CheckForInteractables = 0.1f;
                } else {
                    CheckForInteractables = 0.25f;
                }
                float ClosestIntAngle = 45f;
                GameObject ClosestIntObj = null;
                for (int CheckSINT = 0; CheckSINT < ScannedInteractables.ToArray().Length; CheckSINT++) {
                    GameObject CheckedInt = ScannedInteractables.ToArray()[CheckSINT];
                    if (CheckedInt != null) {
                        Vector3 ActualPos = CheckedInt.transform.position + (CheckedInt.transform.right * CheckedInt.GetComponent<Interactions>().Offset.x) + (CheckedInt.transform.forward * CheckedInt.GetComponent<Interactions>().Offset.z) + (CheckedInt.transform.up * CheckedInt.GetComponent<Interactions>().Offset.y);
                        AngleCheck.LookAt(ActualPos);
                        Ray CheckIfSeen = new Ray(AngleCheck.position, AngleCheck.forward);
                        RaycastHit CheckIfSeenHIT;
                        if (Vector3.Distance(ActualPos, this.transform.position) < CheckedInt.GetComponent<Interactions>().InteractDistance && Quaternion.Angle(LookDir.rotation, AngleCheck.rotation) < ClosestIntAngle) {
                            if (Physics.Raycast(CheckIfSeen, out CheckIfSeenHIT, Mathf.Infinity, GS.IngoreMaskWP) && CheckIfSeenHIT.collider.gameObject == CheckedInt) {
                                ClosestIntAngle = Quaternion.Angle(LookDir.rotation, AngleCheck.rotation);
                                ClosestIntObj = CheckedInt.gameObject;
                            }
                        }
                    } else {
                        ScannedInteractables.Remove(CheckedInt);
                        CheckSINT -= 1;
                    }
                }
                if (ClosestIntObj != null) {
                    InteractedGameobject = ClosestIntObj;
                } else {
                    InteractedGameobject = null;
                }
            } else {
                CheckForInteractables -= 0.02f;
            }

            // Check For Interacted Gameobject

            // State Specific
            if (State == 1) {

                Movement();
                InteractionFunctioning();
                InventoryFunctions("");
                EquipmentFunctions("");
                Buffs("");

                // Stats
                Health[0] = Mathf.Clamp(Health[0], 0f, Mathf.Infinity);
                if (Health[0] > Health[1]) 
                    Health[0] = Mathf.Clamp(Health[0] - 0.02f, 100f, Mathf.Infinity);
                Food[0] = Mathf.Clamp(Food[0], 0f, Mathf.Infinity);
                Energy[0] = Mathf.Clamp(Energy[0], 0f, Energy[1]);
                Oxygen[0] = Mathf.Clamp(Oxygen[0], 0f, Oxygen[1]);

                if (Health[0] <= 0f) {
                    State = 2;
                    GS.Mess(KilledBy);
                    if(GS.Round > 1) GS.PS.AchProg("Ach_TheCycleBegins", "0");
                    if(GS.Round == 19 && GS.GameModePrefab.x == 0) GS.PS.AchProg("Ach_AWholeWeek", "0");
                    Debug.LogError("Here, add a new game over screen!");
                    GS.NeueScore = new string[]{
                        "S" + GS.Score.ToString() + ";R" + GS.Round.ToString() + ";G" + GS.GetSemiClass(GS.RoundSetting, "G", "?") + ";D" + GS.GetSemiClass(GS.RoundSetting, "D", "?") + ";N" + GS.SaveFileName + ";P" + GS.GetSemiClass(GS.RoundSetting, "P", "?"),
                        GS.PlaythroughStats
                    };
                    GS.SaveManipulation(GS.CurrentSave, 2);
                }

                if (EnergyRegen <= 0f && Energy[0] < Energy[1]) {
                    float DrunkSlowDown = Mathf.Clamp(1f - (Drunkenness / 100f), 0.25f, 1f);
                    if (Hydration > 0f) {
                        Energy[0] += 1f * DrunkSlowDown;
                    } else {
                        Energy[0] += 0.5f * DrunkSlowDown;
                    }
                }

                if (IsSwimming == true || IsST == true) {
                    if (Oxygen[0] > 0f && IsST == false) {
                        Oxygen[0] -= 0.02f;
                    } else if (Oxygen[0] <= 0f) {
                        Hurt(0.25f, "Drowning", false, Vector3.zero);
                    }
                } else {
                    if (Oxygen[0] < Oxygen[1]) {
                        Oxygen[0] += Oxygen[1] / 100f;
                    }
                }

                // Stats
            } else if (State == 2) {

                if (GS.Ragdolls) {

                    if (this.GetComponent<BoxCollider>().enabled == false) {
                        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        this.GetComponent<Rigidbody>().drag = 1f;
                        this.GetComponent<Rigidbody>().AddTorque(Vector3.one * Random.Range(-1f, 1f), ForceMode.Impulse);
                        this.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(0f, 5f), ForceMode.Impulse);
                        this.GetComponent<CapsuleCollider>().height = 1.5f;
                        this.GetComponent<CapsuleCollider>().radius = 0.25f;
                        this.GetComponent<CapsuleCollider>().material.bounciness = 1f;
                        this.GetComponent<BoxCollider>().enabled = true;
                        //this.transform.Rotate(new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f)));
                    }
                    LookDir.localPosition = Vector3.MoveTowards(LookDir.localPosition, Vector3.zero, 0.025f * (Time.deltaTime * 50f));
                    ItemsShown.SetActive(false);

                } else {

                    LookDir.localPosition = Vector3.MoveTowards(LookDir.localPosition, new Vector3(0, -0.75f, 0f), 0.025f * (Time.deltaTime * 50f));
                    ItemsShown.SetActive(false);

                }

                bool OnDeepWater = false;
                Ray CheckForWater = new Ray(LookDir.position, Vector3.down);
                RaycastHit CheckForWaterHIT;
                if (Physics.Raycast(CheckForWater, out CheckForWaterHIT, 0.6f)) {
                    if (CheckForWaterHIT.collider.gameObject.layer == 16) {
                        OnDeepWater = true;
                    }
                }
                if (OnDeepWater == true && IsSwimming == false) {
                    SwimmingStance(true);
                    this.GetComponent<Rigidbody>().useGravity = false;
                }
            } else if (State == 0){

                ReleaseCamera = 10f;
                ItemsShown.SetActive(false);

            }
            // State Specific

        }

    }

    void Looking() {

        if (ReleaseCamera > 0f) {
            ReleaseCamera -= 0.02f * (Time.deltaTime * 50f);
        } else if (POV != "FPP"){
            POV = "FPP";
        }

        // Rotation and zoom stuff specific for FPP camera and maybe third person camera
        if (POV == "FPP"){
            BonusZ = Mathf.Lerp(BonusZ, 0f, 0.1f);
            BonusZ = Mathf.Lerp(BonusZ, Vector3.Dot(this.GetComponent<Rigidbody>().velocity, this.transform.right) * GS.CameraShifting, 0.25f);
            if (CantLook <= 0f) {
                if (GS.InvertedMouse == false) {
                    LookX -= (Input.GetAxis("Mouse Y") * GS.MouseSensitivity) * Time.timeScale;
                } else {
                    LookX += (Input.GetAxis("Mouse Y") * GS.MouseSensitivity) * Time.timeScale;
                }
                LookX = Mathf.Clamp(LookX, -80f, 80f);
                float BonusAdd = ((Input.GetAxis("Mouse X") * GS.MouseSensitivity) * Time.timeScale) * GS.CameraShifting;
                if (BonusZ + BonusAdd > -15f && BonusZ + BonusAdd < 15f) {
                    BonusZ = Mathf.Lerp(BonusZ, BonusZ + BonusAdd, 0.25f);
                }
                LookY += (Input.GetAxis("Mouse X") * GS.MouseSensitivity) * Time.timeScale;
            }

            // Camera rotation
            if (CameraRecoilVars[0] > 0f) {
                float CRVPower = Mathf.Clamp((CameraRecoilVars[0] / CameraRecoilVars[1]) - (CameraRecoilVars[2] / CameraRecoilVars[3]), 0.0001f, Mathf.Infinity);
                if (CameraRecoilVars[2] > 0f) {
                    CameraRecoilVars[2] = Mathf.Clamp(CameraRecoilVars[2] - (0.02f * (Time.deltaTime * 50f)), 0f, CameraRecoilVars[3]);
                } else if (CameraRecoilVars[0] > 0f) {
                    CameraRecoilVars[0] = Mathf.Clamp(CameraRecoilVars[0] - (0.02f * (Time.deltaTime * 50f)), 0f, CameraRecoilVars[1]);
                }
                if (DrunkFluctuations[2] != 0f) {
                    MainCamera.localRotation = Quaternion.Euler((new Vector3(CameraRecoil.x * CRVPower, CameraRecoil.y * CRVPower, (CameraRecoil.z * CRVPower) + BonusZ + DrunkFluctuations[2])) * GS.CameraBobbing);
                } else {
                    MainCamera.localRotation = Quaternion.Euler((new Vector3(CameraRecoil.x * CRVPower, CameraRecoil.y * CRVPower, (CameraRecoil.z * CRVPower) + BonusZ)) * GS.CameraBobbing);
                }
            } else {
                if (DrunkFluctuations[2] != 0f) {
                    MainCamera.localRotation = Quaternion.Euler(new Vector3(0f, 0f, BonusZ + DrunkFluctuations[2]) * GS.CameraBobbing);
                } else {
                    MainCamera.localRotation = Quaternion.Euler(new Vector3(0f, 0f, BonusZ));
                }
            }
            ItemsShown.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -BonusZ));
            // Camera rotation

            // Zoom manipulation
            float ZoomShrink = Mathf.Lerp(0.66f, 1f, (ZoomValues[1] - ZoomValues[0]) / (ZoomValues[2] - ZoomValues[0]) );
            if (DrunkFluctuations[1] > 0f) {
                MainCamera.GetComponent<Camera>().fieldOfView = ZoomValues[1] * DrunkFluctuations[1];
                MainCamera.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Clamp(ZoomValues[1] * ZoomShrink, 0f, 60f) * DrunkFluctuations[1];
                MainCamera.GetChild(1).GetComponent<Camera>().fieldOfView = MainCamera.GetComponent<Camera>().fieldOfView;
            } else {
                MainCamera.GetComponent<Camera>().fieldOfView = ZoomValues[1];
                MainCamera.GetChild(0).GetComponent<Camera>().fieldOfView = Mathf.Clamp(ZoomValues[1] * ZoomShrink, 0f, 60f);
                MainCamera.GetChild(1).GetComponent<Camera>().fieldOfView = MainCamera.GetComponent<Camera>().fieldOfView;
            }

            if(FOVoffset[1] > 0f) {FOVoffset[1] -= 0.02f; ZoomValues[0] = 0f; ZoomValues[3] = 0f;}
            else if(FOVoffset[0] != 0f) FOVoffset[0] = Mathf.Lerp(FOVoffset[0], 0f, 0.1f);
            ZoomValues[2] = GS.FOV + FOVoffset[0];
            if (ZoomValues[3] > 0f) {
                ZoomValues[3] -= 0.02f * (Time.unscaledDeltaTime * 50f);
                ZoomValues[1] = Mathf.MoveTowards(ZoomValues[1], ZoomValues[0], (Mathf.Abs(ZoomValues[2] - ZoomValues[0]) / 15f) * (Time.unscaledDeltaTime * 100f));
            } else {
                ZoomValues[1] = Mathf.MoveTowards(ZoomValues[1], ZoomValues[2], (Mathf.Abs(ZoomValues[2] - ZoomValues[0]) / 15f) * (Time.unscaledDeltaTime * 100f));
            }
            // Zoom manipulation

        }

        if (POV == "FPP") {

            if (MainCamera.parent != LookDir) {
                MainCamera.SetParent(LookDir);
            }

            if (State == 1) {

                LookDir.transform.localRotation = Quaternion.Lerp(LookDir.transform.localRotation, Quaternion.Euler(new Vector3(LookX, 0f, 0f)), (1f - GS.MouseSmoothness * 0.8f) * (Time.deltaTime * 50f));
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(new Vector3(0f, LookY, 0f)), (1f - GS.MouseSmoothness * 0.8f) * (Time.deltaTime * 50f));

                // Camera when crouching
                if (IsSwimming == false) {
                    LookDir.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0.75f, 0f), new Vector3(0f, -0.1f, 0f), IsCrouching);
                } else {
                    LookDir.transform.localPosition = Vector3.zero;
                }

                // Item held shake
                if (GS.CameraBobbing > 0f) {
                    if (CantLook <= 0f) {
                        ItemShakePos += new Vector3((Input.GetAxis("Mouse X") / 1000f) * (Time.deltaTime * 100f), (Input.GetAxis("Mouse Y") / 1000f) * (Time.deltaTime * 100f), 0f) * GS.CameraBobbing;
                    }
                    ItemShakePos = Vector3.Lerp(ItemShakePos, Vector3.zero, 0.5f);
                } else {
                    ItemShakePos = Vector3.zero;
                }
                ItemShakePos = Vector3.ClampMagnitude(ItemShakePos /* GS.CameraBobbing*/, 0.01f);

                // DrunknessFluctuations
                if (DrunkFluctuations[0] > 0f && GS.CameraBobbing > 0f) {
                    DrunkFluctuations[0] -= 0.01f * (Time.deltaTime * 100f);
                    if (DrunkFluctuations[0] < 5f) {
                        DrunkFluctuations[1] = Mathf.Lerp(
                            Mathf.Lerp(1f, 0.5f, (Drunkenness / 100f)),
                            Mathf.Lerp(1f, 1.5f, (Drunkenness / 100f)), DrunkFluctuations[0] / 5f);
                        DrunkFluctuations[2] = Mathf.Lerp(-30f * (Drunkenness / 100f), 30f * (Drunkenness / 100f), DrunkFluctuations[0] / 5f);
                    } else {
                        DrunkFluctuations[1] = Mathf.Lerp(
                            Mathf.Lerp(1f, 1.5f, (Drunkenness / 100f)),
                            Mathf.Lerp(1f, 0.5f, (Drunkenness / 100f)), (DrunkFluctuations[0] - 5f) / 5f);
                        DrunkFluctuations[2] = Mathf.Lerp(30f * (Drunkenness / 100f), -30f * (Drunkenness / 100f), (DrunkFluctuations[0] - 5f) / 5f);
                    }
                } else if (Drunkenness > 0f && GS.CameraBobbing > 0f) {
                    DrunkFluctuations[0] = 10f;
                } else {
                    DrunkFluctuations = new float[] { 0f, 0f, 0f };
                }

                // Zooming
                float ZoomVar = 1f;
                float ZoomShakeScaler = 1f;
                Vector3 PosChange = Vector3.zero;
                float LerpSpeed = 0.2f;
                if (ZoomValues[1] != ZoomValues[2] && AimPart != null) {
                    ZoomVar = (ZoomValues[1] - ZoomValues[0]) / (ZoomValues[2] - ZoomValues[0]);
                    ZoomShakeScaler = Mathf.Lerp(0.1f, 1f, ZoomVar);
                    PosChange += Vector3.Lerp(AimPart, Vector3.zero, ZoomVar);
                }
                if (ZoomVar > 0f && ZoomVar < 1f) {
                    LerpSpeed = 1f;
                }

                float EnableCB = 1f;
                if (GS.CameraBobbing > 0f) {
                    EnableCB = GS.CameraBobbing;
                } else {
                    EnableCB = 0f;
                }
                //CBAxis *= EnableCB;
                if (IsGrounded == true && IsSwimming == false) {
                    MainCamera.localPosition = Vector3.Lerp(MainCamera.localPosition, new Vector3(CBAxis / 3f, Mathf.Abs(CBAxis) / 4f, 0f), 0.1f * (Time.deltaTime * 50f));
                    ItemsShown.transform.localPosition = Vector3.Lerp(ItemsShown.transform.localPosition + (ItemShakePos * ZoomShakeScaler), (new Vector3(CBAxis / -40f, Mathf.Abs(CBAxis) / 80f, 0f) * ZoomShakeScaler) + PosChange, LerpSpeed * (Time.deltaTime * 50f));
                    if (this.GetComponent<Rigidbody>().velocity.magnitude > 0f) {
                        CBAxis += ((this.GetComponent<Rigidbody>().velocity.magnitude / Speed) / 15f * CBDir) * (Time.deltaTime * 50f) * GS.CameraBobbing;
                        CBAxis = Mathf.Lerp(0f, CBAxis, this.GetComponent<Rigidbody>().velocity.magnitude * (Speed * 3f));
                        if (CBAxis > EnableCB) {
                            CBDir = -1;
                        } else if (CBAxis < -EnableCB) {
                            CBDir = 1;
                        }
                    }
                } else {
                    float YshakeScaler = 0f;
                    if (GS.CameraShifting > 0f) {
                        YshakeScaler = ((this.GetComponent<Rigidbody>().velocity.y / 200f) * ZoomShakeScaler) * GS.CameraShifting;
                    }
                    MainCamera.localPosition = Vector3.Lerp(MainCamera.localPosition, Vector3.zero, 0.1f * (Time.deltaTime * 50f));
                    ItemsShown.transform.localPosition = Vector3.Lerp(ItemsShown.transform.localPosition + (ItemShakePos * ZoomShakeScaler), new Vector3(0f, Mathf.Clamp(YshakeScaler, -0.03f, 0.03f), 0f) + PosChange, LerpSpeed * (Time.deltaTime * 50f));
                }

                // Camera Bobbing

                // Camera Shake
                if (CameraShakeForce[1] > 0f) {
                    MainCamera.localPosition = new Vector3(Random.Range(-CameraShakeForce[0], CameraShakeForce[0]), Random.Range(-CameraShakeForce[0], CameraShakeForce[0]), Random.Range(-CameraShakeForce[0], CameraShakeForce[0])) * (CameraShakeForce[1] / CameraShakeForce[2]);
                }
                if (CameraShakeForce[1] > 0f) {
                    CameraShakeForce[1] -= 0.01f * (Time.deltaTime * 50f);
                }
                // Camera Shake
            }

        } else if (POV == "Spectate"){

            if (MainCamera.parent == LookDir) {
                MainCamera.SetParent(null);
            }

            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, CAMvectors[0], CAMvariables[0]);
            MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, Quaternion.LookRotation(CAMvectors[1] - MainCamera.transform.position), CAMvariables[1]);

            MainCamera.GetChild(0).GetComponent<Camera>().fieldOfView = MainCamera.GetChild(1).GetComponent<Camera>().fieldOfView = MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp( MainCamera.GetComponent<Camera>().fieldOfView, CAMvariables[2], 0.2f * (Time.deltaTime * 50f));

        } else {

            if (MainCamera.parent == LookDir) {
                MainCamera.SetParent(null);
            }

        }

        // Minimap correction
        MinimapCamera.LookAt(MinimapCamera.position + Vector3.Cross(MainCamera.right, Vector3.up));

    }

    bool CheckStamina(float amount, int importance) {
        // Importance: 0 - normal action 1 - sprinting and jumping
        if(IsCasual){
            if (importance == 0 || CantUseItem <= 0f || Hydration > 0f || Adrenaline > 0f)
                return true;
        } else {
            if (Energy[0] >= amount)
                return true;
        }
        return false;
    }

    void StaminaDrain(float amount) {
        if(!IsCasual) {
            Energy[0] -= amount;
            EnergyRegen = 1f;
        }
    }

    void Movement() {

        // Normal vs Swimming state
        if (IsSwimming == false) {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<CapsuleCollider>().height = Mathf.Lerp(2f, 1f, IsCrouching);
            this.GetComponent<CapsuleCollider>().center = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, -0.5f, 0f), IsCrouching);
            GroundDetectorObj.SetActive(true);
        } else {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<CapsuleCollider>().height = 0.5f;
            this.GetComponent<CapsuleCollider>().center = Vector3.zero;
            GroundDetectorObj.SetActive(false);
            InWater = true;
            this.GetComponent<Rigidbody>().drag = 0f;
        }

        // Check if should swim
        Ray CheckIfSwimUP = new Ray(this.transform.position, Vector3.up);
        Ray CheckIfSwimDOWN = new Ray(this.transform.position, Vector3.down);
        List<RaycastHit> Hits = new List<RaycastHit>();
        Hits.AddRange(Physics.RaycastAll(CheckIfSwimUP, Mathf.Infinity, GS.GetComponent<GameScript>().IngoreMaskWP));
        Hits.AddRange(Physics.RaycastAll(CheckIfSwimDOWN, Mathf.Infinity, GS.GetComponent<GameScript>().IngoreMaskWP));
        foreach (RaycastHit CheckIfSwimHIT in Hits) {
            if (CheckIfSwimHIT.collider.gameObject.layer == 16) {
                if (this.transform.position.y < CheckIfSwimHIT.transform.position.y && IsSwimming == false) {
                    SwimmingStance(true);
                } else if (this.transform.position.y > CheckIfSwimHIT.transform.position.y && IsSwimming == true) {
                    SwimmingStance(false);
                }
            }
        }

        if (CantMove <= 0f && IsSwimming == false) {

            // Enter Swimming state
            if (GS.ReceiveButtonPress("Crouch", "Hold") > 0f && GroundDetectorObj.GetComponent<GroundDetector>().DetectedObject != null) {
                if (GroundDetectorObj.GetComponent<GroundDetector>().DetectedObject.name == "DeepWater") {
                    Ray CheckForGround = new Ray(new Vector3(this.transform.position.x, GroundDetectorObj.GetComponent<GroundDetector>().DetectedObject.transform.position.y, this.transform.position.z), Vector3.down);
                    RaycastHit CheckForGroundHIT;
                    if (!Physics.Raycast(CheckForGround, out CheckForGroundHIT, 2.5f, GS.IngoreMaskWP)) {
                        SwimmingStance(true);
                    }
                }
            }

            //if (IsGrounded == true) {

                // Crouching
                if ((GS.ReceiveButtonPress("Crouch", "Hold") > 0f && InWater == false) || InBox == true) {
                    IsCrouching = Mathf.MoveTowards(IsCrouching, 1f, 0.2f);
                } else {
                    IsCrouching = Mathf.MoveTowards(IsCrouching, 0f, 0.2f);
                }

                if (IsCrouching > 0f) {
                    Ray CheckForRoof = new Ray(this.transform.position, Vector3.up);
                    RaycastHit CheckForRoofHIT;
                    if (Physics.Raycast(CheckForRoof, out CheckForRoofHIT, 1f, GS.IngoreMaskWP)) {
                        IsCrouching = 1f;
                    }
                }
                // Crouching

                // Walking
                float GotSpeed = 0f;
                int moved = 0;
                int SlowDown = 0; // 0 Normal   1 Can't sprint   2 Slow Movement   3 Can't jump

                if ((InWater == true && GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "87") || BrokenBone == 1 || IsCrouching > 0f || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "998") {
                    SlowDown = 3;
                } else if (ZoomValues[1] != ZoomValues[2]) {
                    SlowDown = 2;
                } else if (IsHS == true || RS.RoundState == "TealState") {
                    SlowDown = 1;
                } else {
                    SlowDown = 0;
                }

                if (IsCrouching <= 0f && IsGrounded && GS.ReceiveButtonPress("Sprint", "Hold") > 0f && GS.ReceiveButtonPress("Sprint", "Hold") > 0f && CheckStamina(10f, 1) && SlowDown == 0){
                    MoveDirNorm = Vector3.MoveTowards(MoveDirNorm, this.transform.forward, 0.25f);
                    moved = 2;
                } else if (IsGrounded){
                    MoveDirNorm = Vector3.MoveTowards(MoveDirNorm, (this.transform.forward * GS.ReceiveButtonPress("MoveForward", "Hold")) + (this.transform.forward * -GS.ReceiveButtonPress("MoveBackwards", "Hold")) + (this.transform.right * GS.ReceiveButtonPress("MoveRight", "Hold")) + (this.transform.right * -GS.ReceiveButtonPress("MoveLeft", "Hold")), 0.25f);
                    moved = 1;
                } else if (GS.ReceiveButtonPress("MoveForward", "Hold") > 0f ||GS.ReceiveButtonPress("MoveBackwards", "Hold") > 0f||GS.ReceiveButtonPress("MoveRight", "Hold") > 0f || GS.ReceiveButtonPress("MoveLeft", "Hold") > 0f) {
                    MoveDirNorm = Vector3.MoveTowards(MoveDirNorm, (this.transform.forward * GS.ReceiveButtonPress("MoveForward", "Hold")) + (this.transform.forward * -GS.ReceiveButtonPress("MoveBackwards", "Hold")) + (this.transform.right * GS.ReceiveButtonPress("MoveRight", "Hold")) + (this.transform.right * -GS.ReceiveButtonPress("MoveLeft", "Hold")), 0.05f);
                    moved = 1;
                }

                MoveDirNorm = Vector3.ClampMagnitude(MoveDirNorm, 1f);
                PushbackForce = Vector3.Lerp(PushbackForce, Vector3.zero, 0.1f);

                if (InWater == true) {
                    GotSpeed = SwimmingSpeed;
                } else {
                    GotSpeed = Speed;
                }

                float WetSlowDown = 1f;
                if (InWater != true && Wet > 0f) {
                    WetSlowDown = (1f - (Wet / 150f));
                } else if (InWater == true && Wet > 0f) {
                    WetSlowDown = (1f + (Wet / 300f));
                }

                if (moved == 2) {
                    StaminaDrain(.25f);
                    MoveDirSpeed = (MoveDirNorm * (GotSpeed * 2f)) * (1f - (Drunkenness / 150f)) * WetSlowDown;
                    FOVoffset = new float[]{Mathf.Lerp(FOVoffset[0], 7.5f, 0.2f), 0.1f};
                } else if (moved == 1) {
                    if (SlowDown >= 2) MoveDirSpeed = (MoveDirNorm * (GotSpeed / 2f)) * (1f - (Drunkenness / 150f)) * WetSlowDown;
                    else MoveDirSpeed = (MoveDirNorm * GotSpeed) * (1f - (Drunkenness / 150f)) * WetSlowDown;
                }

                if(moved != 0)
                    this.GetComponent<Rigidbody>().velocity = new Vector3(MoveDirSpeed.x, this.GetComponent<Rigidbody>().velocity.y, MoveDirSpeed.z);
                // Walking

                // Jumping
                if ((IsGrounded || this.GetComponent<Rigidbody>().velocity.magnitude <= 0f) && GS.ReceiveButtonPress("Jump", "Hold") > 0f && CheckStamina(25f, 1) && JumpCooldown <= 0f && SlowDown != 3) {
                    ReturnPushback = 0f;
                    JumpCooldown = 1f;
                    StaminaDrain(25f);
                    this.transform.position += Vector3.up * 0.25f;
                    this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x, 0f, this.GetComponent<Rigidbody>().velocity.z);
                    this.GetComponent<Rigidbody>().AddForce(this.transform.up * 10f, ForceMode.VelocityChange);
                    GameObject JumpEffect = Instantiate(EffectPrefab) as GameObject;
                    JumpEffect.transform.position = this.transform.position;
                    JumpEffect.GetComponent<EffectScript>().EffectName = "Jumping";
                    RecoilCam(new Vector3(-10f * GS.CameraBobbing, 0f, 0f), 1f, 0.1f);
                }
                // Jumping

            /*} else {

                // Mid air control
                Vector3 midAir = (this.transform.forward * GS.ReceiveButtonPress("MoveForward", "Hold")) + (this.transform.forward * -GS.ReceiveButtonPress("MoveBackwards", "Hold")) + (this.transform.right * GS.ReceiveButtonPress("MoveRight", "Hold")) + (this.transform.right * -GS.ReceiveButtonPress("MoveLeft", "Hold"));
                float momentum = this.GetComponent<Rigidbody>().velocity.magnitude;
                Vector3 midAirA = Vector3.ClampMagnitude(midAir, 1f) * Mathf.Clamp(momentum, Speed, Speed*2f);
                midAirA.y = this.GetComponent<Rigidbody>().velocity.y;
                if(midAir.magnitude > 0.1f)
                    this.GetComponent<Rigidbody>().velocity = Vector3.MoveTowards(this.GetComponent<Rigidbody>().velocity, midAirA, 0.5f);

            }*/

        } else if (CantMove <= 0f && IsSwimming == true) {

            // Leave swimming state
            Ray CheckForSurface = new Ray(this.transform.position, this.transform.up);
            RaycastHit CheckForSurfaceHIT;
            if (Physics.Raycast(CheckForSurface, out CheckForSurfaceHIT, 0.6f, GS.IngoreMaskWP)) {
                if (CheckForSurfaceHIT.collider.name == "DeepWater") {
                    SwimmingStance(false);
                }
            }

            // Swimming
            float Drown = -0.25f;
            if (GS.ReceiveButtonPress("Jump", "Hold") > 0f || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "87") {
                Drown = 1f;
            }
            Vector3 SwimDir = Vector3.ClampMagnitude(MainCamera.forward * Input.GetAxis("Vertical") + MainCamera.right * Input.GetAxis("Horizontal") + this.transform.up * Drown, 1f);
            Vector3 SwimDirA = (SwimDir * (SwimmingSpeed / 2f)) * (1f - (Drunkenness / 150f));
            this.GetComponent<Rigidbody>().velocity = Vector3.Lerp(this.GetComponent<Rigidbody>().velocity, SwimDirA, 0.1f);

        }

        if (ReturnPushback > 0f) {
            ReturnPushback -= 0.02f;
            if (IsGrounded == true && IsSwimming == false) {
                this.GetComponent<Rigidbody>().velocity += PushbackForce * ReturnPushback;
            } else {
                this.GetComponent<Rigidbody>().velocity = PushbackForce * ReturnPushback;
            }
        }

    }

    void InteractionFunctioning(){

        if (InteractedGameobject != null) {
            if (InteractedGameobject.transform.parent != null && InteractedGameobject.transform.parent.tag == "Interactable" && GS.ReceiveButtonPress("Interaction", "Hold") > 0f) {
                if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "BreakBarel" && CantInteract <= 0f && CheckStamina(10f, 0)) {
                    CantInteract = 1f;
                    StaminaDrain(10f);
                    InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Interaction("Break", 1f);
                } else if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "EscapeTunel" && CantInteract <= 0f) {
                    CantInteract = 1f;
                    ReleaseCamera = 9999f;
                    POV = "Spectate";
                    CAMvectors = new Vector3[]{
                        InteractedGameobject.transform.position + (InteractedGameobject.transform.right * 30f) + (InteractedGameobject.transform.forward * 10f) + (InteractedGameobject.transform.up * Random.Range(-15f, 15f)),
                        InteractedGameobject.transform.position};
                    CAMvariables = new float[]{0.05f, 0.05f, 60f};
                    RS.SpecialEvent("ESCtunnel");
                    if(GS.Round == 5 && GS.GameModePrefab.x == 0) GS.PS.AchProg("Ach_TheNextDay", "0");
                    //CantInteract = 100f;
                    //RS.GetComponent<RoundScript>().SpecialEvent("Escape");
                } else if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "Door" && CantInteract <= 0f) {
                    CantInteract = 0.5f;
                    InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Interaction("Door", 0f);
                } else if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "VendingMachine" && CantInteract <= 0f) {
                    MainCanvas.DialogedMob = InteractedGameobject.transform.parent.gameObject;
                    MainCanvas.DialogSetting = "VendingMachine";
                } else if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "EmergencyItem" && InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Variables.y >= 0f && CantInteract <= 0f) {
                    CantInteract = 0.5f;
                    InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Interaction("GetItem", 0f);
                } else if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "RingBell" && InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Variables.y == 0f && CantInteract <= 0f) {
                    CantInteract = 0.5f;
                    InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Interaction("RingTheBell", 0f);
                } else if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "GrabAmmo" && InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Variables.y > 0f && CantInteract <= 0f) {
                    CantInteract = 0.25f;
                    InteractedGameobject.transform.parent.GetComponent<InteractableScript>().Interaction("GatherAmmo", 0f);
                }
            } else if (InteractedGameobject.tag == "Mob" && GS.ReceiveButtonPress("Interaction", "Hold") > 0f) {
                if (InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "TalkTo" && CantInteract <= 0f) {
                    MainCanvas.DialogedMob = InteractedGameobject;
                    MainCanvas.DialogSetting = "Default";
                    InteractedGameobject.GetComponent<MobScript>().AiMovePosition = this.transform.position;
                }
            }
        }

    }

    public void InvGet(string What, int Spec, int stackAmount = 1){

        switch(Spec){
            case 0: // Add item
                string cash = GS.GetSemiClass(What, "ch");

                if(cash == ""){

                    int stackable = 0;
                    if(GS.ExistSemiClass(What, "sq")) {
                        stackable = 1;
                        stackAmount = int.Parse(GS.GetSemiClass(What, "sq"));
                    }

                    if(stackable == 1) {
                        for (int cfq = 0; cfq <= MaxInventorySlots; cfq++) {
                            if(cfq == MaxInventorySlots) stackable = 2;
                            else if ( GS.GetSemiClass(Inventory[cfq], "id") == GS.GetSemiClass(What, "id") && GS.RemoveSemiClass(Inventory[cfq], "sq") == GS.RemoveSemiClass(What, "sq") ) {
                                Inventory[cfq] = GS.SetSemiClass(Inventory[cfq], "sq", "/+" + stackAmount);
                                break;
                            }
                        }
                    }

                    if(stackable != 1){
                        if(GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "0"){
                            Inventory[CurrentItemHeld] = What;
                        } else {
                            for (int ci = 0; ci <= MaxInventorySlots; ci++) {
                                if(ci == MaxInventorySlots) {
                                    GameObject DropItem = Instantiate(ItemPrefab) as GameObject;
                                    DropItem.transform.position = this.transform.position + LookDir.forward * 1f;
                                    DropItem.GetComponent<ItemScript>().Variables = What;
                                    DropItem.GetComponent<ItemScript>().DroppedBy = this.gameObject;
                                } else if (GS.GetSemiClass(Inventory[ci], "id") == "0") {
                                    Inventory[ci] = What;
                                    break;
                                }
                            }
                        }
                    }

                } else {

                    switch (cash) {
                        case "Money":
                            GS.Money += int.Parse(GS.GetSemiClass(What, "va"));
                            break;
                        case "Ammo":
                            GS.Ammo += int.Parse(GS.GetSemiClass(What, "va"));
                            break;
                    }

                }
                break;
            case 1: // remove item
                if(GS.ExistSemiClass(What, "id")){
                    for(int rs = 0; rs < MaxInventorySlots; rs++) if (Inventory[rs] == What) {
                        if(GS.ExistSemiClass(Inventory[rs], "sq") && int.Parse(GS.GetSemiClass(Inventory[rs], "sq")) > stackAmount) {
                            int keepStack = int.Parse(GS.GetSemiClass(Inventory[rs], "sq")) - stackAmount;
                            Inventory[rs] = GS.itemCache[ int.Parse(GS.GetSemiClass(Inventory[rs], "id")) ].startVariables;
                            Inventory[rs] = GS.SetSemiClass(Inventory[rs], "sq", keepStack.ToString());
                        } else Inventory[rs] = "id0;";
                        break;
                    }
                } else {
                    int at = int.Parse(What);
                    if(GS.ExistSemiClass(Inventory[at], "sq") && int.Parse(GS.GetSemiClass(Inventory[at], "sq")) > stackAmount) {
                        int keepStack = int.Parse(GS.GetSemiClass(Inventory[at], "sq")) - stackAmount;
                        Inventory[at] = GS.itemCache[ int.Parse(GS.GetSemiClass(Inventory[at], "id")) ].startVariables;
                        Inventory[at] = GS.SetSemiClass(Inventory[at], "sq", keepStack.ToString());
                    } else Inventory[at] = "id0;";
                }
                break;
            case 2: case -1: case -2: // drop item - drop item by player - throw item by player
                if(GS.ExistSemiClass(What, "id")){
                    for(int rs = 0; rs < MaxInventorySlots; rs++) if (Inventory[rs] == What) {
                        stackAmount = (int)Mathf.Clamp(stackAmount, 0, float.Parse(GS.GetSemiClass(Inventory[rs], "sq")));
                        GameObject DropItem = Instantiate(ItemPrefab) as GameObject;
                        DropItem.transform.position = this.transform.position + LookDir.forward * 0.5f;
                        DropItem.GetComponent<ItemScript>().Variables = What;
                        if(GS.ExistSemiClass(DropItem.GetComponent<ItemScript>().Variables, "sq")) 
                            DropItem.GetComponent<ItemScript>().Variables = GS.SetSemiClass(DropItem.GetComponent<ItemScript>().Variables, "sq", stackAmount.ToString());
                        if (Spec == -1) DropItem.GetComponent<ItemScript>().DroppedBy = this.gameObject;
                        else if (Spec == -2){
                            DropItem.transform.position = LookDir.position + LookDir.forward * 0.5f;
                            DropItem.GetComponent<ItemScript>().ThrownDirection = LookDir.forward;
                            DropItem.GetComponent<ItemScript>().State = 2;
                            DropItem.GetComponent<ItemScript>().DroppedBy = this.gameObject;
                            DropItem.GetComponent<ItemScript>().transform.forward = LookDir.forward;
                            ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Throw", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), ""), 0, 0f);
                        }
                        InvGet(rs.ToString(), 1, stackAmount);
                        break;
                    }
                } else {
                    int at = int.Parse(What);
                    //stackAmount = (int)Mathf.Clamp(stackAmount, 0, float.Parse(GS.GetSemiClass(Inventory[at], "sq")));
                    GameObject DropItem = Instantiate(ItemPrefab) as GameObject;
                    DropItem.transform.position = this.transform.position + LookDir.forward * 0.5f;
                    DropItem.GetComponent<ItemScript>().Variables = Inventory[at];
                    if(GS.ExistSemiClass(DropItem.GetComponent<ItemScript>().Variables, "sq")) 
                        DropItem.GetComponent<ItemScript>().Variables = GS.SetSemiClass(DropItem.GetComponent<ItemScript>().Variables, "sq", stackAmount.ToString());
                    DropItem.GetComponent<ItemScript>().DroppedBy = this.gameObject;
                    if (Spec == -1) DropItem.GetComponent<ItemScript>().DroppedBy = this.gameObject;
                    else if (Spec == -2){
                        DropItem.transform.position = LookDir.position + LookDir.forward * 0.5f;
                        DropItem.GetComponent<ItemScript>().ThrownDirection = LookDir.forward;
                        DropItem.GetComponent<ItemScript>().State = 2;
                        DropItem.GetComponent<ItemScript>().DroppedBy = this.gameObject;
                        DropItem.GetComponent<ItemScript>().transform.forward = LookDir.forward;
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Throw", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), ""), 0, 0f);
                    }
                    InvGet(at.ToString(), 1, stackAmount);
                }
                break;
        }

    }

    public void InventoryFunctions(string InventoryToSet){

        // Set Inventory from text
        if (InventoryToSet != "") {

            for (int SetInv = 0; SetInv <= 9; SetInv ++) {
                string[] LoadInv = GS.ListSemiClass(InventoryToSet, "/");
                for(int Check = 0; Check <= 9; Check++){
                    if(Check <= LoadInv.Length) Inventory[Check] = LoadInv[Check];
                    else InvGet(Check.ToString(), 1);
                }
            }

        } else {

            InventoryText = "";
            string AnimationAddition = "";

            for(int SetInv = 0; SetInv <= 9; SetInv ++){
                InventoryText += Inventory[SetInv] + "/";
            }

            // Set Inventory from text

            // BackPack Limitation
            CurrentItemHeld = (int)Mathf.Clamp(0f, CurrentItemHeld, 9f);
            // BackPack Limitation

            // SwitchItemSlots
            string SwitchItemSound = CurrentItemHeld.ToString();
            if (CantSwitchItem <= 0f) {

                int ChangeTo = -1;
                if (Input.GetKey(KeyCode.Alpha1) & MaxInventorySlots >= 1) {
                    ChangeTo = 0;
                } else if (Input.GetKey(KeyCode.Alpha2) & MaxInventorySlots >= 2) {
                    ChangeTo = 1;
                } else if (Input.GetKey(KeyCode.Alpha3) & MaxInventorySlots >= 3) {
                    ChangeTo = 2;
                } else if (Input.GetKey(KeyCode.Alpha4) & MaxInventorySlots >= 4) {
                    ChangeTo = 3;
                } else if (Input.GetKey(KeyCode.Alpha5) & MaxInventorySlots >= 5) {
                    ChangeTo = 4;
                } else if (Input.GetKey(KeyCode.Alpha6) & MaxInventorySlots >= 6) {
                    ChangeTo = 5;
                } else if (Input.GetKey(KeyCode.Alpha7) & MaxInventorySlots >= 7) {
                    ChangeTo = 6;
                } else if (Input.GetKey(KeyCode.Alpha8) & MaxInventorySlots >= 8) {
                    ChangeTo = 7;
                } else if (Input.GetKey(KeyCode.Alpha9) & MaxInventorySlots >= 9) {
                    ChangeTo = 8;
                } else if (Input.GetKey(KeyCode.Alpha0) & MaxInventorySlots >= 10) {
                    ChangeTo = 9;
                }

                if (Input.mouseScrollDelta.y > 0f && CurrentItemHeld + 1 < MaxInventorySlots) {
                    ChangeTo = CurrentItemHeld + 1;
                    CantSwitchItem = 0.05f;
                } else if (Input.mouseScrollDelta.y < 0f && CurrentItemHeld > 0) {
                    ChangeTo = CurrentItemHeld - 1;
                    CantSwitchItem = 0.05f;
                }

                if(ChangeTo != -1){
                    CurrentItemHeld = ChangeTo;
                    DropOrThrow = 0f;
                    SwitchItemSound = ChangeTo.ToString();
                    switch(int.Parse(GS.GetSemiClass(Inventory[ChangeTo], "id"))){
                        case 34: case 35: case 38: case 40: case 42: case 56: case 59: case 62: case 64: case 65: case 61: case 60: case 113: case 137: case 996: case 69: case 157: case 159: case 160:
                            CantUseItem = 1f;
                            break;
                        default:
                            CantUseItem = 0.5f;
                            break;
                    }
                    BulletsLoaded = 0;
                    UseDelay = new[]{0f, 0f};
                }

            }

            switch(int.Parse(GS.GetSemiClass(Inventory[int.Parse(SwitchItemSound, CultureInfo.InvariantCulture)], "id"), CultureInfo.InvariantCulture)){
                case 38: case 42: case 59: case 60: case 137: case 34: case 65: case 113: case 157: case 159: case 160:
                    if(GS.GetSemiClass(Inventory[int.Parse(SwitchItemSound)], "at") == "101") SwitchItemSound = "SwitchItem";
                    else SwitchItemSound = "SwitchItem-AR";
                    break;
                case 29: case 31: case 32: case 58: case 135:
                    if(GS.GetSemiClass(Inventory[int.Parse(SwitchItemSound)], "at") == "101") SwitchItemSound = "SwitchItem";
                    else SwitchItemSound = "SwitchItem-Pistol";
                    break;
                default:
                    SwitchItemSound = "SwitchItem";
                    break;
            }
            // SwitchItemSlots

            // Temp item show
            if(CantSwitchItem > 0f){
                TempItemShown[1] = CantSwitchItem;
            } else if (TempItemShown[1] > 0f){
                TempItemShown[1] -= 0.02f;
            } else {
                TempItemShown[1] = 0f;
                TempItemShown[0] = int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"));
            }
            // Temp item show

            if(UseDelay[0] < UseDelay[1])
                UseDelay[0] += 0.02f;

            // Drop/Pickup/Throw Item
            bool CanAttach = false;
            if (InteractedGameobject != null && InteractedGameobject.tag == "Item") {
                if (InteractedGameobject.GetComponent<ItemScript>() != null) {
                    if (InteractedGameobject.GetComponent<ItemScript>().CanHaveAttachments == true && ((GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "14" && GS.GetSemiClass(Inventory[CurrentItemHeld], "va") == "100") || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "100" || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "101" || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "102" || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "103" || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "104" || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "105")) {
                        CanAttach = true;
                    }
                }
            }

            if (InteractedGameobject != null && InteractedGameobject.GetComponent<Interactions>().Options[InteractedGameobject.GetComponent<Interactions>().ThisOption] == "PickUp" && GS.GetComponent<GameScript>().ReceiveButtonPress("Interaction", "Hold") > 0f && CantUseItem <= 0f && CantInteract <= 0f) {
                CantUseItem = Mathf.Clamp(CantUseItem, 0.5f, Mathf.Infinity);
                CantSwitchItem = CantUseItem;
                CantInteract = 0.5f;
                string newItemVars = InteractedGameobject.GetComponent<ItemScript>().Variables;
                bool MustSwitch = false;
                if(GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "0"){
                    ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("PickUpA", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                } else {
                    MustSwitch = true;
                    ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("PickUpB", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                    for(int cfq = 0; cfq < MaxInventorySlots; cfq++) {
                        if(GS.GetSemiClass(Inventory[cfq], "id") == "0" || GS.ExistSemiClass(newItemVars, "ch") || (GS.ExistSemiClass(newItemVars, "sq") && GS.GetSemiClass(Inventory[cfq], "id") == GS.GetSemiClass(newItemVars, "id"))){
                            MustSwitch = false;
                            break;
                        }
                    }
                }
                if(InteractedGameobject.GetComponent<ItemScript>().DroppedBy == null) RS.SetScore("ItemsFound_", "/+1");
                if(InteractedGameobject.GetComponent<ItemScript>().InWater) GS.PS.AchProg("Ach_UnderwaterTreasure", "/+-1");
                Destroy(InteractedGameobject.gameObject);
                PlaySoundBank("PickupItem", 1, Random.Range(0.75f, 1f), 0f, "Only");

                if(MustSwitch){
                    if(GS.ExistSemiClass(Inventory[CurrentItemHeld], "sq")) InvGet(CurrentItemHeld.ToString(), 2, int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "sq")) );
                    else InvGet(CurrentItemHeld.ToString(), 2);
                    InvGet(newItemVars, 0);
                    GS.Mess(GS.SetString("Inventory is full; switching items", "Inwentarz jest pełen; zamieniono przedmioty"), "MidError");
                } else {
                    InvGet(newItemVars, 0);
                }

            } else if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CanAttach == true && CantUseItem <= 0f) {
                bool Compatible = false;
                foreach (Transform CheckC in InteractedGameobject.GetComponent<ItemScript>().SelectedMesh.transform.GetChild(0)) {
                    if (CheckC.name == GS.GetSemiClass(Inventory[CurrentItemHeld], "id")) {
                        Compatible = true;
                    }
                }
                if (Compatible == true) {
                    string prevAt = GS.GetSemiClass(InteractedGameobject.GetComponent<ItemScript>().Variables, "at");
                    if (prevAt != "" && prevAt != "0") {
                        prevAt = GS.itemCache[int.Parse(prevAt)].startVariables;
                    } else prevAt = "";
                    InteractedGameobject.GetComponent<ItemScript>().Variables = GS.SetSemiClass(InteractedGameobject.GetComponent<ItemScript>().Variables, "at", GS.GetSemiClass(Inventory[CurrentItemHeld], "id")); //InteractedGameobject.GetComponent<ItemScript>().Variables.z = GS.GetSemiClass(Inventory[CurrentItemHeld], "id");
                    InteractedGameobject.GetComponent<ItemScript>().setAtt();
                    GS.Mess(GS.SetString(GS.itemCache[int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"))].getName() + " attached to weapon", "Dodano " + GS.itemCache[int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"))].getName() + " do broni"));
                    InvGet(CurrentItemHeld.ToString(), 1);
                    if(prevAt != "")InvGet(prevAt, 0);
                } else {
                    CantUseItem = 0.5f;
                    CantSwitchItem = CantUseItem;
                    GS.Mess(GS.SetString("That attachment is not compatible with this!", "Ten dodatek nie jest kompatybilny!"), "Error");
                }
            }

            if (GS.ReceiveButtonPress("DropItem", "Hold") > 0f && DropOrThrow < 0.5f && CantUseItem <= 0f && GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "0") {
                DropButtonHeld = true;
                DropOrThrow = Mathf.Clamp(DropOrThrow += 0.02f, 0f, 3f);
                //CantSwitchItem = Mathf.Clamp(CantSwitchItem, 0.1f, Mathf.Infinity);
            } else {
                DropButtonHeld = false;
            }

            if (DropOrThrow > 0f && GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "0" && DropButtonHeld == false) {
                if (DropOrThrow < 0.5f) {
                    // Drop
                    InvGet(CurrentItemHeld.ToString(), -1);
                    CantSwitchItem = 0f;
                } else {
                    // Throw
                    InvGet(CurrentItemHeld.ToString(), -2);
                    CantUseItem = Mathf.Clamp(CantUseItem, 0.5f, Mathf.Infinity);
                    CantSwitchItem = CantUseItem;
                }
                DropOrThrow = 0f;
            }
            // Drop/Pickup/Throw Item

            // Set Item Models
            bool HasEquipped = false;
            if(GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == TempItemShown[0].ToString()) HasEquipped = true;
            foreach (Transform GetItem in ItemsShown.transform.GetChild(0)) {
                if (GetItem.gameObject.name == ((int)TempItemShown[0]).ToString()) {
                    GetItem.gameObject.SetActive(true);
                    if(HasEquipped)
                    foreach (Transform GetChild in GetItem.transform) {
                        if (GetChild.name == "SlimEnd") {
                            SlimEnd = GetChild.gameObject;
                        } else if (GetChild.name == "AimPart") {
                            Vector3 SetAP = GetChild.transform.localPosition;
                            SetAP.z = -ItemsShown.transform.GetChild(0).localPosition.z;
                            AimPart = (ItemsShown.transform.GetChild(0).localPosition / -4f) + (SetAP / -4f);
                        } else if (GetChild.name == "BulletChamber") {
                            BulletChamber = GetChild.gameObject;
                        } else if (GetChild.name == "Scanner") {
                            Scanner = GetChild.gameObject;
                        } else if (GetChild.name == "Attachment") {
                            foreach (Transform GetAttachment in GetChild.transform) {
                                if (GetAttachment.name == GS.GetSemiClass(Inventory[CurrentItemHeld], "at")) {
                                    GetAttachment.gameObject.SetActive(true);
                                    if(GetAttachment.name == "101"){
                                        AnimationAddition = "Grip";
                                    } else if (GetAttachment.name == "103") {
                                        Vector3 SetAP = GetAttachment.transform.parent.localPosition + (GetAttachment.transform.localPosition + new Vector3(0f, 1f, 8f));
                                        SetAP.z = -ItemsShown.transform.GetChild(0).localPosition.z;
                                        AimPart = (ItemsShown.transform.GetChild(0).localPosition / -4f) + (SetAP / -4f);
                                    } else if (GetAttachment.name == "104") {
                                        Vector3 SetAP = GetAttachment.transform.parent.localPosition + (GetAttachment.transform.localPosition + new Vector3(0f, 0.02f, 8f));
                                        SetAP.z = -ItemsShown.transform.GetChild(0).localPosition.z;
                                        AimPart = (ItemsShown.transform.GetChild(0).localPosition / -4f) + (SetAP / -4f);
                                        HoloSight = GetAttachment.position;
                                    }
                                } else {
                                    GetAttachment.gameObject.SetActive(false);
                                }
                            }
                        } else if (GetChild.name == "Glowstick") {
                            GetChild.GetComponent<MeshRenderer>().materials[1].color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                            GetChild.GetComponent<Light>().color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                        } else if (GetChild.name == "Flare") {
                            GetChild.GetComponent<MeshRenderer>().materials[0].color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                            if (GetChild.GetComponent<Light>() != null) {
                                GetChild.GetComponent<Light>().color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                                ParticleSystem.MainModule SetCol = GetChild.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().main;
                                SetCol.startColor = Color.HSVToRGB(float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                            }
                        } else if (GetChild.name == "CrankFlashlight"){
                            if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) > 80f) {
                                GetChild.transform.GetChild(0).GetComponent<Light>().intensity = Mathf.Lerp(0.5f, 2f, (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) - 80f) / 20f);
                            } else {
                                GetChild.transform.GetChild(0).GetComponent<Light>().intensity = Mathf.Lerp(0f, 0.5f, float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) / 80f);
                            }
                        } else if (GetChild.GetComponent<MeshRenderer>() != null) {
                            foreach (Material GetLaser in GetChild.GetComponent<MeshRenderer>().materials) {
                                if (GetLaser.name == "LASER (Instance)") {
                                    GetLaser.color = LaserColor;
                                }
                            }
                        } else if (GetChild.name == "S_Minigun") {
                            if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "64") {
                                GetChild.GetComponent<AudioSource>().pitch = 1f - (ZoomValues[1] - ZoomValues[0]) / (ZoomValues[2] - ZoomValues[0]);
                            } else {
                                GetChild.GetComponent<AudioSource>().pitch = 0f;
                            }
                        } else if (GetChild.name == "Minigun") {
                            GetChild.GetChild(0).Rotate(new Vector3((1f - (ZoomValues[1] - ZoomValues[0]) / (ZoomValues[2] - ZoomValues[0])) * 10f, 0f, 0f));
                        } else if (GetChild.name == "Laser") {
                            ParticleSystem.MainModule SetCol = GetChild.GetComponent<ParticleSystem>().main;
                            SetCol.startColor = new Color(LaserColor.r, LaserColor.g, LaserColor.b, 0.1f);
                        } else if (GetChild.name == "FishingRod"){
                            SlimEnd = GetChild.GetChild(0).gameObject;
                        } else if (GetChild.name == "BaitPos" && IsFishing == false) {
                            FishingRodBait.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            FishingRodBait.transform.position = Vector3.Lerp(FishingRodBait.transform.position, GetChild.position, 0.5f);
                        } else if (GetChild.childCount > 0){
                            foreach(Transform SetChild in GetChild){
                                if (SetChild.GetComponent<MeshRenderer>() != null) {
                                    foreach (Material GetLaser in SetChild.GetComponent<MeshRenderer>().materials) {
                                        if (GetLaser.name == "LASER (Instance)") {
                                            GetLaser.color = LaserColor;
                                        }
                                    }
                                }
                            }
                        }
                    }
                } else {
                    GetItem.gameObject.SetActive(false);
                }
            }
            // Set Item Models

            // Play Switch animations
            if(GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "101") AnimationAddition = "Grip";
            if (TempItemShown[1] <= 0f && (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != previousItem[0].ToString() || CurrentItemHeld != previousItem[1] || ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlaySwitchItem"))) {
                if(GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "0") PlaySoundBank(SwitchItemSound, 1, Random.Range(0.75f, 1f), 0f, "Only");
                else PlaySoundBank("", 1, Random.Range(0.75f, 1f), 0f, "Only");
                ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("SwitchItem", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                TempItemShown = new float[]{ int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), CultureInfo.InvariantCulture), 0f };
                CantUseItem = 0.5f;
                previousItem[0] = int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"));
                previousItem[1] = CurrentItemHeld;
            } else if (/*TempItemShown[1] <= 0f &&*/ ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayPullUp")) {
                ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Pullup", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                TempItemShown = new float[]{ int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), CultureInfo.InvariantCulture), 0f };
                previousItem[0] = int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"));
                previousItem[1] = CurrentItemHeld;
            }
            // Play switch animations

            // Specifics for items
            if(State != 0)
            for (int CheckInventory = 0; CheckInventory <= 9; CheckInventory++) {
                if (CheckInventory >= MaxInventorySlots && Inventory[CheckInventory] != "id0;") {
                    GameObject DropItem = Instantiate(ItemPrefab) as GameObject;
                    DropItem.transform.position = this.transform.position + this.transform.forward * 1f;
                    DropItem.GetComponent<ItemScript>().Variables = Inventory[CheckInventory];
                    DropItem.GetComponent<ItemScript>().DroppedBy = this.gameObject;
                    InvGet(CheckInventory.ToString(), 1);
                }
                if (GS.GetSemiClass(Inventory[CheckInventory], "id") == "13" && CurrentItemHeld != CheckInventory) {
                    GameObject DropItem = Instantiate(ItemPrefab) as GameObject;
                    DropItem.transform.position = this.transform.position;
                    DropItem.GetComponent<ItemScript>().Variables = Inventory[CheckInventory];
                    InvGet(CheckInventory.ToString(), 1);
                    GS.Mess(GS.SetString("Flare must be held in hand!", "Flara musi być trzymana w ręce!"), "Error");
                } else if (GS.GetSemiClass(Inventory[CheckInventory], "id") == "997" && float.Parse(GS.GetSemiClass(Inventory[CheckInventory], "va"), CultureInfo.InvariantCulture) < 100f) {
                    Inventory[CheckInventory] = GS.SetSemiClass(Inventory[CheckInventory], "va", "/+0.005"); //Inventory[CheckInventory].y += 0.005f;
                } else if ((GS.GetSemiClass(Inventory[CheckInventory], "id") == "66" || GS.GetSemiClass(Inventory[CheckInventory], "id") == "110" || GS.GetSemiClass(Inventory[CheckInventory], "id") == "131") && float.Parse(GS.GetSemiClass(Inventory[CheckInventory], "va"), CultureInfo.InvariantCulture) > 0f) {
                    Inventory[CheckInventory] = GS.SetSemiClass(Inventory[CheckInventory], "va", "/+0.4");//Inventory[CheckInventory].y += 0.4f;
                    if (float.Parse(GS.GetSemiClass(Inventory[CheckInventory], "va"), CultureInfo.InvariantCulture) > 100f) {
                        GameObject Boom = Instantiate(SpecialPrefab) as GameObject;
                        Boom.transform.position = this.transform.position;
                        if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "131") {
                            Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Flashbang";
                            Boom.GetComponent<SpecialScript>().ExplosionRange = 20f;
                        } else {
                            Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                            Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                        }
                        InvGet(CheckInventory.ToString(), 1);
                        if (GS.GetSemiClass(Inventory[CheckInventory], "id") == "110") {
                            Boom.GetComponent<SpecialScript>().ExplosionRange = 4f;
                            for (int shootFrag = Random.Range(10, 15); shootFrag > 0; shootFrag--) {
                                //GameObject Shootfrag = Instantiate(AttackPrefab) as GameObject;
                                //Shootfrag.transform.position = this.transform.position + Vector3.up * 0.25f;
                                //Shootfrag.transform.Rotate(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f));
                                //Shootfrag.GetComponent<AttackScript>().GunName = "MosinNagant";
                                //Shootfrag.GetComponent<AttackScript>().Attacker = null;
                                //Shootfrag.GetComponent<AttackScript>().Slimend = this.gameObject;

                                RS.Attack(new string[]{"MosinNagant"}, this.transform.position + Vector3.up * 0.25f, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
                            }
                        }
                    }
                }
            }

            if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "96" || GS.ReceiveButtonPress("Action", "Hold") <= 0f) {
                InBox = false;
            }
            // Specifics for held items
            currBuild = "";
            int currID = int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"));
            switch(currID) {
                case 1: case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10: case 17: case 18: case 19: case 20: case 21: case 22: case 23: case 24: case 25: case 26: case 70: case 71: case 72: case 73: case 74: case 75: case 76: case 77: case 78: case 79: case 80: case 81: case 82: case 83: case 84: case 106: case 116: case 117: case 118: case 119: case 120: case 121: case 122: case 123: case 161:
                    // Get Food info
                    int DrinkOrWhat = 0; // 0 Eat   1 Drink   2 Other
                    float HungerToAddSub = 0f;
                    float HealthToAddSub = 0f;
                    float HydrationToAdd = 0f;
                    float TirednessToAdd = 0f;
                    float InfectionToAdd = 0f;
                    float RadioactivityToAdd = 0f;
                    float DrunkToAdd = 0f;
                    float StaminaToAdd = 0f;
                    float ColdnessToAdd = 0f;
                    Color32 FoodColor = new Color32(0, 0, 0, 0);
                    Color32 FlashColor = new Color32(0, 0, 0, 0);
                    string FoodName = "";
                    string ConsumeAnimation = "Eat";
                    bool CanUse = true;
                    switch(currID) {
                        case 1:
                            FoodName = GS.SetString("Apple", "Jabłko");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 30f;
                            FoodColor = new Color32(200, 0, 0, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 3:
                            FoodName = GS.SetString("Bread", "Chleb");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 120f;
                            FoodColor = new Color32(234, 203, 174, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 4:
                            FoodName = GS.SetString("Soup", "Zupe");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 0;
                            HungerToAddSub = 240f;
                            FoodColor = new Color32(255, 155, 0, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 5:
                            FoodName = GS.SetString("Mackerel", "Makrelę");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(200, 225, 255, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 6:
                            FoodName = GS.SetString("Chocolate", "Czekoladę");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(155, 92, 85, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 7:
                            FoodName = GS.SetString("Sausage", "Kiełbasę");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 180f;
                            FoodColor = new Color32(140, 46, 66, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 8:
                            FoodName = GS.SetString("Jam", "Dżem");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 0;
                            HungerToAddSub = 120f;
                            FoodColor = new Color32(158, 0, 17, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 9:
                            FoodName = GS.SetString("Chips", "Czipsy");
                            ConsumeAnimation = "Chips";
                            DrinkOrWhat = 0;
                            HungerToAddSub = 120f;
                            FoodColor = new Color32(232, 214, 65, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 10:
                            FoodName = GS.SetString("Cheese", "Ser");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 120f;
                            FoodColor = new Color32(242, 222, 130, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 17:
                            FoodName = GS.SetString("Water", "Wodę");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 1;
                            HydrationToAdd = 60f;
                            FoodColor = new Color32(0, 155, 255, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 18:
                            FoodName = GS.SetString("Energy Drink", "Energetyka");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 1;
                            HydrationToAdd = 30f;
                            TirednessToAdd = -25f;
                            FoodColor = new Color32(200, 255, 0, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 19:
                            FoodName = GS.SetString("Candy Bar", "Batonik");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(255, 75, 0, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 20:
                            FoodName = GS.SetString("Beans", "Fasolki");
                            ConsumeAnimation = "Chips";
                            DrinkOrWhat = 0;
                            HungerToAddSub = 240f;
                            FoodColor = new Color32(255, 155, 0, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 21:
                            FoodName = GS.SetString("MRE", "MRE");
                            ConsumeAnimation = "Chips";
                            DrinkOrWhat = 0;
                            HungerToAddSub = Mathf.Max(Food[1] - Food[0], 50f);
                            FoodColor = new Color32(200, 225, 255, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 22:
                            FoodName = GS.SetString("Bandage", "Bandażu");
                            ConsumeAnimation = "Bandage";
                            DrinkOrWhat = 2;
                            HealthToAddSub = Mathf.Clamp(25, 0f, (Health[1] * 0.75f) - Health[0]);
                            FoodColor = new Color32(200, 225, 255, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            if (Health[0] >= Health[1] * 0.75f)
                                CanUse = false;
                            break;
                        case 23:
                            FoodName = GS.SetString("Antibiotics", "Antybiotyki");
                            ConsumeAnimation = "Chips";
                            DrinkOrWhat = 1;
                            InfectionToAdd = -25f;
                            RadioactivityToAdd = -25f;
                            FoodColor = new Color32(117, 158, 130, 255);
                            FlashColor = new Color32(0, 255, 0, 155);
                            if (Infection <= 0f && Radioactivity <= 0f)
                                CanUse = false;
                            break;
                        case 24:
                            FoodName = GS.SetString("Vaccine", "Szczepionkę");
                            DrinkOrWhat = 2;
                            InfectionToAdd = -Infection;
                            FoodColor = new Color32(117, 158, 130, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            if (Infection <= 0f)
                                CanUse = false;
                            break;
                        case 25:
                            FoodName = GS.SetString("Lugol's Solution", "Płyn Lugola");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 1;
                            RadioactivityToAdd = -Radioactivity - 30f;
                            FoodColor = new Color32(181, 97, 124, 255);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 26:
                            FoodName = GS.SetString("First Aid Kit", "Apteczka Pierwszej Pomocy");
                            ConsumeAnimation = "Bandage";
                            DrinkOrWhat = 2;
                            HealthToAddSub = 50f;
                            FoodColor = new Color32(181, 97, 124, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 70:
                            FoodName = GS.SetString("Baguette", "Bagietę");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 120f;
                            FoodColor = new Color32(204, 148, 109, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 71:
                            FoodName = GS.SetString("Pickles", "Ogórki Kiszone");
                            ConsumeAnimation = "Chips";
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(102, 127, 58, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 72:
                            FoodName = GS.SetString("Meat", "Mięso");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 180f;
                            FoodColor = new Color32(166, 116, 81, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 73:
                            FoodName = GS.SetString("Preztel", "Precla");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(204, 148, 109, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 74:
                            FoodName = GS.SetString("Cheeseburger", "Chesseburgera");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 120f;
                            FoodColor = new Color32(255, 155, 0, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 75:
                            FoodName = GS.SetString("Waffle", "Gofra");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(226, 193, 127, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 76:
                            FoodName = GS.SetString("Donut", "Donuta");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(249, 147, 228, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 77:
                            FoodName = GS.SetString("Pâté", "Pasztet");
                            ConsumeAnimation = "Chips";
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(234, 203, 174, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 78:
                            FoodName = GS.SetString("Crackers", "Krakersy");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(226, 192, 133, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 79:
                            FoodName = GS.SetString("Cola", "Kolę");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 1;
                            HydrationToAdd = 30f;
                            TirednessToAdd = -10f;
                            FoodColor = new Color32(0, 0, 0, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 80:
                            FoodName = GS.SetString("Beer", "Piwo");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 1;
                            DrunkToAdd = 25f;
                            FoodColor = new Color32(255, 75, 0, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 81:
                            FoodName = GS.SetString("Vodka", "Wódkę");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 1;
                            DrunkToAdd = 45f;
                            FoodColor = new Color32(255, 75, 0, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 82:
                            FoodName = GS.SetString("Potato", "Ziemniaka");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 30f;
                            FoodColor = new Color32(234, 203, 174, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 83:
                            FoodName = GS.SetString("Milk", "Mleko");
                            ConsumeAnimation = "Drink";
                            DrinkOrWhat = 1;
                            HydrationToAdd = 60f;
                            FoodColor = new Color32(255, 255, 255, 255);
                            FlashColor = new Color32(128, 255, 255, 155);
                            break;
                        case 84:
                            FoodName = GS.SetString("Biscuits", "Herbatniki");
                            ConsumeAnimation = "Chips";
                            DrinkOrWhat = 0;
                            HungerToAddSub = 60f;
                            FoodColor = new Color32(226, 211, 139, 255);
                            FlashColor = new Color32(128, 255, 0, 155);
                            break;
                        case 106:
                            FoodName = GS.SetString("Med Kit", "Apteczka");
                            ConsumeAnimation = "Bandage";
                            DrinkOrWhat = 2;
                            HealthToAddSub = 75f;
                            Infection = Mathf.Clamp(Random.Range(0f, 50f), 0f, Infection);
                            Radioactivity = Mathf.Clamp(Random.Range(0f, 50f), 0f, Radioactivity);
                            FoodColor = new Color32(181, 97, 124, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 116:
                            FoodName = GS.SetString("Herring", "Śledzia");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 30f;
                            FoodColor = new Color32(180, 190, 200, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 117:
                            FoodName = GS.SetString("Salmon", "Łosośa");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 120f;
                            FoodColor = new Color32(216, 224, 234, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 118:
                            FoodName = GS.SetString("Carp", "Karpia");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 300f;
                            FoodColor = new Color32(172, 188, 120, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 119:
                            FoodName = GS.SetString("Coconut", "Kokosa");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 240f;
                            HydrationToAdd = 30f;
                            FoodColor = new Color32(172, 188, 120, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 120:
                            FoodName = GS.SetString("Banana", "Banana");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 30f;
                            FoodColor = new Color32(255, 225, 100, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 121:
                            FoodName = GS.SetString("Sandwich", "Kanapkę");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 240f;
                            FoodColor = new Color32(255, 241, 202, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                        case 122:
                            FoodName = GS.SetString("Coffee", "Kawa");
                            DrinkOrWhat = 1;
                            HydrationToAdd = 30f;
                            FoodColor = new Color32(255, 225, 100, 0);
                            FlashColor = new Color32(128, 255, 255, 155);
                            StaminaToAdd = Energy[1] - Energy[0];
                            break;
                        case 123:
                            FoodName = GS.SetString("Popsicle", "Loda na patyku");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 240f;
                            FoodColor = new Color32(255, 241, 202, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            ColdnessToAdd = 25f;
                            break;
                        case 161:
                            FoodName = GS.SetString("Fish fillet", "Filet z ryby");
                            DrinkOrWhat = 0;
                            HungerToAddSub = 100f;
                            FoodColor = new Color32(204, 100, 81, 0);
                            FlashColor = new Color32(0, 255, 0, 155);
                            break;
                    }
                    // Get Food info
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && CanUse == true) {
                        CantUseItem = 1f;
                        CantSwitchItem = CantUseItem;

                        if(IsCasual){
                            HealthToAddSub += HungerToAddSub / 10f;
                            HungerToAddSub = 0f;
                        }

                        Food[0] += HungerToAddSub;
                        Health[0] += HealthToAddSub;
                        Hydration += HydrationToAdd;
                        Tiredness += TirednessToAdd;
                        Infection += InfectionToAdd;
                        Radioactivity += RadioactivityToAdd;
                        Drunkenness += DrunkToAdd;
                        Energy[0] += StaminaToAdd;
                        Coldness += ColdnessToAdd;

                        if (currID == 22) {
                            Bleeding = 0f;
                        } else if (currID == 83) {
                            int UnbrokeBone = Random.Range(0, 4);
                            if (UnbrokeBone == 0) {
                                BrokenBone = 0;
                            }
                        } else if (currID == 106) {
                            int UnbrokeBone = Random.Range(0, 2);
                            if (UnbrokeBone == 0) {
                                BrokenBone = 0;
                            }
                            Coldness -= Mathf.Clamp(Random.Range(0f, 50f), 0f, Coldness);
                            Hot -= Mathf.Clamp(Random.Range(0f, 50f), 0f, Hot);
                        }

                        string MessageAboutIt = "";
                        if (DrinkOrWhat == 0)
                            MessageAboutIt += GS.SetString(FoodName + " consumed:", "Spożyto " + FoodName + ":");
                        else if (DrinkOrWhat == 1)
                            MessageAboutIt += GS.SetString(FoodName + " drank:", "Wypito " + FoodName + ":");
                        else if (DrinkOrWhat == 2)
                            MessageAboutIt += GS.SetString(FoodName + " used:", "Użyto " + FoodName + ":");

                        if (HungerToAddSub != 0)
                            MessageAboutIt += GS.SetString(" (Hunger " + (int)HungerToAddSub + ")", " (Głód " + (int)HungerToAddSub + ")");
                        if (HealthToAddSub != 0)
                            MessageAboutIt += GS.SetString(" (Health " + (int)HealthToAddSub + ")", " (Zdrowie " + (int)HealthToAddSub + ")");
                        if (HydrationToAdd != 0)
                            MessageAboutIt += GS.SetString(" (Hydration " + (int)HydrationToAdd + ")", " (Nawodnienie " + (int)Hydration + ")");
                        if (TirednessToAdd != 0)
                            MessageAboutIt += GS.SetString(" (Tiredness " + (int)TirednessToAdd + ")", " (Zmęczenie " + (int)Hydration + ")");
                        if (InfectionToAdd != 0)
                            MessageAboutIt += GS.SetString(" (Infection " + (int)InfectionToAdd + ")", " (Infekcja " + (int)InfectionToAdd + ")");
                        if (RadioactivityToAdd != 0)
                            MessageAboutIt += GS.SetString(" (Radioactivity " + (int)RadioactivityToAdd + ")", " (Radioaktywność " + (int)RadioactivityToAdd + ")");
                        if (DrunkToAdd != 0)
                            MessageAboutIt += GS.SetString(" (Drunkenness " + (int)DrunkToAdd + ")", " (Pijaństwo " + (int)DrunkToAdd + ")");
                        if (ColdnessToAdd != 0)
                            MessageAboutIt += GS.SetString(" (Coldness " + (int)ColdnessToAdd + ")", " (Zimno " + (int)ColdnessToAdd + ")");
                        if (StaminaToAdd != 0) 
                            MessageAboutIt += GS.SetString(" (Stamina " + (int)StaminaToAdd + ")", " (Wytrzymałość " + (int)StaminaToAdd + ")");
                        MainCanvas.Flash(FlashColor, new float[]{0.5f, 0.5f});
                        GS.Mess(MessageAboutIt, "Good");

                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim(ConsumeAnimation, GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition));
                        if (DrinkOrWhat == 0 || DrinkOrWhat == 1) {
                            GameObject Crumbs = Instantiate(EffectPrefab) as GameObject;
                            Crumbs.transform.position = LookDir.position - LookDir.up * 0.25f;
                            Crumbs.transform.rotation = LookDir.rotation;
                            if (DrinkOrWhat == 0) {
                                Crumbs.GetComponent<EffectScript>().EffectName = "Eating";
                            } else if (DrinkOrWhat == 1) {
                                Crumbs.GetComponent<EffectScript>().EffectName = "Drinking";
                            }
                            Crumbs.GetComponent<EffectScript>().EffectColor = FoodColor;
                        } else if (DrinkOrWhat == 2) {
                            GameObject PatchUp = Instantiate(EffectPrefab) as GameObject;
                            PatchUp.transform.position = LookDir.position - LookDir.up * 0.25f;
                            PatchUp.transform.rotation = LookDir.rotation;
                            PatchUp.GetComponent<EffectScript>().EffectColor = FoodColor;
                            PatchUp.GetComponent<EffectScript>().EffectName = "PatchUp";
                        }
                            
                        InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                    }
                    break;
                case 12:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        GameObject Matches = Instantiate(EffectPrefab) as GameObject;
                        Matches.transform.position = this.transform.position;
                        Matches.GetComponent<EffectScript>().EffectName = "Matches";
                        Inventory[CurrentItemHeld] = "id13;va30;cl" + GS.GetSemiClass(Inventory[CurrentItemHeld], "cl");//new Vector3(13f, 30f, Inventory[CurrentItemHeld].z);
                    }
                    break;
                case 13:
                    Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-0.02"); //Inventory[CurrentItemHeld].y -= 0.02f;
                    Coldness -= 0.02f;
                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                        GS.Mess(GS.SetString("Flare burnt out!", "Flara się wypaliła!"), "Error");
                    } else if (IsSwimming == true) {
                        InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                        GS.Mess(GS.SetString("Flare got extinguished!", "Flara się zgasiła!"), "Error");
                    }
                    break;
                case 2: case 14: case 15: case 16: case 27: case 28: case 993: case 68: case 108: case 115: case 132: case 134: case 136: case 138: case 152: case 153: case 154: case 155: case 156:

                    // Get Melee info
                    string AttackType = "";
                    float AttackCooldown = 1f;
                    float ParryCooldown = 1f;
                    float EnergyDrain = 0f;
                    float ParryDrain = -1f;
                    float UseDamage = 0f;
                    switch (currID) {
                        case 2:
                            UseDamage = 5f;
                            AttackType = "Flashlight";
                            AttackCooldown = 0.5f;
                            EnergyDrain = 5f;
                            break;
                        case 14:
                            UseDamage = 5f;
                            AttackType = "Knife";
                            AttackCooldown = 0.5f;
                            EnergyDrain = 5f;
                            break;
                        case 15:
                            UseDamage = 3f;
                            AttackType = "Crowbar";
                            AttackCooldown = 0.75f;
                            EnergyDrain = 25f;
                            ParryDrain = 5f;
                            break;
                        case 16:
                            UseDamage = 3f;
                            AttackType = "FireAxe";
                            AttackCooldown = 0.75f;
                            EnergyDrain = 25f;
                            ParryDrain = 5f;
                            break;
                        case 27:
                            UseDamage = 3f;
                            AttackType = "Machete";
                            AttackCooldown = 0.5f;
                            EnergyDrain = 5f;
                            ParryDrain = 15f;
                            break;
                        case 28:
                            UseDamage = 5f;
                            AttackType = "BaseballBat";
                            AttackCooldown = 0.75f;
                            EnergyDrain = 10f;
                            ParryDrain = 15f;
                            break;
                        case 993:
                            UseDamage = 10f;
                            AttackType = "SapphireSpear";
                            AttackCooldown = 2f;
                            EnergyDrain = 50f;
                            ParryDrain = 0f;
                            break;
                        case 68:
                            UseDamage = 0f;
                            AttackType = "Chainsaw";
                            AttackCooldown = 0.05f;
                            EnergyDrain = 0f;
                            break;
                        case 108:
                            UseDamage = 5f;
                            AttackType = "Plunger";
                            AttackCooldown = 0.5f;
                            EnergyDrain = 5f;
                            ParryDrain = 15f;
                            break;
                        case 115:
                            UseDamage = 5f;
                            AttackType = "Shovel";
                            AttackCooldown = 0.75f;
                            EnergyDrain = 25f;
                            ParryDrain = 10f;
                            break;
                        case 132:
                            UseDamage = 5f;
                            AttackType = "Katana";
                            AttackCooldown = 0.25f;
                            EnergyDrain = 5f;
                            ParryDrain = 25f;
                            break;
                        case 134:
                            UseDamage = 3f;
                            AttackType = "Spear";
                            AttackCooldown = 1f;
                            EnergyDrain = 10f;
                            ParryDrain = 15f;
                            break;
                        case 136:
                            UseDamage = 5f;
                            AttackType = "FryingPan";
                            AttackCooldown = 0.5f;
                            EnergyDrain = 5f;
                            break;
                        case 138:
                            UseDamage = 1f;
                            AttackType = "Sledgehammer";
                            AttackCooldown = 2f;
                            EnergyDrain = 50f;
                            ParryDrain = 2f;
                            break;
                        case 152:
                            UseDamage = 3f;
                            AttackType = "FireAxe";
                            AttackCooldown = 0.75f;
                            EnergyDrain = 25f;
                            ParryDrain = 5f;
                            break;
                        case 153:
                            UseDamage = 5f;
                            AttackType = "StoneAxe";
                            AttackCooldown = 0.75f;
                            EnergyDrain = 25f;
                            ParryDrain = 5f;
                            break;
                        case 154:
                            UseDamage = 3f;
                            AttackType = "Fokos";
                            AttackCooldown = 0.5f;
                            EnergyDrain = 10f;
                            ParryDrain = 0f;
                            ParryCooldown = .5f;
                            break;
                        case 155:
                            UseDamage = 3f;
                            AttackType = "Sword";
                            AttackCooldown = 2f;
                            EnergyDrain = 25f;
                            ParryDrain = 25f;
                            break;
                        case 156:
                            UseDamage = 2f;
                            AttackType = "Pickaxe";
                            AttackCooldown = 0.5f;
                            EnergyDrain = 10f;
                            ParryDrain = 10f;
                            break;
                    }

                    // Get Melee info
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && CheckStamina(EnergyDrain, 0)) {
                        CantUseItem = AttackCooldown;
                        CantSwitchItem = CantUseItem;
                        StaminaDrain(EnergyDrain);
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Swing", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                        RS.Attack(new string[]{ AttackType, "MeleeDurability" + UseDamage, "Power" + (Drunkenness / 10f), "Inventory" + CurrentItemHeld }, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                    }

                   if (GS.ReceiveButtonPress("AltAction", "Hold") > 0f && CantUseItem <= 0f && CheckStamina(ParryDrain, 0) && ParryDrain> -1f) {
                        CantUseItem = ParryCooldown;
                        CantSwitchItem = ParryCooldown;
                        StaminaDrain(ParryDrain);
                        ParryingDamage = UseDamage;
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Parry", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition));
                        GameObject Swoosh = Instantiate(EffectPrefab) as GameObject;
                        Swoosh.transform.position = SlimEnd.transform.position;
                        Swoosh.GetComponent<EffectScript>().EffectName = "Parry";
                   }

                    if (currID == 2) {
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-0.0066"); //Inventory[CurrentItemHeld].y -= 0.032f;
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                            InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                            GS.Mess(GS.SetString("Flashlight ran out of battery!", "Latarce wyczerpała się bateria!"), "Error");
                        }
                    } else if (currID == 68) {
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-0.032"); //Inventory[CurrentItemHeld].y -= 0.032f;
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                            InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                            GS.Mess(GS.SetString("Chainsaw ran out of fuel!", "Pile łańcuchowej wyczerpało się paliwo!"), "Error");
                        }
                    } else if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                        GS.Mess(GS.SetString("Your weapon broke!", "Broń się popsuła!"), "ItemBroke");
                    }

                    if (Energy[0] < EnergyDrain) {
                        MainCanvas.CSAlert = new float[] { 1f, 1f };
                    }

                    break;
                case 29: case 31: case 32: case 34: case 35: case 36: case 38: case 40: case 41: case 42: case 55: case 56: case 57: case 58: case 59: case 60: case 61: case 62: case 64: case 65: case 996: case 113: case 135: case 137: case 157: case 159: case 160:
                    // Get ranged info
                    string GunType = "";
                    string[] FireAnimation = new string[]{"", ""};
                    float[] GunCooldown = {1f, 1f};
                    float AmmoInUse = 1f;
                    float AimZoom = 0f;
                    string[] ReloadingAnimation = new string[] { "ItemReload", "Reloading" };
                    string[] AimingAnimation = new[]{"", ""};
                    float[] ReloadVariables = new float[] { 0f, 0f, 0f };
                    int AmountToShoot = 1;
                    int AmountOfGunFires = 1;
                    int BurstFire = 1;
                    float[] RecoilPhysics = new float[]{1f, 1f};
                    float DelayFire = 0f;
                    string[] DelayFireEffects = new string [0];
                    switch (currID) {
                        case 29:
                            GunType = "Colt";
                            FireAnimation = new string[]{"Pistol-Shoot", ""};
                            AimingAnimation = new string[]{"Pistol-Aim", "Pistol-AimShoot"};
                            GunCooldown[0] = 0.25f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Pistol-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 7f, 30f, 2f };
                            break;
                        case 31:
                            GunType = "Luger";
                            FireAnimation = new string[]{"Pistol-Shoot", ""};
                            AimingAnimation = new string[]{"Pistol-Aim", "Pistol-AimShoot"};
                            GunCooldown[0] = 0.25f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Pistol-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 8f, 30f, 2f };
                            break;
                        case 32:
                            GunType = "Revolver";
                            FireAnimation = new string[]{"Pistol-Shoot", ""};
                            AimingAnimation = new string[]{"Pistol-Aim", "Pistol-AimShoot"};
                            GunCooldown[0] = 1f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Revolver-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 6f, 33f, 2f };
                            RecoilPhysics = new float[]{0f, 100f};
                            break;
                        case 34:
                            GunType = "HunterRifle";
                            FireAnimation = new string[]{"BoltAction-Shoot", "BoltAction-ShootNoReload"};
                            AimingAnimation = new string[]{"", "BoltAction-Shoot"};
                            GunCooldown[0] = 1f;
                            AimZoom = 25f;
                            ReloadingAnimation = new string[] { "BoltAction-Reload", "Reloading1BL", "OneByOne" };
                            ReloadVariables = new float[] { 5f, 33f, 2f };
                            RecoilPhysics = new float[]{0f, 100f};
                            break;
                        case 35:
                            GunType = "DBShotgun";
                            FireAnimation = new string[]{"DBshotgun-Shoot", ""};
                            GunCooldown[0] = 0.5f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "DBshotgun-Reload", "Reloading1BL", "FullLoad" };
                            ReloadVariables = new float[] { 2f, 33f, 2f };
                            RecoilPhysics = new float[]{0f, 100f};
                            AmountToShoot = 3;
                            break;
                        case 36:
                            GunType = "Thompson";
                            FireAnimation = new string[]{"Thompson-Shoot", ""};
                            GunCooldown[0] = 0.075f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Thompson-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 30f, 37f, 2f };
                            break;
                        case 38: case 996:
                            GunType = "AK-47";
                            FireAnimation = new string[]{"AR-Shoot", ""};
                            GunCooldown[0] = 0.08f;
                            AimZoom = 40f;
                            ReloadingAnimation = new string[] { "AK-Reload", "Reloading", "FullLoad" };
                            ReloadVariables = new float[] { 30f, 39f, 3f };
                            break;
                        case 40:
                            GunType = "Shotgun";
                            FireAnimation = new string[]{"Shotgun-Shoot", "Shotgun-ShootNoReload"};
                            AimingAnimation = new string[]{"", "Shotgun-Shoot"};
                            GunCooldown[0] = 1.5f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Shotgun-Reload", "Reloading1BL", "OneByOne" };
                            ReloadVariables = new float[] { 5f, 33f, 2f };
                            RecoilPhysics = new float[]{0f, 100f};
                            AmountToShoot = 3;
                            break;
                        case 41:
                            GunType = "MP5";
                            FireAnimation = new string[]{"Thompson-Shoot", ""};
                            GunCooldown[0] = 0.05f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "MP5-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 30f, 37f, 2f };
                            break;
                        case 42:
                            GunType = "M4";
                            FireAnimation = new string[]{"AR-Shoot", ""};
                            GunCooldown[0] = 0.1f;
                            AimZoom = 40f;
                            ReloadingAnimation = new string[] { "M4-Reload", "Reloading", "FullLoad" };
                            ReloadVariables = new float[] { 30f, 39f, 3f };
                            break;
                        case 55:
                            GunType = "Sten";
                            FireAnimation = new string[]{"Sten-Shoot", ""};
                            AimingAnimation = new string[]{"Sten-Aim", ""};
                            GunCooldown[0] = 0.075f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Sten-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 32f, 37f, 2f };
                            break;
                        case 56:
                            if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) > 1f) {
                                GunType = "Garand";
                            } else {
                                GunType = "GarandR";
                            }
                            FireAnimation = new string[]{"BoltAction-ShootNoReload", ""};
                            GunCooldown[0] = 0.25f;
                            AimZoom = 30f;
                            ReloadingAnimation = new string[] { "Garand-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 8f, 33f, 2f };
                            RecoilPhysics = new float[]{0f, 100f};
                            break;
                        case 57:
                            GunType = "Famas";
                            FireAnimation = new string[]{"AR-Shoot", ""};
                            GunCooldown = new[] {0.5f, 0.075f};
                            BurstFire = 3;
                            AimZoom = 40f;
                            ReloadingAnimation = new string[] { "FAMAS-Reload", "Reloading", "FullLoad" };
                            ReloadVariables = new float[] { 25f, 39f, 3f };
                            break;
                        case 58:
                            GunType = "Uzi";
                            FireAnimation = new string[]{"Pistol-Shoot", ""};
                            AimingAnimation = new string[]{"Pistol-Aim", "Pistol-AimShoot"};
                            GunCooldown[0] = 0.075f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Uzi-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 25, 37f, 1.5f };
                            break;
                        case 59:
                            GunType = "G3";
                            FireAnimation = new string[]{"AR-Shoot", ""};
                            GunCooldown[0] = 0.1f;
                            AimZoom = 40f;
                            ReloadingAnimation = new string[] { "G3-Reload", "Reloading", "FullLoad" };
                            ReloadVariables = new float[] { 20f, 39f, 3f };
                            break;
                        case 60:
                            GunType = "Scar";
                            FireAnimation = new string[]{"AR-Shoot", ""};
                            GunCooldown[0] = 0.08f;
                            AimZoom = 40f;
                            ReloadingAnimation = new string[] { "SCAR-Reload", "Reloading", "FullLoad" };
                            ReloadVariables = new float[] { 28f, 39f, 3f };
                            break;
                        case 61:
                            GunType = "SPAS";
                            FireAnimation = new string[]{"Shotgun-ShootNoReload", ""};
                            GunCooldown[0] = 0.5f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Shotgun-Reload", "Reloading1BL", "OneByOne" };
                            ReloadVariables = new float[] { 8f, 33f, 2f };
                            RecoilPhysics = new float[]{0f, 100f};
                            AmountToShoot = 3;
                            break;
                        case 62:
                            GunType = "SAW";
                            FireAnimation = new string[]{"AR-Shoot", ""};
                            GunCooldown[0] = 0.05f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "SAW-Reload", "ReloadingMG", "FullLoad" };
                            ReloadVariables = new float[] { 100f, 63f, 7f };
                            break;
                        case 64:
                            GunType = "Minigun";
                            FireAnimation = new string[]{"Minigun-Shoot", ""};
                            GunCooldown[0] = 0.025f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Minigun-Reload", "ReloadingMG", "FullLoad" };
                            ReloadVariables = new float[] { 500f, 63f, 7f };
                            break;
                        case 65:
                            GunType = "MosinNagant";
                            FireAnimation = new string[]{"BoltAction-Shoot", "BoltAction-ShootNoReload"};
                            AimingAnimation = new string[]{"", "BoltAction-Shoot"};
                            GunCooldown[0] = 1.5f;
                            AimZoom = 25f;
                            ReloadingAnimation = new string[] { "BoltAction-Reload", "Reloading1BL", "OneByOne" };
                            ReloadVariables = new float[] { 5f, 33f, 2f };
                            break;
                        case 113:
                            GunType = "Musket";
                            FireAnimation = new string[]{"BoltAction-ShootNoReload", ""};
                            GunCooldown[0] = 0.1f;
                            AimZoom = 30f;
                            ReloadingAnimation = new string[] { "BoltAction-Reload", "Reloading1BL", "FullLoad" };
                            ReloadVariables = new float[] { 1f, 33f, 2f };
                            break;
                        case 135:
                            GunType = "G18";
                            FireAnimation = new string[]{"Pistol-Shoot", ""};
                            AimingAnimation = new string[]{"Pistol-Aim", "Pistol-AimShoot"};
                            GunCooldown = new []{0.5f, 0.08f};
                            BurstFire = 3;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Pistol-Reload", "ReloadingShort", "FullLoad" };
                            ReloadVariables = new float[] { 17f, 30f, 1.5f };
                            break;
                        case 137:
                            GunType = "M1Carbine";
                            FireAnimation = new string[]{"AR-Shoot", ""};
                            GunCooldown[0] = 0.15f;
                            AimZoom = 30f;
                            ReloadingAnimation = new string[] { "AK-Reload", "Reloading", "FullLoad" };
                            ReloadVariables = new float[] { 15f, 39f, 3f };
                            break;
                        case 157:
                            GunType = "Flintlock";
                            FireAnimation = new string[]{"Flintlock-Shoot", ""};
                            AimingAnimation = new string[]{"", "Flintlock-Shoot"};
                            GunCooldown[0] = 1f;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "Flintlock-Reload", "ReloadingFlintlock", "FullLoad" };
                            ReloadVariables = new float[] { 1f, 158f, 5f };
                            DelayFire = Random.Range(.1f, .5f);
                            DelayFireEffects = new string[] {"Trigger", "GunEmpty"};
                            break;
                        case 159:
                            GunType = "BakerRifle";
                            FireAnimation = new string[]{"BakerRifle-Shoot", ""};
                            GunCooldown[0] = 1f;
                            AimZoom = 25f;
                            ReloadingAnimation = new string[] { "BakerRifle-Reload", "ReloadingBakerRifle", "FullLoad" };
                            ReloadVariables = new float[] { 1f, 158f, 10f };
                            DelayFire = Random.Range(.1f, .5f);
                            DelayFireEffects = new string[] {"Trigger", "GunEmpty"};
                            break;
                        case 160:
                            GunType = "NockGun";
                            FireAnimation = new string[]{"NockGun-Shoot", ""};
                            GunCooldown = new[] {1f, Random.Range(0f, .1f)};
                            BurstFire = 7;
                            AimZoom = 55f;
                            ReloadingAnimation = new string[] { "NockGun-Reload", "ReloadingNockGun", "OneByOne" };
                            ReloadVariables = new float[] { 7f, 158f, 10f };
                            DelayFire = Random.Range(.1f, .5f);
                            DelayFireEffects = new string[] {"Trigger", "GunEmpty"};
                            break;
                    }
                    if(FireAnimation[1] == "") FireAnimation[1] = FireAnimation[0];
                    // Get range info

                    // Attachment stats change
                    if(GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "101"){
                        FireAnimation[0] = "Grip" + FireAnimation[0];
                        FireAnimation[1] = "Grip" + FireAnimation[1];
                        ReloadingAnimation[0] = "Grip" + ReloadingAnimation[0];
                        AimingAnimation = new [] {"", FireAnimation[0]};
                        if(currID == 55) 
                            FireAnimation = new string[]{"GripThompson-Shoot", "GripTHompson-Shoot"};
                    } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "103") {
                        AimZoom = 5f;
                        BurstFire = 1;
                        GunCooldown[0] = 1f;
                        GunSpreadPC = 0f;
                    } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "104") {
                        AimZoom = 25f;
                    }
                    GunSpreadPC = Mathf.Clamp(GunSpreadPC, 0f, 1f);

                    if (GS.GetComponent<GameScript>().ReceiveButtonPress("AltAction", "Hold") > 0f) {
                        if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "14") {
                            if (CantUseItem <= 0f) {
                                CantUseItem = 0.5f;
                                CantSwitchItem = 0.5f;
                                ItemsShown.GetComponent<Animator>().Play(FireAnimation[1], 0, 0f);
                                RS.Attack(new string[]{ "Bayonet", "Power" + (Drunkenness / 10f), "Inventory" + CurrentItemHeld }, LookDir.position, MainCamera.forward, this.gameObject, SlimEnd, SlimEnd);
                            }
                        } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "102") {
                            if (CantUseItem <= 0f && float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) >= (int)(ReloadVariables[0] / 2f)) {
                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-" + ((int)(ReloadVariables[0] / 2f)).ToString(CultureInfo.InvariantCulture));//Inventory[CurrentItemHeld].y -= (int)(ReloadVariables[0] / 2f);
                                CantUseItem = 1f;
                                CantSwitchItem = 1f;
                                ItemsShown.GetComponent<Animator>().Play(FireAnimation[1], 0, 0f);
                                RS.Attack(new string[]{ "GrenadeLauncher", "Power" + (Drunkenness / 10f), "Inventory" + CurrentItemHeld }, LookDir.position, MainCamera.forward, this.gameObject, SlimEnd, SlimEnd);
                            }   
                        } else {
                            if (IsReloading <= 0f) {
                                ZoomValues[0] = AimZoom + FOVoffset[0];
                                ZoomValues[3] = 0.03f;
                                if (AimingAnimation[0] == "" || GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "101") {
                                    if (ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(PlayItemAnim("Idle", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition)) && ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.001f) {
                                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Idle", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                                    }
                                } else {
                                    if (ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(PlayItemAnim("Idle", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition)))
                                        ItemsShown.GetComponent<Animator>().Play(AimingAnimation[0], 0, 0f);
                                    else if (ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
                                        ItemsShown.GetComponent<Animator>().Play(AimingAnimation[0], 0, 0.45f);
                                }
                            }
                        }
                    }

                    bool CanFire = true;
                    if (((GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "103") && ZoomValues[1] > ZoomValues[0]) || (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "64" && ZoomValues[1] > ZoomValues[0])) {
                        CanFire = false;
                    }

                    if(GS.GetComponent<GameScript>().ReceiveButtonPress("Action", "Hold") > 0f && BulletsLoaded <= 0 && CantUseItem <= 0f && float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) >= AmmoInUse && CanFire == true){
                        BulletsLoaded = Mathf.Clamp(BurstFire, 1, int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va")));
                        UseDelay = new float[] {0f, DelayFire};
                        CantUseItem = UseDelay[1];
                        if(DelayFireEffects.Length >= 1) {
                            ItemsShown.GetComponent<Animator>().Play(PlayItemAnim(DelayFireEffects[0], GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                            if(DelayFireEffects.Length == 2)
                                PlaySoundBank(DelayFireEffects[1], 1);
                        }
                    }

                    if (BulletsLoaded > 0 && UseDelay[0] >= UseDelay[1] && CantUseItem <= 0f) {
                        BulletsLoaded -= 1;
                        CantUseItem = BulletsLoaded == 0 ? GunCooldown[0] : GunCooldown[1];
                        CantSwitchItem = CantUseItem;
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-" + AmmoInUse);

                        if (ZoomValues[1] <= ZoomValues[0] + 1) {
                            if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "103") {
                                ItemsShown.GetComponent<Animator>().Play(FireAnimation[0], 0, 0f);
                            } else if (AimingAnimation[1] != "") {
                                ItemsShown.GetComponent<Animator>().Play(AimingAnimation[1], 0, 0f);
                            }
                        } else {
                            ItemsShown.GetComponent<Animator>().Play(FireAnimation[0], 0, 0f);
                        }

                        for (int BulletsToSpanw = AmountToShoot; BulletsToSpanw > 0; BulletsToSpanw--) {
                            List<string> Additionals = new List<string>();
                            Additionals.Add(GunType);
                            float Gunspread = 0f;
                            if(RS.IsCausual) Gunspread = RS.ReceiveGunSpred(int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id")), 0f, GunSpreadPC).x / 2f;
                            else Gunspread = RS.ReceiveGunSpred(int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), CultureInfo.InvariantCulture), this.GetComponent<Rigidbody>().velocity.magnitude / Speed, GunSpreadPC).x;
                            if(ZoomValues[1] <= ZoomValues[0] + 1) Gunspread /= 3f;
                            Additionals.Add("GunSpread" + Gunspread.ToString(CultureInfo.InvariantCulture));
                            Additionals.Add("Inventory" + CurrentItemHeld);
                            if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "103") {
                                Additionals.Add("Power10");
                            } else {
                                Additionals.Add("Power" + (Drunkenness / 10f));
                            }
                            if (BulletsToSpanw > AmountOfGunFires) {
                                Additionals.Add("HideGunFire");
                            }
                            if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "100") {
                                Additionals.Add("IsSilenced");
                            }
                            RS.Attack(Additionals.ToArray(), LookDir.position, MainCamera.forward, this.gameObject, SlimEnd, BulletChamber);
                        }
                        if (!(IsCrouching >= 1f && GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "105")) {
                            float[] RecoilMP = new float[] { Mathf.Lerp(1f, Random.Range(-0.25f, 0.25f), Mathf.Clamp(GunSpreadPC * RecoilPhysics[0], 0f, 1f) ) , Mathf.Lerp(0f, 2f, Mathf.Clamp(GunSpreadPC * RecoilPhysics[1], 0f, 1f) ) };
                            float GunspreadMP = 1f;
                            float YrecoilRandomizer = (Mathf.PerlinNoise(Time.time / 3f, Time.time / -3f) * 2f) - 1f;
                            if (GS.GetSemiClass(Inventory[CurrentItemHeld], "at") == "101") RecoilMP = new float[] { 0.3f, 0.3f };
                            if (IsCasual) {
                                RecoilMP = new float[] {RecoilMP[0] / 2f, RecoilMP[1] / 2f};
                            }
                            if (ZoomValues[1] <= ZoomValues[0] + 1) { 
                                RecoilMP = new float[]{RecoilMP[0] / 2f, RecoilMP[1] / 2f}; 
                                GunspreadMP /= 3f;
                            }
                            float RecX = (RS.ReceiveGunSpred(currID, this.GetComponent<Rigidbody>().velocity.magnitude / Speed, GunSpreadPC).z) * RecoilMP[0];
                            float RecY = (RS.ReceiveGunSpred(currID, this.GetComponent<Rigidbody>().velocity.magnitude / Speed, GunSpreadPC).w * YrecoilRandomizer) * RecoilMP[1];

                            GunSpreadPC += RS.ReceiveGunSpred(currID, this.GetComponent<Rigidbody>().velocity.magnitude / Speed, GunSpreadPC).y;
                            ItemShakePos = new Vector3(
                                (RS.ReceiveGunSpred(currID, this.GetComponent<Rigidbody>().velocity.magnitude / Speed, GunSpreadPC).w * RecoilMP[1] * -YrecoilRandomizer), 
                                RS.ReceiveGunSpred(currID, this.GetComponent<Rigidbody>().velocity.magnitude / Speed, GunSpreadPC).z * RecoilMP[0], 0f) * 10f;

                            if(IsCasual){
                                RecoilCam(new Vector3(RecX * -(GunSpreadPC * 5f), RecY * -(GunSpreadPC * 5f), 0f), 0.25f, 0f);
                                GunSpreadPC = Mathf.Clamp(GunSpreadPC, 0f, 0.5f);
                            } else {
                                RecoilCam(new Vector3(RecX / -2f, RecY / -2f, 0f), 0.25f, 0f);
                                LookX -= RecX / 2f;
                                LookY -= RecY / 2f;
                            }
                            GunSpreadRegain = GunCooldown[0] * 0.75f;
                        }
                    } else if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0 && CanFire == true) {
                        CantUseItem = 1f;
                        PlaySoundBank("GunEmpty", 1);
                    }

                    bool CanReload = false;
                    if (currID == 56) {
                        if (GS.GetSemiClass(Inventory[CurrentItemHeld], "va") == "0") {
                            CanReload = true;
                        } else if (GS.ReceiveButtonPress("Reload", "Hold") > 0f && CantUseItem <= 0f && float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) < ReloadVariables[0]) {
                            GS.Mess(GS.SetString("It must be empty first!", "Magazynek musi zostać opróżniony!"), "Error");
                            CantUseItem = 0.5f;
                        }
                    } else if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) < ReloadVariables[0]) {
                        CanReload = true;
                    }

                    // Reloading

                    // Get permission for reloading
                    if (GS.ReceiveButtonPress("Reload", "Hold") > 0f && CantUseItem <= 0f && CanReload == true) {
                        if (GS.GameModePrefab.x == 1 || IsCasual) {

                            int ToLoad = (int)Mathf.Clamp((ReloadVariables[0] - float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture)), 0, GS.Ammo);
                            
                            if (ToLoad > 0) {
                                ReloadInfo = new string[]{ToLoad.ToString(), ReloadingAnimation[2]};
                                ItemsShown.GetComponent<Animator>().Play(ReloadingAnimation[0], 0, 0f);
                                PlaySoundBank(ReloadingAnimation[1], 1, 1f, 0f, "Override");
                                CantUseItem = ReloadVariables[2];
                                IsReloading = ReloadVariables[2];
                            } else {
                                CantUseItem = 0.5f;
                                GS.Mess(GS.SetString("You don't have any spare ammo!", "Nie masz zapasowej amunicji!"), "Error");
                            }

                        } else {

                            int[] ToLoad = new int[] { (int)(ReloadVariables[0] - float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture)), 0 };
                            for (int CheckInv = 0; CheckInv < MaxInventorySlots; CheckInv++) {
                                if (float.Parse(GS.GetSemiClass(Inventory[CheckInv], "id"), CultureInfo.InvariantCulture) == ReloadVariables[1]) {
                                    if (float.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture) >= ToLoad[0] - ToLoad[1]) {
                                        ToLoad[1] += ToLoad[0] - ToLoad[1];
                                    } else if (float.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture) < ToLoad[0] - ToLoad[1]) {
                                        ToLoad[1] += int.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture);
                                    }
                                }
                                if (ToLoad[1] == ToLoad[0]) {
                                    break;
                                }
                            }
                            if(ToLoad[1] > 0 || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "996"){
                                ReloadInfo = new string[]{ToLoad[1].ToString(), ReloadingAnimation[2]};
                                ItemsShown.GetComponent<Animator>().Play(ReloadingAnimation[0], 0, 0f);
                                PlaySoundBank(ReloadingAnimation[1], 1, 1f, 0f, "Override");
                                CantUseItem = ReloadVariables[2];
                                IsReloading = ReloadVariables[2];
                            } else {
                                CantUseItem = 0.5f;
                                GS.Mess(GS.SetString("You need " + GS.itemCache[(int)ReloadVariables[1]].getName() + "!", "Potrzebujesz " + GS.itemCache[(int)ReloadVariables[1]].getName() + "!"), "Error");
                            }
                                
                        }
                    }

                    // Actually reload
                    if(ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ReloadingAnimation[0])){
                        if(ReloadInfo[1] == "FullLoad"){

                            MainCanvas.CSWait = new float[]{ ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime, 0.2f};

                            if(ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f){
                                
                                if(GS.GameModePrefab.x == 1 || IsCasual){
                                    int ToLoad = Mathf.Clamp(int.Parse(ReloadInfo[0]), 0, GS.Ammo);
                                    Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+" + int.Parse(ReloadInfo[0])); //Inventory[CurrentItemHeld].y += int.Parse(ReloadInfo[0]);
                                    GS.Ammo -= ToLoad;
                                } else {
                                    int ToLoad = 0;
                                    List<Vector2> ConsumedAmmo = new List<Vector2>();
                                    for (int CheckInv = 0; CheckInv < MaxInventorySlots; CheckInv++) {
                                        if (float.Parse(GS.GetSemiClass(Inventory[CheckInv], "id"), CultureInfo.InvariantCulture) == ReloadVariables[1]) {
                                            if (int.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture) >= int.Parse(ReloadInfo[0]) - ToLoad) {
                                                ConsumedAmmo.Add(new Vector2(CheckInv, int.Parse(ReloadInfo[0]) - ToLoad));
                                                ToLoad += int.Parse(ReloadInfo[0]) - ToLoad;
                                            } else if (float.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture) < int.Parse(ReloadInfo[0]) - ToLoad) {
                                                ConsumedAmmo.Add(new Vector2(CheckInv, float.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture)));
                                                ToLoad += int.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture);
                                            }
                                        }
                                        if (ToLoad == int.Parse(ReloadInfo[0])) break;
                                    }
                                    if (ToLoad > 0 || currID == 996) {
                                        if (currID == 996) {
                                            Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", ReloadVariables[0].ToString()); //Inventory[CurrentItemHeld].y = ReloadVariables[0];
                                        } else {
                                            Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+" + ToLoad.ToString()); //Inventory[CurrentItemHeld].y += ToLoad;
                                            foreach (Vector2 WipeMag in ConsumedAmmo) {
                                                Inventory[(int)WipeMag.x] = GS.SetSemiClass(Inventory[(int)WipeMag.x], "va", "/+-" + WipeMag.y.ToString());//Inventory[(int)WipeMag.x].y -= WipeMag.y;
                                            }
                                        }
                                    }
                                }

                                ReloadInfo = new string[]{"0", "None"};

                            }

                        } else if (ReloadInfo[1] == "OneByOne"){

                            MainCanvas.CSWait = new float[]{ 1f - (float.Parse(ReloadInfo[0], CultureInfo.InvariantCulture) / (float)ReloadVariables[0]), 0.2f};

                            if(ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.66f){
                                bool Stop = false;

                                if(GS.GameModePrefab.x == 1 || IsCasual){
                                    Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+1");//Inventory[CurrentItemHeld].y += 1;
                                    ReloadInfo[0] = (int.Parse(ReloadInfo[0]) - 1).ToString();
                                    GS.Ammo -= 1;
                                    CantUseItem = (ReloadVariables[2] / 3f) * 1.1f;
                                    IsReloading = (ReloadVariables[2] / 3f) * 1.1f;
                                    if(ReloadInfo[0] == "0" || GS.Ammo <= 0) 
                                        Stop = true;
                                } else {
                                    bool HasABullet = false;
                                    for (int CheckInv = 0; CheckInv < MaxInventorySlots; CheckInv++) {
                                        if (float.Parse(GS.GetSemiClass(Inventory[CheckInv], "id"), CultureInfo.InvariantCulture) == ReloadVariables[1]) {
                                            if (float.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture) > 0) {
                                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+1");//Inventory[CurrentItemHeld].y += 1;
                                                ReloadInfo[0] = (int.Parse(ReloadInfo[0]) - 1).ToString();
                                                Inventory[CheckInv] = GS.SetSemiClass(Inventory[CheckInv], "va", "/+-1");//Inventory[CheckInv].y -= 1;
                                                CantUseItem = (ReloadVariables[2] / 3f) * 1.1f;
                                                IsReloading = (ReloadVariables[2] / 3f) * 1.1f;
                                                HasABullet = true;
                                            }
                                        }
                                    }
                                    if(!HasABullet || ReloadInfo[0] == "0") Stop = true;
                                }

                                if(!Stop){
                                    ItemsShown.GetComponent<Animator>().Play(ReloadingAnimation[0], 0, 0.33f);
                                    PlaySoundBank(ReloadingAnimation[1], 1, 1f, ReloadVariables[2] * 0.33f, "Override");
                                } else {
                                    ReloadInfo = new string[]{"0", "None"};
                                }

                            }

                        }
                    }
                    // Reloading

                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        MainCanvas.CSAlert = new float[] { 2f, 1f };
                    }

                    break;
                case 43:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && Coldness > 0f) {
                        Coldness = -15f;
                        GameObject Matches = Instantiate(EffectPrefab) as GameObject;
                        Matches.transform.position = this.transform.position;
                        Matches.GetComponent<EffectScript>().EffectName = "Matches";
                        MainCanvas.Flash(new Color32(255, 128, 0, 255), new float[]{0.5f, 0.5f});
                        GS.Mess(GS.SetString("You feel warm", "Jest ci ciepło"), "Good");
                        InvGet(CurrentItemHeld.ToString(), 1);
                    }
                    break;
                case 44:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        Hurt(5f, "Overdose", false, Vector3.zero);
                        Adrenaline += 15f;
                        GameObject Matches = Instantiate(EffectPrefab) as GameObject;
                        Matches.transform.position = this.transform.position;
                        Matches.GetComponent<EffectScript>().EffectName = "PatchUp";
                        MainCanvas.Flash(new Color32(255, 128, 0, 255), new float[]{0.5f, 0.5f});
                        GS.Mess(GS.SetString("You feel energetic", "Czujesz się energetycznie"), "Good");
                        InvGet(CurrentItemHeld.ToString(), 1);
                        ItemsShown.GetComponent<Animator>().Play("Syringe-Eat", 0, 0f);
                        CantUseItem = CantSwitchItem = 1f;
                    }
                    break;
                case 45: case 46: case 47: case 48: case 49: case 994: case 51: case 53: case 86: case 94: case 125: case 126:
                    // Get Clothing Infos
                    string ClothingName = GS.itemCache[currID].getName();
                    Color32 WearColor = new Color32(255, 255, 255, 255);

                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {

                        string Category = GS.GetSemiClass(Inventory[CurrentItemHeld], "ct");

                        int AddHere = -1;
                        for (int CheckEq = 0; CheckEq < 4; CheckEq ++) {
                            if (GS.GetSemiClass(Equipment[CheckEq], "ct") == Category) {
                                AddHere = -2;
                                CantUseItem = 0.5f;
                                string WhatCategory = "";
                                if (Category == "1") {
                                    WhatCategory = GS.itemCache[int.Parse(GS.GetSemiClass(Equipment[CheckEq], "id"))].getName();
                                }
                                GS.Mess(GS.SetString("Can't wear that, unequip " + GS.itemCache[int.Parse(GS.GetSemiClass(Equipment[CheckEq], "id"))].getName() + " first!", "Nie można tego ubrać, " + GS.itemCache[int.Parse(GS.GetSemiClass(Equipment[CheckEq], "id"))].getName() + " zajmuje miejsce!"), "Error");
                                break;
                            } else if (GS.GetSemiClass(Equipment[CheckEq], "id") == "0") {
                                AddHere = CheckEq;
                                break;
                            }
                        }

                        if (AddHere >= 0) {

                            Equipment[AddHere] = Inventory[CurrentItemHeld];//new Vector4(Inventory[CurrentItemHeld].x, Inventory[CurrentItemHeld].y, Inventory[CurrentItemHeld].z, Category);

                            MainCanvas.Flash(new Color32(255, 255, 255, 75), new float[]{0.5f, 0.5f});
                            GS.Mess(GS.SetString(ClothingName + " worn", "Ubrano: " + ClothingName), "Wear");
                            InvGet(CurrentItemHeld.ToString(), 1);

                        } else if (AddHere == -1) {
                            CantUseItem = 0.5f;
                            GS.Mess(GS.SetString("You can't wear more than 4 items at once!", "Nie możesz mieć więcej niż 4 ubrań na raz!"), "Error");
                        }
                    
                    }
                    break;
                case 990:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        if (RS.RoundState != "TealState") {
                            CantUseItem = 1f;
                            InvGet(CurrentItemHeld.ToString(), 1);
                            RS.TealState = 1;
                            MainCanvas.Flash(new Color32(255, 255, 255, 255), new float[]{4f, 2f});
                            GS.Mess(GS.SetString("You're under teal state", "Jesteś pod wpływem cyjanu"), "Teal");
                        } else {
                            CantUseItem = 0.5f;
                            GS.Mess(GS.SetString("You're already under teal state", "Już jesteś pod wpływem cyjanu"), "Error");
                        }
                    }
                    break;
                case 991:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantSwitchItem = 1f;
                        CantUseItem = 1f;
                        CantSwitchItem = 1f;
                        Infection = 0f;
                        Bleeding = 0f;
                        Radioactivity = 0f;
                        Coldness = 0f;
                        Tiredness = 0f;
                        BrokenBone = 0;
                        Wet = 0f;
                        Oxygen[0] = Oxygen[1];
                        Health[0] = Health[1];
                        Energy[0] = Energy[1];
                        Food[0] = Food[1];
                        GameObject Gulp = Instantiate(EffectPrefab) as GameObject;
                        Gulp.transform.position = LookDir.position - LookDir.up * 0.25f;
                        Gulp.transform.rotation = LookDir.rotation;
                        Gulp.GetComponent<EffectScript>().EffectName = "Drinking";
                        Gulp.GetComponent<EffectScript>().EffectColor = new Color32(255, 0, 255, 255);
                        MainCanvas.Flash(new Color32(255, 128, 0, 255), new float[]{0.5f, 0.5f});
                        GS.Mess(GS.SetString("You feel way better", "Czujesz się o wiele lepiej"), "Good");
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-34");//Inventory[CurrentItemHeld].y -= 34f;
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                            InvGet(CurrentItemHeld.ToString(), 1);
                        }
                    }
                    break;
                case 997:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) >= 10f) {
                        CantUseItem = 0.25f;
                        CantSwitchItem = CantUseItem;
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-10");//Inventory[CurrentItemHeld].y -= 10f;
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Shoot", "997", AnimationAddition), 0, 0f);
                        RS.Attack(new string[]{ "LightningBolt", "Power" + (Drunkenness / 10f), "ItemID" + CurrentItemHeld }, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                    }
                    break;
                case 999:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        for (int DropItems = 10; DropItems > 0; DropItems--) {
                            GameObject Present = Instantiate(ItemPrefab) as GameObject;
                            Present.transform.position = this.transform.position + new Vector3(Random.Range(-3f, 3f), 2f, Random.Range(-3f, 3f));
                            Present.GetComponent<ItemScript>().Variables = GS.itemCache[RS.TotalItems[(int)Random.Range(0f, RS.TotalItems.Length - 0.1f)]].startVariables;
                        }
                        MainCanvas.Flash(new Color32(255, 255, 255, 75), new float[]{0.5f, 0.5f});
                        GS.Mess(GS.SetString("Here are some items", "Masz tu kilka przedmiotów"), "Wear");
                        InvGet(CurrentItemHeld.ToString(), 1);
                    }
                    break;
                case 66: case 110: case 131:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && GS.GetSemiClass(Inventory[CurrentItemHeld], "va") == "0" && CantUseItem <= 0f) {
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "1"); //Inventory[CurrentItemHeld].y = 1f;
                        GameObject Matches = Instantiate(EffectPrefab) as GameObject;
                        Matches.transform.position = this.transform.position;
                        Matches.GetComponent<EffectScript>().EffectName = "Unpin";
                        GS.Mess(GS.SetString("Hold Q to throw!", "Przytrzymaj Q by rzucić!"));
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Grenade", GS.GetSemiClass(Inventory[CurrentItemHeld], "id")));
                    }
                    break;
                case 67: case 114:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 1.5f;
                        //GameObject SpawnAttack = Instantiate(AttackPrefab) as GameObject;
                        //SpawnAttack.transform.position = LookDir.position;
                        //SpawnAttack.transform.eulerAngles = LookDir.eulerAngles;
                            
                        //SpawnAttack.GetComponent<AttackScript>().Attacker = this.gameObject;
                        //SpawnAttack.GetComponent<AttackScript>().WchichItemWasHeld = CurrentItemHeld;
                        //SpawnAttack.GetComponent<AttackScript>().Slimend = SlimEnd;
                        if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "67") {
                            //SpawnAttack.GetComponent<AttackScript>().GunName = "Rocket";
                            ItemsShown.GetComponent<Animator>().Play("Panzerfaust-Shoot", 0, 0f);
                            CantSwitchItem = 1f;
                            RS.Attack(new string[]{ "Rocket", "Inventory" + CurrentItemHeld }, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                        } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "114") {
                            //SpawnAttack.GetComponent<AttackScript>().GunName = "LightningBolt";
                            //SpawnAttack.GetComponent<AttackScript>().SpecialGunSpread = 6f;
                            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(255, 255, 255, 128), new float[]{0.5f, 0.5f});
                            RS.Attack(new string[]{ "LightningBolt", "Inventory" + CurrentItemHeld, "GunSpread0" }, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                        }
                        InvGet(CurrentItemHeld.ToString(), 1);
                    }
                    break;
                case 69:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Shoot", "69", AnimationAddition), 0, 0f);
                        PlaySoundBank("BowLoad", 1);
                        CantUseItem = 1f;
                        CantSwitchItem = 1f;
                        PermissionToFireBow = true;
                        UseDelay = new[] {0f, 1f};
                    }
                    if (UseDelay[0] >= 0.9f && PermissionToFireBow == true) {
                        PermissionToFireBow = false;
                        RS.Attack( new string[]{ "Arrow" , "Inventory" + CurrentItemHeld }, LookDir.position, MainCamera.forward, this.gameObject, SlimEnd );
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1");//Inventory[CurrentItemHeld].y -= 1;
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0) {
                            InvGet(CurrentItemHeld.ToString(), 1);
                        }
                    }
                    break;
                case 85:
                    if (this.GetComponent<Rigidbody>().velocity.y < -6f) {
                        this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x, -6f, this.GetComponent<Rigidbody>().velocity.z);
                        Ach_Umbrella += 0.02f;
                        if(Ach_Umbrella >= 5f) GS.PS.AchProg("Ach_Airborne", "0");
                    } else {
                        Ach_Umbrella = 0f;
                    }
                    break;
                case 88: case 89: case 90:
                    float AmountToFix = 0f;
                    float Uses = 1f;
                    string SoundToPlay = "";
                    string AnimationToPlay = "";
                    if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "88") {
                        AmountToFix = 50f;
                        Uses = 100f;
                        SoundToPlay = "Ducktape";
                        AnimationToPlay = "DuckTape";
                    } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "89") {
                        AmountToFix = 100f;
                        Uses = 33f;
                        SoundToPlay = "BlowTorch";
                        AnimationToPlay = "BlowTorch";
                    } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "90") {
                        AmountToFix = 5f;
                        Uses = 0f;
                        SoundToPlay = "Ducktape";
                        AnimationToPlay = "Wrench";
                    }
                    if (InteractedGameobject != null) {
                        if (InteractedGameobject.tag == "Item") {
                            if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && float.Parse(GS.GetSemiClass(InteractedGameobject.GetComponent<ItemScript>().Variables, "va"), CultureInfo.InvariantCulture) < 100f && InteractedGameobject.GetComponent<ItemScript>().CanBeFixed == true) {
                                CantUseItem = 0.5f;
                                CantSwitchItem = 0.5f;
                                ItemsShown.GetComponent<Animator>().Play(PlayItemAnim(AnimationToPlay, GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                                InteractedGameobject.GetComponent<ItemScript>().Variables = GS.SetSemiClass(InteractedGameobject.GetComponent<ItemScript>().Variables, "va", (Mathf.Clamp( float.Parse(GS.GetSemiClass(InteractedGameobject.GetComponent<ItemScript>().Variables, "va"), CultureInfo.InvariantCulture) + AmountToFix, 0f, 100f)).ToString() );//InteractedGameobject.GetComponent<ItemScript>().Variables.y = Mathf.Clamp(InteractedGameobject.GetComponent<ItemScript>().Variables.y + AmountToFix, 0f, 100f);
                                if (SoundToPlay != "") {
                                    GameObject RepairEffect = Instantiate(EffectPrefab) as GameObject;
                                    RepairEffect.transform.position = InteractedGameobject.transform.position;
                                    RepairEffect.transform.LookAt(RepairEffect.transform.position + Vector3.up * 1f);
                                    RepairEffect.GetComponent<EffectScript>().EffectName = SoundToPlay;
                                }
                                if (Uses == 100f) {
                                    InvGet(CurrentItemHeld.ToString(), 1);
                                } else if (Uses != 0f) {
                                    Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-" + Uses.ToString());//Inventory[CurrentItemHeld].y -= Uses;
                                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) - Uses <= 0f) {
                                        InvGet(CurrentItemHeld.ToString(), 1);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 91:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 2f;
                        CantSwitchItem = 2f;
                        GameObject Flash = Instantiate(EffectPrefab) as GameObject;
                        Flash.transform.position = this.transform.position;
                        Flash.transform.rotation = LookDir.rotation;
                        Flash.GetComponent<EffectScript>().EffectName = "Flash";
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Pullup", "91", AnimationAddition), 0, 0f);
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1");//Inventory[CurrentItemHeld].y -= 1f;
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                            InvGet(CurrentItemHeld.ToString(), 1);
                        }
                    }
                    break;
                case 92:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f) {
                        ZoomValues[0] = 10f;
                        ZoomValues[3] = 0.03f;
                    }
                    break;
                case 93:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 1f;
                        CantSwitchItem = 1f;
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Cowbell";
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Ring", "93", AnimationAddition), 0, 0f);
                    }
                    break;
                case 95:
                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1);
                        GS.Mess(GS.SetString("Your shield broke!", "Tarcza się popsuła!"), "ItemBroke");
                    }
                    break;
                case 96:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        InBox = true;
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("SwitchItem", "96", AnimationAddition), 0, 0f);
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-0.1");//Inventory[CurrentItemHeld].y -= 0.1f;
                        CantUseItem = 0.5f;
                        CantSwitchItem = 0.5f;
                        if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.01f) {
                            Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1");//Inventory[CurrentItemHeld].y -= 1f;
                        }
                    }
                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1);
                        GS.Mess(GS.SetString("The cardboard box broke!", "Karton się popsuł!"), "ItemBroke");
                    }
                    break;
                case 97:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && InteractedGameobject != null) {
                        if (InteractedGameobject.tag == "Interactable") {
                            if ((InteractedGameobject.GetComponent<InteractableScript>().Variables.x == 3f && InteractedGameobject.GetComponent<InteractableScript>().Variables.z == 2f) || InteractedGameobject.GetComponent<InteractableScript>().Variables.x == 1f) {
                                CantUseItem = 0.5f;
                                CantSwitchItem = 0.5f;
                                ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("LockPick", "97", AnimationAddition), 0, 0f);
                                int Picklock = (int)Random.Range(0f, 3.9f);
                                if (Picklock == 0 || Picklock == 1) {
                                    GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                                    Ring.transform.position = this.transform.position;
                                    Ring.GetComponent<EffectScript>().EffectName = "Unpin";
                                    MainCanvas.Flash(new Color32(255, 255, 255, 75), new float[]{0.02f, 0.02f});
                                    if (InteractedGameobject.GetComponent<InteractableScript>().Variables.x == 1f) {
                                        GS.Mess(GS.SetString("Barrel has been opened", "Beczka została otwarta"), "Good");
                                        InteractedGameobject.GetComponent<InteractableScript>().Interaction("Break", 9999f);
                                    } else if (InteractedGameobject.GetComponent<InteractableScript>().Variables.x == 3f) {
                                        GS.Mess(GS.SetString("Door has been opened", "Drzwi zostały otwarta"), "Good");
                                        InteractedGameobject.GetComponent<InteractableScript>().Variables.z = 0f;
                                    }
                                } else if (Picklock == 2) {
                                    InvGet(CurrentItemHeld.ToString(), 1);
                                    MainCanvas.Flash(new Color32(255, 255, 255, 75), new float[]{0.5f, 0.5f});
                                    GS.Mess(GS.SetString("Lockpick broke!", "Wytrych się złamał!"), "ItemBroke");
                                } else if (Picklock == 3) {
                                    GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                                    Ring.transform.position = this.transform.position;
                                    Ring.GetComponent<EffectScript>().EffectName = "Unpin";
                                    MainCanvas.Flash(new Color32(255, 255, 255, 75), new float[]{0.5f, 0.5f});
                                    GS.Mess(GS.SetString("Nothing...", "Nic..."));
                                }
                            }
                        }
                    }
                    break;
                case 98:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Wipe", "98", AnimationAddition), 0, 0f);
                        CantUseItem = 1f;
                        CantSwitchItem = 1f;
                        if (Bleeding > 0f) {
                            Bleeding -= 5f;
                        }
                        if (Wet > 0f) {
                            Wet -= 25f;
                        }
                        if (Radioactivity > 0f) {
                            Radioactivity -= 5f;
                        }
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-10");//Inventory[CurrentItemHeld].y -= 10f;
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                            InvGet(CurrentItemHeld.ToString(), 1);
                            GS.Mess(GS.SetString("The towel got too dirty!", "Ręcznik stał się zbyt brudny!"), "Error");
                        }
                    }
                    break;
                case 99:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        bool SeenSomething = false;
                        CantUseItem = 0.5f;
                        CantSwitchItem = 0.5f;
                        foreach (GameObject FoundInteract in GameObject.FindGameObjectsWithTag("Interactable")) {
                            if (FoundInteract.GetComponent<InteractableScript>().Discovered == false && FoundInteract.GetComponent<InteractableScript>().Variables.x == 2f) {
                                FoundInteract.GetComponent<InteractableScript>().Discovered = true;
                                GS.AddToScore(50);
                                SeenSomething = true;
                            }
                        }
                        foreach (GameObject FoundLand in GameObject.FindGameObjectsWithTag("Land")) {
                            if (FoundLand.name.Substring(2, 1) == "M" && FoundLand.name.Substring(0, 1) == "0") {
                                FoundLand.name = "1" + FoundLand.name.Substring(1);
                                GS.AddToScore(50);
                                SeenSomething = true;
                            }
                        }

                        if (SeenSomething == true) {
                            GS.Mess(GS.SetString("You now know of some new places", "Poznałeś kilka nowych miejsc"), "Draw");
                            Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1");//Inventory[CurrentItemHeld].y -= 1f;
                            if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                                InvGet(CurrentItemHeld.ToString(), 1);
                            }
                        } else {
                            GS.Mess(GS.SetString("You've already found everything on the map!", "Już poznałeś wszystko z mapy!"), "Error");
                        }

                    }
                    break;
                case 107:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && BrokenBone != 0) {
                        BrokenBone = 0;
                        CantUseItem = 0.5f;
                        CantSwitchItem = 0.5f;
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Zip";
                        InvGet(CurrentItemHeld.ToString(), 1);
                        MainCanvas.Flash(new Color32(255, 255, 255, 75), new float[]{0.5f, 0.5f});
                        GS.Mess(GS.SetString("Your bones are no longer broken!", "Twoje kości nie są już połamane!"), "Good");
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Bandage", "107"));
                    }
                    break;
                case 109:
                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1);
                        GS.Mess(GS.SetString("Out of fuel!", "Koniec paliwa!"), "Error");
                    }
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 0.1f;
                        CantSwitchItem = 0.1f;
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1"); //Inventory[CurrentItemHeld].y -= 1f;
                        RS.Attack(new string[]{"FlameThrower", "Inventory" + CurrentItemHeld}, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Pullup", "109", AnimationAddition), 0, 0.9f);
                    }
                    break;
                case 111:
                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1);
                    }
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 1f;
                        CantSwitchItem = 1f;
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1"); //Inventory[CurrentItemHeld].y -= 1f;
                        RS.Attack(new string[]{"GrenadeLauncher", "Inventory" + CurrentItemHeld}, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                        if(float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) > 0f) ItemsShown.GetComponent<Animator>().Play("M97-Shoot", 0, 0f);
                        else ItemsShown.GetComponent<Animator>().Play("M97-LastShot", 0, 0f);
                    }
                    break;
                case 112:
                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1);
                    }

                    if (GS.ReceiveButtonPress("AltAction", "Hold") > 0f) {
                        if (IsReloading <= 0f) {
                            ZoomValues[0] = 30f;
                            ZoomValues[3] = 0.03f;
                            if (ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(PlayItemAnim("Idle", "112", AnimationAddition)) && ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f) {
                                ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Idle", "112", AnimationAddition), 0, 0f);
                            }
                        }
                    }
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 2f;
                        CantSwitchItem = 2f;
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1"); //Inventory[CurrentItemHeld].y -= 1f;
                        RS.Attack(new string[]{"Arrow", "Inventory" + CurrentItemHeld}, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                        if(float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) > 0f) ItemsShown.GetComponent<Animator>().Play("Crossbow-Shoot", 0, 0f);
                        else ItemsShown.GetComponent<Animator>().Play("Crossbow-LastShot", 0, 0f);
                    }
                    break;
                case 30: case 33: case 37: case 39: case 63:
                            
                        int[] LoadVariables = new int[] { 0, 0 };
                        if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "30") {
                            LoadVariables = new int[] { 4, 16 };
                        } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "37" || GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "39") {
                            LoadVariables = new int[] { 15, 60 };
                        }

                        if (GS.ReceiveButtonPress("Reload", "Hold") > 0f && CantUseItem <= 0f && int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) < LoadVariables[1]) {
                            int[] ToLoad = new int[] { LoadVariables[0], 0 };
                            if (ToLoad[0] + int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va")) > LoadVariables[1]) {
                                ToLoad[0] -= (int)(ToLoad[0] + float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture)) - LoadVariables[1];
                            }
                            for (int CheckInv = 0; CheckInv < MaxInventorySlots; CheckInv++) {
                                if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "33" && float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) >= ToLoad[0]) {
                                    ToLoad[1] = ToLoad[0];
                                    Inventory[CheckInv] = GS.SetSemiClass(Inventory[CheckInv], "va", "/+-" + ToLoad[0].ToString());//Inventory[CheckInv].y -= ToLoad[0];
                                    if (GS.GetSemiClass(Inventory[CheckInv], "id") == "33" && float.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture) <= 0) {
                                        InvGet(CheckInv.ToString(), 1);
                                    }
                                break;
                                } else if (GS.GetSemiClass(Inventory[CheckInv], "id") == "33" && int.Parse(GS.GetSemiClass(Inventory[CheckInv], "va"), CultureInfo.InvariantCulture) < ToLoad[0]) {
                                    ToLoad[1] = int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture);
                                    InvGet(CheckInv.ToString(), 1); //Inventory[CheckInv] = "";
                                break;
                                }
                            }
                            if (ToLoad[1] > 0) {
                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+" + ToLoad[1].ToString());//Inventory[CurrentItemHeld].y += ToLoad[1];
                                CantUseItem = 3f;
                                CantSwitchItem = 3f;
                                BulletLoad = ToLoad[1];
                                ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Idle", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0f);
                                PlaySoundBank("Reloading", 1, 1f, 0f, "Override");
                            } else {
                                CantUseItem = 0.5f;
                                GS.Mess(GS.SetString("You need ammo pack!", "Potrzebujesz paczki z amunicją!"), "Error");
                            }
                        } else if (Input.GetButton("Reload") && CantUseItem <= 0f && int.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) < LoadVariables[1]) {
                            CantUseItem = 0.5f;
                            GS.Mess(GS.SetString("You can't load more!", "Nie możesz więcej ładować!"), "Error");
                        }

                        if (BulletLoad > 0) {
                            if (ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(PlayItemAnim("Reload", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition)) && ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f) {
                                ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Reload", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"), AnimationAddition), 0, 0.15f);
                                CantUseItem = 2f;
                                CantSwitchItem = 2f;
                                PlaySoundBank("GunEmpty", 1);
                                BulletLoad -= 1;
                            }
                        }

                    break;
                case 124:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 1f;
                        CantSwitchItem = 1f;
                        Energy[0] = Energy[1];
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Puffer", "124", AnimationAddition), 0, 0f);
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Puffer";
                        MainCanvas.Flash(new Color32(200, 225, 255, 128), new float[]{0.5f, 0.5f});
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-10");//Inventory[CurrentItemHeld].y -= 10f;
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                            InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "";
                        }
                    }
                    break;
                case 127:
                    Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", (Mathf.Clamp( float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) - 0.1f, 0f, 100f )).ToString(CultureInfo.InvariantCulture) );//Inventory[CurrentItemHeld].y = Mathf.Clamp(Inventory[CurrentItemHeld].y - 0.1f, 0f, 100f);
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 0.5f;
                        CantSwitchItem = 0.5f;
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", (Mathf.Clamp( float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) + 10f, 0f, 100f )).ToString(CultureInfo.InvariantCulture) );//Inventory[CurrentItemHeld].y = Mathf.Clamp(Inventory[CurrentItemHeld].y + 10f, 0f, 100f);
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Pullup", "127", AnimationAddition), 0, 0.8f);
                        GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                        Ring.transform.position = this.transform.position;
                        Ring.GetComponent<EffectScript>().EffectName = "Crank";
                    }
                    break;
                case 128:
                    if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "";
                    }
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        CantUseItem = 0.1f;
                        CantSwitchItem = 0.1f;
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-5"); //Inventory[CurrentItemHeld].y -= 5f;
                        //GameObject SpawnAttack = Instantiate(AttackPrefab) as GameObject;
                        //SpawnAttack.transform.position = LookDir.position;
                        //SpawnAttack.transform.eulerAngles = LookDir.eulerAngles;
                        //SpawnAttack.GetComponent<AttackScript>().GunName = "FireExtinguisher";
                        //SpawnAttack.GetComponent<AttackScript>().Attacker = this.gameObject;
                        //SpawnAttack.GetComponent<AttackScript>().WchichItemWasHeld = CurrentItemHeld;
                        //SpawnAttack.GetComponent<AttackScript>().Slimend = SlimEnd;
                        RS.Attack(new string[]{"FireExtinguisher", "Inventory" + CurrentItemHeld}, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                        ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Pullup", "128", AnimationAddition), 0, 0.5f);
                        PushbackForce = LookDir.forward * -4f;
                        ReturnPushback = 1f;
                        Fire = Mathf.Clamp(Fire - 5f, 0f, 100f);
                        if (this.transform.position.y > lastGroundY+10f) GS.PS.AchProg("Ach_RocketMan", "0");
                    }
                    break;
                case 129:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f) {
                        if (IsFishing == false) {
                            CantUseItem = 1f;
                            CantSwitchItem = 1f;
                            IsFishing = true;
                            FishingRodBait.transform.position += LookDir.forward * 1.25f;
                            FishingRodBait.GetComponent<Rigidbody>().velocity = LookDir.forward * 10f;
                            ItemsShown.GetComponent<Animator>().Play("FishingRod-Throw", 0, 0f);
                            GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                            Ring.transform.position = this.transform.position;
                            Ring.GetComponent<EffectScript>().EffectName = "Swing";
                        } else {
                            CantUseItem = 1f;
                            CantSwitchItem = 1f;
                            IsFishing = false;
                            ItemsShown.GetComponent<Animator>().Play("FishingRod-Pull", 0, 0f);
                            GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                            Ring.transform.position = this.transform.position;
                            Ring.GetComponent<EffectScript>().EffectName = "Crank";
                        }
                    }
                    break;
                case 130:
                    if (Scanner != null) {
                        if (GS.ReceiveButtonPress("Action", "Hold") > 0f) {
                            if (Scanner.transform.GetChild(0).GetChild(0).localScale == Vector3.zero) {
                                MainCanvas.PlaySoundBank("S_NVON");
                            }
                            Scanner.transform.GetChild(0).GetChild(0).localScale = Vector3.one;
                            Scanner.transform.GetChild(0).GetChild(1).localScale = Vector3.zero;
                            Scanner.transform.GetChild(1).GetComponent<Camera>().backgroundColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Idle", "130", AnimationAddition), 0, 0f);
                            ZoomValues[0] = 15f;
                            ZoomValues[3] = 0.03f;
                            if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) > 0f) {
                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-0.02");//Inventory[CurrentItemHeld].y -= 0.02f;
                            } else {
                                InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "";
                            }

                            // Scanning
                            if(scanBuffer <= 0f){
                                scanBuffer = 1f;
                                scans = new ();

                                foreach (Transform ClearScans in Scanner.transform.GetChild(1)) {
                                    Destroy(ClearScans.gameObject);
                                }

                                if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "0") {
                                    // Items
                                    Scanner.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "ItemScan-" + LookDir.transform.eulerAngles.x + ";" + LookDir.transform.eulerAngles.y;
                                    foreach (GameObject ScanItem in GameObject.FindGameObjectsWithTag("Item")) {
                                        if (Vector3.Distance(this.transform.position, ScanItem.transform.position) < 200f) {
                                            scans.Add(ScanItem);
                                        }
                                    }
                                } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "1") {
                                    // Mobs
                                    Scanner.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "PulseScan-" + LookDir.transform.eulerAngles.x + ";" + LookDir.transform.eulerAngles.y;
                                    foreach (GameObject ScanItem in GameObject.FindGameObjectsWithTag("Mob")) {
                                        if (Vector3.Distance(this.transform.position, ScanItem.transform.position) < 200f) {
                                            scans.Add(ScanItem);
                                        }
                                    }
                                    foreach (GameObject ScanItem in GameObject.FindGameObjectsWithTag("MobPH"))
                                        scans.Add(ScanItem);
                                } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "2") {
                                    // Interactables
                                    Scanner.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "UtilityScan-" + LookDir.transform.eulerAngles.x + ";" + LookDir.transform.eulerAngles.y;
                                    foreach (GameObject ScanItem in GameObject.FindGameObjectsWithTag("Interactable")) {
                                        if (Vector3.Distance(this.transform.position, ScanItem.transform.position) < 200f && ((int)ScanItem.GetComponent<InteractableScript>().Variables.x == 1 || (int)ScanItem.GetComponent<InteractableScript>().Variables.x == 4)) {
                                            scans.Add(ScanItem);
                                        }
                                    }
                                } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "3") {
                                    // Exit
                                    Scanner.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "ExitScan-" + LookDir.transform.eulerAngles.x + ";" + LookDir.transform.eulerAngles.y;
                                    foreach (GameObject ScanItem in GameObject.FindGameObjectsWithTag("Interactable"))
                                        if ((int)ScanItem.GetComponent<InteractableScript>().Variables.x == 2)
                                            scans.Add(ScanItem);
                                }

                                for (int spawnScan = 0; spawnScan < scans.Count; spawnScan++) {
                                    GameObject CreateScan = Instantiate(ScannersScan) as GameObject;
                                    CreateScan.transform.position = Scanner.transform.GetChild(1).position;
                                    CreateScan.transform.SetParent(Scanner.transform.GetChild(1));
                                }

                            } else {

                                scanBuffer -= 0.02f;

                                for (int updateScan = 0; updateScan < scans.Count; updateScan++) if (scans[updateScan]) {
                                    Transform getScan = Scanner.transform.GetChild(1).GetChild(updateScan);
                                    getScan.LookAt(scans[updateScan].transform.position);
                                    getScan.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Clamp((15f - Quaternion.Angle(Scanner.transform.GetChild(1).rotation, getScan.transform.rotation)) / 15f, 0f, 1f));
                                    getScan.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                                }

                            }
                            
                        } else {
                            Scanner.transform.GetChild(0).GetChild(0).localScale = Vector3.zero;
                            Scanner.transform.GetChild(0).GetChild(1).localScale = Vector3.one;
                            // ClearScans
                            if (scans != null && scans.Count > 0){
                                foreach (Transform ClearScans in Scanner.transform.GetChild(1)) {
                                    Destroy(ClearScans.gameObject);
                                }
                                scans = new List<GameObject>();
                            }
                        }
                        if (GS.ReceiveButtonPress("AltAction", "Hold") > 0f && CantUseItem <= 0f) {
                            CantUseItem = 0.5f;
                            CantSwitchItem = 0.5f;
                            MainCanvas.PlaySoundBank("S_NVON");
                            if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "0") {
                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "sw", "1");
                            } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "1") {
                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "sw", "2");
                            } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "2") {
                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "sw", "3");
                            } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "sw") == "3") {
                                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "sw", "0");
                            }
                            scanBuffer = 0f;
                        }
                    }
                    break;
                case 139:
                    if (GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) > 0f) {
                        Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-1"); //Inventory[CurrentItemHeld].y -= 1f;
                        //GameObject SpawnAttack = Instantiate(AttackPrefab) as GameObject;
                        //SpawnAttack.transform.position = LookDir.position;
                        //SpawnAttack.transform.eulerAngles = LookDir.eulerAngles;   
                        //SpawnAttack.GetComponent<AttackScript>().Attacker = this.gameObject;
                        //SpawnAttack.GetComponent<AttackScript>().WchichItemWasHeld = CurrentItemHeld;
                        //SpawnAttack.GetComponent<AttackScript>().Slimend = SlimEnd;
                        //SpawnAttack.GetComponent<AttackScript>().GunName = "Bazooka";
                        //SpawnAttack.GetComponent<AttackScript>().SpecialGunSpread = 3f;
                        RS.Attack(new string[]{"Bazooka", "Inventory" + CurrentItemHeld, "GunSpread3"}, LookDir.position, LookDir.forward, this.gameObject, SlimEnd);
                        if (float.Parse(GS.GetSemiClass(Inventory[CurrentItemHeld], "va"), CultureInfo.InvariantCulture) > 0f) {
                            CantUseItem = 2f;
                            CantSwitchItem = 2f;
                            ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Shoot", "139", AnimationAddition), 0, 0f);
                            PlaySoundBank("BazookaReload", 1, 1f);
                        } else {
                            InvGet(CurrentItemHeld.ToString(), 1); //Inventory[CurrentItemHeld] = "id";
                            CantUseItem = 2f;
                            CantSwitchItem = 2f;
                            ItemsShown.GetComponent<Animator>().Play(PlayItemAnim("Shoot", "139", AnimationAddition), 0, 0f);
                            PlaySoundBank("Bazooka", 1, 1f);
                        }
                    }
                    break;
                case 148: case 149: case 150: case 151:
                    if(GS.ReceiveButtonPress("Reload", "Hold") > 0f && CantUseItem <= 0f){
                        rotBuild += 45f;
                        CantUseItem = 0.2f;
                    }

                    string[] styles = {};
                    switch(currID){
                        case 149:
                            styles = new[]{"WoodenWall", "WoodenFloor", "WoodenRamp"};
                            maxBuildAngle = 180f;
                            plant = false;
                            break;
                        case 151:
                            styles = new[]{"MetalWall", "MetalFloor", "MetalRamp"};
                            maxBuildAngle = 180f;
                            plant = false;
                            break;
                        default:
                            currBuild = GS.itemCache[currID].getName();
                            plant = true;
                            maxBuildAngle = 10f;
                            break;
                    }
                    if(styles.Length > 0){
                        if(styleBuild > styles.Length) styleBuild = 0;
                        currBuild = styles[styleBuild];
                        if(GS.ReceiveButtonPress("AltAction", "Hold") > 0f && CantUseItem <= 0f){
                            styleBuild = (styleBuild+1) % styles.Length;
                            CantUseItem = 0.5f;
                        }
                    }
                    break;
                default:
                    break;
            }
            // Specifics for held items

        }

    }

    public void EquipmentFunctions(string EquipmentToSet){

        // Set Inventory from text
        if (EquipmentToSet != "") {

            for (int SetEq = 0; SetEq <= 3; SetEq++) {
                //string RIS = EquipmentToSet.Substring(SetEq * 12, 12);
                //Equipment[SetEq] = new Vector4(float.Parse(RIS.Substring(0, 3)), float.Parse(RIS.Substring(3, 3)), float.Parse(RIS.Substring(6, 3)), float.Parse(RIS.Substring(9, 3)));
                string[] EqInv = GS.ListSemiClass(EquipmentToSet, "/");
                for(int Check = 0; Check <= 3; Check++){
                    if(Check < EqInv.Length) Equipment[Check] = EqInv[Check];
                    else Equipment[Check] = "id0;";
                }
                //Equipment = GS.ListSemiClass(EquipmentToSet, "/");
            }

        } else {

            EquipmentText = "";
            for (int SetEq = 0; SetEq <= 3; SetEq++) {
                EquipmentText += Equipment[SetEq] + "/";
            }

            // Scan equipment
            int MIStoset = 4;
            if (GS.GameModePrefab.x == 1) {
                MIStoset = 6;
            }
            float Healthtoset = 100f;
            float SpeedMultip = 7f;
            float SwimMultip = 7f;
            bool SetST = false;
            bool SetNV = false;
            bool SetHZ = false;
            int MoveDownPos = -1;
            for (int ScanEq = 0; ScanEq < 4; ScanEq ++) {
                if (MoveDownPos != -1 && GS.GetSemiClass(Equipment[ScanEq], "id") != "0") {
                    Equipment[MoveDownPos] = Equipment[ScanEq];
                    Equipment[ScanEq] = "id0;";
                    ScanEq = MoveDownPos;
                    MoveDownPos = -1;
                }
                if (GS.GetSemiClass(Equipment[ScanEq], "id") == "0") {
                    MoveDownPos = ScanEq;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "45") {
                    MIStoset += 1;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "46") {
                    MIStoset += 2;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "47") {
                    MIStoset += 4;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "48") {
                    Healthtoset = 175f;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "49") {
                    Healthtoset = 250f;
                    MIStoset += 4;
                    SpeedMultip -= 3f;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "51") {
                    SpeedMultip += 3f;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "53") {
                    if (GS.GetSemiClass(Equipment[ScanEq], "tr") != "1"){//(int)Equipment[ScanEq].z == 0) {
                        SetNV = false;
                    } else {
                        SetNV = true;
                        Equipment[ScanEq] = GS.SetSemiClass(Equipment[ScanEq], "va", "/+-0.011");//Equipment[ScanEq].y -= 0.04f;
                    }
                    if (float.Parse(GS.GetSemiClass(Equipment[ScanEq], "va"), CultureInfo.InvariantCulture) <= 0f) {
                        SetNV = false;
                        Equipment[ScanEq] = "id0;";
                    }
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "86") {
                    SetHZ = true;
                    if (float.Parse(GS.GetSemiClass(Equipment[ScanEq], "va"), CultureInfo.InvariantCulture) > 0f) {
                        if(MicroSiverts[0] > 0f)
                            Equipment[ScanEq] = GS.SetSemiClass(Equipment[ScanEq], "va", "/+-0.02" );//Equipment[ScanEq].y -= MicroSiverts[0] / 500f;
                        if (Fire > 0f) {
                            Equipment[ScanEq] = GS.SetSemiClass(Equipment[ScanEq], "va", "/+-" + Fire.ToString());//Equipment[ScanEq].y -= Fire;
                            Fire = 0f;
                        }
                    } else {
                        SetHZ = false;
                        Equipment[ScanEq] = "id0;";
                    }
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "94") {
                    if (RS.GotTerrain != null) {
                        if (RS.GotTerrain.GetComponent<BiomeInfo>() != null) {
                            if (RS.GotTerrain.GetComponent<BiomeInfo>().BiomeName[0] == "Snowy Area") {
                                Coldness = Mathf.Clamp(Coldness - 0.04f, -1f, 100f);
                            } else {
                                Hot = Mathf.Clamp(Hot + 0.04f, 0f, 100f); 
                            }
                        }
                    }
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "994") {
                    Healthtoset = 300f;
                    MIStoset += 6;
                    SpeedMultip += 3f;
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "125") {
                    SetST = true;
                    if (float.Parse(GS.GetSemiClass(Equipment[ScanEq], "va"), CultureInfo.InvariantCulture) > 0f) {
                        Equipment[ScanEq] = GS.SetSemiClass(Equipment[ScanEq], "va", "/+-0.00666");//Equipment[ScanEq].y -= 0.00666f;
                        Oxygen[1] = 300f;
                        Oxygen[0] = float.Parse(GS.GetSemiClass(Equipment[ScanEq], "va"), CultureInfo.InvariantCulture) * 3f;
                    } else {
                        SetST = false;
                        Equipment[ScanEq] = "id0;";
                    }
                } else if (GS.GetSemiClass(Equipment[ScanEq], "id") == "126") {
                    SwimMultip += 7f;
                }
            }

            MaxInventorySlots = Mathf.Clamp(MIStoset, 1, 10);
            Speed = SpeedMultip;
            SwimmingSpeed = SwimMultip;
            if (Healthtoset != Health[1]) {
                float healthratio = Healthtoset / Health[1];
                Health[1] = Healthtoset;
                Health[0] *= healthratio;
            }

            // NV
            if (IsNV == false && SetNV == true) {
                IsNV = true;
                MainCanvas.Flash(new Color32(255, 255, 255, 128), new float[]{0.5f, 0.5f});
                MainCanvas.PlaySoundBank("S_NVON");
            } else if (IsNV == true && SetNV == false) {
                IsNV = false;
                MainCanvas.PlaySoundBank("S_NVOFF");
            }

            // HZ
            if (IsHS == false && SetHZ == true) {
                IsHS = true;
                MainCanvas.Flash(new Color32(0, 0, 0, 255), new float[]{1f, 1f});
                SetArmModel("HazmatSuit", false);
            } else if (IsHS == true && SetHZ == false) {
                IsHS = false;
                MainCanvas.Flash(new Color32(0, 0, 0, 255), new float[]{1f, 1f});
                SetArmModel(Shirt, false);
            }

            // ST
            if (IsST == false && SetST == true) {
                IsST = true;
                MainCanvas.Flash(new Color32(0, 0, 0, 255), new float[]{1f, 1f});
            } else if (IsST == true && SetST == false) {
                IsST = false;
                MainCanvas.Flash(new Color32(0, 0, 0, 255), new float[]{1f, 1f});
                Oxygen[1] = 30f;
            }

            if(!IsST && IsSwimming == true) {MainCanvas.CameraBlur = 7f;}

        }

    }

    void BuildingFunctions(){
        bool avaBuild = true;

        if(currBuild != ""){

            if(State != 1) currBuild = "";

            // Place check
            objBuild.gameObject.SetActive(true);

            RaycastHit hitBuild;
            DestructionScript potAnchor = null;
            if(Physics.Raycast(LookDir.position, LookDir.forward, out hitBuild, Mathf.Infinity)){
                posBuild = hitBuild.point;
                if (plant) objBuild.up = hitBuild.normal; else objBuild.up = Vector3.up;
                if(Vector3.Angle(Vector3.up, hitBuild.normal) > maxBuildAngle) avaBuild = false;
                if(hitBuild.collider.GetComponent<DestructionScript>()) potAnchor = hitBuild.collider.GetComponent<DestructionScript>();
                else if (hitBuild.collider.gameObject.layer == 24) potAnchor = hitBuild.collider.transform.parent.GetComponent<DestructionScript>();
            } else {
                objBuild.up = Vector3.up;
                avaBuild = false;
            }

            objBuild.position = posBuild;
            objBuild.Rotate(Vector3.up * rotBuild);

            // Create building
            if(!avaBuild) buildMat.color = new Color32(100, 0, 0, 255);
            else buildMat.color = new Color32(0, 100, 0, 255);

            if(objBuild.GetChild(0).name != currBuild || !objBuild.GetChild(0).gameObject.activeInHierarchy){
                int thir = -1;
                for(int setBuild = 0; setBuild <= objBuild.childCount; setBuild++) 
                    if (setBuild == objBuild.childCount) {
                        if(thir == -1) Debug.LogError("No building of name " + currBuild + " has been found!");
                        else objBuild.GetChild(thir).SetSiblingIndex(0);
                    } else if (objBuild.GetChild(setBuild).gameObject.name == currBuild){
                        thir = setBuild;
                        if(!objBuild.GetChild(setBuild).gameObject.activeInHierarchy) objBuild.GetChild(setBuild).gameObject.SetActive(true);
                    } else {
                        if(objBuild.GetChild(setBuild).gameObject.activeInHierarchy) objBuild.GetChild(setBuild).gameObject.SetActive(false);
                    }
            }

            if(GS.ReceiveButtonPress("Action", "Hold") > 0f && CantUseItem <= 0f && avaBuild){
                for(int gp = 0; gp <= Buildings.Length; gp++) if (Buildings[gp].name == currBuild) {
                    GameObject newBuild = Instantiate(Buildings[gp]);
                    newBuild.transform.position = objBuild.GetChild(0).position;
                    newBuild.transform.eulerAngles = objBuild.GetChild(0).eulerAngles;
                    if(potAnchor && newBuild.GetComponent<DestructionScript>()) potAnchor.Anchor(newBuild.GetComponent<DestructionScript>());
                    switch(currBuild){
                        default: break;
                    }
                    break;
                }
                CantUseItem = 0.5f;
                InvGet(CurrentItemHeld.ToString(), 1);
                currBuild = "";
            }
        } else {
            objBuild.gameObject.SetActive(false);
        }

    } 

    public void Buffs(string BuffsToSet){
    
        if (BuffsToSet != "") {

            for (int CheckCode = 0; CheckCode <= BuffsToSet.Length - 4; CheckCode += 4) {
                string CheckedCode = BuffsToSet.Substring(CheckCode, 4);
                if (CheckedCode.Substring(0, 1) == "B") {
                    Bleeding = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "H") {
                    Hydration = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "I") {
                    Infection = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "T") {
                    Tiredness = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "R") {
                    Radioactivity = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "C") {
                    Coldness = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "A") {
                    Adrenaline = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "D") {
                    Drunkenness = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "b") {
                    BrokenBone = int.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "W") {
                    Wet = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "h") {
                    Hot = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                } else if (CheckedCode.Substring(0, 1) == "F") {
                    Fire = float.Parse(CheckedCode.Substring(1), CultureInfo.InvariantCulture);
                }
            }

        } else {

            string TextShortcuts = "";
            if (Bleeding > 0f) {
                if (IsCasual){
                    Hurt(Bleeding / 4f, "Bleeding", false, Vector3.zero);
                    Bleeding = 0f;
                } else {
                    Bleeding -= 0.02f;
                    Hurt(0.02f, "Bleeding", false, Vector3.zero);
                    string NumbersToAdd = "00" + (int)Bleeding;
                    NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                    TextShortcuts += "B" + NumbersToAdd;
                }
            }
            if (Hydration > 0f) {
                Hydration -= 0.02f;
                EnergyRegen -= 0.02f;
                string NumbersToAdd = "00" + (int)Hydration;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "H" + NumbersToAdd;
            }
            if (Infection > 0f) {
                Health[0] = Mathf.Clamp(Health[0], 0f, Health[1] - (Health[1] * (Infection / 100f)));
                string NumbersToAdd = "00" + (int)Infection;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "I" + NumbersToAdd;
            }
            Tiredness = Mathf.Clamp(Tiredness, 0f, 75f);
            if (Tiredness > 0f) {
                if (IsCasual) {
                    Tiredness = 0;
                } else {
                    Energy[0] = Mathf.Clamp(Energy[0], 0f, Energy[1] - (Energy[1] * (Tiredness / 100f)));
                    string NumbersToAdd = "00" + (int)Tiredness;
                    NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                    TextShortcuts += "T" + NumbersToAdd;
                }
            }

            if (RS.GotTerrain != null) {
                if (RS.GotTerrain.GetComponent<BiomeInfo>() != null) {
                    if (RS.GotTerrain.GetComponent<BiomeInfo>().BiomeName[0] == "Snowy Area" && Coldness > -1f) {
                        Coldness += 0.03f;
                    }
                }
            }
            
            if (Coldness > 0f) {
                if (Coldness > 50f) {
                    Hurt(0.02f, "Cold", false, Vector3.zero);
                }
                Coldness = Mathf.Clamp(Coldness, 0f, 100f);
                Coldness -= 0.02f;
                string NumbersToAdd = "00" + (int)Coldness;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "C" + NumbersToAdd;
            } else if (Coldness < -1f) {
                Coldness += 0.02f;
            }
            if (Adrenaline > 0f) {
                Energy[0] = Energy[1];
                Adrenaline = Mathf.Clamp(Adrenaline, 0f, 100f);
                Adrenaline -= 0.02f;
                string NumbersToAdd = "00" + (int)Adrenaline;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "A" + NumbersToAdd;
            }

            MicroSiverts[0] = Mathf.MoveTowards(MicroSiverts[0], MicroSiverts[1], 0.025f);
            if (IsHS == false && Radioactivity > -1f) {
                Radioactivity += MicroSiverts[0] / 50f;
            }
            for (int PickGeiger = 0; PickGeiger <= 4; PickGeiger ++) {
                if (MicroSiverts[0] > PickGeiger * 2f && MicroSiverts[0] <= (PickGeiger + 1) * 2f) {
                    if (GeigerCounter[PickGeiger].isPlaying == false) {
                        GeigerCounter[PickGeiger].Play();
                    }
                } else {
                    if (GeigerCounter[PickGeiger].isPlaying == true) {
                        GeigerCounter[PickGeiger].Stop();
                    }
                }
            }

            if (Radioactivity > 0f) {
                Radioactivity = Mathf.Clamp(Radioactivity, 0f, 100f);
                Radioactivity -= 0.02f;
                Hurt(0.001f * Radioactivity, "Radioactivity", false, Vector3.zero);
                string NumbersToAdd = "00" + (int)Radioactivity;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "R" + NumbersToAdd;
            } else if (Radioactivity < -1f) {
                Radioactivity += 0.02f;
                MicroSiverts[0] = 0f;
            }
            Drunkenness = Mathf.Clamp(Drunkenness, 0f, 100f);
            if (Drunkenness > 0f) {
                Drunkenness -= 0.02f;
                if (Coldness > 0f) {
                    Coldness -= 0.06f;
                }
                string NumbersToAdd = "00" + (int)Drunkenness;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "D" + NumbersToAdd;
            }
            if (BrokenBone == 1) {
                if (IsCasual) BrokenBone = 0;
                string NumbersToAdd = "00" + (int)BrokenBone;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "b" + NumbersToAdd;
            }

            if (RS.Weather == 4) {
                Ray CheckForRoof = new Ray(this.transform.position, Vector3.up);
                RaycastHit CheckForRoofHIT;
                if (Physics.Raycast(CheckForRoof, out CheckForRoofHIT, Mathf.Infinity)) {
                    if (Raining.GetComponent<ParticleSystem>().isPlaying == true) {
                        Raining.GetComponent<ParticleSystem>().Stop();
                    }
                } else {
                    if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") != "85") {
                        Wet += 0.03f;
                    }
                    DropWater -= Mathf.Lerp(0.01f, 0.1f, Mathf.Clamp(QualitySettings.GetQualityLevel() / 2f, 0f, 1f));
                    if (Raining.GetComponent<ParticleSystem>().isPlaying == false) {
                        Raining.GetComponent<ParticleSystem>().Play();
                        ParticleSystem.MainModule SetCol = Raining.GetComponent<ParticleSystem>().main;
                        SetCol.startColor = MainCamera.GetComponent<Camera>().backgroundColor * 0.75f;
                    }
                }
            }

            if (Wet > 0f) {
                Wet = Mathf.Clamp(Wet, 0f, 100f);
                Wet -= 0.02f;
                if (Coldness > 25f) {
                    Hurt(0.01f, "Cold", false, Vector3.zero);
                }
                string NumbersToAdd = "00" + (int)Wet;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "W" + NumbersToAdd;
            }

            if (DropWater <= 0f) {
                DropWater = 1f;
                GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "WaterDrop";
                CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = MainCamera.GetComponent<Camera>().backgroundColor;
            }

            if (RS.GotTerrain != null) {
                if (RS.GotTerrain.GetComponent<BiomeInfo>() != null) {
                    if (RS.GotTerrain.GetComponent<BiomeInfo>().BiomeName[0] == "Desert" && RS.TimeOfDay[0] != 0 && Hot > -1f && Hydration <= 0f && Wet <= 0f) {
                        Hot += 0.03f;
                    }
                }
            }
            
            if (Hot > 0f) {
                if (Hot > 25f) {
                    Hurt(0.01f, "Hot", false, Vector3.zero);
                }
                Hot = Mathf.Clamp(Hot, 0f, 100f);
                Hot -= 0.02f;
                if (Wet > 0f) {
                    Hot -= 0.02f;
                }
                if (Hydration > 0f) {
                    Hot -= 0.02f;
                }
                string NumbersToAdd = "00" + (int)Hot;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "h" + NumbersToAdd;
            } else if (Hot < -1f) {
                Hot += 0.02f;
            }

            if (Fire > 0f) {
                FireObj.GetComponent<ParticleSystem>().Play();
                if (FireObj.GetComponent<AudioSource>().isPlaying == false) {
                    FireObj.GetComponent<AudioSource>().Play();
                }
                Hurt(0.04f, "Fire", false, Vector3.zero);
                LookX += Random.Range(Fire / -10f, Fire / 10f);
                LookY += Random.Range(Fire / -10f, Fire / 10f);
                Fire = Mathf.Clamp(Fire, 0f, 100f);
                Fire -= 0.04f;
                if (Wet > 0f) {
                    Fire = 0f;
                }
                if (Hydration > 0f) {
                    Fire -= Hydration;
                    Hydration = 0f;
                }
                string NumbersToAdd = "00" + (int)Fire;
                NumbersToAdd = NumbersToAdd.Substring(NumbersToAdd.Length - 3, 3);
                TextShortcuts += "F" + NumbersToAdd;
            } else {
                FireObj.GetComponent<ParticleSystem>().Stop();
                FireObj.GetComponent<AudioSource>().Stop();
            }

            // Even out cold and hot
            if (Coldness > 0f && Hot > 0f) {
                if (Coldness > Hot) {
                    Coldness -= Hot;
                    Hot = 0f;
                } else if (Hot > Coldness) {
                    Hot -= Coldness;
                    Coldness = 0f;
                } else {
                    Coldness = 0f;
                    Hot = 0f;
                }
            }

            if (Campfire > 0f) {
                Campfire -= 0.02f;
                Coldness = Mathf.Clamp(Coldness - .02f, 0f, 100f);
            }

            if(Fire > 0f && PrevWet != (int)Fire){
                PrevWet = (int)Fire;
                SetArmModel("", false);
                print("Fire - " + PrevWet);
            } else if(Wet > 0f && PrevWet != (int)(Wet / 5f)){
                PrevWet = (int)(Wet / 5f);
                SetArmModel("", false);
                print("Wet - " + PrevWet);
            } else if (Wet <= 0f && Fire <= 0f && PrevWet > 0f){
                PrevWet = 0f;
                SetArmModel("", false);
                print("Should be clean now - " + PrevWet);
            }

            BuffsText = TextShortcuts;

        }

    }

    public void Hurt(float GotDamage, string DamageType, bool Indicate, Vector3 DamageDirection){

        if (Health[0] > 0f && RS.RoundState != "TealState" && State == 1) {
            if (DamageType == "Melee" && ItemsShown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(PlayItemAnim("Parry", GS.GetSemiClass(Inventory[CurrentItemHeld], "id"))) ) {
                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-" + ParryingDamage.ToString()); //Inventory[CurrentItemHeld].y -= ParryingDamage;
                GameObject Blood = Instantiate(EffectPrefab) as GameObject;
                Blood.transform.position = this.transform.position;
                Blood.transform.Rotate(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f));
                Blood.GetComponent<EffectScript>().EffectName = "BullethitMetal";
            } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "95" && (DamageType == "MutantBite" || DamageType == "MutantSpit" || DamageType == "Melee" || DamageType == "Gun" || DamageType == "Explosion")) {
                int Damage = Random.Range(1, 3);
                if (DamageType == "Explosion") {
                    Damage = 100;
                }
                Inventory[CurrentItemHeld] = GS.SetSemiClass(Inventory[CurrentItemHeld], "va", "/+-" + Damage.ToString()); //Inventory[CurrentItemHeld].y -= Damage;
                GameObject Blood = Instantiate(EffectPrefab) as GameObject;
                Blood.transform.position = this.transform.position;
                Blood.transform.Rotate(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f));
                Blood.GetComponent<EffectScript>().EffectName = "BullethitMetal";
            } else if (GS.GetSemiClass(Inventory[CurrentItemHeld], "id") == "998" && DamageType != "Nuke" && DamageType != "Suicide") {
                GameObject Blood = Instantiate(EffectPrefab) as GameObject;
                Blood.transform.position = this.transform.position;
                Blood.transform.Rotate(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f));
                if (Indicate == true) {
                    Blood.GetComponent<EffectScript>().EffectName = "BloodSplat";
                }
            } else {

                if (Indicate == true) {
                    if (GS.GameModePrefab.x == 1 && !MainCanvas.HintsTold.Contains("Hurt")) {
                        MainCanvas.HintsCooldown.Add("Hurt");
                    }
                    RecoilCam(new Vector3(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f)), 0.5f, 0f);
                    MainCanvas.DamagedPosition = DamageDirection;
                    MainCanvas.DamageIndicator.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    MainCanvas.Flash(new Color32(255, 0, 0, 75), new float[]{0.25f, 0.25f});
                    MainCanvas.CameraBlur = GotDamage / (Health[1] / 2f);
                    GameObject Blood = Instantiate(EffectPrefab) as GameObject;
                    Blood.transform.position = this.transform.position;
                    Blood.transform.Rotate(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f));
                    Blood.GetComponent<EffectScript>().EffectName = "BloodSplat";
                    if (Health[0] < Health[1] / 4f && !MainCanvas.HintsTold.Contains("Dying")) {
                        MainCanvas.HintsCooldown.Add("Dying");
                    }
                }

                bool HardcoreInstaKill = false;
                int CasualEase = 1;
                bool Gib = false;
                if (DamageType == "Nuke") {
                    KilledBy = GS.SetString("You've been obliterated by a nuke.", "Zostałeś rozszarpany przez bombę atomową.");
                } else if (DamageType == "Starvation") {
                    KilledBy = GS.SetString("You've starved to death.", "Umarłeś z głodu.");
                } else if (DamageType == "Canibalism") {
                    KilledBy = GS.SetString("You ate yourself", "Zagryzłeś się na śmierć");
                } else if (DamageType == "Bleeding") {
                    KilledBy = GS.SetString("You've bled to death.", "Wykrwawiłeś się.");
                } else if (DamageType == "Radioactivity") {
                    KilledBy = GS.SetString("You died due to radioactivity.", "Radioaktywność cię wykończyła.");
                } else if (DamageType == "Cold") {
                    KilledBy = GS.SetString("You died due to hypothermia.", "Zmarłeś na hipotermię.");
                } else if (DamageType == "Hot") {
                    KilledBy = GS.SetString("You died due to overheating.", "Zmarłeś na udar.");
                } else if (DamageType == "Fire") {
                    KilledBy = GS.SetString("You've burnt down.", "Spaliłeś się.");
                } else if (DamageType == "Overdose") {
                    KilledBy = GS.SetString("You died due to overdose.", "Umarłeś z przedawkowania.");
                } else if (DamageType == "Drowning") {
                    KilledBy = GS.SetString("You've suffocated.", "Udusiłeś się.");
                } else if (DamageType == "Falling") {
                    KilledBy = GS.SetString("You fell to your death.", "Spadłeś i umarłeś.");
                    HardcoreInstaKill = true;
                    Gib = true;
                    int BoneBreakChance = Random.Range(0, 4);
                    if (BoneBreakChance == 0) {
                        BrokenBone = 1;
                    }
                } else if (DamageType == "Electricity") {
                    KilledBy = GS.SetString("You were electrocuted.", "Poraził cię prąd.");
                    HardcoreInstaKill = true;
                    CasualEase = 4;
                    Gib = true;
                } else if (DamageType == "Suicide") {
                    KilledBy = GS.SetString("You gave up and died.", "Poddałeś się i umarłeś.");
                    Health[0] = 0f;
                    Gib = true;
                } else if (DamageType == "MutantBite") {
                    KilledBy = GS.SetString("You've been eaten by mutants.", "Zostałeś pożarty przez mutantów.");
                    HardcoreInstaKill = true;
                    CasualEase = 2;
                    int InfectionChance = Random.Range(0, 5);
                    if (InfectionChance == 0) {
                        Infection += Random.Range(5f, 10f) * int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?"));
                    }
                } else if (DamageType == "MutantSpit") {
                    KilledBy = GS.SetString("You were melted by an acid mutant.", "Zostałeś zabity przez trującego mutanta.");
                    GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "AcidSplash";
                    HardcoreInstaKill = true;
                    CasualEase = 2;
                    int InfectionChance = Random.Range(0, 2);
                    if (InfectionChance == 0) {
                        Infection += Random.Range(10f, 25f) * int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?"));
                    }
                } else if (DamageType == "Melee") {
                    KilledBy = GS.SetString("You died in a melee fight.", "Umarłeś zabity z broni białej.");
                    HardcoreInstaKill = true;
                    CasualEase = 2;
                    int InfectionChance = Random.Range(0, 10);
                    if (InfectionChance == 0) {
                        Bleeding += Random.Range(5f, 20f) * int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?"));
                    }
                } else if (DamageType == "Gun" || DamageType == "Arrow") {
                    KilledBy = GS.SetString("You were shot.", "Zostałeś zastrzelony.");
                    HardcoreInstaKill = true;
                    CasualEase = 4;
                    int InfectionChance = Random.Range(0, 10);
                    if (InfectionChance == 0) {
                        Bleeding += Random.Range(5f, 20f) * int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?"));
                    }
                } else if (DamageType == "Explosion") {
                    KilledBy = GS.SetString("You blew up.", "Zostałeś wysadzony w powietrze.");
                    HardcoreInstaKill = true;
                    CasualEase = 4;
                    Gib = true;
                    int InfectionChance = Random.Range(0, 2);
                    if (InfectionChance == 0) {
                        Bleeding += Random.Range(1f, 5f) * int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?"));
                    }
                } else if (DamageType == "BarbedWire") {
                    KilledBy = GS.SetString("You were caught in barbed wire.", "Utknąłeś w drucie kolczastym.");
                    HardcoreInstaKill = true;
                    CantMove = Mathf.Clamp(5f, CantMove, Mathf.Infinity);
                    Bleeding += Random.Range(5f, 20f) * int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?"));
                } else {
                    KilledBy = GS.SetString("You died.", "Umarłeś.");
                }

                if (IsCasual)
                    GotDamage /= CasualEase;

                if(DamageType == "Canibalism"){
                    Health[0] -= GotDamage;
                    if(Health[0] <= 0f) GS.PS.AchProg("Ach_StarveDeath", "0");
                } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "1") {
                    Health[0] -= GotDamage * 0.5f;
                } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "2") {
                    Health[0] -= GotDamage;
                } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "3") {
                    Health[0] -= GotDamage * 2f;
                } else {
                    Health[0] -= GotDamage * 4f;
                }

                if (HardcoreInstaKill == true && GS.GetSemiClass(GS.RoundSetting, "D", "?") == "5") {
                    Health[0] = 0f;
                }

                if (Gib == true && Health[0] <= 0f) {
                    GameObject GibMe = Instantiate(EffectPrefab) as GameObject;
                    GibMe.transform.position = this.transform.position;
                    GibMe.GetComponent<EffectScript>().EffectName = "Gibs";
                    GibMe.GetComponent<EffectScript>().Gibs = Gibs;
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-4f, 4f), 4f, Random.Range(-4f, 4f)), ForceMode.VelocityChange);
                    this.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)), ForceMode.VelocityChange);
                }

            }

        }

    }

    public void ShakeCam(float Power, float ShakeTime){
        CameraShakeForce = new float[] { Power, ShakeTime, ShakeTime };
    }

    public void RecoilCam(Vector3 Power, float RecoilTime, float Delay) {
        CameraRecoil = Power;
        CameraRecoilVars = new float[] { RecoilTime, Mathf.Clamp(RecoilTime, 0.01f, Mathf.Infinity), Delay, Mathf.Clamp(Delay, 0.01f, Mathf.Infinity) };
    }

    public void PlaySoundBank(string SoundName, int Priority, float pitch = 1f, float From = 0f, string Override = "") {

        // Override methods: "" - play normaly "ifnot" - play if isn't playing "overrride" - stop all that are of the same clip "only" - stop all others

        int PlayThisone = -1;
        int LastOfSamePriority = -1;

        for(int PSB = 0; PSB < Soundbank.childCount; PSB++){
            AudioSource CheckSB = Soundbank.GetChild(PSB).GetComponent<AudioSource>();
            if(CheckSB.isPlaying == false){
                if(PlayThisone == -1) PlayThisone = PSB;
            } else if (int.Parse(CheckSB.name.Substring(2)) <= Priority){
                LastOfSamePriority = PSB;
            }
            if(Override == "Only" || (Override == "Override" && CheckSB.clip && CheckSB.clip.name == SoundName)) CheckSB.Stop();
            else if (Override == "ifnot" && CheckSB.clip && CheckSB.clip.name == SoundName) {PlayThisone = -1; LastOfSamePriority = -1;}
        }

        if(PlayThisone == -1 && LastOfSamePriority > -1) PlayThisone = LastOfSamePriority;

        if(PlayThisone != -1)
        foreach (AudioClip FoundSound in SoundBankAudios) {
            if (FoundSound.name == SoundName) {
                Soundbank.GetChild(PlayThisone).name = "S_" + Priority;
                Soundbank.GetChild(PlayThisone).GetComponent<AudioSource>().clip = FoundSound;
                Soundbank.GetChild(PlayThisone).GetComponent<AudioSource>().pitch = pitch;
                Soundbank.GetChild(PlayThisone).GetComponent<AudioSource>().Play();
                Soundbank.GetChild(PlayThisone).GetComponent<AudioSource>().time = From;
            }
        }

    }

    public void SwimmingStance(bool Swim) {

        if (Swim == true) {

            IsSwimming = true;
            this.transform.position -= Vector3.up * 2f;
            this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x, -0.25f, this.GetComponent<Rigidbody>().velocity.z);
            MainCanvas.Flash(Color.black, new float[]{1f, 1f});
            MainCanvas.PlaySoundBank("S_SwimmingStart");
            CantMove = 1f;

        } else {

            IsSwimming = false;
            this.transform.position += Vector3.up * 2f;
            MainCanvas.Flash(new Color32(255, 255, 255, 155), new float[]{0.5f, 0.5f});
            MainCanvas.PlaySoundBank("S_SwimmingStop");

            if (State == 1) {
                for (int Debris = Random.Range(5, 10); Debris > 0; Debris--) {
                    GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "WaterDrop";
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = RenderSettings.fogColor;
                }
            }

        }

    }

    void SetArmModel(string ArmModel, bool RemoveNotNeeded) {

        if(ArmModel == "") ArmModel = PrevShirt;
        else if (ArmModel != PrevShirt) PrevShirt = ArmModel;

        float WetDark = 0f;
        if(Fire > 0f){
            WetDark = Mathf.Clamp(Fire / 10f, 0f, 10f);
        } else if (Wet > 0f){
            WetDark = Mathf.Clamp(Wet / 50f, 0f, 1f);
        }

        Color32 SkinColorA = GS.PS.SkinColors[SkinColor];

        // Set hand colors
        GameObject[] Hands = new GameObject[] { ItemsShown.transform.GetChild(1).GetChild(0).gameObject, ItemsShown.transform.GetChild(2).GetChild(0).gameObject };
        for (int GitArm = 1; GitArm <= 2; GitArm++) {
            for (int InHand = 0; InHand < 5; InHand ++) {
                for (int SetHandColor = 0; SetHandColor < 5; SetHandColor++) {
                    Material MatRef = Hands[GitArm - 1].transform.GetChild(InHand).GetComponent<MeshRenderer>().materials[SetHandColor];
                    switch (ArmModel) {
                        case "ClassicClothes":
                            switch (MatRef.name) {
                                case "HAND (Instance)": case "LOWER (Instance)": case "FINGERS (Instance)":
                                    MatRef.color = new Color32(110, 130, 130, 255);
                                    MatRef.color *= 1f - WetDark;
                                    break;
                                default:
                                    MatRef.color = SkinColorA;
                                    break;
                            }
                            break;
                        case "HazmatSuit":
                            switch (MatRef.name) {
                                case "TIPS2 (Instance)":
                                    MatRef.color = new Color32(255, 240, 124, 255);
                                    break;
                                default:
                                    MatRef.color = new Color32(55, 55, 55, 255);
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        // Set arm models
        for (int GetArm = 1; GetArm <= 2; GetArm ++) {
            foreach (Transform GetModel in ItemsShown.transform.GetChild(GetArm).GetChild(1)) {
                if (GetModel.name == ArmModel) {
                    GetModel.gameObject.SetActive(true);
                    GetModel.GetComponent<ArmTextureControl>().Set(WetDark);
                } else {
                    if(RemoveNotNeeded && !GetModel.GetComponent<ArmTextureControl>().Needed){
                        Destroy(GetModel.gameObject);
                    } else {
                        GetModel.gameObject.SetActive(false);
                    }
                }
            }
        }

    }

    public string PlayItemAnim(string WhatAnim, string ItemID, string Aditional = "") {

        string PlayThis = "";
        if(WhatAnim == "Throw" || WhatAnim == "PickUpA" || WhatAnim == "PickUpB"){
            PlayThis = WhatAnim;
            if(WhatAnim == "Throw"){
                RecoilCam(new Vector3(25f, -15f, -10f), 0.3f, 0.1f);
            } else if (WhatAnim == "PickUpA" || WhatAnim == "PickUpB"){
                RecoilCam(new Vector3(0f, -5f, -2f), 0.2f, 0.1f);
            }
            TempItemShown = new float[]{ 0f, 0.3f };
        } else {
            int ItemIDint = int.Parse(ItemID);
            switch (ItemIDint) {
            case 1: case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10: case 17: case 18: case 20: case 21: case 23: case 25: case 26: case 45: case 991: case 992: case 53: case 63: case 70: case 71: case 73: case 74: case 75: case 76: case 77: case 78: case 79: case 82: case 83: case 84: case 88: case 89: case 99: case 94: case 98: case 102: case 103: case 104: case 107: case 117: case 119: case 120: case 121: case 141: case 142: case 143: case 144: case 145: case 147: case 158: case 161:
                PlayThis = "Hold-" + WhatAnim;
                break;
            case 12: case 19: case 22: case 30: case 33: case 37: case 39: case 43: case 52: case 54: case 72: case 80: case 81: case 85: case 90: case 97: case 100: case 101: case 105: case 114: case 116: case 123: case 124:
                PlayThis = "Grab-" + WhatAnim;
                break;
            case 50: case 118: case 125: case 140:
                PlayThis = "Bag-" + WhatAnim;
                break;
            case 13:
                PlayThis = "Flare-" + WhatAnim;
                break;
            case 998:
                PlayThis = "Ring-" + WhatAnim;
                break;
            case 95:
                PlayThis = "Shield-" + WhatAnim;
                break;
            case 11: case 93: case 106:
                PlayThis = "Handle-" + WhatAnim;
                break;
            case 46: case 47: case 48: case 49: case 994: case 51: case 86: case 126: case 999:
                PlayThis = "Clothing-" + WhatAnim;
                break;
            case 24: case 44: case 990:
                PlayThis = "Syringe-" + WhatAnim;
                break;
            case 29: case 31: case 32: case 135: case 58: case 130:
                PlayThis = "Pistol-" + WhatAnim;
                if(ItemIDint == 32 && WhatAnim == "SwitchItem") PlayThis = "Revolver-" + WhatAnim;
                else if(ItemIDint == 58 && (WhatAnim == "SwitchItem")) PlayThis = "Uzi-" + WhatAnim;
                if(Aditional == "Grip") PlayThis = "GripPistol-" + WhatAnim;
                break;
            case 34: case 56: case 65: case 113: case 159: case 160:
                PlayThis = "BoltAction-" + WhatAnim;
                if(WhatAnim == "SwitchItem"){
                    if((ItemIDint == 56 || ItemIDint == 113)) PlayThis = "Garand-" + WhatAnim;
                    else if (ItemIDint == 159 || ItemIDint == 160) PlayThis = "BakerRifle-" + WhatAnim;
                }
                if(Aditional == "Grip") PlayThis = "GripBoltAction-" + WhatAnim;
                if(WhatAnim == "Trigger") PlayThis = Aditional + "BakerRifle-" + WhatAnim;
                else if(Aditional == "Bayonet" && PlayThis == "BoltAction-Shoot") PlayThis = "BoltAction-ShootNoReload";
                break;
            case 35: case 997:
                PlayThis = "DBshotgun-" + WhatAnim;
                break;
            case 36: case 41:
                PlayThis = "Thompson-" + WhatAnim;
                if(Aditional == "Grip") PlayThis = "Grip" + PlayThis;
                break;
            case 38: case 42: case 57: case 59: case 60: case 62: case 137: case 996:
                PlayThis = "AR-" + WhatAnim;
                if((ItemIDint == 38 || ItemIDint == 60 || ItemIDint == 137 || ItemIDint == 996) && (WhatAnim == "SwitchItem")) PlayThis = "AK-" + WhatAnim;
                else if(ItemIDint == 42 && (WhatAnim == "SwitchItem")) PlayThis = "M4-" + WhatAnim;
                else if(ItemIDint == 57 && (WhatAnim == "SwitchItem")) PlayThis = "FAMAS-" + WhatAnim;
                else if(ItemIDint == 59 && (WhatAnim == "SwitchItem")) PlayThis = "G3-" + WhatAnim;
                else if(ItemIDint == 62 && (WhatAnim == "SwitchItem")) PlayThis = "SAW-" + WhatAnim;
                if(Aditional == "Grip") PlayThis = "GripAR-" + WhatAnim;
                break;
            case 40: case 61:
                PlayThis = "Shotgun-" + WhatAnim;
                break;
            case 55:
                PlayThis = "Sten-" + WhatAnim;
                if(Aditional == "Grip") PlayThis = "GripThompson-" + WhatAnim;
                break;
            case 64:
                PlayThis = "Minigun-" + WhatAnim;
                break;
            case 68:
                PlayThis = "Chainsaw-" + WhatAnim;
                if (WhatAnim == "Swing") {
                    PlayThis = "Chainsaw-Attack";
                    ShakeCam(0.1f, 0.1f);
                }
                break;
            case 157:
                PlayThis = "Flintlock-" + WhatAnim;
                break;
            case 134: case 993:
                PlayThis = "Spear-" + WhatAnim;
                if(WhatAnim == "Swing"){
                    RecoilCam(new Vector3(15f, -5f, 0f), 0.25f, 0.1f);
                }
                break;
            case 2: case 127:
                PlayThis = "Flashlight-" + WhatAnim;
                if(WhatAnim == "Swing")
                    RecoilCam(new Vector3(-10f, -25f, 0f), 0.25f, 0.1f);
                break;
            case 14:
                PlayThis = "Knife-" + WhatAnim;
                if(WhatAnim == "Swing"){
                    PlayThis = "Knife-" + WhatAnim + (int)Random.Range(1f, 4.9f);
                    switch(PlayThis){
                        case "Knife-Swing1": case "Knife-Swing3":
                            RecoilCam(new Vector3(-5f, -10f, 0f), 0.25f, 0.1f);
                            break;
                        case "Knife-Swing2":
                            RecoilCam(new Vector3(-5f, 10f, 0f), 0.25f, 0.1f);
                            break;
                        case "Knife-Swing4":
                            RecoilCam(new Vector3(10f, 0f, 0f), 0.25f, 0.1f);
                            break;
                    }
                }
                break;
            case 15: case 27: case 108: case 136: case 152: case 153: case 154: case 156:
                PlayThis = "MOH-" + WhatAnim;
                if(WhatAnim == "Swing"){
                    PlayThis = "MOH-" + WhatAnim + (int)Random.Range(1f, 4.9f);
                    switch(PlayThis){
                        case "MOH-Swing1":
                            RecoilCam(new Vector3(10f, -5f, 0f), 0.25f, 0.1f);
                            break;
                        case "MOH-Swing2":
                            RecoilCam(new Vector3(-5f, -10f, 0f), 0.25f, 0.1f);
                            break;
                        case "MOH-Swing3":
                            RecoilCam(new Vector3(-5f, 10f, 0f), 0.25f, 0.1f);
                            break;
                        case "MOH-Swing4":
                            RecoilCam(new Vector3(-25f, 0f, 0f), 0.25f, 0.1f);
                            break;
                    }
                }
                break;
            case 16: case 28: case 115: case 132: case 138: case 155:
                PlayThis = "MTH-" + WhatAnim;
                if(WhatAnim == "Swing"){
                    PlayThis = "MTH-" + WhatAnim + (int)Random.Range(1f, 4.9f);
                    switch(PlayThis){
                        case "MTH-Swing1": case "MTH-Swing4":
                            RecoilCam(new Vector3(-5f, -10f, 0f), 0.25f, 0.1f);
                            break;
                        case "MTH-Swing2":
                            RecoilCam(new Vector3(-5f, 10f, 0f), 0.25f, 0.1f);
                            break;
                        case "MTH-Swing3":
                            RecoilCam(new Vector3(-25f, 0f, 0f), 0.25f, 0.1f);
                            break;
                    }
                }
                break;
            case 995: case 146:
                PlayThis = "Spark-" + WhatAnim;
                break;
            case 66: case 110: case 131: case 133:
                PlayThis = "Grenade-" + WhatAnim;
                break;
            case 67:
                PlayThis = "Panzerfaust-" + WhatAnim;
                break;
            case 87:
                PlayThis = "Lifebuoy-" + WhatAnim;
                break;
            case 91: case 92:
                PlayThis = "Binoculars-" + WhatAnim;
                break;
            case 109:
                PlayThis = "FlameThrower-" + WhatAnim;
                break;
            case 111:
                PlayThis = "M97-" + WhatAnim;
                break;
            case 112:
                PlayThis = "Crossbow-" + WhatAnim;
                break;
            case 122:
                PlayThis = "Cup-" + WhatAnim;
                break;
            case 128:
                PlayThis = "FireExtinguisher-" + WhatAnim;
                break;
            case 129:
                PlayThis = "FishingRod-" + WhatAnim;
                break;
            case 139:
                PlayThis = "Bazooka-" + WhatAnim;
                break;
            case 69:
                PlayThis = "Bow-" + WhatAnim;
                break;
            default:
                if(WhatAnim == "Parry"){
                    PlayThis = "None-Parry";
                } else {
                    PlayThis = "None";
                }
                break;
            }

            if(WhatAnim != "SwitchItem" && WhatAnim != "Pullup"){
                TempItemShown = new float[]{ ItemIDint, 0.02f };
            }

        }

        return PlayThis;

    }

}
