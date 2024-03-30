using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ManualScript : MonoBehaviour
{

    // References
    GameScript GS;
    NewMenuScript NMS;
    public Sprite[] mImages;

    // Manual structs
    public List<mOption> StoredRoots;
    public List<mOption> Displayed;
    public class mOption {
        public string ID;
        public bool Show;
        public int ParentID = 0;

        public mOption(string setID, bool setShow, int setParentID, List<mOption> PassHere){
            ID = setID; Show = setShow; ParentID = setParentID; PassHere.Add(this);
        }
    }


    void Start(){
        StoredRoots = new List<mOption>();
        Displayed = new List<mOption>();

        if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        NMS = this.GetComponent<NewMenuScript>();

        // Implement options
        new mOption("Intro", true, 0, StoredRoots); // Works
        new mOption("", true, 0, StoredRoots); // Works
        new mOption("GameModes", true, 0, StoredRoots);
        new mOption("Classic", true, 1, StoredRoots);
        new mOption("Horde", true, 1, StoredRoots);
        new mOption("Casual", true, 1, StoredRoots);
        new mOption("", true, 0, StoredRoots);
        new mOption("Mechanics", true, 0, StoredRoots);
        new mOption("Movement", true, 1, StoredRoots);
        new mOption("Swimming", true, 1, StoredRoots);
        new mOption("Health", true, 1, StoredRoots);
        new mOption("Hunger", true, 1, StoredRoots);
        new mOption("Inventory", true, 1, StoredRoots);
        new mOption("Crafting", true, 1, StoredRoots);
        new mOption("", true, 0, StoredRoots);
        new mOption("Interface", true, 0, StoredRoots);
        new mOption("Main", true, 1, StoredRoots);
        new mOption("Tab", true, 1, StoredRoots);
        new mOption("", true, 0, StoredRoots);
        new mOption("Interacting", true, 0, StoredRoots);
        new mOption("Barrels", true, 1, StoredRoots);
        new mOption("Exits", true, 1, StoredRoots);
        new mOption("EmergencyBox", true, 1, StoredRoots);
        new mOption("Cowbell", true, 1, StoredRoots);
        new mOption("VendingMachines", true, 1, StoredRoots);
        new mOption("", true, 0, StoredRoots);
        new mOption("Items", true, 0, StoredRoots);
        new mOption("FoodItems", true, 1, StoredRoots);
        new mOption("UtilityItems", true, 1, StoredRoots);
        new mOption("Weapons", true, 1, StoredRoots);
        new mOption("Melee", true, 2, StoredRoots);
        new mOption("Guns", true, 2, StoredRoots);
        new mOption("", true, 0, StoredRoots);
        new mOption("Map", true, 0, StoredRoots);
        new mOption("Radiation", true, 1, StoredRoots);
        new mOption("Monuments", true, 1, StoredRoots);
        new mOption("DayNight", true, 1, StoredRoots);
        new mOption("", true, 0, StoredRoots);
        new mOption("Mobs", true, 0, StoredRoots);
        new mOption("Survivors", true, 1, StoredRoots);
        new mOption("Bandits", true, 1, StoredRoots);
        new mOption("Mutants", true, 1, StoredRoots);
        new mOption("Guards", true, 1, StoredRoots);

    }

    public void Display(){
        Displayed.Clear();

        foreach(mOption passRoot in StoredRoots) if (passRoot.Show)
            Displayed.Add(passRoot);
    }

    public void ShowData(string thisID){
        NMS.mTextes[0].text = RetriveMOdata(thisID, 0); NMS.mTextes[1].text = RetriveMOdata(thisID, 1);
        if(RetriveMOdata(thisID, 2) == ""){
            NMS.mImage.transform.localScale = Vector3.zero;
        } else {
            NMS.mImage.transform.localScale = Vector3.one;
            for(int gim = 0; gim < mImages.Length; gim++) if (mImages[gim].name == RetriveMOdata(thisID, 2)) {
                NMS.mImage.sprite = mImages[gim];
                break;
            }
        }
    }

    public void Unravel(string ID){
        for(int fID = 0; fID < StoredRoots.ToArray().Length; fID++) if(StoredRoots[fID].ID == ID) {
            StoredRoots[fID].Show = true;
            break;
        }
    }

    public string RetriveMOdata(string moID, int What = 0){
        switch(moID){
            case "":
                if(What == 0) return GS.SetString("", ""); 
                else if (What == 1) return GS.SetString("", ""); 
                else return "";
            case "GameModes":
                if(What == 0) return GS.SetString("Game modes", "Tryby gry"); 
                else if (What == 1) return GS.SetString("In Swift Survivors, you may choose to play either of the three game modes:\n\n- Classic\n- Horde mode\n- Casual", "W grze Swift Survivors, można zagrać w któryś z trzech trybów gry:\n\n- Klasyczny\n- Tryb hordy\n- Swobodny"); 
                else return "";
            case "Classic":
                if(What == 0) return GS.SetString("Classic mode", "Tryb klasyczny"); 
                else if (What == 1) return GS.SetString("The classic mode, is the default game mode.\n\nThe game is divided into rounds. At the beginning of a round, you get spawned in the middle of a map. Your objective, is to find an exit (at the borders of the map) before the nuke gets dropped.\n\nYou must find and eat food - trying to escape on empty stomach, might get you killed.\n\nEscaping map ends the current round, and starts a new one.\n\nThis is an endless mode - you can only get higher scores.", "Tryb klasyczny, jest podstawowym trybem gry.\n\nGra jest podzielona na rundy. Na początku rundy, pojawiasz się na środku mapy. Twoim zadaniem jest odnalezienie wyjścia (na granicach mapy) zanim uderzy bomba atomowa.\n\nMusisz znaleźć jedzenie - próba ucieczki z mapy na pustym żołądku, może cię zabić.\n\nUcieczka z mapy kończy obecną rundę, i zaczyna nową.\n\nTryb nie ma końca - można jedynie podbijać wyniki."); 
                else return "";
            case "Horde":
                if(What == 0) return GS.SetString("Horde mode", "Tryb hordy"); 
                else if (What == 1) return GS.SetString("In horde mode, you get spawned on a pre-made map, and must defend yourself from waves of mutants.\n\nBefore each wave, you get a short intermission time, during which you can acquire new weapons and items.\n\nDuring waves, you must kill a certain amount of mutants. With each new wave, there will be more mutants, their strength will increase, and there will be more special mutants.\n\nYou don't need to worry about food and ammo. This is an endless mode - you can only get higher scores.", "W trybie hordy, pojawiasz się na gotowej mapie, i musisz się bronić przeciwko falom mutantów.\n\nPrzed każdą falą, posiadasz krótką przerwę, podczas której możesz zaopatrywać się w nowe bronie i przedmioty.\n\nPodczas fali, musisz zabić konkretną ilość mutantów. Z każdą nową falą, będzie więcej mutantów, ich siła wzrośnie, a także będzie więcej mutantów specjalnych.\n\nNie musisz się martwić o jedzenie i amunicję. Tryb nie ma końca - można jedynie podbijać wyniki."); 
                else return "";
            case "Casual":
                if(What == 0) return GS.SetString("Casual mode", "Tryb swobodny"); 
                else if (What == 1) return GS.SetString("This mode is the same as classic, albeit certain elements have been simplified and/or changed:\n\n- There is no hunger and hurting debuffs\n- Guns are more manageable\n- Mobs take longer to kill you\n-Maps don't degrade with time\n- Disposable items, might have more uses\n\nThis mode is recommended for people, who found the base game too demanding.", "Tryb taki sam jak klasyczny, aczkolwiek niektóre elementy zostały uproszczone lub zmienione:\n\n- Nie ma systemu głodu, oraz raniących debuff'ów\n- Bronie palne kontroluje się łatwiej\n- Zabicie gracza zajmuje postaciom dłużej\n- Mapy nie ulegają degradacji z czasem\n- Zużywalne przedmioty, mogą mieć dłuższe żywotności\n\nTryb przeznaczony dla osób, które uznały podstawkę za zbyt wymagającą."); 
                else return "";
            case "Intro":
                if(What == 0) return GS.SetString("Introduction", "Wprowadzenie"); 
                else if (What == 1) return GS.SetString("Welcome to the ''Survival Manual'' - this booklet contains all the information you'll need, in order to not die as fast.\n\nThis booklet will be automatically opened, should you press ESC button, when a hint appears.", "Witamy w ''Podręczniku Przetrwania'' - ta książeczka posiada wszystkie informacje, które pozwolą ci przeżyć trochę dłużej.\n\nTa książeczka automatycznie się otworzy, jak naciśniesz ESC, podczas pojawienia się podpowiedzi."); 
                else return "";
            case "Mechanics":
                if(What == 0) return GS.SetString("Mechanics", "Mechaniki"); 
                else if (What == 1) return GS.SetString("Aside from the basic WSAD to move and LMB to shoot stuff, the game contains a fair share of mechanics, which may need a more complex explanation.\n\nREMEMBER - you can change key binding in options!", "Poza poruszaniem się WSAD'em i strzelaniem PPM, gra posiada kilka mechanik, które mogą wymagać dokładniejszego wytłumaczenia.\n\nPAMIĘTAJ - możesz zmieniać przypisane klawisze w opcjach!"); 
                else return "";
            case "Movement":
                if(What == 0) return GS.SetString("Movement", "Poruszanie się"); 
                else if (What == 1) return GS.SetString("Much like every other first person perspective game, your character can be controlled like so:\n\nW - Move forward\nS - move backwards\nA - Move left\nD - Move right\nSPACE - Jump\nC - Crouch\nLEFT SHIFT - Sprint\n\nJumping and sprinting will drain your stamina, so use them wisely.", "Jak każda inna gra z widoku pierwszoosobowego, swoją postacią można kontrolować tak:\n\nW - Do przodu\nS - Do tyłu\nA - W lewo\nD - W prawo\nSPACJA - Skok\nC - Kucnięcie\nLEWY SHIFT - Sprint\n\nSkakanie i bieganie czerpie staminę, dlatego używaj ich rozsądnie."); 
                else return "";
            case "Swimming":
                if(What == 0) return GS.SetString("Swimming", "Pływanie"); 
                else if (What == 1) return GS.SetString("You can walk on water, but it'll slow your character, and make them wet.\n\nYou can't crouch in water, but if you find a deep enough pool of water, crouching will result in diving.\n\nYou use the same set of keys to swim underwater, except the forward and backwards directions are relative to where you look.\n\nWithout special equipment, you can stay only for 30 seconds underwater - staying past that, will slowly suffocate your character.", "Można chodzić po wodzie, jednak to spowolni i zmoczy twoją postać.\n\nNie można kucać w wodzie, ale w wystarczająco głębokich zbiornikach, kucanie spowoduje nurkowanie.\n\nPodczas nurkowania, używa się tych samych klawiszy, tylko że tył i przód są względem kierunku patrzenia.\n\nBez specjalnego ekwipunku, pod wodą można być maksymalnie tylko przez 30 sekund - po czym postać zacznie się dusić."); 
                else return "";
            case "Health":
                if(What == 0) return GS.SetString("Health", "Zdrowie"); 
                else if (What == 1) return GS.SetString("The most important thing for you to worry about, is the amount of health you have!\n\nAnything from getting hit by an enemy, falling down, escaping on empty stomach, will lower you health by a certain amount. Passive buffs like bleeding, cold, radiation, will also lower your health over time (without notifying you).\n\nWhen your health drops to a total zero, your character unconditionally dies, and that ends the game.", "Najważniejszą dla ciebie rzeczą, jest ilość twojego zdrowia!\n\nWszystko od uderzeń wrogów, spadania, uciekania z pustym żołądkiem, obniży twój poziom zdrowia o konkretną ilość. Pasywne debuffy jak krwawienie, zimno, czy promieniowanie, również obniży twój poziom zdrowia (bez ostrzegania).\n\nGdy zdrowie spadnie do zera całkowitego, twoja postać natychmiast umiera, co kończy grę."); 
                else return "";
            case "Hunger":
                if(What == 0) return GS.SetString("Food points", "Punkty jedzenia"); 
                else if (What == 1) return GS.SetString("Since this is a survival game, you'll obviously have to worry about staying fed.\n\nAt the beginning of each round, you'll start with 0 food points, and a ''maximum'' amount of food points. In order to escape from map, you'll need at least one food point; otherwise you'll lose some health in exchange for some of them.\n\nEating food items will increase your food points by a certain amount. You can go pass the maximum amount of food points. Food points do not deplete over time.\n\nDepending on your food points, you might get either of the five food levels:\n\nStarving - you lose health\nHungry - you get one punishment\nFine - nothing happens\nWell fed - one reward\nFull - two rewards, the excess amount gets put into the next round\n\nMaximum amount of food points also increases/decreases.", "Ponieważ to jest gra survivalowa, musisz dbać o swój poziom pożywienia.\n\nNa początku każdej rundy, zaczynasz z 0 punktami jedzenia, i ''maksymalną'' ilością punktów jedzenia. Żeby uciec z mapy, musisz posiadać przynajmniej jeden punkt; inaczej stracisz trochę zdrowia w zamian za kilka punktów.\n\nJedzenie przedmiotów spożywczych podniesie punkty jedzenia o konkretną liczbę. Można przekroczyć maksymalną liczbę. Punkty jedzenia nie opadają z czasem.\n\nW zależności od liczby punktów, możesz otrzymać jeden z pięciu poziomów jedzenia:\n\nUmierający - tracisz zdrowie\n-Głodny - dostajesz karę\nW porządku - nic się nie dzieje\nNajedzony - dostajesz nagrodę\nPełen - dwie nagrody, a nadwyżka przeniesiona zostaje do następnej rundy.\n\nMaksymalna liczba zmienia się w zależności od tychże poziomów."); 
                else return "";
            case "Inventory":
                if(What == 0) return GS.SetString("Inventory and equipment", "Ekwipunek i wyposażenie"); 
                else if (What == 1) return GS.SetString("INVENTORY\nYou can carry items with yourself. You can switch items using 0-9 keys, or the scroll wheel. You start with 4 item slots, but can increase to a total of 10 slots.\n\nEQUIPMENT\nCertain items can be worn. Upon wearing an item, it'll be put into your equipment. You can equip only 4 items at a time. You can check your equipment in the information tab - and you can unequip items, by clicking on them in said tab.", "EKWIPUNEK\nMożesz nosić przedmioty ze sobą. Przedmioty można zmieniać używając klawiszy 0-9, lub kółka myszy. Grę zaczynasz z 4 miejscami na przedmioty, ale można je podnieść do 10 miejsc.\n\nWYPOSAŻENIE\nNiektóre przedmioty można założyć. Można założyć tylko 4 przedmioty na raz. Wyposażenie można sprawdzić w menu informacyjnym - można tam zdjąć przedmioty, klikając na ich ikonki."); 
                else return "";
            case "Crafting":
                if(What == 0) return GS.SetString("Crafting", "Tworzenie"); 
                else if (What == 1) return GS.SetString("You can open crafting menu, by holding B key.\n\nIf your character will poses an item used in any crafting recipe, that recipe will be shown in crafting menu.\n\nIn order to craft an item, you need to have all the resources, and hold the craft button.\n\nDifferent recipes will take different amounts of time to craft, and may require special utilities to craft them (like being near a campfire, or having a sharp tool).", "Menu tworzenia można otworzyć przytrzymując klawisz B.\n\nJeżeli będziesz w posiadaniu przedmiotu używanego do tworzenia czegoś, receptura pojawi się w menu.\n\nŻeby stworzyć przedmiot, musisz posiadać wszystkie składniki, i musisz przytrzymać przycisk do tworzenia.\n\nRóżne receptury, pochłoną różne ilości czasu potrzebnego do stworzenia przedmiotu, oraz mogą również wymagać dodatkowych narzędzi (jak ognia, lub ostrego narzędzia)."); 
                else return "";
            case "Interface":
                if(What == 0) return GS.SetString("Game's interface", "Interfejs gry"); 
                else if (What == 1) return GS.SetString("Aside from the main HUD containing the most important variables, there are also a few tabs you can open during game to either get information or manipulate the game in one way or another.", "Poza głównym HUD'em posiadającym najważniejsze zmienne, znajduje się również kilka menu pobocznych które można otworzyć podczas gry, w celu zdobycia informacji, bądź też w celu zmanipulowania gry w jakiś sposób."); 
                else return "";
            case "Main":
                if(What == 0) return GS.SetString("Main HUD", "HUD główny"); 
                else if (What == 1) return GS.SetString("The main HUD contains following elements:\n\nHealth bar - shows your current and maximum health\nFood bar - shows your current and maximum food points, as well as food level\nMinimap - shows your field of view, and nearby map elements (like mobs and exits)\nInventory - shows your items, and variables of currently held item\nStamina and oxygen bars - bars which appear when either stamina or oxygen are being depleted", "Główny HUD posiada następujące elementy:\n\nPasek zdrowia - pokazuje obecny i maksymalny poziom zdrowia\nPasek jedzenia - pokazuje obecny i maksymalny poziom jedzenia\nMinimapa - pokazuje twoje pole widzenia, jak i pobliskie elementy (jak moby i wyjścia)\nEkwipunek - pokazuje twoje przedmioty, oraz wartości obecnie trzymanego przedmiotu\nPasek staminy i tlenu - paski pojawiające się gdy stamina lub tlen są czerpane"); 
                else return "";
            case "Tab":
                if(What == 0) return GS.SetString("Information tab", "Menu informacyjne"); 
                else if (What == 1) return GS.SetString("Information tab can be opened in-game by holding TAB button. It contains some useful information, like:\n\nMap - shows your position on game's map, and where certain landmarks are\nEquipment - shows the items you're wearing\nBuffs - shows your buffs in more detail. You can hover cursor over the icons to read their descriptions\nRound information - stuff like current round, day time, and your objectives\nItem information - shows description of the item you're currently holding", "Menu informacyjne można otworzyć przytrzymując TABULATOR. Posiada przydatne informacje, takie jak:\n\nMapa - pokazuje twoją pozycję na mapie gry, i gdzie są punkty warte uwagi\nWyposażenie - pokazuje wyposażone przedmioty\nBuffy - pokazuje twoje buffy. Możesz najechać kursorem na ich ikonki, żeby przeczytać ich opis\nInformacje odnośnie rundy - takie jak obecna runda, czas dnia, i twoje zadania\nInformacje odnośnie przedmiotów - pokazuje opis trzymanego przedmiotu"); 
                else return "";
            case "Interacting":
                if(What == 0) return GS.SetString("Interacting", "Interakcje"); 
                else if (What == 1) return GS.SetString("You can interact with stuff around you, by pressing E, when an interaction icon appears.\n\nIt will appear as soon as you approach an item you can interact with.\n\nWith many items at once, the one nearest the center of your screen will be picked.", "Możesz wchodzić w interakcje z przedmiotami, naciskając E, gdy ikonka interakcji się pojawi.\n\nPojawi się natychmiastowo po zbliżeniu się do przedmiotu z którym można wchodzić w interakcje./n/nPrzy kilku przedmiotach na raz, wybrany zostanie najbliższy środkowi ekranu."); 
                else return "";
            case "Barrels":
                if(What == 0) return GS.SetString("Barrels", "Beczki"); 
                else if (What == 1) return GS.SetString("Barrels can be found spread out around the map. They can break open, which releases a certain amount of items.\n\nInteracting will deal 1 point of damage to a barrel - but you can also use other means of dealing damage in order to open them.\n\nThere are many types of barrels, with different loot and durability - the better ones appear the more rounds you've survived.", "Beczki są porozrzucane po całej mapie. Można je zniszczyć, co spowoduje uwolnieniem kilku przedmiotów do zebrania.\n\nInterakcja spowoduje zadanie 1 punktu obrażeń - ale można też je zadawać na inne sposoby, ażeby beczkę otworzyć.\n\nJest wiele rodzai beczek, z różnymi przedmiotami i wytrzymałością - te lepsze pojawiają się im więcej rund przegrasz."); 
                else return "";
            case "Exits":
                if(What == 0) return GS.SetString("Exits", "Wyjścia"); 
                else if (What == 1) return GS.SetString("In order to finish a round in classic and casual modes, you'll have to escape the map. In order to escape a map, you'll need to find an exit.\n\nESCAPE TUNNEL - The most common exit. They appear somewhere in the middle of the map borders - and there can be up to 4 of them.\n\nREMEMBER - Make sure you have at least 1 food point before escaping!", "Żeby zakończyć rundę w trybie klasycznym i swobodnym, musisz uciec z mapy. Żeby tego dokonać, musisz odnaleźć jakieś wyjście.\n\nTUNEL EWAKUACYJNY - Najpopularniejsze wyjście. Pojawiają się w środku granic mapy - i może ich być od 1 do 4.\n\nPAMIĘTAJ - Miej przynajmniej 1 punkt jedzenia przed wyjściem!"); 
                else return "";
            case "EmergencyBox":
                if(What == 0) return GS.SetString("Emergency item box", "Skrzynka z przedmiotem"); 
                else if (What == 1) return GS.SetString("A small red box containing an item.\n\nBy interacting with it, you break the glass, and get the item inside the box.\n\nMostly used in horde mode, where you can get your starting items from.", "Mała czerwona skrzynka z przedmiotem.\n\nWchodząc w interakcję, tłuczesz szybę skrzynki, i dostajesz przedmiot znajdujący się wewnątrz.\n\nGłównie znajdują się w trybie hordy, gdzie można z nich zdobyć przedmioty startowe."); 
                else return "";
            case "Cowbell":
                if(What == 0) return GS.SetString("Cowbell", "Dzwonek"); 
                else if (What == 1) return GS.SetString("It skips the intermission stage in horde mode.", "Pomija przerwę w trybie hordy."); 
                else return "";
            case "VendingMachines":
                if(What == 0) return GS.SetString("Vending machines", "Automaty"); 
                else if (What == 1) return GS.SetString("Vending machines can be found in all of the game modes - you can buy items from them for money.\n\nIn classic mode - there are different types of machines, selling one specific type of items (like weapons or medicine) for money.\n\nIn horde mode - after each wave they sell different items for different prices.", "Automaty z przedmiotami znajdują się we wszystkich trybach gry - można tam zdobywać przedmioty za pieniądze.\n\nW trybie klasycznym - jest kilka rodzai maszyn, sprzedających jeden gatunek przedmiotów za pieniądze.\n\nW trybie hordy - po każdej fali, sprzedają inne przedmioty, w różnych cenach. "); 
                else return "";
            case "Items":
                if(What == 0) return GS.SetString("Items", "Przedmioty"); 
                else if (What == 1) return GS.SetString("Items can be found everywhere, and you can carry them. They allow you to manipulate the world around you.\n\nYou can pick up items, by pressing E button when nearby. You can drop them, by pressing Q button. You can also THROW items, by holding Q button.\n\nYou can use item, by pressing left or right mouse button.", "Przedmioty są wszędzie, i możesz je ze sobą nosić. Pozwalają zmieniać świat wokół ciebie.\n\nMożesz podnosić przedmioty, za pomocą klawisza E, gdy są w pobliżu. Możesz upuszczać przedmioty za pomocą klawisza Q. Możesz również rzucać przedmiotami, przytrzymując klawisz Q.\n\nZ przedmiotów można korzystać, używając lewego i prawego przycisku myszy."); 
                else return "";
            case "FoodItems":
                if(What == 0) return GS.SetString("Food items", "Przedmioty spożywcze"); 
                else if (What == 1) return GS.SetString("One of the three main categories of items, are food items. They have only one use - being consumed.\n\nFood items give you a certain amount of food points, after which they disappear from your inventory. Some items might give you bonus buffs.\n\nSimilar to food items, there are also medical items. Instead of food points, they can give you back some health points, and they may get rid of some of the debuffs (like bleeding or broken bones).", "Jedną z trzech głównych kategorii przedmiotów, jest jedzenie. Ma tylko jedno zastosowanie - bycie spożytym.\n\nJedzenie daje tobie kilka punktów jedzenia, po czym znika z ekwipunku. Niektóre przedmioty do zjedzenia mogą oferować dodatkowe buffy.\n\nPodobnymi do jedzenia, są medykamenty. Zamiast punktów jedzenia, mogą tobie przywrócić punkty zdrowia, i mogą również pozbyć się części debuff'ów (jak krwotok lub połamane kości)."); 
                else return "";
            case "UtilityItems":
                if(What == 0) return GS.SetString("Utility items", "Przedmioty użytkowe"); 
                else if (What == 1) return GS.SetString("One of the three main categories of items, are utility items.\n\nEach item has completely different use from others - you should check information tab for those.", "Jedną z trzech głównych kategorii przedmiotów, są przedmioty użytkowe.\n\nKażdy przedmiot ma zupełnie inne zastosowanie od innych - musisz patrzeć do menu informacyjnego dla ich zastosowań."); 
                else return "";
            case "Weapons":
                if(What == 0) return GS.SetString("Weapons", "Bronie"); 
                else if (What == 1) return GS.SetString("One of the three main categories of items, are weapons. They deal damage, and can be used to destroy buildings, protect yourself, and kill other living creatures! There are many different types of weapons.\n\nIf you find a weapon tho, try not to abuse them. You might not find that many weapons, and some of them can be used up pretty quickly.\n\nUse them wisely!", "Jedną z trzech głównych kategorii przedmiotów, są bronie. Zadają obrażenia, mogą być użyte do niszczenia, bronienia się, i zabijania innych żywych istot! Jest wiele różnych rodzai broni.\n\nJak znajdziesz jakąś broń, staraj się jej nie nadużywać. Możesz mieć problemy ze znajdywaniem ich, a niektóre można zużyć bardzo szybko.\n\nUżywaj ich rozważnie!"); 
                else return "";
            case "Melee":
                if(What == 0) return GS.SetString("Melee weapons", "Broń biała"); 
                else if (What == 1) return GS.SetString("Upon using them, your character will swing it, and hit anything withing standing ~3 meter in front. Swinging takes stamina - not having enough of it, will prevent you from using the weapon. Hitting stuff, will decrease weapon's durability, which will destroy it should it drop to 0.\n\nWith some weapons, you can parry oncoming melee attacks - it also takes up stamina, and when successfully parried, durability as well.", "Podczas używania, postać wymachuje bronią, i uderza wszystko stojące ~3 metrów przed nią. Wymachiwanie pobiera staminę - nie wystarczająco duża ilość jej, nie pozwoli ci z niej korzystać. Uderzanie, obniży wytrzymałość broni, a jej upadek do 0, ją zniszczy.\n\nZ niektórych broni, można parować nadchodzące uderzenia z broni białych - to też pobiera staminę, a udane parowanie, również wytrzymałość."); 
                else return "";
            case "Guns":
                if(What == 0) return GS.SetString("Guns", "Broń palna"); 
                else if (What == 1) return GS.SetString("Guns are ranged weapons, which use ammo to shoot fast and deadly projectiles.\n\nYou can reload them by pressing R (if you have the right ammo type with you), after which you can shoot it.\n\nStuff like constant firing, or moving, decrease gun's accuracy. There are a few categories of guns, which differ heavily in accuracy, speed, effective distance, recoil patterns, needed ammo type, and whatnot.\n\nHorde mode is a good place to test out weapons, and get yourself familiar with their mechanics.", "Bronie palne używają amunicji, do strzelania szybkimi i śmiertelnymi pociskami.\n\nMożna je przeładować naciskając R (jeżeli masz odpowiednią amunicję), po czym można z niej strzelać.\n\nRzeczy takie jak bieganie, czy ciągły ostrzał, obniżają celność broni. Jest kilka kategorii broni palnych, które to różnią się prędkością, dystansem, odrzutem, celnością, potrzebną amunicją, i innymi.\n\nTryb hordy jest dobrym miejscem na wypróbowanie broni palnych, jak i zapoznaniem się z ich mechanikami."); 
                else return "";
            case "Map":
                if(What == 0) return GS.SetString("Game's map", "Mapa gry"); 
                else if (What == 1) return GS.SetString("On classic and casual modes, you get spawned in a 500 squared meter randomly generated map. It is divided into 10x10 tiles. The type of tiles, amount of items, radiation, mods, are based on the map's biome.\n\nOn horde mode, you choose a pre-built map on which you'll play.", "W trybie klasycznym i swobodnym, pojawiasz się na losowo generowanej mapie o powierzchni 500 metrów kwadratowych. Jest podzielona na 10x10 kafelków. Rodzaje kafelków, ilość przedmiotów, promieniowania, i mobów, zależą od biomu mapy.\n\nW trybie hordy, wybierasz gotową mapę na której będziesz grał."); 
                else return "";
            case "Radiation":
                if(What == 0) return GS.SetString("Radiation", "Promieniowanie"); 
                else if (What == 1) return GS.SetString("Radiation is a dangerous zone, in which you can get gain radiation sickness. Zones contaminated with radiation, can be checked on map in the information tab.\n\nThe amount as well as severity of radiation, changes depending on biomes and rounds played.", "Promieniowanie do niebezpieczne strefy, na których można złapać chorobę popromienną. Strefy skażone promieniowaniem, można zobaczyć na mapie w menu informacyjnym.\n\nIlość jak i siła promieniowania, zmienia się z biomem i ilością przegranych rund."); 
                else return "";
            case "Monuments":
                if(What == 0) return GS.SetString("Monuments", "Monumenty"); 
                else if (What == 1) return GS.SetString("Monuments are big urban structures, containing special and rare items known as ''Treasures''. Treasures have powerful uses, but can also be sold to other survivors.\n\nBEWARE - Monuments are protected by guards, who will try to kill anyone near the monuments.", "Monumenty to duże konstrukcje, zawierające specjalne i rzadkie przedmioty zwane ''Skarbami''. Skarby posiadają potężne zastosowania, ale można je również sprzedawać innym niedobitkom.\n\nUWAGA - Monumentów strzegą strażnicy, którzy zabiją każdego kto się zbliży do monumentów."); 
                else return "";
            case "DayNight":
                if(What == 0) return GS.SetString("Day and night cycle", "Cykl dnia i nocy"); 
                else if (What == 1) return GS.SetString("The day and night cycle is divided into four stages:\n\nMorning - round 1, between 6:00 and 12:00\nAfternoon - round 2, between 12:00 and 18:00\nEvening - round 3, between 18:00 and 21:00. This is where it's getting dark\nNight - round 4, between 0:00 and 3:00 .This is when it's completely dark\n\n After round 4, the cycle repeats. During night, the minimap gets disabled.", "Cykl dnia i nocy jest podzielony na cztery etapy:\n\nPoranek - runda 1, między 6:00 a 12:00\nPopołudnie - runda 2, między 12:00 a 18:00\nWieczór - runda 3, między 18:00 a 21:00. Wtedy zaczyna się ściemniać\nNoc - runda 4, między 0:00 a 3:00. Wtedy jest już kompletna ćma\n\nPo rundzie 4, cykl się powtarza. Podczas nocy, minimapa zostaje wyłączona."); 
                else return "";
            case "Mobs":
                if(What == 0) return GS.SetString("Mobs", "Moby"); 
                else if (What == 1) return GS.SetString("Mobs are living creatures that roam the map.\n\nWith some of them, you can interact, but others might be hostile.\n\nMobs near you are shown on the minimap - the red ones are hostile, and will try to kill you.", "Moby to żywe istoty które chodzą po mapach.\n\nZ niektórymi można wchodzić w interakcje, inne zaś mogą być wrogo nastawione.\n\nPobliskie moby są wyświetlane na minimapie - te na czerwono są wrogie, i będą próbować cię zabić."); 
                else return "";
            case "Survivors":
                if(What == 0) return GS.SetString("Survivors", "Niedobitki"); 
                else if (What == 1) return GS.SetString("Your fellow survivors\nColor - lime green\n\nThey wear gas masks, appear solo or in packs, and are friendly. You can talk to them, trade with them, gain information, or sell your treasures.\n\nYou can also attack them - they will become hostile though, alert nearby survivors, and will try to kill you (even if you run away, and then reappear).", "Niedobitki takie jak ty\nKolor - limonkowy zielony\n\nNoszą maski gazowe, chodzą solo lub w grupach, i są przyjaźni. Możesz z nimi rozmawiać, handlować, zdobywać informacje, i sprzedawać skarby.\n\nMożesz również ich atakować - staną się wrogo nastawieni, zawiadomią o tym pobliskich niedobitków, i spróbują cię zabić (nawet jak znikniesz i się pojawisz na nowo)."); 
                else return "";
            case "Bandits":
                if(What == 0) return GS.SetString("Bandits", "Bandyci"); 
                else if (What == 1) return GS.SetString("A hostile survivors\nColor - red\n\nBandits are the second most common mob type you'll encounter. They're human survivors, usually found in groups. They carry all sorts of weapons, and much like with mutants, there can be specialized bandits.\n\nThe more rounds you play, the deadlier weapons they'll have, and there also will be more specialized bandits.\n\nIf you don't carry firearms with yourself, it is highly advised to avoid them.", "Wrogo nastawione niedobitki\nKolor - czerwony\n\nBandyci są drugim najczęściej spotykanym rodzajem mobów. Są to ludzie, przeważnie chodzący w grupach. Noszą różnego rodzaju broń, oraz jak w przypadku mutantów, mogą występować wyspecjalizowani bandyci.\n\nIm więcej rund przegrasz, tym bardziej śmiercionośną broń będą posiadać, oraz będzie więcej wyspecjalizowanych bandytów.\n\nJak nie masz broni palnej ze sobą, lepiej jest ich unikać."); 
                else return "";
            case "Mutants":
                if(What == 0) return GS.SetString("Mutants", "Mutanci"); 
                else if (What == 1) return GS.SetString("These people, instead of dying... they changed.\nColor - red\n\nMutants are the most common mob type you'll encounter. They used to be people, but due to certain circumstances, they have mutated into zombie like creatures.\n\nThey're dumb, but will try hard to kill any human they'll spot. The more rounds you play, the faster, stronger, and more specialized they'll become.", "Ci ludzie, zamiast umrzeć... ulegli zmianie.\nKolor - czerwony\n\nMutanci to najczęściej spotykany rodzai mobów. Kiedyś byli ludźmi, ale ze wzgląd na pewne okoliczności, ulegli przemianie w istoty podobne do zombi.\n\nSą głupi, ale będą próbować zabić każdego napotkanego człowieka. Im więcej rund przegrasz, tym szybcy, silniejsi, i bardziej wyspecjalizowani się stanom."); 
                else return "";
            case "Guards":
                if(What == 0) return GS.SetString("Guards", "Strażnicy"); 
                else if (What == 1) return GS.SetString("Very dangerous sentries\nColor - blue\n\nGuards are highly professional, highly organized, members of some kind of a paramilitary organization. They are equipped with very powerful firearms, are tough to kill, and don't panic.\n\nThey can be usually found protecting some key locations on the map, mainly monuments.\n\nWho are they, who are they working for, and why in spite of all this chaos there is an organized group like them? No one knows.", "Bardzo niebezpieczni wartownicy\nKolor - niebieski\n\nStrażnicy to wysoce profesjonalni, zorganizowani, członkowie jakiejś grupy paramilitarnej. Są wyposażeni w potężne bronie palne, trudno ich jest zabić, i nie panikują.\n\nPrzeważnie bronią kluczowych lokacji na mapie, głównie monumentów.\n\nKim oni są, dla kogo pracują, i dlaczego w tym całym chaosie znajduje się zorganizowana grupa jaką są oni? Nikt tego nie wie."); 
                else return "";
            default:
                if(What == 0) return "Uknown entry ''" + moID + "''"; else if (What == 1) return ""; else return "";
        }
    }

}
