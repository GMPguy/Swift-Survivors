using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour {

    // Main Variables
    public float InteractDistance = -1f;
    public Vector3 Offset;
    GameObject MainPlayer;
    GameObject RS;
    GameObject MainCamera;
    GameObject MainCanvas;

    public string[] Icons = new string[] { "" };
    public string[] Options = new string[] { "" };
    public int ThisOption = 0;

    GameScript GS;

	// Use this for initialization
	void Start () {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        if (InteractDistance < 0f) {
            InteractDistance = 3f;
        }

        MainPlayer = GameObject.FindGameObjectWithTag("Player");
        if (MainPlayer != null) {
            MainPlayer.GetComponent<PlayerScript>().ScannedInteractables.Add(this.gameObject);
        }
        RS = GameObject.Find("_RoundScript");
        MainCamera = GameObject.Find("MainCamera");
        MainCanvas = GameObject.Find("MainCanvas");

    }
	
	// Update is called once per frame
	public string SetText () {

        string ReturnThis = "";
        if (Options[ThisOption] == "PickUp") {
            ReturnThis = GS.GetComponent<GameScript>().SetString(
                "Pick Up " + GS.itemCache[int.Parse(GS.GetSemiClass(this.GetComponent<ItemScript>().Variables, "id"))].getName(), 
                "Podnieś " + GS.itemCache[int.Parse(GS.GetSemiClass(this.GetComponent<ItemScript>().Variables, "id"))].getName());
        } else if (Options[ThisOption] == "BreakBarel") {
            ReturnThis = GS.GetComponent<GameScript>().SetString(
                "Break " + this.transform.parent.GetComponent<InteractableScript>().Name + " (" + (int)this.transform.parent.GetComponent<InteractableScript>().Variables.y + ")", 
                "Zniszcz " + this.transform.parent.GetComponent<InteractableScript>().Name + " (" + (int)this.transform.parent.GetComponent<InteractableScript>().Variables.y + ")");
        } else if (Options[ThisOption] == "EscapeTunel") {
            ReturnThis = GS.GetComponent<GameScript>().SetString("Escape via tunel", "Ucieknij przez tunel");
        } else if (Options[ThisOption] == "Door") {
            if (this.transform.parent.GetComponent<InteractableScript>().Variables.z == 0f) {
                ReturnThis = GS.GetComponent<GameScript>().SetString("Open", "Otwórz");
            } else if (this.transform.parent.GetComponent<InteractableScript>().Variables.z == 1f) {
                ReturnThis = GS.GetComponent<GameScript>().SetString("Close", "Zamnkij");
            } else if (this.transform.parent.GetComponent<InteractableScript>().Variables.z == 2f) {
                ReturnThis = GS.GetComponent<GameScript>().SetString("Locked...", "Zamknięte...");
            }
        } else if (Options[ThisOption] == "VendingMachine") {
            ReturnThis = GS.GetComponent<GameScript>().SetString("Buy supplies", "Kup zasoby");
        } else if (Options[ThisOption] == "EmergencyItem") {
            if (this.transform.parent.GetComponent<InteractableScript>().Variables.z > -1f) {
                ReturnThis = GS.GetComponent<GameScript>().SetString("Break glass", "Stłucz szybę");
            } else {
                ReturnThis = GS.GetComponent<GameScript>().SetString("It's empty", "Puste");
            }
        } else if (Options[ThisOption] == "RingBell") {
            ReturnThis = GS.GetComponent<GameScript>().SetString("Begin the wave", "Rozpocznij fale");
        } else if (Options[ThisOption] == "TalkTo") {
            ReturnThis = GS.GetComponent<GameScript>().SetString("Talk to " + this.GetComponent<MobScript>().MobName, "Rozmawiaj z " + this.GetComponent<MobScript>().MobName);
        } else if (Options[ThisOption] == "Loot") {
            ReturnThis = GS.GetComponent<GameScript>().SetString(
                "Loot", 
                "Przeszukaj");
        } else if (Options[ThisOption] == "GrabAmmo") {
            ReturnThis = GS.GetComponent<GameScript>().SetString(
                "Grab ammo (", 
                "Bierz amunicję (") + (int)this.transform.parent.GetComponent<InteractableScript>().Variables.y + ")";
        }

        return ReturnThis;

	}
}
