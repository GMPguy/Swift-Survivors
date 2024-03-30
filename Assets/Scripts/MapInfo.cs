using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour {

    // Variables
    public string[] MapName;
    public string Ambience = "";
    public string Music = "";
    public Color32[] SkyColors;
    public Color32[] LightColors;
    public Color32[] AmbientColors;
    public float[] FogDistances;
    public Color32 GrassColor;
    public GameObject[] HordeSpawnPoints;
    public GameObject[] HordeWayPoints;
    public GameObject ColorableProps;
    public Vector2 MapSize;
    public float Sunnyness = 0f;
    public GameObject[] Waters;
    public GameObject[] DynamicObjects;
    GameScript GS;
    RoundScript RS;
    // Misc
    float DiscoLights = 0f;
    // Misc
    // Variables

    void Start() {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();

        foreach (Transform SetColorable in ColorableProps.transform) {
            Color WallColor = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
            Color TreeColor = Color.Lerp(new Color(1f, 0.75f, 0f, 1f), new Color(0.3f, 0.5f, 0.3f, 1f), Random.Range(0f, 1f));
            foreach (Material Mat in SetColorable.GetComponent<MeshRenderer>().materials) {
                if (Mat.name == "Grass1 (Instance)" || Mat.name == "Moss1 (Instance)" || Mat.name == "Moss2 (Instance)") {
                    Mat.color = GrassColor;
                } else if (Mat.name == "Flower1 (Instance)" || Mat.name == "Flower2 (Instance)" || Mat.name == "Flower3 (Instance)") {
                    Mat.color = new Color32((byte)Random.Range(100f, 255f), (byte)Random.Range(100f, 255f), (byte)Random.Range(100f, 255f), 255);
                } else if (Mat.name == "HouseOuter1 (Instance)") {
                    Mat.color = WallColor;
                } else if (Mat.name == "HouseOuter2 (Instance)") {
                    Mat.color = WallColor / 2f;
                } else if (Mat.name == "HouseInner (Instance)") {
                    Mat.color = new Color32((byte)Random.Range(75f, 255f), (byte)Random.Range(75f, 255f), (byte)Random.Range(75f, 255f), 255);
                } else if (Mat.name == "HouseRoof (Instance)") {
                    Mat.color = Color32.Lerp(new Color32(100, 75, 55, 255), new Color32(255, 225, 155, 255), Random.Range(0f, 1f));
                } else if (Mat.name == "WoodenFence1 (Instance)") {
                    Mat.color = Color32.Lerp(new Color32(100, 75, 55, 255), new Color32(188, 155, 133, 255), Random.Range(0f, 1f));
                } else if (Mat.name == "Tree1 (Instance)" || Mat.name == "Tree2 (Instance)" || Mat.name == "Tree3 (Instance)") {
                    Mat.color = TreeColor * Random.Range(0.5f, 1f);
                }
            }
        }

    }

    void Update() {

        foreach (GameObject Water in Waters) {
            Water.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor.r, GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor.g, GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor.b, 0.75f);
        }

        foreach (GameObject GetObj in DynamicObjects) {
            if (GetObj.name == "Club") {
                if (DiscoLights > 0f) {
                    DiscoLights -= 0.02f * (Time.deltaTime * 50f);
                } else {
                    DiscoLights = 0.5f;
                    foreach (Material SetMat in GetObj.GetComponent<MeshRenderer>().materials) {
                        if (SetMat.name == "DiscoLights" + (int)Random.Range(1f, 4.9f) + " (Instance)") {
                            if (RS.GetComponent<RoundScript>().RoundState == "HordeWave") {
                                SetMat.color = Color.HSVToRGB(Random.Range(0, 1f), 1f, 0.75f);
                            } else {
                                SetMat.color = Color.black;
                            }
                        }
                    }
                }
            } else if (GetObj.name == "DiscoLights") {
                if (RS.GetComponent<RoundScript>().RoundState == "HordeWave" && QualitySettings.GetQualityLevel() > 0) {
                    GetObj.SetActive(true);
                    GetObj.transform.Rotate(new Vector3(0f, 0f, GetObj.transform.GetChild(0).GetComponent<Light>().intensity * (Time.deltaTime * 50f)));
                    if (GetObj.transform.GetChild(0).GetComponent<Light>().intensity > 0.5f) {
                        GetObj.transform.GetChild(0).GetComponent<Light>().intensity -= 0.01f * (Time.deltaTime * 50f);
                    } else {
                        GetObj.transform.GetChild(0).GetComponent<Light>().color = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
                        GetObj.transform.GetChild(0).GetComponent<Light>().intensity = Random.Range(1f, 2f);
                    }
                    if (QualitySettings.GetQualityLevel() > 2) {
                        GetObj.transform.GetChild(1).gameObject.SetActive(true);
                        ParticleSystem.MainModule SetCol = GetObj.transform.GetChild(1).GetComponent<ParticleSystem>().main;
                        SetCol.startColor = new Color(GetObj.transform.GetChild(0).GetComponent<Light>().color.r, GetObj.transform.GetChild(0).GetComponent<Light>().color.g, GetObj.transform.GetChild(0).GetComponent<Light>().color.b, 0.003f);
                    } else {
                        GetObj.transform.GetChild(1).gameObject.SetActive(false);
                    }
                } else {
                    GetObj.SetActive(false);
                }
            } else if (GetObj.name == "Bloom") {
                if (QualitySettings.GetQualityLevel() > 2) {
                    GetObj.gameObject.SetActive(true);
                } else {
                    GetObj.gameObject.SetActive(false);
                }
            }
        }

        if (GS.GetSemiClass(GS.RoundSetting, "H", "?") == "4") {
            if (GameObject.FindGameObjectWithTag("Player").transform.position.y < -18f) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Hurt(9999f, "Falling", true, Vector3.zero);
            }
            foreach (GameObject KillFallen in GameObject.FindGameObjectsWithTag("Mob")) {
                if (KillFallen.transform.position.y < -18f) {
                    KillFallen.GetComponent<MobScript>().Hurt(9999f, null, true, Vector3.zero, "Explosion");
                }
            }
        }

    }

}
