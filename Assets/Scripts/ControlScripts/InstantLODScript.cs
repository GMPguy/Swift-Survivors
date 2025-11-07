using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantLODScript : MonoBehaviour {

    public GraphicGroup[] Groups;
    public MeshCollider InheritedCollider;

    // Start is called before the first frame update
    void Start() {
        GameScript.CachedLODs.Add(this);
        SetLevel(true);
    }

    void OnDestroy() {
        if (GameScript.CachedLODs.Contains(this))
            GameScript.CachedLODs.Remove(this);
    }

    public void SetLevel(bool initial) {
        GameScript GS = null;
        if (GameObject.Find("_GameScript"))
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        else
            return;
        
        for (int cg = 0; cg < Groups.Length; cg++) {
            GraphicGroup group = Groups[cg];

            bool main = false;
            for (int gq = 0; gq < group.WhichQuality.Length; gq++) {
                main = group.WhichQuality[gq] == GS.GraphicsQuality;
                if (main)
                    break;
            }

            for (int so = 0; so < group.Objects.Length; so++)
                group.Objects[so].GetComponent<MeshRenderer>().enabled = main;

            if (initial && cg == GS.GraphicsQuality && InheritedCollider)
                InheritedCollider.sharedMesh = group.MainCollider;
        }
    }

    [System.Serializable]
    public struct GraphicGroup {
        public GameObject[] Objects;
        public Mesh MainCollider;
        public int[] WhichQuality;
    }
}
