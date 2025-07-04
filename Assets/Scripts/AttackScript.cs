using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    // Variables
    public string GunName = "";
    public int AttackHit = 0; // 0 Not hit 1 has hit
    public float AttackMobDamage = 10f;
    public float[] AttackPushForce = new float[] { 0f, 0f };
    public float AttackPropertyDamage = 5f;
    public float AttackDistance = 0f;
    public float[] AttackSpeeds;
    public float[] Lifetime = new float[] { 0f, 0.1f };
    public float DrunknessPower = 1f;
    public string AttackMechanic = "HitScan";
    public string AttackType = "";
    public string FirearmType = "";
    public GameObject Attacker;
    public GameObject HitDetector;
    public AudioSource GunFireObj;
    public GameObject EffectPrefab;
    public GameObject SpecialPrefab;
    public GameObject ItemPrefab;
    public GameObject Slimend;
    GameObject FollowSlimend;
    public GameObject BulletChamber;
    public bool IsSilenced = false;
    public bool DiesInWater = false;
    public bool HitWater = false;
    public string PenetrationStatus = "";
    // Variables

    // References
    public GameObject AcidSpit;
    public GameObject Panzerfaust;
    public GameObject Arrow;
    public GameObject Bullet;
    public GameObject BulletCase;
    public GameObject Grenade;
    public GameObject LightningBolt;
    public GameObject FlameThrower;
    public GameObject FireExtinguisher;
    public GameObject BubblesProjectile;
    public GameObject BubblesHitScan;
    public GameObject BubblesFlameThrower;
    public AudioClip[] Gunfires;

    public GameScript GS;
    public RoundScript RS;
    // References

    // Misc
    public float MeleeDurability = 0f;
    public int WchichItemWasHeld = 0;
    public float SpecialGunSpread = -1f;
    Vector3 HittedPositon;
    public bool GunFire = true;
    public bool CanHurtSelf = false;
    public GameObject WaterNull;
    Vector3 SlimenPos;
    float DropBullet = 0f;
    // Misc

	// Use this for initialization
	void Start () {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GS.GetComponent<RoundScript>();

        Lifetime = new float[] { 0.25f, 0.1f };
        float GunSpread = 0f;

        if(Slimend) SlimenPos = Slimend.transform.position; else {Slimend = this.gameObject; SlimenPos = this.transform.position;}
        if (BulletChamber == null) BulletChamber = Slimend;

        switch(GunName){
            case "Flashlight":
                AttackMobDamage = Random.Range(2f, 5f);
                AttackPushForce = new float[] { 1f, 0f };
                AttackPropertyDamage = Random.Range(2f, 5f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "Knife": case "Bayonet":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "Crowbar":
                AttackMobDamage = Random.Range(5f, 20f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(25f, 50f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "FireAxe":
                AttackMobDamage = Random.Range(25f, 50f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(25f, 50f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                FirearmType = "Axe";
                break;
            case "MutantBite":
                AttackMobDamage = Random.Range(10f, 25f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = 0f;
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "MutantBite";
                break;
            case "StrongMutantBite":
                AttackMobDamage = Random.Range(25f, 50f);
                AttackPushForce = new float[] { 10f, 10f };
                AttackPropertyDamage = 0f;
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "MutantBite";
                break;
            case "MutantSpit":
                AttackMobDamage = Random.Range(5f, 20f);
                AttackPropertyDamage = 0f;
                AttackSpeeds = new float[] { 50f, 0f };
                Lifetime = new float[] { 1f, 5f };
                GunSpread = 2f;
                AttackMechanic = "Projectile";
                AttackType = "MutantSpit";
                AcidSpit.SetActive(true);
                break;
            case "Machete":
                AttackMobDamage = Random.Range(10f, 20f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                FirearmType = "Axe";
                break;
            case "BaseballBat":
                AttackMobDamage = Random.Range(25f, 50f);
                AttackPushForce = new float[] { 6f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "Colt":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 2f, 0f };
                AttackPropertyDamage = Random.Range(5f, 10f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Pistol";
                break;
            case "Luger":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 2f, 0f };
                AttackPropertyDamage = Random.Range(15f, 25f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 5f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Pistol";
                break;
            case "Revolver":
                AttackMobDamage = Random.Range(25, 50f);
                AttackPushForce = new float[] { 6f, 0f };
                AttackPropertyDamage = Random.Range(25, 50f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 1f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Pistol";
                break;
            case "HunterRifle":
                AttackMobDamage = Random.Range(25f, 50f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(25f, 50f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 0.5f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "DBShotgun":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(15f, 25f);
                DiesInWater = true;
                AttackDistance = 25f;
                GunSpread = 5f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Shotgun";
                break;
            case "Thompson":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 2f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 3f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "SMG";
                break;
            case "AK-47":
                AttackMobDamage = Random.Range(10f, 20f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "Shotgun":
                AttackMobDamage = Random.Range(10f, 15f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(5f, 15f);
                DiesInWater = true;
                AttackDistance = 25f;
                GunSpread = 5f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Shotgun";
                break;
            case "MP5":
                AttackMobDamage = Random.Range(2f, 5f);
                AttackPushForce = new float[] { 2f, 0f };
                AttackPropertyDamage = Random.Range(1f, 5f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 3f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "SMG";
                break;
            case "M4":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(10f, 25f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 1f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "M4";
                break;
            case "Sten":
                AttackMobDamage = Random.Range(2f, 8f);
                AttackPushForce = new float[] { 2f, 0f };
                AttackPropertyDamage = Random.Range(5f, 10f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 3f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "SMG";
                break;
            case "Garand": case "GarandR":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 5f, 0f };
                AttackPropertyDamage = Random.Range(10f, 25f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 0.5f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "Famas":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(10f, 25f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "Uzi":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 2f, 0f };
                AttackPropertyDamage = Random.Range(5f, 10f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 3f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "SMG";
                break;
            case "G3":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(10f, 25f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 0.5f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "Scar":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(10f, 25f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "SPAS":
                AttackMobDamage = Random.Range(10f, 15f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(5f, 15f);
                DiesInWater = true;
                AttackDistance = 25f;
                GunSpread = 5f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Shotgun";
                break;
            case "SAW":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 5f, 0f };
                AttackPropertyDamage = Random.Range(10f, 25f);
                DiesInWater = true;
                AttackDistance = 50f;
                GunSpread = 3f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "Minigun":
                AttackMobDamage = Random.Range(15f, 25f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(10f, 25f);
                DiesInWater = true;
                AttackDistance = 50f;
                GunSpread = 3f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "MosinNagant":
                AttackMobDamage = Random.Range(50f, 100f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(50f, 100f);
                DiesInWater = true;
                AttackDistance = 150f;
                GunSpread = 0f;
                AttackMechanic = "HitScan";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "SapphireSpear":
                AttackMobDamage = 1000000f;
                AttackPropertyDamage = 1000000f;
                AttackDistance = 5f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "Rocket": case "GrenadeLauncher": case "Bazooka":
                AttackMobDamage = 0f;
                AttackPushForce = new float[] { 10f, 10f };
                AttackPropertyDamage = 0f;
                if (GunName == "Bazooka") {
                    AttackSpeeds = new float[] { 50f, 0f };
                    Lifetime = new float[] { 1f, 0f };
                } else {
                    AttackSpeeds = new float[] { 50f, 30f };
                    Lifetime = new float[] { 5f, 0f };
                }
                DiesInWater = true;
                GunSpread = 2f;
                AttackMechanic = "Projectile";
                AttackType = "Rocket";
                if (GunName == "Rocket" || GunName == "Bazooka") {
                    Panzerfaust.SetActive(true);
                } else {
                    Grenade.SetActive(true);
                }
                break;
            case "Arrow":
                AttackMobDamage = 50f;
                AttackPropertyDamage = 5f;
                AttackSpeeds = new float[] { 50f, 10f };
                Lifetime = new float[] { 5f, 5f };
                DiesInWater = true;
                GunSpread = 0.5f;
                AttackMechanic = "Projectile";
                AttackType = "Arrow";
                Arrow.SetActive(true);
                break;
            case "Chainsaw":
                AttackMobDamage = Random.Range(1f, 5f);
                AttackPropertyDamage = Random.Range(1f, 5f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                FirearmType = "Axe";
                break;
            case "LightningBolt":
                AttackMobDamage = Random.Range(25f, 50f);
                AttackPropertyDamage = Random.Range(1f, 25f);
                Lifetime = new float[] { 0.1f, 0.1f };
                AttackDistance = 25f;
                GunSpread = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Electricity";
                LightningBolt.SetActive(true);
                break;
            case "FlameThrower":
                AttackMobDamage = 5f;
                AttackPushForce = new float[] { 0f, 0f };
                AttackPropertyDamage = Random.Range(25f, 50f);
                Lifetime = new float[] { 0.5f, 2f };
                DiesInWater = true;
                AttackDistance = 6f;
                GunSpread = 0f;
                AttackMechanic = "HitScan";
                AttackType = "Fire";
                FlameThrower.SetActive(true);
                break;
            case "Musket":
                AttackMobDamage = Random.Range(50f, 100f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(25f, 50f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 0.5f;
                AttackMechanic = "Projectile";
                AttackType = "Gun";
                FirearmType = "Blackpowder";
                break;
            case "Plunger":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "Shovel":
                AttackMobDamage = Random.Range(10f, 15f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(15f, 25f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "FireExtinguisher":
                AttackMobDamage = 0f;
                AttackPushForce = new float[] { 10f, 10f };
                AttackPropertyDamage = 0f;
                Lifetime = new float[] { 0.5f, 2f };
                AttackDistance = 6f;
                GunSpread = 0f;
                AttackMechanic = "HitScan";
                AttackType = "Water";
                FireExtinguisher.SetActive(true);
                break;
            case "Katana":
                AttackMobDamage = Random.Range(20f, 25f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "Spear":
                AttackMobDamage = Random.Range(10f, 20f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 5f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "G18":
                AttackMobDamage = Random.Range(2f, 5f);
                AttackPushForce = new float[] { 1f, 0f };
                AttackPropertyDamage = Random.Range(1f, 2f);
                DiesInWater = true;
                AttackDistance = 75f;
                GunSpread = 3f;
                AttackMechanic = "Projectile";
                AttackType = "Gun";
                FirearmType = "Pistol";
                break;
            case "FryingPan":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "M1Carbine":
                AttackMobDamage = Random.Range(10f, 15f);
                AttackPushForce = new float[] { 5f, 0f };
                AttackPropertyDamage = Random.Range(10f, 15f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 0.5f;
                AttackMechanic = "Projectile";
                AttackType = "Gun";
                FirearmType = "Rifle";
                break;
            case "Sledgehammer":
                AttackMobDamage = Random.Range(25f, 50f);
                AttackPushForce = new float[] { 10f, 10f };
                AttackPropertyDamage = Random.Range(100f, 200f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "StoneAxe":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(5f, 20f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                FirearmType = "Axe";
                break;
            case "Fokos":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(2f, 5f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                FirearmType = "Axe";
                break;
            case "Sword":
                AttackMobDamage = Random.Range(100f, 200f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(25f, 50f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                break;
            case "Pickaxe":
                AttackMobDamage = Random.Range(5f, 10f);
                AttackPushForce = new float[] { 3f, 0f };
                AttackPropertyDamage = Random.Range(50f, 100f);
                AttackDistance = 2f;
                AttackMechanic = "HitScan";
                AttackType = "Melee";
                FirearmType = "Pickaxe";
                break;
            case "Flintlock":
                AttackMobDamage = Random.Range(100f, 200f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(50f, 100f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 10f;
                AttackMechanic = "Projectile";
                AttackType = "Gun";
                FirearmType = "Blackpowder";
                break;
            case "BakerRifle":
                AttackMobDamage = Random.Range(100f, 200f);
                AttackPushForce = new float[] { 10f, 0f };
                AttackPropertyDamage = Random.Range(25f, 50f);
                DiesInWater = true;
                AttackDistance = 200f;
                GunSpread = 0f;
                AttackMechanic = "Projectile";
                AttackType = "Gun";
                FirearmType = "Blackpowder";
                break;
            case "NockGun":
                AttackMobDamage = Random.Range(50f, 100f);
                AttackPushForce = new float[] { 30f, 0f };
                AttackPropertyDamage = Random.Range(50f, 100f);
                DiesInWater = true;
                AttackDistance = 100f;
                GunSpread = 10f;
                AttackMechanic = "Projectile";
                AttackType = "Gun";
                FirearmType = "Blackpowder";
                break;
            default:
                Destroy(this.gameObject);
                break;
        }

        if(AttackType == "Gun"){
            AttackMechanic = "Projectile";
            AttackSpeeds = new float[]{Random.Range(200f, 300f), 10f};
            if(GameObject.Find("_RoundScript").GetComponent<RoundScript>().IsCausual) AttackMechanic = "HitScan";
            else AttackMechanic = "Projectile";
        }

        AttackMobDamage *= Mathf.Clamp(DrunknessPower, 1f, Mathf.Infinity);
        AttackPropertyDamage *= Mathf.Clamp(DrunknessPower, 1f, Mathf.Infinity);

        // Check if in water
        bool UnderWater = false;
        if (DiesInWater == true) {
            Ray CheckIfInWater = new Ray(this.transform.position, Vector3.up);
            foreach (RaycastHit CheckIfInWaterHIT in Physics.RaycastAll(CheckIfInWater, Mathf.Infinity)) {
                if (CheckIfInWaterHIT.collider.gameObject.layer == 16) {
                    UnderWater = true;
                }
            }
        }

        // Gunfires
        string[] GunfiresoundSettings = {""};
        switch(GunName){
            case "Flashlight": case "Knife": case  "Bayonet": case  "Crowbar": case  "FireAxe": case  "Machete": case  "BaseballBat": case "SapphireSpear": case "Katana": case "Spear": case "FryingPan": case "Sledgehammer": case "Plunger": case "Shovel": case "StoneAxe": case "Fokos": case "Sword": case "Pickaxe":
                string[] Swings = {"Swing1", "Swing2", "Swing3"};
                GunfiresoundSettings = new string[]{ Swings[(int)Random.Range(0f, 2.9f)], "100", "Invisible" };
                break;
            case "MutantSpit":
            GameObject Gunfire2 = Instantiate(EffectPrefab) as GameObject;
            Gunfire2.GetComponent<EffectScript>().EffectName = "Spit";
            Gunfire2.transform.position = Slimend.transform.position + (Slimend.transform.forward / 2f);
            Gunfire2.transform.eulerAngles = this.transform.eulerAngles;
                break;
            case "Chainsaw":
                GunfiresoundSettings = new string[]{ "Chainsaw", "100", "Invisible" };
                break;
            case "Arrow":
            GameObject Gunfire4 = Instantiate(EffectPrefab) as GameObject;
            Gunfire4.GetComponent<EffectScript>().EffectName = "Arrow";
            Gunfire4.transform.position = Slimend.transform.position + (Slimend.transform.forward / 2f);
            Gunfire4.transform.eulerAngles = this.transform.eulerAngles;
                break;
            case "Rocket":
            GameObject Gunfire5 = Instantiate(EffectPrefab) as GameObject;
            Gunfire5.GetComponent<EffectScript>().EffectName = "Rocket";
            Gunfire5.transform.position = Slimend.transform.position + (Slimend.transform.forward / 2f);
            Gunfire5.transform.eulerAngles = this.transform.eulerAngles;
                break;
            case "GrenadeLauncher":
            GameObject Gunfire6 = Instantiate(EffectPrefab) as GameObject;
            Gunfire6.GetComponent<EffectScript>().EffectName = "GrenadeLauncher";
            Gunfire6.transform.position = Slimend.transform.position + (Slimend.transform.forward / 2f);
            Gunfire6.transform.eulerAngles = this.transform.eulerAngles;
                break;
            case "LightningBolt":
                GameObject Gunfire7 = Instantiate(EffectPrefab) as GameObject;
                Gunfire7.GetComponent<EffectScript>().EffectName = "LightningBolt";
                Gunfire7.transform.position = Slimend.transform.position + (Slimend.transform.forward / 2f);
                Gunfire7.transform.eulerAngles = this.transform.eulerAngles;
                if (LightningBolt.activeInHierarchy == true) {
                    LightningBolt.transform.localScale = new Vector3(0f, Random.Range(1f, 2f), AttackDistance);
                    LightningBolt.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                    LightningBolt.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Random.Range(0f, 1f));
                }
                break;
            case "FlameThrower":
            GameObject Gunfire8 = Instantiate(EffectPrefab) as GameObject;
            if (UnderWater == true) {
                Gunfire8.GetComponent<EffectScript>().EffectName = "WaterGun";
            } else {
                Gunfire8.GetComponent<EffectScript>().EffectName = "FlameThrower";
            }
            Gunfire8.transform.position = Slimend.transform.position + (Slimend.transform.forward / 2f);
            Gunfire8.transform.eulerAngles = this.transform.eulerAngles;
            FlameThrower.transform.position = Slimend.transform.position;
            if (Attacker != null) {
                if (Attacker.tag == "Player") {
                    FlameThrower.transform.SetParent(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().LookDir.transform);
                    ParticleSystem.MainModule SetGrav = FlameThrower.GetComponent<ParticleSystem>().main;
                    SetGrav.gravityModifier = SetGrav.gravityModifier.constantMin / 4f;
                }
            }
                break;
            case "Colt": case  "Luger": case "Revolver": case "HunterRifle": case "DBShotgun": case "Thompson": case "AK-47": case "Shotgun": case "MP5": case "M4": case "Sten": case "Garand": case "GarandR": case "Famas": case "Uzi": case "G3": case "Scar": case "SPAS": case "SAW": case "Minigun": case "MosinNagant": case "Musket": case "G18": case "M1Carbine": case "Flintlock": case "BakerRifle": case "NockGun":

                if (GunFire == true) {
                    GunfiresoundSettings = new string[]{ GunName, "100", "Gun" };
                    //GameObject Gunfire9 = Instantiate(EffectPrefab) as GameObject;
                    //Gunfire9.transform.position = Slimend.transform.position;
                    //Gunfire9.transform.eulerAngles = this.transform.eulerAngles;
                    //Gunfire9.GetComponent<EffectScript>().BulletChamber = BulletChamber;
                    //if (Slimend.transform.root.tag == "Player") {
                    //    Gunfire9.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    //    FollowSlimend = Gunfire9;
                    //}
                    Lifetime = new float[] { 9999f, 0.5f };
                    Bullet.SetActive(true);
                    if(PenetrationStatus == "Ricochet" || PenetrationStatus == "Penetration"){
                        GunfiresoundSettings = new string[]{ "Bullet" + PenetrationStatus, "100", "Gun" };
                    } else if (IsSilenced == false && UnderWater == false) {
                        //Gunfire9.GetComponent<EffectScript>().EffectName = GunName;
                        // Mobs heard
                        foreach (GameObject MobHeard in GameObject.FindGameObjectsWithTag("Mob")) if (MobHeard != Attacker && Vector3.Distance(this.transform.position, MobHeard.transform.position) < 100f) {
                            MobHeard.GetComponent<MobScript>().React("Curious", 10f, this.transform.position);
                        }
                        foreach (GameObject MobHeard in GameObject.FindGameObjectsWithTag("MobPH")) if (MobHeard.GetComponent<MobPH>().Curious && Vector3.Distance(this.transform.position, MobHeard.transform.position) < 100f) {
                            MobHeard.GetComponent<MobPH>().Attract(this.transform.position, 10f);
                        }
                    } else if (UnderWater == true) {
                        //Gunfire9.GetComponent<EffectScript>().EffectName = "WaterGun";
                        GunfiresoundSettings = new string[]{ "WaterGun", "100", "Gun" };
                    } else if (IsSilenced == true) {
                        //Gunfire9.GetComponent<EffectScript>().EffectName = "Suppressor";
                        GunfiresoundSettings = new string[]{ "Suppressor", "100", "Gun" };
                    }

                }

                break;
            case "FireExtinguisher":
                GameObject Gunfire10 = Instantiate(EffectPrefab) as GameObject;
                Gunfire10.GetComponent<EffectScript>().EffectName = "FireExtinguisher";
                Gunfire10.transform.position = Slimend.transform.position + (Slimend.transform.forward / 2f);
                Gunfire10.transform.eulerAngles = this.transform.eulerAngles;
                FireExtinguisher.transform.position = Slimend.transform.position;
                if (Attacker != null) {
                    if (Attacker.tag == "Player") {
                        FireExtinguisher.transform.SetParent(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().LookDir.transform);
                        ParticleSystem.MainModule SetGrav = FireExtinguisher.GetComponent<ParticleSystem>().main;
                        SetGrav.gravityModifier = SetGrav.gravityModifier.constantMin / 4f;
                    }
                }
                break;
        }

        if(GunfiresoundSettings[0] != "" && GunFire)
        foreach(AudioClip GetGunFireClip in GameObject.Find("_RoundScript").GetComponent<RoundScript>().GunfireSounds){
            if(GetGunFireClip.name == GunfiresoundSettings[0]){
                if(GunfiresoundSettings[2] == "Invisible"){
                    this.transform.position += this.transform.forward / 10f;
                    if(Lifetime[1] < GetGunFireClip.length) Lifetime[1] = GetGunFireClip.length;
                    GunFireObj.clip = GetGunFireClip;
                    GunFireObj.maxDistance = float.Parse(GunfiresoundSettings[1]);
                    GunFireObj.Play();
                } else if(GunfiresoundSettings[2] == "Gun"){
                    if(Slimend) {
                        GunFireObj.transform.position = Slimend.transform.position;
                        Bullet.transform.localPosition += GunFireObj.transform.localPosition;
                    }
                    this.transform.position += this.transform.forward / 10f;
                    Lifetime[1] = 5f;
                    GunFireObj.clip = GetGunFireClip;
                    GunFireObj.maxDistance = float.Parse(GunfiresoundSettings[1]);
                    GunFireObj.Play();

                    string Gunfiretype = "Rifle";
                    bool FlashGunfire = true;
                    switch(GunName){
                        case "Colt": case "Luger": case "Revolver": case "Thompson": case "MP5": case "Sten": case "Uzi": case "G18":
                            Gunfiretype = "Pistol";
                            if(GunName == "Revolver") DropBullet = 9999f;
                            break;
                        case "DBShotgun": case "Shotgun": case "SPAS":
                            Gunfiretype = "Shotgun";
                            if(GunName == "Shotgun") DropBullet = 0.5f;
                            break;
                        case "HunterRifle": case "MosinNagant":
                            Gunfiretype = "Rifle";
                            DropBullet = 0.5f;
                            break;
                        case "Musket": case "Flintlock": case "BakerRifle": case "NockGun":
                            Gunfiretype = "BlackPowder";
                            DropBullet = 9999f;
                            break;
                    }
                    if(GunfiresoundSettings[0] == "Bullet" + PenetrationStatus) {Gunfiretype = PenetrationStatus; DropBullet = 9999f; FlashGunfire = false;}
                    else if(GunfiresoundSettings[0] == "WaterGun" || GunfiresoundSettings[0] == "Suppressor") {Gunfiretype = "null"; FlashGunfire = false;}
                    for(int GT = 0; GT < GunFireObj.transform.childCount; GT ++){
                        if(GunFireObj.transform.GetChild(GT).name == Gunfiretype) {
                            GunFireObj.transform.GetChild(GT).gameObject.SetActive(true);
                            ParticleSystem.MainModule SetCol = Bullet.GetComponent<ParticleSystem>().main;
                            SetCol.startColor = GunFireObj.transform.GetChild(GT).GetComponent<ParticleSystem>().main.startColor;
                            if(GS.LightingQuality > 1 && FlashGunfire && Random.Range(0f, 1f) < 0.3f) {
                                GunFireObj.transform.GetChild(GT).GetComponent<ParticleSystem>().Play();
                                Light SetLight = GunFireObj.gameObject.GetComponent<Light>();
                                SetLight.enabled = true;
                                SetLight.color = Bullet.GetComponent<ParticleSystem>().main.startColor.color;
                                SetLight.intensity = 2f;
                            }
                            BulletCase.transform.SetParent(GunFireObj.transform);
                        }
                    }
                    string Bullettype = Gunfiretype;
                    if(GunName == "GarandR") Bullettype = "Mag";
                    for(int BC = 0; BC < BulletCase.transform.childCount; BC ++){
                        if(BulletCase.transform.GetChild(BC).name == Bullettype) BulletCase.transform.GetChild(BC).SetSiblingIndex(0);
                    }
                }
            }
        }

        if (SpecialGunSpread < 0f) {
            this.transform.Rotate(Random.Range(-GunSpread, GunSpread), Random.Range(-GunSpread, GunSpread), 0f);
        } else {
            this.transform.Rotate(Random.Range(-SpecialGunSpread, SpecialGunSpread), Random.Range(-SpecialGunSpread, SpecialGunSpread), 0f);
        }
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        // Check if in water
        if (DiesInWater == true && AttackHit == 0) {
            Ray CheckIfInWater = new Ray(this.transform.position, Vector3.up);
            foreach (RaycastHit CheckIfInWaterHIT in Physics.RaycastAll(CheckIfInWater, Mathf.Infinity)) {
                if (CheckIfInWaterHIT.collider.gameObject.layer == 16) {
                    Hit(WaterNull, this.transform.position);
                }
            }
        }
        
        if (AttackMechanic == "HitScan" && AttackHit == 0) {
            Ray AttackCheck = new Ray(this.transform.position, this.transform.forward);
            RaycastHit AttackCheckHIT;
            if (Physics.Raycast(AttackCheck, out AttackCheckHIT, AttackDistance, GameObject.Find("_GameScript").GetComponent<GameScript>().IngoreMaskAttack)) {
                if ((AttackCheckHIT.collider.gameObject != Attacker && AttackCheckHIT.collider.transform.root.gameObject != Attacker) || CanHurtSelf == true) {
                    Hit(AttackCheckHIT.collider.gameObject, AttackCheckHIT.point);
                } else {
                    HittedPositon = this.transform.position + (this.transform.forward * AttackDistance);
                }
            }
        } else if (AttackMechanic == "Projectile" && AttackHit == 0 && HitDetector.transform.position != this.transform.position) {
            HitDetector.transform.parent = null;
            HitDetector.transform.LookAt(this.transform.position);
            Ray AttackCheck = new Ray(HitDetector.transform.position, HitDetector.transform.forward);
            RaycastHit AttackCheckHIT;
            if (Physics.Raycast(AttackCheck, out AttackCheckHIT, Vector3.Distance(HitDetector.transform.position, this.transform.position) + 1f, GameObject.Find("_GameScript").GetComponent<GameScript>().IngoreMaskAttack)) {
                if (AttackCheckHIT.collider.gameObject != Attacker) {
                    Hit(AttackCheckHIT.collider.gameObject, AttackCheckHIT.point);
                }
            }
            HitDetector.transform.position = this.transform.position;
        }

        if (Grenade && Grenade.activeInHierarchy == true) {
            Grenade.transform.Rotate(10f, 10f, 10f);
        }

        if (AttackType == "Gun" && (AttackHit == 0 || HitWater)) {
            this.transform.position += this.transform.forward * (AttackSpeeds[0] / 50f);
            this.transform.eulerAngles += Vector3.right * (Vector3.Distance(SlimenPos, this.transform.position) / AttackDistance) / 5f;
        } else if (AttackMechanic == "Projectile" && (AttackHit == 0 || HitWater)) {
            this.transform.position += this.transform.forward * (AttackSpeeds[0] / 50f);
            this.transform.eulerAngles += Vector3.right * (AttackSpeeds[1] / 50f);
        }

        if (GunFireObj && Slimend){ //&& FollowSlimend != null){
            GunFireObj.transform.position = SlimenPos;
            if(GunFireObj.GetComponent<Light>() && (Time.time * 10f) - (int)(Time.time * 10f) < 0.25f) Destroy(GunFireObj.GetComponent<Light>());
        //    //FollowSlimend.transform.position = Slimend.transform.position;
        //    //FollowSlimend.transform.eulerAngles = Slimend.transform.eulerAngles;
        }

        // Removing
        if (Lifetime[0] > 0f) {
            Lifetime[0] -= 0.02f;
        } else if (AttackHit == 0) {
            AttackHit = 1;
        } 
        if (Lifetime[1] > 0f && AttackHit == 1) {
            Lifetime[1] -= 0.02f;
        } else if (Lifetime[1] <= 0f && AttackHit == 1) {
            Destroy(this.gameObject);
        }

        // Bullet Effect
        if (Bullet && Bullet.activeInHierarchy == true) {

            if (AttackHit == 0) {
                if (Slimend && !HitWater && Vector3.Distance(Bullet.transform.position, GameObject.Find("MainCamera").transform.position) < 5f && Vector3.Distance(Slimend.transform.position, GameObject.Find("MainCamera").transform.position) >= 10f && Bullet.name == "S_Bullet" && Bullet.GetComponent<AudioSource>().isPlaying == false) {
                    Bullet.name = "S_Bulleted";
                    Bullet.GetComponent<AudioSource>().Play();
                }
            }

            if(HitWater && BubblesHitScan){
                Bullet.SetActive(false);
                BubblesHitScan.GetComponent<ParticleSystem>().Play();
                ParticleSystem.MainModule SetCol = BubblesHitScan.GetComponent<ParticleSystem>().main;
                SetCol.startColor = RenderSettings.fogColor;
                Debug.Log("Bloop");
            }

        } else if (FlameThrower != null) {
            if (FlameThrower.activeInHierarchy == true && QualitySettings.GetQualityLevel() > 0) {
                FlameThrower.GetComponent<Light>().enabled = true;
                FlameThrower.GetComponent<Light>().intensity = (FlameThrower.GetComponent<ParticleSystem>().particleCount / 20f) * 0.5f;
            }
        }
        // Bullet Effect

        // Bullet drop
        if(BulletCase){
            if(DropBullet > 0f){
                DropBullet -= 0.02f;
            } else if (DropBullet > - 100f) {
                DropBullet = -200f;
                BulletCase.transform.GetChild(0).gameObject.SetActive(true);
                BulletCase.transform.position = BulletChamber.transform.position;
                BulletCase.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                BulletCase.GetComponent<Rigidbody>().velocity = this.transform.up + this.transform.right;
                BulletCase.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
            }
        }
        // Bullet drop

        // Water Effects
        if (HitWater == true && QualitySettings.GetQualityLevel() > 0) {
            if ((Arrow && Arrow.activeInHierarchy == true) || (Grenade && Grenade.activeInHierarchy == true) || (Panzerfaust && Panzerfaust.activeInHierarchy == true)) {
                this.transform.position += (this.transform.forward * 0.01f) + Vector3.down * 0.01f;
                BubblesProjectile.SetActive(true);
                ParticleSystem.MainModule SetCol = BubblesProjectile.GetComponent<ParticleSystem>().main;
                SetCol.startColor = RenderSettings.fogColor;
            }
            if (GameObject.FindGameObjectWithTag("Player") != null && BubblesHitScan && BubblesHitScan.activeInHierarchy == true && BubblesHitScan.transform.eulerAngles.x > 270f) {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().IsSwimming == false) {
                    BubblesHitScan.SetActive(false);
                }
            }
        }
		
	}

    void Hit(GameObject ObjectHit, Vector3 PointHit) {

        HittedPositon = PointHit;

        bool CanHit = true;
        string Penetrate = "";
        if (ObjectHit != null && AttackHit == 0) {
            if ((ObjectHit.layer == 16 || ObjectHit.layer == 4) && DiesInWater == false) {
                CanHit = false;
            }
            if (Attacker != null) {
                if (ObjectHit == Attacker || ObjectHit.transform.root.gameObject == Attacker) {
                    CanHit = false;
                }
            }
        }

        float TraveledDistance = Vector3.Distance(SlimenPos, PointHit);

        if (ObjectHit != null && CanHit == true && AttackHit == 0) {

            if(Bullet) this.transform.position = PointHit;

            // Check if water
            if (ObjectHit.layer == 16 || ObjectHit.layer == 4 || ObjectHit == WaterNull) {
                HitWater = true;
                if ((Arrow && Arrow.activeInHierarchy == true) || (Grenade && Grenade.activeInHierarchy == true) || (Panzerfaust && Panzerfaust.activeInHierarchy == true)) {
                    Lifetime[1] = 10f;
                } else if (Bullet && Bullet.activeInHierarchy == true) {
                    AttackSpeeds[0] /= 100f;
                    AttackMobDamage = 0f;
                    AttackPropertyDamage = 0f;
                    Lifetime = new float[]{0f, 3f};
                    Bullet.GetComponent<ParticleSystem>().Stop();
                    Bullet.name = "S_Bulleted";
                } else if (FlameThrower && FlameThrower.activeInHierarchy == true) {
                    Lifetime[1] = 10f;
                    BubblesFlameThrower.SetActive(true);
                    ParticleSystem.MainModule SetCol = BubblesFlameThrower.GetComponent<ParticleSystem>().main;
                    SetCol.startColor = RenderSettings.fogColor;
                    BubblesFlameThrower.transform.position = FlameThrower.transform.position;
                    Destroy(FlameThrower);
                }
            }

            if (Attacker != null && GS.GameModePrefab.x == 0) {
                if (Attacker.GetComponent<PlayerScript>() != null && ObjectHit != null) {
                    if(MeleeDurability != 0) {
                        if (GS.GameModePrefab.y == 1)
                            MeleeDurability /= 4f;

                        Attacker.GetComponent<PlayerScript>().Inventory[WchichItemWasHeld] = GS.SetSemiClass(Attacker.GetComponent<PlayerScript>().Inventory[WchichItemWasHeld], "va", "/+-" + MeleeDurability.ToString(CultureInfo.InvariantCulture));//Attacker.GetComponent<PlayerScript>().Inventory[WchichItemWasHeld].y -= MeleeDurability;
                    }
                    if (ObjectHit.GetComponent<FootstepMaterial>() != null) {
                        string ItemID = GS.GetSemiClass(Attacker.GetComponent<PlayerScript>().Inventory[WchichItemWasHeld], "id");

                        if (ItemID == "115") {
                            int Digup = Random.Range(0, 10);

                            if (ObjectHit.GetComponent<FootstepMaterial>().WhatToPlay == "Grass" && GS.GameModePrefab.x == 0 && Digup <= 3) {
                                GameObject DigupItem = Instantiate(ItemPrefab) as GameObject;
                                DigupItem.transform.position = PointHit + Vector3.up / 4f;

                                if(Digup == 0)
                                    DigupItem.GetComponent<ItemScript>().Variables = GS.itemCache[(int)Random.Range(1, GameObject.Find("_RoundScript").GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables;
                                else {
                                    int[] materials = new[]{142, 145, 147, 146, 146, 146};
                                    DigupItem.GetComponent<ItemScript>().Variables = GS.itemCache[materials[(int)Random.Range(0f, 5.9f)]].startVariables;
                                }
                            }
                        } else if (ItemID == "156") {
                            Attacker.GetComponent<PlayerScript>().PushbackForce = Vector3.up * 12f;
                            Attacker.GetComponent<PlayerScript>().ReturnPushback = 1f;
                        }
                    }
                }
            }

            Lifetime[0] = 0f;
            if(ObjectHit.GetComponent<DestructionScript>()){
                ObjectHit.GetComponent<DestructionScript>().Hit(AttackPropertyDamage, new string[]{AttackType, FirearmType}, PointHit);
            } else if(ObjectHit.layer == 24){
                ObjectHit.transform.parent.GetComponent<DestructionScript>().Hit(AttackPropertyDamage, new string[]{AttackType, FirearmType}, PointHit);
            } 
            if (ObjectHit.GetComponent<FootstepMaterial>() != null) {
                GameObject BulletHit = Instantiate(EffectPrefab) as GameObject;
                BulletHit.GetComponent<EffectScript>().EffectName = "Bullethit" + ObjectHit.GetComponent<FootstepMaterial>().WhatToPlay;
                BulletHit.transform.position = PointHit;
                BulletHit.transform.LookAt(this.transform.position - this.transform.forward * 1f);
                Penetrate = "Sure";
            } else if (ObjectHit.GetComponent<PlayerScript>() != null && ObjectHit.GetComponent<PlayerScript>().Health[0] > 0f) {
                ObjectHit.GetComponent<PlayerScript>().Hurt(AttackMobDamage, AttackType, true, this.transform.position);
                Ray CheckForGrounded = new Ray(ObjectHit.transform.position, Vector3.down);
                RaycastHit CheckForGroundedHIT;
                if (Physics.Raycast(CheckForGrounded, out CheckForGroundedHIT, 1.1f)) {
                    ObjectHit.GetComponent<PlayerScript>().PushbackForce = new Vector3(this.transform.forward.x * AttackPushForce[0], AttackPushForce[1], this.transform.forward.z * AttackPushForce[0]);
                    ObjectHit.GetComponent<PlayerScript>().ReturnPushback = 1f;
                }
                if (GunName == "FlameThrower") {
                    ObjectHit.GetComponent<PlayerScript>().Fire = Mathf.Clamp(ObjectHit.GetComponent<PlayerScript>().Fire + 5f, 15f, 100f);
                } else if (GunName == "FireExtinguisher") {
                    ObjectHit.GetComponent<PlayerScript>().Fire = Mathf.Clamp(ObjectHit.GetComponent<PlayerScript>().Fire - 5f, 0f, 100f);
                }
            } else if (ObjectHit.transform.root != null && ObjectHit.transform.root.tag == "Mob") {
                int Ach_Knife = 0;
                if (ObjectHit.transform.root.GetComponent<MobScript>() != null && ObjectHit.transform.root.GetComponent<MobScript>().MobHealth[0] > 0f) {
                    if((GunName == "Knife" || GunName == "Bayonet") && ObjectHit.transform.root.GetComponent<MobScript>().MobHealth[0] == ObjectHit.transform.root.GetComponent<MobScript>().MobHealth[1]) Ach_Knife = 1;
                    float DamageMultip = 1f;
                    if (ObjectHit.name == "HEAD") {
                        DamageMultip = 2f;
                    } else if (ObjectHit.name != "TORSO") {
                        DamageMultip = 0.5f;
                    }
                    if (ObjectHit.transform.root.GetComponent<MobScript>().Angered <= 0f && ObjectHit.transform.root.GetComponent<MobScript>().Curious <= 0f && ObjectHit.transform.root.GetComponent<MobScript>().Panic <= 0f) {
                        if (AttackType == "Gun" || AttackType == "Melee") {
                            DamageMultip *= 2f;
                        }
                    }
                    ObjectHit.transform.root.GetComponent<MobScript>().Hurt(AttackMobDamage * DamageMultip, Attacker, true, HittedPositon, AttackType, ObjectHit);
                    ObjectHit.transform.root.GetComponent<MobScript>().PushbackForce = new Vector3(this.transform.forward.x * AttackPushForce[0], AttackPushForce[1], this.transform.forward.z * AttackPushForce[0]);
                    ObjectHit.transform.root.GetComponent<MobScript>().ReturnPushBack = 1f;
                    if(Ach_Knife == 1 && ObjectHit.transform.root.GetComponent<MobScript>().MobHealth[0] <= 0f) GS.PS.AchProg("Ach_Shanked", "0");
                    int Chance = Random.Range(0, 100);
                    if (GunName == "FlameThrower") {
                        ObjectHit.transform.root.GetComponent<MobScript>().Fire = Mathf.Clamp(ObjectHit.transform.root.GetComponent<MobScript>().Fire + 5f, 15f, 100f);
                    } else if (GunName == "FireExtinguisher") {
                        ObjectHit.transform.root.GetComponent<MobScript>().Fire = Mathf.Clamp(ObjectHit.transform.root.GetComponent<MobScript>().Fire - 5f, 0f, 100f);
                    } else if (GunName == "FryingPan" && Chance < 25) {
                        ObjectHit.transform.root.GetComponent<MobScript>().React("Blinded", 2f, this.transform.position);
                    }
                }
            } else if (ObjectHit.layer == 11) {
                ObjectHit.transform.parent.GetComponent<InteractableScript>().Interaction("Break", AttackPropertyDamage);
                Penetrate = "Sure";
            }

            if (Arrow && Arrow.activeInHierarchy == true) {
                this.transform.position = PointHit;
                Arrow.GetComponent<ParticleSystem>().Stop();
                if (ObjectHit != null) {
                    this.transform.SetParent(ObjectHit.transform);
                    if (ObjectHit.tag == "Mob") {
                        if (ObjectHit.GetComponent<MobScript>().WhichModels == "Humanoid") {
                            this.transform.SetParent(ObjectHit.GetComponent<MobScript>().HumanoidBodyParts[1].transform);
                        }
                    }
                }
            } else if (AcidSpit && AcidSpit.activeInHierarchy == true){
                AcidSpit.GetComponent<ParticleSystem>().Stop();
            } else if (Panzerfaust && Panzerfaust.activeInHierarchy == true){
                Panzerfaust.GetComponent<AudioSource>().Stop();
                Panzerfaust.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            } else if (LightningBolt && LightningBolt.activeInHierarchy == true) {
                LightningBolt.transform.localScale = new Vector3(0f, Random.Range(1f, 2f), Vector3.Distance(this.transform.position, PointHit));
                LightningBolt.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                LightningBolt.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Random.Range(0f, 1f));
            }

            if (GunName == "FryingPan") {
                GameObject BulletHit = Instantiate(EffectPrefab) as GameObject;
                BulletHit.GetComponent<EffectScript>().EffectName = "FryingPan";
                BulletHit.transform.position = PointHit;
                BulletHit.transform.LookAt(this.transform.position - this.transform.forward * 1f);
            }

            if(Penetrate == "Sure" && AttackType == "Gun" && TraveledDistance < AttackDistance){
                float PenPower = Mathf.Clamp(DrunknessPower, 0f, 1f);
                // Check for ricochet
                Ray CheckRic = new Ray(this.transform.position + this.transform.up * 0.1f, this.transform.forward);
                RaycastHit CheckRicHit;
                if(PenPower >= 0.5f && Physics.Raycast(CheckRic, out CheckRicHit, 1f)){
                    Vector3 RicVector = Vector3.Reflect(this.transform.forward, CheckRicHit.normal);
                    float BounceChance = 5f;
                    switch(FirearmType){
                        case "Pistol": case "SMG": BounceChance = Random.Range(5f, 90f); break;
                        case "Rifle": BounceChance = Random.Range(-45f, 90f); break;
                        case "Shotgun": case "Blackpowder": BounceChance = 0f; break;
                    }
                    if(Vector3.Angle(this.transform.forward, RicVector) < BounceChance){
                        RoundScript RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
                        RS.Attack(new string[]{ GunName, "Ricochet", "CanHurtSelf" }, CheckRicHit.point, RicVector, Attacker, this.gameObject, this.gameObject);
                        Penetrate = "Ricochet";
                    }
                }
                if(Penetrate == "Sure" && PenPower >= 0.1f && !HitWater){
                    // Check for penetration
                    switch(FirearmType){
                        case "Pistol": case "SMG": PenPower /= Random.Range(4f, 10f); break;
                        case "Rifle": PenPower /= Random.Range(2f, 6f); break;
                    }
                    Ray CheckPen = new Ray(this.transform.position + (this.transform.forward * PenPower), -this.transform.forward);
                    RaycastHit CheckPenHit;
                    if(Physics.Raycast(CheckPen, out CheckPenHit, PenPower)){
                        //print(CheckRicHit.collider.name);
                        RoundScript RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
                        RS.Attack(new string[]{ GunName, "Penetration", "CanHurtSelf" }, CheckPenHit.point, this.transform.forward, Attacker, this.gameObject, this.gameObject);
                        Penetrate = "Penetrated";
                    }
                }
            }

            AttackHit = 1;

        }

    }

    void OnDestroy() {

        if (AttackHit != 0) {

            if (AttackType == "Rocket" && HitWater == false) {
                GameObject Boom = Instantiate(SpecialPrefab) as GameObject;
                Boom.transform.position = this.transform.position;
                Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                Boom.GetComponent<SpecialScript>().CausedBy = Attacker;
            } else if (GunName == "FlameThrower") {
                Destroy(FlameThrower);
                Destroy(BubblesFlameThrower);
            }
            
        }

        Destroy(HitDetector);

    }
}
