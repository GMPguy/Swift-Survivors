using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableScript : MonoBehaviour {

    // Main variables
    public JClass Variables;
    public string Name;
    public int[] TradeOptions;
    public int[] TradePrices;
    public Texture[] WarningMaterials;
    // Main variables

    public GameObject SelectedModel;
    public GameScript GS;
    public RoundScript RS;
    public GameObject ItemPrefab;
    public GameObject EffectPrefab;
    public GameObject SpecialPrefab;
    public GameObject RadioactivityPrefab;

    // Misc
    public int RandomID;
    int ammoID;
    Vector3[] ammoScale;
    SpecialScript radiation;
    // Misc

    // Use this for initialization
    void Awake() {
         RoundScript.CachedInteractables.Add(this);
    }

    // Use this after world spawn
    bool wasStarted;
    public void TheStart () {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();

        RandomID = Random.Range(int.MinValue, int.MaxValue);

        // Set Gameobjects
        foreach (Transform FindObject in this.transform) {
            if (FindObject.name == Variables.GetInt(JType.ID).ToString()) {
                FindObject.gameObject.SetActive(true);
                SelectedModel = FindObject.gameObject;
            } else {
                Destroy(FindObject.gameObject);
            }
        }

        if (Variables.GetInt(JType.ID) == 1) {
            // Barrels
            if (Random.Range(0, 100) < 10)
                Variables.SetInt(JType.InteractableType, (int)Random.Range(-4, -1));
            else
                Variables.SetInt(JType.InteractableType, (int)Mathf.Clamp(Random.Range(3f, 5.9f) * RS.GetComponent<RoundScript>().DifficultySliderB, 1f, 5f));
            
            Color32 BarrelColor = new Color32(0, 0, 0, 0);
            string label = "";

            switch (Variables.GetInt(JType.InteractableType)) {
                case -4:
                    Variables.SetFloat(JType.VariableA, 20f);
                    Name = GS.SetString("Explosive Barrel", "Wybuchowa Beczka");
                    label = "WarningExplosives";
                    BarrelColor = new Color32(155, 0, 0, 255);
                    break;
                case -3:
                    Variables.SetFloat(JType.VariableA, 20f);
                    Name = GS.SetString("Radioactive Barrel", "Radioaktywna Beczka");
                    label = "WarningRadioactivity";
                    BarrelColor = new Color32(255, 255, 0, 255);
                    break;
                case -2:
                    Variables.SetFloat(JType.VariableA, 20f);
                    Name = GS.SetString("Flammable Barrel", "Łatwopalna Beczka");
                    label = "WarningFlames";
                    BarrelColor = new Color32(25, 0, 25, 255);
                    break;
                case -1:
                    Variables.SetFloat(JType.VariableA, 20f);
                    Name = GS.SetString("Toxic Barrel", "Toksyczna Beczka");
                    label = "WarningToxins";
                    BarrelColor = new Color32(100, 200, 0, 255);
                    break;
                case 2:
                    Variables.SetFloat(JType.VariableA, 100f);
                    Name = GS.SetString("Red Barrel", "Czerwona Beczka");
                    BarrelColor = new Color32(155, 0, 0, 255);
                    break;
                case 3:
                    Variables.SetFloat(JType.VariableA, 150f);
                    Name = GS.SetString("Blue Barrel", "Niebieska Beczka");
                    BarrelColor = new Color32(0, 128, 255, 255);
                    break;
                case 4:
                    Variables.SetFloat(JType.VariableA, 200f);
                    Name = GS.SetString("Green Barrel", "Zielona Beczka");
                    BarrelColor = new Color32(75, 155, 75, 255);
                    break;
                case 5:
                    Variables.SetFloat(JType.VariableA, 300f);
                    Name = GS.SetString("Black Barrel", "Czarna Beczka");
                    BarrelColor = new Color32(0, 0, 15, 255);
                    break;
                default:
                    Variables.SetFloat(JType.VariableA, 20f);
                    Variables.SetInt(JType.InteractableType, 1);
                    Name = GS.SetString("Rusty Barrel", "Zardzewiała Beczka");
                    BarrelColor = new Color32(175, 125, 75, 255);
                    break;
            }

            for (int m = 0; m < SelectedModel.GetComponent<MeshRenderer>().materials.Length; m++) {
                Material BarrelMat = SelectedModel.GetComponent<MeshRenderer>().materials[m];

                if (BarrelMat.name == "Barrel2 (Instance)") {
                    BarrelMat.color = new Color32((byte)(BarrelColor.r * 0.75f), (byte)(BarrelColor.g * 0.75f), (byte)(BarrelColor.b * 0.75f), 255);
                } else if (BarrelMat.name == "BarrelLabel (Instance)" && label != "") {
                    foreach (Texture labelMat in WarningMaterials)
                        if (labelMat.name == label)
                            SelectedModel.GetComponent<MeshRenderer>().materials[m].mainTexture = labelMat;
                } else {
                    BarrelMat.color = BarrelColor;
                }
            }

        } else if (Variables.GetInt(JType.ID) == 2f) {
            // EscapeTunnel
            Variables.SetFloat(JType.VariableA, 20f);
            Name = GS.SetString("Escape Tunnel", "Tunel Ewakuacyjny");
        } else if (Variables.GetInt(JType.ID) == 3f) {
            // Door
            Name = GS.SetString("Door", "Drzwi");
            float LockChance = RS.IsCausual ? 2 : Random.Range(0f, 1.5f);
            Variables.SetFloat(JType.VariableA, 200f);
            if (LockChance < RS.DifficultySliderB) {
                Variables.SetInt(JType.InteractableType, 2);
            } else {
                Variables.SetInt(JType.InteractableType, 0);
            }
        } else if (Variables.GetInt(JType.ID) == 4f) {
            // VendingMachine

            if (GS.GameModePrefab.x == 1) {

                Variables.SetInt(JType.InteractableType, (int)GS.FixedPerlinNoise(this.transform.position.x, this.transform.position.z));
                Color32[] MachineColors = new Color32[] { Color.white, Color.white };
                TradeOptions = new int[] { 0, 0, 0, 0, 0, 0 };
                TradePrices = new int[] { 0, 0, 0, 0, 0, 0 };
                Name = GS.SetString("Vending Machine", "Automat");

                if (Variables.GetInt(JType.InteractableType) >= 0f && Variables.GetInt(JType.InteractableType) < 0.25f) {
                    MachineColors = new Color32[] { new Color32(200, 0, 0, 255), new Color32(100, 0, 0, 255) };
                } else if (Variables.GetInt(JType.InteractableType) >= 0.25f && Variables.GetInt(JType.InteractableType) < 0.5f) {
                    MachineColors = new Color32[] { new Color32(0, 125, 255, 255), new Color32(0, 0, 255, 255) };
                } else if (Variables.GetInt(JType.InteractableType) >= 0.5f && Variables.GetInt(JType.InteractableType) < 0.75f) {
                    MachineColors = new Color32[] { new Color32(75, 155, 75, 255), new Color32(55, 100, 55, 255) };
                } else if (Variables.GetInt(JType.InteractableType) >= 0.75f && Variables.GetInt(JType.InteractableType) < 1f) {
                    MachineColors = new Color32[] { new Color32(75, 155, 75, 255), new Color32(55, 100, 55, 255) };
                }

                foreach (Material BarrelMat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                    if (BarrelMat.name == "COLOR1 (Instance)") {
                        BarrelMat.color = MachineColors[0];
                    } else if (BarrelMat.name == "COLOR2 (Instance)") {
                        BarrelMat.color = MachineColors[1];
                    }
                }

            } else {

                Variables.SetInt(JType.InteractableType, (int)Random.Range(0f, 3.9f));
                Color32[] MachineColors = new Color32[] { Color.white, Color.white };
                TradeOptions = new int[] { 0, 0, 0, 0, 0, 0 };
                if (Variables.GetInt(JType.InteractableType) == 0) {
                    Name = GS.SetString("Vending Machine", "Automat z Jedzeniem");
                    MachineColors = new Color32[] { new Color32(75, 155, 75, 255), new Color32(55, 100, 55, 255) };
                    for (int AddTradeOptions = 5; AddTradeOptions >= 0; AddTradeOptions--) {
                        TradeOptions[AddTradeOptions] = RS.FoodItems[(int)Random.Range(0f, RS.FoodItems.Length - 0.1f)];
                    }
                } else if (Variables.GetInt(JType.InteractableType) == 1) {
                    Name = GS.SetString("Vending Machine", "Automat ze Sprzętem");
                    MachineColors = new Color32[] { new Color32(0, 125, 255, 255), new Color32(0, 0, 255, 255) };
                    for (int AddTradeOptions = 5; AddTradeOptions >= 0; AddTradeOptions--) {
                        TradeOptions[AddTradeOptions] = RS.Utilities[(int)Random.Range(0f, RS.Utilities.Length - 0.1f)];
                    }
                } else if (Variables.GetInt(JType.InteractableType) == 2) {
                    Name = GS.SetString("Vending Machine", "Automat z Uzbrojeniem");
                    MachineColors = new Color32[] { new Color32(200, 0, 0, 255), new Color32(100, 0, 0, 255) };
                    for (int AddTradeOptions = 5; AddTradeOptions >= 0; AddTradeOptions--) {
                        int Mag = Random.Range(0, 6);
                        if (Mag == 0) {
                            TradeOptions[AddTradeOptions] = RS.AmmoItems[(int)Random.Range(0f, RS.AmmoItems.Length - 0.1f)];
                        } else if (Mag == 1) {
                            TradeOptions[AddTradeOptions] = RS.AttachmentItems[(int)Random.Range(0f, RS.AttachmentItems.Length - 0.1f)];
                        } else {
                            TradeOptions[AddTradeOptions] = RS.Weapons[(int)Random.Range(0f, RS.Weapons.Length - 0.1f)];
                        }
                    }
                } else if (Variables.GetInt(JType.InteractableType) == 3) {
                    Name = GS.SetString("Vending Machine", "Automat z Jedzeniem");
                    MachineColors = new Color32[] { new Color32(200, 0, 125, 255), new Color32(255, 255, 255, 255) };
                    for (int AddTradeOptions = 5; AddTradeOptions >= 0; AddTradeOptions--)
                    {
                        TradeOptions[AddTradeOptions] = RS.HealingItems[(int)Random.Range(0f, RS.HealingItems.Length - 0.1f)];
                    }
                }
                foreach (Material BarrelMat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                    if (BarrelMat.name == "COLOR1 (Instance)") {
                        BarrelMat.color = MachineColors[0];
                    } else if (BarrelMat.name == "COLOR2 (Instance)") {
                        BarrelMat.color = MachineColors[1];
                    }
                }

            }

        } else if (Variables.GetInt(JType.ID) == 5f) {
            // EmergencyItem
            Name = GS.SetString("Emergency Item Box", "Skrzynka z Przedmiotem");
        } else if (Variables.GetInt(JType.ID) == 6f) {
            // EmergencyItem
            Name = GS.SetString("SKIPWAIT", "SKIPWAIT");
        } else if (Variables.GetInt(JType.ID) == 7f) {
            // AmmoBox
            Name = GS.SetString("Ammo box", "Paczka z amunicją");

            ammoScale = new Vector3[15];
            for (int i = 0; i < 15; i++)
                ammoScale[i] = SelectedModel.transform.GetChild(0).GetChild(i).localScale;
        }

        wasStarted = true;
		
	}

    // Use this inside roundscript update
    public void TheUpdate() {

        if (!wasStarted)
            TheStart();


        if (Variables.GetInt(JType.ID) == 1) {

            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(Vector3.zero), 0.1f * (Time.deltaTime * 100f));

        } else if (Variables.GetInt(JType.ID) == 2) {
            if (RS.GetComponent<RoundScript>().RoundTime < 30f || RS.GetComponent<RoundScript>().RoundState == "Nuked") {
                foreach (Material mat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                    if (mat.name == "EscapeTunnel6 (Instance)") {
                        mat.shader = Shader.Find("Unlit/Color");
                    }
                }
                SelectedModel.transform.GetChild(0).gameObject.SetActive(true);
                SelectedModel.transform.GetChild(0).Rotate(new Vector3(1f, 0f, 0f));
            } else {
                foreach (Material mat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                    if (mat.name == "EscapeTunnel6 (Instance)") {
                        mat.shader = Shader.Find("Diffuse");
                    }
                }
                SelectedModel.transform.GetChild(0).gameObject.SetActive(false);
            }
        } else if (Variables.GetInt(JType.ID) == 3) {
            if (Variables.GetInt(JType.InteractableType) == 0f && SelectedModel.transform.localEulerAngles.y < 90f) {
                // closed
                SelectedModel.transform.localRotation = Quaternion.RotateTowards(SelectedModel.transform.localRotation, Quaternion.Euler(new Vector3(-90f, 0f, 90f)), 3f * (Time.deltaTime * 100f));
            } else if (Variables.GetInt(JType.InteractableType) == 1f && SelectedModel.transform.localEulerAngles.y > 0f) {
                // opened
                SelectedModel.transform.localRotation = Quaternion.RotateTowards(SelectedModel.transform.localRotation, Quaternion.Euler(new Vector3(-90f, 0f, 0f)), 3f * (Time.deltaTime * 100f));
            } else if (Variables.GetInt(JType.InteractableType) == 2f && SelectedModel.transform.localEulerAngles.y < 90f) {
                // locked
                SelectedModel.transform.localRotation = Quaternion.RotateTowards(SelectedModel.transform.localRotation, Quaternion.Euler(new Vector3(-90f, 0f, 90f)), 3f * (Time.deltaTime * 100f));
            } else if (SelectedModel.GetComponent<BoxCollider>().enabled == false) {
                SelectedModel.GetComponent<BoxCollider>().enabled = true;
            }
        } else if (Variables.GetInt(JType.ID) == 4) {
            if (SelectedModel.layer == 11) {
                SelectedModel.transform.GetChild(0).GetComponent<Light>().intensity = Mathf.Clamp(Random.Range(0f, 25f), 0f, 1f);
            } else {
                SelectedModel.transform.GetChild(0).GetComponent<Light>().enabled = false;
            }
            bool HasSomething = false;
            foreach (int CheckOffer in TradeOptions) {
                if (CheckOffer > -1) {
                    HasSomething = true;
                }
            }
            if (HasSomething == false && SelectedModel.layer != 0) {
                SelectedModel.layer = 0;
                if (GameObject.Find("MainCanvas").GetComponent<CanvasScript>().DialogedMob == this.gameObject && GameObject.Find("MainCanvas").GetComponent<CanvasScript>().DialogSetting == "VendingMachine") {
                    GameObject.Find("MainCanvas").GetComponent<CanvasScript>().DialogSetting = "VendingMachineDone";
                }
            }
        } else if (Variables.GetInt(JType.ID) == 6f) {

            if (RS.GetComponent<RoundScript>().RoundState == "BeforeWave") {
                if (RS.GetComponent<RoundScript>().RoundTime > 5f) {
                    Variables.SetFloat(JType.VariableA, 0f);
                } else if (Variables.GetFloat(JType.VariableA) == 0f) {
                    Interaction("RingTheBell", 0f);
                }
            }

            if (Variables.GetFloat(JType.VariableA) == 0) {
                Name = GS.GetComponent<GameScript>().SetString("Horde Bell", "Dzwon Hordy");
                SelectedModel.GetComponent<Interactions>().Options = new string[] {"RingBell"};
            } else if (Variables.GetFloat(JType.VariableA) == 1) {
                Name = GS.GetComponent<GameScript>().SetString("Horde Bell", "Dzwon Hordy");
                SelectedModel.GetComponent<Interactions>().Options = new string[] {""};
            }

        } else if (Variables.GetInt(JType.ID) == 7) {

            if (ammoID != (int)Variables.GetFloat(JType.VariableA)) {
                ammoID = (int)Variables.GetFloat(JType.VariableA);

                for (int getProp = 0; getProp < 15; getProp++) {
                    Transform getObj = SelectedModel.transform.GetChild(0).GetChild(getProp);

                    if (getProp < ammoID) {
                        getObj.localScale = ammoScale[getProp];
                        if (getProp == ammoID - 1)
                            SelectedModel.GetComponent<Interactions>().Offset = getObj.transform.localPosition;
                    } else
                        getObj.localScale = Vector3.zero;
                }
            }
        }

    }

    public void Interaction(string WhatToDo, float VariableBonus) {

        if (WhatToDo == "Break") {

            if (Variables.GetInt(JType.ID) == 1f) {
                if (Variables.GetFloat(JType.VariableA) <= 0f)
                    return;

                Variables.SetFloat(JType.VariableA, -VariableBonus, Maths.Add);
                SelectedModel.transform.GetChild(0).GetComponent<AudioSource>().Play();
                this.transform.Rotate(new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), 0f));

                // Funny barrels
                if (Variables.GetInt(JType.InteractableType) < 0) {
                    for (int a = 0; a < Mathf.Max(Mathf.RoundToInt(VariableBonus), 1); a++)
                        if (Random.Range(1, 20) == 1) {

                            SpecialScript danger = GameObject.Instantiate(Variables.GetInt(JType.InteractableType) == -3 ? RadioactivityPrefab : SpecialPrefab).GetComponent<SpecialScript>();

                            danger.transform.position = this.transform.position;
                            switch (Variables.GetInt(JType.InteractableType)) {
                                case -4:
                                    Variables.SetFloat(JType.VariableA, 0f);
                                    danger.TypeOfSpecial = "Explosion";
                                    danger.ExplosionRange = 4f;
                                    break;
                                case -3:
                                    float power = Random.Range(2f, 8f);
                                    Vector3 radius = new Vector3(
                                        Random.Range(2f, 5f),
                                        1f,
                                        Random.Range(2f, 5f)
                                    );

                                    if (radiation) {
                                        power = Mathf.Min(radiation.ExplosionRange + Random.Range(1f, 2f), 10f);
                                        radius = radiation.transform.localScale;
                                        radius.x += Random.Range(1f, 2f);
                                        radius.z += Random.Range(1f, 2f);
                                        Destroy(radiation.gameObject);
                                    }

                                    danger.TypeOfSpecial = "Radioactivity";
                                    danger.ExplosionRange = power;
                                    radiation = danger;
                                    danger.transform.localScale = radius;
                                    break;
                                case -2:
                                    Variables.SetFloat(JType.VariableA, 0f);
                                    danger.TypeOfSpecial = "Molotow";
                                    danger.ExplosionRange = 5f;
                                    break;
                                case -1:
                                    Variables.SetFloat(JType.VariableA, 0f);
                                    break;
                            }
                            break;
                        }
                }

                if (Variables.GetFloat(JType.VariableA) <= 0f) {
                    GameObject Debris = Instantiate(EffectPrefab) as GameObject;
                    Debris.transform.position = this.transform.position + Vector3.up * 1f;
                    Debris.GetComponent<EffectScript>().EffectName = "BarrelBreak";
                    Debris.GetComponent<EffectScript>().EffectColor = SelectedModel.GetComponent<MeshRenderer>().material.color;
                    List<JClass> ItemsToSpawn = new List<JClass>();
                    int AmountToSpawn;

                    Random.InitState(RandomID);

                    if (Variables.GetInt(JType.InteractableType) == 1) {
                        int[] crapItems = new int[] {2, 3, 14, 11, 19, 17, 18};
                        AmountToSpawn = Random.Range(1, 3);
                        for (int a = 0; a < AmountToSpawn; a++)
                            ItemsToSpawn.Add(
                                new (GS.ItemCache[crapItems[Random.Range(0, crapItems.Length)]].startVariables)
                            );
                    } else if (Variables.GetInt(JType.InteractableType) == 2) {
                        //ItemsToSpawn = new Vector3[] { new Vector3(2f, 100f, 0f), new Vector3(3f, 0f, 0f), new Vector3(14f, 100f, 0f), new Vector3(17f, 0f, 0f), new Vector3(18f, 0f, 0f), new Vector3(6f, 0f, 0f), new Vector3(22f, 0f, 0f), new Vector3(23f, 0f, 0f), new Vector3(15f, 100f, 0f) };
                        AmountToSpawn = Random.Range(2, 5);
                        for (int a = 0; a < AmountToSpawn; a++)
                            ItemsToSpawn.Add(
                                new (GS.ItemCache[(int)Random.Range(1f, 20f)].startVariables)
                            );
                    } else if (Variables.GetInt(JType.InteractableType) == 3) {
                        //ItemsToSpawn = new Vector3[] { new Vector3(15f, 100f, 0f), new Vector3(4f, 0f, 0f), new Vector3(27f, 100f, 0f), new Vector3(22f, 0f, 0f), new Vector3(23f, 0f, 0f), new Vector3(29f, 0f, 0f), new Vector3(18f, 0f, 0f), new Vector3(16f, 100f, 0f) };
                        AmountToSpawn = Random.Range(2, 5);
                        for (int a = 0; a < AmountToSpawn; a++)
                            ItemsToSpawn.Add(
                                new (GS.ItemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables)
                            );
                    } else if (Variables.GetInt(JType.InteractableType) == 4) {
                        AmountToSpawn = Random.Range(1, 2);
                        for (int a = 0; a < AmountToSpawn; a++)
                            ItemsToSpawn.Add( new (Random.Range(0, 3) switch {
                                0 => GS.ItemCache[RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - .1f)]].startVariables,
                                1 => GS.ItemCache[RS.GetComponent<RoundScript>().AmmoItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AmmoItems.Length - .1f)]].startVariables,
                                _ => GS.ItemCache[RS.GetComponent<RoundScript>().AttachmentItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AttachmentItems.Length - .1f)]].startVariables
                            }));
                    } else if (Variables.GetInt(JType.InteractableType) == 5) {
                        AmountToSpawn = Random.Range(5, 10);
                        for (int a = 0; a < AmountToSpawn; a++)
                            ItemsToSpawn.Add( new (Random.Range(0, 3) switch {
                                0 => GS.ItemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables,
                                1 => GS.ItemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables,
                                _ => GS.ItemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables
                            }));
                    } else {
                        AmountToSpawn = 5;
                        for (int a = 0; a < AmountToSpawn; a++)
                            ItemsToSpawn.Add(
                                new (GS.ItemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables)
                            );
                    }
                
                    for (int SpawnStuff = 0; SpawnStuff < ItemsToSpawn.Count; SpawnStuff++) {
                        GameObject CreateItem = Instantiate(ItemPrefab) as GameObject;
                        CreateItem.transform.position = this.transform.position + (Vector3.up * 1f) + (Vector3.up * (SpawnStuff / 2f));

                        JClass variables = new (ItemsToSpawn[SpawnStuff]);
                        CreateItem.GetComponent<ItemScript>().Variables.CopyFrom(variables);
                    }
                    
                    Destroy(this.gameObject);
                }

            } else if (Variables.GetInt(JType.ID) == 3) {

                Variables.SetFloat(JType.VariableA, -VariableBonus, Maths.Add);
                SelectedModel.transform.GetChild(0).GetComponent<AudioSource>().Play();
                SelectedModel.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, Random.Range(60f, 30f)));
                if (Variables.GetFloat(JType.VariableA) <= 0f) {
                    GameObject Debris = Instantiate(EffectPrefab) as GameObject;
                    Debris.transform.position = this.transform.position + (Vector3.up * 1f) + (this.transform.right * 0.5f);
                    Debris.GetComponent<EffectScript>().EffectName = "DoorBreak";
                    Debris.GetComponent<EffectScript>().EffectColor = SelectedModel.GetComponent<MeshRenderer>().material.color;
                    Destroy(this.gameObject);
                }

            }

        } else if (WhatToDo == "Door") {

            SelectedModel.GetComponent<BoxCollider>().enabled = false;
            if (Variables.GetInt(JType.InteractableType) == 0) {
                Variables.SetInt(JType.InteractableType, 1);
                SelectedModel.transform.GetChild(1).GetComponent<AudioSource>().Play();
            } else if (Variables.GetInt(JType.InteractableType) == 1) {
                Variables.SetInt(JType.InteractableType, 0);
                SelectedModel.transform.GetChild(2).GetComponent<AudioSource>().Play();
            } else if (Variables.GetInt(JType.InteractableType) == 2) {
                SelectedModel.transform.GetChild(3).GetComponent<AudioSource>().Play();
                SelectedModel.GetComponent<Interactions>().CanBePicklocked = true;
            }

        } else if (WhatToDo == "SetItem") {

            Variables.SetFloat(JType.VariableA, VariableBonus);
            GameObject PickedObject = null;
            foreach (Transform GetModel in SelectedModel.transform.GetChild(1)) {
                if (GetModel.GetSiblingIndex() == VariableBonus) {
                    GetModel.gameObject.SetActive(true);
                    PickedObject = GetModel.gameObject;
                } else {
                    GetModel.gameObject.SetActive(false);
                }
            }

            foreach (Material SetMat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                if (SetMat.name == "GLASS (Instance)" && VariableBonus >= 0f) {
                    SetMat.color = new Color32(125, 155, 255, 128);
                } else if (SetMat.name == "GLASS (Instance)" && VariableBonus < 0f) {
                    SetMat.color = new Color32(125, 155, 255, 0);
                }
            }

            if (VariableBonus > -1f) {
                SelectedModel.transform.GetChild(0).gameObject.SetActive(true);
                SelectedModel.GetComponent<Interactions>().Options = new string[] {"EmergencyItem"};
            } else {
                SelectedModel.transform.GetChild(0).gameObject.SetActive(false);
                SelectedModel.GetComponent<Interactions>().Options = new string[] {""};
            }

        } else if (WhatToDo == "GetItem") {

            GameObject PickedObject = null;
            foreach (Transform GetModel in SelectedModel.transform.GetChild(1)) {
                if (GetModel.GetSiblingIndex() == Variables.GetFloat(JType.VariableA)) {
                    PickedObject = GetModel.gameObject;
                    SelectedModel.transform.GetChild(2).GetComponent<AudioSource>().Play();
                }
            }

            if (Variables.GetFloat(JType.VariableA) >= 0f) {
                SelectedModel.transform.GetChild(0).gameObject.SetActive(true);
                if (PickedObject.name == "Ammunition") {
                    GS.GetComponent<GameScript>().Money += Random.Range(1, 100) * 10;
                } else {
                    GameObject DropItem = Instantiate(ItemPrefab);
                    DropItem.transform.position = this.transform.position + (this.transform.forward * 0.25f);
                    DropItem.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[int.Parse(PickedObject.name.Substring(4))].startVariables);
                }
            } else {
                SelectedModel.transform.GetChild(0).gameObject.SetActive(false);
            }

            this.GetComponent<InteractableScript>().Interaction("SetItem", -1f);

        } else if (WhatToDo == "RingTheBell") {
            Variables.SetFloat(JType.VariableA, 1f);
            RS.GetComponent<RoundScript>().RoundTime = 5f;
            SelectedModel.transform.GetChild(0).GetComponent<AudioSource>().Play();
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(1).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
            GS.GetComponent<GameScript>().SetText(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(0).GetComponent<Text>(), "Get ready!", "Przygotuj się!");
            GS.GetComponent<GameScript>().SetText(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(1).GetComponent<Text>(), "Wave " + GS.GetComponent<GameScript>().Round + " incoming", "Nadchodzi fala " + GS.GetComponent<GameScript>().Round);
        } else if (WhatToDo == "GatherAmmo") {
            Variables.SetFloat(JType.VariableA, -1, Maths.Add);
            int ammo = Random.Range(1, 10) * 5;
            GS.Ammo += ammo;
            GS.Mess(GS.SetString("Ammo +", "Amunicja +") + ammo, "HordeDropWeapon");
            if (Variables.GetFloat(JType.VariableA) <= 0f)
                this.transform.position = Vector3.one * -999f;
        } else if (WhatToDo == "SetUpAmmo") {
            Variables.SetFloat(JType.VariableA, Random.Range(5, 15));
            GS.Mess(GS.SetString("An ammo crate has been deployed somewhere on the map!", "Gdzieś na mapie dostarczono skrzynię z amunicją!"), "Draw");
        }

    }

}
