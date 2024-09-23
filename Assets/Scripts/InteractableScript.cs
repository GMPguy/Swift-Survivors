using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableScript : MonoBehaviour {

    // Main variables
    public Vector3 Variables;
    public string Name;
    public int[] TradeOptions;
    public int[] TradePrices;
    // Main variables

    public GameObject SelectedModel;
    public GameScript GS;
    public RoundScript RS;
    public GameObject ItemPrefab;
    public GameObject EffectPrefab;

    // Misc
    public bool Discovered = false;
    bool TooFar = false;
    int ammoID;
    Vector3[] ammoScale;
    // Misc

	// Use this for initialization
	void Start () {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();

        // Set Gameobjects
        foreach (Transform FindObject in this.transform) {
            if (FindObject.name == Variables.x.ToString()) {
                FindObject.gameObject.SetActive(true);
                SelectedModel = FindObject.gameObject;
            } else {
                Destroy(FindObject.gameObject);
            }
        }

        if (Variables.x == 1f) {
            // Barrels
            int PickBarrel = (int)Mathf.Clamp(Random.Range(3f, 5.9f) * RS.GetComponent<RoundScript>().DifficultySliderB, 1f, 5f);
            if (PickBarrel == 1f) {
                Variables.z = 1f;
            } else if (PickBarrel == 2f) {
                Variables.z = 2f;
            } else if (PickBarrel == 3f) {
                Variables.z = 3f;
            } else if (PickBarrel == 4f) {
                Variables.z = 4f;
            } else if (PickBarrel == 5f) {
                Variables.z = 5f;
            }
            Color32 BarrelColor = new Color32(0, 0, 0, 0);
            if ((int)Variables.z == 1) {
                Variables.y = 20f;
                Variables.z = 1f;
                Name = GS.SetString("Rusty Barrel", "Zardzewiała Beczka");
                BarrelColor = new Color32(175, 125, 75, 255);
            } else if ((int)Variables.z == 2f) {
                Variables.y = 100f;
                Variables.z = 2f;
                Name = GS.SetString("Red Barrel", "Czerwona Beczka");
                BarrelColor = new Color32(155, 0, 0, 255);
            } else if ((int)Variables.z == 3f) {
                Variables.y = 150f;
                Variables.z = 3f;
                Name = GS.SetString("Blue Barrel", "Niebieska Beczka");
                BarrelColor = new Color32(0, 128, 255, 255);
            } else if ((int)Variables.z == 4f) {
                Variables.y = 200f;
                Variables.z = 4f;
                Name = GS.SetString("Green Barrel", "Zielona Beczka");
                BarrelColor = new Color32(75, 155, 75, 255);
            } else if ((int)Variables.z == 5f) {
                Variables.y = 300f;
                Variables.z = 5f;
                Name = GS.SetString("Black Barrel", "Czarna Beczka");
                BarrelColor = new Color32(0, 0, 15, 255);
            }

            foreach (Material BarrelMat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                if (BarrelMat.name == "Barrel2 (Instance)") {
                    BarrelMat.color = new Color32((byte)(BarrelColor.r * 0.75f), (byte)(BarrelColor.g * 0.75f), (byte)(BarrelColor.b * 0.75f), 255);
                } else {
                    BarrelMat.color = BarrelColor;
                }
            }
        } else if (Variables.x == 2f) {
            // EscapeTunnel
            Variables.y = 20f;
            Name = GS.SetString("Escape Tunnel", "Tunel Ewakuacyjny");
        } else if (Variables.x == 3f) {
            // Door
            Name = GS.SetString("Door", "Drzwi");
            float LockChance = RS.IsCausual ? 2 : Random.Range(0f, 1.5f);
            Variables.y = 200f;
            if (LockChance < RS.DifficultySliderB) {
                Variables.z = 2f;
            } else {
                Variables.z = 0f;
            }
        } else if (Variables.x == 4f) {
            // VendingMachine

            if (GS.GameModePrefab.x == 1) {

                Variables.z = GS.FixedPerlinNoise(this.transform.position.x, this.transform.position.z);
                Color32[] MachineColors = new Color32[] { Color.white, Color.white };
                TradeOptions = new int[] { 0, 0, 0, 0, 0, 0 };
                TradePrices = new int[] { 0, 0, 0, 0, 0, 0 };
                Name = GS.SetString("Vending Machine", "Automat");

                if (Variables.z >= 0f && Variables.z < 0.25f) {
                    MachineColors = new Color32[] { new Color32(200, 0, 0, 255), new Color32(100, 0, 0, 255) };
                } else if (Variables.z >= 0.25f && Variables.z < 0.5f) {
                    MachineColors = new Color32[] { new Color32(0, 125, 255, 255), new Color32(0, 0, 255, 255) };
                } else if (Variables.z >= 0.5f && Variables.z < 0.75f) {
                    MachineColors = new Color32[] { new Color32(75, 155, 75, 255), new Color32(55, 100, 55, 255) };
                } else if (Variables.z >= 0.75f && Variables.z < 1f) {
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

                Variables.z = (int)Random.Range(0f, 3.9f);
                Color32[] MachineColors = new Color32[] { Color.white, Color.white };
                TradeOptions = new int[] { 0, 0, 0, 0, 0, 0 };
                if (Variables.z == 0f) {
                    Name = GS.SetString("Vending Machine", "Automat z Jedzeniem");
                    MachineColors = new Color32[] { new Color32(75, 155, 75, 255), new Color32(55, 100, 55, 255) };
                    for (int AddTradeOptions = 5; AddTradeOptions >= 0; AddTradeOptions--) {
                        TradeOptions[AddTradeOptions] = RS.FoodItems[(int)Random.Range(0f, RS.FoodItems.Length - 0.1f)];
                    }
                } else if (Variables.z == 1f) {
                    Name = GS.SetString("Vending Machine", "Automat ze Sprzętem");
                    MachineColors = new Color32[] { new Color32(0, 125, 255, 255), new Color32(0, 0, 255, 255) };
                    for (int AddTradeOptions = 5; AddTradeOptions >= 0; AddTradeOptions--) {
                        TradeOptions[AddTradeOptions] = RS.Utilities[(int)Random.Range(0f, RS.Utilities.Length - 0.1f)];
                    }
                } else if (Variables.z == 2f) {
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
                } else if (Variables.z == 3f) {
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

        } else if (Variables.x == 5f) {
            // EmergencyItem
            Name = GS.SetString("Emergency Item Box", "Skrzynka z Przedmiotem");
        } else if (Variables.x == 6f) {
            // EmergencyItem
            Name = GS.SetString("SKIPWAIT", "SKIPWAIT");
        } else if (Variables.x == 7f) {
            // AmmoBox
            Name = GS.SetString("Ammo box", "Paczka z amunicją");

            ammoScale = new Vector3[15];
            for (int i = 0; i < 15; i++)
                ammoScale[i] = SelectedModel.transform.GetChild(0).GetChild(i).localScale;
        }
		
	}

    void Update() {

        if (Vector3.Distance(this.transform.position, GameObject.Find("MainCamera").transform.position) < RS.DetectionRange) {
            TooFar = false;
            if (SelectedModel.activeInHierarchy == false) {
                SelectedModel.SetActive(true);
            }
        } else {
            TooFar = true;
            if (SelectedModel.activeInHierarchy == true) {
                SelectedModel.SetActive(false);
            }
        }

        if (Variables.x == 1f && TooFar == false) {

            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(Vector3.zero), 0.1f * (Time.deltaTime * 100f));

        } else if (Variables.x == 2f) {
            if ((RS.GetComponent<RoundScript>().RoundTime < 30f || RS.GetComponent<RoundScript>().RoundState == "Nuked") && TooFar == false) {
                foreach (Material mat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                    if (mat.name == "EscapeTunnel6 (Instance)") {
                        mat.shader = Shader.Find("Unlit/Color");
                    }
                }
                SelectedModel.transform.GetChild(0).gameObject.SetActive(true);
                SelectedModel.transform.GetChild(0).Rotate(new Vector3(1f, 0f, 0f));
            } else if (TooFar == false) {
                foreach (Material mat in SelectedModel.GetComponent<MeshRenderer>().materials) {
                    if (mat.name == "EscapeTunnel6 (Instance)") {
                        mat.shader = Shader.Find("Diffuse");
                    }
                }
                SelectedModel.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.transform.position) < GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane * 0.75f && Discovered == false) {
                Discovered = true;
                GS.Mess(GS.SetString("Exit tunnel found!", "Znaleziono tunel ewakuacyjny"), "Draw");
                GS.AddToScore(50);
            }
        } else if (Variables.x == 3f) {
            if (TooFar == false) {
                if (Variables.z == 0f && SelectedModel.transform.localEulerAngles.y < 90f) {
                    // closed
                    SelectedModel.transform.localRotation = Quaternion.RotateTowards(SelectedModel.transform.localRotation, Quaternion.Euler(new Vector3(-90f, 0f, 90f)), 3f * (Time.deltaTime * 100f));
                } else if (Variables.z == 1f && SelectedModel.transform.localEulerAngles.y > 0f) {
                    // opened
                    SelectedModel.transform.localRotation = Quaternion.RotateTowards(SelectedModel.transform.localRotation, Quaternion.Euler(new Vector3(-90f, 0f, 0f)), 3f * (Time.deltaTime * 100f));
                } else if (Variables.z == 2f && SelectedModel.transform.localEulerAngles.y < 90f) {
                    // locked
                    SelectedModel.transform.localRotation = Quaternion.RotateTowards(SelectedModel.transform.localRotation, Quaternion.Euler(new Vector3(-90f, 0f, 90f)), 3f * (Time.deltaTime * 100f));
                } else if (SelectedModel.GetComponent<BoxCollider>().enabled == false) {
                    SelectedModel.GetComponent<BoxCollider>().enabled = true;
                }
            }
        } else if (Variables.x == 4f) {
            if (TooFar == false) {
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
            }
        } else if (Variables.x == 6f) {

            if (RS.GetComponent<RoundScript>().RoundState == "BeforeWave") {
                if (RS.GetComponent<RoundScript>().RoundTime > 5f) {
                    Variables.y = 0f;
                } else if (Variables.y == 0f) {
                    Interaction("RingTheBell", 0f);
                }
            }

            if (Variables.y == 0f) {
                Name = GS.GetComponent<GameScript>().SetString("Horde Bell", "Dzwon Hordy");
                SelectedModel.GetComponent<Interactions>().Options = new string[] {"RingBell"};
            } else if (Variables.y == 1f) {
                Name = GS.GetComponent<GameScript>().SetString("Horde Bell", "Dzwon Hordy");
                SelectedModel.GetComponent<Interactions>().Options = new string[] {""};
            }

        } else if (Variables.x == 7f) {

            if (ammoID != (int)Variables.y) {
                ammoID = (int)Variables.y;

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

            if (Variables.x == 1f) {

                Variables.y -= VariableBonus;
                SelectedModel.transform.GetChild(0).GetComponent<AudioSource>().Play();
                this.transform.Rotate(new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), 0f));

                if (Variables.y <= 0f) {
                    GameObject Debris = Instantiate(EffectPrefab) as GameObject;
                    Debris.transform.position = this.transform.position + Vector3.up * 1f;
                    Debris.GetComponent<EffectScript>().EffectName = "BarrelBreak";
                    Debris.GetComponent<EffectScript>().EffectColor = SelectedModel.GetComponent<MeshRenderer>().material.color;
                    string[] ItemsToSpawn = new string[] { };
                    int AmountToSpawn = 0;
                    if (Variables.z == 1f) {
                        ItemsToSpawn = new string[]{"id2;" + "va" + (int)Random.Range(50f, 100.9f), "id3;", "id14;va" + (int)Random.Range(50f, 100.9f) + ";", "id11;cl" + (int)Random.Range(0f, 10.9f) + ";", "id19;", "id17;", "id18;"};//new Vector3[] { new Vector3(2f, Random.Range(1f, 100f), 0f), new Vector3(3f, 0f, 0f), new Vector3(14f, Random.Range(1f, 100f), 0f), new Vector3(11f, 0f, 0f), new Vector3(19f, 0f, 0f), new Vector3(17f, 0f, 0f), new Vector3(18f, 0f, 0f), new Vector3(19f, 0f, 0f) };
                        AmountToSpawn = Random.Range(1, 3);
                    } else if (Variables.z == 2f) {
                        //ItemsToSpawn = new Vector3[] { new Vector3(2f, 100f, 0f), new Vector3(3f, 0f, 0f), new Vector3(14f, 100f, 0f), new Vector3(17f, 0f, 0f), new Vector3(18f, 0f, 0f), new Vector3(6f, 0f, 0f), new Vector3(22f, 0f, 0f), new Vector3(23f, 0f, 0f), new Vector3(15f, 100f, 0f) };
                        ItemsToSpawn = new string[]{ GS.itemCache[(int)Random.Range(1f, 20f)].getName(), GS.itemCache[(int)Random.Range(1f, 20f)].startVariables, GS.itemCache[(int)Random.Range(1f, 20f)].startVariables };//new Vector3[] { GS.GetComponent<GameScript>().ReceiveItemVariables(Random.Range(1f, 20f)), GS.GetComponent<GameScript>().ReceiveItemVariables(Random.Range(1f, 20f)), GS.GetComponent<GameScript>().ReceiveItemVariables(Random.Range(1f, 20f)) };
                        AmountToSpawn = Random.Range(2, 5);
                    } else if (Variables.z == 3f) {
                        //ItemsToSpawn = new Vector3[] { new Vector3(15f, 100f, 0f), new Vector3(4f, 0f, 0f), new Vector3(27f, 100f, 0f), new Vector3(22f, 0f, 0f), new Vector3(23f, 0f, 0f), new Vector3(29f, 0f, 0f), new Vector3(18f, 0f, 0f), new Vector3(16f, 100f, 0f) };
                        ItemsToSpawn = new string[] { GS.itemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables, GS.itemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables, GS.itemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables };//new Vector3[] { GS.GetComponent<GameScript>().ReceiveItemVariables((int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)), GS.GetComponent<GameScript>().ReceiveItemVariables((int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)), GS.GetComponent<GameScript>().ReceiveItemVariables((int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)) };
                        AmountToSpawn = Random.Range(2, 5);
                    } else if (Variables.z == 4f) {
                        ItemsToSpawn = new string[]{ "id14;va100;", "id15;va100;", "id16;va100;", "id27;va100;", "id28;va100", "id29;", "id30;", "id26;", "id21;", "id31;va8;", "id32;va6;", "id35;va2;", "id33;", "id36;va30;", "id37;", "id38;va30;", "id39;" };//new Vector3[] { new Vector3(14f, 100f, 0f), new Vector3(15f, 100f, 0f), new Vector3(16f, 100f, 0f), new Vector3(27f, 100f, 0f), new Vector3(28f, 100f, 0f), new Vector3(29f, 0f, 0f), new Vector3(30f, 0f, 0f), new Vector3(26f, 0, 0f), new Vector3(21f, 0f, 0f), new Vector3(31f, 8f, 0f), new Vector3(32f, 6f, 0f), new Vector3(35f, 2f, 0f), new Vector3(33f, 0f, 0f), new Vector3(36f, 30f, 0f), new Vector3(37f, 0f, 0f), new Vector3(38f, 30f, 0f), new Vector3(39f, 0f, 0f) };
                        AmountToSpawn = Random.Range(1, 2);
                    } else if (Variables.z == 5f) {
                        //ItemsToSpawn = new Vector3[] { new Vector3(2f, 100f, 0f), new Vector3(11f, 0f, 0f), new Vector3(12f, 0f, 0f), new Vector3(4f, 0f, 0f), new Vector3(20f, 0f, 0f), new Vector3(16f, 100f, 0f), new Vector3(27f, 100f, 0f), new Vector3(19f, 0f, 0f), new Vector3(17f, 0f, 0f), new Vector3(18f, 0f, 0f), new Vector3(32f, 5f, 0f), new Vector3(33f, 0f, 0f), new Vector3(42f, 30f, 0f), new Vector3(40f, 6f, 0f) };
                        ItemsToSpawn = new string[]{GS.itemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables, GS.itemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables, GS.itemCache[(int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)].startVariables};//new Vector3[] { GS.GetComponent<GameScript>().ReceiveItemVariables((int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)), GS.GetComponent<GameScript>().ReceiveItemVariables((int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)), GS.GetComponent<GameScript>().ReceiveItemVariables((int)Random.Range(1f, RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f)), };
                        AmountToSpawn = Random.Range(5, 10);
                    }
                    for (int SpawnStuff = AmountToSpawn; SpawnStuff > 0; SpawnStuff--) {
                        GameObject CreateItem = Instantiate(ItemPrefab) as GameObject;
                        CreateItem.transform.position = this.transform.position + (Vector3.up * 1f) + (Vector3.up * (SpawnStuff / 2f));
                        string[] PickRandomItem = ItemsToSpawn;
                        CreateItem.GetComponent<ItemScript>().Variables = PickRandomItem[(int)Random.Range(0f, PickRandomItem.Length - 0.1f)];
                    }
                    Destroy(this.gameObject);
                }

            } else if (Variables.x == 3f) {

                Variables.y -= VariableBonus;
                SelectedModel.transform.GetChild(0).GetComponent<AudioSource>().Play();
                SelectedModel.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, Random.Range(60f, 30f)));
                if (Variables.y <= 0f) {
                    GameObject Debris = Instantiate(EffectPrefab) as GameObject;
                    Debris.transform.position = this.transform.position + (Vector3.up * 1f) + (this.transform.right * 0.5f);
                    Debris.GetComponent<EffectScript>().EffectName = "DoorBreak";
                    Debris.GetComponent<EffectScript>().EffectColor = SelectedModel.GetComponent<MeshRenderer>().material.color;
                    Destroy(this.gameObject);
                }

            }

        } else if (WhatToDo == "Door") {

            SelectedModel.GetComponent<BoxCollider>().enabled = false;
            if (Variables.z == 0f) {
                Variables.z = 1f;
                SelectedModel.transform.GetChild(1).GetComponent<AudioSource>().Play();
            } else if (Variables.z == 1f) {
                Variables.z = 0f;
                SelectedModel.transform.GetChild(2).GetComponent<AudioSource>().Play();
            } else if (Variables.z == 2f) {
                SelectedModel.transform.GetChild(3).GetComponent<AudioSource>().Play();
            }

        } else if (WhatToDo == "SetItem") {

            Variables.y = VariableBonus;
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
                if (GetModel.GetSiblingIndex() == Variables.y) {
                    PickedObject = GetModel.gameObject;
                    SelectedModel.transform.GetChild(2).GetComponent<AudioSource>().Play();
                }
            }

            if (Variables.y >= 0f) {
                SelectedModel.transform.GetChild(0).gameObject.SetActive(true);
                if (PickedObject.name == "Ammunition") {
                    GS.GetComponent<GameScript>().Money += Random.Range(1, 100) * 10;
                } else {
                    GameObject DropItem = Instantiate(ItemPrefab);
                    DropItem.transform.position = this.transform.position + (this.transform.forward * 0.25f);
                    DropItem.GetComponent<ItemScript>().Variables = GS.itemCache[int.Parse(PickedObject.name.Substring(4))].startVariables;
                }
            } else {
                SelectedModel.transform.GetChild(0).gameObject.SetActive(false);
            }

            this.GetComponent<InteractableScript>().Interaction("SetItem", -1f);

        } else if (WhatToDo == "RingTheBell") {
            Variables.y = 1f;
            RS.GetComponent<RoundScript>().RoundTime = 5f;
            SelectedModel.transform.GetChild(0).GetComponent<AudioSource>().Play();
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(1).GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
            GS.GetComponent<GameScript>().SetText(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(0).GetComponent<Text>(), "Get ready!", "Przygotuj się!");
            GS.GetComponent<GameScript>().SetText(GameObject.Find("MainCanvas").GetComponent<CanvasScript>().RoundStartInfo.transform.GetChild(1).GetComponent<Text>(), "Wave " + GS.GetComponent<GameScript>().Round + " incoming", "Nadchodzi fala " + GS.GetComponent<GameScript>().Round);
        } else if (WhatToDo == "GatherAmmo") {
            Variables.y -= 1;
            int ammo = Random.Range(1, 10) * 5;
            GS.Ammo += ammo;
            GS.Mess(GS.SetString("Ammo +", "Amunicja +") + ammo, "HordeDropWeapon");
            if (Variables.y <= 0f)
                this.transform.position = Vector3.one * -999f;
        } else if (WhatToDo == "SetUpAmmo") {
            Variables.y = Random.Range(5, 15);
            GS.Mess(GS.SetString("An ammo crate has been deployed somewhere on the map!", "Gdzieś na mapie dostarczono skrzynię z amunicją!"), "Draw");
        }

    }

}
