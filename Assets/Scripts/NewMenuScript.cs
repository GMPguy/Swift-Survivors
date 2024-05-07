using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random=UnityEngine.Random;
using Unity.Mathematics;

public class NewMenuScript : MonoBehaviour {

    // variables
    public string CurrentWindow = "";
    public string[] PrevWindow = {"", ""}; // Checker, Actual
    float FromStart = 0f;
    public bool isVisible = true;
    bool hide = false;
    bool Swatched = false;
    public float intActive;
    float ForceHidden = 0f;
    // variables

    // references
    GameObject mainAnchor;
    public Image Logo;
    float LogoAlpha = 0f;
    // Whiles
    // Loading
    public float LoadingTime = 3f;
    public string AfterLoading = "";
    public Transform LoadingWindow;
    public Text[] LoadingTextes;
    public Transform[] Clockthingys;
    float Spin = 1f;
    // Loading
    // While main
    public Transform Main;
    public Transform[] MainButtons;
    public Sprite[] MainIcons;
    string[] MainOptions;

    public ButtonScript[] SideButtons;
    public Text SideMenuInfo;
    public Transform ProfileSideMenu;
    public Transform MessagesTray;
    public Sprite[] MessageIcons;
    // While options
    public Transform Options;
    public Text[] OptionInfos;
    public Transform[] OptionButtons;
    public Sprite[] OptionImages;
    string OptionTitle;
    string[] OptionOptions;
    public ButtonScript OptionClose;
    public MenuSliderScript OptionSlider;
    // While pick game
    public Transform PickGame;
    public Transform[] PGbuttons;
    public Text PGdesc;
    public Transform[] PGdescButts;
    public Image GMimage;
    public Sprite[] GMsprited;
    public Transform[] GMoptions;
    string[] GMstringers = {"","","","",""};
    int[] GMintegers = {0,0,0,0,0};
    int SelectedFile = -1;
    // While results
    public Transform ResultsMenu;
    float FadeInValue = 0f;
    public Image ResultsBG;
    public Image ResultsFadeIn;
    public Text ScoreRoundText;
    public Text[] ResultTextes;
    public Text ResultsAnyKey;
    public int CheckoutStage = 0;
    public int ToCheckout = 0;

    public string RSfilename = "";
    public int RSgamemode = 0;
    public int RSdifflevel = 0;
    public int RSscore = 0;
    public int RSrounds = 0;
    public string[] RStempstats;
    // While records
    public Transform RecordsWindow;
    public Text[] RecordTitles;
    public string[] RecordList;
    public string RecordSort;
    public Image RecordClose;
    public Text RecordRefresh;
    public Transform[] RecordOptions;
    public ButtonScript[] RecordSortingButtons;
    public ButtonScript[] RecordFilterButtons;
    public int[] RecordFilters = {0, 0}; // Modes, difficulty levels
    // While profile menu
    public Transform ProfileMenu;
    public ButtonScript[] ProfileButtons;
    public string ProfileTab = "";
    public Transform ProfileTabs;
    public MenuSliderScript ProfileSlider;
    public ButtonScript[] ProManButtons;
    string[] PMstats = {};
    public Transform ProfileSide;
    public GameObject improvNMPMbutton;
    public string[] PMslots = {};
    public Sprite[] NMPMicons;
    public Text WhatTabs;
    // Player viewer
    public ButtonScript PVbox;
    public GameObject PVmodel;
    public Transform PVcamera;
    public RenderTexture PVtexture;
    public Vector4 PVvector = new Vector4(0f, 0f, 3f, 0f);
    // While input
    public Transform Inputwindow;
    public Text InputInfo;
    public InputField InputInput;
    public string InputTopic, InputSubject = "";
    // While announcements
    public Transform AnnounceWindow;
    public Text[] Comments;
    public AudioClip[] CommentSounds;
    // While popups
    public Transform PopupWindow;
    public RectTransform PWachievement;
    public List<string> PopupQueue;
    string PrevPopup = "kek";
    float PopupTime = 0f;
    // While warning
    public string[] Warning = {"", ""}; // Type, Additional info
    public Transform WarningWindows;

    public Transform[] SH; // Show, Hide, Left

    public GameScript GS;
    public ProfileScript PS;
    // While credits
    public Transform CreditsWindow;
    public Image[] CreditBars;
    public Text CreditAnyKey;
    public Transform CreditsRoll;
    // while manual
    public Transform ManualWindow;
    public Transform ManualMain;
    public Transform ManualIntro;
    public ButtonScript[] ManualOptionButtons;
    public MenuSliderScript ManualSlider;
    public Text[] mTextes;
    public Image mImage;
    ManualScript MS;
    // references

    void Start(){

        mainAnchor = this.transform.GetChild(0).gameObject;

        if(GameObject.Find("_GameScript")) {
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            PS = GS.GetComponent<ProfileScript>();
        }

        // ReadBootUp
        AfterLoading = GS.WindowToBootUp;

        // Hide player viewer by default
        PVcamera.GetComponent<Camera>().enabled = false;
        PVtexture.Release();

        // Duplicate option buttons
        for(int O = 0; O < 10; O++){
            GameObject Dupo = Instantiate(OptionButtons[0].gameObject) as GameObject;
            Dupo.transform.SetParent(OptionButtons[0].parent);
            Dupo.GetComponent<RectTransform>().localScale = OptionButtons[0].localScale;
            Dupo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-16f, -32*(O+1));
            OptionButtons[O+1] = Dupo.transform;
        }

        // Duplicate pick game buttons
        for(int PG = 0; PG < 17; PG++){
            GameObject Dupo = Instantiate(PGbuttons[0].gameObject) as GameObject;
            Dupo.transform.SetParent(PGbuttons[0].parent);
            Dupo.GetComponent<RectTransform>().localScale = PGbuttons[0].localScale;
            Dupo.GetComponent<RectTransform>().anchoredPosition = new Vector2(240f,-12 + (-24*(PG+1)));
            PGbuttons[PG+1] = Dupo.transform;
        }

