using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

    // References
    public GameScript GS;
    public RoundScript RS;
    public PlayerScript MainPlayer;
    public Transform MainCamera;
    // Whiles
    public Transform HideWindow;
    public Transform UpWindow;
    public Transform ShowWindow;
    // Loading
    public GameObject LoadingWindow;
    public Text LoadingText;
    public Text LoadingTip;
    // Loading
    // While Alive
    public GameObject WhileAliveWindow;
    public GameObject SBGHealth;
    public GameObject SBGHunger;
    public GameObject SBGStamina;
    public GameObject SBGOxygen;
    public GameObject Inventory;
    public Transform ItemInfo;
    string PrevInvSlot = "-----";
    float SwitchItemInfo = 0f;
    public GameObject Equipment;
    float[] EqTextScroll = new float[] { 0f, 0f };
    public GameObject Crosshair;
    public GameObject InteractableIcon;
    public float[] CSAlert = new float[] { 0f, 0f };
    public float[] CSWait = new float[]{ 0f, 0f };
    public GameObject InformationsTab;
    public string ITShown = "";
    public GameObject ITicons;
    public GameObject ITMenuInfo;
    public GameObject ITBG;
    public GameObject ITMap;
    public GameObject MarksTouchPad;
    public List<GameObject> MarksList;
    public GameObject ITItemHeldInfo;
    public GameObject ITRoundInfo;
    public GameObject ITDetails;
    public GameObject ITBuffs;
    float[] BuffTextScroll = new float[]{0f, 0f};
    public GameObject MiniMap;
    public GameObject MiniMapMarker;
    public GameObject ITMenuCraft;
    public GameObject CraftingMain;
    public Text CraftingDetailedText;
    public string CDTstring = "";
    public float[] CDTdisplaye = new float[]{0f, 0f, 0f}; // Show, fade in
    public float CDTx;
    public int[] CraftingPage = new int[] { 1, 1 };
    public GameObject[] CraftButtons;
    public GameObject CraftingTemplates;
    public AudioSource CraftingSound;
    public float PlayCraftingSound = 0f;
    public GameObject RoundStartInfo;
    public GameObject DamageIndicator;
    public Vector3 DamagedPosition;
    public GameObject MobHealthBar;
    public GameObject Buffs;
    public GameObject DialogedMob;
    public GameObject DialogMenu;
    public Text DialogName;
    public Text DialogDesc;
    public GameObject[] DialogOptionsButtons;
    public string DialogSetting = "Default";
    string[] DialogOptions;
    public Text HintText;
    public List<string> HintsCooldown;
    public List<string> HintsTold;
    public float ClearHint = 0f;
    float HintScan = 0f;
    public Sprite[] ItemIcons;
    public Sprite[] SubItemIcons;
    public Sprite[] MinimapMarkerIcons;
    public Image MMaura;
    public float MMdanger, MMsafe, MMunsafe = 0f;
    // While Alive
    // While Dead
    public GameObject WhileDeadWindow;
    public float Leave = 0f;
    // While Dead
    // While Paused
    public bool IsPaused = false;
    public NewMenuScript PauseMenu;
    // While Paused
    // Escaped
    public GameObject EscapedWindow;
    public Text[] EscapedTextes;
    // Escaped
    // Environmental effects
    public GameObject EnvironmentalWindow;
    public Transform EnvironmentalObject;
    public GameObject EnvSplash;
    // Environmental effects
    // Whiles
    float MessageShift = 0f;
    public Image FlashImage;
    public float[] FlashImageSpeed = new float[] { 0f, 1f };
    public GameObject SoundBank;
    public Image NightVision;
    public GameObject ItemPrefab;
    // References

    // Misc
    public float LockCursor = 0f;
    public float HideCursor = 0f;
    float mapRefresh = 0f;
    bool SetRoundTextOnStart = true;
    bool HideCanvas = false;
    public GameObject Billboard;
    public float CameraBlur = 0f;
    // Misc

    // Use this for initialization
    void Start() {

        if (GS == null || RS == null || MainPlayer == null) {
            if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(GameObject.Find("_RoundScript")) RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
            if(GameObject.FindGameObjectWithTag("Player")) MainPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            if(GameObject.Find("MainCamera")) MainCamera = GameObject.Find("MainCamera").transform;
        }

        HintsCooldown.Add("Movement");
        if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0") {
            HintsCooldown.Add("Biome");
        } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
            HintsCooldown.Add("HordeStart");
        }
        HintsCooldown.Add("Tab");

    }

    // Update is called once per frame
    void Update() {

        if (GS == null || RS == null || MainPlayer == null) {
            if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(GameObject.Find("_RoundScript")) RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
            if(GameObject.FindGameObjectWithTag("Player")) MainPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            if(GameObject.Find("MainCamera")) MainCamera = GameObject.Find("MainCamera").transform;
        } else {

            // Hide canvas
            if (Input.GetKeyDown(KeyCode.F1)) {
                if (HideCanvas == false) {
                    HideCanvas = true;
            //        WhileAliveWindow.transform.localScale = Vector3.zero;
                } else {
                    HideCanvas = false;
            //        WhileAliveWindow.transform.localScale = Vector3.one;
                }
            }
            // Hide canvas

            // Cursors
            if (LockCursor > 0f) {
                LockCursor -= 0.01f;
                Cursor.lockState = CursorLockMode.Locked;
            } else {
                Cursor.lockState = CursorLockMode.None;
            }

            if (HideCursor > 0f) {
                HideCursor -= 0.01f;
                Cursor.visible = false;
            } else {
                Cursor.visible = true;
            }
            // Cursors

            // Pause
            if (IsPaused == true) {
                Time.timeScale = 0f;
                /*if (Input.GetKeyDown(KeyCode.Escape)) {
                    IsPaused = false;
                }*/
            } else {
                Time.timeScale = 1f;
                if (Input.GetKeyDown(KeyCode.Escape) && MainPlayer.Health[0] > 0f) {
                    IsPaused = true;
                }
            }
            // Pause

            // Set Windows
            if (PauseMenu.LoadingTime > 0f) {
                WhileLoading(false);
                WhilePaused(false);
                WhileAlive(false);
                WhileDead(false);
                WhileEnvironmental(false);
                WhileEscaped("");
            } else if (IsPaused == true) {
                WhileLoading(false);
                WhilePaused(true);
                WhileAlive(false);
                WhileDead(false);
                WhileEnvironmental(true);
                WhileEscaped("");
            } else if (MainPlayer.State == 1) {
                WhileLoading(false);
                WhilePaused(false);
                WhileAlive(true);
                WhileDead(false);
                WhileEnvironmental(true);
                WhileEscaped("");
            } else if (MainPlayer.State == 2) {
                WhileLoading(false);
                WhilePaused(false);
                WhileAlive(false);
                WhileDead(true);
                WhileEnvironmental(true);
                WhileEscaped("");
            } else if (RS.RoundState.Substring(0, 3) == "ESC"){
                WhileLoading(false);
                WhilePaused(false);
                WhileAlive(false);
                WhileDead(false);
                WhileEnvironmental(false);
                WhileEscaped("Escaped");
            }
            // Set Windows

            // Flash Image
            FlashImageSpeed[0] = Mathf.Clamp(FlashImageSpeed[0] - (0.02f * (Time.deltaTime * 50f)), 0f, Mathf.Infinity);
            FlashImage.color = Color.Lerp(new Color(FlashImage.color.r, FlashImage.color.g, FlashImage.color.b, 0f), new Color(FlashImage.color.r, FlashImage.color.g, FlashImage.color.b, 1f), FlashImageSpeed[0] / FlashImageSpeed[1]);
            // Flash Image

            // Crafting sound
            if (PlayCraftingSound > 0f) {
                PlayCraftingSound = Mathf.Clamp(PlayCraftingSound - (0.02f * (Time.deltaTime * 50f)), 0f, 1f);
                if (CraftingSound.isPlaying == false) {
                    CraftingSound.Play();
                }
            } else if (CraftingSound.isPlaying == true) {
                CraftingSound.Stop();
            }
            // Crafting sound

            // Clear focus
            if(CameraBlur > 0f){
                CameraBlur -= 0.2f * (Time.deltaTime * 50f);
                GS.CameraFocus = new float[]{ Mathf.Lerp(10f, 100f, CameraBlur / 10f), 2f, Mathf.Lerp(4f, 100f, CameraBlur / 10f) };
            } else {
                GS.CameraFocus = new float[]{10f, 2f, 4f};
            }
            // Clear focus

        }

    }

    void WhilePaused(bool Shown) {

        if (Shown == false) {

            //WhilePausedWindow.transform.position = HideWindow.position;
            PauseMenu.isVisible = false;

        } else {

            PauseMenu.isVisible = true;
            CameraBlur = Mathf.Clamp(CameraBlur + (0.4f * Time.unscaledDeltaTime * 50f), 0f, 10f);
            MainPlayer.FOVoffset = new float[] {Mathf.Lerp(MainPlayer.FOVoffset[0], -30f, 0.2f * (Time.unscaledDeltaTime * 50f)), 0.2f};
            /*WhilePausedWindow.transform.position = ShowWindow.position;

            GS.SetText(PauseLogo.GetComponent<Text>(), "PAUSED", "PAUZA");
            GS.SetText(ReturnButton.GetComponent<Text>(), "Unpause", "Odpauzuj");
            GS.SetText(SuicideButton.GetComponent<Text>(), "Give up", "Poddaj się");
            GS.SetText(ExitButton.GetComponent<Text>(), "Back to menu", "Wróć do menu");
            if (ReturnButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButton(0)) {
                IsPaused = false;
            } else if (SuicideButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButton(0)) {
                IsPaused = false;
                MainPlayer.Hurt(MainPlayer.Health[1], "Suicide", false, Vector3.zero);
                GameObject Boom = Instantiate(MainPlayer.SpecialPrefab) as GameObject;
                Boom.transform.position = MainPlayer.transform.position;
                Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
            } else if (ExitButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButton(0)) {
                GS.ChangeLevel("BackToMenu");
            }*/

            MainPlayer.CantInteract = Mathf.Clamp(MainPlayer.CantInteract, 0.25f, Mathf.Infinity);
            MainPlayer.CantMove = Mathf.Clamp(MainPlayer.CantMove, 0.25f, Mathf.Infinity);
            MainPlayer.CantUseItem = Mathf.Clamp(MainPlayer.CantUseItem, 0.25f, Mathf.Infinity);

            // Fullscreen
            /*if (GS.GetComponent<GameScript>().Platform == 2) {
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
            }*/
            // Fullscreen

        }

    }

    void WhileAlive(bool Shown) {

        if (Shown == false) {

            WhileAliveWindow.transform.position = HideWindow.position;

        } else {

            WhileAliveWindow.transform.position = ShowWindow.position;

            // Hide canvas
            if(HideCanvas){
                WhileAliveWindow.transform.position = HideWindow.position;
                //Crosshair.transform.position = ShowWindow.position;
            } else {
                WhileAliveWindow.transform.position = ShowWindow.position;
                //Crosshair.transform.position = ShowWindow.position;
            }

            if (DialogedMob == null && GS.ReceiveButtonPress("InformationTab", "Hold") <= 0f && GS.ReceiveButtonPress("CraftingTab", "Hold") <= 0f) {
                LockCursor = 0.1f;
                HideCursor = 0.1f;
            } else {
                MainPlayer.CantLook = 0.1f;
                MainPlayer.CantUseItem = 0.1f;
            }

            // Contrast temperature Saturation vingette
            GS.ContSaturTempInvi = new float[]{
                RS.DefCST[0],
                Mathf.Lerp(-100f, RS.DefCST[1], MainPlayer.Health[0] / (MainPlayer.Health[1]/2f) ),
                RS.DefCST[2],
                0.25f + ((1f - (MainPlayer.Energy[0] / MainPlayer.Energy[1])) * 0.1f)
            };
            //if(RS.DefCST[3] != 0f) GS.ContSaturTempInvi[3] = RS.DefCST[3];
            GS.PPColor = Color32.Lerp(Color.white, RS.DefPPC, MainPlayer.Health[0] / (MainPlayer.Health[1]/2f));
            if (RS.RoundState == "Nuked") {
                //GS.ContSaturTempInvi = new float[] { 100f, (1f - Mathf.Clamp(MainPlayer.Health[0] / (MainPlayer.Health[1] / 2f), 0f, 1f)) * -100f, 50f, 0.25f };
                if (Vector3.Distance(MainPlayer.transform.position, new Vector3(RS.NukePosition.x, MainPlayer.transform.position.y, RS.GetComponent<RoundScript>().NukePosition.z)) - RS.NukeDistance < 100f) {
                    MainPlayer.ShakeCam((100f - (Vector3.Distance(MainPlayer.transform.position, new Vector3(RS.GetComponent<RoundScript>().NukePosition.x, MainPlayer.transform.position.y, RS.NukePosition.z)) - RS.NukeDistance)) / 100f, 0.1f);
                    Flash(new Color32(255, 255, 255, (byte)(((100f - (Vector3.Distance(MainPlayer.transform.position, new Vector3(RS.GetComponent<RoundScript>().NukePosition.x, MainPlayer.transform.position.y, RS.NukePosition.z)) - RS.NukeDistance)) / 100f) * 255f)), new float[]{1f, 1f});
                }
            } /*else if (RS.RoundState == "TealState"){
                GS.ContSaturTempInvi = new float[] { 0f, -50f, -50f, 0.25f };
            } else {
                GS.ContSaturTempInvi = new float[] { 0f, (1f - Mathf.Clamp(MainPlayer.Health[0] / (MainPlayer.Health[1] / 2f), 0f, 1f)) * -100f, (1f - RS.Sunnyness) * -25f, 0.25f + ((1f - (MainPlayer.Energy[0] / MainPlayer.Energy[1])) * 0.1f) };
            }*/

            // Contrast temperature Saturation vingette

            // Round start info
            if (SetRoundTextOnStart == true) {
                if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0") {
                    GS.SetText(RoundStartInfo.transform.GetChild(0).GetComponent<Text>(), "Round " + GS.Round, "Runda " + GS.Round);
                    GS.SetText(RoundStartInfo.transform.GetChild(1).GetComponent<Text>(), RS.GotTerrain.GetComponent<BiomeInfo>().BiomeName[0], RS.GotTerrain.GetComponent<BiomeInfo>().BiomeName[1]);
                } else {
                    GS.SetText(RoundStartInfo.transform.GetChild(0).GetComponent<Text>(), RS.GotTerrain.GetComponent<MapInfo>().MapName[0], RS.GotTerrain.GetComponent<MapInfo>().MapName[1]);
                    GS.SetText(RoundStartInfo.transform.GetChild(1).GetComponent<Text>(), "Current wave: " + GS.Round, "Obecna fala: " + GS.Round);
                }
                SetRoundTextOnStart = false;
            }
            if (RoundStartInfo.transform.GetChild(0).GetComponent<Text>().color.a < 1f && RoundStartInfo.GetComponent<Image>().color.a == 1f) {
                RoundStartInfo.transform.GetChild(0).GetComponent<Text>().color += new Color(0f, 0f, 0f, 0.02f * (Time.deltaTime * 50f));
            } else if (RoundStartInfo.transform.GetChild(1).GetComponent<Text>().color.a < 1f && RoundStartInfo.GetComponent<Image>().color.a > 0f){
                RoundStartInfo.transform.GetChild(1).GetComponent<Text>().color += new Color(0f, 0f, 0f, 0.02f * (Time.deltaTime * 50f));
                RoundStartInfo.GetComponent<Image>().color -= new Color(0f, 0f, 0f, 0.01f * (Time.deltaTime * 50f));
            } else if (RoundStartInfo.GetComponent<Image>().color.a > 0f) {
                RoundStartInfo.GetComponent<Image>().color -= new Color(0f, 0f, 0f, 0.01f * (Time.deltaTime * 50f));
            } else if (RoundStartInfo.transform.GetChild(0).GetComponent<Text>().color.a > 0f || RoundStartInfo.transform.GetChild(1).GetComponent<Text>().color.a > 0f) {
                RoundStartInfo.transform.GetChild(0).GetComponent<Text>().color -= new Color(0f, 0f, 0f, 0.005f * (Time.deltaTime * 50f));
                RoundStartInfo.transform.GetChild(1).GetComponent<Text>().color -= new Color(0f, 0f, 0f, 0.005f * (Time.deltaTime * 50f));
            }
            // Round start info

            // Needs
            float HealthPC = MainPlayer.Health[0] / MainPlayer.Health[1];
            SBGHealth.transform.GetChild(0).GetComponent<Image>().fillAmount = HealthPC;
            SBGHealth.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1f - HealthPC;
            SBGHealth.transform.GetChild(1).GetComponent<Text>().text = GS.SetString("Health:\n", "Zdrowie:\n") + Mathf.Floor(MainPlayer.Health[0]).ToString() + " / " + Mathf.Floor(MainPlayer.Health[1]).ToString();
            if (HealthPC > 0.5f) {
                SBGHealth.transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 1f, 0f, 0.5f);
            } else if (HealthPC > 0.25f) {
                SBGHealth.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0.75f, 0f, 0.5f);
            } else {
                SBGHealth.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0.25f, 0f, 0.5f);
            }

            float HungerPC = (MainPlayer.Food[0] / MainPlayer.Food[1]);
            SBGHunger.transform.GetChild(0).GetComponent<Image>().fillAmount = HungerPC;
            SBGHunger.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1f - HungerPC;
            if (HungerPC <= 0f){
                SBGHunger.transform.GetChild(1).GetComponent<Text>().text = GS.SetString("Get something to eat - ", "Zjedz coś - ") + Mathf.Floor(MainPlayer.Food[0]).ToString() + "/" + Mathf.Floor(MainPlayer.Food[1]).ToString();
            } else if (HungerPC < MainPlayer.FoodLimits[0] / MainPlayer.Food[1]) {
                SBGHunger.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0.25f, 0f, 0.5f);
                SBGHunger.transform.GetChild(1).GetComponent<Text>().text = GS.SetString("Hungry - ", "Głodny - ") + Mathf.Floor(MainPlayer.Food[0]).ToString() + "/" + Mathf.Floor(MainPlayer.Food[1]).ToString();
            } else if (HungerPC < MainPlayer.FoodLimits[1] / MainPlayer.Food[1]) {
                SBGHunger.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0.75f, 0f, 0.5f);
                SBGHunger.transform.GetChild(1).GetComponent<Text>().text = GS.SetString("Fine - ", "W porządku - ") + Mathf.Floor(MainPlayer.Food[0]).ToString() + "/" + Mathf.Floor(MainPlayer.Food[1]).ToString();
            } else if (HungerPC >= 1f) {
                SBGHunger.transform.GetChild(0).GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.5f);
                SBGHunger.transform.GetChild(1).GetComponent<Text>().text = GS.SetString("Full - ", "Pełen - ") + Mathf.Floor(MainPlayer.Food[0]).ToString() + "/" + Mathf.Floor(MainPlayer.Food[1]).ToString();
            } else {
                SBGHunger.transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 1f, 0f, 0.5f);
                SBGHunger.transform.GetChild(1).GetComponent<Text>().text = GS.SetString("Well fed - ", "Najedzony - ") + Mathf.Floor(MainPlayer.Food[0]).ToString() + "/" + Mathf.Floor(MainPlayer.Food[1]).ToString();
            }

            float EnergyPC = MainPlayer.Energy[0] / MainPlayer.Energy[1];
            byte SetAlpha = (byte)Mathf.Lerp(0, 255, (SBGStamina.GetComponent<RectTransform>().anchoredPosition.y - 24f) / 16f);
            SBGStamina.GetComponent<Image>().color = new Color32((byte)(SBGStamina.transform.GetChild(0).GetComponent<Image>().color.r * 255f), (byte)(SBGStamina.transform.GetChild(0).GetComponent<Image>().color.g * 255f), (byte)(SBGStamina.transform.GetChild(0).GetComponent<Image>().color.b * 255f), (byte)(SetAlpha / 2f));
            SBGStamina.transform.GetChild(0).GetComponent<Image>().color = SBGStamina.GetComponent<Image>().color;
            SBGStamina.transform.GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, SetAlpha);
            if (MainPlayer.Energy[0] < MainPlayer.Energy[1] - MainPlayer.Tiredness) {
                SBGStamina.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(SBGStamina.GetComponent<RectTransform>().anchoredPosition, new Vector2(-80f, 48f), 1f);
                SBGStamina.transform.GetChild(0).GetComponent<Image>().fillAmount = EnergyPC;
                SBGStamina.transform.GetChild(1).GetComponent<Text>().text = ((int)(EnergyPC * 100f)).ToString() + " %";
                if (EnergyPC > 0.5f) {
                    SBGStamina.transform.GetChild(0).GetComponent<Image>().color = new Color32(200, 200, 200, 128);
                } else {
                    SBGStamina.transform.GetChild(0).GetComponent<Image>().color = Color32.Lerp(new Color32(255, 0, 0, 128), new Color32(200, 200, 200, 128), EnergyPC * 2f);
                }
                SBGStamina.GetComponent<Image>().color = SBGStamina.transform.GetChild(0).GetComponent<Image>().color;
            } else {
                SBGStamina.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(SBGStamina.GetComponent<RectTransform>().anchoredPosition, new Vector2(-80f, 24f), 1f);
            }

            float OxygenPC = MainPlayer.Oxygen[0] / MainPlayer.Oxygen[1];
            byte SetAlphaA = (byte)Mathf.Lerp(0, 255, (SBGOxygen.GetComponent<RectTransform>().anchoredPosition.y - 4f) / 16f);
            SBGOxygen.GetComponent<Image>().color = new Color32((byte)(SBGOxygen.transform.GetChild(0).GetComponent<Image>().color.r * 255f), (byte)(SBGOxygen.transform.GetChild(0).GetComponent<Image>().color.g * 255f), (byte)(SBGOxygen.transform.GetChild(0).GetComponent<Image>().color.b * 255f), (byte)(SetAlphaA / 2f));
            SBGOxygen.transform.GetChild(0).GetComponent<Image>().color = SBGOxygen.GetComponent<Image>().color;
            SBGOxygen.transform.GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, SetAlphaA);
            if (OxygenPC < 1f) {
                SBGOxygen.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(SBGOxygen.GetComponent<RectTransform>().anchoredPosition, new Vector2(-80f, 28f), 1f);
                SBGOxygen.transform.GetChild(0).GetComponent<Image>().fillAmount = OxygenPC;
                SBGOxygen.transform.GetChild(1).GetComponent<Text>().text = ((int)MainPlayer.Oxygen[0]).ToString() + GS.SetString(" seconds left", " sekund pozostało");
                if (OxygenPC > 0.5f) {
                    SBGOxygen.transform.GetChild(0).GetComponent<Image>().color = new Color32(128, 200, 255, 128);
                } else {
                    SBGOxygen.transform.GetChild(0).GetComponent<Image>().color = Color32.Lerp(new Color32(255, 0, 0, 128), new Color32(128, 200, 255, 128), OxygenPC * 2f);
                }
                SBGOxygen.GetComponent<Image>().color = SBGOxygen.transform.GetChild(0).GetComponent<Image>().color;
            } else {
                SBGOxygen.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(SBGOxygen.GetComponent<RectTransform>().anchoredPosition, new Vector2(-80f, 4f), 1f);
            }

            if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                SBGHunger.SetActive(false);
            } else {
                SBGHunger.SetActive(true);
            }
            // Needs

            // Inventory
            Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(-16f + MainPlayer.MaxInventorySlots * -46f, 16f);
            for (int InventoryCheck = 0; InventoryCheck <= 9; InventoryCheck++) {
                if (InventoryCheck >= MainPlayer.MaxInventorySlots) {
                    Inventory.transform.GetChild(InventoryCheck).gameObject.SetActive(false);
                } else {
                    Inventory.transform.GetChild(InventoryCheck).gameObject.SetActive(true);
                    string ItemHeld = MainPlayer.Inventory[InventoryCheck];
                    if (float.Parse(GS.GetSemiClass(ItemHeld, "id")) > 0f) {
                        if (Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().sprite.name.Substring(1) != GS.GetSemiClass(ItemHeld, "id") || Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().color.a <= 0f) {
                            foreach (Sprite GetSprite in ItemIcons) {
                                if (GetSprite.name.Substring(1) == GS.GetSemiClass(ItemHeld, "id")) {
                                    Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().sprite = GetSprite;
                                    if (GS.GetSemiClass(ItemHeld, "id") == "11" || GS.GetSemiClass(ItemHeld, "id") == "12" || GS.GetSemiClass(ItemHeld, "id") == "13") {
                                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(ItemHeld, "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                                    } else {
                                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    }
                                }
                            }
                            if (GS.GetSemiClass(ItemHeld, "id") == "11" || GS.GetSemiClass(ItemHeld, "id") == "12" || GS.GetSemiClass(ItemHeld, "id") == "13") {
                                foreach (Sprite GetSubSprite in SubItemIcons) {
                                    if (GetSubSprite.name.Substring(2) == GS.GetSemiClass(ItemHeld, "id")) {
                                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetSubSprite;
                                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    }
                                }
                            } else {
                                Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                            }
                        }

                        if (Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().sprite.name.Substring(0, 1) == "N") {
                            Inventory.transform.GetChild(InventoryCheck).transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                            Inventory.transform.GetChild(InventoryCheck).transform.GetChild(2).GetComponent<Text>().text = "";
                        } else if (Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().sprite.name.Substring(0, 1) == "B") {
                            Inventory.transform.GetChild(InventoryCheck).transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - (float.Parse(GS.GetSemiClass(ItemHeld, "va"), CultureInfo.InvariantCulture) / 100f);
                            Inventory.transform.GetChild(InventoryCheck).transform.GetChild(2).GetComponent<Text>().text = "";
                        } else if (Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().sprite.name.Substring(0, 1) == "C") {
                            Inventory.transform.GetChild(InventoryCheck).transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                            Inventory.transform.GetChild(InventoryCheck).transform.GetChild(2).GetComponent<Text>().text = GS.GetSemiClass(ItemHeld, "va");
                        }

                        if (GS.ExistSemiClass(ItemHeld, "sq") && int.Parse(GS.GetSemiClass(ItemHeld, "sq")) > 1) 
                            Inventory.transform.GetChild(InventoryCheck).transform.GetChild(2).GetComponent<Text>().text += " x"+GS.GetSemiClass(ItemHeld, "sq");

                    } else if (float.Parse(GS.GetSemiClass(ItemHeld, "id")) <= 0f) {
                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                        Inventory.transform.GetChild(InventoryCheck).transform.GetChild(2).GetComponent<Text>().text = "";
                    }
                    if (InventoryCheck == MainPlayer.CurrentItemHeld && Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().BlendForce < 0.75f) {
                        Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().BlendForce = Mathf.MoveTowards(Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().BlendForce, 0.75f, 0.1f * (Time.deltaTime * 50f));
                        Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().SetColor("");
                    } else if (InventoryCheck != MainPlayer.CurrentItemHeld && Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().BlendForce > 0f) {
                        Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().BlendForce = Mathf.MoveTowards(Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().BlendForce, 0f, 0.1f * (Time.deltaTime * 50f));
                        Inventory.transform.GetChild(InventoryCheck).GetComponent<HudColorControl>().SetColor("");
                    }
                }
            }

            if(MainPlayer.Inventory[MainPlayer.CurrentItemHeld] != PrevInvSlot || SwitchItemInfo > 0f){
                if(GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id") != GS.GetSemiClass(PrevInvSlot, "id")) SwitchItemInfo = 1f;
                PrevInvSlot = MainPlayer.Inventory[MainPlayer.CurrentItemHeld];
                SwitchItemInfo = Mathf.Clamp(SwitchItemInfo - 0.04f * (Time.deltaTime * 50f), 0f, 1f);
                SetItemInfo(PrevInvSlot);
            }
            // Inventory

            // Crosshairs
            string IDofitemheld = GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id");
            float CS = 0f;
            if (CSAlert[1] > 0f) {
                CSAlert[1] -= 0.02f;
            } else {
                CSAlert[0] = 0f;
            }
            if (CSWait[1] > 0f) {
                CSWait[1] -= 0.02f;
            } else {
                CSWait[0] = 0f;
            }
            if (IDofitemheld == "14" || IDofitemheld == "15" || IDofitemheld == "16" || IDofitemheld == "27" || IDofitemheld == "28" || IDofitemheld == "29" || IDofitemheld == "31" || IDofitemheld == "32" || IDofitemheld == "34" || IDofitemheld == "35" || IDofitemheld == "36" || IDofitemheld == "38" || IDofitemheld == "40" || IDofitemheld == "41" || IDofitemheld == "42" || IDofitemheld == "993" || IDofitemheld == "55" || IDofitemheld == "56" || IDofitemheld == "57" || IDofitemheld == "58" || IDofitemheld == "59" || IDofitemheld == "60" || IDofitemheld == "61" || IDofitemheld == "62" || IDofitemheld == "65" || IDofitemheld == "67" || IDofitemheld == "68" || IDofitemheld == "69" || IDofitemheld == "996" || IDofitemheld == "108" || IDofitemheld == "109" || IDofitemheld == "111" || IDofitemheld == "112" || IDofitemheld == "113" || IDofitemheld == "114" || IDofitemheld == "115" || IDofitemheld == "132" || IDofitemheld == "134" || IDofitemheld == "135" || IDofitemheld == "136" || IDofitemheld == "137" || IDofitemheld == "138" || IDofitemheld == "139") {
                if(RS.IsCausual) CS = Mathf.Clamp(RS.ReceiveGunSpred(int.Parse(GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id")), 0f, MainPlayer.GunSpreadPC).x, 1f, Mathf.Infinity);
                else CS = Mathf.Clamp(RS.ReceiveGunSpred(int.Parse(GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id")), MainPlayer.GetComponent<Rigidbody>().velocity.magnitude / MainPlayer.Speed, MainPlayer.GunSpreadPC).x, 1f, Mathf.Infinity);
                if (GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "at") == "103") {
                    CS = 0f;
                }
            }
            if (MainPlayer.ZoomValues[1] != MainPlayer.ZoomValues[2] || IDofitemheld == "64" || CSWait[0] > 0f) {
                CS = 0f;
            }
            Color CSColor = new Color(1f, 1f, 1f, 0.75f);
            foreach (Transform CheckCrosshair in Crosshair.transform) {
                if (CheckCrosshair.name == "Alert") {
                    if (CSAlert[0] == 0f) {
                        CSColor = new Color(1f, 1f, 1f, 0.75f);
                    } else if (CSAlert[0] == 1f) {
                        CSColor = new Color(1f, 0.75f, 0.75f, 0.75f);
                        GS.SetText(CheckCrosshair.GetComponent<Text>(), "Too Exhausted", "Zbyt Wyczerpany");
                    } else if (CSAlert[0] == 2f) {
                        CSColor = new Color(0.75f, 0.75f, 0.75f, 0.75f);
                        GS.SetText(CheckCrosshair.GetComponent<Text>(), "Gun's Empty", "Broń Nienaładowana");
                    }
                    CSColor = Color.Lerp(new Color(1f, 1f, 1f, 0.75f), CSColor, CSAlert[1]);
                    CheckCrosshair.GetComponent<Text>().color = new Color(CSColor.r, CSColor.g, CSColor.b, Mathf.Clamp(CSAlert[1], 0f, Crosshair.transform.GetChild(1).GetComponent<Image>().color.a));
                } else if (CheckCrosshair.name == "Wait"){
                    CheckCrosshair.GetComponent<Image>().fillAmount = CSWait[0];
                } else {
                    CheckCrosshair.GetComponent<RectTransform>().anchoredPosition3D = Vector3.MoveTowards(CheckCrosshair.GetComponent<RectTransform>().anchoredPosition3D, Vector3.zero + (CheckCrosshair.GetComponent<RectTransform>().right * (CS * 8f)), 3f);
                    CheckCrosshair.GetComponent<Image>().color = new Color(CSColor.r, CSColor.g, CSColor.b, Mathf.Clamp(Vector2.Distance(Vector2.zero, CheckCrosshair.GetComponent<RectTransform>().anchoredPosition) / 8f, 0f, 0.75f));
                }
            }
            // Crosshairs

            // Interact icon
            if (MainPlayer.InteractedGameobject != null && MainPlayer.InteractedGameobject.GetComponent<Interactions>().Options[MainPlayer.InteractedGameobject.GetComponent<Interactions>().ThisOption] != "") {
                InteractableIcon.transform.localScale = Vector3.one;
                GameObject i = MainPlayer.InteractedGameobject;
                InteractableIcon.transform.position = Vector3.Lerp(InteractableIcon.transform.position, MainCamera.GetComponent<Camera>().WorldToScreenPoint(i.transform.position + i.transform.forward * i.GetComponent<Interactions>().Offset.z + i.transform.right * i.GetComponent<Interactions>().Offset.x + i.transform.up * i.GetComponent<Interactions>().Offset.y), 0.5f * (Time.deltaTime * 50f));
                foreach (Transform CheckInterIcon in InteractableIcon.transform) {
                    if (CheckInterIcon.GetComponent<Text>() != null) {
                        CheckInterIcon.GetComponent<Text>().text = i.GetComponent<Interactions>().SetText();
                        if(i.tag == "Item"){
                            int CHIid = int.Parse(GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id"));
                            if(i.GetComponent<ItemScript>().CanBeFixed && GS.GetSemiClass(i.GetComponent<ItemScript>().Variables, "va") != "100" && (CHIid >= 88 & CHIid <= 90)){
                                CheckInterIcon.GetComponent<Text>().text += GS.SetString("\n[LMB] Repair ", "\n[LMB] Napraw ") + (int)float.Parse(GS.GetSemiClass(i.GetComponent<ItemScript>().Variables, "va"), CultureInfo.InvariantCulture);
                            } else if(i.GetComponent<ItemScript>().CanHaveAttachments && (CHIid >= 100 && CHIid <= 105 || CHIid == 14)){
                                CheckInterIcon.GetComponent<Text>().text += GS.SetString("\n[LMB] Attach ", "\n[LMB] Zamontuj ") + GS.itemCache[CHIid].getName();
                            }
                        }
                    } else if (CheckInterIcon.name == i.GetComponent<Interactions>().Icons[i.GetComponent<Interactions>().ThisOption]) {
                        CheckInterIcon.localScale = Vector3.one;
                    } else {
                        CheckInterIcon.localScale = Vector3.zero;
                    }
                }
            } else {
                InteractableIcon.transform.localScale = Vector3.zero;
                InteractableIcon.transform.position = ShowWindow.transform.position;
            }
            // Interact icon

            // Mission Info
            if (GS.ReceiveButtonPress("InformationTab", "Hold") > 0f) {
                WhileInformationsTab("Info");
                //GS.CameraFocus[2] = Mathf.MoveTowards(GS.CameraFocus[2], 100f, 10f * (Time.deltaTime * 50f));
                CameraBlur = Mathf.MoveTowards(CameraBlur, 10f, 0.5f * (Time.deltaTime * 50f));
                MainPlayer.FOVoffset = new float[] {Mathf.Lerp(MainPlayer.FOVoffset[0], -30f, 0.2f * (Time.deltaTime * 50f)), 0.2f};
            } else if (GS.ReceiveButtonPress("CraftingTab", "Hold") > 0f) {
                WhileInformationsTab("Craft");
                //GS.CameraFocus[2] = Mathf.MoveTowards(GS.CameraFocus[2], 100f, 10f * (Time.deltaTime * 50f));
                //ClearFocus = 1f;
                CameraBlur = Mathf.MoveTowards(CameraBlur, 10f, 0.5f * (Time.deltaTime * 50f));
                MainPlayer.FOVoffset = new float[] {Mathf.Lerp(MainPlayer.FOVoffset[0], -30f, 0.2f * (Time.deltaTime * 50f)), 0.2f};
            } else {
                WhileInformationsTab("");
            }

            // Damage Indicator
            if (DamageIndicator.GetComponent<Image>().color.a > 0f) {
                DamageIndicator.GetComponent<Image>().color -= new Color(0f, 0f, 0f, 0.001f);
                MainPlayer.DamageIndicator.transform.LookAt(DamagedPosition);
                DamageIndicator.transform.eulerAngles = new Vector3(0f, 0f, -MainPlayer.DamageIndicator.transform.localEulerAngles.y);
            }
            // Damage Indicator

            // Mob HealthBar
            Ray CheckForMob = new Ray(MainPlayer.LookDir.transform.position, MainPlayer.LookDir.transform.forward);
            RaycastHit CheckForMobHIT;
            if (Physics.Raycast(CheckForMob, out CheckForMobHIT, 100f, GS.IngoreMaskWP)) {
                if (CheckForMobHIT.collider.tag == "Mob") {
                    if (CheckForMobHIT.collider.gameObject.GetComponent<MobScript>().MobHealth[0] > 0f) {
                        MobHealthBar.SetActive(true);
                        MobHealthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = CheckForMobHIT.collider.GetComponent<MobScript>().MobHealth[0] / CheckForMobHIT.collider.GetComponent<MobScript>().MobHealth[1];
                        MobHealthBar.transform.GetChild(0).GetComponent<Image>().color = CheckForMobHIT.collider.GetComponent<MobScript>().MobColor;
                        MobHealthBar.transform.GetChild(1).GetComponent<Text>().text = CheckForMobHIT.collider.GetComponent<MobScript>().MobName + " - " + (int)CheckForMobHIT.collider.GetComponent<MobScript>().MobHealth[0];
                    }
                } else {
                    MobHealthBar.SetActive(false);
                }
            } else {
                MobHealthBar.SetActive(false);
            }
            // Mob HealthBar

            // Buffs
            float OffsetA = 0f;
            float OffsetB = 0f;
            string BuffText = "";
            GameObject CheckBuffObj = null;
            for (int SetBuff = 0; SetBuff < Buffs.transform.childCount; SetBuff++) {

                GameObject GetBuffA = Buffs.transform.GetChild(SetBuff).gameObject;
                GameObject GetBuffB = ITBuffs.transform.GetChild(SetBuff+1).gameObject;
                bool Hovered = false;
                if (GetBuffB.GetComponent<ButtonScript>().IsSelected) {
                    Hovered = true;
                    CheckBuffObj = GetBuffB;
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().color = Color.white;
                } else {
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().color = new Color(1f,1f,1f, 1f - BuffTextScroll[0]);
                }

                if (GetBuffA.name == "Bleeding" && MainPlayer.Bleeding > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Bleeding + " sec";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Bleeding (" + (int)MainPlayer.Bleeding + "sec)",
                        "Krwotok (" + (int)MainPlayer.Bleeding + "sek)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "Your blood is leaking, and you're losing health per second.",
                        "Twoja krew się leje, i tracisz punkty zdrowia co sekundę.");
                } else if (GetBuffA.name == "Hydration" && MainPlayer.Hydration > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Hydration + " sec";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Hydration (" + (int)MainPlayer.Hydration + " sec)",
                        "Nawodnienie (" + (int)MainPlayer.Hydration + " sek)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "Your stamina regenerates twice as fast.",
                        "Twoja energia regeneruje się z podwójną prędkością.");
                } else if (GetBuffA.name == "Infection" && MainPlayer.Infection > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Infection + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.GetComponent<GameScript>().SetString(
                        "Infection (" + (int)MainPlayer.Infection + "%)",
                        "Infekcja (" + (int)MainPlayer.Infection + "%)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.GetComponent<GameScript>().SetString(
                        "Decreases your maximum health.",
                        "Obniża twoją maksymalną ilość zdrowia.");
                } else if (GetBuffA.name == "Tiredness" && MainPlayer.Tiredness > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Tiredness + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.GetComponent<GameScript>().SetString(
                        "Tiredness (" + (int)MainPlayer.Tiredness + "%)",
                        "Zmęczenie (" + (int)MainPlayer.Tiredness + "%)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.GetComponent<GameScript>().SetString(
                        "Decreases your maximum energy. It might increase when you leave maps, and when you leave hungry.",
                        "Obniża twoją maksymalną ilość energi. Może się zwiększyć, gdy opuszczasz mapy, lub gdy opuszczasz je głodnym.");
                } else if (GetBuffA.name == "Radioactivity" && MainPlayer.Radioactivity > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Radioactivity + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Radioactivity (" + (int)MainPlayer.Radioactivity + "%)",
                        "Radioaktywność (" + (int)MainPlayer.Radioactivity + "%)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "You're covered in radioactive materials. Your health decreases, the more radioactive you are.",
                        "Jesteś pokryty radioaktywnym pyłem. Twoje zdrowie spada, z prędkością zależną od procentu radioaktywności");
                } else if (GetBuffA.name == "Coldness" && MainPlayer.Coldness > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Coldness + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Coldness (" + (int)MainPlayer.Coldness + "%)",
                        "Zimno (" + (int)MainPlayer.Coldness + "%)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "You're cold. If it'll be more than 50%, you'll begin to lose health.",
                        "Jest ci zimno. Jeżeli będzie więcej niż 50%, zaczniesz tracić zdrowie.");
                } else if (GetBuffA.name == "Adrenaline" && MainPlayer.Adrenaline > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Adrenaline + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Adrenaline (" + (int)MainPlayer.Adrenaline + " sec)",
                        "Adrenalina (" + (int)MainPlayer.Adrenaline + " sek)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "Your stamina is not drained.",
                        "Twoja energia się nie wyczerpuje.");
                } else if (GetBuffA.name == "Drunkenness" && MainPlayer.Drunkenness > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Drunkenness + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Drunkenness (" + (int)MainPlayer.Drunkenness + " sec)",
                        "Pijaństwo (" + (int)MainPlayer.Drunkenness + " sek)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "You deal more damage, and decreases coldness. Although, it slows you down.",
                        "Zadajesz więcej obrażeń, i zapobiega zamarzaniu. Duża ilość, może ciebie spowolnić.");
                } else if (GetBuffA.name == "BrokenBone" && MainPlayer.BrokenBone == 1) {
                    GetBuffA.SetActive(true);
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Broken Bones",
                        "Połamane Kości");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "You move slower, and you can't sprint nor jump. It won't heal itself.",
                        "Poruszasz się wolniej, i nie możesz ani biec ani skakać. Samo się nie uleczy.");
                } else if (GetBuffA.name == "Wet" && MainPlayer.Wet > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Wet + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Wet (" + (int)MainPlayer.Wet + "%)",
                        "Mokry (" + (int)MainPlayer.Wet + "%)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "You move sligtly slower (unless you're in water). If you'll be cold, you'll lose a lot of health.",
                        "Poruszasz się nieco wolniej (chyba że jesteś na wodzie). Jeśli będzie ci zimno, stracisz dużo zdrowia.");
                } else if (GetBuffA.name == "Hot" && MainPlayer.Hot > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Hot + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "Hot (" + (int)MainPlayer.Hot + "%)",
                        "Gorąco (" + (int)MainPlayer.Hot + "%)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "You're hot. If it'll be more than 25%, you'll begin to lose health. Use water to cool down.",
                        "Jest ci gorąco. Jeżeli będzie więcej niż 25%, zaczniesz tracić zdrowie. Użyj wody, by się ochłodzić");
                } else if (GetBuffA.name == "Fire" && MainPlayer.Fire > 0f) {
                    GetBuffA.SetActive(true);
                    GetBuffA.transform.GetChild(0).GetComponent<Text>().text = (int)MainPlayer.Fire + "%";
                    GetBuffA.GetComponent<RectTransform>().anchoredPosition = new Vector2(OffsetA, 0f);
                    OffsetA += 32f;

                    GetBuffB.SetActive(true);
                    GetBuffB.transform.GetChild(1).GetComponent<Text>().text = GS.SetString(
                        "FIRE (" + (int)MainPlayer.Fire + "%)",
                        "OGIEŃ (" + (int)MainPlayer.Fire + "%)");
                    GetBuffB.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, OffsetB);
                    OffsetB -= 32f;

                    if(Hovered) BuffText = GS.SetString(
                        "You're on fire! GET IN WATER OR SPLASH YOURSELF WITH SOMETHING!!!",
                        "Palisz się! WŁAŹ DO WODY ALBO SIĘ CZYMŚ OBLEJ!!!");
                } else {
                    GetBuffA.SetActive(false);
                    GetBuffB.SetActive(false);
                }

            }

            if (BuffText == ""){
                BuffTextScroll[0] = 0f;
                ITBuffs.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
            } else {
                BuffTextScroll[0] = Mathf.Clamp(BuffTextScroll[0] + 0.08f * (Time.deltaTime * 50f), 0f, 1f);
                ITBuffs.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = CheckBuffObj.GetComponent<RectTransform>().anchoredPosition + new Vector2(80f + (BuffTextScroll[0] * 32f), -16f);
                ITBuffs.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, BuffTextScroll[0]);
                ITBuffs.transform.GetChild(0).GetComponent<Text>().text = BuffText;
            }
            // Buffs

            // Dialog
            if (DialogedMob == null) {
                DialogMenu.transform.position = HideWindow.transform.position;
                DialogSetting = "Default";
            } else {
                DialogMenu.transform.position = ShowWindow.transform.position;

                MainPlayer.CantMove = 0.1f;
                MainPlayer.CantLook = 0.1f;
                MainPlayer.CantInteract = 0.5f;
                MainPlayer.CantUseItem = 0.5f;

                if (DialogedMob.GetComponent<MobScript>() != null) {
                    GS.GetComponent<GameScript>().SetText(DialogName, DialogedMob.GetComponent<MobScript>().MobName, DialogedMob.GetComponent<MobScript>().MobName);
                    if (DialogedMob.GetComponent<Interactions>().Options[DialogedMob.GetComponent<Interactions>().ThisOption] != "TalkTo" || DialogedMob.GetComponent<MobScript>().MobHealth[0] <= 0f) {
                        DialogedMob = null;
                    }
                } else if (DialogedMob.GetComponent<InteractableScript>() != null) {
                    GS.GetComponent<GameScript>().SetText(DialogName, DialogedMob.GetComponent<InteractableScript>().Name, DialogedMob.GetComponent<InteractableScript>().Name);
                }

                // Textes
                if (DialogSetting == "Default") {
                    GS.SetText(DialogDesc,
                        "Hello fellow survivor! How can I help you?",
                        "Witaj mój przyjacielu! Jak mogę ci pomóc?");
                    DialogOptions = new string[] { "TIP", "TRADE", "PLACES", "TREASURES", "EXIT", "", "", "" };
                } else if (DialogSetting == "VendingMachine") {
                    if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                        if (RS.RoundState == "BeforeWave") {
                            GS.SetText(DialogDesc,
                            "\n\nYou can buy these items for your earned money.",
                            "\n\nTutaj możesz kupować przedmioty, za zdobyte pieniądze.");
                            DialogOptions = new string[] { "1TRADE", "2TRADE", "3TRADE", "4TRADE", "5TRADE", "6TRADE", "EXIT", "" };
                        } else {
                            GS.SetText(DialogDesc,
                            "\n\nYou can't buy supplies during a wave!",
                            "\n\nNie możesz kupować zasobów podczas fali!");
                            DialogOptions = new string[] { "EXIT", "", "", "", "", "", "", "" };
                        }
                    } else {
                        GS.SetText(DialogDesc,
                        "\n\nYou can buy these items for money only.",
                        "\n\nTutaj możesz kupować przedmioty, tylko za pieniądze.");
                        DialogOptions = new string[] { "1TRADE", "2TRADE", "3TRADE", "4TRADE", "5TRADE", "6TRADE", "EXIT", "" };
                    }
                } else if (DialogSetting == "VendingMachineDone") {
                    if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                        GS.SetText(DialogDesc,
                        "\n\nOut of order. Supplies will be replenished after this wave.",
                        "\n\nAutomat przestał działać. Zasoby zostaną uzupełnione po tej fali.");
                        DialogOptions = new string[] { "EXIT", "", "", "", "", "", "", "" };
                    } else {
                        GS.SetText(DialogDesc,
                        "\n\nOut of order. Supplies have ran out.",
                        "\n\nAutomat przestał działać. Skończyły się zasoby.");
                        DialogOptions = new string[] { "EXIT", "", "", "", "", "", "", "" };
                    }
                } else if (DialogSetting == "Tip") {
                    int TipToView = (int)(DialogedMob.GetComponent<MobScript>().GeneratedValue * 4.9f);
                    if (TipToView == 0) {
                        GS.SetText(DialogDesc,
                            "It's obvious that maps, not only differ in layout, but also in type of biomes. The changes are usually visual, but there are few biomes that are special. These biomes are: wasteland, snowy area, and swamp. These biomes contain respectively: high amount of radiation, slowly rising coldness, and waters.",
                            "Oczywistym jest to, że mapy nie tylko różnią się wyglądem, ale także biomami. Biomy w większości różnią się tylko wyglądem i przedmiotami, jednak kilka biomów zasługuje na szczególną uwagę. Tymi biomami są: pustkowie, śnieżny teren, oraz bagno. Te biomy posiadają odpowiednio: bużą ilość promieniowanie, bardzo zimną temperaturę, oraz dużą ilość wody.");
                    } else if (TipToView == 1) {
                        GS.SetText(DialogDesc,
                            "Some maps may contain so called monuments. Those are specific buildings that contain special types of items called treasures, which have paranormal abilities. You can trade treasures with other survivors like me, for a lot of score. But be careful, monuments are protected by guards, which will kill you if you dare enter their monuments!",
                            "Niektóre mapy mogą posiadać tak zwane monumenty. Specyficzne rodzaje budynków, które posiadają specjalne typy przedmiotów, zwane skarbami, które posiadają paranormalne cechy. Możesz nimi handlować z innymi niedobitkami, w zamian za dużą ilość punktów. Jednak bądź ostrożny, gdyż monumenty są chronione przez strażników, którzy cię zabiją jeśli wejdziesz na ich teren!");
                    } else if (TipToView == 2) {
                        GS.SetText(DialogDesc,
                            "We survivors, aren't the only ones roaming around those lands. Besides us, also bandits. They're hostile, and they come in many different types. They differ in weaponary and outfits. The weapons they use are: machetes, baseball bats, or guns. Their outfits also determine their stats: gray hoodies mean they're fast, camo outfit mean they have more health, and suits mean they can detect you from bigger distance. They also come in hazmat suits, which means they're all of the previously mentioned.",
                            "My niedobitkowie, nie jesteśmy jedynymi ludzmi przemieżającymi the ziemie. Poza nami, są też bandyci. Są groźni, i różnią się od siebie bardzo. Różnią się uzbrojeniem i ubraniem. Ich bronie to: maczety, kije basebal'owe, i bronie palne. Ich ubiory także świadczą o tym kim są: ci w dresach są szybcy, ci w ubraniach moro mają więcej zdrowia, a ci w garniturach mogą cię wykryć z większej odległości. Także mogą być ubrani w kombinezonach hazmatycznych, a to oznacza, że mają te wszystkie cechy na raz.");
                    } else if (TipToView == 3) {
                        GS.SetText(DialogDesc,
                            "We survivors, aren't the only ones roaming around those lands. Besides us, also mutants. They're hostile, and they come in many different types. They mostly differ in their types; there are: normal mutants, fast mutants, strong mutants, acid mutants, and phantom mutants. Acid mutants spit acid at you, while phantom mutants are very hard to detect, with dim blue halo being the only indication of their existence, the rest of mutants are self-explanatory. They'll also increase their basic stats like health/speed/detection range, with each round.",
                            "My niedobitkowie, nie jesteśmy jedynymi ludzmi przemieżającymi the ziemie. Poza nami, są też bandyci. Są groźni, i różnią się od siebie bardzo. Głównie się różnia typami; mutanci mogą być: normalni, szybcy, silni, trujący, lub fantomowi. Trujący mutańci plują na ciebie kwasem, natomiast fantomowi mutanci są trudni do wykrycia, a niebieski słaby dym jest jedynym śladem ich istnienia, pozostałe rodzaje mutantów mówią same za siebie. Z każdą rundą, podstawowe statystyki mutantów takie jak zdrowie/szybkość/promień wykrywania, będą rosnąć.");
                    } else if (TipToView == 4) {
                        GS.SetText(DialogDesc,
                            "There are many items you can find, but most of them usually can be classified either as food, medicine, or weaponary. But there are also utilities, items that have unique abilities. I won't tell you all of them, but the ones that are deserving of attention are: sleeping bags, grappling hooks, flares, outfits, and money. They respectively can be used to: remove all tiredness, creating rope bridges, light your way and warm you, change your stats like health/speed/inventory space, and can be used in trading, when you don't have the item the trader wants.",
                            "Jest wiele przedmiotów jakie możesz znaleść, jednak większość klasyfikuje się jako jedzenie, lekarstwa, albo uzbrojenie. Jednakże, istnieją także narzędzia, są to przedmioty z wyjątkowymi zastosowaniami. Nie będe mówił o wszystkich, jednak tymi wartymi wspomnienia są: śpiwory, haki mocujące, flary, ubrania, i pieniądze. Mogą one być użyte odpowiednio do: usuwania zmęczenia, tworzenia mostów liniowych, świecenia i ogrzewania, zmiany statystyk takich jak zdrowie/prędkość/ilość miejsc w inwentarzu, oraz mogą być użyte jako środka płatniczego.");
                    }
                    DialogOptions = new string[] { "BACK", "EXIT", "", "", "", "", "", "" };
                } else if (DialogSetting == "Trade") {
                    GS.SetText(DialogDesc,
                        "Here are my goods, the choice belongs to you!",
                        "Oto moje dobra, wybierz sobie ten idealny!");
                    DialogOptions = new string[] { "1TRADE", "2TRADE", "3TRADE", "4TRADE", "BACK", "EXIT", "", "" };
                } else if (DialogSetting == "TradeNot") {
                    GS.SetText(DialogDesc,
                        "You can't buy that!",
                        "Nie możesz tego kupić!");
                    DialogOptions = new string[] { "1TRADE", "2TRADE", "3TRADE", "4TRADE", "BACK", "EXIT", "", "" };
                } else if (DialogSetting == "TradeGood") {
                    GS.SetText(DialogDesc,
                        "Good choice!",
                        "Dobry wybór!");
                    DialogOptions = new string[] { "1TRADE", "2TRADE", "3TRADE", "4TRADE", "BACK", "EXIT", "", "" };
                } else if (DialogSetting == "PlacesKnown") {
                    GS.SetText(DialogDesc,
                        "Well, I guess I know some locations...",
                        "Cuż, myślę że znam kilka takich miejsc...");
                    DialogOptions = new string[] { "BACK", "EXIT", "", "", "", "", "", "" };
                } else if (DialogSetting == "PlacesUnknown") {
                    GS.SetText(DialogDesc,
                        "You know all of them.",
                        "Znasz je wszystkie.");
                    DialogOptions = new string[] { "BACK", "EXIT", "", "", "", "", "", "" };
                } else if (DialogSetting == "TreasureGot") {
                    GS.SetText(DialogDesc,
                        "Ah, brilliant things, aren't they? Here, take this score, in exchange for those!",
                        "Ah, jakie piękne błyskotki, czyż nie? Proszę, weź te punkty, za te chojne dary!");
                    DialogOptions = new string[] { "BACK", "EXIT", "", "", "", "", "", "" };
                } else if (DialogSetting == "TreasureNot") {
                    GS.SetText(DialogDesc,
                        "You think I'm stupid? Find some treasures first, then we'll talk about it!",
                        "Masz mnie za idiotę? Znjadź jakieś skarby, później porozmawiamy o tym!");
                    DialogOptions = new string[] { "BACK", "EXIT", "", "", "", "", "", "" };
                }

                // DialogButtons
                for (int CheckButton = 0; CheckButton < 8; CheckButton++) {
                    if (DialogedMob != null) {

                        GameObject CheckedButton = DialogOptionsButtons[CheckButton];
                        CheckedButton.name = DialogOptions[CheckButton];
                        if (CheckedButton.name == "TIP") {
                            CheckedButton.GetComponent<ButtonScript>().Active = true;
                            GS.SetText(CheckedButton.GetComponent<Text>(), "- Any tips for me?", "- Jakieś wskazówki?");
                            if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                                DialogSetting = "Tip";
                            }
                        } else if (CheckedButton.name == "TRADE") {
                            CheckedButton.GetComponent<ButtonScript>().Active = true;
                            GS.SetText(CheckedButton.GetComponent<Text>(), "- Let's trade!", "- Pokaż mi swoje towary!");
                            if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                                DialogSetting = "Trade";
                            }
                        } else if (CheckedButton.name == "1TRADE" || CheckedButton.name == "2TRADE" || CheckedButton.name == "3TRADE" || CheckedButton.name == "4TRADE" || CheckedButton.name == "5TRADE" || CheckedButton.name == "6TRADE") {
                            CheckedButton.GetComponent<ButtonScript>().Active = true;
                            Vector2 TradeOptions = Vector2.zero;
                            if (DialogedMob.GetComponent<MobScript>() != null) {
                                TradeOptions = DialogedMob.GetComponent<MobScript>().TradeOptions[int.Parse(CheckedButton.name.Substring(0, 1)) - 1];
                            } else {
                                if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                                    TradeOptions.x = DialogedMob.GetComponent<InteractableScript>().TradeOptions[int.Parse(CheckedButton.name.Substring(0, 1)) - 1];
                                    TradeOptions.y = DialogedMob.GetComponent<InteractableScript>().TradePrices[int.Parse(CheckedButton.name.Substring(0, 1)) - 1];
                                } else {
                                    TradeOptions.x = DialogedMob.GetComponent<InteractableScript>().TradeOptions[int.Parse(CheckedButton.name.Substring(0, 1)) - 1];
                                    TradeOptions.y = 52f;
                                }
                            }

                            if (TradeOptions.x == -1f) {
                                GS.SetText(CheckedButton.GetComponent<Text>(),
                                "SOLD",
                                "SPRZEDANE");
                            } else {
                                if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1" && DialogedMob.GetComponent<InteractableScript>() != null) {
                                    GS.SetText(CheckedButton.GetComponent<Text>(),
                                    "- Get " + GS.itemCache[(int)TradeOptions.x].getName() + " for " + TradeOptions.y + "$",
                                    "- Dostaniesz " + GS.itemCache[(int)TradeOptions.x].getName() + " za " + TradeOptions.y + "$");
                                } else {
                                    GS.SetText(CheckedButton.GetComponent<Text>(),
                                    "- Get " + GS.itemCache[(int)TradeOptions.x].getName() + " for " + GS.itemCache[(int)TradeOptions.y].getName(),
                                    "- Dostaniesz " + GS.itemCache[(int)TradeOptions.x].getName() + " za " + GS.itemCache[(int)TradeOptions.y].getName());
                                }
                            }
                            int GotTradedItem = -1;
                            for (int CheckINV = 0; CheckINV <= 9; CheckINV++) if (GS.GetSemiClass(MainPlayer.Inventory[CheckINV], "id") == TradeOptions.y.ToString()) {
                                GotTradedItem = CheckINV;
                                break;
                            }
                            if (GotTradedItem == -1) {
                                for (int CheckINV = 0; CheckINV <= 9; CheckINV++) if (GS.GetSemiClass(MainPlayer.Inventory[CheckINV], "id") == "52") {
                                    GotTradedItem = CheckINV;
                                    break;
                                }
                            }
                            if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1" && CheckedButton.GetComponent<ButtonScript>().IsSelected == true && TradeOptions.x > -1 && GS.Money >= TradeOptions.y && Input.GetMouseButtonDown(0)) {
                                MainPlayer.InvGet(GS.itemCache[(int)TradeOptions.x].startVariables, 0);
                                GS.Mess(GS.SetString("Item purchased!", "Kupiono ten przedmiot!"), "Buy");
                                DialogedMob.GetComponent<InteractableScript>().TradeOptions[int.Parse(CheckedButton.name.Substring(0, 1)) - 1] = -1;
                                GS.Money -= (int)TradeOptions.y;
                            } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0" && CheckedButton.GetComponent<ButtonScript>().IsSelected == true && TradeOptions.x > -1 && GotTradedItem != -1 && Input.GetMouseButtonDown(0)) {
                                if (DialogedMob.GetComponent<MobScript>() != null) {
                                    GS.Mess(GS.SetString("Items traded!", "Zdobyto ten przedmiot!"), "Buy");
                                    DialogSetting = "TradeGood";
                                    DialogedMob.GetComponent<MobScript>().TradeOptions[int.Parse(CheckedButton.name.Substring(0, 1)) - 1] = new Vector2(-1f, 0f);
                                    GS.PS.AchProg("Ach_Merchant", "/+-1");
                                    RS.SetScore("Trade_", "/+1");
                                } else {
                                    GS.Mess(GS.SetString("Item purchased!", "Kupiono ten przedmiot!"), "Buy");
                                    DialogedMob.GetComponent<InteractableScript>().TradeOptions[int.Parse(CheckedButton.name.Substring(0, 1)) - 1] = -1;
                                }
                                MainPlayer.InvGet(GotTradedItem.ToString(), 1);//MainPlayer.Inventory[GotTradedItem] = GS.ReceiveItemVariables(TradeOptions.x);

                                GS.Score += 50;

                            } else if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && GotTradedItem == -1 && Input.GetMouseButtonDown(0)) {
                                if (DialogedMob.GetComponent<MobScript>() != null) {
                                    DialogSetting = "TradeNot";
                                }
                            }
                        } else if (CheckedButton.name == "PLACES") {
                            CheckedButton.GetComponent<ButtonScript>().Active = true;
                            GS.SetText(CheckedButton.GetComponent<Text>(), "- Any special locations?", "- Jakieś specjalne miejsca?");
                            if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                                bool SaidSomething = false;
                                DialogedMob.GetComponent<MobScript>().ToldPlaces = true;
                                foreach (GameObject FoundInteract in GameObject.FindGameObjectsWithTag("Interactable")) {
                                    if (FoundInteract.GetComponent<InteractableScript>().Discovered == false && FoundInteract.GetComponent<InteractableScript>().Variables.x == 2f) {
                                        FoundInteract.GetComponent<InteractableScript>().Discovered = true;
                                        GS.Score += 50;
                                        SaidSomething = true;
                                    }
                                }
                                foreach (GameObject FoundLand in GameObject.FindGameObjectsWithTag("Land")) {
                                    if (FoundLand.name.Substring(2, 1) == "M" && FoundLand.name.Substring(0, 1) == "0") {
                                        FoundLand.name = "1" + FoundLand.name.Substring(1);
                                        GS.Score += 50;
                                        SaidSomething = true;
                                    }
                                }
                                if (SaidSomething == true) {
                                    DialogSetting = "PlacesKnown";
                                    GS.Mess(GS.SetString("You now know of some new places", "Poznałeś kilka nowych miejsc"), "Draw");
                                } else {
                                    DialogSetting = "PlacesUnknown";
                                }
                            }
                        } else if (CheckedButton.name == "TREASURES") {
                            CheckedButton.GetComponent<ButtonScript>().Active = true;
                            GS.SetText(CheckedButton.GetComponent<Text>(), "- I have some treasures for you!", "- Mam dla ciebie skarby!");
                            List<int> TreausresGot = new List<int>();
                            for (int CheckINV = 0; CheckINV <= 9; CheckINV++) {
                                if (float.Parse(GS.GetSemiClass(MainPlayer.Inventory[CheckINV], "id")) >= 990f && float.Parse(GS.GetSemiClass(MainPlayer.Inventory[CheckINV], "id")) <= 999f) {
                                    TreausresGot.Add(CheckINV);
                                }
                            }
                            if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && TreausresGot.ToArray().Length > 0f && Input.GetMouseButtonDown(0)) {
                                DialogSetting = "TreasureGot";
                                GS.Mess(GS.SetString("Treasures sold!", "Skarby sprzedane!"), "Buy");
                                foreach (int SellTreasure in TreausresGot)
                                {
                                    GS.Score += 10000;
                                    MainPlayer.InvGet(SellTreasure.ToString(), 1);//MainPlayer.Inventory[SellTreasure] = "id0;";
                                    GS.PS.AchProg("Ach_Collectioner", "/+-1");
                                    RS.SetScore("TreasureSold_", "/+1");
                                }
                            } else if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && TreausresGot.ToArray().Length <= 0f && Input.GetMouseButtonDown(0)) {
                                DialogSetting = "TreasureNot";
                            }
                        } else if (CheckedButton.name == "BACK") {
                            CheckedButton.GetComponent<ButtonScript>().Active = true;
                            GS.SetText(CheckedButton.GetComponent<Text>(), "< Back", "< Wróć");
                            if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                                DialogSetting = "Default";
                            }
                        } else if (CheckedButton.name == "EXIT") {
                            CheckedButton.GetComponent<ButtonScript>().Active = true;
                            GS.SetText(CheckedButton.GetComponent<Text>(), "X Goodbye", "X Dowidzenia");
                            if (CheckedButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                                DialogSetting = "Default";
                                DialogedMob = null;
                            }
                        } else {
                            CheckedButton.GetComponent<ButtonScript>().Active = false;
                            CheckedButton.GetComponent<Text>().text = "";
                        }

                    }
                }

            }
            // Dialog

            // Hint
            if (ClearHint > 0f) {
                ClearHint -= 0.02f * (Time.deltaTime * 50f);
                if (!(Input.GetButton("Information Tab") && (float.Parse(GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id")) > 0f || float.Parse(GS.GetSemiClass(MainPlayer.Equipment[0], "id")) > 0f))) {
                    HintText.color = new Color32(255, 255, 255, (byte)Mathf.Clamp((128f * ClearHint), 0f, 200f));
                } else {
                    HintText.color = new Color(1f, 1f, 1f, 0f);
                }
            } else {
                if (HintScan > 0f) {
                    HintScan -= 0.02f * (Time.deltaTime * 50f);
                } else if (RoundStartInfo.GetComponent<Image>().color.a <= 0f) {
                    ScanHint("");
                }
            }

        }

    }

    void SetItemInfo(string ItemInfos){

        string[] Infos = {""};
        Color32[] InfoColors = {};

        // ReceiveInfos
        switch(int.Parse(GS.GetSemiClass(ItemInfos, "id"), CultureInfo.InvariantCulture)){
            case 0: Infos = new string[]{}; break;
            case  14: case  15: case  16: case  27: case  28: case  993: case 88: case 95: case 96: case 132: case 134: case 136: case 138:
                // Mele weapons
                Infos = new string[]{
                    GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "id"))].getName(),
                    GS.SetString("Durability: ", "Wytrzymałość: ") + (int)float.Parse(GS.GetSemiClass(ItemInfos, "va"), CultureInfo.InvariantCulture) + "%"};
                break;
            case 2: case 68: case 127: case 128: case 130:
                // Power stuff
                Infos = new string[]{
                    GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "id"))].getName(),
                    GS.SetString("Power: ", "Moc: ") + (int)float.Parse(GS.GetSemiClass(ItemInfos, "va"), CultureInfo.InvariantCulture) + "%"};
                break;
            case 89: case 991:
                Infos = new string[]{
                    GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "id"))].getName(),
                    GS.SetString("Uses: ", "Użycia: ") + (int)(float.Parse(GS.GetSemiClass(ItemInfos, "va"), CultureInfo.InvariantCulture) / 30f) };
                break;
            case 98: case 124:
                Infos = new string[]{
                    GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "id"))].getName(),
                    GS.SetString("Uses: ", "Użycia: ") + (int)(float.Parse(GS.GetSemiClass(ItemInfos, "va"), CultureInfo.InvariantCulture) / 10f) };
                break;
            case 29: case 31: case 32: case 34: case 35: case 36: case 38: case 40: case 41: case 42: case 55: case 56: case 57: case 58: case 59: case 60: case 61: case 62: case 64: case 65: case 113: case 135: case 137:
                // Guns
                string SpareAmmo = "0";
                if(GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1" || int.Parse(GS.GetSemiClass(ItemInfos, "id"), CultureInfo.InvariantCulture) == 996) {
                    SpareAmmo = "∞";
                } else {
                    for(int GetSA = 0; GetSA < MainPlayer.MaxInventorySlots; GetSA ++){
                        switch(int.Parse(GS.GetSemiClass(ItemInfos, "id"), CultureInfo.InvariantCulture)){
                            case 29: case 31: case 135:
                                if(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "id") == "30")
                                SpareAmmo = (int.Parse(SpareAmmo) + int.Parse(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "va"))).ToString();
                                break;
                            case 32: case 34: case 35: case 40: case 56: case 65:
                                if(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "id") == "33")
                                SpareAmmo = (int.Parse(SpareAmmo) + int.Parse(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "va"))).ToString();
                                break;
                            case 36: case 41: case 55: case 58:
                                if(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "id") == "37")
                                SpareAmmo = (int.Parse(SpareAmmo) + int.Parse(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "va"))).ToString();
                                break;
                            case 38: case 42: case 57: case 59: case 60:
                                if(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "id") == "39")
                                SpareAmmo = (int.Parse(SpareAmmo) + int.Parse(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "va"))).ToString();
                                break;
                            case 62: case 64:
                                if(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "id") == "63")
                                SpareAmmo = (int.Parse(SpareAmmo) + int.Parse(GS.GetSemiClass(MainPlayer.Inventory[GetSA], "va"))).ToString();
                                break;
                        }
                    }
                }
                
                if(GS.ExistSemiClass(ItemInfos, "at")) 
                    Infos = new string[]{
                        GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "id"))].getName(),
                        GS.SetString("Ammo: ", "Ammunicja: ") + int.Parse(GS.GetSemiClass(ItemInfos, "va"), CultureInfo.InvariantCulture) + " / " + SpareAmmo,
                        GS.SetString("Attachment: ", "Dodatek: ") + GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "at"))].getName()};
                else
                    Infos = new string[]{
                        GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "id"))].getName(),
                        GS.SetString("Ammo: ", "Ammunicja: ") + int.Parse(GS.GetSemiClass(ItemInfos, "va"), CultureInfo.InvariantCulture) + " / " + SpareAmmo};

                break;
            default: Infos = new string[]{GS.itemCache[int.Parse(GS.GetSemiClass(ItemInfos, "id"))].getName(),}; break;
        }

        for(int SetInfos = 0; SetInfos < 5; SetInfos++){
            if(SetInfos < Infos.Length)
                ItemInfo.GetChild(SetInfos).GetComponent<Text>().text = Infos[SetInfos];
            else
                ItemInfo.GetChild(SetInfos).GetComponent<Text>().text = "";
            if(SetInfos < InfoColors.Length)
                ItemInfo.GetChild(SetInfos).GetComponent<Text>().color = InfoColors[SetInfos];
            else
                ItemInfo.GetChild(SetInfos).GetComponent<Text>().color = new Color32(255, 255, 255, 128);
            Color SetAlpha = ItemInfo.GetChild(SetInfos).GetComponent<Text>().color;
            SetAlpha.a *= 1f - SwitchItemInfo;
            ItemInfo.GetChild(SetInfos).GetComponent<Text>().color = SetAlpha;
        }

        ItemInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mathf.Lerp(-16f, 0f, SwitchItemInfo), 80f + ((Infos.Length - 1) * 16f) );

    }

    void WhileInformationsTab(string WhichMenu) {

        if (WhichMenu != "") {

            if (ITShown != WhichMenu) {
                ITShown = WhichMenu;
                PlaySoundBank("S_ITshow");
                if (ITShown == "Craft") {
                    SetCraftOptions();
                }
            }

            if (ITBG.GetComponent<HudColorControl>().Alpha < 0.75f) {
                ITBG.GetComponent<HudColorControl>().Alpha = Mathf.MoveTowards(ITBG.GetComponent<HudColorControl>().Alpha, 0.75f, 0.06f * (Time.deltaTime * 50f));
                ITBG.GetComponent<HudColorControl>().SetColor("");
            }

            ITicons.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(400f, 400f), Vector2.zero, ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);
            foreach (Transform GetIcon in ITicons.transform) {
                if (GetIcon.name == WhichMenu) {
                    GetIcon.transform.localScale = Vector3.one;
                } else {
                    GetIcon.transform.localScale = Vector3.zero;
                }
            }

            if (WhichMenu == "Info") {
                CDTdisplaye[2] = -1f;
                CDTdisplaye[0] = 0f;
                ITMenuInfo.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuInfo.GetComponent<RectTransform>().position, ShowWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
                ITMenuCraft.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuCraft.GetComponent<RectTransform>().position, HideWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
                MinimapRefresh("Map");
                // Infos
                if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0") {
                    float TimeOfDayA = RS.TimeOfDay[1];
                    string Weather = "";
                    if (RS.Weather == 0) {
                        Weather = GS.SetString("clear sky", "czyste niebo");
                    } else if (RS.Weather == 1) {
                        Weather = GS.SetString("mostly clear sky", "prawie czyste niebo");
                    } else if (RS.Weather == 2) {
                        Weather = GS.SetString("partly cloudy", "częściowe zachmurzenie");
                    } else if (RS.Weather == 3) {
                        Weather = GS.SetString("cloudy", "zachmurzenie");
                    } else if (RS.Weather == 4) {
                        Weather = GS.SetString("raining", "deszcz");
                    } else if (RS.Weather == 5) {
                        Weather = GS.SetString("thunderstorm", "burza");
                    }
                    string Day = "";
                    if (RS.TimeOfDay[0] == 1 || RS.TimeOfDay[0] == 2) {
                        Day = GS.SetString("Day", "Dzień");
                    } else if (RS.TimeOfDay[0] == 3) {
                        Day = GS.SetString("Dusk", "Zmierzch");
                    } else if (RS.TimeOfDay[0] == 0) {
                        Day = GS.SetString("Night", "Noc");
                    }
                    GS.SetText(ITRoundInfo.transform.GetChild(0).GetComponent<Text>(),
                        "Round " + GS.Round + "\n" + GS.DisplayTime(RS.TimeOfDay[1]) + " (" + Day + ") - " + RS.GotTerrain.GetComponent<BiomeInfo>().BiomeName[0] + " - " + Weather,
                        "Runda " + GS.Round + "\n" + GS.DisplayTime(RS.TimeOfDay[1]) + " (" + Day + ") - " + RS.GotTerrain.GetComponent<BiomeInfo>().BiomeName[1] + " - " + Weather);
                } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                    GS.SetText(ITRoundInfo.transform.GetChild(0).GetComponent<Text>(),
                        "Wave " + GS.Round + " - " + RS.GotTerrain.GetComponent<MapInfo>().MapName[0],
                        "Fala " + GS.Round + " - " + RS.GotTerrain.GetComponent<MapInfo>().MapName[1]);
                }

                // Infos

                // Details
                ITDetails.transform.GetChild(0).GetComponent<Text>().text = "";
                if (RS.RoundState == "Normal") {
                    GS.SetText(ITDetails.transform.GetChild(0).GetComponent<Text>(), ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Nuke will hit in " + GS.DisplayTime(RS.RoundTime) + ", find an exit before it happens!\n", ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Bomba atomowa uderzy za " + GS.DisplayTime(RS.RoundTime) + " sekund, znajdź wyjście przed tym!\n");
                } else if (RS.RoundState == "Nuked") {
                    GS.SetText(ITDetails.transform.GetChild(0).GetComponent<Text>(), ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- NUKE HAS BEEN DROPPED, RUN TO THE EXTI!!!\n", ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- BOMBA ATOMOWA UDERZYŁA, UCIEKAJ DO WYJŚCIA!!!\n");
                } else if (RS.RoundState == "TealState") {
                    GS.SetText(ITDetails.transform.GetChild(0).GetComponent<Text>(), ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- You are in a teal state. There aren't any dangers here, and you're invisible.\n", ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Jesteś pod wpływem cyjanu. Nie ma tutaj żadnych zagrożeń, a sam jesteś nieśmiertelny.\n");
                } else if (RS.RoundState == "BeforeWave") {
                    GS.SetText(ITDetails.transform.GetChild(0).GetComponent<Text>(),
                        ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Prepare for next wave! Incoming horde stats:\n" + "Amount " + RS.HordeVariables[0] + " (" + RS.HordeVariables[1] + " at once), Mutants power " + RS.HordeVariables[2] + "%, Amount of special mutants " + RS.HordeVariables[3] + "%\n",
                        ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Przygotuj się na następną falę! Statystyki hordy:\n" + "Ilość " + RS.HordeVariables[0] + " (" + RS.HordeVariables[1] + " at once), Moc mutantów " + RS.HordeVariables[2] + "%, Ilość specialnych mutantów " + RS.HordeVariables[3] + "%\n");
                } else if (RS.RoundState == "HordeWave") {
                    GS.SetText(ITDetails.transform.GetChild(0).GetComponent<Text>(),
                        ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Survive the wave! Horde stats:\n" + "Amount " + RS.HordeVariables[0] + " (" + RS.HordeVariables[1] + " at once), Mutants power " + RS.HordeVariables[2] + "%, Amount of special mutants " + RS.HordeVariables[3] + "%\n",
                        ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Przetrwaj tą falę! Statystyki hordy:\n" + "Ilość " + RS.HordeVariables[0] + " (" + RS.HordeVariables[1] + " at once), Moc mutantów " + RS.HordeVariables[2] + "%, Ilość specialnych mutantów " + RS.HordeVariables[3] + "%\n");
                }
                if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                    GS.SetText(ITDetails.transform.GetChild(0).GetComponent<Text>(), ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Gather money, ammo, weaponary, and survive as many waves as possible!\n", ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Zbieraj pieniądze, ammunicję, uzbrojenie, i przeżyj jak najwięcej fal!\n");
                } else {
                    GS.SetText(ITDetails.transform.GetChild(0).GetComponent<Text>(), ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Gather resources, food, weaponary, and watch out for any possible dangers!\n", ITDetails.transform.GetChild(0).GetComponent<Text>().text + "- Zbieraj zasoby, jedzenie, uzbrojenie, oraz uważaj na wszelkie zagrożenia!\n");
                }
                // Details
                // Item Held
                if (GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id") == "0") {
                    ITItemHeldInfo.SetActive(false);
                } else {
                    ITItemHeldInfo.SetActive(true);
                    string ItemHeld = MainPlayer.Inventory[MainPlayer.CurrentItemHeld];
                    if (float.Parse(GS.GetSemiClass(ItemHeld, "id")) > 0f) {
                        if (ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite.name.Substring(1) != GS.GetSemiClass(ItemHeld, "id") || ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color.a <= 0f) {
                            foreach (Sprite GetSprite in ItemIcons) {
                                if (GetSprite.name.Substring(1) == GS.GetSemiClass(ItemHeld, "id")) {
                                    ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = GetSprite;
                                    if (GS.GetSemiClass(ItemHeld, "id") == "11" || GS.GetSemiClass(ItemHeld, "id") == "12" || GS.GetSemiClass(ItemHeld, "id") == "13") {
                                        ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(ItemHeld, "cl")) / 10f, 1f, 1f);
                                    } else {
                                        ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    }
                                }
                            }
                            if (GS.GetSemiClass(ItemHeld, "id") == "11" || GS.GetSemiClass(ItemHeld, "id") == "12" || GS.GetSemiClass(ItemHeld, "id") == "13") {
                                foreach (Sprite GetSubSprite in SubItemIcons) {
                                    if (GetSubSprite.name.Substring(2) == GS.GetSemiClass(ItemHeld, "id")) {
                                        ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetSubSprite;
                                        ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                                    }
                                }
                            } else {
                                ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                            }
                        }
                    } else if (float.Parse(GS.GetSemiClass(ItemHeld, "id")) <= 0f) {
                        ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        ITItemHeldInfo.transform.GetChild(0).transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                    }
                    ITItemHeldInfo.transform.GetChild(1).GetComponent<Text>().text = GS.itemCache[int.Parse(GS.GetSemiClass(ItemHeld, "id"))].getName();
                    ITItemHeldInfo.transform.GetChild(2).GetComponent<Text>().text = GS.itemCache[int.Parse(GS.GetSemiClass(ItemHeld, "id"))].getDesc();
                }
                // Item Held

                // Equipment
                bool Reseteq = true;
                Equipment.transform.GetChild(0).GetComponent<Text>().text = "";
                for (int CheckEq = 0; CheckEq < 4; CheckEq++) {
                    GameObject CheckEqObj = Equipment.transform.GetChild(CheckEq + 1).gameObject;
                    if (float.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id")) > 0f) {
                        CheckEqObj.transform.localScale = Vector3.one;
                        CheckEqObj.GetComponent<ButtonScript>().Active = true;

                        foreach (Sprite GetItemImage in ItemIcons) {
                            if (GetItemImage.name.Substring(1) == GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id")) {
                                CheckEqObj.transform.GetChild(0).GetComponent<Image>().sprite = GetItemImage;
                                if (GetItemImage.name.Substring(0, 1) == "B") {
                                    CheckEqObj.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f - float.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "va"), CultureInfo.InvariantCulture) / 100f;
                                    CheckEqObj.transform.GetChild(2).GetComponent<Text>().text = "";
                                } else if (GetItemImage.name.Substring(0, 1) == "C") {
                                    CheckEqObj.transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                                    CheckEqObj.transform.GetChild(2).GetComponent<Text>().text = GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "va");
                                } else {
                                    CheckEqObj.transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                                    CheckEqObj.transform.GetChild(2).GetComponent<Text>().text = "";
                                }
                            }
                        }

                        if (CheckEqObj.GetComponent<ButtonScript>().IsSelected == true) {
                            Reseteq = false;
                            if (EqTextScroll[1] != CheckEq) {
                                Reseteq = true;
                                EqTextScroll[1] = CheckEq;
                            }
                            EqTextScroll[0] = Mathf.Clamp(EqTextScroll[0] + 0.08f * (Time.deltaTime * 50f), 0f, 1f);
                            Equipment.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = CheckEqObj.GetComponent<RectTransform>().anchoredPosition - new Vector2(80f + (EqTextScroll[0] * 32f), 0f);
                            Equipment.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f, EqTextScroll[0]);
                            if (GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id") == "53") {
                                GS.SetText(Equipment.transform.GetChild(0).GetComponent<Text>(),
                                    GS.itemCache[int.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id"))].getName() + "\nLMB - Unequip   RMB - Turn on/off",
                                    GS.itemCache[int.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id"))].getName() + "\nLPM - Zdejmij   PPM - Włącz/wyłącz");
                            } else {
                                GS.SetText(Equipment.transform.GetChild(0).GetComponent<Text>(),
                                    GS.itemCache[int.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id"))].getName() + "\nLMB - Unequip",
                                    GS.itemCache[int.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id"))].getName() + "\nLPM - Zdejmij");
                            }

                            if (Input.GetMouseButtonDown(0)) {
                                GS.Mess(GS.SetString(GS.itemCache[int.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id"))].getName() + " unequipped", "Zdjęto: " + GS.itemCache[int.Parse(GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id"))]), "Unwear");
                                MainPlayer.InvGet(MainPlayer.Equipment[CheckEq], 0);
                                MainPlayer.Equipment[CheckEq] = "id0;";
                            } else if (Input.GetMouseButtonDown(1)) {
                                if (GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "id") == "53") {
                                    if (GS.GetSemiClass(MainPlayer.Equipment[CheckEq], "tr") == "0") {
                                        MainPlayer.Equipment[CheckEq] = GS.SetSemiClass(MainPlayer.Equipment[CheckEq], "tr", "1");// "1";
                                    } else {
                                        MainPlayer.Equipment[CheckEq] = GS.SetSemiClass(MainPlayer.Equipment[CheckEq], "tr", "0");// "0";
                                    }
                                }
                            }

                        }

                    } else if (CheckEqObj.activeInHierarchy == true) {
                        CheckEqObj.transform.localScale = Vector3.zero;
                        CheckEqObj.GetComponent<ButtonScript>().Active = false;
                    }
                }
                if (Reseteq == true) {
                    EqTextScroll[0] = 0f;
                }

            } else if (WhichMenu == "Craft"){

                ITMenuInfo.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuInfo.GetComponent<RectTransform>().position, HideWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
                ITMenuCraft.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuCraft.GetComponent<RectTransform>().position, ShowWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
                MinimapRefresh("Neither");

                if (Input.GetMouseButton(0) && CraftingMain.transform.GetChild(1).GetChild(0).GetComponent<ButtonScript>().IsSelected && CraftingMain.transform.GetChild(1).GetChild(0).localScale.x > 0.9f) {
                    CraftingPage[0] -= 1;
                    SetCraftOptions();
                } else if (Input.GetMouseButton(0) && CraftingMain.transform.GetChild(1).GetChild(1).GetComponent<ButtonScript>().IsSelected && CraftingMain.transform.GetChild(1).GetChild(1).localScale.x > 0.9f) {
                    CraftingPage[0] += 1;
                    SetCraftOptions();
                }

                // Crafting detailed text
                if(CDTdisplaye[1] != CDTdisplaye[2]){
                    CDTdisplaye[2] = CDTdisplaye[1];
                    CDTdisplaye[0] = 0f;
                }

                CraftingDetailedText.color = Color.Lerp(new Color(1f,1f,1f,0f), Color.white, CDTdisplaye[0]);
                CraftingDetailedText.text = CDTstring;
                CraftingDetailedText.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(316f, 348f, CDTdisplaye[0]), 0f);

            } else {

                CDTdisplaye[2] = -1f;
                CDTdisplaye[0] = 0f;
                ITMenuInfo.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuInfo.GetComponent<RectTransform>().position, HideWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
                ITMenuCraft.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuCraft.GetComponent<RectTransform>().position, HideWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
                MinimapRefresh("Neither");

            }

        } else {

            if (ITShown != "") {
                ITShown = "";
                PlaySoundBank("S_IThide");
            }

            CDTdisplaye[2] = -1f;
            CDTdisplaye[0] = 0f;

            ITMenuInfo.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuInfo.GetComponent<RectTransform>().position, HideWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
            ITMenuCraft.GetComponent<RectTransform>().position = Vector2.MoveTowards(ITMenuCraft.GetComponent<RectTransform>().position, HideWindow.GetComponent<RectTransform>().position, (Screen.height / 10f) * (Time.deltaTime * 50f));
            if (ITBG.GetComponent<HudColorControl>().Alpha > 0f) {
                ITBG.GetComponent<HudColorControl>().Alpha = Mathf.MoveTowards(ITBG.GetComponent<HudColorControl>().Alpha, 0f, 0.06f * (Time.deltaTime * 50f));
                ITBG.GetComponent<HudColorControl>().SetColor("");
            }
            ITicons.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(400f, 400f), Vector2.zero, ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);

            // Minimap
            MinimapRefresh("Minimap");
            if (RS.RoundState == "Normal") {
                GS.SetText(MiniMap.transform.GetChild(1).GetComponent<Text>(), "Nuke in " + GS.DisplayTime(RS.RoundTime), "Wybuch za " + GS.DisplayTime(RS.RoundTime));
            } else if (RS.RoundState == "Nuked") {
                GS.SetText(MiniMap.transform.GetChild(1).GetComponent<Text>(), "GET TO THE EXIT", "BIEGNIJ DO WYJŚCIA");
            } else if (RS.RoundState == "TealState") {
                GS.SetText(MiniMap.transform.GetChild(1).GetComponent<Text>(), "", "");
            } else if (RS.RoundState == "BeforeWave") {
                GS.SetText(MiniMap.transform.GetChild(1).GetComponent<Text>(), "Wave in " + GS.DisplayTime(RS.RoundTime), "Fala za " + GS.DisplayTime(RS.RoundTime));
            } else if (RS.RoundState == "HordeWave") {
                int HordeAmountB = RS.HordeAmount;
                foreach (GameObject GetB in GameObject.FindGameObjectsWithTag("Mob")) {
                    if (GetB.GetComponent<MobScript>().ClassOfMob == "Mutant" && GetB.GetComponent<MobScript>().State == 0) {
                        HordeAmountB += 1;
                    }
                }
                GS.SetText(MiniMap.transform.GetChild(1).GetComponent<Text>(), "Mutants left: " + HordeAmountB, "Ilość mutantów: " + HordeAmountB);
            }

            // Minimap Auras
            if(MMdanger > 0f || MMsafe > 0f || MMunsafe > 0f) {
                MMdanger = Mathf.Clamp(MMdanger - 0.02f*(Time.unscaledDeltaTime*50f), 0f, 100f);
                MMsafe = Mathf.Clamp(MMsafe - 0.02f*(Time.unscaledDeltaTime*50f), 0f, 100f);
                MMunsafe = Mathf.Clamp(MMunsafe - 0.02f*(Time.unscaledDeltaTime*50f), 0f, 100f);

                if(MMsafe > 0f) MMaura.color = Color.Lerp(MMaura.color, new Color32(100, 255, 125, 255), 0.2f*(Time.unscaledDeltaTime*50f));
                else if(MMdanger > 0f) MMaura.color = Color.Lerp(MMaura.color, Color.red, 0.2f*(Time.unscaledDeltaTime*50f));
                else if(MMunsafe > 0f) MMaura.color = Color.Lerp(MMaura.color, new Color32(255, 125, 0, 255), 0.2f*(Time.unscaledDeltaTime*50f));
            } else if (MMaura.color.a > 0f) {
                Color Dimmer = MMaura.color;
                Dimmer.a = Mathf.Clamp(Dimmer.a - 0.01f*(Time.unscaledDeltaTime*50f), 0, 1f);
                MMaura.color = Dimmer;
            }

            // Variables
            GameObject StatsVariables = MiniMap.transform.GetChild(2).gameObject;
            float AddPos = 1f;
            for (int CheckStats = 0; CheckStats < StatsVariables.transform.childCount; CheckStats++) {
                GameObject GetSVObj = StatsVariables.transform.GetChild(CheckStats).gameObject;
                if (GetSVObj.name == "Score") {
                    GetSVObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(62f, AddPos * -16f);
                    GS.SetText(GetSVObj.GetComponent<Text>(), "Score: " + GS.Score, "Wynik: " + GS.Score);
                    AddPos += 1f;
                } else if (GetSVObj.name == "Radioactivity") {
                    if (MainPlayer.MicroSiverts[0] > 0f) {
                        GetSVObj.SetActive(true);
                        GetSVObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(6f, AddPos * -16f);
                        AddPos += 1f;
                        GetSVObj.transform.GetChild(0).GetComponent<Text>().text = (int)(MainPlayer.MicroSiverts[0] * Random.Range(75f, 99f)) + "mSv\n";
                        GetSVObj.GetComponent<Image>().color = Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(1f, 0f, 0f, 1f), MainPlayer.MicroSiverts[0] / 10f);
                        GetSVObj.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(1f, 0f, 0f, 1f), MainPlayer.MicroSiverts[0] / 10f);
                    } else {
                        GetSVObj.SetActive(false);
                    }
                } else if (GetSVObj.name == "Money") {
                    if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                        GetSVObj.SetActive(true);
                        GetSVObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(6f, AddPos * -16f);
                        AddPos += 1f;
                        GetSVObj.transform.GetChild(0).GetComponent<Text>().text = GS.Money.ToString();
                    } else {
                        GetSVObj.SetActive(false);
                    }
                }
            }
            // Minimap

        }

    }

    void WhileDead(bool Shown) {

        if (Shown == false) {

            WhileDeadWindow.transform.position = HideWindow.position;

        } else {

            WhileDeadWindow.transform.position = ShowWindow.position;

            NightVision.gameObject.SetActive(false);
            GS.GetComponent<GameScript>().ContSaturTempInvi = new float[] { Mathf.Clamp(GS.GetComponent<GameScript>().ContSaturTempInvi[0] + (0.2f * (Time.deltaTime * 100f)), 0f, 100f), -100f, 0f, Mathf.Clamp(GS.GetComponent<GameScript>().ContSaturTempInvi[3] + (0.001f * (Time.deltaTime * 50f)), 0f, 1f) };
            CameraBlur += 0.4f * (Time.deltaTime * 50f);

            if (RS.GetComponent<RoundScript>().RoundState == "Nuked") {
                if(Vector3.Distance(MainPlayer.transform.position, new Vector3(RS.GetComponent<RoundScript>().NukePosition.x, MainPlayer.transform.position.y, RS.GetComponent<RoundScript>().NukePosition.z)) <= RS.GetComponent<RoundScript>().NukeDistance){
                    Flash(new Color32(255, 255, 255, 255), new float[]{1f, 1f});
                } else if (Vector3.Distance(MainPlayer.transform.position, new Vector3(RS.GetComponent<RoundScript>().NukePosition.x, MainPlayer.transform.position.y, RS.GetComponent<RoundScript>().NukePosition.z)) - RS.GetComponent<RoundScript>().NukeDistance < 100f) {
                    MainPlayer.GetComponent<PlayerScript>().ShakeCam((100f - (Vector3.Distance(MainPlayer.transform.position, new Vector3(RS.GetComponent<RoundScript>().NukePosition.x, MainPlayer.transform.position.y, RS.GetComponent<RoundScript>().NukePosition.z)) - RS.GetComponent<RoundScript>().NukeDistance)) / 100f, 0.1f);
                    Flash(new Color32(255, 255, 255, (byte)(((100f - (Vector3.Distance(MainPlayer.transform.position, new Vector3(RS.GetComponent<RoundScript>().NukePosition.x, MainPlayer.transform.position.y, RS.GetComponent<RoundScript>().NukePosition.z)) - RS.GetComponent<RoundScript>().NukeDistance)) / 100f) * 255f)), new float[]{1f, 1f});
                }
            }

            if (Leave < 5f) {
                Leave += 0.02f * (Time.deltaTime * 50f);
            } else {
                Leave = -15f;
                RS.SpecialEvent("GameOver");
            }

        }

    }

    void WhileEscaped(string Shown){

        if(Shown == "Escaped") {
            EscapedWindow.transform.position = ShowWindow.transform.position;
            
            EscapedTextes[0].text = GS.SetString("ROUND FINISHED", "RUNDA ZAKOŃCZONA");
            if(RS.RoundState.Substring(3) == "tunnel") EscapedTextes[1].text = GS.SetString("You've escaped via an escape tunnel", "Uciekłeś przez tunel ewakuacyjny");

            // Receive scores
            if(EscapedTextes[2].text == ""){

                // Hunger stats
                if(GS.GetSemiClass(RS.TempStats, "Hunger_") == "0"){
                    EscapedTextes[2].text = GS.SetString("You were hungry, and therefore: ", "Byłeś głodny, przez co:") + "\n";
                    EscapedTextes[2].color = new Color(1f, 0.75f, 0.75f, 1f);
                } else if(GS.GetSemiClass(RS.TempStats, "Hunger_") == "1"){
                    EscapedTextes[2].text = GS.SetString("Your food was at normal level.", "Twoje zaspokojenie głodu było na normalnym poziomie.") + "\n";
                    EscapedTextes[2].color = new Color(1f, 1f, 0.75f, 1f);
                } else if(GS.GetSemiClass(RS.TempStats, "Hunger_") == "2"){
                    EscapedTextes[2].text = GS.SetString("You were well fed, and therefore: ", "Byłeś najedzony, przez co:") + "\n";
                    EscapedTextes[2].color = new Color(0.75f, 1f, 0.5f, 1f);
                } else if(GS.GetSemiClass(RS.TempStats, "Hunger_") == "3"){
                    EscapedTextes[2].text = GS.SetString("You've managed to eat to your fullest, and therefore: ", "Najadłeś się do pełna, przez co:") + "\n";
                    EscapedTextes[2].color = new Color(0f, 1f, 0f, 1f);
                }

                if(GS.ExistSemiClass(RS.TempStats, "PItemLost_"))
                    EscapedTextes[2].text += GS.SetString("- You lost ", "- Straciłeś ") + GS.GetSemiClass(RS.TempStats, "PItemLost_") + GS.SetString(" random item/s", " losowych przedmiot/ów") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "PTired_"))
                    EscapedTextes[2].text += GS.SetString("- You got tired by ", "- Zmęczyłeś się o ") + GS.GetSemiClass(RS.TempStats, "PTired_") + "%\n";
                if(GS.ExistSemiClass(RS.TempStats, "PWet_"))
                    EscapedTextes[2].text += GS.SetString("- You got yourself completely wet", "- Całkowicie się zmoczyłeś") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "PDamaged_"))
                    EscapedTextes[2].text += GS.SetString("- You lost ", "- Straciłeś ") + GS.GetSemiClass(RS.TempStats, "PDamaged_") + GS.SetString(" points of health", " punktów życia") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "PNoAmmo_"))
                    EscapedTextes[2].text += GS.SetString("- All variables of your items, have been set to zero", "- Wszystkie zmienne twoich przedmiotów, spadły do zera") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "RItemGot_"))
                    EscapedTextes[2].text += GS.SetString("+ You were given ", "+ Otrzymałeś ") + GS.GetSemiClass(RS.TempStats, "RItemGot_") + GS.SetString(" random item/s", " losowych przedmiot/ów") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "RHealed_"))
                    EscapedTextes[2].text += GS.SetString("+ Your health went back to 100%", "+ Twoje zdrowie zostało zregenerowane do 100%") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "RAdrenalined_"))
                    EscapedTextes[2].text += GS.SetString("+ You were given a full shot of adrenaline, without taking damage", "+ Otrzymałeś pełen zastrzyk adrenaliny, bez utraty zdrowia") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "RTreasure_"))
                    EscapedTextes[2].text += GS.SetString("+ You were given a treasure, usually found in monuments", "+ Otrzymałeś skarb, znajdywalny w monumentach") + "\n";
                if(GS.ExistSemiClass(RS.TempStats, "RDrunk_"))
                    EscapedTextes[2].text += GS.SetString("+ You drank a little, and you've gained ", "+ Wypiłeś trochę, i otrzymałeś ") + GS.GetSemiClass(RS.TempStats, "RDrunk_") + GS.SetString("% of drunkeness", "% pijaństwa") + "\n";
                if(MainPlayer.Food[0] > MainPlayer.Food[1])
                    EscapedTextes[2].text += GS.SetString("+ The excess ", "+ Nadmiar jedzenia w wysokości ") + (MainPlayer.Food[1] - MainPlayer.Food[0]) + GS.SetString(" food, will be carried over to next round", " pójdzie do następnej rundy");

                // Stats
                foreach(string PushData in GS.ListSemiClass(RS.TempStats)){
                    if(GS.GetStatName(PushData, 1) != "misc.") EscapedTextes[3].text += GS.GetStatName(PushData) + "\n";
                }

            }
            
            EscapedTextes[4].text = GS.SetString("Press JUMP button to continue", "Naciśnij klawisz SKOKU by kontynuować");

            if(GS.ReceiveButtonPress("Jump", "Press") == 1f){
                RS.SpecialEvent("Escape");
            }

            CameraBlur = Mathf.Clamp(CameraBlur + (0.4f * Time.deltaTime * 50f), 0f, 10f);

        } else {
            EscapedWindow.transform.position = HideWindow.transform.position;
        }

    }

    void WhileLoading(bool Shown) {

        if (Shown == false) {

            LoadingWindow.transform.position = HideWindow.position;
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

            LoadingWindow.transform.position = ShowWindow.position;
            GS.GetComponent<GameScript>().SetText(LoadingText, "LOADING", "WCZYTYWANIE");

        }

    }

    void WhileEnvironmental(bool Shown) {

        if (Shown == false) {

            EnvironmentalWindow.transform.position = HideWindow.position;
            if(EnvironmentalObject.gameObject.activeInHierarchy) {
                EnvironmentalObject.gameObject.SetActive(false);
                EnvironmentalObject.parent = null;
            }

        } else {

            EnvironmentalWindow.transform.position = ShowWindow.position;
            if(!EnvironmentalObject.gameObject.activeInHierarchy) {
                EnvironmentalObject.gameObject.SetActive(true);
                EnvironmentalObject.parent = MainCamera.transform;
                EnvironmentalObject.localPosition = Vector3.zero;
            }

            for(int CSEO = 0; CSEO < EnvironmentalObject.transform.childCount; CSEO++){
                Transform SetEnvObj = EnvironmentalObject.GetChild(CSEO);
                switch(SetEnvObj.name){
                    case "HazmatCover":
                        // Hazmat Suit
                        if (MainPlayer.IsHS == true) {
                            if(!SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(true);
                            for(int GetHazmat = 0; GetHazmat < 4; GetHazmat ++)
                                if(GS.GetSemiClass(MainPlayer.Equipment[GetHazmat], "id") == "86") {
                                    SetEnvObj.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (float.Parse(GS.GetSemiClass(MainPlayer.Equipment[GetHazmat], "va"), CultureInfo.InvariantCulture) / 100f) );
                                    break;
                                }
                            SetEnvObj.localPosition = Vector3.Lerp(SetEnvObj.localPosition, Vector3.zero, 0.1f * (Time.deltaTime * 50f));
                        } else {
                            if(SetEnvObj.gameObject.activeInHierarchy && SetEnvObj.localPosition.y > 0.2f)
                                SetEnvObj.gameObject.SetActive(false);
                            SetEnvObj.localPosition = Vector3.Lerp(SetEnvObj.localPosition, Vector3.up * 0.25f, 0.1f * (Time.deltaTime * 50f));
                        }
                        break;
                    case "CardboardboxCover":
                        // Cardboard Box
                        if (MainPlayer.InBox == true && MainPlayer.State == 1) {
                            if(!SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(true);
                            SetEnvObj.localPosition = Vector3.MoveTowards(SetEnvObj.localPosition, Vector3.zero, 0.008f * (Time.deltaTime * 50f));
                        } else {
                            if(SetEnvObj.gameObject.activeInHierarchy && SetEnvObj.localPosition.y > 0.09f)
                                SetEnvObj.gameObject.SetActive(false);
                            SetEnvObj.localPosition = Vector3.MoveTowards(SetEnvObj.localPosition, Vector3.up * 0.1f, 0.008f * (Time.deltaTime * 50f));
                        }
                        break;
                    case "ScubaTank":
                        // Scuba tank
                        if (MainPlayer.IsST == true) {
                            if(!SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(true);
                        } else {
                            if(SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(false);
                        }
                        break;
                    case "SniperScope":
                        // Sniper scope
                        if(GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "at") == "103" && MainPlayer.ZoomValues[1] <= MainPlayer.ZoomValues[0] + 1 && MainPlayer.State == 1){
                            if(!SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(true);
                            SetEnvObj.transform.GetChild(0).forward = Vector3.Lerp(MainPlayer.LookDir.forward, MainPlayer.SlimEnd.transform.forward, 0.01f);
                            SetEnvObj.transform.GetChild(0).localPosition = Vector3.zero + (MainCamera.transform.localPosition / -90f);
                            SetEnvObj.transform.GetChild(1).forward = Vector3.Lerp(MainPlayer.LookDir.forward, MainPlayer.SlimEnd.transform.forward, 0.05f);
                            SetEnvObj.transform.GetChild(1).localPosition = Vector3.zero + (MainCamera.transform.localPosition / 30f);
                        } else {
                            if(SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(false);
                        }
                        break;
                    case "HoloSight":
                        // Holo sight
                        if(GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "at") == "104" && MainPlayer.ZoomValues[1] <= MainPlayer.ZoomValues[0] + 1 && MainPlayer.State == 1){
                            if(!SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(true);
                                Color Visib = Color.HSVToRGB((float)GS.LaserColor / 10f, 1f, 1f);
                                Visib.a = Mathf.Clamp(1f - Quaternion.Angle(MainPlayer.SlimEnd.transform.rotation, MainCamera.rotation), 0f, 1f);
                                SetEnvObj.GetChild(0).GetComponent<SpriteRenderer>().color = Visib;
                        } else {
                            if(SetEnvObj.gameObject.activeInHierarchy)
                                SetEnvObj.gameObject.SetActive(false);
                        }
                        break;
                }
            }
            
            // Night Vision
            if (MainPlayer.IsNV == true) {
                NightVision.gameObject.SetActive(true);
                int ScaleX = (int)Random.Range(-1f, 2f);
                if (ScaleX == 0) {
                    ScaleX = -1;
                }
                int ScaleY = (int)Random.Range(-1f, 2f);
                if (ScaleY == 0) {
                    ScaleY = -1;
                }
                NightVision.transform.localScale = new Vector3(ScaleX, ScaleY, 1f);
            } else {
                NightVision.gameObject.SetActive(false);
            }

        }

    }

    void MinimapRefresh(string WhichMap) {

        if (WhichMap == "Minimap") {
            MiniMap.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(96f, -96f), new Vector2(-128f, 96f), ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);
            MiniMap.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);

            MiniMap.transform.GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, MainPlayer.transform.eulerAngles.y + 90);
            MiniMap.transform.GetChild(0).GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, GameObject.Find("MainCamera").GetComponent<Camera>().fieldOfView / 2f);
            MiniMap.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = GameObject.Find("MainCamera").GetComponent<Camera>().fieldOfView / 360f;

            if (Time.time > mapRefresh) {
                mapRefresh = Time.time + 0.1f;

                foreach (Transform WipeObject in MiniMap.transform.GetChild(0)) {
                    if (WipeObject.name != "PlayerCone") {
                        Destroy(WipeObject.gameObject);
                    }
                }

                if (RS.GetComponent<RoundScript>().TimeOfDay[0] != 0 || GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {

                    MiniMap.transform.GetChild(0).localScale = Vector3.one;
                    MiniMap.transform.GetChild(3).GetComponent<Text>().text = "";

                    foreach (GameObject MobStamp in GameObject.FindGameObjectsWithTag("Mob")) {
                        if (Vector3.Distance(MainPlayer.transform.position, MobStamp.transform.position) < 50f && MobStamp.GetComponent<MobScript>().MobHealth[0] > 0f && MobStamp.GetComponent<MobScript>().TypeOfMob != 7f) {
                            GameObject StampMob = Instantiate(MiniMapMarker) as GameObject;
                            StampMob.transform.SetParent(MiniMap.transform.GetChild(0));
                            StampMob.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(MainPlayer.transform.position.z - MobStamp.transform.position.z), MainPlayer.transform.position.x - MobStamp.transform.position.x);
                            StampMob.GetComponent<Image>().sprite = MinimapMarkerIcons[0];
                            StampMob.GetComponent<Image>().color = MobStamp.GetComponent<MobScript>().MobColor;
                            StampMob.GetComponent<Image>().SetNativeSize();
                            StampMob.transform.localScale = Vector3.one;

                            // Hint
                            if (!HintsTold.Contains("MobMutant") && MobStamp.GetComponent<MobScript>().ClassOfMob == "Mutant") {
                                HintsTold.Add("MobMutant");
                                HintsCooldown.Add("MobMutant");
                            } else if (!HintsTold.Contains("MobBandit") && MobStamp.GetComponent<MobScript>().ClassOfMob == "Bandit") {
                                HintsTold.Add("MobBandit");
                                HintsCooldown.Add("MobBandit");
                            } else if (!HintsTold.Contains("MobSurvivor") && MobStamp.GetComponent<MobScript>().ClassOfMob == "Survivor") {
                                HintsTold.Add("MobSurvivor");
                                HintsCooldown.Add("MobSurvivor");
                            }
                        }
                    }

                    foreach (GameObject LandStamp in GameObject.FindGameObjectsWithTag("Interactable")) {
                        if (Vector3.Distance(MainPlayer.transform.position, LandStamp.transform.position) < 50f && LandStamp.name.Substring(2, 1) == "M") {
                            GameObject StamLand = Instantiate(MiniMapMarker) as GameObject;
                            StamLand.GetComponent<Image>().sprite = MinimapMarkerIcons[2];
                            StamLand.GetComponent<Image>().SetNativeSize();
                            StamLand.transform.SetParent(MiniMap.transform.GetChild(0));
                            StamLand.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(MainPlayer.transform.position.z - LandStamp.transform.position.z), MainPlayer.transform.position.x - LandStamp.transform.position.x);
                            StamLand.transform.localScale = Vector3.one;
                        }
                    }

                    foreach (GameObject ExitStamp in GameObject.FindGameObjectsWithTag("Interactable")) {
                        if (Vector3.Distance(MainPlayer.transform.position, ExitStamp.transform.position) < 50f && ExitStamp.GetComponent<InteractableScript>().Variables.x == 2f && ExitStamp.GetComponent<InteractableScript>().Discovered == true) {
                            GameObject StampExit = Instantiate(MiniMapMarker) as GameObject;
                            StampExit.GetComponent<Image>().sprite = MinimapMarkerIcons[1];
                            StampExit.GetComponent<Image>().SetNativeSize();
                            StampExit.transform.SetParent(MiniMap.transform.GetChild(0));
                            StampExit.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(MainPlayer.transform.position.z - ExitStamp.transform.position.z), MainPlayer.transform.position.x - ExitStamp.transform.position.x);
                            StampExit.transform.localScale = Vector3.one;

                            // Hint
                            if (!HintsTold.Contains("InteractableExit")) {
                                HintsTold.Add("InteractableExit");
                                HintsCooldown.Add("InteractableExit");
                            } 
                        }
                    }

                    foreach (GameObject FlareStamp in GameObject.FindGameObjectsWithTag("Item")) {
                        if (Vector3.Distance(MainPlayer.transform.position, FlareStamp.transform.position) < 50f && GS.GetSemiClass(FlareStamp.GetComponent<ItemScript>().Variables, "id") == "13"){//FlareStamp.GetComponent<ItemScript>().Variables.x == 13f) {
                            GameObject StampFlare = Instantiate(MiniMapMarker) as GameObject;
                            StampFlare.GetComponent<Image>().sprite = MinimapMarkerIcons[4];
                            StampFlare.GetComponent<Image>().color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(FlareStamp.GetComponent<ItemScript>().Variables, "cl")) / 10f, 1f, 1f);//StampFlare.GetComponent<Image>().color = Color.HSVToRGB(FlareStamp.GetComponent<ItemScript>().Variables.z / 10f, 1f, 1f);
                            StampFlare.GetComponent<Image>().SetNativeSize();
                            StampFlare.transform.SetParent(MiniMap.transform.GetChild(0));
                            StampFlare.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(MainPlayer.transform.position.z - FlareStamp.transform.position.z), MainPlayer.transform.position.x - FlareStamp.transform.position.x);
                            StampFlare.transform.localScale = Vector3.one;
                        }
                    }

                } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") != "1" && RS.GetComponent<RoundScript>().TimeOfDay[0] == 0) {

                    MiniMap.transform.GetChild(0).localScale = Vector3.zero;
                    GS.GetComponent<GameScript>().SetText(MiniMap.transform.GetChild(3).GetComponent<Text>(),
                        "It's too dark...",
                        "Jest za ciemno...");

                }

            }

            

        } else if (WhichMap == "Map") {

            MiniMap.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(96f, -96f), new Vector2(-128f, 96f), ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);
            MiniMap.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);

            Vector2 MapSize = new Vector2(250f, 250f);
            if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                MapSize = RS.GetComponent<RoundScript>().GotTerrain.GetComponent<MapInfo>().MapSize;
            }

            ITMap.transform.GetChild(3).GetComponent<RectTransform>().localPosition = new Vector2((MainPlayer.transform.position.z / MapSize.x) * 120f, (MainPlayer.transform.position.x / MapSize.y) * -120f);
            ITMap.transform.GetChild(3).localEulerAngles = new Vector3(0f, 0f, -MainPlayer.transform.eulerAngles.y - 90f);

            // Marking stuff
            if (MarksTouchPad.GetComponent<ButtonScript>().IsSelected == true) {
                if (MarksTouchPad.GetComponent<Image>().color.a < 0.25f) {
                    MarksTouchPad.GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.Clamp(MarksTouchPad.GetComponent<Image>().color.a + 0.01f * (Time.deltaTime * 50f), 0f, 0.1f));
                }
                if (Input.GetMouseButtonDown(0) && Input.GetButton("Information Tab")) {
                    PlaySoundBank("S_MMMark");
                    GameObject StampMark = Instantiate(MiniMapMarker) as GameObject;
                    StampMark.name = "Mark";
                    StampMark.GetComponent<Image>().sprite = MinimapMarkerIcons[3];
                    StampMark.GetComponent<Image>().SetNativeSize();
                    StampMark.GetComponent<Image>().color = Color.HSVToRGB(0f, 1f, 1f);
                    StampMark.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    StampMark.transform.SetParent(ITMap.transform);
                    StampMark.transform.localScale = Vector3.one * 2f;
                    MarksList.Add(StampMark);
                    if (MarksList.ToArray().Length > 10f) {
                        Destroy(MarksList[0]);
                        MarksList.RemoveAt(0);
                    }
                }
            } else {
                if (MarksTouchPad.GetComponent<Image>().color.a > 0f) {
                    MarksTouchPad.GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.Clamp(MarksTouchPad.GetComponent<Image>().color.a - 0.01f * (Time.deltaTime * 50f), 0f, 0.1f));
                }
            }
            for (int CheckMark = 0; CheckMark < MarksList.ToArray().Length; CheckMark ++) {
                GameObject SwitchMark = MarksList[CheckMark];
                if (SwitchMark.transform.localScale.x > 1f) {
                    SwitchMark.transform.localScale = Vector3.MoveTowards(SwitchMark.transform.localScale, Vector3.one, 0.2f * (Time.deltaTime * 50f));
                }
                if (SwitchMark.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0) && Input.GetButton("Information Tab")) {
                    float H = 0f;
                    float V = 0f;
                    float S = 0f;
                    Color.RGBToHSV(SwitchMark.GetComponent<Image>().color, out H, out S, out V);
                    if (H < 0.99f) {
                        SwitchMark.GetComponent<Image>().color = Color.HSVToRGB(Mathf.Clamp(H + 0.1f, 0f, 0.99f), 1f, 1f);
                        SwitchMark.transform.localScale = Vector3.one * 2f;
                        PlaySoundBank("S_MMMark");
                    } else {
                        MarksList.Remove(SwitchMark);
                        Destroy(SwitchMark);
                        CheckMark--;
                        PlaySoundBank("S_Error");
                    }
                } else if (SwitchMark.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(1) && Input.GetButton("Information Tab")) {
                    MarksList.Remove(SwitchMark);
                    Destroy(SwitchMark);
                    CheckMark--;
                    PlaySoundBank("S_Error");
                }
            }

            if (Time.time > mapRefresh) {
                mapRefresh = Time.time + 0.1f;

                foreach (Transform WipeObject in ITMap.transform) {
                    if (WipeObject.name != "PlayerPosition" && WipeObject.name != "MapTiles" && WipeObject.name != "OtherMaps" && WipeObject.name != "MarksTouchPad" && WipeObject.name != "Mark" && WipeObject.name != "MapFG") {
                        Destroy(WipeObject.gameObject);
                    }
                }

                if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0") {
                    ITMap.transform.GetChild(1).gameObject.SetActive(true);
                    ITMap.transform.GetChild(2).gameObject.SetActive(false);
                    for (int MapScan = 0; MapScan <= 99; MapScan++) {
                        GameObject ScannedMapA = ITMap.transform.GetChild(1).GetChild(MapScan).gameObject;
                        GameObject ScannedMapB = GameObject.Find("Terrain").transform.GetChild(MapScan + 1).gameObject;
                        if (ScannedMapB.name.Substring(2, 1) == "M") {
                            ScannedMapA.transform.GetChild(3).gameObject.SetActive(true);
                        } else {
                            ScannedMapA.transform.GetChild(3).gameObject.SetActive(false);
                        }
                        if (ScannedMapB.name.Substring(0, 1) == "0") {
                            ScannedMapA.transform.GetChild(1).gameObject.SetActive(false);
                            ScannedMapA.transform.GetChild(2).gameObject.SetActive(true);
                            ScannedMapA.transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 155, 0, (byte)(int.Parse(ScannedMapB.name.Substring(1, 1)) * 14));
                            ScannedMapA.transform.GetChild(3).gameObject.SetActive(false);
                        } else {
                            ScannedMapA.transform.GetChild(1).gameObject.SetActive(true);
                            ScannedMapA.transform.GetChild(2).gameObject.SetActive(false);
                            ScannedMapA.transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 155, 0, (byte)(int.Parse(ScannedMapB.name.Substring(1, 1)) * 14));
                        }
                    }
                    foreach (GameObject ExitStamp in GameObject.FindGameObjectsWithTag("Interactable")) {
                        if (ExitStamp.GetComponent<InteractableScript>().Variables.x == 2f && ExitStamp.GetComponent<InteractableScript>().Discovered == true) {
                            GameObject StampExit = Instantiate(MiniMapMarker) as GameObject;
                            StampExit.GetComponent<Image>().sprite = MinimapMarkerIcons[1];
                            StampExit.GetComponent<Image>().SetNativeSize();
                            StampExit.transform.SetParent(ITMap.transform);
                            StampExit.transform.localScale = Vector3.one;
                            StampExit.GetComponent<RectTransform>().anchoredPosition = new Vector2((ExitStamp.transform.position.z / 250f) * 120f, (ExitStamp.transform.position.x / 250f) * -120f);
                        }
                    }
                    foreach (GameObject FlareStamp in GameObject.FindGameObjectsWithTag("Item")) {
                        if (GS.GetSemiClass(FlareStamp.GetComponent<ItemScript>().Variables, "id") == "13"){//FlareStamp.GetComponent<ItemScript>().Variables.x == 13f) {
                            GameObject StampFlare = Instantiate(MiniMapMarker) as GameObject;
                            StampFlare.GetComponent<Image>().sprite = MinimapMarkerIcons[4];
                            StampFlare.GetComponent<Image>().color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(FlareStamp.GetComponent<ItemScript>().Variables, "cl")) / 10f, 1f, 1f);//Color.HSVToRGB(FlareStamp.GetComponent<ItemScript>().Variables.z / 10f, 1f, 1f);
                            StampFlare.GetComponent<Image>().SetNativeSize();
                            StampFlare.transform.SetParent(ITMap.transform);
                            StampFlare.transform.localScale = Vector3.one;
                            StampFlare.GetComponent<RectTransform>().anchoredPosition = new Vector2((FlareStamp.transform.position.z / 250f) * 120f, (FlareStamp.transform.position.x / 250f) * -120f);
                        }
                    }
                } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "1") {
                    ITMap.transform.GetChild(1).gameObject.SetActive(false);
                    ITMap.transform.GetChild(2).gameObject.SetActive(true);
                    foreach (Transform GetRightMap in ITMap.transform.GetChild(1)) {
                        if (GetRightMap.name.Substring(0,2) == RS.GetComponent<RoundScript>().GotTerrain.name.Substring(0, 2)) {
                            GetRightMap.gameObject.SetActive(true);
                        } else {
                            GetRightMap.gameObject.SetActive(false);
                        }
                    }
                    foreach (GameObject MutantStamp in GameObject.FindGameObjectsWithTag("Mob")) {
                        if (MutantStamp.GetComponent<MobScript>().ClassOfMob == "Mutant" && MutantStamp.GetComponent<MobScript>().MobHealth[0] > 0f) {
                            GameObject StampMutant = Instantiate(MiniMapMarker) as GameObject;
                            StampMutant.GetComponent<Image>().sprite = MinimapMarkerIcons[0];
                            StampMutant.GetComponent<Image>().SetNativeSize();
                            StampMutant.GetComponent<Image>().color = MutantStamp.GetComponent<MobScript>().MobColor;
                            StampMutant.transform.SetParent(ITMap.transform);
                            StampMutant.GetComponent<RectTransform>().anchoredPosition = new Vector2((MutantStamp.transform.position.z / MapSize.x) * 120f, (MutantStamp.transform.position.x / MapSize.y) * -120f);
                            StampMutant.transform.localScale = Vector3.one;
                            if (StampMutant.GetComponent<RectTransform>().anchoredPosition.x < -120f || StampMutant.GetComponent<RectTransform>().anchoredPosition.x > 120f || StampMutant.GetComponent<RectTransform>().anchoredPosition.y < -120f || StampMutant.GetComponent<RectTransform>().anchoredPosition.y > 120f) {
                                Destroy(StampMutant);
                            }
                        }
                    }
                }

            }   

        } else {

            MiniMap.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(96f, -96f), new Vector2(-128f, 96f), ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);
            MiniMap.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, ITBG.GetComponent<HudColorControl>().Alpha / 0.75f);

        }

    }

    public void Flash(Color32 FlashColor, float[] FlashTime) {
        if(FlashColor.a < 255) FlashTime[0] = FlashTime[1] * ((float)FlashColor.a / 255f);
        FlashImage.color = FlashColor;
        FlashImageSpeed = FlashTime;
    }

    public void PlaySoundBank (string AudioToPlay){

        foreach (Transform GetAudio in SoundBank.transform) {
            if (GetAudio.name == AudioToPlay) {
                GetAudio.gameObject.GetComponent<AudioSource>().Play();
            }
        }

    }

    public Vector3 BillboardPosSet(Vector3 Where) {

        Vector3 There = Vector3.zero;
        Billboard.transform.SetParent(MainCamera.transform);
        Billboard.transform.LookAt(Where);
        There = new Vector3((Billboard.transform.localEulerAngles.y / MainCamera.GetComponent<Camera>().fieldOfView), (Billboard.transform.localEulerAngles.x / MainCamera.GetComponent<Camera>().fieldOfView), 0f);
        There = new Vector3(((There.x + 1f) / 2f) * Screen.width, ((There.y + 1f) / 2f) * Screen.width, 0f);
        Billboard.transform.SetParent(this.transform);
        return There;

    }

    void ScanHint(string What) {

        if (What == "" && ClearHint <= 0f) {

            // Scan for hints to give
            if (/*MainPlayer.GetComponent<PlayerScript>().Inventory[MainPlayer.GetComponent<PlayerScript>().CurrentItemHeld].x > 0f*/ float.Parse(GS.GetSemiClass(MainPlayer.GetComponent<PlayerScript>().Inventory[MainPlayer.GetComponent<PlayerScript>().CurrentItemHeld], "id")) > 0f && !HintsTold.Contains("ItemGot")) {
                HintsTold.Add("ItemGot");
                HintsCooldown.Add("ItemGot");
            }
            /*if ((int)MainPlayer.GetComponent<PlayerScript>().Hunger[0] == (int)(MainPlayer.GetComponent<PlayerScript>().Hunger[1] * 0.75f) && !HintsCooldown.Contains("Hungry")) {
                HintsCooldown.Add("Hungry");
            } else if (MainPlayer.GetComponent<PlayerScript>().Hunger[0] == MainPlayer.GetComponent<PlayerScript>().Hunger[1] && !HintsTold.Contains("Starwing")) {
                HintsTold.Add("Starwing");
                HintsCooldown.Add("Starwing");
            }*/

            if (HintsCooldown.ToArray().Length > 0 && ClearHint <= 0f) {
                ScanHint(HintsCooldown.ToArray()[0]);
                HintsCooldown.RemoveAt(0);
            }

            // About buffs
            if (!HintsTold.Contains("Buff") && MainPlayer.GetComponent<PlayerScript>().BuffsText != "") {
                HintsTold.Add("Buff");
                HintsCooldown.Add("Buff");
            }

            // Swimming
            if (!HintsTold.Contains("Swimming") && MainPlayer.GetComponent<PlayerScript>().IsSwimming == true) {
                HintsCooldown.Add("Swimming");
            }

            // Round Info
            if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0" && RS.GetComponent<RoundScript>().RoundTime < 60f && !HintsTold.Contains("Nuke1")) {
                HintsTold.Add("Nuke1");
                HintsCooldown.Add("Nuke1");
            } else if (GS.GetSemiClass(GS.RoundSetting, "G", "?") == "0" && RS.GetComponent<RoundScript>().RoundTime < 0f && !HintsTold.Contains("Nuke2")) {
                HintsTold.Add("Nuke2");
                HintsCooldown.Add("Nuke2");
            }

            // Getting dark
            if (RS.GetComponent<RoundScript>().TimeOfDay[0] == 3 && !HintsTold.Contains("GettingDark")) {
                HintsTold.Add("GettingDark");
                HintsCooldown.Add("GettingDark");
            }


            // Eventually clear from told hints
            if (HintsTold.Contains("Dying") && MainPlayer.GetComponent<PlayerScript>().Health[0] > MainPlayer.GetComponent<PlayerScript>().Health[1] / 4f) {
                HintsTold.Remove("Dying");
            } /*else if (HintsTold.Contains("Starwing") && MainPlayer.GetComponent<PlayerScript>().Hunger[0] < MainPlayer.GetComponent<PlayerScript>().Hunger[1]) {
                HintsTold.Remove("Starwing");
            }*/

        } else if (What != "") {

            // Check Stats
            string CheckForToldHint = "";
            int HowManyTimesToSay = 1;
            if (What == "Movement") {
                CheckForToldHint = "MVM";
                HowManyTimesToSay = 1;
            } else if (What == "Tab") {
                CheckForToldHint = "TAB";
                HowManyTimesToSay = 1;
            } else if (What == "Biome") {
                if (GS.GetComponent<GameScript>().Biome <= 9) {
                    CheckForToldHint = "b0" + GS.GetComponent<GameScript>().Biome;
                } else {
                    CheckForToldHint = "b" + GS.GetComponent<GameScript>().Biome;
                }
                HowManyTimesToSay = 2;
            } else if (What == "ItemGot") {
                CheckForToldHint = "ITG";
                HowManyTimesToSay = 1;
            } else if (What == "Hurt") {
                CheckForToldHint = "HRT";
                HowManyTimesToSay = 1;
            } else if (What == "Dying") {
                CheckForToldHint = "DIE";
                HowManyTimesToSay = 10;
            } else if (What == "Buff") {
                CheckForToldHint = "BUF";
                HowManyTimesToSay = 1;
            } else if (What == "Nuke1") {
                CheckForToldHint = "NK1";
                HowManyTimesToSay = 2;
            } else if (What == "Nuke2") {
                CheckForToldHint = "NK2";
                HowManyTimesToSay = 6;
            } else if (What == "Hungry") {
                CheckForToldHint = "HUG";
                HowManyTimesToSay = 4;
            } else if (What == "Starwing") {
                CheckForToldHint = "STW";
                HowManyTimesToSay = 10;
            } else if (What == "HordeStart") {
                CheckForToldHint = "HRS";
                HowManyTimesToSay = 1;
            } else if (What == "Swimming") {
                CheckForToldHint = "SWM";
                HowManyTimesToSay = 1;
            } else if (What == "MobMutant") {
                CheckForToldHint = "mMT";
                HowManyTimesToSay = 1;
            } else if (What == "MobBandit") {
                CheckForToldHint = "mBT";
                HowManyTimesToSay = 1;
            } else if (What == "MobSurvivor") {
                CheckForToldHint = "mSV";
                HowManyTimesToSay = 1;
            } else if (What == "InteractableExit") {
                CheckForToldHint = "iEX";
                HowManyTimesToSay = 1;
            } else if (What == "Monument") {
                CheckForToldHint = "MNT";
                HowManyTimesToSay = 1;
            } else if (What == "GettingDark") {
                CheckForToldHint = "DRK";
                HowManyTimesToSay = 1;
            }

            // Check if told already
            for (int CheckHintsTold = 0; CheckHintsTold < GS.GetComponent<GameScript>().ToldHints.Length; CheckHintsTold += 3) {
                if (GS.GetComponent<GameScript>().ToldHints.Substring(CheckHintsTold, 3) == CheckForToldHint) {
                    HowManyTimesToSay -= 1;
                }
            }

            // Say
            if (HowManyTimesToSay > 0) {
                GS.GetComponent<GameScript>().ToldHints += CheckForToldHint;
                PlaySoundBank("S_Hint");
                if (What == "Movement") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nUse WSAD keys to move, SPACE to jump, C to crouch, and LEFT SHIFT to sprint. Those are the default controls.",
                        "PORADA\nUżyj klawiszy WSAD do poruszania się, SPACJI do skakania, oraz LEWEGO SHIFTA do biegania. To jest podstawowe sterowanie.");
                } else if (What == "Tab") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou can open 'Information Tab' by holding the TAB button. It contains a lot of useful info!",
                        "PORADA\nMożesz otworzyć 'Menu Informacji' poprzez przytrzymanie TABULATORA. Zawiera wiele użytecznych informacji!");
                } else if (CheckForToldHint == "b00") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Plains' map. There isn't a lot of stuff nor living beings around here.",
                        "PORADA\nJesteś na mapie 'Polana'. Nie ma tu zbyt wielu przedmiotów ani istot żywych.");
                } else if (CheckForToldHint == "b01") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Village' map. There are quite some items to find here.",
                        "PORADA\nJesteś na mapie 'Wieś'. Jest tu troche rzeczy do pozbierania.");
                } else if (CheckForToldHint == "b02") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Forest' map. There isn't a lot of stuff nor living beings around here.",
                        "PORADA\nJesteś na mapie 'Las'. Nie ma tu zbyt wielu przedmiotów ani istot żywych.");
                } else if (CheckForToldHint == "b03") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'City' map. There are a lot of items to find, stuff to explore, but there's also a lot of mutants and radiated areas!",
                        "PORADA\nJesteś na mapie 'Miasto'. Jest tu sporo przedmiotów do zebrania, ale jest też sporo mutantów i napromieniowanych stref!");
                } else if (CheckForToldHint == "b04") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Wasteland' map. There's a lot of mutants and radiated areas here, get out as soon as you can!",
                        "PORADA\nJesteś na mapie 'Pustkowie'. Jest tu sporo przedmiotów do zebrania, ale jest też sporo mutantów i napromieniowanych stref!");
                } else if (CheckForToldHint == "b05") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Snowy Area' map. It's very cold here, be careful not to get too cold and die of hypothermia!",
                        "PORADA\nJesteś na mapie 'Śnieżny Teren'. Jest tu bardzo zimno, uważaj, żeby nie zmarznąć na śmierć!");
                } else if (CheckForToldHint == "b06") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Swamp' map. It has shallow waters, which will slow you down. And, there also may be quite some mutants here.",
                        "PORADA\nJesteś na mapie 'Bagno'. Są tu płytkie wody, które mogą cię spowolnić. A także, mogą tutaj znajdować się mutanty.");
                } else if (CheckForToldHint == "b07") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Mountains' map. There are mountains, be careful not to fall from them and die.",
                        "PORADA\nJesteś na mapie 'Góry'. Są tu góry, uważaj żeby z nich nie spaść i zginąć.");
                } else if (CheckForToldHint == "b08") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Sea' map. There's water everywhere, and it's completely desolate. Though, there may be some treasures under water!",
                        "PORADA\nJesteś na mapie 'Morze'. Woda jest wszędzie, i nikogo tu nie ma. Aczkolwiek, gdzieniegdzie mogą się znajdować skarby pod wodą!");
                } else if (CheckForToldHint == "b09") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Fjords' map. There are mountains and fjords, which are hard to access. It's also completely desolate.",
                        "PORADA\nJesteś na mapie 'Fjordy'. Są tu góry, które są trudne do nawigowania. Nikogo tu także nie ma.");
                } else if (CheckForToldHint == "b10") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Battleground' map. It's constantly being bombarded, so stay cautious! There is also a lot of bandits and survivors here.",
                        "PORADA\nJesteś na mapie 'Pole Bitwy'. Jest ciągle bombardowana, więc uważaj na siebie! Jest tu też sporo bandytów i niedobitków.");
                } else if (CheckForToldHint == "b11") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are on 'Desert' map. It's very hot here, be careful not to get too hot and die of overheating! Water can cool you down.",
                        "PORADA\nJesteś na mapie 'Pustynia'. Jest tu bardzo gorąco, uważaj, żeby się nie przegrzać! Woda może cię ochłodzić.");
                } else if (What == "ItemGot") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou've got a new item! You can press Q to drop it, or hold Q to throw it. For more informations, hold TAB.",
                        "PORADA\nZdobyłeś nowy przedmiot! Możesz nacisnąć Q żeby go upuścić, albo przytrzymać Q żeby nim rzucić. Po więcej informacji, przytrzymaj TABULATOR.");
                } else if (What == "Hurt") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou got hurt! Watch the health bar, if it drops to zero, you'll lose!",
                        "PORADA\nZostałeś zraniony! Pilnuj poziomu zdrowia, jeśli spadnie do zera, przegrasz!");
                } else if (What == "Dying") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou're running low on health! Try to find bandages or health kits to patch yourself up. Eating when you're full, will also heal you.",
                        "PORADA\nKończy ci się zdrowie! Spróbuj znaleźć bandaże albo apteczki, żeby się uleczyć. Jedzenie, będąc najedzonym, też cię uleczy.");
                    if (int.Parse(GS.GetSemiClass(GS.RoundSetting, "D", "?")) > 3) {
                        GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou're running low on health! Try to find bandages or health kits to patch yourself up.",
                        "PORADA\nKończy ci się zdrowie! Spróbuj znaleźć bandaże albo apteczki, żeby się uleczyć.");
                    }
                } else if (What == "Buff") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nSee that icon above your health bar? That's a buff, and it has somekind of effect on you. Hold TAB to know what it does.",
                        "PORADA\nWidzisz tą ikonkę nad swoim paskiem zdrowia? To jest buff, i wywołuje jakiś efekt na tobie. Przytrzymaj TABULATOR żeby się dowiedzieć co to jest.");
                } else if (What == "Nuke1") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou have 60 seconds before the nuke drop, you should focus on finding an exit. Exit tunels are located on borders of the map.",
                        "PORADA\nMasz 60 sekun zanim uderzy bomba nuklearna, lepiej żebyś poszukał wyjścia. Tunele ewakuacyjne znajdują się na granicach mapy.");
                } else if (What == "Nuke2") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nOH CRAP THE NUKE DROPPED, GET TO AN EXIT, NOW!!!",
                        "PORADA\nO CHOLERA, BOMBA WYBUCHŁA, BIEGNIJ DO WYJŚCIA!!!");
                } else if (What == "Hungry") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYour hunger meter is almost full. Get something to eat, in order to decrease it.",
                        "PORADA\nTwój poziom głodu jest prawie pełen. Znajdź coś do jedzenia, żeby go zmniejszyć.");
                } else if (What == "Starwing") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are starving. Get something to eat, or else you'll die!",
                        "PORADA\nTwój poziom głód jest pełen. Znajdź coś do jedzenia, albo umrzesz!");
                } else if (What == "HordeStart") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nA horde of mutants will appear soon. Explore this map, and find weapons.",
                        "PORADA\nFala mutantów zaraz się pojawi. Pozwiedzaj tą mapę, i znajdź jakąś broń.");
                } else if (What == "Swimming") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou are now swimming underwater. Use WSAD keys to swim and SPACE to swim up. Watch the oxygen meter, if it depletes, you'll begin to lose health!",
                        "PORADA\nPływasz pod wodą. Używaj klawiszy WSAD do pływania oraz SPACJI do plynięcia do góry. Uważaj na poziom tlenu, jeśli się skończy, zaczniesz tracić zdrowie!");
                } else if (What == "MobMutant") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nLook at the minimap, there are mutants nearby! They'll attack anyone who comes near them. Their power and types vary depending on how many rounds you've played.",
                        "PORADA\nSpójż na minimapę, w pobliżu znajdują się mutanci! Mutanci atakują każdego kto się do nich zbliży. Ich siła oraz rodzaj, zależą od tego ilości rund.");
                } else if (What == "MobBandit") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nLook at the minimap, there are bandits nearby! They're hostile, come in groups, are as powerful as you are, and they might carry guns!",
                        "PORADA\nSpójż na minimapę, w pobliżu znajdują się bandyci! Są wrodzy, podążają w grupach, są równie silni co ty, i mogą posiadać bronie palne!");
                } else if (What == "MobSurvivor") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nLook at the minimap, there are survivors nearby. They're friendly, and you can talk to them.",
                        "PORADA\nSpójż na minimapę, w pobliżu znajdują się niedobitkowie. Są przyjaźnie nastawieni, i możesz z nimi rozmawiać.");
                } else if (What == "InteractableExit") {
                    if (RS.GetComponent<RoundScript>().RoundTime <= 30f) {
                        GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou found an exit tunel. It allows you to exit this map and move onto the next round.",
                        "PORADA\nZnalazłeś tunel ewakuacyjny. Możesz wyjść z tej mapy do następnej za pomocą niego.");
                    } else {
                        GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou found an exit tunel. It allows you to exit this map and move onto the next round. But you still have some time, you don't need to leave just yet.",
                        "PORADA\nZnalazłeś tunel ewakuacyjny. Możesz wyjść z tej mapy do następnej za pomocą niego. Masz jednak jeszcze trochę czasu, nie musisz się jeszcze ewakuować.");
                    }
                } else if (What == "Monument") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nYou have found a monument. Monuments contain treasures, but are guarded by heavily armed personel. If you want to raid it, either be prepared, or use your brain.",
                        "PORADA\nZnalazłeś monument. Monumenty zawierają skarby, ale są chronione przez ciężko uzbrojony personel. Jeśli chcesz dokonać napadu, uzbroj się, albo użyj mózgu.");
                } else if (What == "GettingDark") {
                    GS.GetComponent<GameScript>().SetText(HintText,
                        "HINT\nIt's dusk already, and the next round will be at night. Find somekind of an item that will provide you a light source.",
                        "PORADA\nJuż nadszedł zmierzch, a następna runda będzie w nocy. Znajdź jakiś przedmiot który posłuży ci za oświetlenie.");
                }
                ClearHint = Mathf.Lerp(5f, 10f, Mathf.Clamp(HintText.text.Length - 50f, 1f, Mathf.Infinity) / 100f);
            }
            

        }

    }

    public void SetCraftOptions() {

        List<GameObject> ReceivedCOs = new List<GameObject>();
        if (MainPlayer != null) {
            for (int GetCO = 0; GetCO < CraftingTemplates.transform.childCount; GetCO ++) {
                for (int GetRES = 0; GetRES < CraftingTemplates.transform.GetChild(GetCO).GetComponent<CraftingOption>().Resources.Length; GetRES ++) {
                    bool FullBreak = false;
                    for (int GetINV = 0; GetINV < MainPlayer.GetComponent<PlayerScript>().MaxInventorySlots; GetINV ++) {
                        if (GS.GetSemiClass(CraftingTemplates.transform.GetChild(GetCO).GetComponent<CraftingOption>().Resources[GetRES], "id") == GS.GetSemiClass(MainPlayer.GetComponent<PlayerScript>().Inventory[GetINV], "id")){ //MainPlayer.GetComponent<PlayerScript>().Inventory[GetINV].x) {
                            ReceivedCOs.Add(CraftingTemplates.transform.GetChild(GetCO).gameObject);
                            FullBreak = true;
                            break;
                        }
                    }
                    if (FullBreak == true) {
                        break;
                    }
                }
            }
        }
        CraftingPage[1] = (ReceivedCOs.ToArray().Length - 1) / 6;
        CraftingPage[0] = Mathf.Clamp(CraftingPage[0], 0, CraftingPage[1]);
        CraftingMain.transform.GetChild(1).localScale = Vector3.zero;
        if (CraftingPage[0] < CraftingPage[1]) {
            CraftingMain.transform.GetChild(1).GetChild(1).localScale = Vector3.one;
            CraftingMain.transform.GetChild(1).localScale = Vector3.one;
        } else {
            CraftingMain.transform.GetChild(1).GetChild(1).localScale = Vector3.zero;
        }
        if (CraftingPage[0] > 0) {
            CraftingMain.transform.GetChild(1).GetChild(0).localScale = Vector3.one;
            CraftingMain.transform.GetChild(1).localScale = Vector3.one;
        } else {
            CraftingMain.transform.GetChild(1).GetChild(0).localScale = Vector3.zero;
        }
        GS.GetComponent<GameScript>().SetText(CraftingMain.transform.GetChild(1).GetChild(2).GetComponent<Text>(), "Page: " + (CraftingPage[0] + 1) + " / " + (CraftingPage[1] + 1), "Strona: " + (CraftingPage[0] + 1) + " / " + (CraftingPage[1] + 1));
        int Got = 0;
        for (int GetCraftingOption = 0; GetCraftingOption < 6; GetCraftingOption ++) {
            if (ReceivedCOs.ToArray().Length - 1 >= GetCraftingOption + (CraftingPage[0] * 6)) {
                CraftButtons[GetCraftingOption].GetComponent<CraftingOption>().SetOption(ReceivedCOs[GetCraftingOption + (CraftingPage[0] * 6)]);
                Got += 1;
            } else {
                CraftButtons[GetCraftingOption].GetComponent<CraftingOption>().SetOption(null);
            }
        }
        if (Got <= 0) {
            CraftingMain.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            GS.GetComponent<GameScript>().SetText(CraftingMain.transform.GetChild(0).GetComponent<Text>(), "Can't craft anything!", "Nie masz czego stworzyć!");
        } else {
            CraftingMain.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, (Got * 27f));
            GS.GetComponent<GameScript>().SetText(CraftingMain.transform.GetChild(0).GetComponent<Text>(), "Recipes:", "Przepisy:");
        }

    }

}
