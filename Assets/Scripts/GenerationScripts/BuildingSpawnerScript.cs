using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnerScript : MonoBehaviour {

    public AnimationCurve ChanceOfRuing, ChanceOfResignation;
    public GameObject[] Buildings, Ruins;

    public Color[] WallColors, InnerColors, BathColors, PorchColors, RoofColors;
    
    public void SpawnBuilding (float diff, Transform parent, Transform foward) {

        if (Random.value > ChanceOfResignation.Evaluate(diff)) {

            GameObject newBuilding = GameObject.Instantiate( Random.value > ChanceOfRuing.Evaluate(diff)
                ? Buildings[Random.Range(0, Buildings.Length)]
                : Ruins[Random.Range(0, Ruins.Length)] );
            
            Vector3 localRotation = newBuilding.transform.eulerAngles;
            newBuilding.transform.SetParent(foward);
            newBuilding.transform.SetLocalPositionAndRotation(
                Vector3.zero,
                Quaternion.Euler(localRotation)
            );
            newBuilding.transform.SetParent(parent);
            newBuilding.name = "OVER HERE";
            
            // Set up material colors
            int pickID;
            
            pickID = Random.Range(0, WallColors.Length - 1);
            Color WallColor = Color.Lerp(WallColors[pickID], WallColors[pickID + 1], Random.value);

            pickID = Random.Range(0, InnerColors.Length - 1);
            Color InnerWallColor = Color.Lerp(InnerColors[pickID], InnerColors[pickID + 1], Random.value);

            pickID = Random.Range(0, BathColors.Length - 1);
            Color BathWallColor = Color.Lerp(BathColors[pickID], BathColors[pickID + 1], Random.value);

            pickID = Random.Range(0, PorchColors.Length - 1);
            Color PorchWallColor = Color.Lerp(PorchColors[pickID], PorchColors[pickID + 1], Random.value);

            pickID = Random.Range(0, RoofColors.Length - 1);
            Color RoofColor = Color.Lerp(RoofColors[pickID], RoofColors[pickID + 1], Random.value);

            foreach (Transform child in newBuilding.transform) {
                if (child.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    foreach (Material Mat in renderer.materials) {
                        if (Mat.name == "HouseOuter1 (Instance)") {
                            Mat.color = WallColor;
                        } else if (Mat.name == "HouseOuter2 (Instance)") {
                            Mat.color = WallColor / 2f;
                        } else if (Mat.name == "HouseInner (Instance)") {
                            Mat.color = InnerWallColor;
                        } else if (Mat.name == "HouseBath (Instance)") {
                            Mat.color = BathWallColor;
                        } else if (Mat.name == "HousePorch (Instance)") {
                            Mat.color = PorchWallColor;
                        } else if (Mat.name == "HouseRoof (Instance)") {
                            Mat.color = RoofColor;
                        } else if (Mat.name == "HouseCarpet1 (Instance)") {
                            Mat.color = Color.HSVToRGB((.7f + Random.value * .6f) % 1f, 1f, .25f);
                        }
                    }
            }

        }

    }

}
