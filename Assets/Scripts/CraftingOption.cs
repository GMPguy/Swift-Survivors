using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOption : MonoBehaviour {

    // Main Variables
    public GameScript GS;
    public CanvasScript MainCanvas;
    public PlayerScript MainPlayer;

    public bool IsButton = false;
    public string[] WhatToCraft;
    public JClass[] Results;
    public string[] Resources; // ItemID - Bonus Value - What to do (0 to remove)
    public JClass[] TheResources;
    public string Special;
    public float[] CraftingTime = new float[] { 1f, 0f }; // Actuall, current
    // For Button
    public GameObject COIcon;
    public GameObject[] COResIcons;
    public GameObject CraftButton;
    public GameObject ItemPrefab;

    // Start is called before the first frame update
    void Start() {

        if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        if(GameObject.Find("MainCanvas")) MainCanvas = GameObject.Find("MainCanvas").GetComponent<CanvasScript>();
        
    }

    // Update is called once per frame
    void Update() {

        if (GS == null || MainCanvas == null || MainPlayer == null) {
            if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(GameObject.Find("MainCanvas")) MainCanvas = GameObject.Find("MainCanvas").GetComponent<CanvasScript>();
            MainPlayer = GameObject.Find("_RoundScript").GetComponent<RoundScript>().MainPlayer;
        } else {
            if (IsButton == true && MainPlayer != null && MainPlayer.State == 1 && MainCanvas.ITShown == "Craft") {
                // Receive resources
                string CanCraft = "";
                List<JClass> GotItems = new List<JClass>();

                foreach (JClass V in MainPlayer.Inventory) {
                    GotItems.Add(V);
                }

                List<Vector3Int> AcquiredItems = new List<Vector3Int>();
                for (int CheckRes = 0; CheckRes < TheResources.Length; CheckRes ++) {
                    int requiredAmount = 1;
                    
                    //if(GS.ExistSemiClass(TheResources[CheckRes], "sq")) requiredAmount = int.Parse(GS.GetSemiClass(TheResources[CheckRes], "sq") );
                    if (TheResources[CheckRes].Exists(JType.StackQuantity))
                        requiredAmount = TheResources[CheckRes].GetInt(JType.StackQuantity);

                    for (int CheckItem = 0; CheckItem < MainPlayer.MaxInventorySlots; CheckItem ++) {
                        if (TheResources[CheckRes].GetString(JType.CraftingFunction) == "remove" && GotItems[CheckItem].GetInt(JType.ID) == TheResources[CheckRes].GetInt(JType.ID)) {
                            if(!GotItems[CheckItem].Exists(JType.StackQuantity)){// GS.ExistSemiClass(GotItems[CheckItem], "sq")) {
                                AcquiredItems.Add(new Vector3Int(CheckItem, 1, 0));
                                requiredAmount--;
                            } else if (GotItems[CheckItem].GetInt(JType.StackQuantity) < requiredAmount){//int.Parse( GS.GetSemiClass(GotItems[CheckItem], "sq") ) < requiredAmount ) {
                                AcquiredItems.Add(new Vector3Int(
                                    CheckItem,
                                    GotItems[CheckItem].GetInt(JType.StackQuantity),
                                    0
                                ));
                                requiredAmount -= GotItems[CheckItem].GetInt(JType.StackQuantity);
                            } else if (GotItems[CheckItem].GetInt(JType.StackQuantity) >= requiredAmount ) {
                                AcquiredItems.Add(new Vector3Int(CheckItem, requiredAmount, 0));
                                requiredAmount = 0;
                            }
                            GotItems[CheckItem] = new (0, JTemplate.JustID);//new Vector3(-1f, 0f, 0f);
                            if(requiredAmount <= 0) break;
                        }
                    }
                    
                    if (requiredAmount > 0) {
                        CanCraft = "NoResources";
                        //break;
                    }
                }

                switch (Special) {
                    case "Fire":
                        if (MainPlayer.Campfire <= 0f)
                            CanCraft = "NoFire";
                        break;
                }

                // Craft button
                CraftButton.transform.GetChild(1).GetComponent<Image>().fillAmount = CraftingTime[1] / CraftingTime[0];
                if (CanCraft == "") {
                    GS.SetText(CraftButton.transform.GetChild(0).GetComponent<Text>(), "Craft", "Twórz");
                    CraftButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(0, 255, 0, 255);
                    CraftButton.GetComponent<Image>().color = new Color32(100, 155, 100, 255);
                    CraftButton.transform.GetChild(1).GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                } else {
                    if (CanCraft == "NoResources") {
                        GS.SetText(CraftButton.transform.GetChild(0).GetComponent<Text>(), "Items?", "Surowce?");
                        CraftButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 0, 0, 255);
                    } else if (CanCraft == "NoFire") {
                        GS.SetText(CraftButton.transform.GetChild(0).GetComponent<Text>(), "Fire?", "Ogień?");
                        CraftButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 0, 0, 255);
                    }
                    CraftButton.GetComponent<Image>().color = new Color32(75, 55, 55, 255);
                }

                if(CraftButton.GetComponent<ButtonScript>().IsSelected == true){
                    MainCanvas.CDTdisplaye[1] = this.transform.GetSiblingIndex();

                    string ToDisplay = ""; 

                    for(int cr = 0; cr < Results.Length; cr++){
                        if(cr > 0) 
                            ToDisplay += " + ";

                        ToDisplay += GS.ItemCache[Results[0].GetInt(JType.ID)].getName().ToUpper();

                        if(Results[0].Exists(JType.StackQuantity) && Results[0].GetInt(JType.StackQuantity) > 1) 
                            ToDisplay += " x" + Results[0].GetInt(JType.StackQuantity);
                    }

                    switch(Special){
                        case "Fire":
                            ToDisplay += "\n" + GS.SetString("Needs fire", "Potrzebuje ognia");
                            break;
                        default:
                            ToDisplay += "\n" + GS.SetString("Handmade", "Rękodzieło");
                            break;
                    }

                    ToDisplay += "\n___________________";

                    for(int sr = 0; sr < TheResources.Length; sr++){
                        if(sr == 0) 
                            ToDisplay += "\nResources:";

                        ToDisplay += "\n- " + GS.ItemCache[TheResources[sr].GetInt(JType.ID)].getName();

                        if(TheResources[sr].Exists(JType.StackQuantity) && TheResources[sr].GetInt(JType.StackQuantity) > 1) 
                            ToDisplay += " x" + TheResources[sr].GetInt(JType.StackQuantity);
                    }

                    MainCanvas.CDTstring = ToDisplay;
                }

                if (CraftButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButton(0) && MainPlayer.CantCraft <= 0f) {
                    if (CanCraft == "") {
                        MainCanvas.PlayCraftingSound = 0.25f;
                        MainPlayer.ItemsShown.GetComponent<Animator>().Play(MainPlayer.PlayItemAnim("Pullup", MainPlayer.Inventory[MainPlayer.CurrentItemHeld].GetInt(JType.ID), ""), 0, 0f);
                        MainPlayer.ShakeCam((CraftingTime[1] / CraftingTime[0]) / 3f, 0.1f);
                        MainPlayer.CantUseItem = Mathf.Clamp(MainPlayer.CantUseItem, 1f, Mathf.Infinity);
                        CraftingTime[1] = Mathf.Clamp(CraftingTime[1] + (0.02f * (Time.deltaTime * 50f)), 0f, CraftingTime[0]);
                        if (CraftingTime[1] >= CraftingTime[0]) {
                            GS.Mess(GS.SetString(GS.ItemCache[Results[0].GetInt(JType.ID)].getName() + " crafted!", "Stworzono " + GS.ItemCache[Results[0].GetInt(JType.ID)].getName() + "!"), "Craft");
                            MainPlayer.CantCraft = Mathf.Clamp(MainPlayer.CantCraft, 0.5f, Mathf.Infinity);
                            // Retrive resources
                            foreach (Vector3Int GetResource in AcquiredItems) {
                                if (GetResource.z == 0f) {
                                    MainPlayer.InvGet(GetResource.x, 1, GetResource.y); //MainPlayer.Inventory[(int)GetResource.x] = "id0;";//Vector3.zero;
                                }
                            }
                            // Craft item
                            foreach (JClass SpawnItem in Results) {
                                MainPlayer.InvGet(SpawnItem, 0);
                            }

                            MainCanvas.SetCraftOptions();
                        }
                    } else {
                        CraftingTime[1] = 0f;
                        string craftingError = CanCraft switch {
                            "NoFire" => GS.SetString("You need a campfire!", "Brakuje ci ogniska!"),
                            _ => GS.SetString("You lack the resources!", "Brakuje ci surowców!")
                        };
                        GS.Mess(craftingError, "Error");
                        MainPlayer.CantCraft = Mathf.Clamp(MainPlayer.CantCraft, 0.5f, Mathf.Infinity);
                    }
                } else {
                    CraftingTime[1] = 0f;
                }
            }
        }

    }

    public void SetOption(GameObject WhichTemplate) {

        if (WhichTemplate != null) {

            this.transform.localScale = Vector3.one;

            Results = WhichTemplate.GetComponent<CraftingOption>().Results;
            TheResources = WhichTemplate.GetComponent<CraftingOption>().TheResources;
            Special = WhichTemplate.GetComponent<CraftingOption>().Special;
            CraftingTime = new float[] {WhichTemplate.GetComponent<CraftingOption>().CraftingTime[0], 0f};

            this.transform.GetChild(1).GetComponent<Text>().text = GS.ItemCache[Results[0].GetInt(JType.ID)].getName();//GS.ReceiveItemName(WhatToCraft[0].x);
            foreach (Sprite SetIcon in MainCanvas.ItemIcons) {
                if (SetIcon.name.Substring(1) == Results[0].GetInt(JType.ID).ToString()) {
                    COIcon.transform.GetChild(0).GetComponent<Image>().sprite = SetIcon;
                    COIcon.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }
                for (int CheckResIcon = 0; CheckResIcon < 4; CheckResIcon ++) {
                    if (CheckResIcon >= TheResources.Length) {
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().sprite = null;
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        COResIcons[CheckResIcon].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        COResIcons[CheckResIcon].transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                        COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().text = "";
                    } else if (SetIcon.name.Substring(1) == TheResources[CheckResIcon].GetInt(JType.ID).ToString()) {
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().sprite = SetIcon;
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        COResIcons[CheckResIcon].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                        COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().fontSize = 24;

                        if(TheResources[CheckResIcon].Exists(JType.StackQuantity) && TheResources[CheckResIcon].GetInt(JType.StackQuantity) > 1) 
                            COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().text = TheResources[CheckResIcon].GetInt(JType.StackQuantity).ToString();
                        else 
                            COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().text = "";

                    }
                }
            }

        } else {
            this.transform.localScale = Vector3.zero;
        }

    }
}
