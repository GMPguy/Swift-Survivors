using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour {

    // Main Variables
    public string EffectName = "";
    public Color32 EffectColor;
    public float Lifetime = 1f;
    string WBulletCase = "";
    // Main Variables

    // References
    public AudioSource JustSound;
    public AudioClip[] SoundClips;
    public GameObject EatingEffect;
    public GameObject DrinkingEffect;
    public GameObject PatchUpEffect;
    public GameObject BullethitGrass;
    public GameObject BullethitBlock;
    public GameObject BullethitWood;
    public GameObject BullethitMetal;
    public GameObject BullethitWater;
    //public GameObject GunFireInvisible;
    public GameObject GunFireMuzzle;
    public GameObject BulletCase;
    Vector3 BulletCaseVelocity;
    Vector3 BCPost;
    public GameObject BulletChamber;
    float BulletDelay = 0f;
    bool PlayedGunShot = true;
    public GameObject BarrelBreak;
    public GameObject DoorBreak;
    public GameObject BloodSplat;
    public GameObject ItemBreak;
    public GameObject Flash;
    int PickSplat = 0;
    public GameObject GibCorpse;
    public GameObject[] Gibs;

    public GameScript GS;
    // References

	// Use this for initialization
	void Start () {

        string JustSoundsClip = "";
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        // Destroy random
        if(GameObject.FindGameObjectsWithTag("Effect").Length > 10){
            Destroy(GameObject.FindGameObjectWithTag("Effect"));
        }

        switch(EffectName){
            case "Eating":
                EatingEffect.SetActive(true);
                ParticleSystem.MainModule SetColor = EatingEffect.GetComponent<ParticleSystem>().main;
                SetColor.startColor = new ParticleSystem.MinMaxGradient(EffectColor);
                Lifetime = 1f;
                break;
            case "Drinking":
                DrinkingEffect.SetActive(true);
                ParticleSystem.MainModule SetColorA = DrinkingEffect.GetComponent<ParticleSystem>().main;
                SetColorA.startColor = new ParticleSystem.MinMaxGradient(EffectColor);
                Lifetime = 1f;
                break;
            case "PatchUp":
                PatchUpEffect.SetActive(true);
                Lifetime = 2f;
                break;
            case "FootstepGrass": case "FootstepBlock": case "FootstepMetal": case "FootstepWood": case "FootstepWater": case "Swimming": case "Jumping": case "Matches": case "Zip": case "Puffer": case "Unpin": case "Ducktape": case "BlowTorch": case "Cowbell": case "Crank": case "FryingPan": case "BodyDrop": case "Parry":
                JustSoundsClip = EffectName;
                Lifetime = 2f;
                if (EffectName == "BodyDrop" || EffectName == "Jumping") {
                    JustSound.pitch = Random.Range(0.75f, 1f);
                }
                break;
            case "BullethitGrass":

                BullethitGrass.SetActive(true);
                BullethitGrass.GetComponent<ParticleSystem>().Play();
                BullethitGrass.GetComponent<AudioSource>().Play();
                BullethitGrass.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1f);
                Lifetime = 5f;

                if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < 4f) {
                    GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "Dirt";
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().Size = Random.Range(128f, 640f);
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().Epicentrum = (1f - (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) / 4f)) * 0.3f;
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = Color32.Lerp(new Color32(55, 25, 25, 255), new Color32(15, 25, 10, 255), Random.Range(0f, 1f));
                }

                break;
            case "BullethitBlock":
                BullethitBlock.SetActive(true);
                BullethitBlock.GetComponent<ParticleSystem>().Play();
                BullethitBlock.GetComponent<AudioSource>().Play();
                BullethitBlock.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1f);
                Lifetime = 5f;

                if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < 4f) {
                    GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "Dirt";
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().Size = Random.Range(128f, 640f);
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().Epicentrum = (1f - (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) / 4f)) * 0.3f;
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = Color32.Lerp(new Color32(10, 10, 10, 255), new Color32(25, 25, 25, 255), Random.Range(0f, 1f));
                }
                break;
            case "BullethitWater":
                BullethitWater.SetActive(true);
                BullethitWater.GetComponent<ParticleSystem>().Play();
                ParticleSystem.MainModule SetCol = BullethitWater.GetComponent<ParticleSystem>().main;
                SetCol.startColor = RenderSettings.fogColor;
                BullethitWater.GetComponent<AudioSource>().Play();
                BullethitWater.GetComponent<AudioSource>().pitch = Random.Range(1f, 2f);
                Lifetime = 5f;
                this.transform.LookAt(this.transform.position + Vector3.up);

                for (int ToDrop = (int)Mathf.Lerp(15f, 0f, Mathf.Clamp(Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) / 4f, 0f, 1f)); ToDrop > 0; ToDrop --) {
                    if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < 4f) {
                        GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                        CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "WaterDrop";
                        CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = RenderSettings.fogColor;
                    }
                }
                break;
            case "BullethitWood":
                BullethitWood.SetActive(true);
                BullethitWood.GetComponent<ParticleSystem>().Play();
                BullethitWood.GetComponent<AudioSource>().Play();
                BullethitWood.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1f);
                Lifetime = 5f;
                break;
            case "BullethitMetal":
                BullethitMetal.SetActive(true);
                BullethitMetal.GetComponent<ParticleSystem>().Play();
                BullethitMetal.GetComponent<AudioSource>().Play();
                BullethitMetal.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1f);
                Lifetime = 5f;
                break;
            case "Swing": case "Spit": case "Chainsaw": case "Arrow": case "Suppressor": case "GrenadeLauncher": case "LightningBolt": case "FlameThrower": case "WaterGun": case "FireExtinguisher":
                JustSoundsClip = EffectName;
                if (EffectName == "Surpressor" || EffectName == "WaterGun") {
                    BulletCase.SetActive(true);
                    BulletDelay = Lifetime;
                } else if (EffectName == "Swing"){
                    JustSoundsClip = "Swing" + (int)Random.Range(1f, 3.9f);
                    JustSound.pitch = Random.Range(0.75f, 1f);
                }
                Lifetime = 5f;
                break;
            case "BarrelBreak":
                BarrelBreak.SetActive(true);
                BarrelBreak.GetComponent<AudioSource>().Play();
                foreach (Transform Debris in BarrelBreak.transform) {
                    Debris.transform.localScale = new Vector3(Random.Range(1f, 3f), Random.Range(1f, 3f), Random.Range(1f, 3f));
                    Debris.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5f, 5f), 5f, Random.Range(-5f, 5f)), ForceMode.VelocityChange);
                    Debris.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)), ForceMode.VelocityChange);
                    Debris.GetComponent<MeshRenderer>().material.color = EffectColor;
                }
                Lifetime = 5f;
                break;
            case "DoorBreak":
                DoorBreak.SetActive(true);
                DoorBreak.GetComponent<AudioSource>().Play();
                foreach (Transform Debris in DoorBreak.transform) {
                    Debris.transform.localScale = new Vector3(Random.Range(1f, 3f), Random.Range(1f, 3f), Random.Range(1f, 3f));
                    Debris.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)), ForceMode.VelocityChange);
                    Debris.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)), ForceMode.VelocityChange);
                    Debris.GetComponent<MeshRenderer>().material.color = new Color32(188, 163, 140, 255);
                }
                Lifetime = 5f;
                break;
            case "Colt": case "Luger": case "Revolver": case "HunterRifle": case "DBShotgun": case "Thompson": case "AK-47": case "Shotgun": case "MP5": case "M4": case "Sten": case "Garand": case "GarandR": case "Famas": case "Uzi": case "G3": case "Scar": case "SPAS": case "SAW": case "Minigun": case "MosinNagant": case "Rocket": case "Musket": case "G18": case "M1Carbine":
                GunFireMuzzle.SetActive(true);
                BulletCase.SetActive(true);
                int Muzzle = Random.Range(0, 3);
                if (Muzzle == 0 || GS.LightingQuality < 2) {
                    GunFireMuzzle.GetComponent<ParticleSystem>().Stop();
                } else {
                    GunFireMuzzle.GetComponent<ParticleSystem>().Play();
                    GunFireMuzzle.GetComponent<Light>().enabled = true;
                }
                Lifetime = 5f;
                if (EffectName == "Shotgun" || EffectName == "HunterRifle" || EffectName == "MosinNagant") {
                    BulletDelay = Lifetime - 0.5f;
                } else {
                    BulletDelay = Lifetime;
                }
                break;
            case "BloodSplat":
                BloodSplat.SetActive(true);
                BloodSplat.GetComponent<ParticleSystem>().Play();
                BloodSplat.GetComponent<AudioSource>().Play();
                Lifetime = 2f;
                PickSplat = (int)Random.Range(0f, 2.9f);
                Ray CheckBloodB = new Ray(this.transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)), Vector3.down);
                RaycastHit CheckBloodHITB;
                if (Physics.Raycast(CheckBloodB, out CheckBloodHITB, 3f, 9)) {
                    if (CheckBloodHITB.collider.name != "Water") {
                        BloodSplat.transform.GetChild(0).GetChild(PickSplat).gameObject.SetActive(true);
                        BloodSplat.transform.GetChild(0).position = CheckBloodHITB.point - (Vector3.up * -0.01f);
                        BloodSplat.transform.GetChild(0).LookAt(BloodSplat.transform.GetChild(0).transform.position + Vector3.up);
                        BloodSplat.transform.GetChild(0).GetChild(PickSplat).Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                        BloodSplat.transform.GetChild(0).rotation = Quaternion.FromToRotation(BloodSplat.transform.GetChild(0).transform.up, CheckBloodHITB.normal);
                        BloodSplat.transform.GetChild(0).localScale = Vector3.one * Random.Range(0.5f, 2f);
                        BloodSplat.transform.GetChild(0).GetChild(PickSplat).GetComponent<SpriteRenderer>().color = new Color32((byte)Random.Range(100f, 255f), 0, 0, (byte)Random.Range(55f, 255f));
                        Lifetime = 10f * QualitySettings.GetQualityLevel();
                    }
                }
                if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < 5f) {
                    GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "BloodSplash";
                    CreateCanvasSplash.GetComponent<EnvEffectScript>().Epicentrum = Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position);
                }
                break;
            case "ItemBreak":
                ItemBreak.SetActive(true);
                ItemBreak.GetComponent<ParticleSystem>().Play();
                ItemBreak.GetComponent<AudioSource>().Play();
                Lifetime = 2f;
                break;
            case "Flash":
                Flash.SetActive(true);
                Flash.GetComponent<AudioSource>().Play();
                Lifetime = 2f;
                break;
            case "Gibs":
                GibCorpse.SetActive(true);
                GibCorpse.GetComponent<AudioSource>().Play();
                Lifetime = 5f + (10f * QualitySettings.GetQualityLevel());
                foreach (GameObject Gib in Gibs) {
                    Gib.transform.SetParent(GibCorpse.transform);
                    if (Gib.GetComponent<BoxCollider>() != null) {
                        Gib.GetComponent<BoxCollider>().isTrigger = false;
                    } else {
                        BoxCollider Box = Gib.AddComponent<BoxCollider>();
                        Box.size = Vector3.one / 4f;
                    }
                    Rigidbody RB = Gib.AddComponent(typeof(Rigidbody)) as Rigidbody;
                    Gib.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-4f, 4f), 4f, Random.Range(-4f, 4f)), ForceMode.VelocityChange);
                    Gib.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)), ForceMode.VelocityChange);
                }
                for (int SplatBlood = Random.Range(3, 6); SplatBlood > 0; SplatBlood --) {
                    GameObject BloodSplat = Instantiate(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().EffectPrefab);
                    BloodSplat.transform.position = this.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1f), Random.Range(-2f, 2f));
                    BloodSplat.GetComponent<EffectScript>().EffectName = "BloodSplat";
                }
                break;
        }

        if (JustSoundsClip != "") {
            JustSound.gameObject.SetActive(true);
            foreach (AudioClip GetSound in SoundClips) {
                if (GetSound.name == JustSoundsClip) {
                    JustSound.clip = GetSound;
                    JustSound.Play();
                }
            }
        }

        if (BulletCase.activeInHierarchy == true) {
            WBulletCase = EffectName;
        }

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Lifetime > 0f) {
            Lifetime -= 0.02f;
            if (GunFireMuzzle.activeInHierarchy == true && Lifetime < 4.9f) {
                GunFireMuzzle.GetComponent<Light>().enabled = false;
            } else if (EffectName == "Flash" && Flash.GetComponent<Light>().intensity > 2f) {
                Flash.GetComponent<Light>().intensity -= 0.25f;
                foreach (GameObject Mob in GameObject.FindGameObjectsWithTag("Mob")) {
                    if (Vector3.Distance(Mob.transform.position, this.transform.position) < 15f) {
                        Mob.GetComponent<MobScript>().React("Blinded", 2f, this.transform.position);
                    }
                }
            } else if (EffectName == "Flash" && Flash.GetComponent<Light>().intensity > 0f) {
                Flash.GetComponent<Light>().intensity -= 0.04f;
            } else if (EffectName == "Cowbell" && Lifetime >= 1.9f) {
                foreach (GameObject Mob in GameObject.FindGameObjectsWithTag("Mob")) {
                    if (Vector3.Distance(Mob.transform.position, this.transform.position) < 100f) {
                        Mob.GetComponent<MobScript>().React("Curious", 20f, this.transform.position);
                    }
                }
            } else if (EffectName == "BloodSplat" && Lifetime < 5f) {
                BloodSplat.transform.GetChild(0).GetChild(PickSplat).GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.0025f);
            }

            if (GunFireMuzzle.activeInHierarchy == true && Lifetime < 4.95f) {
                foreach (AudioClip GetAudio in SoundClips) {
                    if (GetAudio.name == EffectName && PlayedGunShot == true) {
                        GunFireMuzzle.GetComponent<AudioSource>().clip = GetAudio;
                        GunFireMuzzle.GetComponent<AudioSource>().Play();
                        PlayedGunShot = false;
                    }
                }
            }
            if (WBulletCase != "" && BulletCase != null && BulletChamber != null && Lifetime <= BulletDelay) {
                if (BulletCaseVelocity == Vector3.zero) {
                    BulletCaseVelocity = this.transform.up * 0.05f + this.transform.right * 0.05f;
                    BCPost = Vector3.Lerp(this.transform.position, BulletChamber.transform.position, 1f);
                }
                GameObject SelectedCase = null;
                foreach (Transform WCase in BulletCase.transform) {
                    if (WCase.name == "Pistol" && (EffectName == "Colt" || EffectName == "Luger" || EffectName == "Thompson" || EffectName == "MP5" || EffectName == "Sten" || EffectName == "Uzi" || EffectName == "G18")) {
                        WCase.gameObject.SetActive(true);
                        SelectedCase = WCase.gameObject;
                    } else if (WCase.name == "Rifle" && (EffectName == "Suppressor" || EffectName == "WaterGun" || EffectName == "HunterRifle" || EffectName == "AK-47" || EffectName == "M4" || EffectName == "Garand" || EffectName == "G3" || EffectName == "Famas" || EffectName == "Scar" || EffectName == "MosinNagant" || EffectName == "SAW" || EffectName == "Minigun" || EffectName == "M1Carbine")) {
                        WCase.gameObject.SetActive(true);
                        SelectedCase = WCase.gameObject;
                    } else if (WCase.name == "Gauge" && (EffectName == "Shotgun" || EffectName == "SPAS")) {
                        WCase.gameObject.SetActive(true);
                        SelectedCase = WCase.gameObject;
                    } else if (WCase.name == "RifleMag" && EffectName == "GarandR") {
                        WCase.gameObject.SetActive(true);
                        SelectedCase = WCase.gameObject;
                    }
                }
                if (SelectedCase != null) {
                    BulletCase.transform.position = BCPost;
                    BCPost += BulletCaseVelocity;
                    SelectedCase.transform.Rotate(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
                    BulletCaseVelocity -= Vector3.up * 0.025f;
                    SelectedCase.transform.localScale = Vector3.one * Mathf.Clamp((BulletDelay - Lifetime) * 25f, 0f, 1f);
                }
            }

            if (Lifetime < 4f) {
                BulletCase.SetActive(false);
            }

        } else {
            Destroy(this.gameObject);
        }

	}

}
