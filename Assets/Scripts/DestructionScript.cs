using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionScript : MonoBehaviour {

    // Main variables
    public float Health = 10f;
    float prevHealth = 10f;
    public string mainType = "";
    public string subType = "";
    public string penType = "";

    public string State = "";
    public float KeepState = 0f;
    RoundScript RS;
    GameScript GS;
    GameObject ItemPrefab;
    bool dropedLoot = false;
    Vector3 orgScale;

    // Misc
    Vector3[] TreeLean;
    List<DestructionScript> Anchors;

    // Start is called before the first frame update
    void Start() {
        RS = GameObject.FindObjectOfType<RoundScript>();
        GS = RS.GS;
        ItemPrefab = RS.ItemPrefab;
        prevHealth = Health;
        orgScale = this.transform.localScale;

        switch(mainType){
            case "Tree":
                TreeLean = new Vector3[]{this.transform.eulerAngles, this.transform.eulerAngles, this.transform.localScale};
                break;
            case "Construction": case "Construction3":
                TreeLean = new Vector3[]{this.transform.localScale, this.transform.eulerAngles, this.transform.localScale * Random.Range(0.8f, 0.9f), this.transform.eulerAngles + Vector3.one*Random.Range(-10f, 10f)};
                break;
            case "Construction2":
                TreeLean = new Vector3[]{this.transform.position, this.transform.position - Vector3.up, this.transform.eulerAngles, this.transform.eulerAngles + Vector3.one*Random.Range(-15f, 15f)};
                break;
        }
    }

    // Update is called once per frame
    public void Do() {

        if(KeepState > 0f){
            KeepState -= Time.deltaTime;

            // States
            switch(State){
                case "Chop":
                    this.transform.eulerAngles = Vector3.Lerp(TreeLean[0], TreeLean[1], KeepState);
                    break;
                case "Timber": case "Bashed":
                    if(KeepState < 1f){
                        if(this.GetComponent<Rigidbody>()) 
                            Destroy(this.GetComponent<Rigidbody>());
                        this.transform.localScale = Vector3.Lerp( Vector3.zero, orgScale, KeepState);
                    } else if(this.GetComponent<MeshCollider>() && !this.GetComponent<MeshCollider>().convex){
                        this.GetComponent<MeshCollider>().convex = true;
                        Rigidbody rig = this.gameObject.AddComponent<Rigidbody>();
                        rig.mass = State == "Bashed" ? 1f : 10f;

                        if (State == "Bashed") {
                            transform.localScale *= .8f;
                            rig.AddForce(new (
                                Random.Range(-10f, 10f),
                                Random.Range(0f, 5f),
                                Random.Range(-10f, 10f)
                            ), ForceMode.VelocityChange);

                            rig.AddTorque(new (
                                Random.Range(-100f, 100f),
                                Random.Range(-100f, 100f),
                                Random.Range(-100f, 100f)
                            ), ForceMode.VelocityChange);
                        }
                    } else if (!this.GetComponent<Rigidbody>()) {
                        Rigidbody rig = this.gameObject.AddComponent<Rigidbody>();
                        rig.mass = State == "Bashed" ? 1f : 10f;

                        if (State == "Bashed") {
                            transform.localScale *= .8f;
                            rig.AddForce(new (
                                Random.Range(-10f, 10f),
                                Random.Range(0f, 5f),
                                Random.Range(-10f, 10f)
                            ), ForceMode.VelocityChange);

                            rig.AddTorque(new (
                                Random.Range(-100f, 100f),
                                Random.Range(-100f, 100f),
                                Random.Range(-100f, 100f)
                            ), ForceMode.VelocityChange);
                        }
                    }
                    break;
                case "Construction":
                    if(KeepState < 1f){
                        this.transform.localScale = Vector3.Lerp( Vector3.zero, orgScale, KeepState);
                    } else {
                        this.transform.position = Vector3.Lerp( TreeLean[1], TreeLean[0], KeepState-1f);
                        this.transform.eulerAngles = Vector3.Lerp( TreeLean[3], TreeLean[2], KeepState-1f);
                    }
                    break;
            }

        } else {
            if(RS.ActiveDestructs.Contains(this)) {
                RS.ActiveDestructs.Remove(this);
                RS.ActiveDestructs.TrimExcess();
            }
            if(Health <= 0f) {
                if(State == "Timber") dropLoot();
                Destroy(this.gameObject);
            }
        }
        
    }

    public void Hit(float Damage, string[] AttackType, Vector3 AttackHit, GameObject killer = null){

        if(Health > 0f){

            // Penetration types
            bool pened = false;
            switch(penType){
                case "Tree": if (AttackType[0] != "Melee" || (AttackType.Length > 1 && AttackType[1] == "Axe")) {
                    Health -= Damage; pened = true; }
                    break;
                default:
                    Health -= Damage; pened = true;
                    break;
            }

            if(pened == true){
                if(Health > 0f){

                    // Hit effect
                    switch(mainType){
                        case "Tree":
                            if (GS.DestructionQuality > 0) {
                                KeepState = 1f;
                                State = "Chop";
                                TreeLean[1] = TreeLean[0] + Vector3.one*15f;
                            }
                            break;
                        case "Construction": case "Construction3":
                            if (GS.DestructionQuality > 0) {
                                this.transform.localScale = Vector3.Lerp(TreeLean[2], TreeLean[0], Health/prevHealth);
                                this.transform.eulerAngles = Vector3.Lerp(TreeLean[3], TreeLean[1], Health/prevHealth);
                            }

                            if (subType == "Window" && transform.childCount > 0 && transform.GetChild(0).name == "Glass") {
                                Transform effect = GameObject.Instantiate(RS.EffectPrefab).transform;
                                effect.position = transform.position;
                                effect.GetComponent<EffectScript>().EffectName = "GlassBreak";
                                effect.GetComponent<EffectScript>().EffectColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
                                Destroy(transform.GetChild(0).gameObject);
                            }
                            break;
                        default:
                            break;
                    }

                } else {

                    if (killer && killer.tag == "Player")
                        RS.SetScore("ObjectsDestroyed_", "/+1");

                    // Destroy effect
                    switch(mainType){
                        case "Tree":
                            KeepState = GS.DestructionQuality == 0 ? .9f : 5f;
                            State = "Timber";
                            TreeLean[1] = TreeLean[0] + Vector3.one*15f;
                            break;
                        case "Construction":
                            KeepState = GS.DestructionQuality switch {
                                0 => .9f,
                                1 => 10f,
                                _ => 300f
                            };
                            State = "Timber";
                            TreeLean[1] = TreeLean[0] + Vector3.one*15f;
                            break;
                        case "Construction3":
                            KeepState = GS.DestructionQuality switch {
                                0 => .9f,
                                1 => 10f,
                                _ => 300f
                            };
                            State = "Bashed";
                            TreeLean[1] = TreeLean[0] + Vector3.one*15f;

                            if (subType == "Window" && transform.childCount > 0 && transform.GetChild(0).name == "Glass") {
                                Transform effect = GameObject.Instantiate(RS.EffectPrefab).transform;
                                effect.position = transform.position;
                                effect.GetComponent<EffectScript>().EffectName = "GlassBreak";
                                effect.GetComponent<EffectScript>().EffectColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
                                Destroy(transform.GetChild(0).gameObject);
                            }
                            break;
                        case "Construction2":
                            KeepState = 2f;
                            State = "Construction";
                            break;
                        default:
                            dropLoot();
                            Destroy(this.gameObject);
                            break;
                    }

                    // Destroy anchors
                    if(Anchors != null) {
                        DestructionScript[] bAnchors = Anchors.ToArray();
                        for (int bye = 0; bye < bAnchors.Length; bye++) 
                            bAnchors[bye].Hit(9999f, new[]{"Broke"}, this.transform.position, killer);
                    }

                }   
            }

            // must effect out
            if(KeepState > 0f && !RS.ActiveDestructs.Contains(this))
                RS.ActiveDestructs.Add(this);

        }

    }

    public void Anchor(DestructionScript stick){
        if(Anchors == null) Anchors = new();
        Anchors.Add(stick);
    }

    void dropLoot(){
        if(!dropedLoot){

            switch(subType){
                case "Tree": case "TreePalm": case "TreeApple":
                    for(int Wood = Random.Range(1, 3); Wood > 0; Wood--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[140].startVariables);//"id140;va0;sq1;";
                    }

                    if (subType=="TreeApple") for(int Apple = Random.Range(1, 3); Apple > 0; Apple--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[1].startVariables);// "id1;va0;sq1;";
                    }

                    int pickfrut = (int)Random.Range(119f, 120.9f);
                    if (subType=="TreePalm") for(int BanCoc = Random.Range(1, 3); BanCoc > 0; BanCoc--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[pickfrut].startVariables);
                    }
                    break;
                case "TreeBig":
                    for(int Wood = Random.Range(3, 9); Wood > 0; Wood--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[140].startVariables);// "id140;va0;sq1;";
                    }
                    break;
                case "TreeDead":
                    for(int Wood = Random.Range(-2, 2); Wood > 0; Wood--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        if(Random.Range(0f, 1f) > 0.5f) Wooddrop.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[140].startVariables);// "id140;va0;sq1;";
                        else Wooddrop.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[147].startVariables);// "id147;va0;sq1;";
                    }
                    break;
            }

            dropedLoot = true;
        }
    }

    void OnDestroy(){
        if(RS.ActiveDestructs.Contains(this)) {
            RS.ActiveDestructs.Remove(this);
            RS.ActiveDestructs.TrimExcess();
        }
    }

}