        // Duplicate game making options
        for(int GM = 0; GM < 7; GM++){
            GameObject Dupo = Instantiate(GMoptions[0].gameObject) as GameObject;
            Dupo.transform.SetParent(GMoptions[0].parent);
            Dupo.GetComponent<RectTransform>().localScale = GMoptions[0].localScale;
            Dupo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 48f + (-32*(GM+1)) );
            GMoptions[GM+1] = Dupo.transform;
        }

        // Duplicate record button
        for(int RB = 0; RB < 29; RB++){
            GameObject Dupo = Instantiate(RecordOptions[0].gameObject) as GameObject;
            Dupo.transform.SetParent(RecordOptions[0].parent);
            Dupo.GetComponent<RectTransform>().localScale = RecordOptions[0].localScale;
            Dupo.GetComponent<RectTransform>().anchoredPosition = new Vector2(225f,-8 + (-16*(RB+1)));
            RecordOptions[RB+1] = Dupo.transform;
        }

        // Duplicate profile managment buttons
        for(int PM = 0; PM < 9; PM++){
            GameObject Dupo = Instantiate(ProManButtons[0].gameObject) as GameObject;
            Dupo.transform.SetParent(ProManButtons[0].transform.parent);
            Dupo.GetComponent<RectTransform>().localScale = ProManButtons[0].transform.localScale;
            Dupo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-11f,233 + (-48*(PM+1)));
            ProManButtons[PM+1] = Dupo.GetComponent<ButtonScript>();
        }

        // Duplicate inventory and achievement buttons
        for(int yP = 0; yP < 10; yP++ ) for (int xP = 0; xP < 10; xP++){
            GameObject Dupo = Instantiate(improvNMPMbutton) as GameObject;
            Dupo.transform.SetParent(improvNMPMbutton.transform.parent);
            Dupo.GetComponent<RectTransform>().anchoredPosition = new Vector2(xP*50f, yP*-50f);
            Dupo.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        // Duplicate manual option buttons
        for(int mob = 0; mob < 24; mob++){
            GameObject Dupo = Instantiate(ManualOptionButtons[0].gameObject) as GameObject;
            Dupo.transform.SetParent(ManualOptionButtons[0].transform.parent);
            Dupo.GetComponent<RectTransform>().localScale = ManualOptionButtons[0].transform.localScale;
            Dupo.GetComponent<RectTransform>().anchoredPosition = new Vector2(184f,-12 + (-24f*(mob+1)));
            ManualOptionButtons[mob+1] = Dupo.GetComponent<ButtonScript>();
        }

        GameObject SetAch = Instantiate(improvNMPMbutton.transform.parent.gameObject) as GameObject;
        SetAch.transform.SetParent(improvNMPMbutton.transform.parent.parent);
        SetAch.GetComponent<RectTransform>().anchoredPosition = new Vector2(24f, -24f);
        SetAch.GetComponent<RectTransform>().localScale = Vector3.one;
        SetAch.name = "Achievements";

        Destroy(improvNMPMbutton.gameObject);
        Destroy(SetAch.transform.GetChild(0).gameObject);

    }

    void Update(){

        // Switching windows
        if(PrevWindow[0] != CurrentWindow){
            FromStart = 0f;
            PrevWindow[1] = PrevWindow[0];
            PrevWindow[0] = CurrentWindow;
            Swatched = true;
        }

        // Displaying
        if(hide && mainAnchor.activeSelf) mainAnchor.SetActive(false);
        else if(!hide && !mainAnchor.activeSelf) mainAnchor.SetActive(true);

        if(ForceHidden > 0f){
            ForceHidden -= 0.02f * (Time.unscaledDeltaTime*50f);
            isVisible = false;
        }

        Logo.color = new Color(1f, 1f, 1f, LogoAlpha);

        WhileInput();
        WhileAnnouncements();
        WhileWarnings();
        if (PopupQueue.ToArray().Length > 0) WhilePopups(PopupQueue.ToArray()[0]);
        else WhilePopups("");
        if(LoadingTime > 0f || AfterLoading != ""){

            hide = false;
            WhileLoading(true);
            WhileMain();
            WhileOptions();
            WhileGamePick();
            WhileResults();
            WhileRecords();
            WhileProfileMenu();
            WhileCredits();

        } else if (InputTopic != "") {

            hide = false;
            WhileLoading();

        } else if(isVisible){
            hide = false;
            FromStart += 0.02f * (Time.unscaledDeltaTime*50f);
            switch(CurrentWindow){
                case "Main":
                    WhileMain(true);
                    WhileOptions();
                    WhileGamePick();
                    WhileResults();
                    WhileRecords();
                    WhileProfileMenu();
                    WhileCredits();
                    WhileManual();
                    if(SceneManager.GetActiveScene().name == "MainGame" && Input.GetKeyDown(KeyCode.Escape) && intActive <= 0f)
                        GameObject.Find("MainCanvas").GetComponent<CanvasScript>().IsPaused = false;
                    break;
                case "Options":
                    WhileMain();
                    WhileOptions(true);
                    WhileGamePick();
                    WhileResults();
                    WhileRecords();
                    WhileProfileMenu();
                    WhileCredits();
                    WhileManual();
                    if(Input.GetKeyDown(KeyCode.Escape)) CurrentWindow = "Main";
                    break;
                case "LoadGame": case "NewGame":
                    WhileMain();
                    WhileOptions();
                    WhileGamePick(CurrentWindow);
                    WhileResults();
                    WhileRecords();
                    WhileProfileMenu();
                    WhileCredits();
                    WhileManual();
                    if(Input.GetKeyDown(KeyCode.Escape)) CurrentWindow = "Main";
                    break;
                case "GameOver":
                    WhileMain();
                    WhileOptions();
                    WhileResults(CurrentWindow);
                    WhileGamePick();
                    WhileRecords();
                    WhileProfileMenu();
                    WhileCredits();
                    WhileManual();
                    break;
                case "Records":
                    WhileMain();
                    WhileOptions();
                    WhileGamePick();
                    WhileResults();
                    WhileRecords(true);
                    WhileProfileMenu();
                    WhileCredits();
                    WhileManual();
                    if(Input.GetKeyDown(KeyCode.Escape)) CurrentWindow = "Main";
                    break;
                case "ProfileMenu":
                    WhileMain();
                    WhileOptions();
                    WhileGamePick();
                    WhileResults();
                    WhileRecords();
                    WhileProfileMenu(ProfileTab);
                    WhileCredits();
                    WhileManual();
                    if(Input.GetKeyDown(KeyCode.Escape)) CurrentWindow = "Main";
                    break;
                case "Credits":
                    WhileMain();
                    WhileOptions();
                    WhileGamePick();
                    WhileResults();
                    WhileRecords();
                    WhileProfileMenu();
                    WhileCredits(true);
                    WhileManual();
                    break;
                case "Manual":
                    WhileMain();
                    WhileOptions();
                    WhileGamePick();
                    WhileResults();
                    WhileRecords();
                    WhileProfileMenu();
                    WhileCredits();
                    WhileManual(true);
                    if(Input.GetKeyDown(KeyCode.Escape)) CurrentWindow = "Main";
                    break;
                default:
                    WhileMain();
                    WhileOptions();
                    WhileGamePick();
                    WhileResults();
                    WhileRecords();
                    WhileProfileMenu();
                    WhileCredits();
                    WhileManual();
                    break;
            }

            if(intActive > 0f){
                intActive -= 0.02f*(Time.unscaledDeltaTime*50f);
            }

            WhileLoading();

        } else {

            LogoAlpha = Mathf.Clamp(LogoAlpha - 0.1f * (Time.unscaledDeltaTime*50f), 0f, 1f);

            //PrevWindow = new string[]{"",""};
            intActive = 0.1f;
            FromStart = 0f;
            WhileMain();
            WhileOptions();
            WhileLoading();
            WhileGamePick();
            WhileResults();
            WhileProfileMenu();
            WhileManual();

            CurrentWindow = "Main";
            PrevWindow[0] = CurrentWindow;
            PrevWindow[1] = "";

            if(Input.GetKeyDown(KeyCode.Escape)) isVisible = true;

            hide = true;

        }        

    }

    void SetMO(string[] Options, string Set=""){

        switch(Set){
            case "Main": 
                if(SceneManager.GetActiveScene().name == "MainGame") MainOptions = new string[]{"Menu", "Credits", "Options", "", "GiveUp", "Resume"}; 
                else MainOptions = new string[]{"Exit", "Credits", "Options", "", "Play"}; 
                break;
            default: MainOptions = Options; break;
        }

    }

    void WhileLoading(bool Show = false){

        if(Show){

            LoadingWindow.position = SH[0].position;

            if(LoadingTime > 0f){
                LoadingTime -= 0.02f * (Time.unscaledDeltaTime*50f);

                if(Spin > 0f){
                    Spin -= 0.02f * Time.unscaledDeltaTime*50f;
                    Clockthingys[0].eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.timeSinceLevelLoad)*3f);
                    Clockthingys[1].eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.timeSinceLevelLoad));
                }
                if (Time.timeSinceLevelLoad%1f < 0.1f) Spin = Random.Range(-5f, 10f);

                LoadingTextes[0].text = GS.SetString("LOADING", "WCZYTYWANIE");
                Time.timeScale = 1f;
                switch(AfterLoading){
                    case "f_GameOver": LoadingTextes[1].text = GS.SetString("End results menu", "Menu wyników końcowych"); break;
                    case "f_EscapeMap": LoadingTextes[1].text = GS.SetString("Next roud", "Następnej rundy"); break;
                    case "f_MainMenu": LoadingTextes[1].text = GS.SetString("Main menu", "Main menu"); break;
                    case "f_StartGame": LoadingTextes[1].text = GS.SetString("Starting a new game", "Tworzenie nowej gry"); break;
                    case "f_LoadGame": LoadingTextes[1].text = GS.SetString("Loading game", "Wczytywanie gry"); break;
                    default: LoadingTextes[1].text = ""; break;
                }

            } else if (AfterLoading != ""){

                switch(AfterLoading){
                    case "f_EscapeMap": 
                        GS.ChangeLevel("EscapeMap");
                        AfterLoading = "Don'tclose";
                        break;
                    case "f_ResetMap": 
                        GS.ChangeLevel("ResetMap"); 
                        AfterLoading = "Don'tclose";
                        break;
                    case "f_GameOver":
                        GS.ChangeLevel("GameOver"); 
                        AfterLoading = "Don'tclose";
                        break;
                    case "f_MainMenu":
                        GS.ChangeLevel("BackToMenu");
                        AfterLoading = "Don'tclose";
                        break;
                    case "f_StartGame":
                        GS.ChangeLevel("NewGame");
                        AfterLoading = "Don'tclose";
                        break;
                    case "f_LoadGame":
                        GS.ChangeLevel("LoadGame");
                        AfterLoading = "Don'tclose";
                        break;
                    case "MainMenu":
                        CurrentWindow = "Main";
                        AfterLoading = "";
                        break;
                    case "GameOver":
                        CurrentWindow = AfterLoading;
                        AfterLoading = "";
                        break;
                    case "BootUp":
                        CurrentWindow = "Main";
                        AfterLoading = "";
                        break;
                    default: AfterLoading = ""; break;
                }

            }

        } else {

            Spin = 1f;
            LoadingWindow.position = SH[1].position;

        }

    }

    void WhileAnnouncements(){

        // Comment control
        for(int c = 0; c <=4; c++){
            if(Comments[c].GetComponent<Outline>().effectColor.r + Comments[c].GetComponent<Outline>().effectColor.g + Comments[c].GetComponent<Outline>().effectColor.b > 0.1f){
                Comments[c].GetComponent<Outline>().effectColor = Color.Lerp(Comments[c].GetComponent<Outline>().effectColor, Color.black, (0.02f * (50f*Time.unscaledDeltaTime)));
                Comments[c].GetComponent<Outline>().effectDistance = Vector2.one * Mathf.Lerp(Mathf.Clamp(Comments[c].GetComponent<Outline>().effectColor.r+Comments[c].GetComponent<Outline>().effectColor.g+Comments[c].GetComponent<Outline>().effectColor.b, 0f, 1f), 1f, 2f);
            } else if (Comments[c].GetComponent<Outline>().effectColor.a > 0f){
                Comments[c].GetComponent<Outline>().effectColor = new Color(0f,0f,0f, Comments[c].GetComponent<Outline>().effectColor.a - (0.004f * (50f*Time.unscaledDeltaTime)));
                Color SetTextColor = Comments[c].GetComponent<Text>().color;
                SetTextColor.a = Mathf.Clamp(Comments[c].GetComponent<Outline>().effectColor.a * 2f, 0f, 1f);
                Comments[c].GetComponent<Text>().color = SetTextColor;
                Comments[c].GetComponent<Outline>().effectDistance = Vector2.one;
            }
        }

    }

    public void Pop(string Message, string TypeOMess = ""){

        // Push others
        for (int p = 4; p >= 1; p--){
            Comments[p].text = Comments[p-1].text;
            Comments[p].color = Comments[p-1].color;
            Comments[p].GetComponent<Outline>().effectColor = Comments[p-1].GetComponent<Outline>().effectColor;

            if(Comments[p].GetComponent<Outline>().effectColor.a >= 1f) Comments[p].GetComponent<Outline>().effectColor = Color.Lerp(Comments[p].GetComponent<Outline>().effectColor, Color.black, 0.5f);
        }

        // Here comes the firtr
        string MessyText = Message;
        Color MessyColor = Color.white;
        string MessyAudio = "";
        switch(TypeOMess){
            case "Error": MessyColor = Color.red; MessyAudio = "Alarm"; break;
            case "ItemBroke": MessyColor = Color.red; MessyAudio = "ItemBroke"; break;
            case "MidError": MessyColor = Color.yellow; MessyAudio = "Alarm"; break;
            case "Wear": MessyAudio = "Zip"; break;
            case "Unwear": MessyAudio = "Paper"; break;
            case "Draw": MessyColor = new Color32(255, 255, 200, 255); MessyAudio = "Drawing"; break;
            case "Good": MessyColor = Color.green; break;
            case "Craft": MessyColor = Color.green; MessyAudio = "Microwave"; break;
            case "Buy": MessyColor = Color.green; MessyAudio = "CashOut"; break;
            case "Teal": MessyColor = Color.cyan; MessyAudio = "Teal"; break;
            default: MessyColor = Color.white; MessyAudio = ""; break;
        }
        if(TypeOMess != "" && TypeOMess.Substring(0,1) == "ś") MessyAudio = TypeOMess.Substring(1);
        for(int ac = 0; ac < CommentSounds.Length; ac++) if(CommentSounds[ac].name == MessyAudio){
            Comments[0].transform.parent.GetComponent<AudioSource>().clip = CommentSounds[ac];
            Comments[0].transform.parent.GetComponent<AudioSource>().Play();
            break;
        }
        Comments[0].GetComponent<Outline>().effectColor = MessyColor;
        Comments[0].GetComponent<Text>().color = MessyColor;
        Comments[0].GetComponent<Text>().text = Message;

    }

    void WhileWarnings(){
        if(Warning[0] != ""){
            WarningWindows.position = SH[0].position;

            bool ClearIt = false;

            // Prepearing data
            string[] WarningAddData = {"Default"};
            switch(Warning[0]){
                case "EraseSave":
                    string[] TheFile = {"", ""};
                    foreach(string CheckName in GS.ListSemiClass(PlayerPrefs.GetString("Saves"), "©")) if(GS.GetSemiClass(CheckName, "id", "®") == Warning[1]) {
                        TheFile[0] = GS.GetSemiClass(CheckName, "sn", "®");
                        TheFile[1] = GS.GetSemiClass(GS.GetSemiClass(CheckName, "rs", "®"), "P", "?");
                    }

                    WarningAddData = new string[]{
                        "Default",
                        GS.SetString("ERASE SAVE FILE?", "USUNĄĆ TEN ZAPIS?"),
                        GS.SetString("Are you sure you want to erase save file '", "Jesteś pewien że chcesz usunąć zapis '") + TheFile[0].ToString() + "'?",
                        "DELETE_SAVE",
                        "NO"};
                    if (TheFile[1] != "1")
                        WarningAddData[2] += GS.SetString(" All of the progress from this file will be lost!", " Cały postęp z tego pliku zostanie utracony!");
                    break;
                case "EraseProfile":
                    string TheProfile = "";
                    foreach(string CheckProf in GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©")) if(GS.GetSemiClass(CheckProf, "ID_", "®") == Warning[1])
                        TheProfile = GS.GetSemiClass(CheckProf, "PN_", "®");

                    if (GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©").Length > 1){
                        WarningAddData = new string[]{
                            "Default",
                            GS.SetString("DELETE PROFILE?", "USUNĄĆ TEN PROFIL?"),
                            GS.SetString("Are you REALLY sure, that you want to delete this profile '", "Czy jesteś NA PEWNO pewien tego, żeby usunąć ten profil '") + TheProfile.ToString() + GS.SetString("'? Everything associated with this profile, will be wiped clean from your device, IRREVERSIBLY!", "'? Wszystko związane z tym profilem zostanie usunięte z tego urządzenia, BEZPOWROTNIE!"),
                            "DELETE_PROFILE",
                            "NO"};
                    } else {
                        WarningAddData = new string[]{
                            "Default",
                            GS.SetString("ERROR", "BŁĄD"),
                            GS.SetString("You cannot delete a profile, if it's the only one you have!", "Nie możesz usunąć profilu, jeżeli jest jedynym jakiego posiadasz."),
                            "OK",
                            ""};
                    }
                    break;
                case "ExitGame":
                    WarningAddData = new string[]{
                        "Default",
                        GS.SetString("EXIT THE GAME?", "WYJŚĆ Z GRY?"),
                        GS.SetString("Either way, thanks for playing Swift Survivors!", "Tak czy inaczej, dziękujemy za zagranie w Swift Survivors!"),
                        "EXIT_GAME",
                        "STAY_GAME"};
                    break;
                case "GiveUp":
                    WarningAddData = new string[]{
                        "Default",
                        GS.SetString("YOU WANT TO GIVE UP?", "CHCESZ SIĘ PODDAĆ?"),
                        GS.SetString("Giving up will cause your character to die instantly.\n\nUse this option, if you want to get rid of this save file, but also want to keep your statistics.", "Poddanie się spowoduje, że twoja postać natychmiastowo umrze.\n\nUżyj tej opcji, gdy chcesz usunąć zapis z grą, jednocześnie nie tracąc statystyk nabytych w grze."),
                        "KYS",
                        "STAY_GAME"};
                    if(Warning[0] == "BackToMenu0") WarningAddData[2] = GS.SetString("Game is saved after completing rounds. Since you haven't finished at least one, this save file will be lost!", "Gra zapisuje się po ukończeniu rundy. Ponieważ nie przeszedłeś ani jednej, zapis z tą grą zostanie utracony!");
                    break;
                case "BackToMenu":
                    WarningAddData = new string[]{
                        "Default",
                        GS.SetString("GO BACK TO MAIN MENU?", "WRÓCIĆ DO MENU GŁÓWNEGO?"),
                        GS.SetString("Game is saved, but you'll have to start from the beginning of this round.", "Gra została zapisana, ale będziesz musiał zaczynać od początku tej rundy."),
                        "BACK_TO_MENU",
                        "STAY_GAME"};
                    if(GS.Round == 1) WarningAddData[2] = GS.SetString("Game is saved after completing rounds. Since you haven't finished at least one, this save file will be lost!", "Gra zapisuje się po ukończeniu rundy. Ponieważ nie przeszedłeś ani jednej, zapis z tą grą zostanie utracony!");
                    break;
                case "NMPM_ItemPreview":
                    WarningAddData = new string[]{
                        "ItemPreview",
                        GS.PS.GetProfileItemName(GS.GetSemiClass(Warning[1], "id_")),
                        GS.PS.GetProfileItemDesc(GS.GetSemiClass(Warning[1], "id_")),
                        "EQUIP_ITEM",
                        "TRASH_ITEM",
                        "",
                        "BACK"};
                    break;
                case "NMPM_ItemInUse":
                    WarningAddData = new string[]{
                        "ItemPreview",
                        GS.PS.GetProfileItemName(GS.GetSemiClass(Warning[1], "id_")),
                        GS.PS.GetProfileItemDesc(GS.GetSemiClass(Warning[1], "id_")),
                        "UNEQUIP_INUSE",
                        "TRASH_ITEM",
                        "",
                        "BACK"};
                    break;
                case "NMPM_Achievement":
                    WarningAddData = new string[]{
                        "ItemPreview",
                        GS.PS.GetProfileItemName(GS.GetSemiClass(Warning[1], "id_")),
                        GS.PS.GetProfileItemDesc(GS.GetSemiClass(Warning[1], "id_") + "-A"),
                        "",
                        "",
                        "",
                        "BACK"};

                        if(int.Parse(GS.GetSemiClass(Warning[1], "va_")) > 0){
                            WarningAddData[1] = "???";
                            WarningAddData[2] = GS.PS.GetProfileItemDesc(GS.GetSemiClass(Warning[1], "id_") + "-B");
                        }
                    break;
                case "PMdefault":
                    WarningAddData = new string[]{
                        "Default",
                        GS.GetSemiClass(Warning[1], "ti_"),
                        GS.GetSemiClass(Warning[1], "de_"),
                        "OK",
                        ""};
                    break;
            }

            // Displaying warnings
            foreach(Transform GetWarn in WarningWindows.transform){
                if(GetWarn.name == WarningAddData[0]){
                    GetWarn.localScale = Vector3.one;
                    switch(WarningAddData[0]){
                        case "Default":
                            GetWarn.GetChild(2).GetComponent<Text>().text = WarningAddData[1];
                            GetWarn.GetChild(3).GetComponent<Text>().text = WarningAddData[2];
                            for(int bs = 0; bs < 2; bs++){
                                string ButtonVar = WarningAddData[3+bs];
                                Text ButtonText = GetWarn.GetChild(4+bs).GetComponent<Text>();
                                ButtonScript ButtonButton = GetWarn.GetChild(4+bs).GetComponent<ButtonScript>();
                                switch(ButtonVar){
                                    case "DELETE_SAVE":
                                        ButtonText.text = GS.SetString("Erase it", "Usuń go");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)){
                                            GS.SaveManipulation(int.Parse(Warning[1]), 2);
                                            ClearIt = true;
                                        }
                                        break;
                                    case "DELETE_PROFILE":
                                        ButtonText.text = GS.SetString("Delete it", "Usuń go");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)){
                                            int BackUp = 1337;
                                            foreach(string CheckProfile in GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©")) if(GS.GetSemiClass(CheckProfile, "ID_", "®") != Warning[1])
                                                BackUp = int.Parse(GS.GetSemiClass(CheckProfile, "ID_", "®"));
                                            PS.SaveProfile(-PS.ProfileID);
                                            PS.SaveProfile(BackUp);
                                            CurrentWindow = "Main";
                                            ClearIt = true;
                                        }
                                        break;
                                    case "KYS": 
                                        ButtonText.text = GS.SetString("Give up", "Poddaj się");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) {
                                            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().IsPaused = false;
                                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Hurt(9999f, "Suicide", false, Vector3.zero);
                                            ClearIt = true;
                                        }
                                        break;
                                    case "BACK_TO_MENU": 
                                        ButtonText.text = GS.SetString("Exit", "Wyjdź");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) {
                                            LoadingTime = Random.Range(1f, 3f);
                                            AfterLoading = "f_MainMenu";
                                        }
                                        break;
                                    case "EXIT_GAME": 
                                        ButtonText.text = GS.SetString("Exit", "Wyjdź");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) Application.Quit(); 
                                        break;
                                    case "STAY_GAME": 
                                        ButtonText.text = GS.SetString("Stay", "Pozostań");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) ClearIt = true; 
                                        break;
                                    case "NO": 
                                        ButtonText.text = GS.SetString("No", "Nie");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) ClearIt = true; 
                                        break;
                                    case "OK": 
                                        ButtonText.text = GS.SetString("Ok", "Ok");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) ClearIt = true; 
                                        break;
                                    default:
                                        ButtonText.text = "";
                                        break;
                                }
                            }
                            break;
                        case "ItemPreview":
                            GetWarn.GetChild(2).GetComponent<Text>().text = WarningAddData[1];
                            GetWarn.GetChild(3).GetComponent<Text>().text = WarningAddData[2];

                            if(GS.GetSemiClass(Warning[1], "tp_") == "Achievements") {
                                for(int ai = 0; ai < NMPMicons.Length; ai++) if ((int.Parse(GS.GetSemiClass(Warning[1], "va_")) <= 0 && NMPMicons[ai].name == GS.GetSemiClass(Warning[1], "id_")+"-A") || (int.Parse(GS.GetSemiClass(Warning[1], "va_")) > 0 && NMPMicons[ai].name == GS.GetSemiClass(Warning[1], "id_")+"-B")){
                                    GetWarn.GetChild(4).GetComponent<Image>().sprite = NMPMicons[ai];
                                    break;
                                }
                            } else {
                                for(int gi = 0; gi < NMPMicons.Length; gi++) if (NMPMicons[gi].name == GS.GetSemiClass(Warning[1], "id_")){
                                    GetWarn.GetChild(4).GetComponent<Image>().sprite = NMPMicons[gi];
                                    break;
                                }
                            }

                            for(int bs = 0; bs < 4; bs++){
                                string ButtonVar = WarningAddData[3+bs];
                                Text ButtonText = GetWarn.GetChild(5+bs).GetComponent<Text>();
                                ButtonScript ButtonButton = GetWarn.GetChild(5+bs).GetComponent<ButtonScript>();
                                switch(ButtonVar){
                                    case "UNEQUIP_INUSE":
                                        ButtonText.text = GS.SetString("Unequip this item", "Zdejmij ten przedmiot");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)){
                                            PS.MoveItem(Warning[1], GS.GetSemiClass(Warning[1], "tp_"));
                                            ClearIt = true;
                                        }
                                        break;
                                    case "EQUIP_ITEM":
                                        ButtonText.text = GS.SetString("Use this item", "Użyj tego przedmiotu");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)){
                                            PS.MoveItem(Warning[1], "InUse");
                                            ClearIt = true;
                                        }
                                        break;
                                    case "TRASH_ITEM":
                                        ButtonText.text = GS.SetString("Delete this item", "Usuń ten przedmiot");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)){
                                            PS.RemoveItem(GS.GetSemiClass(Warning[1], "mr_"));
                                            ClearIt = true;
                                        }
                                        break;
                                    case "BACK":
                                        ButtonText.text = GS.SetString("Back", "Wróć");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) ClearIt = true;
                                        break;
                                    default:
                                        ButtonText.text = "";
                                        break;
                                }
                            }
                            break;
                        case "AchPreview":
                            GetWarn.GetChild(2).GetComponent<Text>().text = WarningAddData[1];
                            GetWarn.GetChild(3).GetComponent<Text>().text = WarningAddData[2];

                            if(GS.GetSemiClass(Warning[1], "tp_") == "Achievements")
                            for(int gi = 0; gi < NMPMicons.Length; gi++) if ((int.Parse(GS.GetSemiClass(Warning[1], "va_")) <= 0 && NMPMicons[gi].name == GS.GetSemiClass(Warning[1], "id_")+"-A") || (int.Parse(GS.GetSemiClass(Warning[1], "va_")) > 0 && NMPMicons[gi].name == GS.GetSemiClass(Warning[1], "id_")+"-B")){
                                GetWarn.GetChild(4).GetComponent<Image>().sprite = NMPMicons[gi];
                                break;
                            }

                            for(int bs = 0; bs < 4; bs++){
                                string ButtonVar = WarningAddData[3+bs];
                                Text ButtonText = GetWarn.GetChild(5+bs).GetComponent<Text>();
                                ButtonScript ButtonButton = GetWarn.GetChild(5+bs).GetComponent<ButtonScript>();
                                switch(ButtonVar){
                                    case "BACK":
                                        ButtonText.text = GS.SetString("Back", "Wróć");
                                        if(ButtonButton.IsSelected && Input.GetMouseButtonDown(0)) ClearIt = true;
                                        break;
                                    default:
                                        ButtonText.text = "";
                                        break;
                                }
                            }
                            break;
                    }
                } else {
                    GetWarn.localScale = Vector3.zero;
                }
            }

            if(ClearIt){
                Warning[0] = Warning[1] = "";
            }

        } else {
            WarningWindows.position = SH[1].position;
        }
    }

    void WhileMain(bool Show = false){

        if(Show){
            Main.position = Vector3.Lerp(Main.position, SH[0].position, 0.1f);

            LogoAlpha = Mathf.Clamp(LogoAlpha + 0.02f * (Time.unscaledDeltaTime*50f), 0f, 1f);

            if(Swatched){
                SetMO( new string[] {}, "Main" );
                Swatched = false;
            }

            // Set main buttons
            for(int x=0; x < 8; x++){
                Transform MB = MainButtons[x];
                if(x < MainOptions.Length && MainOptions[x] != "") {
                    MB.localScale = Vector3.one;
                    MB.GetComponent<ButtonScript>().Active = true;
                } else {
                    MB.localScale = Vector3.zero;
                    MB.GetComponent<ButtonScript>().Active = false;
                }
                if(x < MainOptions.Length) {
                    foreach(Sprite ThisOne in MainIcons)
                        if(ThisOne.name == MainOptions[x]) MB.GetChild(1).GetComponent<Image>().sprite = ThisOne;
                    switch(MainOptions[x]){
                        case "Resume":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Resume", "Wznów");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                GameObject.Find("MainCanvas").GetComponent<CanvasScript>().IsPaused = false;
                            break;
                        case "GiveUp":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Give up", "Poddaj się");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                Warning = new string[]{"GiveUp", ""};
                            break;
                        case "Play":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Play", "Graj");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                SetMO( new string[]{"Back", "", "Records", "LoadGame"} );
                            break;
                        case "Options":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Options", "Opcje");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                CurrentWindow = "Options";
                            break;
                        case "Credits":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Credits", "Lista płac");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                CurrentWindow = "Credits";
                            break;
                        case "Exit":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Exit", "Wyjdź");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                Warning = new string[]{"ExitGame", ""};
                            break;
                        case "Menu":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Quit to menu", "Wyjdź do menu");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                Warning = new string[]{"BackToMenu", ""};
                            break;
                        case "Records":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Records", "Rekordy");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                CurrentWindow = "Records";
                            break;
                        case "LoadGame":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Game files", "Zapiski gier");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                CurrentWindow = "LoadGame";
                            break;
                        case "Back":
                            MB.GetChild(0).GetComponent<Text>().text = GS.SetString("Back", "Wróć");
                            if(MB.GetComponent<ButtonScript>().IsHovering && Input.GetMouseButtonDown(0) && intActive<=0f)
                                Swatched = true;
                            break;
                    }
                }
            }

            // Side buttons
            string DisplaySideInfo = "";
            for(int sb = 0; sb < SideButtons.Length; sb++){
                Image ButtonImage = SideButtons[sb].GetComponent<Image>();

                switch(SideButtons[sb].gameObject.name){
                    case "Fullscreen":
                        if (SideButtons[sb].IsHovering && Screen.fullScreen) DisplaySideInfo = GS.SetString(
                            "Fullscreen: enabled", "Pełen ekran: włączony"
                        ); else if (SideButtons[sb].IsHovering && !Screen.fullScreen) DisplaySideInfo = GS.SetString(
                            "Fullscreen: disabled", "Pełen ekran: wyłączony"
                        );

                        if(SideButtons[sb].IsSelected && Input.GetMouseButtonDown(0)) Screen.fullScreen = !Screen.fullScreen;
                        break;
                    case "Language":
                        if (SideButtons[sb].IsHovering) DisplaySideInfo = GS.SetString(
                            "Language: English", "Język: Polski"
                        );

                        if(SideButtons[sb].IsSelected && Input.GetMouseButtonDown(0))
                            if(GS.Language == "English") GS.Language = "Polski";
                            else GS.Language = "English";
                        break;
                    case "Mute":
                        if (SideButtons[sb].IsHovering && GS.MuteVolume <= 0f) DisplaySideInfo = GS.SetString(
                            "Sound: unmuted", "Udźwiękowienie: nie wyciszone"
                        ); else if (SideButtons[sb].IsHovering && GS.MuteVolume > 0f) DisplaySideInfo = GS.SetString(
                            "Sound: muted", "Udźwiękowienie: wyciszone"
                        );

                        if(GS.MuteVolume > 0f){
                            GS.Volumes[0] = 0f;
                            ButtonImage.fillAmount = 0.5f;
                        } else {
                            ButtonImage.fillAmount = 1f;
                        }

                        if(SideButtons[sb].IsSelected && Input.GetMouseButtonDown(0))
                            if(GS.MuteVolume <= 0f && GS.Volumes[0] > 0f) {
                                GS.MuteVolume = GS.Volumes[0];
                                GS.Volumes[0] = 0f;
                            } else if (GS.MuteVolume > 0f){
                                GS.Volumes[0] = GS.MuteVolume;
                                GS.MuteVolume = 0f;
                            }
                        break;
                    case "Manual":
                        if (SideButtons[sb].IsHovering) DisplaySideInfo = GS.SetString(
                            "Open the 'Survival Manual'", "Otwórz 'Podręcznik Przetrwania'"
                        );

                        if(SideButtons[sb].IsSelected && Input.GetMouseButtonDown(0)) {
                            CurrentWindow = "Manual";
                        }
                        break;
                    case "Profiles":
                        if (SideButtons[sb].IsHovering) DisplaySideInfo = GS.SetString(
                            "Open profile menu", "Otwórz menu profilowe"
                        );

                        if(SideButtons[sb].IsSelected && Input.GetMouseButtonDown(0)) {
                            ProfileTab = "Stats";
                            CurrentWindow = "ProfileMenu";
                        }
                        break;
                    case "News":
                        if (SideButtons[sb].IsHovering) DisplaySideInfo = GS.SetString(
                            "Open game's main page", "Otwórz główną stronę gry"
                        );
                        if(SideButtons[sb].IsSelected && Input.GetMouseButtonDown(0)) Application.OpenURL("https://gmpguy.itch.io/swift-survivors");
                        break;
                }
            }

            if(DisplaySideInfo != ""){
                SideMenuInfo.text = DisplaySideInfo;
                ProfileSideMenu.localScale = Vector3.zero;
            } else {
                SideMenuInfo.text = "";
                ProfileSideMenu.localScale = Vector3.one;
                string[] Timies = {System.DateTime.Now.Hour.ToString(), "00" + System.DateTime.Now.Minute, "00" + System.DateTime.Now.Second};
                Timies = new string[]{Timies[0], Timies[1].Substring(Timies[1].Length-2, 2), Timies[2].Substring(Timies[2].Length-2, 2)};
                ProfileSideMenu.GetChild(0).GetComponent<Text>().text = PS.Profilename + GS.SetString("\nCurrent time: ", "\nObecna godzina: ") + System.DateTime.Now.Hour + ":" + Timies[1] + ":" + Timies[2];
                ProfileSideMenu.GetChild(2).GetComponent<Image>().fillAmount = (float)PS.Exp[1] / (float)PS.Exp[2];
                ProfileSideMenu.GetChild(3).GetComponent<Text>().text = PS.Exp[0].ToString();
            }

            // Messages tray
            MessagesTray.localScale = Vector3.one;
            for(int mt = 0; mt < 6; mt++){
                if(mt < GS.PS.Messages.ToArray().Length){
                    MessagesTray.GetChild(mt).localScale = Vector3.one;
                    if(MessagesTray.GetChild(mt).GetComponent<ButtonScript>().IsSelected) {
                        MessagesTray.GetChild(mt).GetChild(1).GetComponent<Text>().text = GS.GetSemiClass(GS.PS.Messages[mt], "ti_") + GS.SetString("\n[Read more]", "\n[Czytaj dalej]");
                        MessagesTray.GetChild(mt).GetChild(1).GetComponent<Text>().color = new Color(1f,1f,1f, Mathf.Clamp(MessagesTray.GetChild(mt).GetChild(1).GetComponent<Text>().color.a + Time.unscaledDeltaTime*3f, 0f, 1f) );
                    } else {
                        MessagesTray.GetChild(mt).GetChild(1).GetComponent<Text>().color = new Color(1f,1f,1f,0f);
                    }

                    for(int gi = 0; gi < MessageIcons.Length; gi++) if (MessageIcons[gi].name == GS.GetSemiClass(GS.PS.Messages[mt], "vi_")) {
                        MessagesTray.GetChild(mt).GetChild(0).GetComponent<Image>().sprite = MessageIcons[gi];
                        break;
                    }

                    if( (MessagesTray.GetChild(mt).GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) || (GS.GetSemiClass(GS.PS.Messages[mt], "im_") == "2") ){
                        Warning = new string[]{GS.GetSemiClass(GS.PS.Messages[mt], "sp_"), GS.PS.Messages[mt]};
                        GS.PS.Messages.RemoveAt(mt);
                    }
                } else {
                    MessagesTray.GetChild(mt).localScale = Vector3.zero;
                }
            }

        } else if (!Show && PrevWindow[1] == "Main") {
            Main.position = Vector3.Lerp(Main.position, SH[2].position, 0.025f);
            MessagesTray.localScale = Vector3.zero;
        } else {
            Main.position = SH[2].position;
            MessagesTray.localScale = Vector3.zero;
        }

    }

    void WhileOptions(bool Show = false){

        if(Show){

            LogoAlpha = 0.5f;

            Options.position = SH[0].position;
            Options.localScale = Vector3.one * Mathf.Lerp(0.75f, 1f, FromStart * 3f);
            OptionSlider.Active = true;
            OptionSlider.gameObject.SetActive(true);
            if(Options.GetChild(0).GetComponent<HudColorControl>().Alpha < 1f) {
                Options.GetChild(0).GetComponent<HudColorControl>().Alpha = Mathf.MoveTowards(Options.GetChild(0).GetComponent<HudColorControl>().Alpha, 1f, 0.1f * (Time.unscaledDeltaTime*50f));
                Options.GetChild(0).GetComponent<HudColorControl>().SetColor("");
            }

            OptionButtons[0].parent.localScale = Vector3.one;

            if (OptionClose.IsSelected && Input.GetMouseButtonDown(0))
                if(OptionTitle == "main") CurrentWindow = "Main";
                else OptionTitle = "main";

            // Set option options
            string[] Keybinders = {};

            switch(OptionTitle){
                case "main": 
                    OptionOptions = new string[]{ "graphics", "sound", "controls", "camera", "misc" }; 
                    OptionInfos[0].text = GS.SetString("OPTIONS", "OPCJE"); 
                    break;
                case "graphics": 
                    OptionOptions = new string[]{ "graphs", "GQ", "LQ", "PQ", "FQ", "SQ", "", "hudsettings", "HC", "MR", "HR" }; 
                    OptionInfos[0].text = GS.SetString("GRAPHICS", "GRAFIKA"); 
                    break;
                case "sound": 
                    OptionOptions = new string[]{ "EP" ,"" , "volumes", "MV", "SV", "BGM" }; 
                    OptionInfos[0].text = GS.SetString("SOUNDS", "UDŹWIĘKOWIENIE"); 
                    break;
                case "misc": 
                    OptionOptions = new string[]{"LG", "LC", "HS", "", "goresettings", "RD" }; 
                    OptionInfos[0].text = GS.SetString("MISC", "INNE"); 
                    break;
                case "camera": 
                    OptionOptions = new string[]{"FOV", "CB", "CS", "HS", "", "camerasettings", "MS", "MSM", "MI" }; 
                    OptionInfos[0].text = GS.SetString("MISC", "INNE"); 
                    break;
                case "controls": 
                    OptionOptions = new string[]{ "kb0" , "kb1", "kb2", "kb3", "kb4", "kb5", "kb6", "", "kb7", "kb8", "kb10", "kb12", "kb9", "", "kb11", "kb13" }; 
                    OptionInfos[0].text = GS.SetString("CONTROLS", "STEROWANIE");

                    Keybinders = new string[]{
                        "f_MoveForward;eng_Forward;pl_Do przodu", // 0
                        "f_MoveBackwards;eng_Backwards;pl_Do tyłu",
                        "f_MoveRight;eng_Forward;pl_Do przodu",
                        "f_MoveLeft;eng_Backwards;pl_Do tyłu",
                        "f_Sprint;eng_Sprinting;pl_Bieg", // 4
                        "f_Jump;eng_Jumpings;pl_Skakanie",
                        "f_Crouch;eng_Crouching;pl_Kucanie",
                        "f_Action;eng_Action;pl_Akcja",
                        "f_AltAction;eng_Alternative action;pl_Akcja alternatywna", // 8
                        "f_Interaction;eng_Interacting;pl_Interakcje",
                        "f_DropItem;eng_Drop item;pl_Upuszczanie",
                        "f_InformationTab;eng_Information tab;pl_Menu informacji",
                        "f_Reload;eng_Reloading;pl_Przeładowywanie", // 12
                        "f_CraftingTab;eng_Crafting tab;pl_Menu tworzenia",
                        ""
                    };
                    break;
            }

            // Options display
            string[] QualityNames = new string[]{
                GS.SetString("Minimal", "Minimal"),
                GS.SetString("Low quality", "Niska jakość"),
                GS.SetString("Medium quality", "Średnia jakość"),
                GS.SetString("Good quality", "Dobra jakość"),
                GS.SetString("High quality", "Wysoka jakość")
            };
            string[] OnOff = new string[]{ GS.SetString("No", "Nie"), GS.SetString("Yes", "Tak") };

            OptionSlider.MaxA = 11;
            OptionSlider.MaxB = OptionOptions.Length;
            int Scroll = (int)Mathf.Lerp(0, Mathf.Clamp( OptionSlider.MaxB-OptionSlider.MaxA, 0, OptionSlider.MaxB-OptionSlider.MaxA ), OptionSlider.Current);

            for (int d = 0; d < 11; d++) {

                string GotOO = "";
                if(d < OptionOptions.Length) GotOO = OptionOptions[d + Scroll];

                Transform OObj = OptionButtons[d];
                string[] objVARS = {"", "", ""};
                int objBG = 2;
                bool Activated = true;
                int Clicked = 0;
                if(OObj.GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) Clicked = 1;
                if(OObj.GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(1)) Clicked = -1;

                switch(GotOO){

                    // Main
                    case "graphics":
                        objVARS = new string[]{ GS.SetString("Graphics", "Grafika"), "", "" };
                        if(Clicked == 1) OptionTitle = GotOO; objBG = 1; break;
                    case "sound":
                        objVARS = new string[]{ GS.SetString("Sounds", "Udźwiękowienie"), "", "" };
                        if(Clicked == 1) OptionTitle = GotOO; objBG = 1; break;
                    case "controls":
                        objVARS = new string[]{ GS.SetString("Controls", "Sterowanie"), "", "" };
                        if(Clicked == 1) OptionTitle = GotOO; objBG = 1; break;
                    case "camera":
                        objVARS = new string[]{ GS.SetString("Camera", "Kamera"), "", "" };
                        if(Clicked == 1) OptionTitle = GotOO; objBG = 1; break;
                    case "misc":
                        objVARS = new string[]{ GS.SetString("Misc", "Inne"), "", "" };
                        if(Clicked == 1) OptionTitle = GotOO; objBG = 1; break;
                    // Main

                    // graphics
                    case "graphs": 
                        objVARS = new string[]{ GS.SetString("Visual qualities", "Jakość efektów graficznych"), "", "" };
                        objBG = 0; Activated = false; break;
                    case "GQ":
                        objVARS = new string[]{ GS.SetString("Graphics: ", "Grafika: "), QualityNames[GS.GraphicsQuality], "" };
                        if(Clicked != 0) GS.GraphicsQuality = (5 + (GS.GraphicsQuality + Clicked ))%5;
                        break;
                    case "LQ":
                        objVARS = new string[]{ GS.SetString("Lighting: ", "Oświetlenie: "), QualityNames[GS.LightingQuality], "" };
                        if(Clicked != 0) GS.LightingQuality = (5 + (GS.LightingQuality + Clicked ))%5;
                        break;
                    case "PQ":
                        objVARS = new string[]{ GS.SetString("Particles: ", "Cząsteczki: "), QualityNames[GS.ParticlesQuality], "" };
                        if(Clicked != 0) GS.ParticlesQuality = (5 + (GS.ParticlesQuality + Clicked ))%5;
                        break;
                    case "FQ":
                        objVARS = new string[]{ GS.SetString("Foliage: ", "Roślinność: "), QualityNames[GS.GrassQuality], "" };
                        if(Clicked != 0) GS.GrassQuality = (5 + (GS.GrassQuality + Clicked ))%5;
                        break;
                    case "SQ":
                        string[] SkyboxQualities = new string[]{GS.SetString("Plain", "Blade"), GS.SetString("Static", "Statyczne"), GS.SetString("Dynamic", "Dynamiczne")};
                        objVARS = new string[]{ GS.SetString("Skybox: ", "Skybox: "), SkyboxQualities[GS.SkyboxType], "" };
                        if(Clicked != 0) GS.SkyboxType = (3 + (GS.SkyboxType + Clicked ))%3;
                        break;
                    case "camerasettings": 
                        objVARS = new string[]{ GS.SetString("Camera controls", "Kontrola kamery"), "", "" };
                        objBG = 0; Activated = false; break;
                    case "FOV":
                        objVARS = new string[]{ GS.SetString("Field of view: ", "Pole widzenia: "), ((int)GS.FOV).ToString() + "°", "" };
                        if(Clicked != 0) GS.FOV = 60 + (5+(GS.FOV + (Clicked*5) ))%65;
                        break;
                    case "CB":
                        objVARS = new string[]{ GS.SetString("Camera bobbing: ", "Trzęsienie kamery: "), ((int)(GS.CameraBobbing*100f + 0.1f)).ToString() + "%", "" };
                        if(Clicked != 0) {
                            GS.CameraBobbing = ( (GS.CameraBobbing*100f) + (Clicked*5f) ) / 100f;
                            if (GS.CameraBobbing > 1.01f) GS.CameraBobbing = 0f;
                            else if (GS.CameraBobbing < 0f) GS.CameraBobbing = 1f; }
                        break;
                    case "CS":
                        objVARS = new string[]{ GS.SetString("Camera shifting: ", "Kołysanie kamery: "), ((int)(GS.CameraShifting*100f + 0.1f)).ToString() + "%", "" };
                        if(Clicked != 0) {
                            GS.CameraShifting = ( (GS.CameraShifting*100f) + (Clicked*5f) ) / 100f;
                            if (GS.CameraShifting > 1.01f) GS.CameraShifting = 0f;
                            else if (GS.CameraShifting < 0f) GS.CameraShifting = 1f; }
                        break;
                    case "hudsettings": 
                        objVARS = new string[]{ GS.SetString("User interface settings", "Opcje interfejsu użytkiwnika"), "", "" };
                        objBG = 0; Activated = false; break;
                    case "HC":
                        objVARS = new string[]{ GS.SetString("UI color: ", "Kolor IU: "), GS.HudColorMain, "" };
                        if(Clicked != 0) {
                            switch(GS.HudColorMain){
                                case "Black": GS.HudColorMain = "White"; break;
                                case "White": GS.HudColorMain = "Rainbow"; break;
                                default: GS.HudColorMain = "Black"; break;
                            }
                        };
                        break;
                    case "MR":
                        objVARS = new string[]{ GS.SetString("Main resolution: ", "Główna rozdzielczość: "), GS.MainResolution.x + "x" + GS.MainResolution.y, "" };
                        if(Clicked != 0) {
                            for(int r = 0; r < GS.Resolutions.ToArray().Length; r++){
                                if(GS.Resolutions.ToArray()[r].ToString() == GS.MainResolution.ToString()){
                                    GS.MainResolution = ( GS.Resolutions[ (GS.Resolutions.ToArray().Length + (r+Clicked))%GS.Resolutions.ToArray().Length ] );
                                    break;
                                }
                            }
                        };
                        break;
                    case "HR":
                        objVARS = new string[]{ GS.SetString("UI resolution: ", "Rozdzielczość IU: "), GS.UIResolution.x + "x" + GS.UIResolution.y, "" };
                        if(Clicked != 0) {
                            for(int r = 0; r < GS.Resolutions.ToArray().Length; r++){
                                if(GS.Resolutions.ToArray()[r].ToString() == GS.UIResolution.ToString()){
                                    GS.UIResolution = ( GS.Resolutions[ (GS.Resolutions.ToArray().Length + (r+Clicked))%GS.Resolutions.ToArray().Length ] );
                                    break;
                                }
                            }
                        };
                        break;
                    // graphics
                    // audios
                    case "EP": 
                        objVARS = new string[]{ GS.SetString("Earpiercing: ", "Piszczenie: "), GS.EarpiercingAllowed.ToString(), "" };
                        if(Clicked != 0) GS.EarpiercingAllowed = !GS.EarpiercingAllowed; break;
                    case "volumes": 
                        objVARS = new string[]{ GS.SetString("Volumes", "Nasilenia"), "", "" };
                        objBG = 0; Activated = false; break;
                    case "MV":
                        objVARS = new string[]{ GS.SetString("Master: ", "Ogólne: "), ((int)(GS.Volumes[0]*100f)).ToString() + "%", "" };
                        if(Clicked != 0) { 
                            GS.Volumes[0] = ( (GS.Volumes[0]*100f) + (Clicked*5f) ) / 100f;
                            if (GS.Volumes[0] > 1.01f) GS.Volumes[0] = 0f;
                            else if (GS.Volumes[0] < 0f) GS.Volumes[0] = 1f; 
                            }
                        break;
                    case "SV":
                        objVARS = new string[]{ GS.SetString("Sound effects: ", "Efekty dźwiękowe: "), ((int)(GS.Volumes[2]*100f)).ToString() + "%", "" };
                        if(Clicked != 0) {
                            GS.Volumes[2] = ( (GS.Volumes[2]*100f) + (Clicked*5f) ) / 100f;
                            if (GS.Volumes[2] > 1.01f) GS.Volumes[2] = 0f;
                            else if (GS.Volumes[2] < 0f) GS.Volumes[2] = 1f;}
                        break;
                    case "BGM":
                        objVARS = new string[]{ GS.SetString("Music: ", "Muzyka: "), ((int)(GS.Volumes[1]*100f)).ToString() + "%", "" };
                        if(Clicked != 0) {
                            GS.Volumes[1] = ( (GS.Volumes[1]*100f) + (Clicked*5f) ) / 100f;
                            if (GS.Volumes[1] > 1.01f) GS.Volumes[1] = 0f;
                            else if (GS.Volumes[1] < 0f) GS.Volumes[1] = 1f;}
                        break;
                    // audios
                    // misc
                    case "LG": 
                        objVARS = new string[]{ GS.SetString("Language: ", "Język: "), GS.SetString("English", "Polski"), "" };
                        if(Clicked != 0){
                            switch(GS.Language){
                                case "English": GS.Language = "Polski"; break;
                                default: GS.Language = "English"; break;
                            }
                        }
                        break;
                    case "LC": 
                        objVARS = new string[]{ GS.SetString("Laser color: ", "Kolor laserów: "), "Hue_" + GS.LaserColor.ToString() + ";", "HueBLOCK" };
                        if(Clicked != 0) GS.LaserColor = (10 + (GS.LaserColor + Clicked))%10; break;
                    case "goresettings": 
                        objVARS = new string[]{ GS.SetString("Brutality settings", "Opcje przemocy"), "", "" };
                        objBG = 0; Activated = false; break;
                    case "RD": 
                        objVARS = new string[]{ GS.SetString("Ragdolls: ", "Ragdolle: "), GS.Ragdolls.ToString(), "" };
                        if(Clicked != 0) GS.Ragdolls = !GS.Ragdolls; break;
                    // misc
                    // controls
                    case "MS":
                        objVARS = new string[]{ GS.SetString("Sensitivity: ", "Czułość: "), ((int)GS.MouseSensitivity).ToString(), "" };
                        if(Clicked != 0) GS.MouseSensitivity = (31f + (GS.MouseSensitivity + Clicked))%31f; break;
                    case "MSM":
                        objVARS = new string[]{ GS.SetString("Smoothness: ", "Gładkość: "), ((int)(GS.MouseSmoothness*100f)).ToString() + "%", "" };
                        if(Clicked != 0) GS.MouseSmoothness = ((10f + (GS.MouseSmoothness*10f + Clicked/2f))%10f)/10f; break;
                    case "MI": 
                        objVARS = new string[]{ GS.SetString("Inverted Y axis: ", "Odwrócona oś Y: "), GS.InvertedMouse.ToString(), "" };
                        if(Clicked != 0) GS.InvertedMouse = !GS.InvertedMouse; break;
                    case "keybinds": 
                        objVARS = new string[]{ GS.SetString("Key bindings", "Przypisanie klawiszy"), "", "" };
                        objBG = 0; Activated = false; break;
                    case "kb0": case "kb1": case "kb2": case "kb3": case "kb4": case "kb5": case "kb6": case "kb7": case "kb8": case "kb9": case "kb10": case "kb11": case "kb12": case "kb13": 
                        int KBindes = int.Parse(GotOO.Substring(2));
                        string docelKeybind = GS.GetSemiClass(Keybinders[KBindes], "f_");
                        for(int ca = 0; ca < GS.Controlls.Length; ca++){
                            if(GS.Controlls[ca].Name == docelKeybind) {docelKeybind = GS.Controlls[ca].Key.ToString();
                                if(Clicked != 0) {
                                    InputTopic = "KeyBind";
                                    InputSubject = ca.ToString();
                                }
                                break;
                            }
                        }
                        objVARS = new string[]{ 
                            GS.SetString(GS.GetSemiClass(Keybinders[KBindes], "eng_"), GS.GetSemiClass(Keybinders[KBindes], "pl_")), 
                            docelKeybind, 
                            "" };
                        break;
                    // controls

                    default:
                        objBG = 0; Activated = false; break;
                }

                OObj.GetChild(0).GetComponent<Text>().text = objVARS[0];
                OObj.GetChild(1).GetComponent<Text>().text = objVARS[1];
                if(objVARS[2] == "") OObj.GetChild(2).GetComponent<Image>().sprite = OptionImages[0];
                else foreach(Sprite FindSprit in OptionImages) {
                    if (FindSprit.name == objVARS[2]) OObj.GetChild(2).GetComponent<Image>().sprite = FindSprit;
                    if (GS.ExistSemiClass(objVARS[1], "Hue_")) {
                        OObj.GetChild(1).GetComponent<Text>().text = "";
                        OObj.GetChild(2).GetComponent<Image>().color = Color.HSVToRGB( float.Parse(GS.GetSemiClass(objVARS[1], "Hue_"))/10f , 1f, 1f);
                    }
                }

                OObj.GetComponent<Image>().sprite = OptionImages[objBG];
                OObj.GetComponent<ButtonScript>().Active = Activated;

            }
            // Options display



        } else if (!Show && Options.GetChild(0).GetComponent<HudColorControl>().Alpha > 0f){

            Options.position = SH[0].position;
            Options.localScale = Vector3.one * Mathf.Lerp(1f, 1.1f, FromStart * 3f);
            Options.GetChild(0).GetComponent<HudColorControl>().Alpha = Mathf.MoveTowards(Options.GetChild(0).GetComponent<HudColorControl>().Alpha, 0f, 0.1f * (Time.unscaledDeltaTime*50f));
            Options.GetChild(0).GetComponent<HudColorControl>().SetColor("");

            OptionButtons[0].parent.localScale = Vector3.zero;
            OptionSlider.Current = 0f;
            OptionSlider.Selected = 0;
            OptionSlider.Active = false;
            OptionSlider.gameObject.SetActive(false);

        } else {

            Options.position = SH[1].position;
            Options.localScale = Vector3.zero;

            OptionButtons[0].parent.localScale = Vector3.zero;
            OptionSlider.Current = 0f;
            OptionSlider.Selected = 0;
            OptionSlider.Active = false;
            OptionSlider.gameObject.SetActive(false);
            OptionTitle = "main";

        }

    }

    void WhileGamePick(string Show = ""){

        if (Show == "NewGame" || Show == "LoadGame"){

            LogoAlpha = 0f;
            PickGame.position = SH[0].position;
            PickGame.localScale = Vector3.one;
            switch (Show) {
                case "LoadGame":
                    PGbuttons[0].parent.localScale = Vector3.one;
                    GMimage.transform.parent.localScale = Vector3.zero;
                    PickGame.GetChild(0).GetChild(0).GetComponent<Text>().text = GS.SetString("CHOOSE A GAME FILE", "WYBIERZ PLIK Z GRĄ");

                    // Choosing files
                    string[] ListOfSaveFiles = new string[]{}; 
                    if(PlayerPrefs.HasKey("Saves")) {
                        List<string> AvaSaves = new List<string>();
                        foreach(string chick in GS.ListSemiClass(PlayerPrefs.GetString("Saves"), "©")) if (GS.GetSemiClass(GS.GetSemiClass(chick, "rs", "®"), "P", "?") == PS.ProfileID.ToString() || GS.GetSemiClass(GS.GetSemiClass(chick, "rs", "®"), "P", "?") == "1")
                            AvaSaves.Add(chick);
                        ListOfSaveFiles = AvaSaves.ToArray();
                    }
                    if (SelectedFile >= ListOfSaveFiles.Length) SelectedFile = -1;

                    for(int sb = 0; sb < 18; sb++){

                        Text PGtext = PGbuttons[sb].GetComponent<Text>();

                        if (sb < ListOfSaveFiles.Length){
                            string SaveInfo = ListOfSaveFiles[sb];
                            if(GS.GetSemiClass(GS.GetSemiClass(SaveInfo, "rs", "®"), "P", "?") == "1") PGtext.text = "> " + GS.GetSemiClass(SaveInfo, "sn", "®");
                            else {
                                string OwnerProfile = "";
                                for(int gp = 0; gp < GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©").Length; gp++) if (GS.GetSemiClass(GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©")[gp], "ID_", "®") == GS.GetSemiClass(GS.GetSemiClass(SaveInfo, "rs", "®"), "P", "?")){
                                    OwnerProfile = GS.GetSemiClass(GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©")[gp], "PN_", "®");
                                    break;
                                }
                                PGtext.text = "> " + OwnerProfile + " - " + GS.GetSemiClass(SaveInfo, "sn", "®");
                            }

                            if(PGbuttons[sb].GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)){
                                SelectedFile = sb;
                            }
                        } else if (sb == ListOfSaveFiles.Length){
                            PGtext.text = GS.SetString("+ NEW GAME", "+ NOWA GRA");
                            if(PGbuttons[sb].GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)){
                                GMstringers[0] = FNcheck();
                                CurrentWindow = "NewGame";
                            }
                        } else {
                            PGtext.text = "";
                        }

                    }

                    // Descriptions
                    if (SelectedFile >= 0){
                        PGdescButts[0].localScale = PGdescButts[1].localScale = PGdescButts[2].localScale = Vector3.one;
                        PGdescButts[0].GetChild(0).GetComponent<Text>().text = GS.SetString("Back", "Wróć");
                        PGdescButts[1].GetChild(0).GetComponent<Text>().text = GS.SetString("ERASE", "USUŃ");
                        PGdescButts[2].GetChild(0).GetComponent<Text>().text = GS.SetString("PLAY", "ZAGRAJ");
                        if (Input.GetMouseButtonDown(0))
                            if (PGdescButts[0].GetComponent<ButtonScript>().IsSelected) CurrentWindow = "Main";
                            else if (PGdescButts[1].GetComponent<ButtonScript>().IsSelected) Warning = new string[]{"EraseSave", GS.GetSemiClass(ListOfSaveFiles[SelectedFile], "id", "®")};
                            else if (PGdescButts[2].GetComponent<ButtonScript>().IsSelected) {
                            
                                GS.CurrentSave = int.Parse(GS.GetSemiClass(ListOfSaveFiles[SelectedFile], "id", "®"));
                                LoadingTime = Random.Range(1f, 3f);
                                AfterLoading = "f_LoadGame";

                            }


                        string[] GameModes = {GS.SetString("Classic", "Klasyczny"), GS.SetString("Horde", "Horda"), GS.SetString("Casual", "Niedzielny")};
                        string[] DiffLevels = {GS.SetString("Easy", "Łatwy"), GS.SetString("Normal", "Normalny"), GS.SetString("Hard", "Trudny"), GS.SetString("Very hard", "Bardzo trudny"), GS.SetString("HARDCORE", "HARDKOROWY")};
                        int[] MD = {int.Parse(GS.GetSemiClass(GS.GetSemiClass(ListOfSaveFiles[SelectedFile], "rs", "®"), "G", "?")), int.Parse(GS.GetSemiClass(GS.GetSemiClass(ListOfSaveFiles[SelectedFile], "rs", "®"), "D", "?"))};
                        PGdesc.text = GS.GetSemiClass(ListOfSaveFiles[SelectedFile], "sn", "®") + GS.SetString("\nGame mode: ", "\nTryb gry:") + GameModes[MD[0]] + GS.SetString("\nDifficulty level: ", "\nPoziom trudności:") + DiffLevels[MD[1]-1];
                    } else {
                        //PGdesc.text = GS.SetString("Start a new game, or continue a saved one.", "Rozpocznij nową grę, albo kontynuuj którąś z zapisanych.");
                        PGdesc.text = "Save file registry: " + PlayerPrefs.GetString("Saves");
                        PGdescButts[1].localScale = PGdescButts[2].localScale = Vector3.zero;
                        PGdescButts[0].localScale = Vector3.one;
                        PGdescButts[0].GetChild(0).GetComponent<Text>().text = GS.SetString("Back", "Wróć");
                        if (PGdescButts[0].GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) CurrentWindow = "Main";
                    }

                    break;
                case "NewGame":
                    SelectedFile = -1;
                    PGbuttons[0].parent.localScale = Vector3.zero;
                    GMimage.transform.parent.localScale = Vector3.one;
                    PickGame.GetChild(0).GetChild(0).GetComponent<Text>().text = GS.SetString("PREPARE A NEW GAME", "ROZPOCZNIJ NOWĄ GRĘ");

                    // Option buttons
                    PGdesc.text = "...";
                    string[] OptionIDS = {"FN", "PD", "", "GM", "DL"};
                    switch(GMintegers[0]){
                        case 1:
                            OptionIDS = new string[]{"FN", "PD", "", "GM", "DL", "HM"};
                            break;
                    }

                    for(int sb = 0; sb < 8; sb++){
                        if (sb < OptionIDS.Length) {
                            GMoptions[sb].localScale = Vector3.one;
                            Text GMtext = GMoptions[sb].GetChild(0).GetComponent<Text>();
                            switch (OptionIDS[sb]){
                                case "FN":
                                    // STRING 0
                                    GMtext.text = GS.SetString("Filename: ", "Nazwa pliku: ") + GMstringers[0];

                                    if (GMoptions[sb].GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0))
                                        InputTopic = "Filename";
                                    break;
                                case "GM":
                                    // INTEGER 0
                                    if (GMintegers[0] > 2) GMintegers[0] = 0;
                                    GMimage.sprite = GMsprited[GMintegers[0]];

                                    string[] GameModes = {GS.SetString("Classic", "Klasyczny"), GS.SetString("Horde", "Horda"), GS.SetString("Casual", "Niedzielny")};
                                    string[] GameModeDESCs = {
                                            GS.SetString("CLASSIC\nLeap from one map to another, to avoid nukes. Gather resources, and keep yourself fed!", "TRYB KLASYCZNY\nSkacz z mapy na mapę, by unikać bomb atomowych. Zbieraj zasoby, i jedz coś po drodze!"), 
                                            GS.SetString("HORDE\nDefend your position from oncoming hordes of mutants. Don't worry about food and other resources, just shoot the mutants!", "HORDA\nBroń swojej pozycji przed hordami mutantów. Nie przejmuj się jedzeniem i zasobami, tylko mordowaniem mutantów!"), 
                                            GS.SetString("CASUAL\nSame as classic, albeit simplified. There's no hunger, enemies are easier to kill, and other mechanics have been simplified aswell.", "TRYB NIEDZIELNY\nTaki jak klasyczny, tylko uproszczony. Nie ma głodu, wrogowie są łatwiejsi, i inne mechaniki również zostały uproszczone.")
                                        };
                                    GMimage.transform.GetChild(0).GetComponent<Text>().text = GameModeDESCs[GMintegers[0]];
                                    GMtext.text = GS.SetString("Game mode: ", "Tryb gry: ") + GameModes[GMintegers[0]];

                                    if (GMoptions[sb].GetComponent<ButtonScript>().IsSelected) {
                                        PGdesc.text = GS.SetString("Game mode\nDetermines rules of the game. Some modes might be slightly altered versions of classic mode, and some might be completely different experiences.", "Tryb gry\nOdpowiada za zasady gry. Niektóre tryby to lekko zmodyfikowane wersje trybu klasycznego, zaś inne, stanowią zupełnie innowatywne doświadczenia.");
                                        if(Input.GetMouseButtonDown(0)) GMintegers[0] = (GMintegers[0] + 1)%3;
                                    }
                                    break;
                                case "DL":
                                    // INTEGER 1
                                    if (GMintegers[1] > 4) GMintegers[1] = 0;

                                    string[] DiffLevels = {GS.SetString("Easy", "Łatwy"), GS.SetString("Normal", "Normalny"), GS.SetString("Hard", "Trudny"), GS.SetString("Very hard", "Bardzo trudny"), GS.SetString("HARDCORE", "HARDKOROWY")};
                                    GMtext.text = GS.SetString("Difficulty level: ", "Poziom trudności: ") + DiffLevels[GMintegers[1]];

                                    if (GMoptions[sb].GetComponent<ButtonScript>().IsSelected) {
                                        PGdesc.text = GS.SetString("Difficulty level\nDetermines the lenght of rounds, rate at which terrain deteriorates, the amount of damage you take, and much more.", "Poziom trudności\nOdpowiada za długość rund, prędkości degradacji środowiska, ilości obrażeń otrzymywanych przez ciebie, i wiele innych.");
                                        if(Input.GetMouseButtonDown(0)) GMintegers[1] = (GMintegers[1] + 1)%5;
                                    }
                                    break;
                                case "PD":
                                    // INTEGER 2
                                    if (GMintegers[2] > 1) GMintegers[2] = 0;

                                    string[] PDbools = {GS.SetString("Profile dependency: Enabled", "Zależność od profilu: Włączona"), GS.SetString("Profile dependency: Disabled", "Zależność od profilu: Wyłączona")};
                                    GMtext.text = PDbools[GMintegers[2]];

                                    if (GMoptions[sb].GetComponent<ButtonScript>().IsSelected) {
                                        PGdesc.text = GS.SetString(
                                            "Profile dependency\nWhen disabled, stats gained in this file will not be saved on your profile. You also won't be able to use stuff from your profile. Final score will still be saved in the records.", 
                                            "Zależność od profilu\nWyłączona, nie będzie zapisywać statystyk z tego pliku na twoim profilu. Również nie będziesz mógł pobierać rzeczy ze swojego profilu. Wynik końcowy i tak zostanie zapisany w rekordach.");
                                        if(Input.GetMouseButtonDown(0)) GMintegers[2] = (GMintegers[2] + 1)%2;
                                    }
                                    break;
                                case "HM":
                                    // INTEGER 3
                                    if (GMintegers[3] > 5) GMintegers[3] = 0;

                                    string[] HordeMaps = {
                                        GS.SetString("Shopping mall", "Galeria handlowa"), 
                                        GS.SetString("Trenches", "Okopy"), 
                                        GS.SetString("Disco club", "Klub disco"),
                                        GS.SetString("Sewers", "Ścieki"), 
                                        GS.SetString("Roofs", "Dachy"), 
                                        GS.SetString("Ship", "Statek")
                                    };
                                    GMtext.text = GS.SetString("Horde map: ", "Mapa hordy: ") + HordeMaps[GMintegers[3]];

                                    if (GMoptions[sb].GetComponent<ButtonScript>().IsSelected) {
                                        PGdesc.text = GS.SetString("Horde map\nHorde mode is played not on a randomly generated map, but rather, on a prebuilt one.", "Mapa hordy\nTryb hordy nie jest rozgrywany na losowo generowanej mapie, lecz na już swtorzonej.");
                                        if(Input.GetMouseButtonDown(0)) GMintegers[3] = (GMintegers[3] + 1)%6;
                                    }
                                    break;
                                default:
                                    GMoptions[sb].localScale = Vector3.zero;
                                    break;
                            }
                        } else {
                            GMoptions[sb].localScale = Vector3.zero;
                        }
                    }

                    // Final options
                    PGdescButts[0].localScale = PGdescButts[2].localScale = Vector3.one;
                    PGdescButts[1].localScale = Vector3.zero;
                    PGdescButts[0].GetChild(0).GetComponent<Text>().text = GS.SetString("Back", "Wróć");
                    if (PGdescButts[0].GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) CurrentWindow = "LoadGame";
                    PGdescButts[2].GetChild(0).GetComponent<Text>().text = GS.SetString("START", "START");
                    if (PGdescButts[2].GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) {
                    
                    
                        // STARTING THE GAME, PREPARE THE VALUES
                        GS.CurrentSave = Random.Range(10000, 99999);
                        string RoundSetters = "" 
                        + "G" + GMintegers[0].ToString() // Gamemode
                        + "?D" + (GMintegers[1]+1).ToString() // Difflevel
                        ;

                        if (GMintegers[2] == 0) RoundSetters += "?P" + PS.ProfileID.ToString();
                        else RoundSetters += "?P1";

                        if(GMintegers[0] == 1) RoundSetters += "?H" + GMintegers[3].ToString(); // Horde map
                        
                        GS.SaveFileName = GMstringers[0];
                        GS.RoundSetting = RoundSetters;

                        LoadingTime = Random.Range(1f, 3f);
                        AfterLoading = "f_StartGame";
                    
                    }
                    break;
            }

        } else {

            PickGame.position = SH[1].position;
            PickGame.localScale = Vector3.zero;
            SelectedFile = -1;

        }

    }

    void WhileResults(string Ending = ""){

        if (Ending != "") {

            ResultsMenu.position = SH[0].position;
            ResultsMenu.localScale = Vector3.one;

            // Stages
            string[] RoundTypeName = {GS.SetString(" round", " rundy"), GS.SetString(" wave", " fali"), GS.SetString(" rounds", " rund"), GS.SetString(" waves", " fal")};
            switch(CheckoutStage){
                case 0:
                    // Preparations
                    GS.UpdateRecord();

                    RSfilename = GS.GetSemiClass(GS.NeueScore[0], "N");
                    RSgamemode = int.Parse(GS.GetSemiClass(GS.NeueScore[0], "G"));
                    RSdifflevel = int.Parse(GS.GetSemiClass(GS.NeueScore[0], "D"));
                    RSscore = int.Parse(GS.GetSemiClass(GS.NeueScore[0], "S"));
                    RSrounds = int.Parse(GS.GetSemiClass(GS.NeueScore[0], "R"));
                    RStempstats = GS.ListSemiClass(GS.NeueScore[1]);

                    foreach(Transform GetLogo in ResultsMenu.GetChild(0)){
                        if(Ending == "GameOver" && GetLogo.name == GS.Language + "-Dead") {
                            GetLogo.SetSiblingIndex(0);
                            GetLogo.localScale = Vector3.one;
                            GetLogo.GetComponent<Image>().fillAmount = 0f;
                        }else GetLogo.localScale = Vector3.zero;
                    }
                    ResultsMenu.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    ScoreRoundText.text = ResultTextes[0].text = ResultTextes[1].text = ResultTextes[2].text = "";
                    ResultsAnyKey.text = GS.SetString("Press any key to continue", "Naciśnij dowolny klawisz aby kontynuować");
                    ResultsAnyKey.color = new Color(0f,0f,0f,0f);
                    ResultsBG.transform.localScale = Vector3.zero;

                    FadeInValue = 0f;
                    ToCheckout = 0;
                    CheckoutStage = 1;
                    break;
                case 1:
                    // Fade in sequence
                    FadeInValue += 0.02f * (Time.unscaledDeltaTime*50f);
                    if (FadeInValue < 1f) {
                        ResultsFadeIn.color = new Color(0f,0f,0f, 1f-FadeInValue);
                    } else if (FadeInValue < 2f){
                        ResultsMenu.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = FadeInValue-1f;
                    } else if (FadeInValue >= 3f && FadeInValue < 4f){
                        ResultsMenu.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(0f, 300f), FadeInValue-3f);
                    } else if (FadeInValue >= 4f) {
                        ResultsBG.transform.localScale = Vector3.one;
                        CheckoutStage = 2;
                        FadeInValue = 0f;
                        ScoreRoundText.text = GS.SetString("You haven't survived a single ", "Nie przetrwałeś żadnej ") + RoundTypeName[RSgamemode];
                    }
                    break;
                case 2:
                    // Amount of rounds
                    if(FadeInValue > 0f){
                        FadeInValue -= 0.02f * (Time.unscaledDeltaTime*50f);
                    } else if (ToCheckout < RSrounds){
                        ToCheckout ++;
                        FadeInValue = Mathf.Lerp(0.5f, 0.02f, RSrounds/100f);
                        ScoreRoundText.text = GS.SetString("You have survived ", "Przetrwałeś ") + ToCheckout.ToString() + RoundTypeName[RSgamemode+2];
                    } else if (ToCheckout == RSrounds){
                        ToCheckout ++;
                        FadeInValue = 1f;
                    } else {
                        ToCheckout = 0;
                        CheckoutStage = 3;
                    }
                    break;
                case 3:
                    // Amount of score
                    string Prevtext = GS.SetString("You haven't survived a single ", "Nie przetrwałeś żadnej") + RoundTypeName[RSgamemode] + "\n";
                    if (RSrounds > 0) Prevtext = GS.SetString("You have survived ", "Przetrwałeś ") + RSrounds.ToString() + RoundTypeName[RSgamemode+2] + "\n";

                    if(FadeInValue > 0f){
                        FadeInValue -= 0.02f * (Time.unscaledDeltaTime*50f);
                    } else if (ToCheckout < RSscore){
                        ToCheckout = Mathf.Clamp(ToCheckout + (int)Mathf.Clamp(RSscore/100, 1, Mathf.Infinity), 0, RSscore);
                        FadeInValue = Mathf.Lerp(0.04f, 0.001f, RSrounds/1000f);
                        Prevtext += GS.SetString("Score gained ", "Zdobyty wynik ") + ToCheckout.ToString();
                        ScoreRoundText.text = Prevtext;
                    } else if (ToCheckout == RSrounds){
                        ToCheckout ++;
                        if (RSscore == 0 && RSrounds == 0) {
                            Prevtext += GS.SetString("nor have you gained any score", "i nie zdobyłeś żadnego wyniku");
                            ScoreRoundText.text = Prevtext;
                        }
                        FadeInValue = 1f;
                    } else {
                        ToCheckout = 1337;
                        CheckoutStage = 4;
                        FadeInValue = 1f;
                    }
                    
                    break;
                case 4:
                    // Aditional stats
                    if(ToCheckout == 1337){
                        if(RStempstats.Length > 0) ScoreRoundText.text += "\n\n" + GS.SetString("Aditional statistics:", "Dodatkowe statystyki:");
                        else if (RSscore > 0 || RSrounds > 0) ScoreRoundText.text += "\n\n" + GS.SetString("No aditional statistics", "Brak dodatkowych statystyki");
                        else ScoreRoundText.text += "\n\n" + GS.SetString("That's pathetic...", "Żałosna robota...");
                        ToCheckout = 0;
                    }

                    if(FadeInValue > 0f){
                        FadeInValue -= 0.02f * (Time.unscaledDeltaTime*50f);
                    } else if (ToCheckout < RStempstats.Length){
                        FadeInValue = 0.5f;
                        if(GS.GetStatName(RStempstats[ToCheckout], 1) != "misc."){
                            if (RStempstats.Length < 29){
                                ResultTextes[0].text += GS.GetStatName(RStempstats[ToCheckout]) + "\n";
                            } else if (ToCheckout <= 29) {
                                ResultTextes[1].text += GS.GetStatName(RStempstats[ToCheckout]) + "\n";
                            } else {
                                ResultTextes[2].text += GS.GetStatName(RStempstats[ToCheckout]) + "\n";
                            }
                        }
                        ToCheckout ++;
                    } else {
                        CheckoutStage = 5;
                    }
                    break;
                case 5:

                    // Quickly the visuals
                    ResultsMenu.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1f;
                    ResultsMenu.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 300f);
                    ResultsBG.transform.localScale = Vector3.one;
                    ResultsFadeIn.color = new Color(0f,0f,0f,0f);

                    // Quickly set the data again
                    if(RSrounds <= 0) ScoreRoundText.text = GS.SetString("You haven't survived a single ", "Nie przetrwałeś żadnej ") + RoundTypeName[RSgamemode] + "\n";
                    else ScoreRoundText.text = GS.SetString("You have survived ", "Przetrwałeś ") + RSrounds.ToString() + RoundTypeName[RSgamemode+2] + "\n";

                    if(RSscore <= 0) ScoreRoundText.text += GS.SetString("nor have you gained any score ", "i nie zdobyłeś żadnego wyniku ")+ "\n\n";
                    else ScoreRoundText.text += GS.SetString("Score gained ", "Zdobyty wynik ") + RSscore.ToString() + "\n\n";

                    if(RStempstats.Length > 0) ScoreRoundText.text += GS.SetString("Aditional statistics:", "Dodatkowe statystyki:");
                    else if (RSscore > 0 || RSrounds > 0) ScoreRoundText.text += GS.SetString("No aditional statistics", "Brak dodatkowych statystyki");
                    else ScoreRoundText.text += GS.SetString("That's pathetic...", "Żałosna robota...");

                    ResultTextes[0].text = ResultTextes[1].text = ResultTextes[2].text = "";
                    for (int pts = 0; pts < RStempstats.Length; pts++) {
                        if(GS.GetStatName(RStempstats[pts], 1) != "misc."){
                            if (RStempstats.Length < 29){
                                ResultTextes[0].text += GS.GetStatName(RStempstats[pts]) + "\n";
                            } else if (pts <= 29) {
                                ResultTextes[1].text += GS.GetStatName(RStempstats[pts]) + "\n";
                            } else {
                                ResultTextes[2].text += GS.GetStatName(RStempstats[pts]) + "\n";
                            }
                        }
                    }

                    // Finisher
                    CheckoutStage = 6;
                    FadeInValue = 0f;
                    break;
                case 6:
                    FadeInValue += 0.02f * (Time.unscaledDeltaTime*50f);
                    ResultsAnyKey.color = new Color(1f,1f,1f, Mathf.Abs(Mathf.Sin(FadeInValue)));
                    break;
            }

            // Skipping
            if (Input.anyKeyDown) {
                if (CheckoutStage == 6) {
                    CurrentWindow = "Main";
                } else if (CheckoutStage > 1 || (CheckoutStage == 1 && FadeInValue > 1f)) {
                    CheckoutStage = 5;
                }
            }

        } else {

            ResultsMenu.position = SH[1].position;
            ResultsMenu.localScale = Vector3.zero;
            FadeInValue = 0f;
            ToCheckout = CheckoutStage = 0;

        }

    }

    void WhileRecords(bool Shown = false){

        if(Shown){

            LogoAlpha = 0f;
            RecordsWindow.position = SH[0].position;
            RecordsWindow.localScale = Vector3.one * Mathf.Lerp(0.75f, 1f, FromStart * 3f);
            if(RecordsWindow.GetChild(0).GetComponent<HudColorControl>().Alpha < 1f) {
                RecordsWindow.GetChild(0).GetComponent<HudColorControl>().Alpha = Mathf.MoveTowards(RecordsWindow.GetChild(0).GetComponent<HudColorControl>().Alpha, 1f, 0.1f * (Time.unscaledDeltaTime*50f));
                RecordsWindow.GetChild(0).GetComponent<HudColorControl>().SetColor("");
            }

            RecordTitles[0].text = GS.SetString("PROFILE RECORDS", "REKORDY TEGO PROFILU");
            RecordTitles[1].text = GS.SetString("SORT BY:", "SORTUJ:");
            RecordTitles[2].text = GS.SetString("FILTERS:", "FILTRY:");

            // Refresh button
            RecordRefresh.text = GS.SetString("REFRESH", "ODŚWIEŻ");
            if(RecordRefresh.GetComponent<ButtonScript>().IsSelected && Input.GetMouseButton(0))
                SortRecords();
            if(RecordClose.GetComponent<ButtonScript>().IsSelected && Input.GetMouseButton(0))
                CurrentWindow = "Main";

            // Sort buttons
            foreach(ButtonScript SortButton in RecordSortingButtons){
                if(SortButton.IsSelected && Input.GetMouseButtonDown(0)) RecordSort = SortButton.gameObject.name;
                if(SortButton.gameObject.name == RecordSort) SortButton.GetComponent<Text>().text = "▣ ";
                else SortButton.GetComponent<Text>().text = "□ ";

                string buttText = "";
                switch(SortButton.gameObject.name){
                    case "SH": buttText = GS.SetString("Score descending", "Wynikiem malejąco"); break;
                    case "SL": buttText = GS.SetString("Score rising", "Wynikiem rosnąco"); break;
                    case "AZ": buttText = GS.SetString("From A to Z", "Od A do Z"); break;
                    case "ZA": buttText = GS.SetString("From Z to A", "Od Z do A"); break;
                    case "OL": buttText = GS.SetString("Newest to oldest", "Od najnowszych"); break;
                    case "NE": buttText = GS.SetString("Oldest to newest", "Od najstarszych"); break;
                }
                SortButton.GetComponent<Text>().text += buttText;
            }

            // Filter buttons
            for(int filbut = 0; filbut < 2; filbut++){
                ButtonScript FilterButton = RecordFilterButtons[filbut];
                Text FilterText = FilterButton.GetComponent<Text>();
                int FilterValue = RecordFilters[filbut];
                if(FilterButton.IsSelected && Input.GetMouseButtonDown(0)){
                    RecordFilters[filbut] += 1;
                }

                switch(filbut){
                    case 0:
                        FilterText.text = GS.SetString("Game modes: ", "Tryby gry: ");
                        if(FilterValue == -1) FilterText.text += GS.SetString("all", "wszystkie");
                        else if(FilterValue == 0) FilterText.text += GS.SetString("classic", "klasyczny");
                        else if(FilterValue == 1) FilterText.text += GS.SetString("horde", "hordy");
                        else if(FilterValue == 2) FilterText.text += GS.SetString("casual", "niedzielny");
                        else RecordFilters[filbut] = -1;
                        break;
                    case 1:
                        FilterText.text = GS.SetString("Difficulty level: ", "Poziom trudności: ");
                        if(FilterValue == -1) FilterText.text += GS.SetString("all", "wszystkie");
                        else if(FilterValue == 0) FilterText.text += GS.SetString("easy", "łatwy");
                        else if(FilterValue == 1) FilterText.text += GS.SetString("normal", "normalny");
                        else if(FilterValue == 2) FilterText.text += GS.SetString("hard", "trudny");
                        else if(FilterValue == 3) FilterText.text += GS.SetString("very hard", "bardzo trudny");
                        else if(FilterValue == 4) FilterText.text += GS.SetString("HARDCORE", "HARDKOROWY");
                        else RecordFilters[filbut] = -1;
                        break;
                }
            }

            // Displayed records
            string[] gmrec = {GS.SetString("Classic", "Klasyczny"), GS.SetString("Horde", "Horda"), GS.SetString("Casual", "Niedzielny")};
            string[] gmdl = {GS.SetString("Easy", "Łatwy"), GS.SetString("Normal", "Normalny"), GS.SetString("Hard", "Trudny"), GS.SetString("Very hard", "Bardzo trudny"), GS.SetString("Hardcore", "Hardkorowy")};
            for(int SR = 0; SR < 30; SR++){
                string DisplayText = "";
                Color DisplayColor = new Color(0f, 0f, 0f, 0f);
                if (SR < RecordList.Length) {
                    DisplayText = GS.GetSemiClass(RecordList[SR], "N") + " - " + GS.SetString("rounds: ", "rundy: ") + GS.GetSemiClass(RecordList[SR], "R") + GS.SetString(" / score: ", " / wynik: ") + GS.GetSemiClass(RecordList[SR], "S") + " - " + gmdl[int.Parse(GS.GetSemiClass(RecordList[SR], "D"))-1] + " " + gmrec[int.Parse(GS.GetSemiClass(RecordList[SR], "G"))];
                    DisplayColor = Color.white;
                }
                RecordOptions[SR].GetComponent<Text>().text = DisplayText;
                RecordOptions[SR].GetComponent<Text>().color = DisplayColor;
            }

        } else if (!Shown && RecordsWindow.GetChild(0).GetComponent<HudColorControl>().Alpha > 0f){

            RecordsWindow.position = SH[0].position;
            RecordsWindow.localScale = Vector3.one * Mathf.Lerp(1f, 1.1f, FromStart * 3f);
            RecordsWindow.GetChild(0).GetComponent<HudColorControl>().Alpha = Mathf.MoveTowards(RecordsWindow.GetChild(0).GetComponent<HudColorControl>().Alpha, 0f, 0.1f * (Time.unscaledDeltaTime*50f));
            RecordsWindow.GetChild(0).GetComponent<HudColorControl>().SetColor("");

            RecordList = new string[]{};

        } else {

            RecordsWindow.position = SH[1].position;
            RecordsWindow.localScale = Vector3.zero;

            RecordList = new string[]{};

        }

    }

    void WhileProfileMenu(string Tab = ""){

        if (Tab != ""){
            ProfileMenu.position = Vector3.Lerp(SH[1].position, SH[0].position, FromStart*3f);
            ProfileMenu.localScale = Vector3.one;

            // Read stats
            if(PMstats.Length <= 9999){
                string[] ReadProfileStats = GS.ListSemiClass(PS.Statistics);
                List<string> toPMstats = new List<string>();
                List<string> Main = new List<string>();
                List<string> Misc = new List<string>();
                for(int ps = 0; ps < ReadProfileStats.Length; ps++){
                    string n = GS.GetStatName(ReadProfileStats[ps], 1);
                    if(n == "TotalScore" || n == "TotalRounds" || n == "HighestScore" || n == "TotalWaves" || n == "MostWaves"|| n == "MostRounds" || n == "LongestSurvivedTime" || n == "SurvivedTime") Main.Add(GS.GetStatName(ReadProfileStats[ps]));
                    else if (n == "MapDiscovered" && GS.ExistSemiClass(PS.Statistics, "TotalRounds_")) Misc.Add(GS.SetString("Average map discovery: ", "Przeciętne zbadanie mapy: ") + (int.Parse(GS.GetStatName(ReadProfileStats[ps], 2)) / Mathf.Clamp(int.Parse(GS.GetSemiClass(PS.Statistics, "TotalRounds_")), 0, 9999) ).ToString() + "%" );
                    else Misc.Add(GS.GetStatName(ReadProfileStats[ps]));
                }
                for(int adder = 0; adder < Main.ToArray().Length + Misc.ToArray().Length; adder++){
                    if(adder < Main.ToArray().Length) {
                        if(adder == 0) {toPMstats.Add(GS.SetString("MAIN SCORE", "GŁÓWNE WYNIKI"));}
                        toPMstats.Add(Main.ToArray()[adder]);
                    } else {
                        if(adder == Main.ToArray().Length) {toPMstats.Add(""); toPMstats.Add(GS.SetString("MISCELLANEOUS STATS", "GŁOWNE STATYSTYKI"));}
                        toPMstats.Add(Misc.ToArray()[adder-Main.ToArray().Length]);
                    }
                }
                PMstats = toPMstats.ToArray();
            }

            WhatTabs.text = "";
            string tabName = Tab;
            for(int pt = 0; pt < ProfileButtons.Length; pt++){
                if(ProfileButtons[pt].name == "WhatTab"){
                    Text pbt = ProfileButtons[pt].GetComponent<Text>();
                    switch(tabName){
                        case "Profiles": pbt.text = GS.SetString("Profiles", "Profile"); break;
                        case "Stats": pbt.text = GS.SetString("Statistics", "Statystyki"); break;
                        case "Inventory": pbt.text = GS.SetString("Inventory", "Ekwipunek"); break;
                        case "Achievements": pbt.text = GS.SetString("Achievements", "Osiągnięcia"); break;
                        case "Settings": pbt.text = GS.SetString("Settings", "Ustawienia"); break;
                        case "no": pbt.text = "..."; break;
                    }
                } else if (ProfileButtons[pt].name == "Close" && ProfileButtons[pt].IsSelected && Input.GetMouseButtonDown(0)){
                    Tab = "";
                    CurrentWindow = "Main";
                } else {
                    if(ProfileButtons[pt].IsSelected) {
                        tabName = ProfileButtons[pt].name;
                        if(Input.GetMouseButtonDown(0)) {
                            displayPMslots(Tab);
                            if(SceneManager.GetActiveScene().name == "MainGame" && (ProfileButtons[pt].name == "Profiles" || ProfileButtons[pt].name == "Settings" || ProfileButtons[pt].name == "Inventory")) ProfileTab = "no";
                            else ProfileTab = ProfileButtons[pt].name;
                        }
                    }
                }
            }

            // Tabs
            int[] SliderValues = {0,0,0}; // Enabled, A value, B value
            foreach(Transform GetTab in ProfileTabs){
                if(GetTab.name == Tab){
                    GetTab.localScale = Vector3.one;
                    switch(Tab){
                        case "Stats":
                            PMslots = new string[]{};
                            SliderValues = new int[]{1,32,PMstats.Length};
                            GetTab.GetChild(0).GetComponent<Text>().text = "";
                            int Scroll = (int)Mathf.Lerp(0, Mathf.Clamp( ProfileSlider.MaxB-ProfileSlider.MaxA, 0, ProfileSlider.MaxB-ProfileSlider.MaxA ), ProfileSlider.Current);
                            for(int Printeh = 0; Printeh < 32; Printeh++){
                                int Offset = (int)Mathf.Lerp(0, Mathf.Clamp(SliderValues[2]-32, 0, 9999), ProfileSlider.Current);
                                if(Printeh < PMstats.Length) GetTab.GetChild(0).GetComponent<Text>().text += PMstats[Printeh+Offset] + "\n";
                                else break;
                            }
                            break;
                        case "Settings":
                            PMstats = new string[]{};
                            PMslots = new string[]{};
                            for(int B = 0; B < GetTab.childCount; B++){
                                Text bT = GetTab.GetChild(B).GetComponent<Text>();
                                ButtonScript bB = GetTab.GetChild(B).GetComponent<ButtonScript>();
                                switch(GetTab.GetChild(B).name){
                                    case "Name":
                                        bT.text = GS.SetString("Change profile name","Zmień nazwę profilu");
                                        if (bB.IsSelected && Input.GetMouseButtonDown(0)) InputTopic = "ChangeProfileName";
                                        break;
                                    case "Manage":
                                        bT.text = GS.SetString("Manage profiles","Zarządzaj profilami");
                                        if (bB.IsSelected && Input.GetMouseButtonDown(0)) ProfileTab = "Profiles";
                                        break;
                                    case "Erase":
                                        bT.text = GS.SetString("Erase profile","Usuń profil");
                                        if (bB.IsSelected && Input.GetMouseButtonDown(0)) Warning = new string[]{"EraseProfile", PS.ProfileID.ToString()};
                                        break;
                                }
                            }
                            break;
                        case "Profiles":
                            PMstats = new string[]{};
                            PMslots = new string[]{};
                            string[] ProfileStrings = GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©");
                            for(int P = 0; P < ProManButtons.Length; P++){
                                if(P == ProfileStrings.Length){
                                    GetTab.GetChild(P).GetChild(0).GetComponent<Text>().text = GS.SetString("< Create a new profile >", "< Stwórz nowy profil >");
                                    GetTab.GetChild(P).GetChild(1).localScale = Vector3.one;
                                    GetTab.GetChild(P).GetChild(2).localScale = Vector3.zero;
                                    if(ProManButtons[P].IsSelected && Input.GetMouseButtonDown(0)){
                                        InputTopic = "NewProfile";
                                    }
                                } else if(P < ProfileStrings.Length){
                                    GetTab.GetChild(P).GetChild(0).GetComponent<Text>().text = GS.GetSemiClass(ProfileStrings[P], "PN_", "®");
                                    GetTab.GetChild(P).GetChild(1).localScale = Vector3.zero;
                                    GetTab.GetChild(P).GetChild(2).localScale = Vector3.one;
                                    if(GS.GetSemiClass(ProfileStrings[P], "ID_", "®") == PS.ProfileID.ToString()) {
                                        GetTab.GetChild(P).GetChild(0).GetComponent<Text>().color = new Color32(125, 255, 125, 255);
                                        GetTab.GetChild(P).GetChild(2).GetComponent<Image>().color = new Color32(125, 255, 125, 255);
                                    } else {
                                        GetTab.GetChild(P).GetChild(0).GetComponent<Text>().color = Color.white;
                                        GetTab.GetChild(P).GetChild(2).GetComponent<Image>().color = Color.white;
                                    }
                                    if(ProManButtons[P].IsSelected && Input.GetMouseButtonDown(0)){
                                        PS.SaveProfile(-1);
                                        PS.SaveProfile(int.Parse(GS.GetSemiClass(ProfileStrings[P], "ID_", "®")));
                                    }
                                } else {
                                    GetTab.GetChild(P).GetChild(0).GetComponent<Text>().text = "";
                                    GetTab.GetChild(P).GetChild(1).localScale = GetTab.GetChild(P).GetChild(2).localScale = Vector3.zero;
                                }
                            }
                            break;
                        case "Inventory": case "Achievements":
                            PMstats = new string[]{};
                            displayPMslots(Tab);
                            SliderValues = new int[]{1,10,(int)(PMslots.Length/10)};
                            int ScrollA = (int)Mathf.Lerp(0, Mathf.Clamp( ProfileSlider.MaxB-ProfileSlider.MaxA, 0, ProfileSlider.MaxB-ProfileSlider.MaxA ), ProfileSlider.Current);

                            // Try to display
                            for(int set = 0; set < 100; set++){
                                Transform InvSlot = GetTab.GetChild(set);
                                int receiveIndex = set + (ScrollA * 10);

                                if(receiveIndex < PMslots.Length && PMslots[receiveIndex] != ""){
                                    InvSlot.localScale = Vector3.one;
                                    InvSlot.transform.GetChild(0).transform.localScale = Vector3.zero;
                                    InvSlot.transform.GetChild(1).transform.localScale = Vector3.zero;
                                    InvSlot.transform.GetChild(2).transform.localScale = Vector3.zero;
                                    InvSlot.transform.GetChild(3).GetComponent<Text>().text = "";
                                    InvSlot.transform.GetChild(4).GetComponent<Text>().text = "";
                                    switch(PMslots[receiveIndex]){
                                        case "MISCITEMS":
                                            InvSlot.transform.GetChild(1).transform.localScale = Vector3.one;
                                            InvSlot.transform.GetChild(3).GetComponent<Text>().text = GS.SetString("MISC", "RÓŻNE");
                                            break;
                                        case "INUSE":
                                            InvSlot.transform.GetChild(1).transform.localScale = Vector3.one;
                                            InvSlot.transform.GetChild(3).GetComponent<Text>().text = GS.SetString("ITEMS IN USE", "PRZEDMIOTY W UŻYCIU");
                                            break;
                                        case "CLOTHES":
                                            InvSlot.transform.GetChild(1).transform.localScale = Vector3.one;
                                            InvSlot.transform.GetChild(3).GetComponent<Text>().text = GS.SetString("CLOTHES", "UBRANIA");
                                            break;
                                        case "KITS":
                                            InvSlot.transform.GetChild(1).transform.localScale = Vector3.one;
                                            InvSlot.transform.GetChild(3).GetComponent<Text>().text = GS.SetString("KITS", "ZESTAWY");
                                            break;
                                        case "ACHIEVEMENTS":
                                            InvSlot.transform.GetChild(1).transform.localScale = Vector3.one;
                                            InvSlot.transform.GetChild(3).GetComponent<Text>().text = GS.SetString("ACHIEVEMENTS", "OSIĄGNIĘCIA");
                                            break;
                                        case "TROPHIES":
                                            InvSlot.transform.GetChild(1).transform.localScale = Vector3.one;
                                            InvSlot.transform.GetChild(3).GetComponent<Text>().text = GS.SetString("TROPHIES", "TROFEA");
                                            break;
                                        case "Empty":
                                            InvSlot.transform.GetChild(0).transform.localScale = Vector3.one;
                                            break;
                                        case "": break;
                                        default:
                                            InvSlot.transform.GetChild(0).transform.localScale = Vector3.one;
                                            InvSlot.transform.GetChild(2).transform.localScale = Vector3.one;
                                            NMPBfunc(PMslots[receiveIndex], GS.GetSemiClass(PMslots[receiveIndex], "tp_"), new Transform[]{InvSlot.transform.GetChild(2), InvSlot.transform.GetChild(4)});
                                            break;
                                    }
                                } else {
                                    InvSlot.localScale = Vector3.zero;
                                }
                            }
                            
                            break;
                        case "no":
                            PMstats = new string[]{};
                            PMslots = new string[]{};
                            GetTab.GetChild(0).GetComponent<Text>().text = GS.SetString("This tab is available only in the main menu.", "Ta zakładka jest dostępna tylko w menu głównym.");
                            break;
                    }
                } else {
                    GetTab.localScale = Vector3.zero;
                }
            }

            // Slider settings
            if(SliderValues[0] == 1){
                ProfileSlider.MaxA = SliderValues[1];
                ProfileSlider.MaxB = SliderValues[2];
                ProfileSlider.Active = true;
                ProfileSlider.gameObject.SetActive(true);
            } else {
                ProfileSlider.Current = 0f;
                ProfileSlider.Selected = 0;
                ProfileSlider.Active = false;
                ProfileSlider.gameObject.SetActive(false);
            }

            // Profile side viewer
            foreach(Transform cps in ProfileSide){
                switch(cps.name){
                    case "ProfileName": cps.GetComponent<Text>().text = PS.Profilename; break;
                    case "LevelBar": cps.GetChild(1).GetComponent<Image>().fillAmount = (float)PS.Exp[1] / (float)PS.Exp[2]; break;
                    case "LevelData": cps.GetComponent<Text>().text = GS.SetString("Level ", "Poziom ") + PS.Exp[0] + "\n" + PS.Exp[1] + " / " + PS.Exp[2]; break;
                    case "ClothButton": 
                        NMPBfunc("id_" + PS.cl_Clothes, "ClothButton", new Transform[]{cps.GetChild(0)}); 
                        if(cps.GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) PS.cl_Clothes = "ClassicClothes";
                        break;
                    case "HatButton": 
                        NMPBfunc("id_" + PS.cl_Hat, "HatButton", new Transform[]{cps.GetChild(0)}); 
                        if(cps.GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) PS.cl_Hat = "Null";
                        break;
                    case "MiscButton": 
                        NMPBfunc("id_" + PS.cl_Misc, "MiscButton", new Transform[]{cps.GetChild(0)}); 
                        if(cps.GetComponent<ButtonScript>().IsSelected && Input.GetMouseButtonDown(0)) PS.cl_Misc = "Null";
                        break;
                    case "HairColor": 
                        cps.GetChild(0).GetComponent<Image>().color = PS.HairColors[PS.cl_Hair];
                        if(cps.GetComponent<ButtonScript>().IsSelected) {
                            WhatTabs.text = GS.SetString("Hair color - ", "Kolor włosów - ") + PS.cl_Hair;
                            if(Input.GetMouseButtonDown(0)) PS.cl_Hair = (PS.cl_Hair+1)%10;
                        }
                        break;
                    case "SkinColor": 
                        cps.GetChild(0).GetComponent<Image>().color = PS.SkinColors[PS.cl_Skin];
                        if(cps.GetComponent<ButtonScript>().IsSelected) {
                            WhatTabs.text = GS.SetString("Skin color - ", "Kolor skóry - ") + PS.cl_Skin;
                            if(Input.GetMouseButtonDown(0)) PS.cl_Skin = (PS.cl_Skin+1)%10;
                        }
                        break;
                    case "BodyType":
                        cps.GetChild(0).localScale = new Vector3(Mathf.Lerp(1f, 0.5f, (PS.cl_Body-0.5f)*2f), Mathf.Lerp(0.5f, 1f, PS.cl_Body*2f), 1f);
                        if(cps.GetComponent<ButtonScript>().IsSelected) {
                            if(PS.cl_Body <= 0.3f) WhatTabs.text = GS.SetString("Body type - plump ", "Budowa ciała - okrągła ") + (int)((PS.cl_Body-0.5f)*10f);
                            else if(PS.cl_Body >= 0.6f) WhatTabs.text = GS.SetString("Body type - slender ", "Budowa ciała - szczupła ") + (int)((PS.cl_Body-0.5f)*10f);
                            else WhatTabs.text = GS.SetString("Body type - average ", "Budowa ciała - przeciętna ") + (int)((PS.cl_Body-0.5f)*10f);
                            if(Input.GetMouseButtonDown(0)) PS.cl_Body = (PS.cl_Body+0.1f)%1f;
                        }
                        break;
                }
            }

            // Profile player viewer
            if(!PVcamera.GetComponent<Camera>().enabled) {
                PVcamera.GetComponent<Camera>().enabled = true;
                PVtexture.Create();
            }
            PVcamera.localPosition = new Vector3(PVvector.x, PVvector.y, -PVvector.z);
            PVmodel.transform.eulerAngles = new Vector3(0f, PVvector.w, 0f);

            if(PVbox.IsSelected){
                if(Input.GetMouseButton(0)){
                    PVvector = new Vector4(Mathf.Clamp(PVvector.x + -Input.GetAxis("Mouse X")/10f, -1f, 1f), Mathf.Clamp(PVvector.y + -Input.GetAxis("Mouse Y")/10f, -1f, 1f), PVvector.z, PVvector.w);
                } else if(Input.GetMouseButton(2)){
                    PVvector = new Vector4(PVvector.x, PVvector.y, PVvector.z, PVvector.w + Input.GetAxis("Mouse X")*-50f);
                } else if (Input.mouseScrollDelta.y != 0f){
                    PVvector = new Vector4(PVvector.x, PVvector.y, Mathf.Clamp(PVvector.z + Input.mouseScrollDelta.y/-10f, 0.5f, 3f), PVvector.w);
                }
            }

        } else if (PrevWindow[1] == "ProfileMenu") {
            ProfileMenu.position = Vector3.Lerp(SH[0].position, SH[1].position, FromStart*3f);
            ProfileMenu.localScale = Vector3.one;

            PMstats = new string[]{};
        } else {
            if(PVcamera.GetComponent<Camera>().enabled) {
                PVcamera.GetComponent<Camera>().enabled = false;
                PVtexture.Release();
            }
            ProfileMenu.position = SH[1].position;
            ProfileMenu.localScale = Vector3.zero;
            ProfileSlider.Current = 0f;
            ProfileSlider.Selected = 0;
            ProfileSlider.Active = false;
            ProfileSlider.gameObject.SetActive(false);

            PMstats = new string[]{};
        }

    }

    void NMPBfunc(string ButtonData, string ButtonType, Transform[] ButtonParts){

        // Visuals
        string achName = GS.GetSemiClass(ButtonData, "id_");
        if(ButtonType == "Achievements"){
            ButtonParts[0].GetComponent<Outline>().effectColor = new Color(0f,0f,0f,0f);
            if(int.Parse(GS.GetSemiClass(ButtonData, "va_")) <= 0) achName = GS.GetSemiClass(ButtonData, "id_")+"-A";
            else achName = GS.GetSemiClass(ButtonData, "id_")+"-B";
        } else {
            ButtonParts[0].GetComponent<Outline>().effectColor = new Color(0f,0f,0f,1f);
        }

        bool hasIcon = false;
        for(int gi = 0; gi < NMPMicons.Length; gi++) if (NMPMicons[gi].name == achName || (achName == "Null" && NMPMicons[gi].name == "Transparent")){
            ButtonParts[0].GetComponent<Image>().sprite = NMPMicons[gi];
            hasIcon = true;
            break;
        }

        if(!hasIcon) Debug.LogError("There is no icon for " + achName);

        if(GS.ExistSemiClass(ButtonData, "am_"))
            ButtonParts[1].GetComponent<Text>().text = GS.GetSemiClass(ButtonData, "am_");

        // Functionals
        ButtonScript butt;
        if(ButtonParts[0].parent && ButtonParts[0].parent.GetComponent<ButtonScript>()) {
            butt = ButtonParts[0].parent.GetComponent<ButtonScript>();

            if(butt.IsSelected){
                switch(ButtonType){
                    case "ClothButton": WhatTabs.text = GS.SetString("Clothes - ", "Ubiór - ") + PS.GetProfileItemName(GS.GetSemiClass(ButtonData, "id_")); break;
                    case "HatButton":
                        if(GS.GetSemiClass(ButtonData, "id_") == "Null") WhatTabs.text = GS.SetString("No hat", "Brak czapki");
                        else WhatTabs.text = GS.SetString("Hat - ", "Czapka - ") + PS.GetProfileItemName(GS.GetSemiClass(ButtonData, "id_"));
                        break;
                    case "MiscButton":
                        if(GS.GetSemiClass(ButtonData, "id_") == "Null") WhatTabs.text = GS.SetString("No cosmetics", "Brak ozdoby");
                        else WhatTabs.text = GS.SetString("Cosmetic - ", "Ozdoba - ") + PS.GetProfileItemName(GS.GetSemiClass(ButtonData, "id_"));
                        break;
                    case "Clothes": case "Kits":
                        WhatTabs.text = PS.GetProfileItemName(GS.GetSemiClass(ButtonData, "id_"));
                        if(Input.GetMouseButtonDown(0) && PS.FindParentArray(ButtonData) == "InUse"){
                            Warning[0] = "NMPM_ItemInUse";
                            Warning[1] = ButtonData;
                        } else if (Input.GetMouseButtonDown(0)){
                            Warning[0] = "NMPM_ItemPreview";
                            Warning[1] = ButtonData;
                        }
                        break;
                    case "Achievements":
                        if(int.Parse(GS.GetSemiClass(ButtonData, "va_")) <= 0) WhatTabs.text = PS.GetProfileItemName(GS.GetSemiClass(ButtonData, "id_"));
                        else WhatTabs.text = "???";

                        if(Input.GetMouseButtonDown(0)){
                            Warning[0] = "NMPM_Achievement";
                            Warning[1] = ButtonData;
                            print(Warning[0]);
                        }
                        break;
                    default: break;
                }
            }
        }

    }

    void displayPMslots(string tabName){
        if(tabName == "Inventory"){
            List<string> AddSlots = new List<string>();
            // Add in use
            if(PS.InUse.Length > 0){
                AddSlots.Add("INUSE"); for(int fill = 9; fill > 0; fill--) AddSlots.Add("");
                for(int ac = 0; ac < PS.InUse.Length; ac++){AddSlots.Add(PS.InUse[ac]);}
                for(int fillback = 10 - AddSlots.ToArray().Length%10; fillback > 0; fillback--) AddSlots.Add("Empty");
            }
            // Add clothing
            if(PS.Clothes.Length > 0){
                AddSlots.Add("CLOTHES"); for(int fill = 9; fill > 0; fill--) AddSlots.Add("");
                for(int ac = 0; ac < PS.Clothes.Length; ac++){AddSlots.Add(PS.Clothes[ac]);}
                for(int fillback = 10 - AddSlots.ToArray().Length%10; fillback > 0; fillback--) AddSlots.Add("Empty");
            }
            // Add kits
            if(PS.Kits.Length > 0){
                AddSlots.Add("KITS"); for(int fill = 9; fill > 0; fill--) AddSlots.Add("");
                for(int ac = 0; ac < PS.Kits.Length; ac++){AddSlots.Add(PS.Kits[ac]);}
                for(int fillback = 10 - AddSlots.ToArray().Length%10; fillback > 0; fillback--) AddSlots.Add("Empty");
            }
            // Add misc
            if(PS.MiscItems.Length > 0){
                AddSlots.Add("MISCITEMS"); for(int fill = 9; fill > 0; fill--) AddSlots.Add("");
                for(int ac = 0; ac < PS.MiscItems.Length; ac++){AddSlots.Add(PS.InUse[ac]);}
                for(int fillback = 10 - AddSlots.ToArray().Length%10; fillback > 0; fillback--) AddSlots.Add("Empty");
            }
            PMslots = AddSlots.ToArray();
        } else if (tabName == "Achievements"){
            List<string> AddAch = new List<string>();
            // Add achievements
            if(PS.Achievements.Length > 0){
                AddAch.Add("ACHIEVEMENTS"); for(int fill = 9; fill > 0; fill--) AddAch.Add("");
                for(int ac = 0; ac < PS.Achievements.Length; ac++){AddAch.Add(PS.Achievements[ac]);}
                for(int fillback = 10 - AddAch.ToArray().Length%10; fillback > 0; fillback--) AddAch.Add("Empty");
            }
            // Add trophies
            if(PS.Trophies.Length > 0){
                AddAch.Add("TROPHIES"); for(int fill = 9; fill > 0; fill--) AddAch.Add("");
                for(int ac = 0; ac < PS.Trophies.Length; ac++){AddAch.Add(PS.Trophies[ac]);}
                for(int fillback = 10 - AddAch.ToArray().Length%10; fillback > 0; fillback--) AddAch.Add("Empty");
            }
            PMslots = AddAch.ToArray();
        }
    }

    void WhilePopups (string CurrentPopup){
        if(CurrentPopup != ""){
            PopupWindow.localScale = Vector3.one;

            bool swatched = false;
            if(PrevPopup != CurrentPopup){
                PrevPopup = CurrentPopup;
                swatched = true;
            }

            switch(GS.GetSemiClass(CurrentPopup, "type_")){
                case "Achievement":
                    if(swatched){
                        PopupTime = 5f;
                        string AchievementName = GS.GetSemiClass(CurrentPopup, "achname_");
                        PWachievement.GetChild(1).GetComponent<Text>().text = GS.SetString("Achievement got!\n", "Zdobyto osiągnięcie!\n") + PS.GetProfileItemName(AchievementName);
                        PWachievement.GetChild(0).localScale = Vector3.one;
                        PWachievement.GetChild(2).localScale = Vector3.zero;
                        for(int gi = 0; gi < NMPMicons.Length; gi++) if (NMPMicons[gi].name == AchievementName + "-A") {
                            PWachievement.GetChild(0).GetComponent<Image>().sprite = NMPMicons[gi];
                            break;
                        }
                    } else {
                        float PushVal = Mathf.Clamp(Mathf.Abs(PopupTime - 2.5f) - 1.5f, 0f, 1f);
                        PWachievement.anchoredPosition = new Vector2(Mathf.Lerp(-160f, 160f, PushVal), -32f);
                    }
                    break;
                case "LevelUp":
                    if(swatched){
                        PopupTime = 5f;
                        float leveledup = float.Parse(GS.GetSemiClass(CurrentPopup, "level_"));
                        PWachievement.GetChild(1).GetComponent<Text>().text = GS.SetString("PROMOTED!\nProfile level ", "AWANSUJESZ\nPoziom profilu ") + ((int)leveledup).ToString();
                        PWachievement.GetChild(0).localScale = Vector3.zero;
                        PWachievement.GetChild(2).localScale = Vector3.one;
                        
                        PWachievement.GetChild(2).GetChild(0).GetComponent<Text>().text = ((int)leveledup).ToString();
                        PWachievement.GetChild(2).GetChild(0).GetComponent<Text>().color = Color.HSVToRGB(((leveledup+2f)/10f)%1f, 0.5f, 1f);
                        PWachievement.GetChild(2).GetComponent<Image>().color = Color.HSVToRGB((leveledup/10f)%1f, 1f, 0.5f);
                    } else {
                        float PushVal = Mathf.Clamp(PopupTime, 0f, 1f);
                        PWachievement.anchoredPosition = new Vector2(Mathf.Lerp(160f, -160f, PushVal), -32f);
                        PWachievement.GetChild(2).GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, PopupTime-4f);
                    }
                    break;
            }

            if(PopupTime > 0f) PopupTime -= 0.02f*(Time.unscaledDeltaTime*50f);
            else {
                PopupQueue.RemoveAt(0);
            }

        } else {
            PopupWindow.localScale = Vector3.zero;

        }
    }

    void WhileCredits (bool Showit = false) {
        if(Showit){
            CreditsWindow.position = SH[0].position;

            if(CreditsRoll.GetChild(1).GetComponent<Text>().text == ""){
                CreditAnyKey.text = GS.SetString("Press any key to return to menu", "Naciśnij dowolny przycisk by powrócić do menu");
                CreditsRoll.GetChild(1).GetComponent<Text>().text = GS.SetString(
                    "CREDITS\n\n\n\n\nCREATED BY\nGMPguy\n\n\nTOOLS USED\nUnity 2022\nAudacity\nGIMP\nBlender\n\n\n\nInitially created for Game Off 2019 game jam.\n\nIt was then later worked on and improved over the span of 3 years, into the version that you are now playing\n\n\n\nI know, it's kinda hard to notice.\n\nDon't do the same thing, and don't work that long on a niche passion project (of which your passion ran out a long time ago).\n\nThe final effect, as satisfying as it is, was not worth it.",
                    "LISTA PŁAC\n\n\n\n\nSTWORZONE PRZEZ\nGMPguy\n\n\nUŻYTE NARZĘDZIA\nUnity 2022\nAudacity\nGIMP\nBlender\n\n\n\nGra początkowo stworzona na potrzeby Game Off 2019.\n\nPóźniej była ulepszana, w przeciągu 3 lat, do wersji w którą teraz grasz.\n\n\n\nWiem, trudno to zauważyć\n\nNie róbcie tego co ja, i nie pracujcie tyle czasu nad niszowym projektem pasji jak ta gra (zresztą pasję do tego straciłem lata temu).\n\nEfekt końcowy, o ile satysfakcjonujący, nie był tego wart."
                );
            }

            if(FromStart < 1f) CreditsRoll.GetChild(0).localScale = Vector3.zero; else CreditsRoll.GetChild(0).localScale = Vector3.one;
            CreditBars[0].color = CreditBars[1].color = CreditBars[2].color = Color.Lerp(CreditBars[0].color, Color.black, FromStart);
            CreditsRoll.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, Time.unscaledDeltaTime*20f);
            if (CreditsRoll.GetComponent<RectTransform>().anchoredPosition.y < -800 - CreditsRoll.GetComponent<RectTransform>().sizeDelta.y) CreditsRoll.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            if(Input.anyKeyDown) CurrentWindow = "Main";
        } else {
            CreditsWindow.position = SH[1].position;
            CreditBars[0].color = CreditBars[1].color = CreditBars[2].color = new Color(0f,0f,0f,0f);
            CreditsRoll.GetChild(1).GetComponent<Text>().text = "";
            CreditsRoll.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void WhileManual (bool Showit = false){
        if(Showit){
            MS = this.GetComponent<ManualScript>();
            ManualWindow.position = SH[0].position;
            ManualSlider.Active = true;
            ManualSlider.gameObject.SetActive(true);

            if (FromStart < 1f) {
                ManualIntro.GetComponent<Image>().color = Color.white;
                ManualIntro.GetChild(0).GetComponent<Image>().color = Color.Lerp(new Color(0.5f,0.5f,0.75f, 1f), Color.white, Mathf.Abs(ManualIntro.GetChild(0).localScale.x));
                ManualIntro.GetChild(1).GetComponent<Image>().color = Color.Lerp(new Color(0.75f,0.75f,0.75f, 1f), Color.white, Mathf.Abs(ManualIntro.GetChild(1).localScale.x));

                MS.Display();
                ManualIntro.localScale = Vector3.one;
                ManualMain.localScale = Vector3.zero;
                if (FromStart < 0.5f) {
                    ManualIntro.position = Vector3.Slerp(SH[1].position, SH[0].position, FromStart*2f);
                    ManualIntro.GetChild(0).localScale = Vector3.one;
                    ManualIntro.GetChild(1).localScale = Vector3.zero;
                } else {
                    ManualIntro.position = SH[0].position;
                    ManualIntro.GetChild(0).localScale = Vector3.Lerp(Vector3.one, new Vector3(0f, 1f, 1f), (FromStart-0.5f)*4f);
                    ManualIntro.GetChild(1).localScale = Vector3.Lerp(new Vector3(0f, -1f, -1f), -Vector3.one, (FromStart-0.75f)*4f);
                }
            } else {
                ManualIntro.localScale = Vector3.one;
                ManualIntro.GetComponent<Image>().color = ManualIntro.GetChild(0).GetComponent<Image>().color = ManualIntro.GetChild(1).GetComponent<Image>().color = new Color(1f,1f,1f, (1.3f-FromStart)*3f);
                ManualMain.localScale = Vector3.one;

                ManualSlider.MaxA = 25;
                ManualSlider.MaxB = MS.Displayed.ToArray().Length;
                int Scroll = (int)Mathf.Lerp(0, Mathf.Clamp( ManualSlider.MaxB-ManualSlider.MaxA, 0, ManualSlider.MaxB-ManualSlider.MaxA ), ManualSlider.Current);

                for(int dm = 0; dm < 25; dm++){
                    if(dm+Scroll < MS.Displayed.ToArray().Length && MS.Displayed[dm+Scroll].ID != ""){
                        ManualOptionButtons[dm].Active = true;
                        ManualOptionButtons[dm].GetComponent<Text>().text = "";
                        for(int pb = MS.Displayed[dm+Scroll].ParentID; pb > 0; pb--) ManualOptionButtons[dm].GetComponent<Text>().text += "- ";
                        ManualOptionButtons[dm].GetComponent<Text>().text += MS.RetriveMOdata(MS.Displayed[dm+Scroll].ID, 0);

                        if(ManualOptionButtons[dm].IsSelected && Input.GetMouseButtonDown(0)) MS.ShowData(MS.Displayed[dm+Scroll].ID);
                    } else {
                        ManualOptionButtons[dm].Active = false;
                        ManualOptionButtons[dm].GetComponent<Text>().text = "";
                    }
                }
            }

        } else if (!Showit && PrevWindow[1] == "Manual") {
            ManualWindow.position = SH[0].position;

            ManualIntro.localScale = Vector3.one;
            ManualMain.localScale = Vector3.zero;

            ManualIntro.position = Vector3.Lerp(SH[0].position, SH[1].position, FromStart*2f);
            ManualIntro.GetChild(0).localScale = Vector3.Lerp( new Vector3(0f, 1f, 1f), Vector3.one, (FromStart-0.25f)*4f);
            ManualIntro.GetChild(1).localScale = Vector3.Lerp(-Vector3.one, new Vector3(-0f, -1f, -1f), FromStart*4f);

            ManualIntro.GetComponent<Image>().color = Color.white;
            ManualIntro.GetChild(0).GetComponent<Image>().color = Color.Lerp(new Color(0.5f,0.5f,0.75f, 1f), Color.white, Mathf.Abs(ManualIntro.GetChild(0).localScale.x));
            ManualIntro.GetChild(1).GetComponent<Image>().color = Color.Lerp(new Color(0.75f,0.75f,0.75f, 1f), Color.white, Mathf.Abs(ManualIntro.GetChild(1).localScale.x));

            ManualSlider.Active = false;
            ManualSlider.gameObject.SetActive(false);
        } else {
            ManualWindow.position = SH[1].position;

            ManualIntro.localScale = ManualMain.localScale = Vector3.zero;
            ManualSlider.Current = 0f;
            ManualSlider.Selected = 0;
            ManualSlider.Active = false;
            ManualSlider.gameObject.SetActive(false);
        }
    }

    string FNcheck(string tc = "", int WhichArray = 0){

        if(tc != ""){
            string[] CheckNames = {}; 
            
            List<string> ParseNames = new List<string>();
            if(PlayerPrefs.HasKey("Saves") && WhichArray == 0)
                foreach(string GitSave in GS.ListSemiClass(PlayerPrefs.GetString("Saves"), "©")) ParseNames.Add(GS.GetSemiClass(GitSave, "sn", "®"));
            else if(PlayerPrefs.HasKey("ProfilesSaves") && WhichArray == 1)
                foreach(string GitSave in GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©")) ParseNames.Add(GS.GetSemiClass(GitSave, "PN_", "®"));
            CheckNames = ParseNames.ToArray();

            for(int cn = 0; cn <= CheckNames.Length; cn++){
                if (cn == CheckNames.Length) {
                    return tc;
                } else if(CheckNames[cn] == tc){
                    string orgTC = tc;
                    for(int GetNumb = 2; GetNumb < 1000; GetNumb++){
                        string Duplicated = orgTC + "-" + GetNumb.ToString();
                        bool isFine = true;

                        for(int cnB = 0; cnB < CheckNames.Length; cnB++)
                            if(CheckNames[cnB] == Duplicated)
                                isFine = false;

                        if(isFine) {
                            return Duplicated;
                        }
                    }
                } else {
                    return tc;
                }
            }
            return "";
        } else {
            if(WhichArray == 0) return "Player no." + Random.Range(1000, 9999).ToString();
            else return "Generic profile no." + Random.Range(1000, 9999).ToString();
        }

    }

    void SortRecords(){

        string[] RawRecords = GS.ListSemiClass(PS.Records, "/");
        List<string> FilteredRecords = new List<string>();
        foreach(string RawRecord in RawRecords){
            bool GoodToGo = true;
            if((RecordFilters[0] >= 0 && GS.GetSemiClass(RawRecord, "G") != RecordFilters[0].ToString()) || (RecordFilters[1] >= 0 && GS.GetSemiClass(RawRecord, "D") != RecordFilters[1].ToString()))
                GoodToGo = false;
            if (GoodToGo)
                FilteredRecords.Add(RawRecord);
        }

        string[] bubbles = FilteredRecords.ToArray();
        bool RepeatBubble = false;

        if(bubbles.Length > 1)
        for(int bubcek = 1; bubcek <= bubbles.Length; bubcek++){
            if(bubcek < bubbles.Length){
                string[] samples = {bubbles[bubcek-1], bubbles[bubcek], "", ""};
                bool Rev = false;
                switch(RecordSort){
                    case "SH":
                        samples[2] = GS.GetSemiClass(samples[0], "S");
                        samples[3] = GS.GetSemiClass(samples[1], "S");
                        break;
                    case "SL":
                        samples[2] = GS.GetSemiClass(samples[0], "S");
                        samples[3] = GS.GetSemiClass(samples[1], "S");
                        Rev = true;
                        break;
                    case "AZ":
                        samples[2] = GS.GetSemiClass(samples[0], "N");
                        samples[3] = GS.GetSemiClass(samples[1], "N");
                        break;
                    case "ZA":
                        samples[2] = GS.GetSemiClass(samples[0], "N");
                        samples[3] = GS.GetSemiClass(samples[1], "N");
                        Rev = true;
                        break;
                    case "OL":
                        samples[2] = GS.GetSemiClass(samples[0], "T");
                        samples[3] = GS.GetSemiClass(samples[1], "T");
                        break;
                    case "NE":
                        samples[2] = GS.GetSemiClass(samples[0], "T");
                        samples[3] = GS.GetSemiClass(samples[1], "T");
                        Rev = true;
                        break;
                }

                if (RecordSort == "AZ" || RecordSort == "ZA") {
                    char[] charA = samples[2].ToCharArray();
                    char[] charB = samples[3].ToCharArray();
                    bool kSwitch = false;
                    for(int sc = 0; sc < charA.Length; sc++){
                        if(sc >= charB.Length){
                            break;
                        } else if( (!Rev && charA[sc] < charB[sc]) || (Rev && charA[sc] > charB[sc]) ){
                            kSwitch = true;
                            break;
                        }
                    }
                    if(kSwitch){
                        string moveAss = bubbles[bubcek];
                        bubbles[bubcek] = bubbles[bubcek-1];
                        bubbles[bubcek-1] = moveAss;
                        RepeatBubble = true;
                    }
                } else if(  (!Rev && int.Parse(samples[2]) < int.Parse(samples[3])) || (Rev && int.Parse(samples[2]) > int.Parse(samples[3])) ){
                    string moveAss = bubbles[bubcek];
                    bubbles[bubcek] = bubbles[bubcek-1];
                    bubbles[bubcek-1] = moveAss;
                    RepeatBubble = true;
                }
            } else if (RepeatBubble) {
                RepeatBubble = false;
                bubcek = 0;
            }
        }

        RecordList = bubbles;

    }

    void WhileInput(){

        if(InputTopic != ""){

            intActive = 1f;
            Inputwindow.position = SH[0].position;

            string Focused = "";
            // Input Functioning
            switch(InputTopic){
                case "KeyBind":
                    InputInfo.text = GS.SetString("CHANGE KEYBIND\nPress any key to bind that action to it\npress ESCAPE to cancel, or BACKSPACE to clear", "PRZYPISYWANIE KLAWISZA\nNaciśnij dowolny przycisk, ażeby go przypisać do danej akcji\nnaciśnij ESCAPE żeby to anulować, lub BACKSPACE żeby wycziścić przypisanie");
                    if(Input.GetKeyDown(KeyCode.Escape)){
                        InputTopic = InputSubject = "";
                    } else if(Input.GetKeyDown(KeyCode.Backspace)){
                        GS.Controlls[int.Parse(InputSubject)].Key = KeyCode.None;
                        InputTopic = InputSubject = "";
                    } else {
                        foreach(KeyCode GitCode in System.Enum.GetValues(typeof(KeyCode))) if(Input.GetKeyDown(GitCode)){
                            GS.Controlls[int.Parse(InputSubject)].Key = GitCode;
                            InputTopic = InputSubject = "";
                        }
                    }
                    Focused = GS.SetString("> AWAITING INPUT <", "> NACIŚNIJ COŚ <");
                    break;
                case "Filename":
                    InputInfo.text = GS.SetString("CHANGE FILENAME\nUse only letters, numbers, spaces, and maybe a few of special signs.\npress ENTER to accept, or ESCAPE to cancel", "ZMIANA NAZWY PLIKU\nMożesz korzystać tylko z liter, cyfr, spacji, i może tam kilku znaków specjalnych\nnaciśnij ENTER żeby to zaakceptować, lub ESCAPE żeby to anulować");
                    InputSubject = InputInput.text;

                    if(Input.GetKeyDown(KeyCode.Return)){
                        InputTopic = "";
                        InputSubject = FNcheck(InputSubject, 0);
                        GMstringers[0] = InputSubject;
                        InputSubject = "";
                    } else if(Input.GetKeyDown(KeyCode.Escape)){
                        InputTopic = InputSubject = "";
                    }
                    break;
                case "ChangeProfileName":
                    InputInfo.text = GS.SetString("CHANGE PROFILE NAME\nUse only letters, numbers, spaces, and maybe a few of special signs.\npress ENTER to accept, or ESCAPE to cancel", "ZMIANA NAZWY PROFILU\nMożesz korzystać tylko z liter, cyfr, spacji, i może tam kilku znaków specjalnych\nnaciśnij ENTER żeby to zaakceptować, lub ESCAPE żeby to anulować");
                    InputSubject = InputInput.text;

                    if(Input.GetKeyDown(KeyCode.Return)){
                        InputTopic = "";
                        InputSubject = FNcheck(InputSubject, 1);
                        PS.Profilename = InputSubject;
                        InputSubject = "";
                    } else if(Input.GetKeyDown(KeyCode.Escape)){
                        InputTopic = InputSubject = "";
                    }
                    break;
                case "NewProfile":
                    InputInfo.text = GS.SetString("CREATING NEW PROFILE\nType in the name of the new profile, in order to create it.\npress ENTER to create the profile, or ESCAPE to cancel", "TWORZENIE NOWEGO PROFILU\nWpisz nazwę nowego profilu, ażeby go utworzyć\nnaciśnij ENTER żeby to zaakceptować, lub ESCAPE żeby to anulować");
                    InputSubject = InputInput.text;

                    if(Input.GetKeyDown(KeyCode.Return)){
                        InputTopic = "";
                        InputSubject = FNcheck(InputSubject, 1);

                        // creating a new profile
                        PS.SaveProfile(-1);
                        PS.Profilename = InputSubject;
                        PS.SaveProfile(Random.Range(100000, 999999));
                        InputSubject = "";
                        PMstats = new string[]{};
                    } else if(Input.GetKeyDown(KeyCode.Escape)){
                        InputTopic = InputSubject = "";
                    }
                    break;
            }

            if (Focused != ""){
                InputInput.enabled = false;
                InputInput.text = Focused;
            } else {
                InputInput.enabled = true;
            }

        } else {

            Inputwindow.position = SH[1].position;
            InputInput.text = "";

        }

    }

}
