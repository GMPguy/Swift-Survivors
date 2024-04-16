using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ItemScript : MonoBehaviour {

    // Variables
    public string Variables;
    public string Name = "";
    public int State = 0; // 0 Unfrozen   1 Frozen   2 Thrown
    public Vector3 ThrownVariables;
    public Vector3 ThrownDirection;
    public bool CanBeFixed = false;
    public bool CanHaveAttachments = false;
    public bool InWater = false;
    // Variables

    // References
    public GameScript GS;
    public GameObject EffectPrefab;
    public GameObject SelectedMesh;
    public Collider MainCollider;
    public GameObject HitDetector;
    public GameObject Bubbles;
    public GameObject DroppedBy;
    public GameObject SpecialPrefab;
    public GameObject AttackPrefab;
    // References

    // Misc
    Vector3 HackAt;
    float CheckForWater = 1f;
    // Misc

	// Use this for initialization
	void Start () {

        if(Variables == "") Variables = "id1;";

        if(GameObject.Find("_GameScript")) GS = GameObject.Find("_GameScript").GetComponent<GameScript>();

        HitDetector.transform.position = this.transform.position;

        ThrownVariables = GS.itemCache[int.Parse(GS.GetSemiClass(Variables, "id"))].ThrowVariables;

        // Check if in water
        Ray CheckWaterUP = new Ray(this.transform.position, Vector3.up);
        foreach (RaycastHit CheckWaterUPHIT in Physics.RaycastAll(CheckWaterUP, Mathf.Infinity)) {
            if (CheckWaterUPHIT.collider.gameObject.layer == 4 || CheckWaterUPHIT.collider.gameObject.layer == 16) {
                InWater = true;
            }
        }

        if (State == 0) {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            this.GetComponent<Rigidbody>().isKinematic = false;
        } else if (State == 1) {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            this.GetComponent<Rigidbody>().isKinematic = true;
        } else if (State == 2) {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            this.GetComponent<Rigidbody>().isKinematic = false;
            GameObject Swing = Instantiate(EffectPrefab) as GameObject;
            Swing.transform.position = this.transform.position;
            Swing.GetComponent<EffectScript>().EffectName = "Swing";
            if (DroppedBy != null && GS.GetSemiClass(Variables, "id") == "54") {
                Ray CheckForHack = new Ray(DroppedBy.transform.position - Vector3.up * 0.9f, Vector3.down);
                RaycastHit CheckFoHackHIT;
                if (Physics.Raycast(CheckForHack, out CheckFoHackHIT, 2f)) {
                    HackAt = CheckFoHackHIT.point;
                }
            }
            if(ThrownVariables.z <= 0f) this.GetComponent<Rigidbody>().angularVelocity = new Vector3( Random.Range(-30f,30f), Random.Range(-30f,30f), Random.Range(-30f,30f) );
        }

        string ID = GS.GetSemiClass(Variables, "id");
        if(ID == "148" || ID == "149" || ID == "150") ID = "Toolbox";

        foreach (Transform GetMesh in this.transform) {
            if (GetMesh.name == ID) {
                GetMesh.gameObject.SetActive(true);
                SelectedMesh = GetMesh.gameObject;

                if(SelectedMesh.GetComponent<MeshFilter>() && State != 1){
                    MainCollider = this.GetComponent<MeshCollider>();
                    this.GetComponent<MeshCollider>().sharedMesh = SelectedMesh.GetComponent<MeshFilter>().sharedMesh;
                    this.GetComponent<BoxCollider>().enabled = false;
                } else {
                    MainCollider = this.GetComponent<BoxCollider>();
                    this.GetComponent<MeshCollider>().enabled = false;
                }

                if (State != 1) SelectedMesh.transform.localPosition = SelectedMesh.transform.localEulerAngles = Vector3.zero;

                if (GS.GetSemiClass(Variables, "id") == "133" && InWater == false && State == 2) {
                    GetMesh.transform.GetChild(0).gameObject.SetActive(true);
                }

                if (GetMesh.GetComponent<MeshRenderer>() != null) {
                    foreach (Material GetMat in GetMesh.GetComponent<MeshRenderer>().materials) {
                        if (GetMat.name == "LASER (Instance)" && DroppedBy != null && DroppedBy == GameObject.FindGameObjectWithTag("Player")) {
                            GetMat.color = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().LaserColor;
                        } else if (GetMat.name == "Glowstick2 (Instance)" || GetMat.name == "Flare2 (Instance)") {
                            GetMat.color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(Variables, "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                        }
                    }
                    if (GS.GetSemiClass(Variables, "id") == "13") {
                        GetMesh.transform.GetChild(1).GetComponent<Light>().color = Color.HSVToRGB(float.Parse(GS.GetSemiClass(Variables, "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f);
                        ParticleSystem.MainModule SetMesh = GetMesh.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().main;
                        SetMesh.startColor = new ParticleSystem.MinMaxGradient(Color.HSVToRGB(float.Parse(GS.GetSemiClass(Variables, "cl"), CultureInfo.InvariantCulture) / 10f, 1f, 1f));
                    }
                }

            } else if (GetMesh.name != "Bubbles" && GetMesh.name != "HitDetector" && GetMesh.name != "Interactions") {
                Destroy(GetMesh.gameObject);
            }
        }

        Name = GS.itemCache[int.Parse(GS.GetSemiClass(Variables, "id"))].getName();
        if(GS.ExistSemiClass(Variables, "sq") && GS.GetSemiClass(Variables, "sq") != "1") Name += " x" + GS.GetSemiClass(Variables, "sq");
        if(GS.ExistSemiClass(Variables, "rep")) CanBeFixed = true;
        if(GS.ExistSemiClass(Variables, "at")) CanHaveAttachments = true;

        if (InWater == true) {
            ThrownVariables[0] /= 2f;
        }

        if (State == 2){
            this.GetComponent<Rigidbody>().velocity = ThrownDirection * ThrownVariables.x;
        }

        if (CanHaveAttachments == true) {
            foreach (Transform Attachment in SelectedMesh.transform.GetChild(0)) {
                if (Attachment.name == GS.GetSemiClass(Variables, "at")) {
                    Attachment.gameObject.SetActive(true);
                    if (Attachment.GetComponent<MeshRenderer>() != null) {
                        if (DroppedBy != null && DroppedBy == GameObject.FindGameObjectWithTag("Player")) {
                            foreach (Material GetMat in Attachment.GetComponent<MeshRenderer>().materials) {
                                if (GetMat.name == "LASER (Instance)") {
                                    GetMat.color = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().LaserColor;
                                }
                            }
                        }
                    }
                } else {
                    Attachment.gameObject.SetActive(false);
                }
            }
        }

    }

    void Update() {

        // Stagnation
        bool Stagnate = true;

        // Water Stuff
        if (CheckForWater > 0f) {
            CheckForWater -= 0.02f * (Time.deltaTime * 50f);
        } else {
            CheckForWater = 1f;
            Ray CheckWaterUP = new Ray(this.transform.position - Vector3.up * 0.1f, Vector3.up);
            foreach (RaycastHit CheckWaterUPHIT in Physics.RaycastAll(CheckWaterUP, Mathf.Infinity)) {
                if (CheckWaterUPHIT.collider.gameObject.layer == 4 || CheckWaterUPHIT.collider.gameObject.layer == 16) {
                    InWater = true;
                }
            } 
        }

        if (InWater == true) {
            Stagnate = false;
            if (Bubbles.GetComponent<ParticleSystem>().isPlaying == false) {
                Bubbles.GetComponent<ParticleSystem>().Play();
            }
            if (this.GetComponent<Rigidbody>().drag != 2f) {
                this.GetComponent<Rigidbody>().drag = 2f;
            }
            ParticleSystem.MainModule SetCol = Bubbles.GetComponent<ParticleSystem>().main;
            SetCol.startColor = RenderSettings.fogColor;
        } else if (InWater == false) {
            if (Bubbles.GetComponent<ParticleSystem>().isPlaying == true) {
                Bubbles.GetComponent<ParticleSystem>().Stop();
            }
            if (this.GetComponent<Rigidbody>().drag != 0f) {
                this.GetComponent<Rigidbody>().drag = 0f;
            }
        }

        if (GS.GetSemiClass(Variables, "id") == "13") {
            Stagnate = false;
            Variables = GS.SetSemiClass(Variables, "va", "/+-" + (0.01f * (Time.deltaTime * 100f)).ToString(CultureInfo.InvariantCulture) ); //Variables.y -= 0.01f * (Time.deltaTime * 100f);
            if (float.Parse(GS.GetSemiClass(Variables, "va"), CultureInfo.InvariantCulture) <= 0f) {
                Destroy(this.gameObject);
            }
            if (InWater == true) {
                Destroy(this.gameObject);
            }
        } else if ((GS.GetSemiClass(Variables, "id") == "66" || GS.GetSemiClass(Variables, "id") == "110" || GS.GetSemiClass(Variables, "id") == "131") && float.Parse(GS.GetSemiClass(Variables, "va"), CultureInfo.InvariantCulture) > 0f) {
            Stagnate = false;
            Variables = GS.SetSemiClass(Variables, "va", "/+" + (0.2f * (Time.deltaTime * 100f)).ToString(CultureInfo.InvariantCulture) );//Variables.y += 0.2f * (Time.deltaTime * 100f);
            if (float.Parse(GS.GetSemiClass(Variables, "va"), CultureInfo.InvariantCulture) > 100f) {
                if (GS.GetSemiClass(Variables, "id") == "66" || GS.GetSemiClass(Variables, "id") == "110" || GS.GetSemiClass(Variables, "id") == "131") {
                    GameObject Boom = Instantiate(SpecialPrefab) as GameObject;
                    Boom.transform.position = this.transform.position;
                    if (GS.GetSemiClass(Variables, "id") == "131") {
                        Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Flashbang";
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 50f;
                    } else if (GS.GetSemiClass(Variables, "id") != "110") {
                        Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                    } else if (GS.GetSemiClass(Variables, "id") == "110") {
                        Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 2f;
                    }
                    Boom.GetComponent<SpecialScript>().CausedBy = DroppedBy;
                    if (GS.GetSemiClass(Variables, "id") == "110") {
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 3f;
                        for (int shootFrag = 32; shootFrag > 0; shootFrag --) {
                            string[] Ids = new string[]{"Luger", "Revolver", "Garand", "M4", "Musket", "AK-47"};
                            Bubbles.transform.parent = null;
                            Bubbles.GetComponent<ParticleSystem>().Stop();
                            int Yturn = shootFrag - ((int)(shootFrag / 6) * 6);
                            int Xturn = (int)(shootFrag / 6);
                            this.transform.eulerAngles = new Vector3(Xturn * Random.Range(0f, -10f), Yturn * Random.Range(0f, 60f), 0f);
                            GameObject.Find("_RoundScript").GetComponent<RoundScript>().Attack(new string[]{Ids[(int)Random.Range(0f, 5.9f)], "CanHurtSelf", "Power100;"}, this.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0.25f, 1f), Random.Range(-1f, 1f)), this.transform.forward, DroppedBy, Bubbles, Bubbles);
                        }
                    }
                    Destroy(this.gameObject);
                    //Variables = "id1;";
                }
            }
        }

        if (CanHaveAttachments == true && GS.GetSemiClass(Variables, "at") != "") {
            foreach (Transform Attachment in SelectedMesh.transform.GetChild(0)) {
                if (Attachment.name == GS.GetSemiClass(Variables, "at")) {
                    Attachment.gameObject.SetActive(true);
                } else {
                    Attachment.gameObject.SetActive(false);
                }
            }
        }

        if (State == 2) {
            Stagnate = false;
            HitDetector.SetActive(true);
            HitDetector.transform.parent = null;
            MainCollider.enabled = false;
            //this.transform.position += (ThrownDirection * (ThrownVariables.x / 100f)) * (Time.deltaTime * 100f);
            /*if (GS.GetSemiClass(Variables, "id") != "993" && GS.GetSemiClass(Variables, "id") != "134") {
                this.transform.Rotate(new Vector3(0.25f, 0.25f, 0.25f) * ThrownVariables.x * (Time.deltaTime * 100f));
            }*/
            // Check For Hit
            if (HitDetector.transform.position != this.transform.position) {
                HitDetector.transform.LookAt(this.transform.position);
                if (GS.GetSemiClass(Variables, "id") == "993" || GS.GetSemiClass(Variables, "id") == "134") {
                    this.transform.forward = HitDetector.transform.forward * 1000f;
                }
                Ray CheckObstacle = new Ray(HitDetector.transform.position, HitDetector.transform.forward);
                RaycastHit CheckObstacleHIT;
                if (Physics.Raycast(CheckObstacle, out CheckObstacleHIT, Vector3.Distance(HitDetector.transform.position, this.transform.position), GS.GetComponent<GameScript>().IgnoreMaks1)) {
                    if (DroppedBy == null || CheckObstacleHIT.collider.gameObject != DroppedBy) {
                        float ChanceOfDestruction = Random.Range(0f, 100f);
                        State = 0;
                        HitDetector.SetActive(false);
                        HitDetector.transform.parent = this.transform;
                        MainCollider.enabled = true;
                        this.transform.position = CheckObstacleHIT.point - (HitDetector.transform.forward * 0.25f);
                        if (CheckObstacleHIT.collider.gameObject.layer == 4 || CheckObstacleHIT.collider.gameObject.layer == 16) {
                            InWater = true;
                        } else if (GS.GetSemiClass(Variables, "id") == "133") {
                            // Molotow
                            GameObject Boom = Instantiate(SpecialPrefab) as GameObject;
                            Boom.transform.position = this.transform.position;
                            Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Molotow";
                            Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                            Boom.GetComponent<SpecialScript>().CausedBy = DroppedBy;
                            Destroy(this.gameObject);
                        } else if (GS.GetSemiClass(Variables, "id") == "136") {
                            // Frying pan
                            GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                            DropEffect.GetComponent<EffectScript>().EffectName = "FryingPan";
                            DropEffect.transform.position = this.transform.position;
                        }
                        if (CheckObstacleHIT.collider.GetComponent<MobScript>() != null) {
                            if (GS.GetSemiClass(Variables, "id") == "108") {
                                // Plunger
                                CheckObstacleHIT.collider.GetComponent<MobScript>().React("Blinded", 5f, this.transform.position);
                                CheckObstacleHIT.collider.GetComponent<MobScript>().Plunged = true;
                                foreach (GameObject Plunger in CheckObstacleHIT.collider.GetComponent<MobScript>().Plungers) {
                                    Plunger.SetActive(true);
                                }
                                Destroy(this.gameObject);
                            } else if (GS.GetSemiClass(Variables, "id") == "136"){
                                // frying pan
                                int Chance = Random.Range(0, 100);
                                if (Chance < 25) {
                                    CheckObstacleHIT.collider.GetComponent<MobScript>().React("Blinded", 2f, this.transform.position);
                                }
                            } else {
                                if (GS.GetSemiClass(Variables, "id") == "13") {
                                    // Flare
                                    CheckObstacleHIT.collider.GetComponent<MobScript>().Fire = 10f;
                                    if (DroppedBy != null) {
                                        CheckObstacleHIT.collider.GetComponent<MobScript>().Hurt(1f, DroppedBy, true, this.transform.position, "Flare");
                                    }
                                }
                                CheckObstacleHIT.collider.GetComponent<MobScript>().Hurt(ThrownVariables.z, DroppedBy, true, this.transform.position, "Item");
                            }
                        }
                        if (DroppedBy != null && GS.GetSemiClass(Variables, "id") == "992") {
                            DroppedBy.transform.position = this.transform.position + (Vector3.up * 1f);
                            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(75, 200, 75, 255), new float[]{0.5f, 0.5f});
                        } else if (GS.GetSemiClass(Variables, "id") == "93") {
                            GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                            Ring.transform.position = this.transform.position;
                            Ring.GetComponent<EffectScript>().EffectName = "Cowbell";
                        }
                        if (CanHaveAttachments == true && GS.GetSemiClass(Variables, "at") != "" && GS.GetSemiClass(Variables, "at") != "0") {
                            GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                            DropEffect.GetComponent<EffectScript>().EffectName = "Unpin";
                            DropEffect.transform.position = this.transform.position;
                            DropEffect.transform.LookAt(Vector3.up);
                            GameObject Attachment = Instantiate(GameObject.Find("_RoundScript").GetComponent<RoundScript>().ItemPrefab) as GameObject;
                            Attachment.GetComponent<ItemScript>().Variables = GS.itemCache[int.Parse(GS.GetSemiClass(Variables, "at"))].startVariables;
                            Attachment.transform.position = this.transform.position;
                            Variables = GS.RemoveSemiClass(Variables, "at"); //Variables = new Vector3(Variables.x, Variables.y, 0f);
                            setAtt();
                        } else {
                            if (ChanceOfDestruction <= ThrownVariables.y) {
                                Destroy(this.gameObject);
                                GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                                DropEffect.GetComponent<EffectScript>().EffectName = "ItemBreak";
                                DropEffect.transform.position = this.transform.position;
                                DropEffect.transform.LookAt(Vector3.up);
                            } else if (CheckObstacleHIT.collider.gameObject.layer == 4 || CheckObstacleHIT.collider.gameObject.layer == 16) {
                                GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                                DropEffect.GetComponent<EffectScript>().EffectName = "BullethitWater";
                                DropEffect.transform.position = this.transform.position;
                                DropEffect.transform.LookAt(Vector3.up);
                            } else {
                                GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                                DropEffect.GetComponent<EffectScript>().EffectName = "BullethitBlock";
                                DropEffect.transform.position = this.transform.position;
                                DropEffect.transform.LookAt(Vector3.up);
                            }
                        }
                    
                        foreach (GameObject MobHear in GameObject.FindGameObjectsWithTag("Mob")) {
                            if (Vector3.Distance(this.transform.position, MobHear.transform.position) < ThrownVariables.x * 3f) {
                                if ((GS.GetSemiClass(Variables, "id") == "66" || GS.GetSemiClass(Variables, "id") == "110" || GS.GetSemiClass(Variables, "id") == "131") && Vector3.Distance(this.transform.position, MobHear.transform.position) < 9f && (MobHear.GetComponent<MobScript>().ClassOfMob != "Mutant")) {
                                    MobHear.GetComponent<MobScript>().React("Panic", ((100f - float.Parse(GS.GetSemiClass(Variables, "va"), CultureInfo.InvariantCulture)) / 20f) + 1f, this.transform.position + (MobHear.transform.position - this.transform.position) * 9f);
                                } else if (GS.GetSemiClass(Variables, "id") != "66" && GS.GetSemiClass(Variables, "id") != "110" && GS.GetSemiClass(Variables, "id") != "131" && MobHear.GetComponent<MobScript>().Angered <= 0f) {
                                    MobHear.GetComponent<MobScript>().React("Curious", 10f, this.transform.position);
                                }
                            }
                        }
                        if (GS.GetSemiClass(Variables, "id") == "54" && HackAt != Vector3.zero) {
                            SelectedMesh.transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
                            State = 1;
                            this.GetComponent<Rigidbody>().useGravity = false;
                            this.GetComponent<Rigidbody>().isKinematic = true;
                        }
                    }
                }
                HitDetector.transform.position = this.transform.position - (HitDetector.transform.forward / 10f);
            }
            // Check For Hit
            // Grappling Hook
            if (GS.GetSemiClass(Variables, "id") == "54" && HackAt != Vector3.zero) {
                SelectedMesh.transform.GetChild(1).LookAt(HackAt);
                SelectedMesh.transform.GetChild(1).localScale = new Vector3(1f, 1f , Vector3.Distance(this.transform.position, HackAt));
            }
        }

        if (Stagnate)
            this.enabled = false;

    }

    public void setAtt(){
        foreach (Transform AttachmentMesh in SelectedMesh.transform.GetChild(0)) AttachmentMesh.gameObject.SetActive(false);
    }

    void OnDestroy() {

        Destroy(HitDetector);

    }

    public string ReceiveName() {

        string ItemNameA = "";



        return ItemNameA;

    }

}
