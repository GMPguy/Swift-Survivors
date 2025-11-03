using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveryScript : MonoBehaviour {

    // Stats
    public string[] Info;
    public int Score;
    public bool Discovered;

    // References
    public MinimapMarker Marker;
    PlayerScript PS;
    GameScript GS;

    // Start is called before the first frame update
    void Start() {
        RoundScript.CachedDiscoveries.Add(this);
    }

    // Update is called once per frame
    public void TheUpdate() {

        if (!GS || !PS) {
            if (GameObject.Find("_GameScript"))
                GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

            if (GameObject.FindWithTag("Player"))
                PS = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
            return;
        }
        
        if (Vector3.Distance(PS.transform.position, this.transform.position) < GameObject.Find("MainCamera").GetComponent<Camera>().farClipPlane * 0.75f && Discovered == false)
            Found();

    }

    public bool Found (bool tell = true) {
        if (Discovered)
            return false;
        
        if (tell)
            GS.Mess(GS.SetString(Info[0], Info[1]), "Draw");

        Discovered = true;
        GS.AddToScore(Score);
        Marker.gameObject.SetActive(true);

        return true;
    }
}
