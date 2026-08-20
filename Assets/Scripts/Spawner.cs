using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // Variables
    public string Special = "";
    public string[] NewStuffToSpawn;
    public JClass[] theNewStuffToSpawn;
    public float[] Chance;
    public bool Freeze;
    // Variables

    // References
    public GameScript GS;
    public GameObject ObjectToSpawn;
    public GameObject RS;
    // References

    // Update is called once per frame
    void Awake() {

        if (this.GetComponent<BoxCollider>() != null)
            Destroy(this.GetComponent<BoxCollider>());

        if (!Freeze)
            RoundScript.CachedSpawner.Add(this);

    }

    public void Spawn(string[] args = default) {

        RS = GameObject.Find("_RoundScript");
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        if (Special == "HouseStuff" || Special == "CabinStuff") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(Mathf.Clamp(Random.Range(1f, 20f + (RS.GetComponent<RoundScript>().DifficultySlider * 2f) * (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 20f)), 0f, (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f))) };
            theNewStuffToSpawn = new JClass[]{
                new (GS.ItemCache[(int)Mathf.Clamp(Random.Range(1f, 20f + RS.GetComponent<RoundScript>().DifficultySliderB * 2f * (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 20f)), 0f, (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f))].startVariables)
            };
            if (theNewStuffToSpawn[0].GetInt(JType.ID) == 13) {
                theNewStuffToSpawn[0].SetInt(JType.ID, 1);
            }
            Chance = new float[] { 50f, 10f };
        } else if (Special == "KitchenStuff" || Special == "Food") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().FoodItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().FoodItems.Length - 0.1f)]) };
            theNewStuffToSpawn = new JClass[]{
                new (GS.ItemCache[RS.GetComponent<RoundScript>().FoodItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().FoodItems.Length - 0.1f)]].startVariables)
            };
            Chance = new float[] { 50f, 10f };
        } else if (Special == "IndustrialStuff") {
            int WhatExactly = Random.Range(0, 5);
            if (WhatExactly == 0) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]].startVariables)
                };
            } else {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]].startVariables)
                };
            }
            Chance = new float[] { 50f, 10f };
        } else if (Special == "BasementSpecial" || Special == "WellSpecial" || Special == "SafeSpecial") {
            int WhatExactly = Random.Range(0, 3);
            if (WhatExactly == 0) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]].startVariables)
                };
            } else if (WhatExactly == 1) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]].startVariables)
                };
            } else {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]].startVariables)
                };
            }
            Chance = Special == "SafeSpecial" ? new float[] {100f, 100f} :  new float[] { 100f, 25f };
        } else if (Special == "Weaponary") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]) };
            theNewStuffToSpawn = new JClass[]{
                new (GS.ItemCache[RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]].startVariables)
            };
            Chance = new float[] { 10f, 2f };
        } else if (Special == "Utilities") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]) };
            float WhatExactly = Random.Range(0, 10);
            if (WhatExactly < 1f)
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().CraftingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().CraftingItems.Length - 0.1f)]].startVariables)
                };
            else
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]].startVariables)
                };
            Chance = new float[] { 100f, 25f };
        } else if (Special == "MilitaryStuff"){
            int WhatExactly = Random.Range(0, 6);
            if (WhatExactly == 0) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().AmmoItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AmmoItems.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().AmmoItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AmmoItems.Length - 0.1f)]].startVariables)
                };
            } else if (WhatExactly == 1) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().AttachmentItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AttachmentItems.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().AttachmentItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AttachmentItems.Length - 0.1f)]].startVariables)
                };
            } else {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]) };
                theNewStuffToSpawn = new JClass[]{
                    new (GS.ItemCache[RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]].startVariables)
                };
            }
            Chance = new float[] { 50f, 10f};
        } else if (Special == "MedicalStuff"){
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]) };
            theNewStuffToSpawn = new JClass[]{
                new (GS.ItemCache[RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]].startVariables)
            };
            Chance = new float[] { 50f, 10f };
        } else if (Special == "BuildingItems") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]) };
            theNewStuffToSpawn = new JClass[]{
                new (GS.ItemCache[RS.GetComponent<RoundScript>().BuildingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().BuildingItems.Length - 0.1f)]].startVariables)
            };
            Chance = new float[] { 100f, 100f };
        } else if (Special == "LeftBarrel") {
            //StuffToSpawn = new Vector3[] { new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 0f) };
            theNewStuffToSpawn = new JClass[]{new (1, JTemplate.JustID)};//new string[]{"x1;y0;z0;"};
            Chance = new float[] { 50f, 50f };
        } else if (Special == "Doors") {
            //StuffToSpawn = new Vector3[] { new Vector3(3f, 200f, 0f), new Vector3(3f, 200f, 0f) };
            theNewStuffToSpawn = new JClass[]{
                new JClass (new JEntry[]{
                    new JInt (JType.ID, 3),
                    new JFloat (JType.VariableA, 200f)
                })
            };//new string[]{"x3;y200;z0;"};
            Chance = new float[] { 25f, 25f };
        } else if (Special == "Guards") {
            //StuffToSpawn = new Vector3[] { new Vector3(8f, 0f, 0f)};
            theNewStuffToSpawn = new JClass[]{ new JClass(8, JTemplate.JustID) };
            Chance = new float[] { 100f, 100f };
        } else if (Special == "VendingMachine") {
            //StuffToSpawn = new Vector3[] { new Vector3(4f, 0f, 0f), new Vector3(4f, 0f, 0f) };
            theNewStuffToSpawn = new JClass[]{ new JClass(4, JTemplate.JustID) };
            Chance = new float[] { 10f, 5f };
        } else if (Special == "Chests_SmallHouse") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "Closet")),
                new (new JString(JType.SpawnStuffString, "Chest")),
                new (new JString(JType.SpawnStuffString, "Fridge")),
                new (new JString(JType.SpawnStuffString, "Desk")),
                new (new JString(JType.SpawnStuffString, "Safe")),
                new (new JString(JType.SpawnStuffString, "Bathroom"))
            };
            Chance = new float[] { 100f, 25f };
        } else if (Special == "Chests_BigHouse") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "Closet")),
                new (new JString(JType.SpawnStuffString, "Chest")),
                new (new JString(JType.SpawnStuffString, "Fridge")),
                new (new JString(JType.SpawnStuffString, "Desk")),
                new (new JString(JType.SpawnStuffString, "Safe")),
                new (new JString(JType.SpawnStuffString, "Bathroom")),
                new (new JString(JType.SpawnStuffString, "Bathroom2")),
                new (new JString(JType.SpawnStuffString, "WallUnit")),
                new (new JString(JType.SpawnStuffString, "Kitchen"))
            };
            Chance = new float[] { 75f, 5f };
        } else if (Special == "Chests_SmallLivingroom") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "Closet")),
                new (new JString(JType.SpawnStuffString, "Chest")),
                new (new JString(JType.SpawnStuffString, "Desk")),
                new (new JString(JType.SpawnStuffString, "Safe"))
            };
            Chance = new float[] { 100f, 25f };
        } else if (Special == "Chests_BigLivingroom") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "Closet")),
                new (new JString(JType.SpawnStuffString, "Chest")),
                new (new JString(JType.SpawnStuffString, "Desk")),
                new (new JString(JType.SpawnStuffString, "Safe")),
                new (new JString(JType.SpawnStuffString, "WallUnit"))
            };
            Chance = new float[] { 75f, 5f };
        } else if (Special == "Chests_SmallKitchen") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "Fridge")),
                new (new JString(JType.SpawnStuffString, "Safe")),
                new (new JString(JType.SpawnStuffString, "Bathroom"))
            };
            Chance = new float[] { 100f, 25f };
        } else if (Special == "Chests_BigKitchen") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "HardwareDesk")),
                new (new JString(JType.SpawnStuffString, "Fridge")),
                new (new JString(JType.SpawnStuffString, "Safe")),
                new (new JString(JType.SpawnStuffString, "Bathroom")),
                new (new JString(JType.SpawnStuffString, "Bathroom2")),
                new (new JString(JType.SpawnStuffString, "Kitchen"))
            };
            Chance = new float[] { 75f, 5f };
        } else if (Special == "Chests_SmallRuin") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "Closet")),
                new (new JString(JType.SpawnStuffString, "Chest")),
                new (new JString(JType.SpawnStuffString, "Fridge")),
                new (new JString(JType.SpawnStuffString, "Desk")),
                new (new JString(JType.SpawnStuffString, "Safe")),
                new (new JString(JType.SpawnStuffString, "Bathroom")),
            };
            Chance = new float[] { 50f, 2f };
        } else if (Special == "Chests_BigRuin") {
            theNewStuffToSpawn = new JClass[] {
                new (new JString(JType.SpawnStuffString, "Closet")),
                new (new JString(JType.SpawnStuffString, "Chest")),
                new (new JString(JType.SpawnStuffString, "Fridge")),
                new (new JString(JType.SpawnStuffString, "Desk")),
                new (new JString(JType.SpawnStuffString, "Safe")),
                new (new JString(JType.SpawnStuffString, "Bathroom")),
                new (new JString(JType.SpawnStuffString, "Bathroom2")),
                new (new JString(JType.SpawnStuffString, "WallUnit")),
                new (new JString(JType.SpawnStuffString, "Kitchen"))
            };
            Chance = new float[] { 25f, 1f };
        } else if (theNewStuffToSpawn.Length <= 0){
            Chance = new float[]{};
        }

        float PickChance = Random.Range(0f, 100f);
        if (Chance.Length >= 2 && PickChance <= Mathf.Lerp(Chance[0], Chance[1], RS.GetComponent<RoundScript>().DifficultySliderB)) {
            if (ObjectToSpawn.tag == "Item") {
                GameObject SpawnItem = Instantiate(ObjectToSpawn) as GameObject;
                SpawnItem.GetComponent<ItemScript>().Variables.CopyFrom( theNewStuffToSpawn[(int)Random.Range(0f, theNewStuffToSpawn.Length - 0.1f)] );
                
                if (args != null && args.Length > 0 && args[0] == "BrokenChest") {
                    SpawnItem.GetComponent<ItemScript>().State = 0;
                    SpawnItem.GetComponent<Rigidbody>().AddForce(new (
                        Random.Range(-10f, 10f),
                        Random.Range(0f, 10f),
                        Random.Range(-10f, 10f)
                    ), ForceMode.VelocityChange);
                } else
                    SpawnItem.GetComponent<ItemScript>().State = 1;

                SpawnItem.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            } else if (ObjectToSpawn.tag == "Interactable") {
                GameObject SpawnItem = Instantiate(ObjectToSpawn) as GameObject;
                SpawnItem.transform.position = this.transform.position;
                JClass pickitem = theNewStuffToSpawn[Random.Range(0, theNewStuffToSpawn.Length)];

                SpawnItem.GetComponent<InteractableScript>().Variables.CopyFrom( pickitem );
                if (SpawnItem.GetComponent<InteractableScript>().Variables.GetInt(JType.ID) is 3 or 4) {
                    SpawnItem.transform.rotation = this.transform.rotation;
                }
            } else if (ObjectToSpawn.tag == "Mob") {
                GameObject SpawnItem = Instantiate(ObjectToSpawn) as GameObject;
                SpawnItem.transform.position = this.transform.position;
                //print(NewStuffToSpawn.Length);
                SpawnItem.GetComponent<MobScript>().TypeOfMob = theNewStuffToSpawn[Random.Range(0, theNewStuffToSpawn.Length)].GetInt(JType.ID);
            } else if (ObjectToSpawn.tag == "Chest") {
                JClass chosen = theNewStuffToSpawn[Random.Range(0, theNewStuffToSpawn.Length)];
                for (int c = 0; c <= ObjectToSpawn.transform.childCount; c++)
                    if (c == ObjectToSpawn.transform.childCount)
                        Debug.LogError($"No chest of name {chosen.GetString(JType.SpawnStuffString)} found");
                    else if (ObjectToSpawn.transform.GetChild(c).name == chosen.GetString(JType.SpawnStuffString)) {
                        ChestScript SpawnChest = Instantiate(ObjectToSpawn.transform.GetChild(c).gameObject).GetComponent<ChestScript>();
                        SpawnChest.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
                        break;
                    }
                
            }
        }
		
	}
}
