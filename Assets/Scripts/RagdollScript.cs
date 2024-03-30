using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScript : MonoBehaviour {

    // MainVariables
    public GameObject DroppedBy;
    public Vector4 DeadPush;
    public GameObject[] BodyParts;
    public string WhatSet;
    public bool DontDelete;
    public float Freeze = 10f;
    bool IsSwimming = false;
    public GameObject EffectPrefab;

	// Use this for initialization
	void Start () {

        if (DroppedBy == null) {
            Destroy(this.gameObject);
        } else if (DroppedBy.GetComponent<MobScript>() != null) {
            if (DroppedBy.GetComponent<MobScript>().AnimationSet == "HumanoidMelee" || DroppedBy.GetComponent<MobScript>().AnimationSet == "HumanoidPistol" || DroppedBy.GetComponent<MobScript>().AnimationSet == "HumanoidGun" || DroppedBy.GetComponent<MobScript>().AnimationSet == "Mutant") {
                WhatSet = "Humanoid";
                BodyParts = DroppedBy.GetComponent<MobScript>().HumanoidBodyParts;
            }
        }

        // Remove previous
        int AmountOfMaxRags = 1 + (5 * QualitySettings.GetQualityLevel());
        if (GameObject.Find("_GameScript").GetComponent<GameScript>().Platform == 2 && AmountOfMaxRags > 2) {
            AmountOfMaxRags = 2;
        }
        int CurrAmountOfRags = GameObject.FindObjectsOfType<RagdollScript>().Length;
        if (CurrAmountOfRags > AmountOfMaxRags) {
            GameObject RagToRemove = null;
            float Futherest = 0f;
            foreach (RagdollScript CheckRag in GameObject.FindObjectsOfType<RagdollScript>()) {
                if (Vector3.Distance(CheckRag.gameObject.transform.position, GameObject.Find("MainCamera").transform.position) > Futherest && CheckRag.gameObject != this.gameObject) {
                    Futherest = Vector3.Distance(CheckRag.gameObject.transform.position, GameObject.Find("MainCamera").transform.position);
                    RagToRemove = CheckRag.gameObject;
                }
            }
            if (RagToRemove != null) {
                Destroy(RagToRemove);
            }
        }

        DeadPush = new Vector4(DeadPush.x, this.transform.position.y + Random.Range(-0.25f, 0.25f), DeadPush.z, DeadPush.w);
        if (WhatSet == "Humanoid") {
            BodyParts[0].transform.SetParent(this.transform);
            BodyParts[1].transform.SetParent(BodyParts[0].transform);
            BodyParts[2].transform.SetParent(BodyParts[0].transform);
            BodyParts[3].transform.SetParent(BodyParts[2].transform);
            BodyParts[4].transform.SetParent(BodyParts[0].transform);
            BodyParts[5].transform.SetParent(BodyParts[4].transform);
            BodyParts[6].transform.SetParent(BodyParts[0].transform);
            BodyParts[7].transform.SetParent(BodyParts[6].transform);
            BodyParts[8].transform.SetParent(BodyParts[0].transform);
            BodyParts[9].transform.SetParent(BodyParts[8].transform);
            foreach (GameObject GetPart in BodyParts) {

                //GetPart.layer = 13;
                //Rigidbody NewRig = GetPart.AddComponent<Rigidbody>();
                //NewRig.constraints = RigidbodyConstraints.FreezePosition;
                //GetPart.AddComponent<BoxCollider>();
                //GetPart.GetComponent<BoxCollider>().size = Vector3.one / 4f;

                //GetPart.layer = 13;
                //Rigidbody NewRig = GetPart.AddComponent<Rigidbody>();
                //NewRig.constraints = RigidbodyConstraints.FreezePosition;
                //BoxCollider Col = GetPart.AddComponent<BoxCollider>();
                //Col.size = new Vector3(0.1f, 0.1f, 0.1f);

                //GetPart.layer = 13;
                //GetPart.AddComponent<Rigidbody>();
                //SpringJoint Jointt = GetPart.AddComponent<SpringJoint>();
                //Jointt.anchor = GetPart.transform.position;
                //Jointt.connectedBody = GetPart.transform.parent.GetComponent<Rigidbody>();
                //Jointt.transform.SetParent(this.transform);

                //GetPart.layer = 13;
                //GetPart.AddComponent<Rigidbody>();
                //FixedJoint Jointt = GetPart.AddComponent<FixedJoint>();
                //Jointt.connectedBody = GetPart.transform.parent.GetComponent<Rigidbody>();
                //Jointt.transform.SetParent(this.transform);

                //GetPart.layer = 13;
                //BoxCollider Col = GetPart.AddComponent<BoxCollider>();
                //Col.size = Vector3.one / 4f;
                //GetPart.AddComponent<Rigidbody>();
                //SpringJoint Jointt = GetPart.AddComponent<SpringJoint>();
                //Jointt.connectedBody = GetPart.transform.parent.GetComponent<Rigidbody>();
                //Jointt.transform.SetParent(this.transform);

                GetPart.layer = 13;
                foreach (Transform GetMesh in GetPart.transform) {
                    if (GetMesh.GetComponent<MeshRenderer>() != null && GetMesh.name != "Plunger") {
                        GetMesh.gameObject.SetActive(true);
                    }
                }

                if (GameObject.Find("_GameScript").GetComponent<GameScript>().Ragdolls == true) {

                    if (GetPart.GetComponent<BoxCollider>() != null) {
                        GetPart.GetComponent<BoxCollider>().isTrigger = false;
                        GetPart.GetComponent<BoxCollider>().size = new Vector3(GetPart.GetComponent<BoxCollider>().size.x, GetPart.GetComponent<BoxCollider>().size.y /2f, GetPart.GetComponent<BoxCollider>().size.z);
                    } else if (GetPart.GetComponent<CapsuleCollider>() != null) {
                        GetPart.GetComponent<CapsuleCollider>().isTrigger = false;
                        GetPart.GetComponent<CapsuleCollider>().height /= 2f;
                    }
                    Rigidbody Rig = GetPart.AddComponent<Rigidbody>();
                    if(Random.Range(0f, 1f) > 2f) Rig.velocity = this.GetComponent<Rigidbody>().velocity;
                    else Rig.velocity = ( Rig.transform.position - new Vector3(DeadPush.x, DeadPush.y, DeadPush.z) ) * Mathf.Clamp(DeadPush.w, 0f, 25f);
                    Rig.angularDrag = 0f;
                    Rig.angularVelocity = new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f));
                    CharacterJoint Jointt = GetPart.AddComponent<CharacterJoint>();
                    Jointt.connectedBody = GetPart.transform.parent.GetComponent<Rigidbody>();
                    Jointt.swingAxis = new Vector3(1f, 0f, 0f);
                    if (GetPart.name == "LBArm" || GetPart.name == "RBArm") {
                        Jointt.axis = new Vector3(1f, 0f, 0f);
                    } else {
                        Jointt.axis = new Vector3(-1f, 0f, 0f);
                    }
                    SoftJointLimit SL1 = new SoftJointLimit();
                    SoftJointLimit SL2 = new SoftJointLimit();
                    SoftJointLimit HTL = new SoftJointLimit();
                    if (GetPart.name == "LBArm" || GetPart.name == "RBArm" || GetPart.name == "LBLeg" || GetPart.name == "RBLeg") {
                        SL1.limit = 0f;
                        SL2.limit = 0f;
                        HTL.limit = 90f;
                    } else {
                        SL1.limit = 30f;
                        SL2.limit = 30f;
                        HTL.limit = 90f;
                    }
                    Jointt.swing1Limit = SL1;
                    Jointt.swing2Limit = SL2;
                    Jointt.highTwistLimit = HTL;
                    
                    Jointt.transform.SetParent(this.transform);

                }

            }
        }

        // Check for water
        Ray CheckUp = new Ray(this.transform.position - Vector3.up, Vector3.up);
        foreach (RaycastHit HitCheck in Physics.RaycastAll(CheckUp, Mathf.Infinity, GameObject.Find("_GameScript").GetComponent<GameScript>().IngoreMaskWP)) {
            if (HitCheck.collider.gameObject.layer == 14 || HitCheck.collider.gameObject.layer == 16) {
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                this.GetComponent<Rigidbody>().drag = 0f;
                foreach (GameObject FreezeLimb in BodyParts) {
                    if (FreezeLimb.GetComponent<Rigidbody>() != null) {
                        FreezeLimb.GetComponent<Rigidbody>().useGravity = false;
                        FreezeLimb.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    }
                }
                Freeze = 3000f;
                IsSwimming = true;
            }
        }

        if (GameObject.Find("_GameScript").GetComponent<GameScript>().Ragdolls == false) {
            Destroy(this.GetComponent<Rigidbody>());
        }
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Freeze > 0f) {
            Freeze -= 0.02f * (Time.deltaTime * 50f);
            // Check for water
            if (IsSwimming == false) {
                Ray CheckUp = new Ray(this.transform.position, Vector3.up);
                foreach (RaycastHit HitCheck in Physics.RaycastAll(CheckUp, Mathf.Infinity, GameObject.Find("_GameScript").GetComponent<GameScript>().IngoreMaskWP)) {
                    if (HitCheck.collider.gameObject.layer == 14 || HitCheck.collider.gameObject.layer == 16) {
                        this.GetComponent<Rigidbody>().useGravity = false;
                        this.GetComponent<Rigidbody>().drag = 0f;
                        foreach (GameObject FreezeLimb in BodyParts) {
                            if (FreezeLimb.GetComponent<Rigidbody>() != null) {
                                FreezeLimb.GetComponent<Rigidbody>().useGravity = false;
                                FreezeLimb.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                            }
                        }
                        Freeze = 3000f;
                        IsSwimming = true;
                        GameObject Splash = Instantiate(EffectPrefab) as GameObject;
                        Splash.transform.position = this.transform.position;
                        Splash.GetComponent<EffectScript>().EffectName = "BullethitWater";
                    }
                }
            } else {
                this.GetComponent<Rigidbody>().velocity = new Vector3(0f, -5f, 0f);
            }
        } else if (Freeze > -10f) {
            Freeze = -20f;
            foreach (GameObject FreezeLimb in BodyParts) {
                if (FreezeLimb.GetComponent<Rigidbody>() != null) {
                    Destroy(FreezeLimb.GetComponent<CharacterJoint>());
                    Destroy(FreezeLimb.GetComponent<Rigidbody>());
                }
                if (FreezeLimb.GetComponent<BoxCollider>() != null) {
                    Destroy(FreezeLimb.GetComponent<BoxCollider>());
                } else if (FreezeLimb.GetComponent<CapsuleCollider>() != null) {
                    Destroy(FreezeLimb.GetComponent<CapsuleCollider>());
                }
            }
            Destroy(this.GetComponent<Rigidbody>());
            this.enabled = false;
        }
		
	}
}
