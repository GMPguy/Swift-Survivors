using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour {

    // Main variables
    public string PlantType;
    public float VariableA;
    int[] HarvestItems;
    int HarvestAmount;
    int RandomID;

    // References
    public AudioSource HarvestSound;
    public GameObject Interactions;
    public GameObject SelectedPlant;
    public GameObject ItemPrefab;
    GameScript GS;

    public static Color GetBerryColor (float safety) {
        if (safety > .95f)
            return Color32.Lerp (
                new Color32 (128, 255, 0, 255),
                new Color32 (0, 200, 100, 255),
                (safety - .95f) / .05f
            );
        else if (safety > .75f)
            return Color32.Lerp (
                Color.yellow,
                Color.white,
                (safety - .75f) / .1f
            );
        else if (safety > .5f)
            return Color32.Lerp (
                Color.red,
                Color.yellow,
                (safety - .5f) / .25f
            );
        else if (safety > .25f)
            return Color32.Lerp (
                Color.blue,
                Color.red,
                (safety - .25f) / .25f
            );
        else
            return Color32.Lerp (
                Color.black,
                Color.blue,
                safety / .25f
            );
    }

    void Awake() {
        RoundScript.CachedPlants.Add(this);
    }

    public void TheStart () {
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RandomID = Random.Range(int.MinValue, int.MaxValue);

        for (int pp = 0; pp <= transform.childCount; pp++)
            if (pp == transform.childCount)
                Debug.LogError($"Unknown plant type {PlantType}");
            else if (transform.GetChild(pp).name == PlantType) {
                transform.GetChild(pp).gameObject.SetActive(true);
                SelectedPlant = transform.GetChild(pp).gameObject;
                break;
            }
        
        switch (PlantType) {
            case "Rocks":
                HarvestItems = new int[] {142, 147, 145, 175, 146};
                HarvestAmount = Random.Range(1, 4);
                break;
            case "Nettle":
                HarvestItems = new int[] {181};
                HarvestAmount = Random.Range(1, 4);
                break;
            case "Toadstool":
                HarvestItems = new int[] {183};
                HarvestAmount = 1;
                break;
            case "Boletus":
                HarvestItems = new int[] {184};
                HarvestAmount = 1;
                break;
            case "Wheat":
                HarvestItems = new int[] {185};
                HarvestAmount = Random.Range(1, 4);
                break;
            case "Shrub":
                HarvestItems = new int[] {140, 172};
                HarvestAmount = Random.Range(1, 4);
                break;
            case "Sage":
                HarvestItems = new int[] {186};
                HarvestAmount = Random.Range(1, 4);
                break;
            case "WildTubers":
                HarvestItems = new int[] {82, 170};
                HarvestAmount = 1;
                break;
            case "Berries":
                HarvestItems = new int[] {187};
                HarvestAmount = 1;

                VariableA = Random.value;
                foreach (Material mat in SelectedPlant.GetComponent<MeshRenderer>().materials)
                    if (mat.name == "BerriesColor (Instance)")
                        mat.color = GetBerryColor(VariableA);
                break;
            case "Tulip":
                HarvestItems = new int[] {188};
                HarvestAmount = 1;
                break;
            case "Mint":
                HarvestItems = new int[] {182};
                HarvestAmount = Random.Range(1, 4);
                break;
            case "Stump":
                HarvestItems = new int[] {140, 140, 147, 172};
                HarvestAmount = Random.Range(1, 4);
                break;
            default:
                Debug.LogError($"No harvest data for {PlantType}");
                break;
        }

    }

    public void Harvest () {
        Random.InitState(RandomID);

        for (int sh = 0; sh < HarvestAmount; sh++) {
            GameObject drop = Instantiate(ItemPrefab) as GameObject;
            drop.transform.position = this.transform.position + this.transform.up * Random.Range(.5f, 1f);

            int dropID = HarvestItems[Random.Range(0, HarvestItems.Length)];
            drop.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[dropID].startVariables);            

            switch (PlantType) {
                case "Berries":
                    drop.GetComponent<ItemScript>().Variables.SetFloat(JType.VariableA, VariableA);
                    break;
            }
        }

        HarvestSound.transform.SetParent(null);
        HarvestSound.Play();

        Destroy(HarvestSound.gameObject, 1f);
        Destroy(this.gameObject);
    }

    public string ReturnName() =>
        PlantType switch {
            "Rocks" => GS.SetString("Rocks", "Kamienie"),
            "Nettle" => GS.SetString("Nettle", "Pokrzywa"),
            "Toadstool" => GS.SetString("Toadstool", "Muchomor"),
            "Boletus" => GS.SetString("Boletus", "Borowik"),
            "Wheat" => GS.SetString("Wheat", "Zboże"),
            "Shrub" => GS.SetString("Shrub", "Krzak"),
            "Sage" => GS.SetString("Sage", "Szałwia"),
            "WildTubers" => GS.SetString("Wild tubers", "Dzikie bulwy"),
            "Berries" => GS.SetString("Berries", "Jagody"),
            "Tulip" => GS.SetString("Tulip", "Tulipan"),
            "Mint" => GS.SetString("Mint", "Mięta"),
            "Stump" => GS.SetString("Stump", "Pień"),
            _ => ""
        };
}
