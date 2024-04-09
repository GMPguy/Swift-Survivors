using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class ProfileScript : MonoBehaviour{

    // Variables
    GameScript GS;

    public int ProfileID = 0;
    public string Profilename = "";

    public int[] Exp = {1,0,0};

    public string Statistics = ""; // Update records is suppossed to add records, but instead, it replaces them. Fix it
    public int RecordDate = 0;
    public string Records = "";

    // Character loadout
    public string cl_Clothes = "ClassicClothes";
    public string cl_Hat = "Null";
    public string cl_Misc = "Null";
    public int cl_Skin = 0;
    public Color[] SkinColors;
    public int cl_Hair = 0;
    public Color[] HairColors;
    public float cl_Body = 0.5f;

    // Inventories - id is the string identifiactor - mr is place in memory - am is amount
    public string[] InUse = {};
    public string[] MiscItems = {};
    public string[] Clothes = {};
    public string[] Kits = {};

    // Achievements
    public string[] Achievements = {};
    public string[] Trophies = {};

    // Messages
    public List<string> Messages; // ti_Good;de_Description;vi_BonusVisuals;im_0; - importance levels 0-not 1-save 2-popup 3-popupinstant

    void Start(){

        SkinColors = new Color[]{
            new Color32(245, 240, 235, 255),
            new Color32(225, 200, 190, 255),
            new Color32(216, 194, 188, 255),
            new Color32(255, 200, 200, 255),
            new Color32(224, 185, 158, 255),
            new Color32(233, 180, 145, 255),
            new Color32(196, 163, 153, 255),
            new Color32(130, 87, 74, 255),
            new Color32(75, 55, 55, 255),
            new Color32(112, 146, 190, 255)
        };

        HairColors = new Color[]{
            new Color32(0,0,0, 255),
            new Color32(95, 63, 54, 255),
            new Color32(160, 108, 84, 255),
            new Color32(210, 134, 87, 255),
            new Color32(209, 172, 131, 255),
            new Color32(218, 187, 122, 255),
            new Color32(226, 221, 184, 255),
            new Color32(200, 200, 200, 255),
            new Color32(234, 234, 234, 255),
            new Color32(255, 255, 255, 255)
        };
        

        GS = this.GetComponent<GameScript>();
        if(ProfileID == 1337){
            SaveProfile(1337);
        }
        Exp[2] = Exp[0] * 5000;
    }

    void Update(){
        if(Time.timeSinceLevelLoad > 3600f && !hasAch("Ach_Hour")) AchProg("Ach_Hour", "0");
        if(Input.GetKeyDown(KeyCode.L)) GitExp(Exp[2]-Exp[1]);
    }

    public void GitExp(int HowMuch){
        Exp[2] = Exp[0] * 1500;
        Exp[1] += HowMuch;
        for(int cu = 0; cu < 9999; cu++){
            if (Exp[1] >= Exp[2]){

                // Rewards
                int nItems = (int)Random.Range(0f, 2.9f);
                string nRewards = GS.SetString("\n\nRewards:", "\n\nNagrody:");
                if(nItems > 0) nRewards += "\n- " + nItems + GS.SetString(" new items in inventory", " nowe przedmioty w ekwipunku");
                if(nItems <= 0) nRewards = "";

                Exp[0] += 1;
                Exp[1] -= Exp[2];
                Exp[2] = Exp[0] * 1500;
                GameObject.Find("_NewMenu").GetComponent<NewMenuScript>().PopupQueue.Add("type_LevelUp;level_" + Exp[0]);
                profMessage(GS.SetString("Level " + Exp[0], "Poziom " + Exp[0]), GS.SetString(
                    "Congratulations on achieving level " + Exp[0] + "! Good luck acquiring " + Exp[2] + " experience points for the next level.", 
                    "Gratulujemy zdobycia poziomu" + Exp[0] + "! Powodzenia w zdobywaniu " + Exp[2] + " punktów doświadczenia na następny.") + nRewards, 
                    1, "PMpromoted");
                if(Exp[0] == 10) AchProg("Ach_10level", "0");
            } else {
                break;
            }
        }
        
    }

    public void profMessage(string title, string desc, int importance = 0, string icon = "PMmessage", string spec = "PMdefault"){
        Messages.Insert(0, "ti_" + title + ";de_" + desc + ";im_" + importance + ";vi_" + icon + ";sp_" + spec + ";");
    }

    public void SaveProfile(int What){
        GS = this.GetComponent<GameScript>();

        string[] PrevProfs = GS.ListSemiClass(PlayerPrefs.GetString("ProfileSaves"), "©");
        switch(What){
            case -1: // Save profile

                string[] cols = new string[]{"", "", "", "", ""}; // 0 inuse, 1 misc, 2 clothes, 3 kits, 4 important messages
                for(int ac = 0; ac < InUse.Length; ac++) cols[0] += InUse[ac] + "/";
                for(int ac = 0; ac < MiscItems.Length; ac++) cols[1] += MiscItems[ac] + "/";
                for(int ac = 0; ac < Clothes.Length; ac++) cols[2] += Clothes[ac] + "/";
                for(int ac = 0; ac < Kits.Length; ac++) cols[3] += Kits[ac] + "/";
                for(int ac = 0; ac < Messages.ToArray().Length; ac++) if(GS.GetSemiClass(Messages[ac], "im_") != "0") cols[4] += Messages[ac] + "/";

                string DataToSave = "ID_" + ProfileID.ToString() +
                "®PN_" + Profilename +
                "®Stats_" + Statistics +
                "®Records_" + Records +
                "®RecDat_" + RecordDate +
                "®LVL_" + Exp[0].ToString() +
                "®EXP_" + Exp[1].ToString() +

                "®clCl_" + cl_Clothes +
                "®clHt_" + cl_Hat +
                "®clHr_" + cl_Hair +
                "®clSk_" + cl_Skin +
                "®clBt_" + cl_Skin +

                "®invIu_" + cols[0] +
                "®invMisc_" + cols[1] +
                "®invCl_" + cols[2] +
                "®invKt_" + cols[3] +
                "®pms_" + cols[4];

                DataToSave += AddAchievementsAndStuff("Save") + "®©";
                
                for (int AddOld = 0; AddOld < PrevProfs.Length; AddOld++) 
                    if (GS.GetSemiClass(PrevProfs[AddOld], "ID_", "®") != ProfileID.ToString()) 
                        DataToSave += PrevProfs[AddOld] + "©";

                PlayerPrefs.SetString("ProfileSaves", DataToSave);
                break;
            default: // Load or delete profile of ID
                if(What < 0){

                    print("Removing profile of id " + What.ToString());
                    string RemainingData = "";
                    for (int AddOld = 0; AddOld < PrevProfs.Length; AddOld++) if (GS.GetSemiClass(PrevProfs[AddOld], "ID_", "®") != Mathf.Abs(What).ToString()) RemainingData += PrevProfs[AddOld];
                    PlayerPrefs.SetString("ProfileSaves", RemainingData);

                } else {

                    string GottenProfile = GS.GetSemiClass(PlayerPrefs.GetString("ProfileSaves"), "ID_" + What.ToString(), "©");
                    if (GottenProfile != ""){
                        // Loading that profile
                        ProfileID = What;
                        Profilename = GS.GetSemiClass(GottenProfile, "PN_", "®");
                        Statistics = GS.GetSemiClass(GottenProfile, "Stats_", "®");
                        Records = GS.GetSemiClass(GottenProfile, "Records_", "®");
                        RecordDate = int.Parse(GS.GetSemiClass(GottenProfile, "RecDat_", "®"));
                        Exp = new int[]{int.Parse(GS.GetSemiClass(GottenProfile, "LVL_", "®")), int.Parse(GS.GetSemiClass(GottenProfile, "EXP_", "®")), 9999};
                        Exp[2] = Exp[0] * 5000;

                        cl_Clothes = GS.GetSemiClass(GottenProfile, "clCl_", "®");
                        cl_Hat = GS.GetSemiClass(GottenProfile, "clHt_", "®");
                        cl_Hair = int.Parse(GS.GetSemiClass(GottenProfile, "clHr_", "®"));
                        cl_Skin = int.Parse(GS.GetSemiClass(GottenProfile, "clSk_", "®"));
                        cl_Body = float.Parse(GS.GetSemiClass(GottenProfile, "clBt_", "®"), CultureInfo.InvariantCulture);

                        InUse = GS.ListSemiClass(GS.GetSemiClass(GottenProfile, "invIu_", "®"), "/");
                        MiscItems = GS.ListSemiClass(GS.GetSemiClass(GottenProfile, "invMisc_", "®"), "/");
                        Clothes = GS.ListSemiClass(GS.GetSemiClass(GottenProfile, "invCl_", "®"), "/");
                        Kits = GS.ListSemiClass(GS.GetSemiClass(GottenProfile, "invKt_", "®"), "/");

                        foreach(string pushback in GS.ListSemiClass(GS.GetSemiClass(GottenProfile, "pms_", "®"), "/")) Messages.Add(pushback);

                        AddAchievementsAndStuff("Add");
                        AddAchievementsAndStuff(GS.GetSemiClass(GottenProfile, "ach_", "®"));

                    } else {
                        // That profile does not exist, creating a new one
                        if(ProfileID == 1337) Profilename = "Generic Profile";
                        ProfileID = Random.Range(100000, 999999);
                        Statistics = "";
                        RecordDate = 0;
                        Records = "";
                        Exp = new int[]{1, 0, 5000};

                        cl_Clothes = "ClassicClothes";
                        cl_Hat = cl_Misc = "Null";
                        cl_Hair = (int)Random.Range(0f, 9.9f);
                        cl_Skin = (int)Random.Range(0f, 5.9f);
                        cl_Body = Random.Range(0.25f, 0.75f);

                        Achievements = InUse = MiscItems = Clothes = Kits = new string[]{};
                        AddItem("ClassicClothes", "Clothes", "Clothes", false);
                        for(int c = 10; c > 0; c--) AddItem("StarterKit", "Kits", "Kits", true);

                        AddAchievementsAndStuff("Add");
                        SaveProfile(-1);
                    }

                }
                break;
        }

    }

    string AddAchievementsAndStuff(string what){
        switch (what){
            case "Add": // Add achievements
                Achievements = new string[]{};
                AddItem("Ach_Headshot", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_Hour", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_Dinner", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_TheCycleBegins", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_TheNextDay", "Achievements", "Achievements", false, "va_1;"); // Works

                AddItem("Ach_AWholeWeek", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_StarveDeath", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_BeatIntoSubmission", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_Merchant", "Achievements", "Achievements", false, "va_10;"); // Works
                AddItem("Ach_RocketMan", "Achievements", "Achievements", false, "va_1;"); // Works

                AddItem("Ach_Fisherman", "Achievements", "Achievements", false, "va_1;vcarp_0;vgarbage_0;vnpc_0;"); //
                AddItem("Ach_Liquidator", "Achievements", "Achievements", false, "va_100;"); // Works
                AddItem("Ach_Murderer", "Achievements", "Achievements", false, "va_50;"); // Works
                AddItem("Ach_Collectioner", "Achievements", "Achievements", false, "va_10;"); // Works
                AddItem("Ach_Airborne", "Achievements", "Achievements", false, "va_1;"); // Works

                AddItem("Ach_Birthday", "Achievements", "Achievements", false, "va_1;"); //
                AddItem("Ach_10level", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_UnderwaterTreasure", "Achievements", "Achievements", false, "va_5;"); // Works
                AddItem("Ach_EmergencyFlare", "Achievements", "Achievements", false, "va_1;"); // Works
                AddItem("Ach_Shanked", "Achievements", "Achievements", false, "va_1;"); //
                return "";
            case "Save": // save achievements
                string achpass = "®ach_";
                foreach(string checkAch in Achievements) if (GS.ExistSemiClass(checkAch, "wc_"))
                    achpass += checkAch + "/";
                return achpass;
            default:
                for(int setString = 0; setString < Achievements.Length; setString++) {
                    string gitName = "id_" + GS.GetSemiClass(Achievements[setString], "id_");
                    if(GS.ExistSemiClass(what, gitName, "/")){
                        Achievements[setString] = gitName + GS.GetSemiClass(what, gitName, "/");
                    }
                }
                return "";
        }
        
    }

    public void AchProg(string achName, string scValue, string[] bonusValue = default){
        for(int ap = 0; ap <= Achievements.Length; ap++){
            if(ap == Achievements.Length){
                Debug.LogError("No achievement of name " + achName);
            } else if (GS.GetSemiClass(Achievements[ap], "id_") == achName) {
                if(bonusValue != default){
                    Achievements[ap] = GS.SetSemiClass(Achievements[ap], bonusValue[0], bonusValue[1]);
                    Achievements[ap] = GS.SetSemiClass(Achievements[ap], "wc_", "1");

                    switch(achName){
                        case "Ach_Fisherman":
                            if(GS.GetSemiClass(Achievements[ap], "vcarp_") == "1" && GS.GetSemiClass(Achievements[ap], "vgarbage_") == "1" && GS.GetSemiClass(Achievements[ap], "vnpc_") == "1") AchProg("Ach_Fisherman", "0");
                            break;
                    }
                } else if(int.Parse(GS.GetSemiClass(Achievements[ap], "va_")) > 0){
                    Achievements[ap] = GS.SetSemiClass(Achievements[ap], "va_", scValue);
                    Achievements[ap] = GS.SetSemiClass(Achievements[ap], "wc_", "1");
                    if(int.Parse(GS.GetSemiClass(Achievements[ap], "va_")) <= 0){
                        GameObject.Find("_NewMenu").GetComponent<NewMenuScript>().PopupQueue.Add("type_Achievement;achname_" + achName);
                    }
                }
                break;
            }
        }
    }

    public string getAch(string achName){
        for(int gA = 0; gA < Achievements.Length; gA++) if(GS.GetSemiClass(Achievements[gA], "id_") == achName)
            return Achievements[gA];
        return "";
    }

    public bool hasAch(string achName){
        if(int.Parse(GS.GetSemiClass(getAch(achName), "va_")) <= 0) return true;
        else return false;
    }

    // Before adding item create pm message about new items
    public void AddItem(string ID, string Array, string Type = "", bool Stackable = false, string AdditionalData = ""){

        List<string> theList = new List<string>();
        switch(Array){
            case "InUse": theList = new List<string>(InUse); break;
            case "MiscItems": theList = new List<string>(MiscItems); break;
            case "Clothes": theList = new List<string>(Clothes); break;
            case "Kits": theList = new List<string>(Kits); break;
            case "Achievements": theList = new List<string>(Achievements); break;
        }

        string theType = "";
        if(Type != "") theType = "tp_" + Type + ";";

        bool okPlace = true;
        if(Stackable){
            for(int lS = 0; lS < theList.ToArray().Length; lS++)
                if(GS.GetSemiClass(theList.ToArray()[lS], "id_") == ID) {
                    okPlace = false;
                    string[] qs = theList.ToArray();
                    qs[lS] = GS.SetSemiClass(qs[lS], "am_", "/+1");
                    theList = new List<string>(qs);
                    break;
                }

            if(okPlace) theList.Add("id_" + ID + ";am_1;mr_" + ((int)Random.Range(100000000, 999999999)).ToString() + ";" + theType + AdditionalData);

        } else {
            theList.Add("id_" + ID + ";mr_" + ((int)Random.Range(100000000, 999999999)).ToString() + ";" + theType + AdditionalData);
        }

        switch(Array){
            case "InUse": InUse = theList.ToArray(); break;
            case "MiscItems": MiscItems = theList.ToArray(); break;
            case "Clothes": Clothes = theList.ToArray(); break;
            case "Kits": Kits = theList.ToArray(); break;
            case "Achievements": Achievements = theList.ToArray(); break;
        }

    }

    public string FindParentArray(string itemData){
        for(int ca = 0; ca <= 5; ca++){
            string havIt = "";
            switch(ca){
                case 0: for (int cihi = 0; cihi < InUse.Length; cihi++) if (InUse[cihi] == itemData)
                    havIt = "InUse";
                    break;
                case 1: for (int cihi = 0; cihi < MiscItems.Length; cihi++) if (MiscItems[cihi] == itemData)
                    havIt = "MiscItems";
                    break;
                case 2: for (int cihi = 0; cihi < Clothes.Length; cihi++) if (Clothes[cihi] == itemData)
                    havIt = "Clothes";
                    break;
                case 3: for (int cihi = 0; cihi < Kits.Length; cihi++) if (Kits[cihi] == itemData)
                    havIt = "Kits";
                    break;
                case 4: for (int cihi = 0; cihi < Achievements.Length; cihi++) if (Achievements[cihi] == itemData)
                    havIt = "Achievements";
                    break;
                case 5:
                    havIt = "Null";
                    break;
            }
            if (havIt != "") return havIt;
        }
        return "shit";
    }

    public void MoveItem(string itemData, string toArray){

        RemoveItem(GS.GetSemiClass(itemData, "mr_"));

        List<string> theList = new List<string>();
        switch(toArray){
            case "InUse": theList = new List<string>(InUse); break;
            case "MiscItems": theList = new List<string>(MiscItems); break;
            case "Clothes": theList = new List<string>(Clothes); break;
            case "Kits": theList = new List<string>(Kits); break;
            case "Achievements": theList = new List<string>(Achievements); break;
        }

        theList.Add(itemData);

        switch(toArray){
            case "InUse": InUse = theList.ToArray(); break;
            case "MiscItems": MiscItems = theList.ToArray(); break;
            case "Clothes": Clothes = theList.ToArray(); break;
            case "Kits": Kits = theList.ToArray(); break;
            case "Achievements": Achievements = theList.ToArray(); break;
        }
        
    }

    public void RemoveItem(string ID, int amount = 0){
        // amount0 = remove completely, else remove as much as in amount
        for(int wA = 0; wA <= 4; wA++){

            if(wA == 4){
                Debug.LogError("Could not find an item of id " + ID + " in any of the arrays.");
            } else {
                string[] checkThisOne = new string[]{};
                switch(wA){
                    case 0: checkThisOne = InUse; break;
                    case 1: checkThisOne = MiscItems; break;
                    case 2: checkThisOne = Clothes; break;
                    case 3: checkThisOne = Kits; break;
                }

                bool hasIt = false;
                List<string> renew = new List<string>();
                for(int hi = 0; hi < checkThisOne.Length; hi++){
                    if(GS.ExistSemiClass(checkThisOne[hi], "mr_") && GS.GetSemiClass(checkThisOne[hi], "mr_") == ID) {
                        hasIt = true;
                    } else {
                        renew.Add(checkThisOne[hi]);
                    }
                }

                if(hasIt){
                    switch(wA){
                        case 0: InUse = renew.ToArray(); break;
                        case 1: MiscItems = renew.ToArray(); break;
                        case 2: Clothes = renew.ToArray(); break;
                        case 3: Kits = renew.ToArray(); break;
                    }
                    return;
                }
            }

        }
    }

    // Functions that retrive inventory items info
    public string GetProfileItemName(string ID){
        switch(ID){

            case "ClassicClothes": return GS.SetString("Classic clothes", "Klasyczny ubiór");
            case "SoldierClothes": return GS.SetString("Soldier clothes", "Mundur żołnierza");

            case "StarterKit": return GS.SetString("Starter kit", "Zestaw startowy");

            case "Ach_Headshot": return GS.SetString("Boom headshot", "Bum, strzał w głowę");
            case "Ach_Hour": return GS.SetString("One hour", "Jedna godzina");
            case "Ach_Dinner": return GS.SetString("Dinner", "Obiad");
            case "Ach_TheCycleBegins": return GS.SetString("The cycle begins", "Cykl rozpoczęty");
            case "Ach_TheNextDay": return GS.SetString("The next day", "Następny dzień");
            case "Ach_AWholeWeek": return GS.SetString("Weekend", "Fajrant");
            case "Ach_StarveDeath": return GS.SetString("Starvation", "Śmierć głodowa");
            case "Ach_BeatIntoSubmission": return GS.SetString("Beaten into submission", "Przemówiono do rozsądku");
            case "Ach_Merchant": return GS.SetString("Merchant", "Kupiec");
            case "Ach_RocketMan": return GS.SetString("Rocket man", "Pan od rakietów");
            case "Ach_Fisherman": return GS.SetString("Fishing fanatic", "Fanatyk wędkarstwa");
            case "Ach_Liquidator": return GS.SetString("Liquidator", "Likwidator");
            case "Ach_Murderer": return GS.SetString("Murderer", "Morderca");
            case "Ach_Collectioner": return GS.SetString("Collectionner", "Kolekcjoner");
            case "Ach_Airborne": return GS.SetString("Airborne", "Desantowiec");
            case "Ach_Birthday": return GS.SetString("Birthday", "Urodziny");
            case "Ach_10level": return GS.SetString("10th level", "10 poziom");
            case "Ach_UnderwaterTreasure": return GS.SetString("Underwater treasures", "Podwodne skarby");
            case "Ach_EmergencyFlare": return GS.SetString("Emergency flare", "Flara awaryjna");
            case "Ach_Shanked": return GS.SetString("Shanked", "Shanked");

            default: return "";
        }
    }

    // Functions that retrive inventory items desc
    public string GetProfileItemDesc(string ID){
        string achDesc = GS.SetString("(Description hidden)", "(Opis ukryty)");
        switch(ID){

            case "ClassicClothes":
                return GS.SetString("A pair of pale blue jeans, woolen sweater, and an old conscript jacket.\n\nIt should do.", "Para bladych dżinsów, wełniany sweter, i stara kurtka poborowego.\n\nPowinno wystarczyć.");
            case "SoldierClothes":
                return GS.SetString("Soldier clothes", "Mundur żołnierza");



            case "StarterKit":
                return GS.SetString(
                    "A kit given out to all new players. After creation of profile, it can be used a total of 10 times.\n\nThis kit gives you:\n- Backpack\n- Colt pistol, and a fireaxe\n- Two cans of soup\n\nIntendent for new players, to make the first few games a bit easier.", 
                    "Zestaw rozdawany każdemu nowemu graczu. Po utworzeniu profilu, można z niego skorzystać 10 razy\n\nZestaw zawiera:\n- Plecak\n- Pistolet Colt'a, i siekera strażacka\n- Dwie puszki z zupą\n\nZ myślą o nowych graczach, w celu ułatwienia kilku pierwszych gier.");



            case "Ach_Headshot-A":
                return GS.SetString(
                    "Surprisingly, it's very hard to kill someone this way.\n\n- Kill someone with one shot to the head",
                    "O dziwo, taka metoda zabójstwa jest trudna do wykonania.\n\n- Zabij kogoś jednym strzałem w głowę");
            case "Ach_Headshot-B":
                return achDesc + GS.SetString(
                    "\n\n- Kill someone with one shot to the head",
                    "\n\n- Zabij kogoś jednym strzałem w głowę");
            case "Ach_Hour-A":
                return GS.SetString(
                    "You have managed to play for an hour straight! This isn't really impressive.\n\n- Play for an hour straight",
                    "Udało ci się grać w tą grę przez godzinę! Nie jesteśmy pod wrażeniem.\n\n- Graj przez godzinę bez przerw");
            case "Ach_Hour-B":
                return achDesc + GS.SetString(
                    "\n\n- Play for an hour straight",
                    "\n\n- Graj przez godzinę bez przerw");
            case "Ach_Dinner-A":
                return GS.SetString(
                    "If you wanna run away from the nuclear holocaust, it might be wise to be well fed before the departure!\n\n- Have enough excess food point to get a full stomach the next round",
                    "Jak masz zamiar uciekać przed nuklearnym holokaustem, wypadało by dobrze podjeść przed wyprawą!\n\n- Miej wystarczająco zapasowych punktów jedzenia, by mieć pełen brzuch w następnej rundzie");
            case "Ach_Dinner-B":
                return achDesc + GS.SetString(
                    "\n\n- Have enough excess food point to get a full stomach the next round",
                    "\n\n- Miej wystarczająco zapasowych punktów jedzenia, by mieć pełen brzuch w następnej rundzie");
            case "Ach_TheCycleBegins-A":
                return GS.SetString(
                    "That survivor has died. This wasn't the first nor the last one to go in this whole ordeal, but it's the first time that has happened to you.\n\n- Die",
                    "Twój niedobitek zmarł. Nie był to pierwszy ni ostatni raz jak ktoś zmarł w tym całym zamieszaniu, lecz to był pierwszy raz kiedy to tobie się przytrafiło.\n\n- Zgiń");
            case "Ach_TheCycleBegins-B":
                return achDesc + GS.SetString(
                    "\n\n- Die",
                    "\n\n- Zgiń");
            case "Ach_TheNextDay-A":
                return GS.SetString(
                    "So you've managed to survive your first day. I hope you've slept well during the night!\n\n- Play on classic mode\n- Survive 5 rounds in a row",
                    "A więc udało ci się przeżyć swój pierwszy dzień. Mam nadzieję, że dobrze się wam spało tej nocy!\n\n- Graj w trybie klasycznym\n- Przeżyj 5 pierwszych rund");
            case "Ach_TheNextDay-B":
                return achDesc + GS.SetString(
                    "\n\n- Play on classic mode\n- Survive 5 rounds in a row",
                    "\n\n- Graj w trybie klasycznym\n- Przeżyj 5 pierwszych rund");
            case "Ach_AWholeWeek-A":
                return GS.SetString(
                    "You have managed to die, on the evening, on the fifth day of your run. I know it's weekend, but you didn't need to die on Friday night.\n\n- Play on classic mode\n- Die on the 19th round",
                    "Udało ci się zginąć, wieczorem, piątego dnia swojej wędrówki. Wiem że jest weekend, ale nie musiałeś zdychać w piątek piąteczek piątunio.\n\n- Graj w trybie klasycznym\n- Zgiń podczas rundy 19");
            case "Ach_AWholeWeek-B":
                return achDesc + GS.SetString(
                    "\n\n- Play on classic mode\n- Die on the 19th round",
                    "\n\n- Graj w trybie klasycznym\n- Zgiń podczas rundy 19");
            case "Ach_StarveDeath-A":
                return GS.SetString(
                    "You know what happens when you try to leave maps on empty stomach. The only thing to wonder about now is: was that death by starvation, or by... you know...\n\n- Try to escape while starving\n- Die",
                    "Wiesz co się dzieje, jak próbujesz uciec z mapy na pustym żołądku. Jedyną rzeczą nie wiadomą jest teraz: czy to była śmierć z głodu, czy... z... no wiesz...\n\n- Spróbuj uciec będąc głodnym\n- Zgiń");
            case "Ach_StarveDeath-B":
                return achDesc + GS.SetString(
                    "\n\n- Try to escape while starving\n- Die",
                    "\n\n- Spróbuj uciec będąc głodnym\n- Zgiń");
            case "Ach_BeatIntoSubmission-A":
                return GS.SetString(
                    "Considering what you have done, I'm surprised they haven't ran away the first they saw you!\n\n- Attack a bandit with a gun, using a melee weapon\n- Make the bandit run away from fear",
                    "Biorąc pod uwagę czego dokonaliście, dziwi mnie, że ci bandyci nie uciekli na sam wasz widok!\n\n- Zaatakuj bandytę z bronią palną, przy użyciu broni białej\n- Spraw, by bandyta uciekł od ciebie");
            case "Ach_BeatIntoSubmission-B":
                return achDesc + GS.SetString(
                    "\n\n- Attack a bandit with a gun, using a melee weapon\n- Make the bandit run away from fear",
                    "\n\n- Zaatakuj bandytę z bronią palną, przy użyciu broni białej\n- Spraw, by bandyta uciekł od ciebie");
            case "Ach_Merchant-A":
                return GS.SetString(
                    "You've made quite a lot of transactions in your playthrough. Whether they were worth it or not, is of lesser concern.\n\n- Trade items with other survivors a total of 10 times",
                    "Udało ci się dokonać sporej ilości transakcji podczas grania. To czy były tego warte czy nie, nie jest istotne.\n\n- Dokonaj handlu z innymi niedobitkami 10 razy w sumie");
            case "Ach_Merchant-B":
                return achDesc + GS.SetString(
                    "\n\n- Trade items with other survivors a total of 10 times",
                    "\n\n- Dokonaj handlu z innymi niedobitkami 10 razy w sumie");
            case "Ach_RocketMan-A":
                return GS.SetString(
                    "The 'Extinguisher' part of that item wasn't enough. So we added another one.\n\n- Fly using fire extinguisher at an altitude of 10m",
                    "Samo gaszenie, nie starczało. Dlatego dodaliśmy dodatkową funkcję dla gaśnicy.\n\n- Poleć 10m do góry używając gaśnicy");
            case "Ach_RocketMan-B":
                return achDesc + GS.SetString(
                    "\n\n- Fly using fire extinguisher at an altitude of 10m",
                    "\n\n- Poleć 10m do góry używając gaśnicy");
            case "Ach_Fisherman-A":
                return GS.SetString(
                    "Wędkarstwo to przydatna umiejętność! Dzięki temu, możesz wiele rzeczy osiągnąć w desperackich sytuacjach.\n\n- Fish out a carp\n- Fish out a garbage\n- Pull a fleeing NPC using fishing rod",
                    "Fishing is a very useful skill! With it, uou can achieve a lot in desperate situations.\n\n- Wyłów karpia\n- Wyłów jakiegoś śmiecia\n- Przyciągnij uciekającego NPC wędką");
            case "Ach_Fisherman-B":
                return achDesc + GS.SetString(
                    "\n\n- Fish out a carp\n- Fish out a garbage\n- Pull a fleeing NPC using fishing rod",
                    "\n\n- Wyłów karpia\n- Wyłów jakiegoś śmiecia\n- Przyciągnij uciekającego NPC wędką");
            case "Ach_Liquidator-A":
                return GS.SetString(
                    "That's a lot of dead mutants. And yet, there's still a lot of them out there.\n\n- Kill 100 mutants in classic mode",
                    "To sporo martwych mutantów. A jednak, wciąż jest ich sporo na zewnątrz.\n\n- Zabij 100 mutantów w klasycznym trybie");
            case "Ach_Liquidator-B":
                return achDesc + GS.SetString(
                    "\n\n- Kill 100 mutants in classic mode",
                    "\n\n- Zabij 100 mutantów w klasycznym trybie");
            case "Ach_Murderer-A":
                return GS.SetString(
                    "We're unsure, if that high amount of homicides was necessary... But given the fact there is no functioning government where you were then, you'll be let off easy probably.\n\n- Kill 50 people",
                    "Nie jesteśmy pewni, czy taka ilość zabójstw była potrzebna... ale ponieważ nie ma żadnego działającego rządu tam gdzie byliście w trakcie tego, nic wam raczej nie będzie.\n\n- Zabij 50 ludzi");
            case "Ach_Murderer-B":
                return achDesc + GS.SetString(
                    "\n\n- Kill 50 people",
                    "\n\n- Zabij 50 ludzi");
            case "Ach_Collectioner-A":
                return GS.SetString(
                    "Ah, pretty things aren't they? No idea why you would want to sell them, but you did it anyway - fulfilling a niche on the market.\n\n- Sell 4 treasures found at monuments",
                    "Ah, ładne cudeńka czyż nie? Nie wiem dlaczego chcieliście je sprzedać, no ale zostały sprzedane - wypełniacie niszę na rynku.\n\n- Sprzedaj 4 skarby które można znaleźć w monumentach");
            case "Ach_Collectioner-B":
                return achDesc + GS.SetString(
                    "\n\n- Sell 4 treasures found at monuments",
                    "\n\n- Sprzedaj 4 skarby które można znaleźć w monumentach");
            case "Ach_Airborne-A":
                return GS.SetString(
                    "The frame must be like, made from pure titanium. You know what, I won't bother guessing how that would actually work in real life.\n\n- Use umbrella to glide for 5 seconds",
                    "Framuga musi być wykonana chyba z tytanu. Wiecie co, może lepiej nie będę zgadywał jakby to miało działać w prawdziwym życiu.\n\n- Użyj parasolu do szybowania przez 5 sekund");
            case "Ach_Airborne-B":
                return achDesc + GS.SetString(
                    "\n\n- Use umbrella to glide for 5 seconds",
                    "\n\n- Użyj parasolu do szybowania przez 5 sekund");
            case "Ach_Birthday-A":
                return GS.SetString(
                    "Happy birthday me! Here's a cake for you.\n\n- Play this game on 19th of May",
                    "Sto lat ja! Masz torta.\n\n- Zagraj w tą grę 19 maja");
            case "Ach_Birthday-B":
                return achDesc + GS.SetString(
                    "\n\n- Play this game on 19th of May",
                    "\n\n- Zagraj w tą grę 19 maja");
            case "Ach_10level-A":
                return GS.SetString(
                    "Congratulations! The progress that you've managed to achieve so far is pretty decent.\n\n- Get the 10th level on your profile",
                    "Gratulacje! Postęp który udało ci się osiągnąć do tej pory jest godny podziwu.\n\n- Zdobądź 10 poziom na tym profilu");
            case "Ach_10level-B":
                return achDesc + GS.SetString(
                    "\n\n- Get the 10th level on your profile",
                    "\n\n- Zdobądź 10 poziom na tym profilu");
            case "Ach_UnderwaterTreasure-A":
                return GS.SetString(
                    "These dark and traitorous cavities contain treasures... which can be easily found on the surface. And yet, you have prepared yourself for all those deep diving expeditions. You must have felt the depth, calling for you...\n\n- Collect 5 items from caves under water",
                    "Te ciemne i zdradliwe czeluści posiadają skarby... które łatwo można znaleźć na powierzchni. Ale i tak, zadaliście sobie trudu w przygotowaniach do podwodnych ekspedycji. Widocznie, poczuliście wezwanie do tychże głębin.\n\n- Zdobądź 5 przedmiotów z podwodnych jaskiń");
            case "Ach_UnderwaterTreasure-B":
                return achDesc + GS.SetString(
                    "\n\n- Collect 5 items from caves under water",
                    "\n\n- Zdobądź 5 przedmiotów z podwodnych jaskiń");
            case "Ach_EmergencyFlare-A":
                return GS.SetString(
                    "Flares can be used to light and mark areas. But they can also burn stuff obviously! Not the most efficient way to kill someone, but still.\n\n- Kill someone by throwing flare at them",
                    "Flary mogą oświetlać i oznaczać tereny. Ale można również nimi podpalać! Nie jest to zbyt efektywny sposób na zabicie kogoś, ale jest.\n\n- Zabij kogoś rzucając w niego flarą");
            case "Ach_EmergencyFlare-B":
                return achDesc + GS.SetString(
                    "\n\n- Kill someone by throwing flare at them",
                    "\n\n- Zabij kogoś rzucając w niego flarą");
            case "Ach_Shanked-A":
                return GS.SetString(
                    "God save the que... king!\n\n- Get drunk\n- Kill someone with 1 hit using a knife",
                    "Niech żyje królow... król!\n\n- Upij się\n- Zabij kogoś 1 uderzeniem noża");
            case "Ach_Shanked-B":
                return achDesc + GS.SetString(
                    "\n\n- Get drunk\n- Kill someone with 1 hit using a knife",
                    "\n\n- Upij się\n- Zabij kogoś 1 uderzeniem noża");

            default:
                return "";
        }
    }

}
