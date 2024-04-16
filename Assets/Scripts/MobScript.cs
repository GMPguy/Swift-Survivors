using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;
using Random=UnityEngine.Random;

public class MobScript : MonoBehaviour {

    // Main Variables
    public string MobName = "";
    public int[] Squad = new int[] { 0, 0 };
    public int TypeOfMob = 0;
    public string ClassOfMob = "";
    public int State = 0; // 0 Alive   1 Dead
    public float[] MobHealth;
    public float CantDoAnything = 0f;
    public float AttackRange = 1f;
    public float AttackCooldown = 1f;
    public string AttackType = "";
    public float[] DetectionRange;
    public float[] MovementSpeed;
    public GameObject MoveDir;
    // Buffs
    public float Fire = 0f;
    public GameObject FireObj;
    public float Blinded = 0f;
    public bool Plunged = false;
    public GameObject[] Plungers;
    // Buffs
    // Main Variables

    // AiVariables
    public float Angered = 0f;
    public float Curious = 0f;
    public float Panic = 0f;
    public GameObject AiTarget;
    public Vector3 AiPosition;
    public Vector3 AiTargetLastSeen;
    public GameObject CurrentWaypoint;
    int CWBAF = 0;
    float SwitchPosition = 0f;
    int[] Ammo;
    float ReturnToWayPoint = 0f;
    public string mobOrigins = "";
    // AiVariables

    // References
    public GameScript GS;
    public RoundScript RS;
    public GameObject SelectedMobModel;
    public GameObject EyeSight;
    //public GameObject AttackPrefab;
    public GameObject EffectPrefab;
    public GameObject ItemPrefab;
    public GameObject SpecialPrefab;
    public GameObject HordeDropPrefab;
    public GameObject RagdollPrefab;
    public Animator Anim;
    public GameObject SoundBank;
    public AudioClip[] SoundBankClips;
    // Mob Models
    public string AnimationSet;
    public string WhichModels;
    public string WhichSubModels;
    public GameObject Humanoid;
    public GameObject[] HumanoidBodyParts;
    // Mob Models
    // References

    // Misc
    bool Ach_Flare = false;
    public Color32 MobColor;
    public Vector3 PushbackForce;
    public float ReturnPushBack;
    Vector3 PrevPosition;
    Vector3 StartPosition;
    float FootstepCooldown = 0f;
    float CleanupAfterDead = 100f;
    float FixedTimeCooldown = 1f;
    float BurnCooldown = 0f;
    float UpdateDelay = 0f;
    string ReasonOfDeath = "";
    // For dialog options
    public float GeneratedValue = 0;
    public bool ToldPlaces = false;
    public Vector2[] TradeOptions;
    // For dialog options
    string MobAudio = "";
    GameObject leadAggresor;
    bool IsCrouching = false;
    bool InWater = false;
    public bool TooFar = false;
    int WeaponToDrop = -1;
    float Chatter = 0f;
    bool SetTheGodDamnPosition = true;
    float[] PreventJamming = new float[] { 0f, 0f };
    string BasicMutantJob = "";
    int[] ToAttack = new int[] { 0, 1, 1 };
    public float[] GunSpread = new float[] { 0f, 0f };
    float DroppedRagdoll = -1f;
    Vector4 RagdollPush;
    float PowerLevel = 0f;
    // Misc

