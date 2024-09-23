using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // Variables
    public string Special = "";
    public string[] NewStuffToSpawn;
    public Vector3[] StuffToSpawn;
    public float[] Chance;
    // Variables

    // References
    public GameScript GS;
    public GameObject ObjectToSpawn;
    public GameObject RS;
    // References

    // Update is called once per frame
    void Start() {

        if (this.GetComponent<BoxCollider>() != null) {
            Destroy(this.GetComponent<BoxCollider>());
        }

    }

    public void Spawn() {

        RS = GameObject.Find("_RoundScript");
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        if (Special == "HouseStuff" || Special == "CabinStuff") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(Mathf.Clamp(Random.Range(1f, 20f + (RS.GetComponent<RoundScript>().DifficultySlider * 2f) * (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 20f)), 0f, (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f))) };
            NewStuffToSpawn = new string[]{GS.itemCache[(int)Mathf.Clamp(Random.Range(1f, 20f + RS.GetComponent<RoundScript>().DifficultySliderB * 2f * (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 20f)), 0f, (int)(RS.GetComponent<RoundScript>().TotalItems.Length - 0.1f))].startVariables};
            if (GS.GetSemiClass(NewStuffToSpawn[0], "id") == "13") {
                NewStuffToSpawn[0] = GS.SetSemiClass(NewStuffToSpawn[0], "id", "1");
            }
            Chance = new float[] { 50f, 10f };
        } else if (Special == "KitchenStuff" || Special == "Food") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().FoodItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().FoodItems.Length - 0.1f)]) };
            NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().FoodItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().FoodItems.Length - 0.1f)]].startVariables};
            Chance = new float[] { 50f, 10f };
        } else if (Special == "IndustrialStuff") {
            int WhatExactly = Random.Range(0, 5);
            if (WhatExactly == 0) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]].startVariables};
            } else {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]].startVariables};
            }
            Chance = new float[] { 50f, 10f };
        } else if (Special == "BasementSpecial" || Special == "WellSpecial") {
            int WhatExactly = Random.Range(0, 3);
            if (WhatExactly == 0) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]].startVariables};
            } else if (WhatExactly == 1) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]].startVariables};
            } else {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]].startVariables};
            }
            Chance = new float[] { 100f, 25f };
        } else if (Special == "Weaponary") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]) };
            NewStuffToSpawn = new string[]{ GS.itemCache[RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]].startVariables };
            Chance = new float[] { 10f, 2f };
        } else if (Special == "Utilities") {
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]) };
            NewStuffToSpawn = new string[]{ GS.itemCache[RS.GetComponent<RoundScript>().Utilities[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Utilities.Length - 0.1f)]].startVariables };
            Chance = new float[] { 100f, 25f };
        } else if (Special == "MilitaryStuff"){
            int WhatExactly = Random.Range(0, 6);
            if (WhatExactly == 0) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().AmmoItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AmmoItems.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().AmmoItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AmmoItems.Length - 0.1f)]].startVariables};
            } else if (WhatExactly == 1) {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().AttachmentItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AttachmentItems.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().AttachmentItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().AttachmentItems.Length - 0.1f)]].startVariables};
            } else {
                //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]) };
                NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().Weapons[(int)Random.Range(0f, RS.GetComponent<RoundScript>().Weapons.Length - 0.1f)]].startVariables};
            }
            Chance = new float[] { 50f, 10f};
        } else if (Special == "MedicalStuff"){
            //StuffToSpawn = new Vector3[] { GameObject.Find("_GameScript").GetComponent<GameScript>().ReceiveItemVariables(RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]) };
            NewStuffToSpawn = new string[]{GS.itemCache[RS.GetComponent<RoundScript>().HealingItems[(int)Random.Range(0f, RS.GetComponent<RoundScript>().HealingItems.Length - 0.1f)]].startVariables };
            Chance = new float[] { 50f, 10f };
        } else if (Special == "LeftBarrel") {
            //StuffToSpawn = new Vector3[] { new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 0f) };
            NewStuffToSpawn = new string[]{"x1;y0;z0;"};
            Chance = new float[] { 50f, 50f };
        } else if (Special == "Doors") {
            //StuffToSpawn = new Vector3[] { new Vector3(3f, 200f, 0f), new Vector3(3f, 200f, 0f) };
            NewStuffToSpawn = new string[]{"x3;y200;z0;"};
            Chance = new float[] { 25f, 25f };
        } else if (Special == "Guards") {
            //StuffToSpawn = new Vector3[] { new Vector3(8f, 0f, 0f)};
            NewStuffToSpawn = new string[]{"id8;"};
            Chance = new float[] { 100f, 100f };
        } else if (Special == "VendingMachine") {
            //StuffToSpawn = new Vector3[] { new Vector3(4f, 0f, 0f), new Vector3(4f, 0f, 0f) };
            NewStuffToSpawn = new string[]{"x4;y0;z0;"};
            Chance = new float[] { 10f, 5f };
        } else if (NewStuffToSpawn.Length <= 0){
            Chance = new float[]{};
        }

        float PickChance = Random.Range(0f, 100f);
        if (Chance.Length >= 2 && PickChance <= Mathf.Lerp(Chance[0], Chance[1], RS.GetComponent<RoundScript>().DifficultySliderB)) {
            if (ObjectToSpawn.tag == "Item") {
                GameObject SpawnItem = Instantiate(ObjectToSpawn) as GameObject;
                SpawnItem.transform.position = this.transform.position;
                //print(Special);
                //print(NewStuffToSpawn.Length);
                SpawnItem.GetComponent<ItemScript>().Variables = NewStuffToSpawn[(int)Random.Range(0f, NewStuffToSpawn.Length - 0.1f)];
                SpawnItem.GetComponent<ItemScript>().State = 1;
            } else if (ObjectToSpawn.tag == "Interactable") {
                GameObject SpawnItem = Instantiate(ObjectToSpawn) as GameObject;
                SpawnItem.transform.position = this.transform.position;
                //print(NewStuffToSpawn.Length);
                string pickitem = NewStuffToSpawn[Random.Range(0, NewStuffToSpawn.Length)];
                //print(pickitem);
                //print(GS.GetSemiClass(pickitem, "x") + ";" + GS.GetSemiClass(pickitem, "y") + ";" + GS.GetSemiClass(pickitem, "z") + ";");
                SpawnItem.GetComponent<InteractableScript>().Variables = new Vector3(float.Parse(GS.GetSemiClass(pickitem, "x")), float.Parse(GS.GetSemiClass(pickitem, "y")), float.Parse(GS.GetSemiClass(pickitem, "z")));
                if (SpawnItem.GetComponent<InteractableScript>().Variables.x == 3f || SpawnItem.GetComponent<InteractableScript>().Variables.x == 4f) {
                    SpawnItem.transform.rotation = this.transform.rotation;
                }
            } else if (ObjectToSpawn.tag == "Mob") {
                GameObject SpawnItem = Instantiate(ObjectToSpawn) as GameObject;
                SpawnItem.transform.position = this.transform.position;
                //print(NewStuffToSpawn.Length);
                SpawnItem.GetComponent<MobScript>().TypeOfMob = int.Parse(GS.GetSemiClass(NewStuffToSpawn[Random.Range(0, NewStuffToSpawn.Length)], "id"));
            }
        }
		
	}
}
