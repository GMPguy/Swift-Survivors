using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class MenuSceneScript : MonoBehaviour {
    
    public string[] CurrentScene = {"Normal", ""}; // Current - to switch
    string DelayedMusic;
    public float TimeSincePassed = 0f;
    public GameObject selectedScene;
    public AudioSource[] Musics;
    public Transform MainCamera;
    public Transform[] CameraPoints;
    public Vector2 Cursor;
    float cursorShift, cursorRotation, Vignette = 0f;
    float fogDistance;
    NewMenuScript NMS;
    GameScript GS;
    bool flash;

    public Texture[] Skyboxes;

    void Start()  {
        NMS = GameObject.FindObjectOfType<NewMenuScript>();
        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        ChangeScene(GS.WindowToBootUp switch {
            "BootUp" => "Main",
            "GameOver" => "GameOver",
            _ => "Main"
        });

        DelayedMusic = GS.WindowToBootUp switch {
            "BootUp" => "MenuLooped",
            "GameOver" => "GameOver",
            _ => "MenuLooped"
        };
    }

    void Update() {

        if(NMS.LoadingTime <= 0f){
            Camera cam = MainCamera.GetComponent<Camera>();
            //GS.SetDrawDistance(cam, fogDistance);
            RenderSettings.fogEndDistance = fogDistance;

            if (DelayedMusic != "") {
                ChangeMusic(DelayedMusic);
                DelayedMusic = "";
            }

            if(CurrentScene[1] == ""){
                if(CurrentScene[0] != ""){
                    TimeSincePassed += Time.unscaledDeltaTime;
                    Cursor = Vector2.Lerp(Cursor, new Vector3(Mathf.Clamp(((Input.mousePosition.x / Screen.width)-0.5f) * 2f, -1f, 1f), Mathf.Clamp(((Input.mousePosition.y / Screen.height)-0.5f) * 2f, -1f, 1f)), 0.1f * Time.unscaledDeltaTime*50f);

                    if(TimeSincePassed < 2f) {
                        MainCamera.transform.position = Vector3.Lerp(CameraPoints[0].position, CameraPoints[1].position, TimeSincePassed/2f);
                        MainCamera.transform.rotation = Quaternion.Lerp(CameraPoints[0].rotation, CameraPoints[1].rotation, TimeSincePassed/2f);

                        GS.ContSaturTempInvi[3] = Vignette;
                        
                        if(GS.ExistSemiClass(CameraPoints[0].name, "fv_") && GS.ExistSemiClass(CameraPoints[1].name, "fv_"))
                            cam.fieldOfView = Mathf.Lerp(
                                float.Parse(GS.GetSemiClass(CameraPoints[0].name, "fv_")),
                                float.Parse(GS.GetSemiClass(CameraPoints[1].name, "fv_")),
                                TimeSincePassed/2f
                            );

                        if(GS.ExistSemiClass(CameraPoints[0].name, "sc_Black"))
                            if (!flash) {
                                NMS.Flash(
                                    new []{Color.black, Color.clear},
                                    new []{2f, 2f}
                                );
                                flash = true;
                            }

                    } else {
                        MainCamera.transform.position = CameraPoints[1].position + (CameraPoints[1].right*Cursor.x*cursorShift) + (CameraPoints[1].up*-Cursor.y*cursorShift);
                        MainCamera.transform.eulerAngles = CameraPoints[1].eulerAngles + (CameraPoints[1].right*Cursor.y*cursorRotation) + (CameraPoints[1].up*Cursor.x*cursorRotation);
                    
                        if(GS.ExistSemiClass(CameraPoints[1].name, "fv_"))
                            cam.fieldOfView = float.Parse(GS.GetSemiClass(CameraPoints[1].name, "fv_"));
                    }
                }
            } else {

                if(TimeSincePassed > 0f){
                    TimeSincePassed -= Time.unscaledDeltaTime;
                    MainCamera.transform.position = Vector3.Lerp(CameraPoints[2].position, CameraPoints[1].position, TimeSincePassed);
                    MainCamera.transform.rotation = Quaternion.Lerp(CameraPoints[2].rotation, CameraPoints[1].rotation, TimeSincePassed);

                    if(GS.ExistSemiClass(CameraPoints[1].name, "fv_") && GS.ExistSemiClass(CameraPoints[2].name, "fv_"))
                        cam.fieldOfView = Mathf.Lerp(
                            float.Parse(GS.GetSemiClass(CameraPoints[2].name, "fv_")),
                            float.Parse(GS.GetSemiClass(CameraPoints[1].name, "fv_")),
                            TimeSincePassed
                        );

                    if(GS.ExistSemiClass(CameraPoints[2].name, "sc_Black")) 
                        if (!flash) {
                            NMS.Flash(
                                new []{Color.clear, Color.black},
                                new []{1f, 1f}
                            );
                            flash = true;
                        }

                } else {
                    CurrentScene[0] = CurrentScene[1];
                    CurrentScene[1] = "";
                    flash = false;

                    // Here's the code that sets up scene
                    foreach(Transform setScene in this.transform){
                        if(setScene.name == CurrentScene[0]) {
                            setScene.gameObject.SetActive(true);
                            selectedScene = setScene.gameObject;
                            CameraPoints = new Transform[]{
                                selectedScene.transform.GetChild(0).GetChild(0),
                                selectedScene.transform.GetChild(0).GetChild(1),
                                selectedScene.transform.GetChild(0).GetChild(2)
                            };
                        } else if (setScene.name != "MusicBox")
                            setScene.gameObject.SetActive(false);
                    }

                    switch(CurrentScene[0]){
                        case "Main":
                            GS.ContSaturTempInvi = new float[]{0f,0f,0f,0.25f};
                            cursorShift = 0f;
                            cursorRotation = 2f;
                            GS.PPColor = Color.white;
                            string[] types = new string[]{"Plains", "Forest", "Village", "Snow", "Sand", "Sea"};
                            string MainSubType = "Snow";//types[(int)Random.Range(0f, 5.9f)];

                            float Daytime = Random.Range(-1f, 1f);//Mathf.Clamp(Random.Range(-1f, 2f), -1f, 1f);
                            if(MainSubType == "Sand") {
                                cam.backgroundColor = RenderSettings.ambientLight = new Color32(100, 100, 100, 255);
                                Daytime = 0f;
                            } else if(Daytime > 0f){
                                cam.backgroundColor = Color.Lerp(new Color32(0, 125, 255, 255), new Color32(155, 200, 255, 255), Daytime);
                                RenderSettings.ambientLight = Color.Lerp(Color.black, new Color32(55,55,75,255), Daytime);
                            } else {
                                cam.backgroundColor = Color.Lerp(new Color32(0, 0, 100, 255), new Color32(75, 75, 100, 255), Mathf.Abs(Daytime));
                                RenderSettings.ambientLight = Color.black;
                            }

                            foreach(Transform subChild in selectedScene.transform){
                                switch(subChild.name){
                                    case "Skybox":
                                        if(MainSubType == "Sand"){
                                            subChild.GetComponent<MeshRenderer>().material.mainTexture = Skyboxes[0];
                                            subChild.GetComponent<MeshRenderer>().material.color = new Color32(230, 200, 130,255);
                                        } else {
                                            subChild.GetComponent<MeshRenderer>().material.mainTexture = Skyboxes[1];
                                            subChild.GetComponent<MeshRenderer>().material.color = cam.backgroundColor;
                                        }
                                        break;
                                    case "Sun":
                                        if(MainSubType == "Sand") subChild.GetComponent<Light>().color = Color.black;
                                        else if(Daytime > 0f) subChild.GetComponent<Light>().color = Color.Lerp(new Color32(100,75,55, 255), new Color32(255, 255, 255, 255), Daytime);
                                        else subChild.GetComponent<Light>().color = Color.Lerp(new Color32(0,0,255, 255), new Color32(100, 100, 200, 255), Mathf.Abs(Daytime));
                                        break;
                                    case "Fog":
                                        if(MainSubType == "Sand") subChild.GetComponent<SpriteRenderer>().color = new Color32(230, 200, 130,255);
                                        else if(Daytime > 0f) subChild.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color32(255,175,255, 255), new Color32(225, 225, 255, 255), Daytime);
                                        else subChild.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color32(55,55,100, 255), new Color32(55, 100, 155, 255), Mathf.Abs(Daytime));
                                        
                                        RenderSettings.fogColor = subChild.GetComponent<SpriteRenderer>().color;
                                        fogDistance = 100f;
                                        break;
                                    case "MainLand":
                                        Color32[] MainGrass = new Color32[]{new Color32(160, 180, 150, 255), new Color32(190, 230, 130,255)};
                                        switch(MainSubType){
                                            case "Plains": case "Forest":
                                                MainGrass = new Color32[]{new Color32(100, 155, 125, 255), new Color32(100, 200, 155,255)};
                                                break;
                                            case "Sand": case "Sea":
                                                MainGrass = new Color32[]{new Color32(230, 190, 120, 255), new Color32(230, 200, 130,255)};
                                                break;
                                            case "Snow":
                                                MainGrass = new Color32[]{new Color32(225, 255, 255, 255), new Color32(255, 255, 255,255)};
                                                break;
                                        }
                                        foreach(Transform setSubType in subChild) foreach (Material getmat in setSubType.GetComponent<MeshRenderer>().materials)
                                            getmat.color = Color.Lerp(MainGrass[0], MainGrass[1], Random.Range(0f, 1f));
                                        break;
                                    case "SubTypes":
                                        foreach(Transform setSubType in subChild) 
                                            if (setSubType.name == MainSubType) setSubType.gameObject.SetActive(true);
                                            else setSubType.gameObject.SetActive(false);
                                        break;
                                    case "Water":
                                        subChild.GetComponent<MeshRenderer>().material.color = cam.backgroundColor;
                                        break;
                                    case "Campfire":
                                        if(Daytime > 0.5f) Destroy(subChild.gameObject);
                                        break;
                                }
                            }
                            break;
                        case "GameOver":
                            GS.ContSaturTempInvi = new float[]{0f,-100f,0f,0.75f};
                            RenderSettings.fogColor = Color.white;
                            GS.PPColor = Color.white;
                            fogDistance = 25f;
                            RenderSettings.ambientLight = Color.white / 10f;
                            break;
                        default: 
                            break;
                    }
                }
            }

        }
        
    }

    public void ChangeScene(string newScene){
        if (CurrentScene[1] == newScene)
            return;

        flash = false;
        CurrentScene[1] = newScene;

        if(CurrentScene[0] != "")
            TimeSincePassed = 1f;
        else 
            TimeSincePassed = 0f;
    }

    public void ChangeMusic (string newMusic) {
        foreach (AudioSource music in Musics)
            if (music.name == newMusic && !music.isPlaying)
                music.Play();
            else if (music.name != newMusic && music.isPlaying)
                music.Stop();
    }

}
