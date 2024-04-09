using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    // Main Variables
    public string CurrentWindow = "";
    // Main Variables

    // References
    public GameObject GS;
    public AudioSource TitleMusic;
    public AudioSource MenuMusic;
    public AudioSource GameOverMusic;
    public Transform ShowWindow;
    public Transform HideWindow;
    // Loading
    public float LoadingTime = 0f;
    public GameObject WhileLoadingWindow;
    public Text LoadingText;
    public Text LoadingTip;
    // Loading
    // BootUp
    public GameObject WhileBootUpWindow;
    public GameObject BootUpLogo;
    public Text BootUpContinue;
    // BootUp
    // MainMenu
    public GameObject WhileMainMenuWindow;
    public Text GameVersion;
    public GameObject MMPlayButton;
    public GameObject MMHowToPlayButton;
    public GameObject MMOptionsButton;
    public GameObject MMCreditButton;
    public GameObject MMExitButton;
    public GameObject LanguageButton;
    public GameObject FullscreenButton;
    // MainMenu
    // Options
    public GameObject WhileOptionsWindow;
    public GameObject OptionsBackButton;
    public Text OptionsText;
    public Text OMouseSensitivity;
    public Text OMouseInverted;
    public Text OCameraBobbing;
    public Text OFov;
    public Text OMasterAudio;
    public Text OSoundAudio;
    public Text OMusicAudio;
    public Text OGraphics;
    public Text OSightsColor;
    // Options
    // Credits
    public GameObject WhileCreditsWindows;
    public GameObject CreditsBackButton;
    public Text CreditsText;
    // Credits
    // BeforePlay
    public GameObject WhileBeforePlayWindow;
    public GameObject HighscoreForMode;
    int WhichModeForHighscore = 1;
    public GameObject HighscoreText;
    public GameObject WBPBackButton;
    public GameObject[] WBPSaveFiles;
    public GameObject NewGameOptions;
    int NewSave = -1;
    public Text BOOptionsText;
    public Text BORoundType;
    public Text BODifficultyLevel;
    public Text BOOptionalSetting1;
    int RoundType = 1;
    int DifficultyLevel = 1;
    int OptionalRoundSetting1 = 0;
    public Text BOBegin;
    // BeforePlay
    // HowToPlay
    public GameObject WhileHowToPlayWindow;
    public string HTPPage = "";
    public Text HTPTitle;
    public Text HTPText;
    public GameObject HTPGameplayButton;
    public GameObject HTPSurvivingButton;
    public GameObject HTPHordeButton;
    public GameObject HTPScoresButton;
    public GameObject HTPControlsButton;
    public GameObject HTPBackButton;
    // HowToPlay
    // GameOver
    public GameObject GameOverWindow;
    public Image YourDeadImage;
    public Image GameOverBG;
    public Text GameOverText;
    // GameOver
    // Message
    public string MessageContent = "";
    public GameObject MessageWindow;
    public Text MessageText;
    public GameObject[] MessageButtons;
    // Message
    public Image StartFlash;
    // References

    // Title Screens
    public string CurrentTitleScreen = "";
    public GameObject[] TitleScreens;
    // Title Screens

    // Camera References
    public GameObject MainCamera;
    // References

    // Misc
    int[] GotGOVariables = new int[] { 0, 0, 0 };
    bool GotHighScore = false;
    bool GotHighRounds = false;
    float SwitchSceneA = 4f;
    string SceneToSwitch = "";
    // Misc

    // Use this for initialization
    void Start () {

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LoadingTime = Random.Range(1f, 4f);
        GS = GameObject.Find("_GameScript");
        int PickHint = Random.Range(1, 10);
        if (PickHint == 1) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "When nuke drops, you still have a short moment to escape, before the firewall hits you!",
                "Gdy bomba uderza, masz jeszcze krótki czas w którym to możesz uciec, zanim dopadnie cię ściana ognia!");
        } else if (PickHint == 2) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "With each round, supplies will get scarce, maps will get more desolate, and there will be more dangerous mobs!",
                "Z każdą rundą, zasoby będą maleć, mapy będą bardziej puste, a moby będą bardziej niebezpieczne!");
        } else if (PickHint == 3) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "Don't rush the exits, search your surroundings when you have the time!",
                "Nie pędź odrazu do wyjścia, przeszukaj swoje tereny gdy masz na to czas!");
        } else if (PickHint == 4) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "Guns are rare, use melee weapons when fighting weak enemies!",
                "Bronie są rzadkie, używaj broni białych gdy przeciwnicy są słabi!");
        } else if (PickHint == 5) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "Watch out for the radioactivity (use your map to check radioactive terrains)",
                "Uważaj na radioaktywność (na mapie masz zaznaczone radioaktywne tereny)");
        } else if (PickHint == 6) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "Mutants and bandits will get more advanced the futher you go!",
                "Mutańci i bandyci będą stawali się bardziej niebezpieczni im dalej pójdziesz!");
        } else if (PickHint == 7) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "On some maps, there may be monuments. Specific, heavily guarded constructions, with alien-like treasures!",
                "Na niektórych mapach znajdują się monumenty. Specyficzne, dobrze chronione konstrukcje, z przedziwacznymi skarbami!");
        } else if (PickHint == 8) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "On your way, you may find other survivors. You can trade, ask them questions, or kill them!",
                "Na swojej drodze, możesz spotkać innych przetrwańców. Możesz z nimi handlować, zadawać im pytania, lub ich po prostu zabić!");
        } else if (PickHint == 9) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "You should find your escape route first!",
                "Na początku lepiej znaleść sobie trasę do ucieczki!");
        } else if (PickHint == 10) {
            GS.GetComponent<GameScript>().SetText(LoadingTip,
                "Biomes differ in supplies and mobs, keep that in mind before leaving the map!",
                "Biomy różnią się zasobami oraz mobami, pamiętaj o tym, gdy będzisz opuszczał mapę!");
        }

        // Set Grass color

    }
	
	// Update is called once per frame
	void Update () {

        if (GS == null) {
            GS = GameObject.Find("_GameScript");
        } else {
            // TitleScreens
            if (LoadingTime <= 0f && CurrentWindow != "Loading") {
                if (CurrentWindow == "GameOver") {
                    if (CurrentTitleScreen != "GameOver") {
                        CurrentTitleScreen = "GameOver";
                        MainCamera.GetComponent<Camera>().fieldOfView = 60f;
                        GS.GetComponent<GameScript>().ContSaturTempInvi = new float[] { 0f, -100f, 0f, 0.5f };
                        StartFlash.color = new Color32(0, 0, 0, 0);
                    }
                } else if (GS.GetComponent<GameScript>().WindowToBootUp == "BootUp") {
                    if (CurrentTitleScreen != "BootUp") {
                        CurrentTitleScreen = "BootUp";
                        MainCamera.GetComponent<Camera>().fieldOfView = 0f;
                        GS.GetComponent<GameScript>().ContSaturTempInvi = new float[] { 0f, 0f, 0f, 0.25f };
                        StartFlash.color = new Color32(255, 255, 255, 255);
                    }
                } else if (GS.GetComponent<GameScript>().WindowToBootUp != "BootUp") {
                    if (CurrentTitleScreen.Substring(0, 1) != "N") {
                        CurrentTitleScreen = "Normal" + (int)Random.Range(1f, 7.9f);
                        MainCamera.GetComponent<Camera>().fieldOfView = 60f;
                        GS.GetComponent<GameScript>().ContSaturTempInvi = new float[] { 0f, 0f, 0f, 0.25f };
                        StartFlash.color = new Color32(0, 0, 0, 255);
                    }
                }
            }

            if (CurrentTitleScreen != "" && LoadingTime <= 0f) {
                foreach (GameObject SetScreen in TitleScreens) {
                    if (SetScreen.name == CurrentTitleScreen) {
                        SetScreen.SetActive(true);
                        MainCamera.transform.position = SetScreen.transform.GetChild(0).position;
                        MainCamera.transform.rotation = SetScreen.transform.GetChild(0).rotation;
                        MainCamera.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(MainCamera.GetComponent<Camera>().fieldOfView, float.Parse(SetScreen.transform.GetChild(0).name), 0.5f * (Time.deltaTime * 100f));
                        Color32[] SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        if (CurrentTitleScreen == "BootUp") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(175, 125, 0, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 1000f;
                            RenderSettings.fogEndDistance = 1000f;
                            RenderSettings.ambientLight = new Color32(100, 100, 100, 255);
                            SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        } else if (CurrentTitleScreen == "GameOver") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(55, 65, 105, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 1000f;
                            RenderSettings.fogEndDistance = 1000f;
                            RenderSettings.ambientLight = new Color32(55, 55, 55, 255);
                            SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        } else if (CurrentTitleScreen == "Normal1" || CurrentTitleScreen == "Normal7") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(255, 200, 155, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 250f;
                            RenderSettings.fogEndDistance = 250f;
                            RenderSettings.ambientLight = new Color32(25, 0, 55, 255);
                            SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        } else if (CurrentTitleScreen == "Normal2" || CurrentTitleScreen == "Normal8") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(155, 180, 212, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 1000f;
                            RenderSettings.fogEndDistance = 1000f;
                            RenderSettings.ambientLight = new Color32(55, 55, 100, 255);
                            SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        } else if (CurrentTitleScreen == "Normal3") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(67, 78, 94, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 50f;
                            RenderSettings.fogEndDistance = 50f;
                            RenderSettings.ambientLight = new Color32(55, 55, 55, 255);
                            SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        } else if (CurrentTitleScreen == "Normal4") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(77, 84, 89, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 50f;
                            RenderSettings.fogEndDistance = 50f;
                            RenderSettings.ambientLight = new Color32(55, 55, 55, 255);
                            SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        } else if (CurrentTitleScreen == "Normal5") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(155, 55, 75, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 50f;
                            RenderSettings.fogEndDistance = 50f;
                            RenderSettings.ambientLight = new Color32(25, 0, 55, 255);
                            SetColor = new Color32[] { new Color32(154, 188, 134, 255), new Color32(121, 178, 103, 255) };
                        } else if (CurrentTitleScreen == "Normal6") {
                            MainCamera.GetComponent<Camera>().backgroundColor = new Color32(155, 180, 212, 255);
                            RenderSettings.fogColor = MainCamera.GetComponent<Camera>().backgroundColor;
                            MainCamera.GetComponent<Camera>().farClipPlane = 50f;
                            RenderSettings.fogEndDistance = 25f;
                            RenderSettings.ambientLight = new Color32(55, 55, 100, 255);
                            SetColor = new Color32[] { new Color32(200, 225, 255, 255), new Color32(255, 255, 255, 255) };
                        }
                        if (QualitySettings.GetQualityLevel() == 0) {
                            MainCamera.GetComponent<Camera>().farClipPlane = 50f;
                            RenderSettings.fogEndDistance = 50f;
                        }
                        foreach (Transform GetObject in SetScreen.transform) {
                            if (GetObject.GetComponent<MeshRenderer>() != null) {
                                foreach (Material SetMat in GetObject.GetComponent<MeshRenderer>().materials) {
                                    Color WallColor = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
                                    Color TreeColor = Color.Lerp(new Color(1f, 0.75f, 0f, 1f), new Color(0.3f, 0.5f, 0.3f, 1f), Random.Range(0f, 1f));
                                    if (SetMat.name == "Grass1 (Instance)" || SetMat.name == "Grass2 (Instance)" || SetMat.name == "Grass3 (Instance)") {
                                        SetMat.color = Color32.Lerp(SetColor[0], SetColor[1], Random.Range(0f, 1f));
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "Flower1 (Instance)" || SetMat.name == "Flower2 (Instance)" || SetMat.name == "Flower3 (Instance)") {
                                        SetMat.color = new Color32((byte)Random.Range(100f, 255f), (byte)Random.Range(100f, 255f), (byte)Random.Range(100f, 255f), 255);
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "HouseOuter1 (Instance)") {
                                        SetMat.color = WallColor;
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "HouseOuter2 (Instance)") {
                                        SetMat.color = WallColor / 2f;
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "HouseInner (Instance)") {
                                        SetMat.color = new Color32((byte)Random.Range(75f, 255f), (byte)Random.Range(75f, 255f), (byte)Random.Range(75f, 255f), 255);
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "HouseRoof (Instance)") {
                                        SetMat.color = Color32.Lerp(new Color32(100, 75, 55, 255), new Color32(255, 225, 155, 255), Random.Range(0f, 1f));
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "WoodenFence1 (Instance)") {
                                        SetMat.color = Color32.Lerp(new Color32(100, 75, 55, 255), new Color32(188, 155, 133, 255), Random.Range(0f, 1f));
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "Tree1 (Instance)" || SetMat.name == "Tree2 (Instance)" || SetMat.name == "Tree3 (Instance)") {
                                        SetMat.color = TreeColor * Random.Range(0.5f, 1f);
                                        SetMat.name += " SET";
                                    } else if (SetMat.name == "Water (Instance)") {
                                        SetMat.color = MainCamera.GetComponent<Camera>().backgroundColor;
                                        SetMat.name += " SET";
                                    }
                                }
                            }
                        }
                    } else {
                        SetScreen.SetActive(false);
                    }
                }
            }


            // TitleScreens

            if (LoadingTime <= 0f) {
                StartFlash.color -= new Color(0f, 0f, 0f, 0.005f * (Time.deltaTime * 100f));
            }

            if (SceneToSwitch != "") {
                if (SwitchSceneA > 0f) {
                    SwitchSceneA -= 0.1f;
                    LoadingTime = 10f;
                } else {
                    GS.GetComponent<GameScript>().ChangeLevel(SceneToSwitch);
                }
            }

            if (LoadingTime > 0f) {
                LoadingTime -= 0.01f;
                CurrentWindow = "Loading";
            } else if (CurrentWindow == "Loading") {
                CurrentWindow = GS.GetComponent<GameScript>().WindowToBootUp;
                if (GS.GetComponent<GameScript>().WindowToBootUp == "GameOver") {
                    GameOverMusic.Play();
                } else {
                    if (GS.GetComponent<GameScript>().WindowToBootUp != "BootUp") {
                        MenuMusic.Play();
                        int ConsiderChance = Random.Range(0, 5);
                        if (ConsiderChance == 0) {
                            MessageContent = "Consideration";
                        } else if (ConsiderChance == 1) {
                            MessageContent = "Feedback";
                        }
                    } else {
                        TitleMusic.Play();
                    }
                }
            }

            if (MessageContent != "") {
                WhileMessage(true);
            } else {
                WhileMessage(false);
            }

            if(CurrentWindow == "Loading"){
                WhileLoading(true);
                WhileBootUp(false);
                WhileMainMenu(false);
                WhileOptions(false);
                WhileCredits(false);
                WhileBeforePlay(false);
                WhileHowToPlay(false);
                WhileGameOver(false);
            } else if (CurrentWindow == "BootUp") {
                WhileLoading(false);
                WhileBootUp(true);
                WhileMainMenu(false);
                WhileOptions(false);
                WhileCredits(false);
                WhileBeforePlay(false);
                WhileHowToPlay(false);
                WhileGameOver(false);
            } else if (CurrentWindow == "MainMenu") {
                WhileLoading(false);
                WhileBootUp(false);
                WhileMainMenu(true);
                WhileOptions(false);
                WhileCredits(false);
                WhileBeforePlay(false);
                WhileHowToPlay(false);
                WhileGameOver(false);
            } else if (CurrentWindow == "Options") {
                WhileLoading(false);
                WhileBootUp(false);
                WhileMainMenu(false);
                WhileOptions(true);
                WhileCredits(false);
                WhileBeforePlay(false);
                WhileHowToPlay(false);
                WhileGameOver(false);
            } else if (CurrentWindow == "Credits") {
                WhileLoading(false);
                WhileBootUp(false);
                WhileMainMenu(false);
                WhileOptions(false);
                WhileCredits(true);
                WhileBeforePlay(false);
                WhileHowToPlay(false);
                WhileGameOver(false);
            } else if (CurrentWindow == "BeforePlay") {
                WhileLoading(false);
                WhileBootUp(false);
                WhileMainMenu(false);
                WhileOptions(false);
                WhileCredits(false);
                WhileBeforePlay(true);
                WhileHowToPlay(false);
                WhileGameOver(false);
            } else if (CurrentWindow == "HowToPlay") {
                WhileLoading(false);
                WhileBootUp(false);
                WhileMainMenu(false);
                WhileOptions(false);
                WhileCredits(false);
                WhileBeforePlay(false);
                WhileHowToPlay(true);
                WhileGameOver(false);
            } else if (CurrentWindow == "GameOver") {
                WhileLoading(false);
                WhileBootUp(false);
                WhileMainMenu(false);
                WhileOptions(false);
                WhileCredits(false);
                WhileBeforePlay(false);
                WhileHowToPlay(false);
                WhileGameOver(true);
            } else {
                WhileLoading(false);
                WhileBootUp(false);
                WhileMainMenu(false);
                WhileOptions(false);
                WhileCredits(false);
                WhileBeforePlay(false);
                WhileHowToPlay(false);
                WhileGameOver(false);
            }

        }
		
	}

    void WhileLoading(bool Shown) {
        if (Shown == false) {
            WhileLoadingWindow.transform.position = HideWindow.position;
            int PickHint = Random.Range(1, 10);
            if (PickHint == 1) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "When nuke drops, you still have a short moment to escape, before the firewall hits you!",
                    "Gdy bomba uderza, masz jeszcze krótki czas w którym to możesz uciec, zanim dopadnie cię ściana ognia!");
            } else if (PickHint == 2) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "With each round, supplies will get scarce, maps will get more desolate, and there will be more dangerous mobs!",
                    "Z każdą rundą, zasoby będą maleć, mapy będą bardziej puste, a moby będą bardziej niebezpieczne!");
            } else if (PickHint == 3) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "Don't rush the exits, search your surroundings when you have the time!",
                    "Nie pędź odrazu do wyjścia, przeszukaj swoje tereny gdy masz na to czas!");
            } else if (PickHint == 4) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "Guns are rare, use melee weapons when fighting weak enemies!",
                    "Bronie są rzadkie, używaj broni białych gdy przeciwnicy są słabi!");
            } else if (PickHint == 5) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "Watch out for the radioactivity (use your map to check radioactive terrains)",
                    "Uważaj na radioaktywność (na mapie masz zaznaczone radioaktywne tereny)");
            } else if (PickHint == 6) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "Mutants and bandits will get more advanced the futher you go!",
                    "Mutańci i bandyci będą stawali się bardziej niebezpieczni im dalej pójdziesz!");
            } else if (PickHint == 7) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "On some maps, there may be monuments. Specific, heavily guarded constructions, with alien-like treasures!",
                    "Na niektórych mapach znajdują się monumenty. Specyficzne, dobrze chronione konstrukcje, z przedziwacznymi skarbami!");
            } else if (PickHint == 8) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "On your way, you may find other survivors. You can trade, ask them questions, or kill them!",
                    "Na swojej drodze, możesz spotkać innych przetrwańców. Możesz z nimi handlować, zadawać im pytania, lub ich po prostu zabić!");
            } else if (PickHint == 9) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "You should find your escape route first!",
                    "Na początku lepiej znaleść sobie trasę do ucieczki!");
            } else if (PickHint == 10) {
                GS.GetComponent<GameScript>().SetText(LoadingTip,
                    "Biomes differ in supplies and mobs, keep that in mind before leaving the map!",
                    "Biomy różnią się zasobami oraz mobami, pamiętaj o tym, gdy będzisz opuszczał mapę!");
            }
        } else {
            GS.GetComponent<GameScript>().SetText(LoadingText, "LOADING", "WCZYTYWANIE");
            WhileLoadingWindow.transform.position = ShowWindow.position;
        }
    }

    void WhileBootUp(bool Shown) {
        if (Shown == false) {
            WhileBootUpWindow.transform.position = HideWindow.position;
            BootUpLogo.transform.localScale = Vector3.zero;
            BootUpContinue.text = "";
            BootUpContinue.color = new Color32(255, 255, 255, 0);
        } else {
            WhileBootUpWindow.transform.position = ShowWindow.position;
            BootUpLogo.transform.localScale = Vector3.MoveTowards(BootUpLogo.transform.localScale, Vector3.one, 0.025f * (Time.deltaTime * 100f));
            if (BootUpLogo.transform.localScale.x  > 0.9f && BootUpContinue.GetComponent<Text>().color.a < 1f) {
                BootUpContinue.color += new Color(0f, 0f, 0f, 0.01f * (Time.deltaTime * 100f));
            }
            GS.GetComponent<GameScript>().SetText(BootUpContinue, "Press any key to continue", "Naciśnij dowolny klawisz by kontynuować");
            if (Input.anyKeyDown) {
                CurrentWindow = "MainMenu";
            }
        }
    }

    void WhileMainMenu(bool Shown){

        if (Shown == false) {

            WhileMainMenuWindow.transform.position = Vector3.Lerp(WhileMainMenuWindow.transform.position, HideWindow.position, 0.1f * (Time.deltaTime * 100f));

        } else {

            WhileMainMenuWindow.transform.position = Vector3.Lerp(WhileMainMenuWindow.transform.position, ShowWindow.position, 0.1f * (Time.deltaTime * 100f));
            if (GS.GetComponent<GameScript>().Platform == 1) {
                GameVersion.text = GS.GetComponent<GameScript>().Version + " Desktop";
                MMExitButton.SetActive(true);
            } else if (GS.GetComponent<GameScript>().Platform == 2) {
                GameVersion.text = GS.GetComponent<GameScript>().Version + " WebGL";
                MMExitButton.SetActive(false);
            }

            // Buttons
            GS.GetComponent<GameScript>().SetText(MMPlayButton.transform.GetChild(0).GetComponent<Text>(), "Play", "Graj");
            GS.GetComponent<GameScript>().SetText(MMHowToPlayButton.transform.GetChild(0).GetComponent<Text>(), "How to Play", "Jak Grać");
            GS.GetComponent<GameScript>().SetText(MMOptionsButton.transform.GetChild(0).GetComponent<Text>(), "Options", "Opcje");
            GS.GetComponent<GameScript>().SetText(MMCreditButton.transform.GetChild(0).GetComponent<Text>(), "Credits", "Lista Płac");
            GS.GetComponent<GameScript>().SetText(MMExitButton.transform.GetChild(0).GetComponent<Text>(), "Exit", "Wyjdź");
            if (MMPlayButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "BeforePlay";
            } else if (MMHowToPlayButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "HowToPlay";
            } else if (MMOptionsButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "Options";
            } else if (MMCreditButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "Credits";
            } else if (MMExitButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                MessageContent = "Exit";
            }

            // Language
            foreach (Transform Buttonlanguages in LanguageButton.transform) {
                if (Buttonlanguages.name == GS.GetComponent<GameScript>().Language) {
                    Buttonlanguages.gameObject.SetActive(true);
                } else {
                    Buttonlanguages.gameObject.SetActive(false);
                }
            }
            if (LanguageButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                if (GS.GetComponent<GameScript>().Language == "English") {
                    GS.GetComponent<GameScript>().Language = "Polski";
                } else if (GS.GetComponent<GameScript>().Language == "Polski") {
                    GS.GetComponent<GameScript>().Language = "English";
                }
            }
            // Language
            // Fullscreen
            if (GS.GetComponent<GameScript>().Platform == 2) {
                if (Screen.fullScreen == false) {
                    FullscreenButton.transform.GetChild(0).gameObject.SetActive(true);
                    FullscreenButton.transform.GetChild(1).gameObject.SetActive(false);
                } else {
                    FullscreenButton.transform.GetChild(0).gameObject.SetActive(false);
                    FullscreenButton.transform.GetChild(1).gameObject.SetActive(true);
                }
                if (FullscreenButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    if (Screen.fullScreen == false) {
                        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    } else {
                        Screen.fullScreenMode = FullScreenMode.Windowed;
                    }
                }
            } else {
                FullscreenButton.SetActive(false);
            }
            // Fullscreen
            // Buttons

        }

    }

    void WhileOptions(bool Shown) {

        if (Shown == false) {

            WhileOptionsWindow.transform.position = Vector3.Lerp(WhileOptionsWindow.transform.position, HideWindow.position, 0.1f * (Time.deltaTime * 100f));

        } else {

            WhileOptionsWindow.transform.position = Vector3.Lerp(WhileOptionsWindow.transform.position, ShowWindow.position, 0.1f * (Time.deltaTime * 100f));
            GS.GetComponent<GameScript>().SetText(OptionsBackButton.transform.GetChild(0).GetComponent<Text>(), "Back to Menu", "Wróć do Menu");
            if (OptionsBackButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "MainMenu";
                GS.GetComponent<GameScript>().SaveSettingsTime = 0f;
            }

            GS.GetComponent<GameScript>().SetText(OptionsText, "~ Options ~", "~ Opcje ~");
            // Mouse Sensitivity
            GS.GetComponent<GameScript>().SetText(OMouseSensitivity, "Mouse sensitivity: " + (int)(GS.GetComponent<GameScript>().MouseSensitivity * 10f), "Czułość myszki: " + (int)(GS.GetComponent<GameScript>().MouseSensitivity * 10f));
            if (OMouseSensitivity.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                GS.GetComponent<GameScript>().MouseSensitivity += 0.2f;
                if (GS.GetComponent<GameScript>().MouseSensitivity > 3.1f) {
                    GS.GetComponent<GameScript>().MouseSensitivity = 0.2f;
                }
            }

            // Inverted Mouse;
            GS.GetComponent<GameScript>().SetText(OMouseInverted, "Inverted mouse: " + GS.GetComponent<GameScript>().InvertedMouse, "Odwrócona oś myszki: " + GS.GetComponent<GameScript>().InvertedMouse);
            if (OMouseInverted.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                if (GS.GetComponent<GameScript>().InvertedMouse == true) {
                    GS.GetComponent<GameScript>().InvertedMouse = false;
                } else {
                    GS.GetComponent<GameScript>().InvertedMouse = true;
                }
            }

            // Camera Bobbing;
            GS.GetComponent<GameScript>().SetText(OCameraBobbing, "Camera bobbing: " + GS.GetComponent<GameScript>().CameraBobbing, "Kołysanie się kamery: " + GS.GetComponent<GameScript>().CameraBobbing);
            if (OCameraBobbing.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                if (GS.GetComponent<GameScript>().CameraBobbing == 1f) {
                    GS.GetComponent<GameScript>().CameraBobbing = 0f;
                } else {
                    GS.GetComponent<GameScript>().CameraBobbing += 0.1f;
                }
            }

            // Field of View
            GS.GetComponent<GameScript>().SetText(OFov, "Field of view: " + (int)(GS.GetComponent<GameScript>().FOV), "Pole widzenia: " + (int)(GS.GetComponent<GameScript>().FOV));
            if (OFov.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                GS.GetComponent<GameScript>().FOV += 2f;
                if (GS.GetComponent<GameScript>().FOV > 101f) {
                    GS.GetComponent<GameScript>().FOV = 60f;
                }
            }

            // Master Audio
            /*GS.GetComponent<GameScript>().SetText(OMasterAudio, "Master volume: " + (int)(GS.GetComponent<GameScript>().MasterVolume * 100f), "Ogólna głośność: " + (int)(GS.GetComponent<GameScript>().MasterVolume * 100f));
            if (OMasterAudio.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                GS.GetComponent<GameScript>().MasterVolume += 0.1f;
                if (GS.GetComponent<GameScript>().MasterVolume > 1.05f) {
                    GS.GetComponent<GameScript>().MasterVolume = 0f;
                }
            }
            // Sound Audio
            GS.GetComponent<GameScript>().SetText(OSoundAudio, "Sound volume: " + (int)(GS.GetComponent<GameScript>().SoundVolume * 100f), "Głośność dźwięków: " + (int)(GS.GetComponent<GameScript>().SoundVolume * 100f));
            if (OSoundAudio.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                GS.GetComponent<GameScript>().SoundVolume += 0.1f;
                if (GS.GetComponent<GameScript>().SoundVolume > 1.05f) {
                    GS.GetComponent<GameScript>().SoundVolume = 0f;
                }
            }
            // Music Audio
            GS.GetComponent<GameScript>().SetText(OMusicAudio, "Music volume: " + (int)(GS.GetComponent<GameScript>().MusicVolume * 100f), "Głośność muzyki: " + (int)(GS.GetComponent<GameScript>().MusicVolume * 100f));
            if (OMusicAudio.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                GS.GetComponent<GameScript>().MusicVolume += 0.1f;
                if (GS.GetComponent<GameScript>().MusicVolume > 1.05f) {
                    GS.GetComponent<GameScript>().MusicVolume = 0f;
                }
            }*/

            // Graphics settings
            string Graphicssettings = "";
            if (QualitySettings.GetQualityLevel() == 0) {
                Graphicssettings = GS.GetComponent<GameScript>().SetString("Minimal", "Minimalna");
            } else if (QualitySettings.GetQualityLevel() == 1) {
                Graphicssettings = GS.GetComponent<GameScript>().SetString("Low", "Niska");
            } else if (QualitySettings.GetQualityLevel() == 2) {
                Graphicssettings = GS.GetComponent<GameScript>().SetString("Medium", "Średnia");
            } else if (QualitySettings.GetQualityLevel() == 3) {
                Graphicssettings = GS.GetComponent<GameScript>().SetString("Good", "Dobra");
            } else if (QualitySettings.GetQualityLevel() == 4) {
                Graphicssettings = GS.GetComponent<GameScript>().SetString("High", "Wysoka");
            }
            GS.GetComponent<GameScript>().SetText(OGraphics, "Graphics: " + Graphicssettings, "Grafika: " + Graphicssettings);
            if (OGraphics.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                if (QualitySettings.GetQualityLevel() == 4) {
                    QualitySettings.SetQualityLevel(0);
                } else {
                    QualitySettings.IncreaseLevel();
                }
            }

            // Sight's color
            string SightsColors = "";
            if (GS.GetComponent<GameScript>().LaserColor == 0) {
                SightsColors = GS.GetComponent<GameScript>().SetString("Red", "Czerwony");
            } else if (GS.GetComponent<GameScript>().LaserColor == 1) {
                SightsColors = GS.GetComponent<GameScript>().SetString("Green", "Zielony");
            } else if (GS.GetComponent<GameScript>().LaserColor == 2) {
                SightsColors = GS.GetComponent<GameScript>().SetString("Blue", "Niebieski");
            } else if (GS.GetComponent<GameScript>().LaserColor == 3) {
                SightsColors = GS.GetComponent<GameScript>().SetString("Yellow", "Żółty");
            } else if (GS.GetComponent<GameScript>().LaserColor == 4) {
                SightsColors = GS.GetComponent<GameScript>().SetString("Magenta", "Magenta");
            } else if (GS.GetComponent<GameScript>().LaserColor == 5) {
                SightsColors = GS.GetComponent<GameScript>().SetString("Cyan", "Cyjan");
            } else if (GS.GetComponent<GameScript>().LaserColor == 6) {
                SightsColors = GS.GetComponent<GameScript>().SetString("White", "Biały");
            }
            GS.GetComponent<GameScript>().SetText(OSightsColor, "Sight's color: " + SightsColors, "Kolor celowników: " + SightsColors);
            if (OSightsColor.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                if (GS.GetComponent<GameScript>().LaserColor == 6) {
                    GS.GetComponent<GameScript>().LaserColor = 0;
                } else {
                    GS.GetComponent<GameScript>().LaserColor += 1;
                }
            }

        }

    }

    void WhileCredits(bool Shown){

        if (Shown == false) {
            WhileCreditsWindows.transform.position = Vector3.Lerp(WhileCreditsWindows.transform.position, HideWindow.position, 0.1f * (Time.deltaTime * 100f));
        } else {
            WhileCreditsWindows.transform.position = Vector3.Lerp(WhileCreditsWindows.transform.position, ShowWindow.position, 0.1f * (Time.deltaTime * 100f));
            GS.GetComponent<GameScript>().SetText(CreditsBackButton.transform.GetChild(0).GetComponent<Text>(), "Back to Menu", "Wróć do Menu");
            if (CreditsBackButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "MainMenu";
            }
            GS.GetComponent<GameScript>().SetText(CreditsText, 
                "~ Credits ~" +
                "\n\n\nGame created by:\n-GMPguy" +
                "\n\nTools used:\n-Unity\n-Gimp\n-Blender\n-Audacity" +
                "\n\nMusic:\n'Bossa Antigua' 'Chase Pulse' 'Corruption' 'Enter the Maze' 'Lightless Dawn' 'Pale Rider' 'Reaching Out' 'The Descent' 'Windswept' 'Western Streets' 'Almost New' 'Chase' 'Exit the Premises' by Kevin MacLeod (https:/ /incompetech.com) License: CC BY(http:/ /creativecommons.org/licenses/by/4.0/)" +
                "\n\n\nMade for Game Off 2019",
                "~ Lista Płac ~" +
                "\n\n\nGra stworzona przez:\n-GMPguy" +
                "\n\nUżyte programy:\n-Unity\n-Gimp\n-Blender\n-Audacity" +
                "\n\nMuzyka:\n'Bossa Antigua' 'Chase Pulse' 'Corruption' 'Enter the Maze' 'Lightless Dawn' 'Pale Rider' 'Reaching Out' 'The Descent' 'Windswept' 'Western Streets' 'Almost New' 'Chase' 'Exit the Premises' by Kevin MacLeod (https:/ /incompetech.com) License: CC BY(http:/ /creativecommons.org/licenses/by/4.0/)" +
                "\n\n\nStworzone na Game Off 2019");
        }

    }

    void WhileBeforePlay(bool Shown){

        if (Shown == false) {

            WhileBeforePlayWindow.transform.position = Vector3.Lerp(WhileBeforePlayWindow.transform.position, HideWindow.position, 0.1f * (Time.deltaTime * 100f));
            NewSave = -1;

        } else {

            WhileBeforePlayWindow.transform.position = Vector3.Lerp(WhileBeforePlayWindow.transform.position, ShowWindow.position, 0.1f * (Time.deltaTime * 100f));
            GS.GetComponent<GameScript>().SetText(WBPBackButton.transform.GetChild(0).GetComponent<Text>(), "Back to Menu", "Wróć do Menu");
            if (WBPBackButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "MainMenu";
            }

            foreach(GameObject SaveButton in WBPSaveFiles){
                int CurrentSave = int.Parse(SaveButton.name.Substring(0, 1));
                if (PlayerPrefs.HasKey("S" + CurrentSave + "Round")) {

                    if (PlayerPrefs.GetString("S" + CurrentSave + "RoundSettings").Substring(0, 1) == "1") {
                        GS.GetComponent<GameScript>().SetText(SaveButton.transform.GetChild(0).GetComponent<Text>(), "Classic Game " + CurrentSave, "Klasyczny Tryb " + CurrentSave);
                    } else if (PlayerPrefs.GetString("S" + CurrentSave + "RoundSettings").Substring(0, 1) == "2") {
                        GS.GetComponent<GameScript>().SetText(SaveButton.transform.GetChild(0).GetComponent<Text>(), "Horde Game " + CurrentSave, "Tryb Hordy " + CurrentSave);
                    }

                    GS.GetComponent<GameScript>().SetText(SaveButton.transform.GetChild(1).GetChild(0).GetComponent<Text>(), "Play", "Graj");
                    if (SaveButton.transform.GetChild(1).GetChild(0).GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                        GS.GetComponent<GameScript>().CurrentSave = CurrentSave;
                        SceneToSwitch = "LoadGame";
                    }

                    SaveButton.transform.GetChild(2).gameObject.SetActive(true);
                    SaveButton.transform.GetChild(2).GetChild(0).GetComponent<ButtonScript>().Active = true;
                    GS.GetComponent<GameScript>().SetText(SaveButton.transform.GetChild(2).GetChild(0).GetComponent<Text>(), "Clear", "Usuń");
                    if (SaveButton.transform.GetChild(2).GetChild(0).GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                        MessageContent = CurrentSave + "ClearSaveFile";
                    }

                } else {

                    GS.GetComponent<GameScript>().SetText(SaveButton.transform.GetChild(0).GetComponent<Text>(), "Empty file", "Pusty zapis");
                    if (SaveButton.transform.GetChild(1).GetChild(0).GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                        NewSave = CurrentSave;
                    }

                    GS.GetComponent<GameScript>().SetText(SaveButton.transform.GetChild(1).GetChild(0).GetComponent<Text>(), "New Game", "Nowa Gra");

                    SaveButton.transform.GetChild(2).gameObject.SetActive(false);
                    SaveButton.transform.GetChild(2).GetChild(0).GetComponent<ButtonScript>().Active = false;

                }
            }

            if (NewSave != -1) {
                NewGameOptions.SetActive(true);
                HighscoreForMode.SetActive(false);
                HighscoreText.SetActive(false);

                GS.GetComponent<GameScript>().SetText(BOOptionsText, "~ New game ~", "~ Nowa Gra ~");

                // Round Settings
                // RoundType
                if (RoundType == 1) {
                    GS.GetComponent<GameScript>().SetText(BORoundType, "Game mode: Classic", "Tryb gry: Klasyczny");
                } else if (RoundType == 2) {
                    GS.GetComponent<GameScript>().SetText(BORoundType, "Game mode: Horde", "Tryb gry: Horda");
                }
                if (BORoundType.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    RoundType += 1;
                    if (RoundType > 2) {
                        RoundType = 1;
                    }
                }

                // Difficulty Level
                if (DifficultyLevel == 1) {
                    GS.GetComponent<GameScript>().SetText(BODifficultyLevel, "Difficulty level: Easy", "Poziom trudności: Łatwy");
                } else if (DifficultyLevel == 2) {
                    GS.GetComponent<GameScript>().SetText(BODifficultyLevel, "Difficulty level: Normal", "Poziom trudności: Normalny");
                } else if (DifficultyLevel == 3) {
                    GS.GetComponent<GameScript>().SetText(BODifficultyLevel, "Difficulty level: Hard", "Poziom trudności: Trudny");
                } else if (DifficultyLevel == 4) {
                    GS.GetComponent<GameScript>().SetText(BODifficultyLevel, "Difficulty level: Very Hard", "Poziom trudności: Bardzo Trudny");
                } else if (DifficultyLevel == 5) {
                    GS.GetComponent<GameScript>().SetText(BODifficultyLevel, "Difficulty level: Hardcore", "Poziom trudności: Hardkorowy");
                }
                if (BODifficultyLevel.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    DifficultyLevel += 1;
                    if (DifficultyLevel > 5) {
                        DifficultyLevel = 1;
                    }
                }

                // Optional Round Setting 1
                int MaxORS1 = 0;
                if (RoundType == 2) {
                    MaxORS1 = 5;
                    BOOptionalSetting1.GetComponent<ButtonScript>().Active = true;
                    if (OptionalRoundSetting1 == 0) {
                        GS.GetComponent<GameScript>().SetText(BOOptionalSetting1, "Map: Shopping Mall", "Mapa: Galeria Handlowa");
                    } else if (OptionalRoundSetting1 == 1) {
                        GS.GetComponent<GameScript>().SetText(BOOptionalSetting1, "Map: Trenches", "Mapa: Okopy");
                    } else if (OptionalRoundSetting1 == 2) {
                        GS.GetComponent<GameScript>().SetText(BOOptionalSetting1, "Map: Disco Club", "Mapa: Dyskoteka");
                    } else if (OptionalRoundSetting1 == 3) {
                        GS.GetComponent<GameScript>().SetText(BOOptionalSetting1, "Map: Sewers", "Mapa: Kanalizacja");
                    } else if (OptionalRoundSetting1 == 4) {
                        GS.GetComponent<GameScript>().SetText(BOOptionalSetting1, "Map: Roofs", "Mapa: Dachy");
                    } else if (OptionalRoundSetting1 == 5) {
                        GS.GetComponent<GameScript>().SetText(BOOptionalSetting1, "Map: Ship", "Mapa: Statek");
                    }
                } else {
                    BOOptionalSetting1.text = "";
                    BOOptionalSetting1.GetComponent<ButtonScript>().Active = false;
                    OptionalRoundSetting1 = 0;
                }
                if (BOOptionalSetting1.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    OptionalRoundSetting1 += 1;
                    if (OptionalRoundSetting1 > MaxORS1) {
                        OptionalRoundSetting1 = 0;
                    }
                }

                // Start it
                GS.GetComponent<GameScript>().SetText(BOBegin, "BEGIN", "ZACZYNAJ");
                if (BOBegin.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    GS.GetComponent<GameScript>().CurrentSave = NewSave;
                    string SetORS1 = "0" + OptionalRoundSetting1.ToString();
                    
                    //GS.GetComponent<GameScript>().RoundSetting = RoundType.ToString() + DifficultyLevel.ToString() + (SetORS1).Substring(SetORS1.Length - 2, 2);
                    
                    SceneToSwitch = "NewGame";
                }

            } else {
                NewGameOptions.SetActive(false);
                HighscoreForMode.SetActive(true);
                HighscoreText.SetActive(true);

                if (WhichModeForHighscore == 1) {
                    GS.GetComponent<GameScript>().SetText(HighscoreForMode.GetComponent<Text>(), "< Records for Classic mode >", "< Rekordy dla trybu Klasycznego >");
                } else if (WhichModeForHighscore == 2) {
                    GS.GetComponent<GameScript>().SetText(HighscoreForMode.GetComponent<Text>(), "< Records for Horde mode >", "< Rekordy dla trybu Hordy >");
                }
                if (HighscoreForMode.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    WhichModeForHighscore += 1;
                    if (WhichModeForHighscore > 2) {
                        WhichModeForHighscore = 1;
                    }
                }

                if (!PlayerPrefs.HasKey("HS" + WhichModeForHighscore + "1") && !PlayerPrefs.HasKey("HS" + WhichModeForHighscore + "2") && !PlayerPrefs.HasKey("HS" + WhichModeForHighscore + "3") && !PlayerPrefs.HasKey("HS" + WhichModeForHighscore + "4") && !PlayerPrefs.HasKey("HS" + WhichModeForHighscore + "5")) {

                    GS.GetComponent<GameScript>().SetText(HighscoreText.GetComponent<Text>(), "~ NO RECORDS YET ~", "~ NIE MA REKORDÓW ~");

                } else {

                    string HighScoresText = "";

                    for (int setScore = 1; setScore <= 5; setScore ++) {
                        string DifficultyName = "";
                        string RoundOrWaves = "";
                        if (setScore == 1) {
                            DifficultyName = GS.GetComponent<GameScript>().SetString("Easy", "Łatwy");
                        } else if (setScore == 2) {
                            DifficultyName = GS.GetComponent<GameScript>().SetString("Normal", "Normalny");
                        } else if (setScore == 3) {
                            DifficultyName = GS.GetComponent<GameScript>().SetString("Hard", "Trudny");
                        } else if (setScore == 4) {
                            DifficultyName = GS.GetComponent<GameScript>().SetString("Very Hard", "Bardzo Trudny");
                        } else if (setScore == 5) {
                            DifficultyName = GS.GetComponent<GameScript>().SetString("Hardcore", "Hardkorowy");
                        }
                        if (WhichModeForHighscore == 1) {
                            RoundOrWaves = GS.GetComponent<GameScript>().SetString("rounds", "rund");
                        } else {
                            RoundOrWaves = GS.GetComponent<GameScript>().SetString("waves", "fal");
                        }
                        if (PlayerPrefs.HasKey("HS" + WhichModeForHighscore + setScore) && PlayerPrefs.HasKey("HR" + WhichModeForHighscore + setScore)) {
                            HighScoresText += GS.GetComponent<GameScript>().SetString(
                                DifficultyName + ": " + PlayerPrefs.GetInt("HR" + WhichModeForHighscore + setScore) + " " + RoundOrWaves + " - " + PlayerPrefs.GetInt("HS" + WhichModeForHighscore + setScore) + " score \n",
                                DifficultyName + ": " + PlayerPrefs.GetInt("HR" + WhichModeForHighscore + setScore) + " " + RoundOrWaves + " - " + PlayerPrefs.GetInt("HS" + WhichModeForHighscore + setScore) + " wynik \n");
                        } else if (PlayerPrefs.HasKey("HR" + WhichModeForHighscore + setScore)) {
                            HighScoresText += GS.GetComponent<GameScript>().SetString(
                                DifficultyName + ": " + PlayerPrefs.GetInt("HR" + WhichModeForHighscore + setScore) + RoundOrWaves + "\n",
                                DifficultyName + ": " + PlayerPrefs.GetInt("HR" + WhichModeForHighscore + setScore) + RoundOrWaves + "\n");
                        } else if (PlayerPrefs.HasKey("HS" + WhichModeForHighscore + setScore)) {
                            HighScoresText += GS.GetComponent<GameScript>().SetString(
                                DifficultyName + ": " + PlayerPrefs.GetInt("HS" + WhichModeForHighscore + setScore) + " score \n",
                                DifficultyName + ": " + PlayerPrefs.GetInt("HS" + WhichModeForHighscore + setScore) + " wynik \n");
                        }
                    }

                    HighscoreText.GetComponent<Text>().text = HighScoresText;

                }
                
            }

        }

    }

    void WhileHowToPlay(bool Shown) {

        if (Shown == false) {

            WhileHowToPlayWindow.transform.position = Vector3.Lerp(WhileHowToPlayWindow.transform.position, HideWindow.position, 0.1f * (Time.deltaTime * 100f));

        } else {

            WhileHowToPlayWindow.transform.position = Vector3.Lerp(WhileHowToPlayWindow.transform.position, ShowWindow.position, 0.1f * (Time.deltaTime * 100f));
            GS.GetComponent<GameScript>().SetText(HTPBackButton.transform.GetChild(0).GetComponent<Text>(), "Back to Menu", "Wróć do Menu");
            if (HTPBackButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "MainMenu";
            }


            GS.GetComponent<GameScript>().SetText(HTPGameplayButton.GetComponent<Text>(), "Gameplay", "Rozgrywka");
            GS.GetComponent<GameScript>().SetText(HTPSurvivingButton.GetComponent<Text>(), "Surviving", "Przetrwanie");
            GS.GetComponent<GameScript>().SetText(HTPHordeButton.GetComponent<Text>(), "Horde Mode", "Tryb Hordy");
            GS.GetComponent<GameScript>().SetText(HTPScoresButton.GetComponent<Text>(), "Scores", "Wyniki");
            GS.GetComponent<GameScript>().SetText(HTPControlsButton.GetComponent<Text>(), "Controls", "Sterowanie");
            if (HTPGameplayButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                HTPPage = "Gameplay";
            }
            if (HTPSurvivingButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                HTPPage = "Surviving";
            }
            if (HTPHordeButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                HTPPage = "Horde";
            }
            if (HTPScoresButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                HTPPage = "Scores";
            }
            if (HTPControlsButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                HTPPage = "Controls";
            }


            if(HTPPage == ""){
                GS.GetComponent<GameScript>().SetText(HTPTitle, "How to Play", "Jak Grać");
                HTPText.text = "";
            } else if (HTPPage == "Gameplay") {
                GS.GetComponent<GameScript>().SetText(HTPTitle, "Gameplay Overview", "Przegląd Rozgrywki");
                GS.GetComponent<GameScript>().SetText(HTPText, 
                    "Swift Survivors is a fast-paced survival game. You play as a survivor, whose job is to survive as long as possible, by eating food, avoiding many types of hazards, and most importantly, escaping from nuclear bombardment. \n\nThe game is divided into rounds, each round is on a different map, with different layout, and different resources. \n\nYour objective is to explore and exploit the contents of maps, after which you'll have to find an exit through which you'll get to the next round, and thus you'll avoid getting nuked. \n\nBesides surviving, you can also care about increasing your highscores, by doing stuff like collecting treasures, or fighting mobs.", 
                    "Swift Survivors jest grą survival'ową z szybkim tempem. Wcielasz się w rolę niedobitka, którego zadaniem jest jak najdłuższe przeżycie, poprzez jedzenie, unikanie zagrożeń, oraz uciekanie przed bobmami jądrowymi. \n\nGra się dzieli na rundy, każda runda rozgrywa się na innej mapie, z innym rozkładem, i innymi surowcami. \n\nTwoim zadaniem jest eksploracja i eksploatacja zawartości tych map, po czym będziesz musiał znaleść wyjście, przez które dostaniesz się do następnej rundy, przez co unikniesz bobmy jądrowej. \n\nPoza przeżywaniem, możesz także zdobywać punkty, poprzez takie rzeczy jak zdobywanie skarbów, lub walczenie z bandytami i mutantami.");
            } else if (HTPPage == "Surviving") {
                GS.GetComponent<GameScript>().SetText(HTPTitle, "How to Survive", "Jak Przeżyć");
                GS.GetComponent<GameScript>().SetText(HTPText, 
                    "In order to survive, you'll need to do few things. \n\nThe main thing, is gathering resources, stuff like: food, medicine, weaponary, and sleeping bags. These resources will help you in your journey, and will help you in controling your needs. \n\nYou aren't the only one exploring the maps though! Another thing to consider, are hostile mobs, like mutants or bandits. They are all over the map, are unpredictable, and can either be pathetic weaklings, or the last thing you'll see before dying. \n\nThe living things aren't the only danger you'll need to take into consideration; you should also watchout for radiation on the map. Some areas of the map might be radiated, and standing on them can give you radiation sickness, which may eventually kill you.", 
                    "Aby przeżyć, musisz zwrócić uwagę na kilka rzeczy. \n\nGłówną rzeczą, jest zbieranie surowców, takich jak: jedzenie, lekarstwa, uzbrojenie, i śpiwory. The surowce pomogą tobie w twojej podrózy, i pomogą w kontrolowaniu twoich potrzeb. \n\nNie jesteś jednak jedyną osobą zwiedzającą mapy! Kolejną rzeczą na którą trzeba zwrócić uwagę, są groźne stworzenia, takich jak mutanci, i bandyci. Są nieprzewidywalni, i mogą być albo żałośnie słabi, albo ostatnią rzeczą którą zobaczysz prze śmiercią. \n\nPoza stworzeniami, trzeba też zwrócić uwagę na rzeczy martwe, czyli na aspekt promieniowania rozsianego na mapie. Niektóre tereny na mapie mogą być napromieniowane, stojąc na nich, możesz sam się napromienować, co może ciebie ewentualnie zabić.");
            } else if (HTPPage == "Horde") {
                GS.GetComponent<GameScript>().SetText(HTPTitle, "How to play Horde mode", "Jak grać w tryb hordy");
                GS.GetComponent<GameScript>().SetText(HTPText,
                    "A Horde mode, is an alternative game mode which differs a lot from the classic game mode. You are confined to a small premade map, on which you'll have to survive an infinite amount of mutant waves. \n\nBefore every wave, you'll have 60 seconds of intermission phase, during which you can explore the map and get equipment from vending machines and emergency boxes. \n\nAfter intermission, a wave begins, during which you'll have to defend yourself from certain amount of mutants, by killing them. Keep in mind, that each consecutive wave will have more mutants to kill, more mutants that can be on map at once, more special mutants, and more powerful mutants. \n\nKilling mutants will not only grant you score and money, but also random drops like: weapons, money, health packs, ammo packs, and more.", 
                    "Tryb hordy, jest alternatywnym trybem gry, który dość sporo różni się od trybu klasycznego. Znajdujesz się na małej mapie, na której będziesz musiał przeżyć nieskończoną ilość fal mutantów. \n\nPrzed każdą falą, będziesz miał 60 sekund przerwy, podczas której będziesz mógł zbadać mapę oraz pozyskać ekwipunek z automatów i skrzynek awaryjnych. \n\nPo przerwie, rozpoczyna się fala, podczas której będziesz się bronił przed pewną ilością mutantów, poprzez ich zabijanie. Warto wspomnić, iż każda kolejna fala, będzie miała więcej mutantów do zabicia, więcej mutantów na raz, więcej specjalnych mutantów, oraz bardziej potężnych mutantów. \n\nZabijanie mutantów nie tylko da ci pieniądze i punkty, ale także losowe dropy, takie jak: bronie, pieniądze, apteczki, amunicje, i inne.");
            } else if (HTPPage == "Scores") {
                GS.GetComponent<GameScript>().SetText(HTPTitle, "Scoring System", "Liczenie Wyników");
                GS.GetComponent<GameScript>().SetText(HTPText, 
                    "This is how you increase your score in the game: \n\nEscaping maps - Current round * 10\nKilling normal mutants - 100\nKilling bandits, special mutants, and guards - 1000\nTrading - 50 for goods, 10000 for treasures\nDiscovering exits and monuments - 50",
                    "Oto jak możesz powiększyść swoje wyniki: \n\nOpuszczanie map - Numer rundy * 10\nZabijanie zwykłych mutantów - 100\nZabijanie bandytów, specialnych mutantów, i strażników - 1000\nHandel - 50 za dobra, 10000 za skarby\nOdkrywanie wyjść i monumentów - 50");
            } else if (HTPPage == "Controls") {
                GS.GetComponent<GameScript>().SetText(HTPTitle, "Controls", "Sterowanie");
                GS.GetComponent<GameScript>().SetText(HTPText, 
                    "WSAD - Moving around\nSHIFT - Sprinting\nC - Crouching\nSPACE - Jumping\nE - Interacting and picking items up\nQ - Dropping (by pressing the key) and throwing (by holding the key) items\nTAB - Information tab, contains informations about round, map, buffs, and items you hold\nMOUSE 1 - Use item\nMOUSE 2 - Alternative use for item\nR - Reload",
                    "WSAD - Przemieszczanie się\nSHIFT - Bieg\nC - Kucanie\nSPACE - Skakanie\nE - Interakcje i podnoszenie przedmiotów\nQ - Upuszczanie (kliknięciem klawisza) oraz rzucanie (przytrzymaniem klawisza) przedmiotów\nTAB - Zakładka informacyjna, posiada informacje dotyczące rundy, mapy, efektów, oraz przedmiotów które trzymasz\nMOUSE 1 - Użyj przedmiotu\nMOUSE 2 - Alternatywne użycie przedmiotu\nR - Przeładowywanie");
            }

        }

    }

    void WhileGameOver(bool Shown){

        if (Shown == false) {

            GameOverWindow.transform.position = HideWindow.position;

        } else {

            GameOverWindow.transform.position = ShowWindow.position;

            /*if (GS.GetComponent<GameScript>().ScoreToSet != null) {
                print("HS" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1] + " is " + GS.GetComponent<GameScript>().ScoreToSet[2]);
                print("HR" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1] + " is " + GS.GetComponent<GameScript>().ScoreToSet[3]);
                GotGOVariables[2] = GS.GetComponent<GameScript>().ScoreToSet[0];

                // Highscores
                if (!PlayerPrefs.HasKey("HS" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1]) && GS.GetComponent<GameScript>().ScoreToSet[2] > 0) {
                    PlayerPrefs.SetInt("HS" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1], GS.GetComponent<GameScript>().ScoreToSet[2]);
                    GotGOVariables[0] = GS.GetComponent<GameScript>().ScoreToSet[2];
                    GotHighScore = true;
                } else if (PlayerPrefs.GetInt("HS" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1]) < GS.GetComponent<GameScript>().ScoreToSet[2]) {
                    PlayerPrefs.SetInt("HS" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1], GS.GetComponent<GameScript>().ScoreToSet[2]);
                    GotGOVariables[0] = GS.GetComponent<GameScript>().ScoreToSet[2];
                    GotHighScore = true;
                } else {
                    GotGOVariables[0] = GS.GetComponent<GameScript>().ScoreToSet[2];
                    GotHighScore = false;
                }

                if (!PlayerPrefs.HasKey("HR" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1]) && GS.GetComponent<GameScript>().ScoreToSet[3] > 0) {
                    PlayerPrefs.SetInt("HR" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1], GS.GetComponent<GameScript>().ScoreToSet[3]);
                    GotGOVariables[1] = GS.GetComponent<GameScript>().ScoreToSet[3];
                    GotHighRounds = true;
                } else if (PlayerPrefs.GetInt("HR" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1]) < GS.GetComponent<GameScript>().ScoreToSet[3]) {
                    PlayerPrefs.SetInt("HR" + GS.GetComponent<GameScript>().ScoreToSet[0] + GS.GetComponent<GameScript>().ScoreToSet[1], GS.GetComponent<GameScript>().ScoreToSet[3]);
                    GotGOVariables[1] = GS.GetComponent<GameScript>().ScoreToSet[3];
                    GotHighRounds = true;
                } else {
                    GotGOVariables[1] = GS.GetComponent<GameScript>().ScoreToSet[3];
                    GotHighRounds = false;
                }

                GS.GetComponent<GameScript>().ScoreToSet = null;
            }*/

            string GOTtoset = "";
            string RoundOrWaves = "";
            print(GotGOVariables[2]);
            if (GotGOVariables[2] == 1) {
                if (GotGOVariables[1] <= 0) {
                    RoundOrWaves = GS.GetComponent<GameScript>().SetString("round", "rundy");
                } else {
                    RoundOrWaves = GS.GetComponent<GameScript>().SetString("Rounds", "rundy");
                }
            } else {
                if (GotGOVariables[1] <= 0) {
                    RoundOrWaves = GS.GetComponent<GameScript>().SetString("wave", "fali");
                } else {
                    RoundOrWaves = GS.GetComponent<GameScript>().SetString("Waves", "Fale");
                }
            }
            if (GotHighRounds == true) {
                GOTtoset += GS.GetComponent<GameScript>().SetString(RoundOrWaves + " survived: " + GotGOVariables[1] + " ~ New Record!", "Przeżyte " + RoundOrWaves + ": " + GotGOVariables[1] + " ~ Nowy Rekord!");
            } else if (GotGOVariables[1] > 0) {
                GOTtoset += GS.GetComponent<GameScript>().SetString(RoundOrWaves + " survived: " + GotGOVariables[1], "Przeżyte " + RoundOrWaves + ": " + GotGOVariables[1]);
            } else if (GotGOVariables[1] <= 0 && GotGOVariables[0] > 0) {
                GOTtoset += GS.GetComponent<GameScript>().SetString("You haven't survived a single " + RoundOrWaves + "!", "Nie przeżyłeś nawet jednej " + RoundOrWaves + "!");
            }

            if (GotHighScore == true) {
                GOTtoset += GS.GetComponent<GameScript>().SetString("\nScore achieved: " + GotGOVariables[0] + " ~ New Record!", "\nZdobyty wynik: " + GotGOVariables[0] + " ~ Nowy Rekord!");
            } else if (GotGOVariables[0] > 0) {
                GOTtoset += GS.GetComponent<GameScript>().SetString("\nScore achieved: " + GotGOVariables[0], "\nZdobyty wynik: " + GotGOVariables[0]);
            }

            if (GotGOVariables[1] <= 0 && GotGOVariables[0] <= 0) {
                GOTtoset += GS.GetComponent<GameScript>().SetString("You haven't survived a single " + RoundOrWaves + ", and your score is equal to zero. Weak!", "Nie przeżyłeś nawet jednej " + RoundOrWaves + ", a twój wynik jest równy zeru. Słabiutko!");
            }

            GOTtoset += GS.GetComponent<GameScript>().SetString("\n\nPress any key to continue", "\n\nNaciśnij dowolny klawisz by kontynuować");

            GameOverText.text = GOTtoset;

            GameOverBG.color -= new Color(0f, 0f, 0f, 0.0025f * (Time.deltaTime * 100f));
            if (YourDeadImage.color.a < 1f) {
                YourDeadImage.color += new Color(0f, 0f, 0f, 0.01f * (Time.deltaTime * 100f));
            } else if (GameOverText.color.a < 1f) {
                GameOverText.color += new Color(0f, 0f, 0f, 0.01f * (Time.deltaTime * 100f));
            }

            if (Input.anyKeyDown) {
                if (GameOverText.color.a < 0.9f) {
                    YourDeadImage.color = Color.white;
                    GameOverText.color = Color.white;
                    GameOverBG.color = new Color(0f, 0f, 0f, 0f);
                } else {
                    MainCamera.GetComponent<Camera>().orthographicSize = 0f;
                    GameOverMusic.Stop();
                    MenuMusic.Play();
                    CurrentWindow = "MainMenu";
                    int ConsiderChance = Random.Range(0, 5);
                    if (ConsiderChance == 0) {
                        MessageContent = "Consideration";
                    } else if (ConsiderChance == 1) {
                        MessageContent = "Feedback";
                    } else if (ConsiderChance == 2 && GS.GetComponent<GameScript>().Platform == 2) {
                        MessageContent = "Download";
                    }
                }
            }

        }

    }

    void WhileMessage(bool Shown) {

        if (Shown == false) {

            MessageWindow.transform.position = HideWindow.position;

        } else {

            MessageWindow.transform.position = ShowWindow.position;

            if (MessageButtons[0].GetComponent<ButtonScript>().Active == false) {
                MessageButtons[0].transform.localScale = Vector3.zero;
            }
            if (MessageButtons[1].GetComponent<ButtonScript>().Active == false) {
                MessageButtons[1].transform.localScale = Vector3.zero;
            }
            if (MessageButtons[2].GetComponent<ButtonScript>().Active == false) {
                MessageButtons[2].transform.localScale = Vector3.zero;
            }

            if (MessageContent == "1ClearSaveFile" || MessageContent == "2ClearSaveFile" || MessageContent == "3ClearSaveFile") {

                GS.GetComponent<GameScript>().SetText(MessageText, "Are you sure you want to clear that save file? Your score will not be saved!", "Jesteś pewien, że chcesz usunąć ten zapis? Twój wynik nie zostanie zapisany!");

                MessageButtons[0].GetComponent<ButtonScript>().Active = false;
                MessageButtons[1].GetComponent<ButtonScript>().Active = true;
                GS.GetComponent<GameScript>().SetText(MessageButtons[1].transform.GetChild(0).GetComponent<Text>(), "Yes", "Tak");
                MessageButtons[2].GetComponent<ButtonScript>().Active = true;
                GS.GetComponent<GameScript>().SetText(MessageButtons[2].transform.GetChild(0).GetComponent<Text>(), "No", "Nie");

                if (MessageButtons[1].GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                   // GS.GetComponent<GameScript>().Saves(int.Parse(MessageContent.Substring(0, 1)), 2);
                   Debug.LogError("This menu system is obsolete, and it does not work!");
                    MessageContent = "";
                } else if (MessageButtons[2].GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    MessageContent = "";
                }

            } else if (MessageContent == "Consideration") {

                GS.GetComponent<GameScript>().SetText(MessageText, 
                    "Enjoying the game so far? Why not give it a rating, good or bad!", 
                    "Podoba się gra? Dlaczego by tak nie dać jej oceny, dobrej lub złej!");

                MessageButtons[0].GetComponent<ButtonScript>().Active = true;
                GS.GetComponent<GameScript>().SetText(MessageButtons[0].transform.GetChild(0).GetComponent<Text>(), "Sure", "Jasne");
                MessageButtons[1].GetComponent<ButtonScript>().Active = false;
                MessageButtons[2].GetComponent<ButtonScript>().Active = false;

                if (MessageButtons[0].GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    MessageContent = "";
                }

            } else if (MessageContent == "Feedback") {

                GS.GetComponent<GameScript>().SetText(MessageText, 
                    "Have you encountered any bugs, glitches, typos, or anything like that? Do you think this game lacks something, or needs some changes? Why not leave a comment with a feedback about it!", 
                    "Napotkałeś jakieś błędy, glicze, literówki, albo tym podobne? Uważasz że grze czegoś brakuje? Dlaczego by tak nie pozostawić komentarza z informacjami zwrotnymi!");

                MessageButtons[0].GetComponent<ButtonScript>().Active = true;
                GS.GetComponent<GameScript>().SetText(MessageButtons[0].transform.GetChild(0).GetComponent<Text>(), "Sure", "Jasne");
                MessageButtons[1].GetComponent<ButtonScript>().Active = false;
                MessageButtons[2].GetComponent<ButtonScript>().Active = false;

                if (MessageButtons[0].GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    MessageContent = "";
                }

            } else if (MessageContent == "Download") {

                GS.GetComponent<GameScript>().SetText(MessageText, 
                    "You are playing this game on a browser. However, you can also download the game. It is highly recommended if you're expirencing low frame rate!", 
                    "Obecnie, grasz w przeglądarce. Jednakże, możesz także pobrać tą grę na komputer. Jest to zalecane, jeżeli masz problemy z słabą optymalizacją!");

                MessageButtons[0].GetComponent<ButtonScript>().Active = true;
                GS.GetComponent<GameScript>().SetText(MessageButtons[0].transform.GetChild(0).GetComponent<Text>(), "Sure", "Jasne");
                MessageButtons[1].GetComponent<ButtonScript>().Active = false;
                MessageButtons[2].GetComponent<ButtonScript>().Active = false;

                if (MessageButtons[0].GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    MessageContent = "";
                }

            } else if (MessageContent == "Exit") {

                GS.GetComponent<GameScript>().SetText(MessageText, 
                    "Are you sure you want to exit?", 
                    "Na pewno chcesz wyjść?");

                MessageButtons[0].GetComponent<ButtonScript>().Active = false;
                MessageButtons[1].GetComponent<ButtonScript>().Active = true;
                GS.GetComponent<GameScript>().SetText(MessageButtons[1].transform.GetChild(0).GetComponent<Text>(), "Exit", "Wyjdź");
                MessageButtons[2].GetComponent<ButtonScript>().Active = true;
                GS.GetComponent<GameScript>().SetText(MessageButtons[2].transform.GetChild(0).GetComponent<Text>(), "Stay", "Wróć");

                if (MessageButtons[1].GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    Application.Quit();
                    MessageContent = "";
                } else if (MessageButtons[2].GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                    MessageContent = "";
                }

            }

        }

    }

}
