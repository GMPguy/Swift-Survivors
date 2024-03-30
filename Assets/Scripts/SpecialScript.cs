using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SpecialScript : MonoBehaviour {

    // Main Variables
    public string TypeOfSpecial = "";
    public float lifetime = 0f;
    // Main Variables

    // Explosion scripts
    public GameObject CausedBy;
    public GameObject ExplosionEffect;
    public GameObject WaterExplosionEffect;
    public GameObject FlashbangEffect;
    public GameObject MolotowEffect;
    public GameObject Lightning;
    public float ExplosionRange = 1f;
    public float[] Struck;
    Color32 OriginalSkyColor;
    GameScript GS;
    // Explosion scripts

    // Use this for initialization
    void Start() {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        if (TypeOfSpecial == "Explosion") {
            this.transform.position += Vector3.up / 10f;
            bool AboveWater = true;
            Ray CheckForWater = new Ray(this.transform.position, Vector3.up);
            foreach (RaycastHit CheckForWaterHIT in Physics.RaycastAll(CheckForWater, Mathf.Infinity)) {
                if (CheckForWaterHIT.collider.gameObject.layer == 16) {
                    AboveWater = false;
                }
            }
            if (AboveWater == true) {
                ExplosionEffect.SetActive(true);
                ExplosionEffect.transform.localScale = Vector3.one * ExplosionRange;
                lifetime = 3f;
                BoomDetect(this.transform.position);
                this.GetComponent<Light>().enabled = true;
                this.GetComponent<Light>().intensity = 10f;
                this.GetComponent<Light>().range = ExplosionRange * 3f;
                this.GetComponent<Light>().color = new Color32(255, 125, 0, 255);
            } else {
                ExplosionRange *= 2f;
                WaterExplosionEffect.SetActive(true);
                ParticleSystem.MainModule SetCol = WaterExplosionEffect.GetComponent<ParticleSystem>().main;
                SetCol.startColor = RenderSettings.fogColor;
                ParticleSystem.MainModule SetColB = WaterExplosionEffect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                SetColB.startColor = RenderSettings.fogColor;
                WaterExplosionEffect.transform.localScale = Vector3.one * ExplosionRange * 2f;
                lifetime = 3f;
                BoomDetect(this.transform.position);
            }
        } else if (TypeOfSpecial == "Flashbang") {
            this.transform.position += Vector3.up / 10f;
            bool AboveWater = true;
            Ray CheckForWater = new Ray(this.transform.position, Vector3.up);
            foreach (RaycastHit CheckForWaterHIT in Physics.RaycastAll(CheckForWater, Mathf.Infinity)) {
                if (CheckForWaterHIT.collider.gameObject.layer == 16) {
                    AboveWater = false;
                }
            }
            if (AboveWater == true) {
                FlashbangEffect.SetActive(true);
                lifetime = 3f;
                Flashbeng(this.transform.position);
                this.GetComponent<Light>().enabled = true;
                this.GetComponent<Light>().intensity = 50f;
                this.GetComponent<Light>().range = ExplosionRange;
                this.GetComponent<Light>().color = new Color32(255, 255, 255, 255);
            } else {
                ExplosionRange /= 4f;
                WaterExplosionEffect.SetActive(true);
                ParticleSystem.MainModule SetCol = WaterExplosionEffect.GetComponent<ParticleSystem>().main;
                SetCol.startColor = RenderSettings.fogColor;
                ParticleSystem.MainModule SetColB = WaterExplosionEffect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                SetColB.startColor = RenderSettings.fogColor;
                lifetime = 3f;
                BoomDetect(this.transform.position);
            }
        } else if (TypeOfSpecial == "Molotow") {
            this.transform.position += Vector3.up / 10f;
            bool AboveWater = true;
            Ray CheckForWater = new Ray(this.transform.position, Vector3.up);
            foreach (RaycastHit CheckForWaterHIT in Physics.RaycastAll(CheckForWater, Mathf.Infinity)) {
                if (CheckForWaterHIT.collider.gameObject.layer == 16) {
                    AboveWater = false;
                }
            }
            if (AboveWater == true) {
                MolotowEffect.SetActive(true);
                MolotowEffect.transform.localScale = Vector3.one * ExplosionRange;
                lifetime = 3f;
                Molotow(this.transform.position);
                this.GetComponent<Light>().enabled = true;
                this.GetComponent<Light>().intensity = 1f;
                this.GetComponent<Light>().range = ExplosionRange;
                this.GetComponent<Light>().color = new Color32(255, 125, 0, 255);
            } else {
                Destroy(this.gameObject);
            }
        } else if (TypeOfSpecial == "Lightning") {
            Lightning.SetActive(true);
            ExplosionRange = 12f;
            lifetime = 10f;
            Struck = new float[]{ 0f, Random.Range(4f, 8f)};
            OriginalSkyColor = GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor;
            this.GetComponent<Light>().enabled = true;
            this.GetComponent<Light>().intensity = 10f;
            this.GetComponent<Light>().range = 500f;
            this.GetComponent<Light>().color = new Color32(0, 125, 255, 255);
        }
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (lifetime > 0f) {
            lifetime -= 0.02f;
        } else {
            Destroy(this.gameObject);
        }

        if (TypeOfSpecial == "Explosion") {
            if (lifetime < 2.95f) {
                this.GetComponent<Light>().intensity = 0f;
            } else {
                float ShakePerCent = Mathf.Clamp(1f - (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) / (ExplosionRange * 4f)), 0f, 1f);
                if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < ExplosionRange * 4f) {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().ShakeCam(ShakePerCent, 1f);
                    GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[1] = 5f;
                    GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[0] = 5f * Mathf.Clamp(-1f + (ShakePerCent * 2f), 0f, 1f);
                }
                if (ExplosionEffect.activeInHierarchy == true) {
                    for (int Debris = Random.Range(1, 4); Debris > 0; Debris--) {
                        if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < ExplosionRange * 2f) {
                            GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "Dirt";
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().Size = Random.Range(320f, 640f);
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().Epicentrum = 1f - (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) / (ExplosionRange * 2f));
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = Color32.Lerp(new Color32(10, 10, 10, 255), new Color32(25, 25, 25, 255), Random.Range(0f, 1f));
                        }
                    }
                } else if (WaterExplosionEffect.activeInHierarchy == true) {
                    for (int Debris = Random.Range(5, 10); Debris > 0; Debris--) {
                        if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < ExplosionRange * 2f) {
                            GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "WaterDrop";
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = RenderSettings.fogColor;
                        }
                    }
                }
            }

        } else if (TypeOfSpecial == "Flashbang") {
            if (lifetime < 2.9f) {
                this.GetComponent<Light>().intensity = 0f;
            } else {
                if (ExplosionEffect.activeInHierarchy == true) {
                    for (int Debris = Random.Range(1, 4); Debris > 0; Debris--) {
                        if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < ExplosionRange * 2f) {
                            GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "Dirt";
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().Size = Random.Range(320f, 640f);
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().Epicentrum = 1f - (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) / (ExplosionRange * 2f));
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = Color32.Lerp(new Color32(10, 10, 10, 255), new Color32(25, 25, 25, 255), Random.Range(0f, 1f));
                        }
                    }
                } else if (WaterExplosionEffect.activeInHierarchy == true) {
                    for (int Debris = Random.Range(5, 10); Debris > 0; Debris--) {
                        if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < ExplosionRange * 2f) {
                            GameObject CreateCanvasSplash = Instantiate(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().EnvSplash);
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectName = "WaterDrop";
                            CreateCanvasSplash.GetComponent<EnvEffectScript>().EffectColor = RenderSettings.fogColor;
                        }
                    }
                }
            }
        } else if (TypeOfSpecial == "Molotow") {
            if (lifetime > 2f) {
                this.GetComponent<Light>().intensity = (lifetime - 2f) * 3f;
            } else {
                this.GetComponent<Light>().intensity = 0f;
            }
        } else if (TypeOfSpecial == "Lightning") {
            if (Struck[0] <= 0f && Struck[1] > 0f) {
                Struck[0] = Random.Range(0.025f, 0.05f);
                Struck[1] -= 1f;
                this.transform.LookAt(this.transform.position - (Vector3.up * 100f) + new Vector3(Random.Range(-12f, 12f), 0f, Random.Range(-12f, 12f)));
                this.transform.localScale = new Vector3(Random.Range(1f, 3f), 1f, 1f);
                this.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
                Lightning.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Random.Range(0.5f, 1f));
                Ray CheckStruck = new Ray(this.transform.position, this.transform.forward);
                RaycastHit CheckStruckHIT;
                if (Physics.Raycast(CheckStruck, out CheckStruckHIT, 200f, GameObject.Find("_GameScript").GetComponent<GameScript>().IgnoreMaks1)) {
                    BoomDetect(CheckStruckHIT.point);
                    this.transform.localScale = new Vector3(Random.Range(0.1f, 4f), CheckStruckHIT.distance / 100f, 1f);
                    Lightning.transform.GetChild(0).transform.position = CheckStruckHIT.point;
                    if (Vector3.Distance(CheckStruckHIT.point, GameObject.FindGameObjectWithTag("Player").transform.position) < 25f) {
                        float ShakePerCent = 1f - (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) / (ExplosionRange * 4f));
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().ShakeCam(ShakePerCent, ShakePerCent);
                    }
                }
                Lightning.transform.GetChild(1).GetComponent<AudioSource>().pitch = Random.Range(0.25f, 1f);
                this.GetComponent<Light>().intensity = Random.Range(1f, 10f);
                GameObject.Find("_RoundScript").GetComponent<RoundScript>().AmbientSet("ThunderFlash");
                GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = Color.Lerp(OriginalSkyColor, Color.white, this.GetComponent<Light>().intensity / 10f);
            } else if (Struck[0] > 0f){
                Struck[0] -= 0.02f;
            } else if (Struck[1] <= 0f) {
                Lightning.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                GameObject.Find("_RoundScript").GetComponent<RoundScript>().AmbientSet("Normal");
                this.GetComponent<Light>().enabled = false;
            }
        }
		
	}

    void BoomDetect(Vector3 Where) {

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) < ExplosionRange) {
            bool GotHit = false;
            this.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
            Ray CheckForHit = new Ray(this.transform.position, this.transform.forward);
            RaycastHit CheckForHitA;
            if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) < 1.25f){
                GotHit = true;
            } else if (Physics.Raycast(CheckForHit, out CheckForHitA, ExplosionRange, GameObject.Find("_GameScript").GetComponent<GameScript>().IgnoreMaks1)) {
                if (CheckForHitA.collider.transform.root.gameObject == GameObject.FindGameObjectWithTag("Player")) {
                    GotHit = true;
                }
            }
            if (GotHit == true) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Hurt(Mathf.Clamp(((1f - (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) / ExplosionRange)) * 200f), 0f, 100f), "Explosion", true, Where);
            }
        }

        foreach (GameObject FoundMob in GameObject.FindGameObjectsWithTag("Mob")) {
            if (Vector3.Distance(FoundMob.transform.position, Where) < ExplosionRange) {
                bool GotHit = false;
                this.transform.LookAt(FoundMob.transform.position);
                Ray CheckForHit = new Ray(this.transform.position, this.transform.forward);
                RaycastHit CheckForHitA;
                if (Vector3.Distance(FoundMob.transform.position, Where) < 1.25f) {
                    GotHit = true;
                } else if (Physics.Raycast(CheckForHit, out CheckForHitA, ExplosionRange, GameObject.Find("_GameScript").GetComponent<GameScript>().IgnoreMaks1)) {
                    if (CheckForHitA.collider.gameObject == FoundMob) {
                        GotHit = true;
                    }
                }
                if (GotHit == true) {
                    FoundMob.GetComponent<MobScript>().Hurt(Mathf.Clamp(((1f - (Vector3.Distance(FoundMob.transform.position, Where) / ExplosionRange)) * 200f), 0f, 100f), CausedBy, true, this.transform.forward, "Explosion");
                }
            } else if (Vector3.Distance(FoundMob.transform.position, Where) < ExplosionRange * 10f) {
                FoundMob.GetComponent<MobScript>().React("Curious", 10f, Where);
            }
        }

        foreach (GameObject FoundInteractable in GameObject.FindGameObjectsWithTag("Interactable")) {
            if (Vector3.Distance(FoundInteractable.transform.position, Where) < ExplosionRange) {
                FoundInteractable.GetComponent<InteractableScript>().Interaction("Break", 1000f);
            }
        }

        GameObject ItemPref = GameObject.Find("_RoundScript").GetComponent<RoundScript>().ItemPrefab;
        foreach (GameObject FoundItem in GameObject.FindGameObjectsWithTag("Item")) {
            if (Vector3.Distance(FoundItem.transform.position, Where) > 0.25f && Vector3.Distance(FoundItem.transform.position, Where) < ExplosionRange) {
                GameObject ThrewItem = Instantiate(ItemPref) as GameObject;
                ThrewItem.transform.position = Vector3.Lerp(FoundItem.transform.position, this.transform.position, 0.75f);
                ThrewItem.GetComponent<ItemScript>().ThrownDirection = Vector3.ClampMagnitude(Vector3.up + Vector3.right*Random.Range(-2f, 2f) + Vector3.forward*Random.Range(-2f, 2f), 1f);
                ThrewItem.GetComponent<ItemScript>().Variables = FoundItem.GetComponent<ItemScript>().Variables;
                ThrewItem.GetComponent<ItemScript>().State = 2;
                ThrewItem.GetComponent<ItemScript>().DroppedBy = CausedBy;
                string specID = GS.GetSemiClass(ThrewItem.GetComponent<ItemScript>().Variables, "id");
                if((specID == "110" || specID == "66") &&  GS.GetSemiClass(ThrewItem.GetComponent<ItemScript>().Variables, "va") == "0"){
                    ThrewItem.GetComponent<ItemScript>().Variables = GS.SetSemiClass(ThrewItem.GetComponent<ItemScript>().Variables, "va", Random.Range(60f, 99f).ToString());
                    ThrewItem.tag = "Untagged";
                }
                Destroy(FoundItem);
            }
        }

    }

    void Flashbeng(Vector3 Where) {

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) < ExplosionRange) {
            float DistPerCent = 1f - (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) / ExplosionRange);
            bool GotHit = false;
            this.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
            Ray CheckForHit = new Ray(this.transform.position, this.transform.forward);
            RaycastHit CheckForHitA;
            if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) < 1.25f){
                GotHit = true;
            } else if (Physics.Raycast(CheckForHit, out CheckForHitA, ExplosionRange, GameObject.Find("_GameScript").GetComponent<GameScript>().IgnoreMaks1)) {
                if (CheckForHitA.collider.transform.root.gameObject == GameObject.FindGameObjectWithTag("Player")) {
                    GotHit = true;
                }
            }
            if (DistPerCent < 0.75f && GotHit == true) {
                GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[1] = 2.5f;
                GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[0] = ((DistPerCent - 0.25f) / 0.75f) * 5f;
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(255, 255, 255, 255), new float[]{((DistPerCent - 0.25f) / 0.75f) * 5f, 2.5f});
            } else if (GotHit == true) {
                GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[0] = 5f;
                GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[1] = 2.5f;
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(255, 255, 255, 255), new float[]{5f, 2.5f});
            } else {
                GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[1] = 1f;
                GameObject.Find("_GameScript").GetComponent<GameScript>().Earpiercing[0] = ((DistPerCent - 0.25f) / 0.75f) * 1f;
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(255, 255, 255, 55), new float[]{1f, 1f});
            }
        }

        foreach (GameObject FoundMob in GameObject.FindGameObjectsWithTag("Mob")) {
            if (Vector3.Distance(FoundMob.transform.position, Where) < ExplosionRange) {
                float DistPerCent = 1f - (Vector3.Distance(FoundMob.transform.position, Where) / ExplosionRange);
                bool GotHit = false;
                this.transform.LookAt(FoundMob.transform.position);
                Ray CheckForHit = new Ray(this.transform.position, this.transform.forward);
                RaycastHit CheckForHitA;
                if (Vector3.Distance(FoundMob.transform.position, Where) < 1.25f) {
                    GotHit = true;
                } else if (Physics.Raycast(CheckForHit, out CheckForHitA, ExplosionRange, GameObject.Find("_GameScript").GetComponent<GameScript>().IgnoreMaks1)) {
                    if (CheckForHitA.collider.gameObject == FoundMob) {
                        GotHit = true;
                    }
                }
                if (DistPerCent < 0.75f && GotHit == true) {
                    FoundMob.GetComponent<MobScript>().React("Blinded", Mathf.Lerp(1f, 5f, (DistPerCent - 0.25f) / 0.75f), this.transform.position);
                } else if (GotHit == true) {
                    FoundMob.GetComponent<MobScript>().React("Blinded", 10f, this.transform.position);
                } else {
                    FoundMob.GetComponent<MobScript>().React("Blinded", 1f, this.transform.position);
                }
            }
        }

        this.transform.eulerAngles = Vector3.zero;

    }

    void Molotow(Vector3 Where) {

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) < ExplosionRange) {
            float DistPerCent = 1f - (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) / ExplosionRange);
            bool GotHit = false;
            this.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
            Ray CheckForHit = new Ray(this.transform.position, this.transform.forward);
            RaycastHit CheckForHitA;
            if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, Where) < 1.25f){
                GotHit = true;
            } else if (Physics.Raycast(CheckForHit, out CheckForHitA, ExplosionRange, GameObject.Find("_GameScript").GetComponent<GameScript>().IgnoreMaks1)) {
                if (CheckForHitA.collider.transform.root.gameObject == GameObject.FindGameObjectWithTag("Player")) {
                    GotHit = true;
                }
            }
            if (GotHit == true) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Fire = Mathf.Clamp(Mathf.Lerp(0f, 100f, DistPerCent), GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Fire, 100f);
            }
        }

        foreach (GameObject FoundMob in GameObject.FindGameObjectsWithTag("Mob")) {
            if (Vector3.Distance(FoundMob.transform.position, Where) < ExplosionRange) {
                float DistPerCent = 1f - (Vector3.Distance(FoundMob.transform.position, Where) / ExplosionRange);
                bool GotHit = false;
                this.transform.LookAt(FoundMob.transform.position);
                Ray CheckForHit = new Ray(this.transform.position, this.transform.forward);
                RaycastHit CheckForHitA;
                if (Vector3.Distance(FoundMob.transform.position, Where) < 1.25f) {
                    GotHit = true;
                } else if (Physics.Raycast(CheckForHit, out CheckForHitA, ExplosionRange, GameObject.Find("_GameScript").GetComponent<GameScript>().IgnoreMaks1)) {
                    if (CheckForHitA.collider.gameObject == FoundMob) {
                        GotHit = true;
                    }
                }
                if (GotHit == true) {
                    FoundMob.GetComponent<MobScript>().Fire = Mathf.Clamp(Mathf.Lerp(0f, 100f, DistPerCent), FoundMob.GetComponent<MobScript>().Fire, 100f);
                }
            }
        }

        this.transform.eulerAngles = Vector3.zero;

    }

}
