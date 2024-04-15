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

    // Misc
    Vector3[] TreeLean;

    // Start is called before the first frame update
    void Start() {
        RS = GameObject.FindObjectOfType<RoundScript>();
        GS = RS.GS;
        ItemPrefab = RS.ItemPrefab;
        prevHealth = Health;

        switch(mainType){
            case "Tree":
                TreeLean = new Vector3[]{this.transform.eulerAngles, this.transform.eulerAngles, this.transform.localScale};
                break;
            case "Construction":
                TreeLean = new Vector3[]{this.transform.localScale, this.transform.eulerAngles, this.transform.localScale * Random.Range(0.8f, 0.9f), this.transform.eulerAngles + Vector3.one*Random.Range(-10f, 10f)};
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
                case "Timber":
                    if(KeepState < 1f){
                        if(this.GetComponent<Rigidbody>()) Destroy(this.GetComponent<Rigidbody>());
                        this.transform.localScale = Vector3.Lerp( Vector3.zero, TreeLean[2], KeepState);
                    } else if(this.GetComponent<MeshCollider>() && !this.GetComponent<MeshCollider>().convex){
                        this.GetComponent<MeshCollider>().convex = true;
                        this.gameObject.AddComponent<Rigidbody>();
                        this.GetComponent<Rigidbody>().mass = 10f;
                    } else {
                        this.gameObject.AddComponent<Rigidbody>();
                        this.GetComponent<Rigidbody>().mass = 10f;
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

    public void Hit(float Damage, string[] AttackType, Vector3 AttackHit){

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
                            KeepState = 1f;
                            State = "Chop";
                            TreeLean[1] = TreeLean[0] + Vector3.one*15f;
                            break;
                        case "Construction":
                            this.transform.localScale = Vector3.Lerp(TreeLean[0], TreeLean[2], Health/prevHealth);
                            this.transform.eulerAngles = Vector3.Lerp(TreeLean[1], TreeLean[3], Health/prevHealth);
                            break;
                        default:break;
                    }

                } else {

                    // Destroy effect
                    switch(mainType){
                        case "Tree": case "Construction":
                            KeepState = 5f;
                            State = "Timber";
                            TreeLean[1] = TreeLean[0] + Vector3.one*15f;
                            break;
                        default:
                            dropLoot();
                            Destroy(this.gameObject);
                            break;
                    }

                }   
            }

            // must effect out
            if(KeepState > 0f && !RS.ActiveDestructs.Contains(this))
                RS.ActiveDestructs.Add(this);

        }

    }

    void dropLoot(){
        if(!dropedLoot){

            switch(subType){
                case "Tree": case "TreePalm": case "TreeApple":
                    for(int Wood = Random.Range(1, 3); Wood > 0; Wood--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables = "id140;va0;sq1;";
                    }

                    if (subType=="TreeApple") for(int Apple = Random.Range(1, 3); Apple > 0; Apple--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables = "id1;va0;sq1;";
                    }

                    int pickfrut = (int)Random.Range(119f, 120.9f);
                    if (subType=="TreePalm") for(int BanCoc = Random.Range(1, 3); BanCoc > 0; BanCoc--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables = GS.itemCache[pickfrut].startVariables;
                    }
                    break;
                case "TreeBig":
                    for(int Wood = Random.Range(3, 9); Wood > 0; Wood--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        Wooddrop.GetComponent<ItemScript>().Variables = "id140;va0;sq1;";
                    }
                    break;
                case "TreeDead":
                    for(int Wood = Random.Range(-2, 2); Wood > 0; Wood--){
                        GameObject Wooddrop = Instantiate(ItemPrefab) as GameObject;
                        Wooddrop.transform.position = this.transform.position + this.transform.forward*Random.Range(1f, 5f);
                        if(Random.Range(0f, 1f) > 0.5f) Wooddrop.GetComponent<ItemScript>().Variables = "id140;va0;sq1;";
                        else Wooddrop.GetComponent<ItemScript>().Variables = "id147;va0;sq1;";
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