	// Use this for initialization
	void Start () {

        // Ground
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x, -250f, 250f),
            this.transform.position.y+0.1f,
            Mathf.Clamp(this.transform.position.z, -250f, 250f)
        );
        Ray Ground = new Ray(this.transform.position, Vector3.down);
        RaycastHit GroundHIT;
        if (Physics.Raycast(Ground, out GroundHIT, Mathf.Infinity)) {
            this.transform.position = GroundHIT.point + (Vector3.up * 0.51f);
        }

        StartPosition = this.transform.position;
        AiPosition = StartPosition;
        MobHealth = new float[] { 0f, 0f };
        ToAttack = new int[] { 0, 1, 1 };
        
        if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
            PowerLevel = RS.HordeVariables[2] / 100f;
        } else {
            PowerLevel = RS.DifficultySlider;
        }

        switch((int)TypeOfMob){
            case 1:
                // Basic Mutant
                MobName = GS.SetString("Mutant", "Mutant");
                ClassOfMob = "Mutant";
                int SubClassofMob = (int)Random.Range(0f, 13.9f);
                if (SubClassofMob == 0 || SubClassofMob == 1 || SubClassofMob == 2) {
                    BasicMutantJob = "Normal";
                } else if (SubClassofMob == 3 || SubClassofMob == 4 || SubClassofMob == 5) {
                    BasicMutantJob = "Shirt";
                } else if (SubClassofMob == 6 || SubClassofMob == 7 || SubClassofMob == 8) {
                    BasicMutantJob = "Suit";
                } else if (SubClassofMob == 9) {
                    BasicMutantJob = "Shirtless";
                } else if (SubClassofMob == 10) {
                    BasicMutantJob = "Police";
                } else if (SubClassofMob == 11) {
                    BasicMutantJob = "Builder";
                } else if (SubClassofMob == 12) {
                    BasicMutantJob = "Doctor";
                } else if (SubClassofMob == 13) {
                    BasicMutantJob = "Cook";
                }
                MobHealth = new float[] {
                    Mathf.Lerp(20f, 100f, PowerLevel),
                    Mathf.Lerp(20f, 100f, PowerLevel) };
                MovementSpeed = new float[] {
                    Mathf.Lerp(1f, 1f, PowerLevel),
                    Mathf.Lerp(5f, 10f, PowerLevel) };
                AttackCooldown = 0.5f;
                AttackRange = 1.5f;
                AttackType = "MutantBite";
                DetectionRange = new float[] {
                    Mathf.Lerp(5f, 50f, PowerLevel),
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "BasicMutant";
                MobAudio = "Mutant";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 2:
                // Bandit
                MobName = GS.SetString("Bandit", "Bandyta");
                ClassOfMob = "Bandit";
                int PickClothes = (int)Mathf.Clamp(Random.Range(1f, 6f) * PowerLevel, 1f, 6f);
                if (PickClothes == 1 || PickClothes == 6) {
                    MobHealth = new float[] { 50f, 50f };
                    MovementSpeed = new float[] { 5f, 10f };
                    DetectionRange = new float[] { Mathf.Lerp(25f, 50f, PowerLevel), 50f, 25f };
                    WhichSubModels = "Bandit1";
                } else if (PickClothes == 2) {
                    MobHealth = new float[] { 75f, 75f };
                    MovementSpeed = new float[] { 5f, 15f };
                    DetectionRange = new float[] { Mathf.Lerp(25f, 50f, PowerLevel), 50f, 25f };
                    WhichSubModels = "Bandit2";
                } else if (PickClothes == 3) {
                    MobHealth = new float[] { 125f, 125f };
                    MovementSpeed = new float[] { 5f, 10f };
                    DetectionRange = new float[] { 75f, 75f, 25f };
                    WhichSubModels = "Bandit3";
                } else if (PickClothes == 4) {
                    MobHealth = new float[] { 200f, 200f };
                    MovementSpeed = new float[] { 5f, 10f };
                    DetectionRange = new float[] { Mathf.Lerp(25f, 50f, PowerLevel), 50f, 25f };
                    WhichSubModels = "Bandit4";
                } else if (PickClothes == 5) {
                    MobHealth = new float[] { 200f, 200f };
                    MovementSpeed = new float[] { 5f, 15f };
                    DetectionRange = new float[] { 75f, 75f, 25f };
                    WhichSubModels = "Bandit5";
                }
                int PickWeapon = (int)Mathf.Clamp(Random.Range(1f, 16f) * PowerLevel, 1f, 8f);
                switch(PickWeapon){
                    case 1: case 8:
                        AttackRange = 1.5f;
                        AttackType = "Machete";
                        WeaponToDrop = 27;
                        AttackCooldown = 0.5f;
                        AnimationSet = "HumanoidMelee";
                        break;
                    case 2:
                        AttackRange = 1.5f;
                        AttackType = "BaseballBat";
                        WeaponToDrop = 28;
                        AttackCooldown = 0.75f;
                        AnimationSet = "HumanoidMelee";
                        break;
                    case 3:
                        AttackRange = 25f;
                        AttackType = "Luger";
                        WeaponToDrop = 31;
                        AttackCooldown = 0.25f;
                        Ammo = new int[] { 8, 8 };
                        AnimationSet = "HumanoidPistol";
                        break;
                    case 4:
                        AttackRange = 15f;
                        AttackType = "DBShotgun";
                        WeaponToDrop = 35;
                        AttackCooldown = 1f;
                        Ammo = new int[] { 2, 2 };
                        ToAttack[2] = 3;
                        AnimationSet = "HumanoidGun";
                        break;
                    case 5:
                        AttackRange = 25f;
                        AttackType = "Sten";
                        WeaponToDrop = 55;
                        AttackCooldown = 0.075f;
                        Ammo = new int[] { 32, 32 };
                        ToAttack[1] = 5;
                        AnimationSet = "HumanoidPistol";
                        break;
                    case 6:
                        AttackRange = 25f;
                        AttackType = "AK-47";
                        WeaponToDrop = 38;
                        AttackCooldown = 0.1f;
                        Ammo = new int[] { 30, 30 };
                        ToAttack[1] = 3;
                        AnimationSet = "HumanoidGun";
                        break;
                    case 7:
                        AttackRange = 100f;
                        AttackType = "HunterRifle";
                        WeaponToDrop = 34;
                        AttackCooldown = 1f;
                        Ammo = new int[] { 5, 5 };
                        AnimationSet = "HumanoidGun";
                        break;
                }
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                MobAudio = "Bandit";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 3:
                // Survivor
                this.GetComponent<Interactions>().Icons = new string[] {"TalkTo"};
                this.GetComponent<Interactions>().Options = new string[] {"TalkTo"};
                ClassOfMob = "Survivor";
                GeneratedValue = Random.Range(0f, 1f);
                TradeOptions = new Vector2[] { new Vector2((int)Random.Range(1f, RS.TotalItems.Length - 0.1f), (int)Random.Range(1f, RS.TotalItems.Length - 0.1f)), new Vector2((int)Random.Range(1f, RS.TotalItems.Length - 0.1f), (int)Random.Range(1f, RS.TotalItems.Length - 0.1f)), new Vector2((int)Random.Range(1f, RS.TotalItems.Length - 0.1f), (int)Random.Range(1f, RS.TotalItems.Length - 0.1f)), new Vector2((int)Random.Range(1f, RS.TotalItems.Length - 0.1f), (int)Random.Range(1f, RS.TotalItems.Length - 0.1f)) };
                MobName = GS.SetString("Survivor", "Niedobitek");
                MobHealth = new float[] { 100f, 100f };
                MovementSpeed = new float[] { 5f, 10f };
                DetectionRange = new float[] { 25f, 50f, 25f };
                WhichSubModels = "Survivor";
                int PickWeapon1 = Random.Range(1, 9);
                switch(PickWeapon1){
                    case 1: case 9:
                        AttackRange = 1.5f;
                        AttackType = "Knife";
                        WeaponToDrop = 14;
                        AttackCooldown = 0.5f;
                        AnimationSet = "HumanoidMelee";
                        break;
                    case 2:
                        AttackRange = 25f;
                        AttackType = "Colt";
                        WeaponToDrop = 29;
                        AttackCooldown = 0.25f;
                        Ammo = new int[] { 7, 7 };
                        AnimationSet = "HumanoidPistol";
                        break;
                    case 3:
                        AttackRange = 15f;
                        AttackType = "Thompson";
                        WeaponToDrop = 36;
                        AttackCooldown = 0.075f;
                        Ammo = new int[] { 30, 30 };
                        ToAttack[1] = 5;
                        AnimationSet = "HumanoidPistol";
                        break;
                    case 4:
                        AttackRange = 100f;
                        AttackType = "HunterRifle";
                        WeaponToDrop = 34;
                        AttackCooldown = 1f;
                        Ammo = new int[] { 5, 5 };
                        AnimationSet = "HumanoidGun";
                        break;
                    case 5:
                        AttackRange = 100f;
                        AttackType = "Garand";
                        WeaponToDrop = 56;
                        AttackCooldown = 0.25f;
                        Ammo = new int[] { 8, 8 };
                        AnimationSet = "HumanoidGun";
                        break;
                    case 6:
                        AttackRange = 15f;
                        AttackType = "Uzi";
                        WeaponToDrop = 58;
                        AttackCooldown = 0.075f;
                        Ammo = new int[] { 30, 30 };
                        ToAttack[1] = 5;
                        AnimationSet = "HumanoidPistol";
                        break;
                    case 7:
                        AttackRange = 25f;
                        AttackType = "G3";
                        WeaponToDrop = 59;
                        AttackCooldown = 0.1f;
                        Ammo = new int[] { 20, 20 };
                        AnimationSet = "HumanoidGun";
                        break;
                    case 8:
                        AttackRange = 10f;
                        AttackType = "Arrow";
                        WeaponToDrop = 69;
                        AttackCooldown = 1f;
                        AnimationSet = "HumanoidMelee";
                        break;
                }
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                MobAudio = "";
                MobColor = new Color32(0, 175, 0, 255);
                break;
            case 4:
                // Fast Mutant
                MobName = GS.SetString("Fast Mutant", "Szybki Mutant");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    Mathf.Lerp(10f, 50f, PowerLevel),
                    Mathf.Lerp(10f, 50f, PowerLevel) };
                MovementSpeed = new float[] {
                    Mathf.Lerp(5f, 5f, PowerLevel),
                    Mathf.Lerp(15f, 25f, PowerLevel) };
                AttackCooldown = 0.25f;
                AttackRange = 1.5f;
                AttackType = "MutantBite";
                DetectionRange = new float[] {
                    25f,
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "FastMutant";
                MobAudio = "FastAcidMutant";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 5:
                // Strong Mutant
                MobName = GS.SetString("Strong Mutant", "Silny Mutant");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    Mathf.Lerp(150f, 300f, PowerLevel),
                    Mathf.Lerp(150f, 300f, PowerLevel) };
                MovementSpeed = new float[] {
                    Mathf.Lerp(1f, 1f, PowerLevel),
                    Mathf.Lerp(5f, 10f, PowerLevel) };
                AttackCooldown = 1f;
                AttackRange = 1.5f;
                AttackType = "StrongMutantBite";
                DetectionRange = new float[] {
                    Mathf.Lerp(5f, 25f, PowerLevel),
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "StrongMutant";
                MobAudio = "StrongMutant";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 6:
                // Acid Mutant
                MobName = GS.SetString("Acid Mutant", "Trujący Mutant");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    Mathf.Lerp(20f, 100f, PowerLevel),
                    Mathf.Lerp(20f, 100f, PowerLevel) };
                MovementSpeed = new float[] { 0.5f, 2.5f };
                AttackCooldown = 1f;
                AttackRange = 35f;
                AttackType = "MutantSpit";
                DetectionRange = new float[] {
                    Mathf.Lerp(25f, 50f, PowerLevel),
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "AcidMutant";
                MobAudio = "FastAcidMutant";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 7:
                // Phantom Mutant
                MobName = GS.SetString("Phantom Mutant", "Fantomowy Mutant");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    Mathf.Lerp(20f, 50f, PowerLevel),
                    Mathf.Lerp(20f, 50f, PowerLevel) };
               MovementSpeed = new float[] {
                    1f,
                    Mathf.Lerp(5f, 10f, PowerLevel) };
                AttackCooldown = 0.5f;
                AttackRange = 1.5f;
                AttackType = "MutantBite";
                DetectionRange = new float[] {
                    Mathf.Lerp(5f, 25f, PowerLevel),
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "PhantomMutant";
                MobAudio = "Phantom";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 8:
                // Guard
                MobName = GS.SetString("Guard", "Strażnik");
                ClassOfMob = "Guard";
                MobHealth = new float[] { 50f, 50f };
                MovementSpeed = new float[] { 5f, 10f };
                DetectionRange = new float[] { 25f, 50f, 25f };
                WhichSubModels = "Guard";
                int PickWeapon2 = Random.Range(1, 7);
                switch(PickWeapon2){
                    case 1: case 7:
                        AttackRange = 25f;
                        AttackType = "Revolver";
                        WeaponToDrop = 32;
                        AttackCooldown = 1f;
                        Ammo = new int[] { 6, 6 };
                        AnimationSet = "HumanoidPistol";
                        break;
                    case 2:
                        AttackRange = 25f;
                        AttackType = "Shotgun";
                        WeaponToDrop = 40;
                        AttackCooldown = 1.5f;
                        Ammo = new int[] { 6, 6 };
                        ToAttack[2] = 3;
                        AnimationSet = "HumanoidGun";
                        break;
                    case 3:
                        AttackRange = 12f;
                        AttackType = "MP5";
                        WeaponToDrop = 41;
                        AttackCooldown = 0.05f;
                        Ammo = new int[] { 30, 30 };
                        ToAttack[1] = 5;
                        AnimationSet = "HumanoidPistol";
                        break;
                    case 4:
                        AttackRange = 15f;
                        AttackType = "M4";
                        WeaponToDrop = 42;
                        AttackCooldown = 0.1f;
                        Ammo = new int[] { 30, 30 };
                        ToAttack[1] = 3;
                        AnimationSet = "HumanoidGun";
                        break;
                    case 5:
                        AttackRange = 25f;
                        AttackType = "Famas";
                        WeaponToDrop = 57;
                        AttackCooldown = 0.1f;
                        Ammo = new int[] { 25, 25 };
                        ToAttack[1] = 2;
                        AnimationSet = "HumanoidGun";
                        break;
                    case 6:
                        AttackRange = 25f;
                        AttackType = "Scar";
                        WeaponToDrop = 60;
                        AttackCooldown = 0.1f;
                        Ammo = new int[] { 25, 25 };
                        ToAttack[1] = 2;
                        AnimationSet = "HumanoidGun";
                        break;
                }
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                MobAudio = "Guard";
                MobColor = new Color32(0, 128, 255, 255);
                break;
            case 9:
                // Boomer Mutant
                MobName = GS.SetString("Boomer Mutant", "Wybuchowy Mutant");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    5f,
                    5f};
                MovementSpeed = new float[] {
                    Mathf.Lerp(1f, 1f, PowerLevel),
                    Mathf.Lerp(10f, 10f, PowerLevel) };
                AttackCooldown = 0.25f;
                AttackRange = 1.5f;
                AttackType = "MutantBite";
                DetectionRange = new float[] {
                    25f,
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "BoomerMutant";
                MobAudio = "Boomer";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 10:
                // Tesla Mutant
                MobName = GS.SetString("Tesla Mutant", "Tesla Mutant");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    Mathf.Lerp(20f, 100f, PowerLevel),
                    Mathf.Lerp(20f, 100f, PowerLevel) };
                MovementSpeed = new float[] { 0.5f, 2.5f };
                AttackCooldown = 1f;
                AttackRange = 25f;
                AttackType = "LightningBolt";
                DetectionRange = new float[] {
                    Mathf.Lerp(25f, 50f, PowerLevel),
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "TeslaMutant";
                MobAudio = "Phantom";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 11:
                // Fire Mutant
                MobName = GS.SetString("Fire Mutant", "Ognisty Mutant");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    Mathf.Lerp(20f, 100f, PowerLevel),
                    Mathf.Lerp(20f, 100f, PowerLevel) };
                MovementSpeed = new float[] {
                    1f,
                    Mathf.Lerp(5f, 10f, PowerLevel) };
                AttackCooldown = 0.5f;
                AttackRange = 0f;
                AttackType = "MutantBite";
                DetectionRange = new float[] {
                    Mathf.Lerp(25f, 50f, PowerLevel),
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "FireMutant";
                MobAudio = "StrongMutant";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 12:
                // Soldier Mutant
                MobName = GS.SetString("Mutant Soldier", "Mutant Żołnierz");
                ClassOfMob = "Mutant";
                MobHealth = new float[] {
                    Mathf.Lerp(20f, 100f, PowerLevel),
                    Mathf.Lerp(20f, 100f, PowerLevel) };
                MovementSpeed = new float[] {
                    Mathf.Lerp(1f, 1f, PowerLevel),
                    Mathf.Lerp(5f, 10f, PowerLevel) };
                AttackCooldown = 2f;
                AttackRange = 20f;
                AttackType = "MutantBite";
                DetectionRange = new float[] {
                    Mathf.Lerp(25f, 50f, PowerLevel),
                    50f,
                    0f};
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                AnimationSet = "Mutant";
                WhichSubModels = "SoldierMutant";
                MobAudio = "Mutant";
                MobColor = new Color32(255, 0, 0, 255);
                break;
            case 13:
                // Half Turned Mutant
                ClassOfMob = "Mutant";
                MobName = GS.SetString("Half Turned", "Połowicznie Odmieniony");
                MobHealth = new float[] { 100f, 100f };
                MovementSpeed = new float[] { 2f, 5f };
                DetectionRange = new float[] { 25f, 50f, 25f };
                WhichSubModels = "HalfturnedMutant";
                int PickWeapon3 = Random.Range(1, 5);
                switch(PickWeapon3){
                    case 1: case 5:
                        AttackRange = 1.5f;
                        AttackType = "FireAxe";
                        WeaponToDrop = 16;
                        AttackCooldown = 0.75f;
                        AnimationSet = "Mutant";
                        break;
                    case 2:
                        AttackRange = 1.5f;
                        AttackType = "Knife";
                        WeaponToDrop = 14;
                        AttackCooldown = 0.5f;
                        AnimationSet = "Mutant";
                        break;
                    case 3:
                        AttackRange = 10f;
                        AttackType = "Arrow";
                        WeaponToDrop = 69;
                        AttackCooldown = 1f;
                        AnimationSet = "Mutant";
                        break;
                    case 4:
                        AttackRange = 25f;
                        AttackType = "Colt";
                        WeaponToDrop = 29;
                        AttackCooldown = 0.5f;
                        Ammo = new int[] { 7, 7 };
                        AnimationSet = "Mutant";
                        break;
                    default:
                        break;
                }
                WhichModels = "Humanoid";
                Humanoid.SetActive(true);
                SelectedMobModel = Humanoid;
                MobAudio = "Mutant";
                MobColor = new Color32(255, 0, 0, 255);
                break;
        }

        Color32 MutantSkin = Color32.Lerp(new Color32(155, 155, 155, 255), new Color32((byte)Random.Range(100f, 200f), (byte)Random.Range(100f, 200f), (byte)Random.Range(100f, 200f), 255), PowerLevel);
        Color[] MutantClothing = new Color[] { }; // TORSO > SUIT > UPPER ARM > LOWER ARM > PANTS
        if (BasicMutantJob == "Normal") {
            MutantClothing = new Color[] { Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 0.5f), Color.black, Color.black, Color.black, Color.Lerp(new Color32(100, 75, 55, 255), new Color32(155, 200, 255, 255), Random.Range(0f, 1f)) };
            MutantClothing = new Color[] { MutantClothing[0], MutantClothing[0], MutantClothing[0], MutantClothing[0], MutantClothing[4] };
        } else if (BasicMutantJob == "Shirt") {
            MutantClothing = new Color[] { Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 0.5f), Color.black, Color.black, MutantSkin, Color.Lerp(new Color32(100, 75, 55, 255), new Color32(155, 200, 255, 255), Random.Range(0f, 1f)) };
            MutantClothing = new Color[] { MutantClothing[0], MutantClothing[0], MutantClothing[0], MutantClothing[3], MutantClothing[4] };
        } else if (BasicMutantJob == "Suit") {
            MutantClothing = new Color[] { Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 0.5f), Color.white, Color.black, Color.black, Color.black };
            MutantClothing = new Color[] { MutantClothing[0], MutantClothing[1], MutantClothing[0], MutantClothing[0], MutantClothing[0] * 0.75f };
        } else if (BasicMutantJob == "Shirtless") {
            MutantClothing = new Color[] { MutantSkin, MutantSkin, MutantSkin, MutantSkin, Color.Lerp(new Color32(100, 75, 55, 255), new Color32(155, 200, 255, 255), Random.Range(0f, 1f)) };
        } else if (BasicMutantJob == "Police") {
            MutantClothing = new Color[] { new Color32(100, 125, 175, 255), Color.white, new Color32(100, 125, 175, 255), new Color32(100, 125, 175, 255), new Color32(55, 55, 55, 255) };
        } else if (BasicMutantJob == "Builder") {
            MutantClothing = new Color[] { new Color32(55, 150, 200, 255), new Color32(200, 55, 55, 255), new Color32(200, 55, 55, 255), new Color32(200, 55, 55, 255), new Color32(55, 150, 200, 255) };
        } else if (BasicMutantJob == "Doctor") {
            MutantClothing = new Color[] { new Color32(155, 200, 200, 255), Color.white, new Color32(155, 200, 200, 255), new Color32(155, 200, 200, 255), new Color32(55, 55, 55, 255) };
        } else if (BasicMutantJob == "Cook") {
            MutantClothing = new Color[] { Color.white, Color.white, Color.white, Color.white, new Color32(55, 55, 55, 255) };
        }

        if (SelectedMobModel == Humanoid) {
            foreach (GameObject HumanoidBodyPart in HumanoidBodyParts) {
                foreach (Transform FoundBodyPart in HumanoidBodyPart.transform) {
                    if (FoundBodyPart.name == WhichSubModels || FoundBodyPart.name == "Plunger" || FoundBodyPart.name == "Tools" || FoundBodyPart.name == "MutantHats" || FoundBodyPart.tag == "Bodypart") {
                        FoundBodyPart.gameObject.SetActive(true);
                        if (FoundBodyPart.name == "MutantHats") {
                            foreach (Transform GetHat in FoundBodyPart.transform) {
                                if (GetHat.name == BasicMutantJob) {
                                    GetHat.gameObject.SetActive(true);
                                } else {
                                    GetHat.gameObject.SetActive(false);
                                }
                            }
                        } else if (FoundBodyPart.name == "Tools") {
                            foreach (Transform GetTool in FoundBodyPart.transform) {
                                if (GetTool.name == AttackType) {
                                    GetTool.gameObject.SetActive(true);
                                } else {
                                    GetTool.gameObject.SetActive(false);
                                }
                            }
                        } else if (FoundBodyPart.GetComponent<MeshRenderer>() != null) {
                            foreach (Material mat in FoundBodyPart.GetComponent<MeshRenderer>().materials) {
                                if (WhichSubModels == "FireMutant") {
                                    mat.color = new Color32(55, 0, 0, 255);
                                } else if (WhichSubModels == "HalfturnedMutant" && mat.name == "Survivor3 (Instance)") {
                                    mat.color = Color32.Lerp(new Color32(0, 0, 0, 255), new Color32(255, 255, 255, 255), PowerLevel);
                                } else if (WhichSubModels == "HalfturnedMutant" && mat.name == "Survivor6 (Instance)") {
                                    mat.color = MutantSkin;
                                } else if (mat.name == "MutantSkin1 (Instance)") {
                                    mat.color = MutantSkin;
                                } else if (mat.name == "MutantSkin2 (Instance)") {
                                    mat.color = new Color32((byte)(MutantSkin.r / 2f), (byte)(MutantSkin.g / 2f), (byte)(MutantSkin.b / 2f), 255);
                                } else if (mat.name == "MutantSkin3 (Instance)") {
                                    mat.color = Color32.Lerp(new Color32(0, 0, 0, 255), new Color32(255, 255, 255, 255), PowerLevel);
                                } else if (mat.name == "BM-TORSO (Instance)") {
                                    mat.color = MutantClothing[0];
                                } else if (mat.name == "BM-SUIT (Instance)") {
                                    mat.color = MutantClothing[1];
                                } else if (mat.name == "BM-UPPERARM (Instance)") {
                                    mat.color = MutantClothing[2];
                                } else if (mat.name == "BM-LOWERARM (Instance)") {
                                    mat.color = MutantClothing[3];
                                } else if (mat.name == "BM-PANTS (Instance)") {
                                    mat.color = MutantClothing[4];
                                }
                            }
                        }
                    } else {
                        Destroy(FoundBodyPart.gameObject);
                    }
                }
            }
        }

        // Get Starting Waypoint
        if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
            foreach (GameObject SWP in RS.GotTerrain.GetComponent<MapInfo>().HordeWayPoints) {
                if (SWP.name.Substring(0, 2) == CurrentWaypoint.name.Substring(0, 2)) {
                    CurrentWaypoint = SWP;
                }
            }
        }

        // Hide plunger
        foreach (GameObject GetPlunger in Plungers) {
            GetPlunger.SetActive(false);
        }

        Anim.Play(AnimationSet + "Idle");
        State = 0;
		
	}

    void FixedUpdate() {

        if ((Angered > 0f) || State == 1 || (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1" || Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < RS.DetectionRange)) {
            TooFar = false;
        } else {
            TooFar = true;
        }

        if (TooFar == false) {
            Do(1f);
        } else {
            if (UpdateDelay > 0f) {
                UpdateDelay -= 0.02f;
            } else {
                UpdateDelay = 4f;
                Do(UpdateDelay / 0.02f);
            }
        }

        if (this.transform.position.y < -100f) {
            Hurt(999999999f, null, true, Vector3.zero, "Nuked");
        }
        
    }

    void Do (float DelayCompensation) {

        if (CantDoAnything > 0f) {
            CantDoAnything -= 0.02f * DelayCompensation;
        }
        if (Panic > 0f) {
            Panic -= 0.02f * DelayCompensation;
        }
        if (Angered > 0f) {
            Angered -= 0.02f * DelayCompensation;
        }
        if (Curious > 0f) {
            Curious -= 0.02f * DelayCompensation;
        }
        if (Fire > 0f || (TypeOfMob == 11f && State == 0)) {
            if (TypeOfMob != 11f) {
                Hurt(0.04f * DelayCompensation, null, false, Vector3.zero, "Fire");
                Fire -= 0.04f * DelayCompensation;
                if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 2f && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Fire < Fire) {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Fire += 5f;
                }
                foreach (GameObject SetMobOnFire in GameObject.FindGameObjectsWithTag("Mob")) {
                    if (Vector3.Distance(this.transform.position, SetMobOnFire.transform.position) < 2f && SetMobOnFire.GetComponent<MobScript>().State == 0 && SetMobOnFire.GetComponent<MobScript>().Fire < Fire) {
                        SetMobOnFire.GetComponent<MobScript>().Fire += 1f;
                    }
                }
                if (InWater == true) {
                    Fire = 0f;
                }
            }
            if (FireObj.GetComponent<ParticleSystem>().isPlaying == false || FireObj.GetComponent<AudioSource>().isPlaying == false) {
                FireObj.GetComponent<ParticleSystem>().Play();
                FireObj.GetComponent<AudioSource>().Play();
            }
        } else {
            Ach_Flare = false;
            if (FireObj.GetComponent<ParticleSystem>().isPlaying == true || FireObj.GetComponent<AudioSource>().isPlaying == true) {
                FireObj.GetComponent<ParticleSystem>().Stop();
                FireObj.GetComponent<AudioSource>().Stop();
            }
        }
        if (Blinded > 0f) {
            Blinded -= 0.02f * DelayCompensation;
            CantDoAnything = 0.25f;
        } else if (Plunged == true) {
            Plunged = false;
            foreach (GameObject Plunger in Plungers) {
                Plunger.SetActive(false);
            }
        }

        
        if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "1") {
            GunSpread[0] = Mathf.Clamp(GunSpread[0], 0f, 0f);
        } else if (GS.GetSemiClass(GS.RoundSetting, "D", "?") == "4" || GS.GetSemiClass(GS.RoundSetting, "D", "?") == "5") {
            GunSpread[0] = Mathf.Clamp(GunSpread[0], 0.5f, 1f);
        } else {
            GunSpread[0] = Mathf.Clamp(GunSpread[0], 0f, 1f);
        }
        if (GunSpread[1] > 0f) {
            GunSpread[1] -= 0.02f * DelayCompensation;
        } else {
            if (GunSpread[0] > 0f) {
                GunSpread[0] -= 0.02f * DelayCompensation;
            }
        }

        // Occlusion
        /*if (TooFar == false && State == 0 && WhichModels == "Humanoid" && HumanoidBodyParts[1].transform.GetChild(1).gameObject.activeInHierarchy == false) {
            foreach (GameObject HidPart in HumanoidBodyParts) {
                foreach (Transform HidPartB in HidPart.transform) {
                    if (HidPartB.GetComponent<MeshRenderer>() != null && HidPartB.name != "Plunger") {
                        HidPartB.gameObject.SetActive(true);
                    }
                }
            }
        } else if (TooFar == true && State == 0 && WhichModels == "Humanoid" && HumanoidBodyParts[1].transform.GetChild(1).gameObject.activeInHierarchy == true) {
            foreach (GameObject HidPart in HumanoidBodyParts) {
                foreach (Transform HidPartB in HidPart.transform) {
                    if (HidPartB.GetComponent<MeshRenderer>() != null && HidPartB.name != "Plunger") {
                        HidPartB.gameObject.SetActive(false);
                    }
                }
            }
        }*/

        if (State == 0) {

            // Die
            if (MobHealth[0] <= 0f) {
                State = 1;
            }

            // Walking and Running
            if (Panic > 0f) {
                Anim.SetFloat("IdleStance", 1f);
            } else if (Angered > 0f || Curious > 0f) {
                Anim.SetFloat("IdleStance", 0.5f);
            } else {
                Anim.SetFloat("IdleStance", 0f);
            }

            // Check if blocked way
            bool BlockedWay = false;
            for (float ChechkEach = 0f; ChechkEach < 5f; ChechkEach ++) {
                Ray CheckIfBlocked = new Ray(this.transform.position + (this.transform.up * (ChechkEach / 5f)), this.transform.forward);
                RaycastHit CheckIfBlockedHIT;
                if (Physics.Raycast(CheckIfBlocked, out CheckIfBlockedHIT, 0.75f)) {
                    if (CheckIfBlockedHIT.collider.GetComponent<MobScript>() != null && GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                        CheckIfBlockedHIT.collider.GetComponent<MobScript>().CurrentWaypoint = CurrentWaypoint;
                    } else if (CheckIfBlockedHIT.collider.name != "MayGoThrough" && CheckIfBlockedHIT.transform.root.tag != "Mob" && CheckIfBlockedHIT.transform.root.tag != "Player") {
                        BlockedWay = true;
                    }
                    break;
                }
            }

            if (Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationSet + "Idle") && Anim.GetFloat("StayOrGo") <= 0f && Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f) {
                Anim.speed = 0.25f;
            }


            // Speeds depending on stance
            int PickMovementSpeed = 0;
            if (Angered > 0f || Panic > 0f || (Curious > 0f && !(ClassOfMob == "Mutant" && PowerLevel <= 0.2f))) {
                PickMovementSpeed = 1;
            }


            // Set Pushback
            if (ReturnPushBack > 0f) {
                Ray CheckForObstacl = new Ray(this.transform.position, PushbackForce.normalized);
                RaycastHit CheckForObstaclHIT;
                if (!Physics.Raycast(CheckForObstacl, out CheckForObstaclHIT, 1f, GS.IgnoreMaskEnemy)) {
                    this.transform.position += (PushbackForce / 50f);
                }
                ReturnPushBack -= 0.02f * DelayCompensation;
            }

            if (TooFar == false) {
                if (CantDoAnything <= 0f) {
                    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(new Vector3(0f, MoveDir.transform.eulerAngles.y, MoveDir.transform.eulerAngles.z)), 3f * DelayCompensation);
                    MoveDir.transform.LookAt(new Vector3(AiPosition.x, this.transform.position.y, AiPosition.z));
                }
                if (Vector3.Distance(this.transform.position, new Vector3(AiPosition.x, this.transform.position.y, AiPosition.z)) >= 1.1f && BlockedWay == false && CantDoAnything <= 0f) {
                    if (InWater == true) {
                        this.GetComponent<Rigidbody>().velocity = new Vector3(MoveDir.transform.forward.x * (MovementSpeed[PickMovementSpeed] / 2f), this.GetComponent<Rigidbody>().velocity.y, MoveDir.transform.forward.z * (MovementSpeed[PickMovementSpeed] / 2f));
                        Anim.SetFloat("StayOrGo", 1f);
                        Anim.speed = (MovementSpeed[PickMovementSpeed] / 4f);
                    } else {
                        this.GetComponent<Rigidbody>().velocity = new Vector3(MoveDir.transform.forward.x * (MovementSpeed[PickMovementSpeed] / 1f), this.GetComponent<Rigidbody>().velocity.y, MoveDir.transform.forward.z * (MovementSpeed[PickMovementSpeed] / 1f));
                        Anim.SetFloat("StayOrGo", 1f);
                        Anim.speed = (MovementSpeed[PickMovementSpeed] / 4f);
                    }
                } else {
                    Anim.SetFloat("StayOrGo", 0f);
                    this.GetComponent<Rigidbody>().velocity = new Vector3(0f, this.GetComponent<Rigidbody>().velocity.y, 0f);
                }
            } else {
                this.transform.LookAt(new Vector3(AiPosition.x, this.transform.position.y, AiPosition.z));
                if (Vector3.Distance(this.transform.position, new Vector3(AiPosition.x, this.transform.position.y, AiPosition.z)) >= 1.1f && BlockedWay == false && CantDoAnything <= 0f) {
                    this.GetComponent<Rigidbody>().velocity = new Vector3(MoveDir.transform.forward.x * (MovementSpeed[PickMovementSpeed] / 1f), this.GetComponent<Rigidbody>().velocity.y, MoveDir.transform.forward.z * (MovementSpeed[PickMovementSpeed] / 1f));
                } else {
                    this.GetComponent<Rigidbody>().velocity = new Vector3(0f, this.GetComponent<Rigidbody>().velocity.y, 0f);
                }
            }

            // Crouching
            /*if (TooFar == false) {
                if ((Anim.GetFloat("StayOrGo") == 0f && Panic > 0f) || Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationSet + "Reload")) {
                    IsCrouching = true;
                } else {
                    IsCrouching = false;
                }
            } else {
                IsCrouching = false;
            }

            if (IsCrouching == true && this.GetComponent<CapsuleCollider>().height != 1.5f) {
                this.GetComponent<CapsuleCollider>().height = 1.5f;
                this.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.25f, 0f);
            } else if (this.GetComponent<CapsuleCollider>().height != 2f) {
                this.GetComponent<CapsuleCollider>().height = 2f;
                this.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);
            }*/

            // Movement
            if (TooFar == false && TypeOfMob != 7f) {

                if (Vector3.Distance(this.transform.position, PrevPosition) > 0.1f) {
                    FootstepCooldown -= Vector3.Distance(this.transform.position, PrevPosition);
                    PrevPosition = this.transform.position;

                    Ray CheckGround = new Ray(this.transform.position, Vector3.down);
                    RaycastHit CheckGroundHIT;
                    if (Physics.Raycast(CheckGround, out CheckGroundHIT, 1.25f)) {
                        if (CheckGroundHIT.collider.gameObject.GetComponent<FootstepMaterial>() != null) {
                            if (CheckGroundHIT.collider.gameObject.GetComponent<FootstepMaterial>().WhatToPlay == "Water") {
                                InWater = true;
                            } else {
                                InWater = false;
                            }
                        }
                        if (FootstepCooldown <= 0f && CheckGroundHIT.collider.gameObject.GetComponent<FootstepMaterial>() != null) {
                            FootstepCooldown = 2f;
                            GameObject Step = Instantiate(EffectPrefab);
                            Step.transform.position = this.transform.position - Vector3.up * 0.5f;
                            Step.GetComponent<EffectScript>().EffectName = "Footstep" + CheckGroundHIT.collider.gameObject.GetComponent<FootstepMaterial>().WhatToPlay;
                        }
                    } else {
                        CantDoAnything = Mathf.Clamp(CantDoAnything, 0.5f, Mathf.Infinity);
                    }
                }

            } else {
                InWater = false;
            }

            if (Time.fixedTime > FixedTimeCooldown && GameObject.Find("MainCanvas").GetComponent<CanvasScript>().DialogedMob != this.gameObject) {
                FixedTimeCooldown = Time.fixedTime + 3f;
                switch(ClassOfMob){
                    case "Mutant":
                        AiThink("Mutant", DelayCompensation, true);
                        break;
                    case "Bandit":
                        AiThink("Bandit", DelayCompensation, true);
                        break;
                    case "Survivor":
                        AiThink("Survivor", DelayCompensation, true);
                        break;
                    case "Guard":
                        AiThink("Guard", DelayCompensation, true);
                        break;
                }
            } else if (GameObject.Find("MainCanvas").GetComponent<CanvasScript>().DialogedMob != this.gameObject) {
                switch(ClassOfMob){
                    case "Mutant":
                        AiThink("Mutant", DelayCompensation, false);
                        break;
                    case "Bandit":
                        AiThink("Bandit", DelayCompensation, false);
                        break;
                    case "Survivor":
                        AiThink("Survivor", DelayCompensation, false);
                        break;
                    case "Guard":
                        AiThink("Guard", DelayCompensation, false);
                        break;
                }
            }

            // Fleeing
            if ((ClassOfMob == "Bandit" || ClassOfMob == "Survivor") && RS.RoundTime < 30f && RS.RoundState == "Normal") {
                foreach (GameObject foundExit in GameObject.FindGameObjectsWithTag("Interactable")) {
                    if (foundExit.GetComponent<InteractableScript>().Variables.x == 2f && Vector3.Distance(this.transform.position, foundExit.transform.position) < 2f) {
                        Destroy(this.gameObject);
                    }
                }
            }

            // HordeAI
            if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1" && GameObject.FindGameObjectWithTag("Player") != null) {
                Angered = 10f;
                AiTarget = GameObject.FindGameObjectWithTag("Player");
            }

        } else {

            if (CleanupAfterDead == 100f) {
                if ((ReasonOfDeath == "Nuked" || ReasonOfDeath == "Explosion" || ReasonOfDeath == "Electricity" || TypeOfMob == 6f || TypeOfMob == 9f) && (AnimationSet == "Mutant" || AnimationSet == "HumanoidMelee" || AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol")) {
                    if (TypeOfMob == 6f) {
                        for (int Blow = Random.Range(2, 5); Blow > 0; Blow--) {
                            //GameObject LastSplash = Instantiate(AttackPrefab) as GameObject;
                            //LastSplash.transform.Rotate(Random.Range(-15f, 90f), Random.Range(0f, 360f), 0f);
                            //LastSplash.transform.position = this.transform.position + (LastSplash.transform.forward * 1f);
                            //LastSplash.GetComponent<AttackScript>().GunName = "MutantSpit";
                            //LastSplash.GetComponent<AttackScript>().Attacker = this.gameObject;
                            //LastSplash.GetComponent<AttackScript>().Slimend = this.gameObject;
                            RS.Attack(new string[]{"MutantSpit"}, this.transform.position, new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)), this.gameObject, this.gameObject);
                        }
                    }

                    GameObject Gibs = Instantiate(EffectPrefab) as GameObject;
                    Gibs.transform.position = this.transform.position;
                    Gibs.GetComponent<EffectScript>().EffectName = "Gibs";
                    foreach (GameObject SavePart in HumanoidBodyParts) {
                        SavePart.transform.SetParent(null);
                    }
                    Gibs.GetComponent<EffectScript>().Gibs = HumanoidBodyParts;
                    Destroy(this.gameObject);

                } else if (InWater == true && (AnimationSet == "Mutant" || AnimationSet == "HumanoidMelee" || AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol")) {

                    PlayMobSound(MobAudio + "Dead", 1);
                    Anim.Play("HumanoidDeadWater", 0, 0f);
                    Anim.speed = 0.25f;
                    DroppedRagdoll = 0f;

                } else if (Fire > 0f && (AnimationSet == "Mutant" || AnimationSet == "HumanoidMelee" || AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol")) {

                    PlayMobSound(MobAudio + "Dead", 1);
                    Anim.Play("HumanoidDeadFire", 0, 0f);
                    Anim.speed = 0.5f;
                    DroppedRagdoll = 0.75f;

                } else {

                    PlayMobSound(MobAudio + "Dead", 1);
                    if ((AnimationSet == "Mutant" || AnimationSet == "HumanoidMelee" || AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol")) {
                        Anim.Play("HumanoidDead" + (int)Random.Range(1f, 3.9f), 0, 0f);
                        Anim.speed = Random.Range(0.5f, 1f);
                    }
                    DroppedRagdoll = Random.Range(0f, 0.1f);

                }

                if (Fire > 0f) {
                    Fire = 10f;
                }

                this.gameObject.layer = 13;
                CleanupAfterDead = 5f;
                this.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.8f, 0f);
                this.GetComponent<CapsuleCollider>().radius = 0.2f;
                this.GetComponent<CapsuleCollider>().height = 0.2f;
                if (AnimationSet == "Mutant" || AnimationSet == "HumanoidMelee" || AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol") {
                    Anim.SetFloat("IdleStance", 0f);
                    Anim.SetFloat("StayOrGo", 0f);
                }

                if (TypeOfMob == 2 || TypeOfMob == 3) {
                    for (int DropLoot = Random.Range(0, 3); DropLoot > 0; DropLoot--) {
                        GameObject DropedLoot = Instantiate(ItemPrefab) as GameObject;
                        DropedLoot.transform.position = this.transform.position + new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
                        DropedLoot.GetComponent<ItemScript>().Variables = GS.itemCache[RS.TotalItems[(int)Random.Range(0f, (RS.TotalItems.Length - 0.1f))]].startVariables;
                    }
                } else if (ClassOfMob == "Mutant") {
                    int SpawnChance = (int)Random.Range(-10f, 1.9f);
                    if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                        SpawnChance = (int)Random.Range(Mathf.Lerp(-1f, -3f, float.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?")) / 5f), 1.9f);
                    } else {
                        if (BasicMutantJob == "Police" || BasicMutantJob == "Builder" || BasicMutantJob == "Doctor" || BasicMutantJob == "Cook") {
                            SpawnChance = (int)Random.Range(0.5f, 2.5f);
                        } else {
                            SpawnChance = (int)Random.Range(-10f, 1.9f);
                        }
                    }
                    for (int DropLoot = SpawnChance; DropLoot > 0; DropLoot--) {
                        if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                            GameObject DropedLoot = Instantiate(HordeDropPrefab) as GameObject;
                            DropedLoot.transform.position = this.transform.position;
                        } else {
                            GameObject DropedLoot = Instantiate(ItemPrefab) as GameObject;
                            DropedLoot.transform.position = this.transform.position + new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
                            if (BasicMutantJob == "Police") {
                                DropedLoot.GetComponent<ItemScript>().Variables = GS.itemCache[RS.Weapons[(int)Random.Range(0f, RS.Weapons.Length - 0.1f)]].startVariables;
                            } else if (BasicMutantJob == "Builder") {
                                DropedLoot.GetComponent<ItemScript>().Variables = GS.itemCache[RS.Utilities[(int)Random.Range(0f, RS.Utilities.Length - 0.1f)]].startVariables;
                            } else if (BasicMutantJob == "Doctor") {
                                DropedLoot.GetComponent<ItemScript>().Variables = GS.itemCache[RS.HealingItems[(int)Random.Range(0f, RS.HealingItems.Length - 0.1f)]].startVariables;
                            } else if (BasicMutantJob == "Cook") {
                                DropedLoot.GetComponent<ItemScript>().Variables = GS.itemCache[RS.FoodItems[(int)Random.Range(0f, RS.FoodItems.Length - 0.1f)]].startVariables;
                            } else {
                                DropedLoot.GetComponent<ItemScript>().Variables = GS.itemCache[RS.TotalItems[(int)Random.Range(0f, RS.TotalItems.Length - 0.1f)]].startVariables;
                            }
                        }
                        
                    }
                    if (TypeOfMob == 9f) {
                        GameObject GoBoom = Instantiate(SpecialPrefab) as GameObject;
                        GoBoom.transform.position = this.transform.position;
                        GoBoom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                        GoBoom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                    }
                }

                if (WeaponToDrop > -1 && GS.GetSemiClass(GS.RoundSetting, "G", "?") != "1") {
                    GameObject DropedLoot = Instantiate(ItemPrefab) as GameObject;
                    DropedLoot.transform.position = this.transform.position + this.transform.forward * 0.5f;
                    DropedLoot.GetComponent<ItemScript>().Variables = GS.itemCache[WeaponToDrop].startVariables;
                    foreach (Transform HideGun in HumanoidBodyParts[3].transform.GetChild(0)) {
                        HideGun.gameObject.SetActive(false);
                    }
                }

                bool GiveCredit = false;
                if (leadAggresor != null) {
                    if (leadAggresor.tag == "Player") {
                        GiveCredit = true;
                    }
                } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                    GiveCredit = true;
                }

                if (GiveCredit == true) {
                    GS.Mess(GS.SetString(MobName + " killed", MobName + " zabity"));
                    if (TypeOfMob == 2f || TypeOfMob == 8f || (ClassOfMob == "Mutant" && TypeOfMob != 1f)) {
                        GS.AddToScore(250);
                    } else if (TypeOfMob == 1f) {
                        GS.AddToScore(50);
                    }
                }

            } else if (CleanupAfterDead > 0f) {

                CleanupAfterDead -= 0.02f * DelayCompensation;
                if (Anim.GetCurrentAnimatorStateInfo(0).IsName("HumanoidDeadWater") && Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
                    Anim.Play("HumanoidDeadWater", 0, 0.25f);
                }

                // Ragdoll
                if (GS.Ragdolls == false && DroppedRagdoll >= 0f) {
                    DroppedRagdoll = 1f;
                }
                if (DroppedRagdoll >= 0f) {
                    if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= DroppedRagdoll && (AnimationSet == "Mutant" || AnimationSet == "HumanoidMelee" || AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol")) {
                        DroppedRagdoll = -1f;
                        Anim.enabled = false;
                        GameObject DropRagdoll = Instantiate(RagdollPrefab) as GameObject;
                        DropRagdoll.transform.position = this.transform.position;
                        DropRagdoll.GetComponent<RagdollScript>().DroppedBy = this.gameObject;
                        DropRagdoll.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity/-10f;
                        DropRagdoll.GetComponent<RagdollScript>().DeadPush = RagdollPush;
                    }
                }

            } else {
                Destroy(this.gameObject);
            }

        }
		
	}

    public void Hurt(float Damage, GameObject Aggresor, bool Visible, Vector3 PointOfHit, string TypeOfDamage, GameObject PartHit = null){

        if (State == 0) {

            int Ach_Headshot = 0;
            if(PartHit && PartHit.name == "HEAD") Ach_Headshot += 1;
            int Stun = Random.Range(0, 4);
            int BlockChance = -1;
            if (AnimationSet == "HumanoidMelee") {
                BlockChance = Random.Range(0, 10);
            }

            if (Stun != 0 && BlockChance != 0 && Panic <= 0f && Blinded <= 0f && Visible == true && (MobHealth[0] - Damage > 0f)) {
                CantDoAnything = 0.5f;
                Anim.Play(AnimationSet + "Hurt" + (int)Random.Range(1f, 3.9f), 0, 0f);
                Anim.speed = 2f;
            }

            if(TypeOfDamage == "Flare"){
                TypeOfDamage = "Fire";
                Ach_Flare = true;
            } else if (TypeOfDamage == "BarbedWire"){
                CantDoAnything = Damage;
                Anim.Play(AnimationSet + "Hurt2", 0, 0f);
                Anim.speed = 1f/Damage;
            }

            if (Damage > 0f) {

                if (BlockChance == 0 && TypeOfDamage == "Melee") {

                    GameObject Blood = Instantiate(EffectPrefab) as GameObject;
                    Blood.transform.position = this.transform.position;
                    Blood.transform.Rotate(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f));
                    Blood.GetComponent<EffectScript>().EffectName = "BullethitMetal";

                    CantDoAnything = 0.5f;
                    Anim.Play(AnimationSet + "Parry", 0, 0f);
                    Anim.speed = 2f;

                } else {

                    
                    if (Visible == true) {
                        PlayMobSound(MobAudio + "Hurt", 1);
                        GameObject Blood = Instantiate(EffectPrefab) as GameObject;
                        Blood.transform.position = this.transform.position;
                        Blood.transform.Rotate(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0f));
                        Blood.GetComponent<EffectScript>().EffectName = "BloodSplat";
                        int ChanceOfPanic = (int)Random.Range(0f, Mathf.Lerp(0f, 4f, MobHealth[0] / (MobHealth[1] / 2f)));
                        if (MobHealth[0] < MobHealth[1] / 2f && ChanceOfPanic == 0 && Aggresor != null && ClassOfMob != "Mutant" && ClassOfMob != "Guard") {
                            EyeSight.transform.LookAt(Aggresor.transform.position);
                            React("Panic", 15f, Aggresor.transform.position - EyeSight.transform.forward * 50f + new Vector3(Random.Range(-12f, 12f), 0f, Random.Range(-12f, 12f)));
                            if(TypeOfMob == 2 && AttackType == "Melee" && AnimationSet == "HumanoidGun" && Aggresor && Aggresor.tag == "Player") GS.PS.AchProg("Ach_BeatIntoSubmission", "0");
                        }
                        if (TypeOfDamage == "Gun" && AnimationSet == "HumanoidMelee" && Aggresor) {
                            React("Panic", 15f, Aggresor.transform.position - EyeSight.transform.forward * 50f + new Vector3(Random.Range(-12f, 12f), 0f, Random.Range(-12f, 12f)));
                        }
                    }
                    
                    if(MobHealth[1] == MobHealth[0]) Ach_Headshot += 1;

                    if(Aggresor && Aggresor.tag == "Player") RS.SetScore("Damage_", "/+" + ((int)Mathf.Clamp(Damage, 1, MobHealth[0])).ToString());
                    MobHealth[0] -= Damage;
                    if(MobHealth[0] <= 0) Ach_Headshot += 1;

                    RagdollPush = new Vector4(PointOfHit.x, PointOfHit.y, PointOfHit.z, Mathf.Clamp(Damage, 2f, 10f));

                    ReasonOfDeath = TypeOfDamage;

                    if(Aggresor) leadAggresor = Aggresor;

                }

                if (Aggresor != null && Panic <= 0f) {
                    if (Aggresor.GetComponent<MobScript>() != null) {
                        if (Aggresor.GetComponent<MobScript>().ClassOfMob != ClassOfMob) {
                            Angered = 10f;
                            AiTarget = Aggresor;
                        }
                    } else {
                        Angered = 10f;
                        AiTarget = Aggresor;
                        if (TypeOfMob == 3f && Aggresor.tag == "Player") {
                            MobColor = new Color32(255, 0, 0, 255);
                            this.GetComponent<Interactions>().Options = new string[] {""};
                        }
                    }
                    
                }

                // Warns others
                if (MobHealth[0] > 0f && ClassOfMob != "Mutant") {
                    foreach (GameObject Alerted in GameObject.FindGameObjectsWithTag("Mob")) {
                        if (Alerted.GetComponent<MobScript>().Squad[0] != 0 && Alerted != this.gameObject && Alerted.GetComponent<MobScript>().Squad[0] == Squad[0] && Aggresor != null && Alerted.GetComponent<MobScript>().Panic <= 0f) {
                            Alerted.GetComponent<MobScript>().Angered = 10f;
                            Alerted.GetComponent<MobScript>().Curious = 0f;
                            Alerted.GetComponent<MobScript>().AiTarget = Aggresor;
                            Alerted.GetComponent<MobScript>().AiPosition = this.transform.position;
                            if (TypeOfMob == 3f && Aggresor.tag == "Player") {
                                Alerted.GetComponent<MobScript>().MobColor = new Color32(255, 0, 0, 255);
                            }
                        } else if (Alerted.GetComponent<MobScript>().TypeOfMob == TypeOfMob && Alerted != this.gameObject && Vector3.Distance(Alerted.transform.position, this.transform.position) < 25f && Aggresor != null && Alerted.GetComponent<MobScript>().Panic <= 0f) {
                            Alerted.GetComponent<MobScript>().Angered = 10f;
                            Alerted.GetComponent<MobScript>().Curious = 0f;
                            Alerted.GetComponent<MobScript>().AiTarget = Aggresor;
                            Alerted.GetComponent<MobScript>().AiPosition = this.transform.position;
                            if (TypeOfMob == 3f && Aggresor.tag == "Player") {
                                Alerted.GetComponent<MobScript>().MobColor = new Color32(255, 0, 0, 255);
                            }
                        }
                    }
                }
                

                // Reward
                if (MobHealth[0] <= 0f && leadAggresor != null) {
                    if(Ach_Flare) GS.PS.AchProg("Ach_EmergencyFlare", "0");
                    if(leadAggresor.tag == "Player"){
                        RS.SetScore("Kill" + ClassOfMob + "_", "/+1");
                        RS.SetScore("Killed_", "/+1");
                        if(ClassOfMob == "Mutant" && GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0")GS.PS.AchProg("Ach_Liquidator", "/+-1");
                        if(ClassOfMob != "Mutant")GS.PS.AchProg("Ach_Murderer", "/+-1");
                        if(Ach_Headshot == 3) GS.PS.AchProg("Ach_Headshot", "0");
                    }
                }
            }
            

        }

    }

    public void React(string How, float HowLong, Vector3 Where) {

        if (State == 0) {

            if (How == "Curious" && Angered <= 0f && Panic <= 0f) {
                if (Curious <= 0f && (ClassOfMob != "Mutant")) {
                    Anim.Play(AnimationSet + "Confused", 0, 0f);
                    float RealisationTime = Random.Range(0.5f, 2f);
                    Anim.speed = RealisationTime;
                    CantDoAnything = 1f / RealisationTime;
                }
                Curious = HowLong;
                AiPosition = Where;
            } else if (How == "Blinded") {
                Blinded = HowLong;
                Anim.Play(AnimationSet + "Blinded", 0, 0f);
                Anim.speed = 1f / HowLong;
            } else if (How == "Panic") {
                Angered = 0f;
                Curious = 0f;
                Panic = HowLong;
                AiPosition = Where;
            }

        }

    }

    void AiThink(string Philosophy, float FixedTimeDelay, bool Check) {

        bool CharacterSeen = false;
        bool CharacterOnSights = false;
        if (ReturnToWayPoint > 0f) {
            ReturnToWayPoint -= 0.02f;
        } else if (CurrentWaypoint != null) {
            AiPosition = CurrentWaypoint.transform.position;
        }

        if (AiTarget != null) {

            float DetectA = DetectionRange[1];
            if (AiTarget.tag == "Player") {
                float[] PlayerDistance = new float[] { 1f, 0.5f };
                if (RS.MainPlayer.InBox == true && RS.MainPlayer.IsCrouching >= 1f && Angered < 9f) {
                    PlayerDistance = new float[] { 0f, 0f };
                } else if (RS.MainPlayer.IsCrouching >= 1f) {
                    PlayerDistance = new float[] { 0.5f, 0f };
                } else {
                    PlayerDistance = new float[] { 1f, 0.5f };
                }
                float Angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0f, this.transform.eulerAngles.y, 0f)), Quaternion.Euler(new Vector3(0f, EyeSight.transform.eulerAngles.y, 0f)));
                if (Angle < 90f) {
                    DetectA = DetectionRange[1] * PlayerDistance[0];
                } else {
                    DetectA = DetectionRange[1] * PlayerDistance[1];
                }
            }

            EyeSight.transform.LookAt(AiTarget.transform.position);
            Ray CheckForTarget = new Ray(EyeSight.transform.position, EyeSight.transform.forward);
            float CheckDistance = 0f;
            if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                CheckDistance = 50f;
            } else {
                CheckDistance = DetectA;
            }
            RaycastHit CheckForTargetHIT;
            if (Physics.Raycast(CheckForTarget, out CheckForTargetHIT, CheckDistance, GS.IgnoreMaskEnemy)) {
                if (CheckForTargetHIT.collider.gameObject == AiTarget) {
                    CharacterSeen = true;
                    AiTargetLastSeen = CheckForTargetHIT.collider.transform.position;
                    ReturnToWayPoint = 10f;
                    Angered = 10f;
                }
            }
            if (Physics.Raycast(CheckForTarget, out CheckForTargetHIT, AttackRange + 0.25f, GS.IgnoreMaks1)) {
                if (CheckForTargetHIT.collider.gameObject == AiTarget) {
                    CharacterOnSights = true;
                }
            }

            // Check if target is hiding
            if (Vector3.Distance(this.transform.position, AiTarget.transform.position) > DetectionRange[1] && GS.GetSemiClass(GS.RoundSetting, "G", "?") != "1") {
                AiTarget = null;
                Angered = 0f;
                CharacterSeen = false;
            } else if (AiTarget.GetComponent<PlayerScript>() != null) {
                if (AiTarget.GetComponent<PlayerScript>().Health[0] <= 0f) {
                    AiTarget = null;
                    Angered = 0f;
                    CharacterSeen = false;
                }
            } else if (AiTarget.GetComponent<MobScript>() != null) {
                if (AiTarget.GetComponent<MobScript>().MobHealth[0] <= 0f) {
                    AiTarget = null;
                    Angered = 0f;
                    CharacterSeen = false;
                }
            }
            if (Angered <= 0f) {
                AiTarget = null;
                Angered = 0f;
                CharacterSeen = false;
            }
        } else {
            Angered = 0f;
            CharacterSeen = false;
        }

        if (TypeOfMob == 11) {
            if (InWater == true || RS.Weather == 4) {
                MobHealth[0] = 0f;
            }
            if (BurnCooldown > 0f) {
                BurnCooldown -= 0.02f * FixedTimeDelay;
            } else {
                BurnCooldown = 0.5f;
                foreach (GameObject BurnEm in GameObject.FindGameObjectsWithTag("Mob")) {
                    if (BurnEm.GetComponent<MobScript>().ClassOfMob != "Mutant" && Vector3.Distance(this.transform.position, BurnEm.transform.position) < 10f) {
                        BurnEm.GetComponent<MobScript>().Fire += 5f;
                    }
                }
                if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 10f) {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Fire += 5f;
                }
            }
        }


        if (Panic > 0f && ClassOfMob != "Mutant") {

            Curious = 0f;
            Angered = 0f;
            PlayMobSound(MobAudio + "Angered" + (int)Random.Range(1f, 3.9f), 2);

        } else if (Angered > 0f) {

            Curious = 0f;
            PlayMobSound(MobAudio + "Angered" + (int)Random.Range(1f, 3.9f), 2);

            if(AiTarget.tag == "Player" && CharacterSeen){
                RS.CS.MMdanger = 1f;
            } else if(AiTarget.tag == "Player" && !CharacterSeen){
                RS.CS.MMunsafe = 1f;
            }

            // Set position depending if seen or not
            if (CharacterSeen == true || CharacterOnSights == true) {
                if (Vector3.Distance(this.transform.position, AiPosition) < AttackRange - 0.5f) {
                    PreventJamming[0] = 0f;
                }
                if (PreventJamming[0] > 0f) {
                    PreventJamming[0] -= 0.01f;
                } else {
                    AiPosition = AiTarget.transform.position;
                }
                if (PreventJamming[1] > 0f) {
                    PreventJamming[1] -= 0.01f;
                } else {
                    PreventJamming[1] = 0.25f;
                    AiTargetLastSeen = AiTarget.transform.position;
                }
                SwitchPosition = 0f;
            } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0") {
                //PreventJamming[0] = 1f;
                if (SwitchPosition > 0f) {
                    SwitchPosition -= 0.02f * FixedTimeDelay;
                } else {
                    SwitchPosition = Random.Range(0.5f, 1f);
                    AiPosition = AiTargetLastSeen + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
                }
            } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                //PreventJamming[0] = 1f;
                if (ReturnToWayPoint <= 0f) {
                    if (Vector3.Distance(this.transform.position, new Vector3(CurrentWaypoint.transform.position.x, this.transform.position.y, CurrentWaypoint.transform.position.z)) < 1.5f) {
                        if (CurrentWaypoint != null) {
                            if (CWBAF == 0) {
                                if (CurrentWaypoint.transform.childCount > 0) {
                                    CurrentWaypoint = CurrentWaypoint.transform.GetChild((int)Random.Range(0, CurrentWaypoint.transform.childCount - 0.1f)).gameObject;
                                } else {
                                    CWBAF = 1;
                                }
                            } else if (CWBAF == 1) {
                                if (CurrentWaypoint.transform.parent != null && CurrentWaypoint.transform.parent.name.Substring(0, 1) != "T" && CurrentWaypoint.transform.parent.name.Substring(0, 1) != "0") {
                                    CurrentWaypoint = CurrentWaypoint.transform.parent.gameObject;
                                } else {
                                    CWBAF = 0;
                                }
                            }
                        } else {
                            float NearestAWP = Mathf.Infinity;
                            GameObject PNAWP = null;
                            foreach (GameObject CheckAWP in RS.GotTerrain.GetComponent<MapInfo>().HordeWayPoints) {
                               if (Vector3.Distance(this.transform.position, CheckAWP.transform.position) < NearestAWP) {
                                    NearestAWP = Vector3.Distance(this.transform.position, CheckAWP.transform.position);
                                    PNAWP = CheckAWP;
                               }
                            }
                            CurrentWaypoint = PNAWP;
                        }
                    }
                } else {
                    if (SwitchPosition > 0f) {
                        SwitchPosition -= 0.02f * FixedTimeDelay;
                    } else {
                        SwitchPosition = Random.Range(0.5f, 1f);
                        AiPosition = AiTargetLastSeen + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
                    }
                }
            }

            // Attack if too close
            bool Gun = false;
            if ((AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol") && Ammo[0] <= 0 && CantDoAnything <= 0f) {
                Gun = true;
                if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("HumanoidGunReload") && !Anim.GetCurrentAnimatorStateInfo(0).IsName("HumanoidPistolReload")) {
                    Anim.Play(AnimationSet + "Reload", 0, 0f);
                    Anim.speed = 0.3f;
                    PlayMobSound("Reloading", 1);
                    CantDoAnything = 3f;
                } else if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f) {
                    Ammo[0] = Ammo[1];
                }
            }
            float Angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0f, this.transform.eulerAngles.y, this.transform.eulerAngles.z)), Quaternion.Euler(new Vector3(0f, EyeSight.transform.eulerAngles.y, EyeSight.transform.eulerAngles.z)));
            if (Vector3.Distance(this.transform.position, new Vector3(AiTarget.transform.position.x, this.transform.position.y, AiTarget.transform.position.z)) <= AttackRange && CharacterOnSights == true && CantDoAnything <= 0f && Angle < 10f) {
                ToAttack[0] = ToAttack[1];
            }
            if (ToAttack[0] > 0 && CantDoAnything <= 0f) {
                ToAttack[0] -= 1;
                for (int ToShoot = ToAttack[2]; ToShoot > 0; ToShoot --) {
                    List<string> Additionals = new List<string>();
                    GameObject Setslimend = null;
                    //GameObject Attack = Instantiate(AttackPrefab) as GameObject;
                    //Attack.transform.position = this.transform.position + Vector3.up * 0.5f + this.transform.forward * 0.5f;
                    //Attack.transform.LookAt(AiTarget.transform.position);
                    //Attack.GetComponent<AttackScript>().GunName = AttackType;
                    Additionals.Add(AttackType);
                    if (ToShoot != 1) {
                        //Attack.GetComponent<AttackScript>().GunFire = false;
                        Additionals.Add("HideGunFire;");
                    }
                    //Attack.GetComponent<AttackScript>().Attacker = this.gameObject;
                    if (SelectedMobModel == Humanoid) {
                        Setslimend = HumanoidBodyParts[3];
                    }
                    if (Gun == true) {
                        Additionals.Add("GunSpread" + RS.ReceiveGunSpred(WeaponToDrop, 0f, 1 - GunSpread[0]).x + ";");
                        //Attack.GetComponent<AttackScript>().SpecialGunSpread = RS.ReceiveGunSpred(WeaponToDrop, 0f, 1 - GunSpread[0]).x;
                        //Attack.transform.Rotate(new Vector3(
                        //    (1f - GunSpread[0]) * -RS.ReceiveGunSpred(WeaponToDrop, 0f, 1 - GunSpread[0]).z,
                        //    (1f - GunSpread[0]) * RS.ReceiveGunSpred(WeaponToDrop, 0f, 1 - GunSpread[0]).w * Random.Range(-1f, 1f),
                        //    0f));
                    } else if (TypeOfMob == 6 || TypeOfMob == 10 || TypeOfMob == 13){
                        Additionals.Add("GunSpread" + "5;");
                    }
                    RS.Attack(Additionals.ToArray(), this.transform.position + Vector3.up * 0.5f + this.transform.forward * 0.5f, AiTarget.transform.position - (this.transform.position + Vector3.up * 0.5f + this.transform.forward * 0.5f) , this.gameObject, Setslimend);
                }
                
                Anim.Play(AnimationSet + "Attack", 0, 0f);
                Anim.speed = 1f / AttackCooldown;
                CantDoAnything = AttackCooldown;
                if (AnimationSet == "HumanoidGun" || AnimationSet == "HumanoidPistol") {
                    Ammo[0] -= 1;
                    GunSpread[0] += RS.ReceiveGunSpred(WeaponToDrop, 0f, 1 - GunSpread[0]).y;
                    GunSpread[1] = AttackCooldown;
                }
                if (TypeOfMob == 9f) {
                    GameObject GoBoom = Instantiate(SpecialPrefab) as GameObject;
                    GoBoom.transform.position = this.transform.position;
                    GoBoom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                    GoBoom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                } else if (TypeOfMob == 12f) {
                    CantDoAnything = 3f;
                    GameObject Grenade = Instantiate(ItemPrefab) as GameObject;
                    Grenade.transform.position = this.transform.position + this.transform.forward * 0.25f;
                    string[] Nades = new string[] {"66", "110", "131"};
                    Grenade.GetComponent<ItemScript>().Variables = "id0;";
                    Grenade.GetComponent<ItemScript>().Variables = GS.SetSemiClass(Grenade.GetComponent<ItemScript>().Variables, "id", Nades[(int)Random.Range(0f, 2.9f)]);//Grenade.GetComponent<ItemScript>().Variables = new Vector3(Nades[(int)Random.Range(0f, 2.9f)], 33f, 0f);
                    Grenade.GetComponent<ItemScript>().Variables = GS.SetSemiClass(Grenade.GetComponent<ItemScript>().Variables, "va", "33");
                    Grenade.GetComponent<ItemScript>().State = 2;
                    Grenade.GetComponent<ItemScript>().ThrownDirection = this.transform.forward * 1f + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f) + Vector3.up * 0.25f;
                }
            }

        } else if (Curious > 0f) {

            //PreventJamming[0] = 0;

        }

        if (Angered <= 0f) {

            //PreventJamming[0] = 0;

        }

        // Fire
        if (Fire > 0f && ClassOfMob != "Mutant") {
            Panic = 1f;
            if (SwitchPosition > 0f) {
                SwitchPosition -= 0.01f;
            } else {
                SwitchPosition = Random.Range(0.1f, 2f);
                AiPosition = this.transform.position + new Vector3(Random.Range(-25f, 25f), this.transform.position.y, Random.Range(-25f, 25f));
            }
        }

        if (Angered <= 0f && Panic <= 0f) {

            // Idle Chatter
            if (Chatter > 0f) {
                Chatter -= 0.04f * FixedTimeDelay;
            } else {
                Chatter = Random.Range(5f, 15f);
                PlayMobSound(MobAudio + "Idle" + (int)Random.Range(1f, 3.9f), 2);
            }

            // Wander off
            if ((Philosophy == "Mutant" || Philosophy == "Bandit" || Philosophy == "Survivor") && !(mobOrigins == "RaidSurvivors" || mobOrigins == "RaidBandits") && Curious <= 0f) {
                if (SwitchPosition > 0f) {
                    SwitchPosition -= 0.02f * FixedTimeDelay;
                } else {
                    if (Squad[0] != 0 && Squad[1] == 1) {
                        SwitchPosition = Random.Range(1f, 10f);
                        AiPosition = this.transform.position + new Vector3(Random.Range(-50f, 50f), 0f, Random.Range(-50f, 50f));
                        foreach (GameObject Squadmember in GameObject.FindGameObjectsWithTag("Mob")) {
                            if (Squadmember.GetComponent<MobScript>().Squad[0] == Squad[0] && GameObject.Find("MainCanvas").GetComponent<CanvasScript>().DialogedMob != Squadmember) {
                                Squadmember.GetComponent<MobScript>().AiPosition = AiPosition + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
                            }
                        }
                    } else if (Squad[0] == 0) {
                        SwitchPosition = Random.Range(1f, 10f);
                        AiPosition = this.transform.position + new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f));
                    }
                }
            } else if ((Philosophy == "Guard" || mobOrigins == "RaidBandits" || mobOrigins == "RaidSurvivors") && Curious <= 0f) {
                if (SwitchPosition > 0f) {
                    SwitchPosition -= 0.02f * FixedTimeDelay;
                } else {
                    SwitchPosition = Random.Range(7f, 10f);
                    AiPosition = StartPosition + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
                }
            }



            // Find Targets
            if ((Philosophy == "Mutant" || Philosophy == "Bandit" || Philosophy == "Survivor" || Philosophy == "Guard") && Check == true) {

                float[] PlayerDistance = new float[] { 1f, 0.5f };
                if (RS.MainPlayer.Health[0] <= 0f || (RS.MainPlayer.InBox == true && RS.MainPlayer.IsCrouching >= 1f)) {
                    PlayerDistance = new float[] { 0f, 0f };
                } else if (RS.MainPlayer.IsCrouching >= 1f) {
                    PlayerDistance = new float[] { 0.5f, 0f };
                } else {
                    PlayerDistance = new float[] { 1f, 0.5f };
                }

                Vector3 FromPosition = Vector3.zero;
                List<GameObject> Targets = new List<GameObject>();
                if (Philosophy == "Mutant") {
                    FromPosition = this.transform.position;

                    if (PlayerDistance[0] > 0f) {
                        Targets.Add(RS.MainPlayer.gameObject);
                    }
                    foreach (GameObject FoundMob in GameObject.FindGameObjectsWithTag("Mob")) {
                        if ((FoundMob.GetComponent<MobScript>().TypeOfMob == 2 || FoundMob.GetComponent<MobScript>().TypeOfMob == 3 || FoundMob.GetComponent<MobScript>().TypeOfMob == 8) && FoundMob.GetComponent<MobScript>().MobHealth[0] > 0f) {
                            Targets.Add(FoundMob);
                        }
                    }

                } else if (Philosophy == "Bandit") {
                    FromPosition = this.transform.position;

                    if (PlayerDistance[0] > 0f) {
                        Targets.Add(RS.MainPlayer.gameObject);
                    }
                    foreach (GameObject FoundMob in GameObject.FindGameObjectsWithTag("Mob")) {
                        if ((FoundMob.GetComponent<MobScript>().TypeOfMob == 3 || (mobOrigins == "RaidBandits" && FoundMob.GetComponent<MobScript>().TypeOfMob == 8)) && FoundMob.GetComponent<MobScript>().MobHealth[0] > 0f) {
                            Targets.Add(FoundMob);
                        }
                    }

                } else if (Philosophy == "Survivor") {
                    FromPosition = this.transform.position;

                    if (PlayerDistance[0] > 0f && MobColor.ToString() == new Color32(255, 0, 0, 255).ToString()) {
                        Targets.Add(RS.MainPlayer.gameObject);
                    }
                    foreach (GameObject FoundMob in GameObject.FindGameObjectsWithTag("Mob")) {
                        if ((FoundMob.GetComponent<MobScript>().TypeOfMob == 2f || (mobOrigins == "RaidSurvivors" && FoundMob.GetComponent<MobScript>().TypeOfMob == 8)) && FoundMob.GetComponent<MobScript>().MobHealth[0] > 0f) {
                            Targets.Add(FoundMob);
                        }
                    }

                } else if (Philosophy == "Guard") {
                    FromPosition = StartPosition;

                    if (RS.MainPlayer.GetComponent<PlayerScript>().Health[0] > 0f) {
                        Targets.Add(RS.MainPlayer.gameObject);
                    }
                    foreach (GameObject FoundMob in GameObject.FindGameObjectsWithTag("Mob")) {
                        if (FoundMob.GetComponent<MobScript>().TypeOfMob != 8f && FoundMob.GetComponent<MobScript>().MobHealth[0] > 0f) {
                            Targets.Add(FoundMob);
                        }
                    }

                }
                // Find Targets

                // Acquire Targets
                foreach (GameObject PotentialFoe in Targets) {
                    float DetectA = DetectionRange[0];
                    EyeSight.transform.LookAt(PotentialFoe.transform.position);
                    if (PotentialFoe.tag == "Player" && PotentialFoe.GetComponent<PlayerScript>().State == 1) {
                        float Angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0f, this.transform.eulerAngles.y, 0f)), Quaternion.Euler(new Vector3(0f, EyeSight.transform.eulerAngles.y, 0f)));
                        if (Angle < 90f) {
                            DetectA = DetectionRange[0] * PlayerDistance[0];
                        } else {
                            DetectA = DetectionRange[0] * PlayerDistance[1];
                        }
                    }
                    if (RS.TimeOfDay[0] == 0) {
                        DetectA /= 2f;
                    }
                    if (Vector3.Distance(PotentialFoe.transform.position, FromPosition) < DetectA) {
                        Ray CheckForTarget = new Ray(EyeSight.transform.position, EyeSight.transform.forward);
                        RaycastHit CheckForTargetHIT;
                        if (Physics.Raycast(CheckForTarget, out CheckForTargetHIT, DetectA)) {
                            if (CheckForTargetHIT.collider.gameObject == PotentialFoe) {
                                Angered = 10f;
                                Curious = 0f;
                                PreventJamming[0] = 0;
                                ToAttack[0] = 0;
                                AiTarget = PotentialFoe;
                                // Warns others
                                foreach (GameObject Alerted in GameObject.FindGameObjectsWithTag("Mob")) {
                                    if (Alerted.GetComponent<MobScript>().Angered <= 0f && Alerted.GetComponent<MobScript>().TypeOfMob == TypeOfMob && Vector3.Distance(this.transform.position, Alerted.transform.position) < 25f && PotentialFoe != null) {
                                        Alerted.GetComponent<MobScript>().Angered = 10f;
                                        Alerted.GetComponent<MobScript>().Curious = 0f;
                                        Alerted.GetComponent<MobScript>().AiTarget = PotentialFoe;
                                        Alerted.GetComponent<MobScript>().AiPosition = this.transform.position;
                                    }
                                }
                            }
                        }
                    }
                }
                // Acquire Targets

            }

            // Survivor stance
            if (TypeOfMob == 3f && MobColor.ToString() != new Color32(255, 0, 0, 255).ToString()) {
                this.GetComponent<Interactions>().Icons = new string[] {"TalkTo"};
                this.GetComponent<Interactions>().Options = new string[] {"TalkTo"};
            }

        } else {

            if (TypeOfMob == 3f) {
                this.GetComponent<Interactions>().Icons = new string[] {""};
                this.GetComponent<Interactions>().Options = new string[] {""};
            }

        }

    }

    void PlayMobSound(string SoundToPlay, int Overwrite) {

        // Overwrite 0 = Don't, 1 Overwrite, 2 If isn't playing

        string GetSound = SoundToPlay;
        float Pitch = 1f;
        if (GetSound == "MutantIdle1" || GetSound == "MutantIdle2" || GetSound == "MutantIdle3") {
            GetSound = "MutantAngered" + (int)Random.Range(1f, 3.9f);
        } else if (GetSound == "StrongMutantIdle1" || GetSound == "StrongMutantIdle2" || GetSound == "StrongMutantIdle3") {
            GetSound = "MutantAngered" + (int)Random.Range(1f, 3.9f);
            Pitch = 0.25f;
        } else if (GetSound == "StrongMutantAngered1" || GetSound == "StrongMutantAngered2" || GetSound == "StrongMutantAngered3") {
            GetSound = "MutantAngered" + (int)Random.Range(1f, 3.9f);
            Pitch = 0.25f;
        } else if (GetSound == "StrongMutantHurt") {
            GetSound = "MutantHurt";
            Pitch = 0.25f;
        } else if (GetSound == "StrongMutantDead") {
            GetSound = "MutantDead";
            Pitch = 0.25f;
        } else if (GetSound == "FastAcidMutantIdle1" || GetSound == "FastAcidMutantIdle2" || GetSound == "FastAcidMutantIdle3") {
            GetSound = "FastAcidMutantAngered" + (int)Random.Range(1f, 3.9f);
        } else if (GetSound == "PhantomIdle1" || GetSound == "PhantomIdle2" || GetSound == "PhantomIdle3") {
            GetSound = "PhantomAngered" + (int)Random.Range(1f, 3.9f);
        } else if (GetSound == "BoomerAngered1" || GetSound == "BoomerAngered2" || GetSound == "BoomerAngered3") {
            GetSound = "BoomerAngered1";
        }

        if (Overwrite == 1 || (Overwrite == 2 && SoundBank.GetComponent<AudioSource>().isPlaying == false)) {

            foreach (AudioClip GetAudio in SoundBankClips) {
                if (GetAudio.name == GetSound) {
                    SoundBank.GetComponent<AudioSource>().clip = GetAudio;
                    SoundBank.GetComponent<AudioSource>().Play();
                    SoundBank.GetComponent<AudioSource>().pitch = Pitch;
                }
            }

        }

    }

}
