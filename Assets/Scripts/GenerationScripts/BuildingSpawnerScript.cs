using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnerScript : MonoBehaviour {

    public AnimationCurve ChanceOfRuing, ChanceOfResignation;
    public GameObject[] Buildings, Ruins;

    public Color[] WallColors, InnerColors, BathColors, PorchColors, RoofColors, TileColorsA, TileColorsB;
    Color WallColor, InnerWallColor, BathWallColor, PorchWallColor, RoofColor, TileColorA, TileColorB;
    
    public void SpawnBuilding (float diff, Transform parent, Transform foward) {

        // Disable gizmo
        if (TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
            mesh.enabled = false;

        if (Random.value > ChanceOfResignation.Evaluate(diff)) {

            GameObject newBuilding;

            if (Ruins != null && Ruins.Length > 0)
                newBuilding = GameObject.Instantiate( Random.value > ChanceOfRuing.Evaluate(diff)
                    ? Buildings[Random.Range(0, Buildings.Length)]
                    : Ruins[Random.Range(0, Ruins.Length)] );
            else
                newBuilding = GameObject.Instantiate(Buildings[Random.Range(0, Buildings.Length)]);
            
            Vector3 localRotation = newBuilding.transform.eulerAngles;
            newBuilding.transform.SetParent(foward);
            newBuilding.transform.SetLocalPositionAndRotation(
                Vector3.zero,
                Quaternion.Euler(localRotation)
            );
            newBuilding.transform.SetParent(parent);
            newBuilding.name = "OVER HERE";
            newBuilding.transform.localScale = Vector3.one;
            
            // Set up material colors            
            WallColor = GenerateColor(WallColors);
            InnerWallColor = GenerateColor(InnerColors);
            BathWallColor = GenerateColor(BathColors);
            PorchWallColor = GenerateColor(PorchColors);
            RoofColor = GenerateColor(RoofColors);
            TileColorA = GenerateColor(TileColorsA);
            TileColorB = GenerateColor(TileColorsB);

            foreach (Transform child in newBuilding.transform) {
                TryPart(child);
                if (child.childCount > 0)
                    foreach (Transform subChild in child)
                        TryPart(subChild);
            }

        }

    }

    void TryPart (Transform Part) {
        if (Part.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            foreach (Material Mat in renderer.materials) {
                if (Mat.name == "HouseOuter1 (Instance)") {
                    Mat.color = WallColor;
                } else if (Mat.name == "HouseOuter2 (Instance)") {
                    Mat.color = WallColor * .75f;
                } else if (Mat.name == "HouseInner (Instance)") {
                    Mat.color = InnerWallColor;
                } else if (Mat.name == "HouseBath (Instance)") {
                    Mat.color = BathWallColor;
                } else if (Mat.name == "HousePorch (Instance)") {
                    Mat.color = PorchWallColor;
                } else if (Mat.name == "HouseRoof (Instance)") {
                    Mat.color = RoofColor;
                } else if (Mat.name == "HouseRoof2 (Instance)") {
                    Mat.color = RoofColor * .8f;
                } else if (Mat.name == "HouseRoof3 (Instance)") {
                    Mat.color = RoofColor * .6f;
                } else if (Mat.name == "HouseCarpet1 (Instance)") {
                    Mat.color = Color.HSVToRGB((.7f + Random.value * .6f) % 1f, 1f, .25f);
                } else if (Mat.name == "HouseTiles1 (Instance)") {
                    Mat.color = TileColorA;
                } else if (Mat.name == "HouseTiles2 (Instance)") {
                    Mat.color = TileColorB;
                }
            }
    }

    Color GenerateColor (Color[] colors) {
        int pickID = Random.Range(0, colors.Length - 1);

        if (colors == null || colors.Length == 0)
            return Color.clear;
        else if (colors.Length == 1)
            return colors[0];
        else
            return Color.Lerp(colors[pickID], colors[pickID + 1], Random.value);
    }

}
