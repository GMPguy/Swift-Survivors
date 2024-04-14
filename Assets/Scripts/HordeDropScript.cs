using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeDropScript : MonoBehaviour {

    // Main Variables
    public float Lifetime = 30f;
    public string WhatToDrop = "";
    public int SpecificValue = 0;
    string SoundWhenFlash = "";
    Color32 SetColor;
    // References
    public ParticleSystem Smoke;
    public ParticleSystem Sparkles;
    public GameObject ItemModels;
    public GameObject EffectPrefab;
    GameScript GS;
    // References
    // Main Variables

	// Use this for initialization
	void Start () {

        if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        if (WhatToDrop == "") {

            int PickWhich = Random.Range(0, 8);
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health[0] <= 10f) {
                PickWhich = 2;
            }

            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health[0] >= 90f && PickWhich == 2) {
                PickWhich = 4;
            }

            if (PickWhich == 0 || PickWhich == 1 || PickWhich == 8) {
                WhatToDrop = "Weapon";
                SpecificValue = GameObject.Find("_RoundScript").GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, GameObject.Find("_RoundScript").GetComponent<RoundScript>().Weapons.Length - 0.1f)];
            } else if (PickWhich == 2) {
                WhatToDrop = "Health";
                SpecificValue = (int)Random.Range(1, 4) * 25;
            } else if (PickWhich == 3) {
                WhatToDrop = "Attachment";
                SpecificValue = GameObject.Find("_RoundScript").GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, GameObject.Find("_RoundScript").GetComponent<RoundScript>().Weapons.Length - 0.1f)];
            } else if (PickWhich == 4) {
                WhatToDrop = "Money";
                SpecificValue = (int)Random.Range(1, 10) * 10;
            } else if (PickWhich == 5) {
                WhatToDrop = "Drunk";
                SpecificValue = (int)Random.Range(1, 10) * 10;
            } else if (PickWhich == 6) {
                WhatToDrop = "Camera";
                SpecificValue = (int)Random.Range(1, 5) * 3;
            } else if (PickWhich == 7) {
                WhatToDrop = "Nuke";
                SpecificValue = (int)Random.Range(1, 5) * 2;
            }

        }

        foreach (Transform SetModel in ItemModels.transform) {
            if (WhatToDrop == "Weapon" && SetModel.name == "Item" + SpecificValue) {
                SetModel.gameObject.SetActive(true);
                SetColor = new Color32(255, 0, 128, 255);
                SoundWhenFlash = "śHordeDropWeapon";
            } else if (WhatToDrop == "Health" && SetModel.name == "Health"){
                SetModel.gameObject.SetActive(true);
                SetColor = new Color32(0, 255, 0, 255);
                SoundWhenFlash = "śDucktape";
            } else if (WhatToDrop == "Attachment" && SetModel.name == "Item" + SpecificValue){
                SetModel.gameObject.SetActive(true);
                SetColor = new Color32(255, 255, 0, 255);
                if(SetModel.GetComponent<MeshRenderer>()){
                    foreach(Material ColMat in SetModel.GetComponent<MeshRenderer>().materials){
                        ColMat.color = SetColor;
                    }
                } else {
                    foreach(Transform ChildModel in SetModel){
                        if(ChildModel.GetComponent<MeshRenderer>()){
                            foreach(Material ColMat in ChildModel.GetComponent<MeshRenderer>().materials){
                                ColMat.color = SetColor;
                                ColMat.shader = Shader.Find("Unlit/Color");
                            }
                        }
                    }
                }
                SoundWhenFlash = "śHordeDropWeapon";
            } else if (WhatToDrop == "Money" && SetModel.name == "Money"){
                SetModel.gameObject.SetActive(true);
                SetColor = new Color32(128, 255, 0, 255);
                SoundWhenFlash = "śCashOut";
            } else if (WhatToDrop == "Drunk" && SetModel.name == "Vodka"){
                SetModel.gameObject.SetActive(true);
                SetColor = new Color32(0, 255, 255, 255);
                SoundWhenFlash = "śDrinking";
            } else if (WhatToDrop == "Camera" && SetModel.name == "Camera"){
                SetModel.gameObject.SetActive(true);
                SetColor = new Color32(100, 100, 255, 255);
                SoundWhenFlash = "śFlash";
            } else if (WhatToDrop == "Nuke" && SetModel.name == "Nuke"){
                SetModel.gameObject.SetActive(true);
                SetColor = new Color32(255, 255, 255, 255);
                SoundWhenFlash = "śNuke";
            } else {
                SetModel.gameObject.SetActive(false);
            }
        }

        this.GetComponent<Light>().color = SetColor;
        ParticleSystem.MainModule SetSmoke = Smoke.main;
        SetSmoke.startColor = new ParticleSystem.MinMaxGradient(new Color32(SetColor.r, SetColor.g, SetColor.b, 3));
        ParticleSystem.MainModule SetSparkles = Sparkles.main;
        SetSparkles.startColor = new ParticleSystem.MinMaxGradient(SetColor);

        Lifetime = Mathf.Lerp(30f, 10f, float.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?")) / 5f);

    }

    void Update() {
        
        ItemModels.transform.Rotate(new Vector3(0f, 1f * (Time.deltaTime * 100f), 0f));
        this.transform.LookAt(new Vector3(GameObject.Find("MainCamera").transform.position.x, this.transform.position.y, GameObject.Find("MainCamera").transform.position.z), Vector3.up);

        if (Lifetime > 0f) {
            Lifetime -= 0.02f * (Time.deltaTime * 50f);
            // flashing
            if (Lifetime <= 5f && Lifetime - (int)Lifetime < 0.5f) {
                this.GetComponent<Light>().enabled = false;
                Smoke.gameObject.SetActive(false);
                ItemModels.gameObject.SetActive(false);
            } else {
                this.GetComponent<Light>().enabled = true;
                Smoke.gameObject.SetActive(true);
                ItemModels.gameObject.SetActive(true);
            }
        } else {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerStay(Collider other) {

        if (other.tag == "Player") {
            if (WhatToDrop == "Weapon") {
                int FreeSpace = -1;
                if (GS.GetSemiClass(other.GetComponent<PlayerScript>().Inventory[other.GetComponent<PlayerScript>().CurrentItemHeld], "id") == "0") {
                    FreeSpace = other.GetComponent<PlayerScript>().CurrentItemHeld;
                }
                for (int CheckSpace = other.GetComponent<PlayerScript>().MaxInventorySlots - 1; CheckSpace >= 0; CheckSpace--) {
                    if (GS.GetSemiClass((other.GetComponent<PlayerScript>().Inventory[CheckSpace]), "id") == "0") {
                        FreeSpace = CheckSpace;
                    }
                }
                if (FreeSpace == -1 || GS.GetSemiClass(other.GetComponent<PlayerScript>().Inventory[other.GetComponent<PlayerScript>().CurrentItemHeld], "id") == "0") {
                    FreeSpace = other.GetComponent<PlayerScript>().CurrentItemHeld;
                }
                other.GetComponent<PlayerScript>().Inventory[FreeSpace] = GS.itemCache[SpecificValue].startVariables;
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(SetColor, new float[]{0.5f, 0.5f});
                GS.Mess(GS.SetString("Picked up weapon", "Podniesiono broń"), "ś" + SoundWhenFlash);
            } else if (WhatToDrop == "Health") {
                other.GetComponent<PlayerScript>().Infection = 0f;
                other.GetComponent<PlayerScript>().Bleeding = 0f;
                other.GetComponent<PlayerScript>().BrokenBone = 0;
                other.GetComponent<PlayerScript>().Health[0] += SpecificValue;
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(SetColor, new float[]{0.5f, 0.5f});
                GS.Mess(GS.SetString("Healed +" + SpecificValue, "Uleczono +" + SpecificValue), "ś" + SoundWhenFlash);
            } else if (WhatToDrop == "Attachment") {
                SpecificValue = int.Parse(GS.GetSemiClass(other.GetComponent<PlayerScript>().Inventory[other.GetComponent<PlayerScript>().CurrentItemHeld], "id"));
                int[] AvailableAttachments = new int[] { 0 };
                if (SpecificValue == 38 || SpecificValue == 42 || SpecificValue == 34 || SpecificValue == 56 || SpecificValue == 57 || SpecificValue == 59 || SpecificValue == 60 || SpecificValue == 65 || SpecificValue == 137) {
                    AvailableAttachments = new int[] { 14, 100, 101, 102, 103, 104, 105 };
                } else if (SpecificValue == 36 || SpecificValue == 55) {
                    AvailableAttachments = new int[] { 14, 100, 101, 102, 104, 105 };
                } else if (SpecificValue == 40 || SpecificValue == 36) {
                    AvailableAttachments = new int[] { 14, 101, 102, 104 };
                } else if (SpecificValue == 29 || SpecificValue == 31 || SpecificValue == 32 || SpecificValue == 113 || SpecificValue == 135) {
                    AvailableAttachments = new int[] { 100, 104 };
                } else if (SpecificValue == 58) {
                    AvailableAttachments = new int[] { 100, 101, 104 };
                } else if (SpecificValue == 41 || SpecificValue == 61){
                    AvailableAttachments = new int[] { 14, 100, 104, 105 };
                } else if (SpecificValue == 35){
                    AvailableAttachments = new int[] { 104 };
                } else {
                    SpecificValue = -1;
                }

                if(SpecificValue != -1){
                    SpecificValue = AvailableAttachments[(int)Random.Range(0f, AvailableAttachments.Length - 0.1f)];
                    other.GetComponent<PlayerScript>().Inventory[other.GetComponent<PlayerScript>().CurrentItemHeld] = GS.SetSemiClass(other.GetComponent<PlayerScript>().Inventory[other.GetComponent<PlayerScript>().CurrentItemHeld], "at", SpecificValue.ToString());//other.GetComponent<PlayerScript>().Inventory[other.GetComponent<PlayerScript>().CurrentItemHeld].z = SpecificValue;
                    GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(SetColor, new float[]{0.5f, 0.5f});
                    GS.Mess(GS.SetString("New attachement: " + GS.itemCache[SpecificValue].getName(), "Nowy dodatek: " + GS.itemCache[SpecificValue].getName()), "ś" + SoundWhenFlash);
                }
            } else if (WhatToDrop == "Money") {
                GS.GetComponent<GameScript>().Money += SpecificValue;
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(SetColor, new float[]{0.5f, 0.5f});
                GS.Mess(GS.SetString("Money collected +" + SpecificValue, "Zebrano pieniądze +" + SpecificValue), "ś" + SoundWhenFlash);
            } else if (WhatToDrop == "Drunk") {
                other.GetComponent<PlayerScript>().Drunkenness += SpecificValue;
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(SetColor, new float[]{0.5f, 0.5f});
                GS.Mess(GS.SetString("Drunkenness +" + SpecificValue, "Upicie +" + SpecificValue), "ś" + SoundWhenFlash);
            } else if (WhatToDrop == "Camera") {
                foreach (GameObject Blinded in GameObject.FindGameObjectsWithTag("Mob")) {
                    Blinded.GetComponent<MobScript>().React("Blinded", SpecificValue, this.transform.position);
                }
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(SetColor, new float[]{0.5f, 0.5f});
                GS.Mess(GS.SetString("Mutants blinded for " + SpecificValue + " seconds!", "Mutanci oślepieni na " + SpecificValue + " sekund!"), "ś" + SoundWhenFlash);
                GameObject Flash = Instantiate(EffectPrefab);
                Flash.GetComponent<EffectScript>().EffectName = "Flash";
                Flash.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            } else if (WhatToDrop == "Nuke") {
                int Nuked = 0;
                for (int Rect = SpecificValue; Rect > 0; Rect --) {
                    GameObject MarkTarget = null;
                    foreach (GameObject GetTarget in GameObject.FindGameObjectsWithTag("Mob")) {
                        if (GetTarget.GetComponent<MobScript>().MobHealth[0] > 0 && GetTarget.GetComponent<MobScript>().ClassOfMob == "Mutant") {
                            MarkTarget = GetTarget;
                        }
                    }
                    if (MarkTarget != null) {
                        MarkTarget.GetComponent<MobScript>().Hurt(9999f, GameObject.FindGameObjectWithTag("Player"), true, Vector3.zero, "Nuked");
                        Nuked += 1;
                    }
                }
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().ShakeCam(1f, 1f);
                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(SetColor, new float[]{4f, 4f});
                GS.Mess(GS.SetString(Nuked + " mutants have been nuked!", Nuked + " mutantów zostało wysadzonych!"), SoundWhenFlash);
            }
            if(SpecificValue != -1) Destroy(this.gameObject);
        }
        
    }

}
