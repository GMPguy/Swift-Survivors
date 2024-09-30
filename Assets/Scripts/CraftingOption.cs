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
    public string[] Resources; // ItemID - Bonus Value - What to do (0 to remove)
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
                List<string> GotItems = new List<string>();
                foreach (string V in MainPlayer.Inventory) {
                    GotItems.Add(V);
                }
                List<Vector3> AcquiredItems = new List<Vector3>();
                for (int CheckRes = 0; CheckRes < Resources.Length; CheckRes ++) {
                    int requiredAmount = 1;
                    
                    if(GS.ExistSemiClass(Resources[CheckRes], "sq")) requiredAmount = int.Parse(GS.GetSemiClass(Resources[CheckRes], "sq") );
                    
                    for (int CheckItem = 0; CheckItem < MainPlayer.MaxInventorySlots; CheckItem ++) {
                        if (GS.GetSemiClass(Resources[CheckRes], "do") == "0" && GS.GetSemiClass(GotItems[CheckItem], "id") == GS.GetSemiClass(Resources[CheckRes], "id")) {
                            if(!GS.ExistSemiClass(GotItems[CheckItem], "sq")) {
                                AcquiredItems.Add(new Vector3(CheckItem, 1f, 0f));
                                requiredAmount--;
                            } else if ( int.Parse( GS.GetSemiClass(GotItems[CheckItem], "sq") ) < requiredAmount ) {
                                AcquiredItems.Add(new Vector3(CheckItem, int.Parse(GS.GetSemiClass(GotItems[CheckItem], "sq")), 0f));
                                requiredAmount -= int.Parse(GS.GetSemiClass(GotItems[CheckItem], "sq"));
                            } else if ( int.Parse( GS.GetSemiClass(GotItems[CheckItem], "sq") ) >= requiredAmount ) {
                                AcquiredItems.Add(new Vector3(CheckItem, requiredAmount, 0f));
                                requiredAmount = 0;
                            }
                            GotItems[CheckItem] = "id-1;";//new Vector3(-1f, 0f, 0f);
                            if(requiredAmount <= 0) break;
                        }
                    }
                    
                    if (requiredAmount > 0) {
                        CanCraft = "NoResources";
                        //break;
                    }
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
                    }
                    CraftButton.GetComponent<Image>().color = new Color32(75, 55, 55, 255);
                }

                if(CraftButton.GetComponent<ButtonScript>().IsSelected == true){
                    MainCanvas.CDTdisplaye[1] = this.transform.GetSiblingIndex();
                    MainCanvas.CDTdisplaye[0] += Time.unscaledDeltaTime*3f;

                    string ToDisplay = ""; 

                    for(int cr = 0; cr < WhatToCraft.Length; cr++){
                        if(cr > 0) ToDisplay += " + ";
                        ToDisplay += GS.itemCache[int.Parse(GS.GetSemiClass(WhatToCraft[0], "id"))].getName().ToUpper();
                        //ToDisplay += GS.ReceiveItemName(float.Parse(GS.GetSemiClass(WhatToCraft[0], "id"))).ToUpper();
                        if(GS.ExistSemiClass(WhatToCraft[0], "sq") && GS.GetSemiClass(WhatToCraft[0], "sq") != "1") ToDisplay += " x" + GS.GetSemiClass(WhatToCraft[0], "sq");
                    }

                    switch(Special){
                        default:
                            ToDisplay += "\n" + GS.SetString("Handmade", "Rękodzieło");
                            break;
                    }

                    ToDisplay += "\n___________________";

                    for(int sr = 0; sr < Resources.Length; sr++){
                        if(sr == 0) ToDisplay += "\nResources:";
                        ToDisplay += "\n- " + GS.itemCache[int.Parse(GS.GetSemiClass(Resources[sr], "id"))].getName();//GS.ReceiveItemName(float.Parse(GS.GetSemiClass(Resources[sr], "id")));
                        if(GS.ExistSemiClass(WhatToCraft[0], "sq") && GS.GetSemiClass(WhatToCraft[0], "sq") != "1") ToDisplay += " x" + GS.GetSemiClass(WhatToCraft[0], "sq");
                    }

                    MainCanvas.CDTstring = ToDisplay;
                }

                if (CraftButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButton(0) && MainPlayer.CantCraft <= 0f) {
                    if (CanCraft == "") {
                        MainCanvas.PlayCraftingSound = 0.25f;
                        MainPlayer.ItemsShown.GetComponent<Animator>().Play(MainPlayer.PlayItemAnim("Pullup", GS.GetSemiClass(MainPlayer.Inventory[MainPlayer.CurrentItemHeld], "id"), ""), 0, 0f);
                        MainPlayer.ShakeCam((CraftingTime[1] / CraftingTime[0]) / 3f, 0.1f);
                        CraftingTime[1] = Mathf.Clamp(CraftingTime[1] + (0.02f * (Time.deltaTime * 50f)), 0f, CraftingTime[0]);
                        if (CraftingTime[1] >= CraftingTime[0]) {
                            GS.Mess(GS.SetString(GS.itemCache[int.Parse(GS.GetSemiClass(WhatToCraft[0], "id"))].getName() + " crafted!", "Stworzono " + GS.itemCache[int.Parse(GS.GetSemiClass(WhatToCraft[0], "id"))].getName() + "!"), "Craft");
                            MainPlayer.CantCraft = Mathf.Clamp(MainPlayer.CantCraft, 0.5f, Mathf.Infinity);
                            // Retrive resources
                            foreach (Vector3 GetResource in AcquiredItems) {
                                if (GetResource.z == 0f) {
                                    MainPlayer.InvGet(GetResource.x.ToString(), 1, (int)GetResource.y); //MainPlayer.Inventory[(int)GetResource.x] = "id0;";//Vector3.zero;
                                }
                            }
                            // Craft item
                            foreach (string SpawnItem in WhatToCraft) {
                                MainPlayer.InvGet(SpawnItem, 0);
                                /*for (int CheckInv = 0; CheckInv <= MainPlayer.MaxInventorySlots; CheckInv ++) {
                                    if (float.Parse(GS.GetSemiClass(MainPlayer.Inventory[CheckInv], "id")) <= 0f) {
                                        MainPlayer.Inventory[CheckInv] = SpawnItem;
                                        break;
                                    } else if (CheckInv == MainPlayer.MaxInventorySlots) {
                                        GameObject DropItem = Instantiate(ItemPrefab) as GameObject;
                                        DropItem.transform.position = MainPlayer.transform.position + MainPlayer.transform.forward;
                                    }
                                }*/
                            }

                            MainCanvas.SetCraftOptions();
                        }
                    } else if (CanCraft == "NoResources") {
                        CraftingTime[1] = 0f;
                        GS.Mess(GS.SetString("You lack the resources!", "Brakuje ci surowców!"), "Error");
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

            WhatToCraft = WhichTemplate.GetComponent<CraftingOption>().WhatToCraft;
            Resources = WhichTemplate.GetComponent<CraftingOption>().Resources;
            Special = WhichTemplate.GetComponent<CraftingOption>().Special;
            CraftingTime = new float[] {WhichTemplate.GetComponent<CraftingOption>().CraftingTime[0], 0f};

            this.transform.GetChild(1).GetComponent<Text>().text = GS.itemCache[int.Parse(GS.GetSemiClass(WhatToCraft[0], "id"))].getName();//GS.ReceiveItemName(WhatToCraft[0].x);
            foreach (Sprite SetIcon in MainCanvas.ItemIcons) {
                if (SetIcon.name.Substring(1) == GS.GetSemiClass(WhatToCraft[0], "id")) {
                    COIcon.transform.GetChild(0).GetComponent<Image>().sprite = SetIcon;
                    COIcon.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }
                for (int CheckResIcon = 0; CheckResIcon < 4; CheckResIcon ++) {
                    if (CheckResIcon >= Resources.Length) {
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().sprite = null;
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        COResIcons[CheckResIcon].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                        COResIcons[CheckResIcon].transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                        COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().text = "";
                    } else if (SetIcon.name.Substring(1) == GS.GetSemiClass(Resources[CheckResIcon], "id")) {
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().sprite = SetIcon;
                        COResIcons[CheckResIcon].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                        COResIcons[CheckResIcon].transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

                        COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().fontSize = 24;
                        if(GS.ExistSemiClass(Resources[CheckResIcon], "sq") && GS.GetSemiClass(Resources[CheckResIcon], "sq") != "1") COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().text = GS.GetSemiClass(Resources[CheckResIcon], "sq");
                        else COResIcons[CheckResIcon].transform.GetChild(2).GetComponent<Text>().text = "";

                    }
                }
            }

        } else {
            this.transform.localScale = Vector3.zero;
        }

    }
}
