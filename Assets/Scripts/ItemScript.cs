using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine;

public class ItemScript : MonoBehaviour {

    // Variables
    public JClass Variables;
    public string Name = "";
    public int State = 0; // 0 Unfrozen   1 Frozen   2 Thrown
    public Vector3 ThrownVariables;
    public Vector3 ThrownDirection;
    public bool CanBeFixed = false;
    public bool CanHaveAttachments = false;
    public bool InWater = false;
    public string PickupReward = "ItemsFound_";
    // Variables

    // References
    public GameScript GS;
    public RoundScript RS;
    public GameObject EffectPrefab;
    public GameObject SelectedMesh;
    public Collider MainCollider;
    public GameObject HitDetector;
    public GameObject Bubbles;
    public GameObject DroppedBy;
    public GameObject SpecialPrefab;
    public GameObject AttackPrefab;
    public SpriteRenderer MinimapMarker;
    // References

    // Misc
    Vector3 HackAt;
    float CheckForWater = 1f;
    // Misc

	// Use this for initialization
	void Start () {

        if(Variables == null) 
            Variables = new JClass(new JEntry[]{
                new JInt(JType.ID, 1)
            });

        if(GameObject.Find("_RoundScript")) {
            RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
            GS = RS.GS;
        }

        HitDetector.transform.position = this.transform.position;

        ThrownVariables = GS.ItemCache[Variables.GetInt(JType.ID)].ThrowVariables;

        if (Variables.GetInt(JType.ID) >= 990)
            PickupReward = "TreasuresFound_";

        // Flare marker
        if (Variables.GetInt(JType.ID) == 13) {
            MinimapMarker.transform.parent.GetComponent<MinimapMarker>().MapSize = MinimapMarker.transform.parent.GetComponent<MinimapMarker>().MinimapSize;
            MinimapMarker.color = Color.HSVToRGB(Variables.GetFloat(JType.Color) / 10f, 1f, 1f);
        }

        // Check if in water
        Ray CheckWaterUP = new Ray(this.transform.position, Vector3.up);
        foreach (RaycastHit CheckWaterUPHIT in Physics.RaycastAll(CheckWaterUP, Mathf.Infinity)) {
            if (CheckWaterUPHIT.collider.gameObject.layer == 4 || CheckWaterUPHIT.collider.gameObject.layer == 16) {
                InWater = true;
                MinimapMarker.color = new (0f, .5f, 1f, Random.Range(0f, .25f));
                PickupReward = "ItemsUnderwaterFound_";
            }
        }

        if (State == 0) {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            this.GetComponent<Rigidbody>().isKinematic = false;
            MinimapMarker.transform.parent.GetComponent<MinimapMarker>().UpdateRotation = true;
        } else if (State == 1) {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            this.GetComponent<Rigidbody>().isKinematic = true;
        } else if (State == 2) {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            this.GetComponent<Rigidbody>().isKinematic = false;
            MinimapMarker.transform.parent.GetComponent<MinimapMarker>().UpdateRotation = true;
            GameObject Swing = Instantiate(EffectPrefab) as GameObject;
            Swing.transform.position = this.transform.position;
            Swing.GetComponent<EffectScript>().EffectName = "Swing";
            if (DroppedBy != null && Variables.GetInt(JType.ID) == 54) {
                Ray CheckForHack = new Ray(DroppedBy.transform.position - Vector3.up * 0.9f, Vector3.down);
                RaycastHit CheckFoHackHIT;
                if (Physics.Raycast(CheckForHack, out CheckFoHackHIT, 2f)) {
                    HackAt = CheckFoHackHIT.point;
                }
            }
            if(ThrownVariables.z <= 0f) this.GetComponent<Rigidbody>().angularVelocity = new Vector3( Random.Range(-30f,30f), Random.Range(-30f,30f), Random.Range(-30f,30f) );
        }

        string ID = Variables.GetInt(JType.ID).ToString();
        string meshID = ID;
        if(ID == "148" || ID == "149" || ID == "150" || ID == "151") 
            meshID = "Toolbox";

        SelectedMesh = RS.GetItemModel(meshID);
        SelectedMesh.transform.SetParent(this.transform);

        if(SelectedMesh.GetComponent<MeshFilter>() && State != 1){
            MainCollider = this.GetComponent<MeshCollider>();
            this.GetComponent<MeshCollider>().sharedMesh = SelectedMesh.GetComponent<MeshFilter>().sharedMesh;
            this.GetComponent<BoxCollider>().enabled = false;
        } else {
            MainCollider = this.GetComponent<BoxCollider>();
            this.GetComponent<MeshCollider>().enabled = false;
        }

        if (State != 1) 
            SelectedMesh.transform.localPosition = SelectedMesh.transform.localEulerAngles = Vector3.zero;
        else {
            SelectedMesh.transform.localPosition = SelectedMesh.transform.position;
            SelectedMesh.transform.localEulerAngles = SelectedMesh.transform.eulerAngles;
        }

        if (meshID == "133" && InWater == false && State == 2) {
            SelectedMesh.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (SelectedMesh.GetComponent<MeshRenderer>() != null) {
            foreach (Material GetMat in SelectedMesh.GetComponent<MeshRenderer>().materials) {
                if (GetMat.name == "LASER (Instance)" && DroppedBy != null && DroppedBy == GameObject.FindGameObjectWithTag("Player")) {
                    GetMat.color = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().LaserColor;
                } else if (GetMat.name == "Glowstick2 (Instance)" || GetMat.name == "Flare2 (Instance)") {
                    GetMat.color = Color.HSVToRGB(Variables.GetFloat(JType.Color) / 10f, 1f, 1f);
                }
            }
            if (meshID == "13") {
                SelectedMesh.transform.GetChild(1).GetComponent<Light>().color = Color.HSVToRGB(Variables.GetFloat(JType.Color) / 10f, 1f, 1f);
                ParticleSystem.MainModule SetMesh = SelectedMesh.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().main;
                SetMesh.startColor = new ParticleSystem.MinMaxGradient(Color.HSVToRGB(Variables.GetFloat(JType.Color) / 10f, 1f, 1f));
            }
        }


        Name = GS.ItemCache[int.Parse(ID)].getName();
        if(Variables.Exists(JType.StackQuantity) && Variables.GetInt(JType.StackQuantity) != 1) 
            Name += " x" + Variables.GetInt(JType.StackQuantity);
        if(Variables.Exists(JType.Repairable)) CanBeFixed = true;
        if(Variables.Exists(JType.Attachment)) CanHaveAttachments = true;

        if (InWater == true) {
            ThrownVariables[0] /= 2f;
        }

        if (State == 2){
            this.GetComponent<Rigidbody>().velocity = ThrownDirection * ThrownVariables.x;
        }

        if (CanHaveAttachments == true) {
            foreach (Transform Attachment in SelectedMesh.transform.GetChild(0)) {
                if (Attachment.name == Variables.GetInt(JType.Attachment).ToString()) {
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

        if (Variables.GetInt(JType.ID) == 13) {
            Stagnate = false;
            //Variables = GS.SetSemiClass(Variables, "va", "/+-" + (0.01f * (Time.deltaTime * 100f)).ToString(CultureInfo.InvariantCulture) ); //Variables.y -= 0.01f * (Time.deltaTime * 100f);
            Variables.SetFloat(JType.VariableA, -(0.01f * (Time.deltaTime * 100f)), Maths.Add);
            if (Variables.GetFloat(JType.VariableA) <= 0f) {
                Destroy(this.gameObject);
            }
            if (InWater == true) {
                Destroy(this.gameObject);
            }
        } else if ((Variables.GetInt(JType.ID) == 66 || Variables.GetInt(JType.ID) == 110 || Variables.GetInt(JType.ID) == 131) && Variables.GetFloat(JType.VariableA) > 0f) {
            Stagnate = false;
            //Variables = GS.SetSemiClass(Variables, "va", "/+" + (0.2f * (Time.deltaTime * 100f)).ToString(CultureInfo.InvariantCulture) );//Variables.y += 0.2f * (Time.deltaTime * 100f);
            Variables.SetFloat(JType.VariableA, 0.2f * (Time.deltaTime * 100f), Maths.Add);
            if (Variables.GetFloat(JType.VariableA) > 100f) {
                if (Variables.GetInt(JType.ID) == 66 || Variables.GetInt(JType.ID) == 110 || Variables.GetInt(JType.ID) == 131) {
                    GameObject Boom = Instantiate(SpecialPrefab) as GameObject;
                    Boom.transform.position = this.transform.position;
                    if (Variables.GetInt(JType.ID) == 131) {
                        Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Flashbang";
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 50f;
                    } else if (Variables.GetInt(JType.ID) != 110) {
                        Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                    } else if (Variables.GetInt(JType.ID) == 110) {
                        Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 2f;
                    }
                    Boom.GetComponent<SpecialScript>().CausedBy = DroppedBy;
                    if (Variables.GetInt(JType.ID) == 110) {
                        Boom.GetComponent<SpecialScript>().ExplosionRange = 3f;
                        for (int shootFrag = 32; shootFrag > 0; shootFrag --) {
                            GameObject.Find("_RoundScript").GetComponent<RoundScript>().FragElements.Add(this.transform.position);
                        }
                    }
                    Destroy(this.gameObject);
                    //Variables = "id1;";
                }
            }
        }

        if (CanHaveAttachments == true && Variables.GetInt(JType.Attachment) != 0) {
            foreach (Transform Attachment in SelectedMesh.transform.GetChild(0)) {
                if (Attachment.name == Variables.GetInt(JType.Attachment).ToString()) {
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

            if (HitDetector.transform.position != this.transform.position) {
                HitDetector.transform.LookAt(this.transform.position);
                if (Variables.GetInt(JType.ID) == 993 || Variables.GetInt(JType.ID) == 134) {
                    this.transform.right = HitDetector.transform.forward * 1000f;
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
                        } else if (Variables.GetInt(JType.ID) == 133) {
                            // Molotow
                            GameObject Boom = Instantiate(SpecialPrefab) as GameObject;
                            Boom.transform.position = this.transform.position;
                            Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Molotow";
                            Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                            Boom.GetComponent<SpecialScript>().CausedBy = DroppedBy;
                            Destroy(this.gameObject);
                        } else if (Variables.GetInt(JType.ID) == 136) {
                            // Frying pan
                            GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                            DropEffect.GetComponent<EffectScript>().EffectName = "FryingPan";
                            DropEffect.transform.position = this.transform.position;
                        }

                        if (CheckObstacleHIT.collider.GetComponent<MobScript>() != null) {
                            if (Variables.GetInt(JType.ID) == 108) {
                                // Plunger
                                CheckObstacleHIT.collider.GetComponent<MobScript>().React("Blinded", 5f, this.transform.position);
                                CheckObstacleHIT.collider.GetComponent<MobScript>().Plunged = true;
                                foreach (GameObject Plunger in CheckObstacleHIT.collider.GetComponent<MobScript>().Plungers) {
                                    Plunger.SetActive(true);
                                }
                                Destroy(this.gameObject);
                            } else if (Variables.GetInt(JType.ID) == 136){
                                // frying pan
                                int Chance = Random.Range(0, 100);
                                if (Chance < 25) {
                                    CheckObstacleHIT.collider.GetComponent<MobScript>().React("Blinded", 2f, this.transform.position);
                                }
                            } else {
                                if (Variables.GetInt(JType.ID) == 13) {
                                    // Flare
                                    CheckObstacleHIT.collider.GetComponent<MobScript>().Fire = 10f;
                                    if (DroppedBy != null) {
                                        CheckObstacleHIT.collider.GetComponent<MobScript>().Hurt(1f, DroppedBy, true, this.transform.position, "Flare");
                                    }
                                }
                                CheckObstacleHIT.collider.GetComponent<MobScript>().Hurt(ThrownVariables.z, DroppedBy, true, this.transform.position, "Item");
                            }
                        }

                        if (DroppedBy != null && Variables.GetInt(JType.ID) == 992) {
                            DroppedBy.transform.position = this.transform.position + (Vector3.up * 1f);
                            GameObject.Find("MainCanvas").GetComponent<CanvasScript>().Flash(new Color32(75, 200, 75, 255), new float[]{0.5f, 0.5f});
                        } else if (Variables.GetInt(JType.ID) == 93) {
                            GameObject Ring = Instantiate(EffectPrefab) as GameObject;
                            Ring.transform.position = this.transform.position;
                            Ring.GetComponent<EffectScript>().EffectName = "Cowbell";
                        }

                        if (CanHaveAttachments == true && Variables.GetInt(JType.Attachment) != 0) {
                            GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                            DropEffect.GetComponent<EffectScript>().EffectName = "Unpin";
                            DropEffect.transform.position = this.transform.position;
                            DropEffect.transform.LookAt(Vector3.up);
                            GameObject Attachment = Instantiate(GameObject.Find("_RoundScript").GetComponent<RoundScript>().ItemPrefab) as GameObject;
                            Attachment.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[Variables.GetInt(JType.Attachment)].startVariables);
                            Attachment.transform.position = this.transform.position;
                            Variables.SetInt(JType.Attachment, 0);
                            setAtt();
                        } else {
                            // Hit
                            if (ChanceOfDestruction <= ThrownVariables.y) {
                                Destroy(this.gameObject);
                                GameObject DropEffect = Instantiate(EffectPrefab) as GameObject;
                                DropEffect.GetComponent<EffectScript>().EffectName = "ItemBreak";
                                DropEffect.transform.position = this.transform.position;
                                DropEffect.transform.LookAt(Vector3.up);

                                switch (Variables.GetInt(JType.ID)) {
                                    case 111: case 139: // Grenade launcher, bazooka
                                        int ammo = (int)Variables.GetFloat(JType.VariableA);

                                        for (int dup = ammo; dup > 0; dup--) {
                                            GameObject BoomA = Instantiate(SpecialPrefab) as GameObject;
                                            float upperLerp = (float)dup / ammo;
                                            BoomA.transform.position = transform.position + new Vector3 (Random.Range(-10f, 10f) * upperLerp, dup * Random.Range(1f, 6f), Random.Range(-10f, 10f) * upperLerp);
                                            BoomA.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                                            BoomA.GetComponent<SpecialScript>().ExplosionRange = 6f;
                                        }
                                        break;
                                    case 67: // Panzerfaust
                                        RS.Attack(new string[]{ "Rocket" }, transform.position + Vector3.up, transform.forward, DroppedBy, gameObject);
                                        break;
                                    case 89: // Blowtorch
                                        GameObject Boom = Instantiate(SpecialPrefab) as GameObject;
                                        Boom.transform.position = transform.position;
                                        Boom.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                                        Boom.GetComponent<SpecialScript>().ExplosionRange = 6f;
                                        break;
                                    case 109: // Flame thrower
                                        GameObject BoomB = Instantiate(SpecialPrefab) as GameObject;
                                        BoomB.transform.position = transform.position;
                                        BoomB.GetComponent<SpecialScript>().TypeOfSpecial = "Explosion";
                                        BoomB.GetComponent<SpecialScript>().ExplosionRange = 6f;

                                        GameObject FlameUp = Instantiate(SpecialPrefab) as GameObject;
                                        FlameUp.transform.position = this.transform.position;
                                        FlameUp.GetComponent<SpecialScript>().TypeOfSpecial = "Molotow";
                                        FlameUp.GetComponent<SpecialScript>().ExplosionRange = 6f;
                                        FlameUp.GetComponent<SpecialScript>().CausedBy = DroppedBy;
                                        break;
                                    case 128: // Fire extinguisher
                                        for (int fe = 0; fe < 10; fe++) {
                                            Vector3 dir = new (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                                            RS.Attack(new string[]{ "FireExtinguisher" }, transform.position, dir, DroppedBy, gameObject);
                                        }
                                        break;
                                    case 168: // Watermelon
                                        int slice = Random.Range(2, 9);

                                        DropEffect.GetComponent<EffectScript>().EffectName = "Gibs";

                                        for (int s = 0; s < slice; s++) {
                                            GameObject item = Instantiate(GameObject.Find("_RoundScript").GetComponent<RoundScript>().ItemPrefab) as GameObject;
                                            item.GetComponent<ItemScript>().Variables.CopyFrom(GS.ItemCache[169].startVariables);
                                            item.transform.position = this.transform.position + Vector3.up * s / 4f;
                                        }
                                        break;
                                }
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
                                if ((Variables.GetInt(JType.ID) == 66 || Variables.GetInt(JType.ID) == 110 || Variables.GetInt(JType.ID) == 131) && Vector3.Distance(this.transform.position, MobHear.transform.position) < 9f && (MobHear.GetComponent<MobScript>().ClassOfMob != "Mutant")) {
                                    MobHear.GetComponent<MobScript>().React("Panic", ((100f - Variables.GetFloat(JType.VariableA)) / 20f) + 1f, this.transform.position + (MobHear.transform.position - this.transform.position) * 9f);
                                } else if (Variables.GetInt(JType.ID) != 66 && Variables.GetInt(JType.ID) != 110 && Variables.GetInt(JType.ID) != 131 && MobHear.GetComponent<MobScript>().Angered <= 0f) {
                                    MobHear.GetComponent<MobScript>().React("Curious", 10f, this.transform.position);
                                }
                            }
                        }
                        if (Variables.GetInt(JType.ID) == 54 && HackAt != Vector3.zero) {
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
            if (Variables.GetInt(JType.ID) == 54 && HackAt != Vector3.zero) {
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
