using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class MobPH : MonoBehaviour {    
    
    // Main variables
    public string spawnType;
    public int Activation = 0; // 0 actiavte near - 1 activated by time - 2 activate near or time - 3 activated, may be unactivated - 4 activated for good - 5 activated, but also do something lol
    string ActivationBonus = "";
    public float TimeTillActivation = 0f;
    float PrevTime = 0f;
    public float DifficultyLevel = 0f;
    public bool Curious = false;
    float setDistance;

    // Misc
    public int SpawnRadius;
    public int SpawnAmount;
    public Transform MainPlayer;
    public GameObject MobPrefab;
    public GameObject[] SubOrdinates;
    public GameObject[] MonumentPointsOfInterest;
    public int DeactivateID, goodtogoDeactivate = 0;
    string prevSpec = "";

    void Start() {

        PrevTime = Time.timeSinceLevelLoad;
        setDistance = Mathf.Clamp(RenderSettings.fogEndDistance, 100f, Mathf.Infinity);

        Vector3 flopin = new Vector3(Random.Range(50f, 250f), 10f, Random.Range(50f, 250f));
        int flipin = (int)Random.Range(0f, 3.9f); switch(flipin) {
            case 1: flopin.x *= -1f; break;
            case 2: flopin.z *= -1f; break;
            case 3: flopin.x *= -1f; flopin.z *= -1f; break;
        }
        this.transform.position = flopin;

        // read suggestion
        prevSpec = spawnType;
        List<string> Lottery = new List<string>();
        switch(spawnType){
            case "Default": case "Urban": case "Wilderness":
                for(int sb = (int)Mathf.Lerp(10f, 1f, DifficultyLevel); sb > 0; sb--) Lottery.Add(Ticket("Survivor"));
                for(int bb = (int)Mathf.Lerp(10f, 5f, DifficultyLevel); bb > 0; bb--) Lottery.Add(Ticket("Bandit"));
                for(int mb = (int)Mathf.Lerp(15f, 25f, DifficultyLevel); mb > 0; mb--) Lottery.Add(Ticket("Mutant"));

                spawnType = Lottery.ToArray()[(int)Random.Range(0f, Lottery.ToArray().Length-0.01f)];
                break;
            case "War":
                for(int sb = (int)Mathf.Lerp(20f, 10f, DifficultyLevel); sb > 0; sb--) Lottery.Add(Ticket("Survivor"));
                for(int bb = (int)Mathf.Lerp(10f, 20f, DifficultyLevel); bb > 0; bb--) Lottery.Add(Ticket("Bandit"));
                for(int mb = (int)Mathf.Lerp(0f, 5f, DifficultyLevel); mb > 0; mb--) Lottery.Add(Ticket("Mutant"));

                spawnType = Lottery.ToArray()[(int)Random.Range(0f, Lottery.ToArray().Length-0.01f)];
                break;
            case "Infested":
                for(int bb = (int)Mathf.Lerp(5f, 0f, DifficultyLevel); bb > 0; bb--) Lottery.Add(Ticket("Bandit"));
                for(int mb = (int)Mathf.Lerp(30f, 60f, DifficultyLevel); mb > 0; mb--) Lottery.Add(Ticket("Mutant"));

                spawnType = Lottery.ToArray()[(int)Random.Range(0f, Lottery.ToArray().Length-0.01f)];
                break;
        }

        // read ticket
        switch(spawnType){
            case "Survivors": case "Bandits": case "BanditAmbush":
                SpawnAmount = (int)Mathf.Clamp(Random.Range(-1f, 4f), 1f, 4f);
                SpawnRadius = 3;
                Activation = 0;
                Curious = true;
                break;
            case "DeadSurvivors": case "DeadBandits":
                SpawnAmount = (int)Mathf.Clamp(Random.Range(-1f, 2f), 1f, 2f);
                SpawnRadius = 10;
                Activation = 2;
                TimeTillActivation = Random.Range(-600f, 600f);
                break;
            case "BanditSurvivorClash":
                SpawnAmount = (int)Random.Range(2f, 8.9f);
                SpawnRadius = 10;
                Activation = 2;
                TimeTillActivation = Random.Range(-60f, 300f);
                Curious = true;
                break;
            case "RaidSurvivors": case "RaidBandits": case "MonumentOverride":
                SpawnAmount = (int)Random.Range(1f, 4.9f);
                SpawnRadius = 10;
                Activation = 2;
                TimeTillActivation = Random.Range(30f, 150f);

                // points of interest
                GameObject FirstGuard = null;
                List<GameObject> newMPOI = new List<GameObject>();
                GameObject[] Fethedmobs = GameObject.FindGameObjectsWithTag("Mob");
                for(int ffg = 0; ffg <Fethedmobs.Length; ffg++) if(Fethedmobs[ffg].GetComponent<MobScript>().TypeOfMob == 8){
                    if (!FirstGuard) { 
                        FirstGuard = Fethedmobs[ffg];
                    } else if (Vector3.Distance(FirstGuard.transform.position, Fethedmobs[ffg].transform.position) < 25f) {
                        newMPOI.Add(Fethedmobs[ffg]);
                    }
                }

                if(FirstGuard){
                    this.transform.position = FirstGuard.transform.position;
                    MonumentPointsOfInterest = newMPOI.ToArray();
                } else {
                    Destroy(this.gameObject);
                }
                break;
            case "Mutants": case "MutantsWave":
                SpawnAmount = (int)Mathf.Clamp(Random.Range(1f, 10f), 1f, math.lerp(3f, 10f, DifficultyLevel));
                SpawnRadius = 25;
                Activation = 0;
                Curious = true;
                break;
            case "MutantBanditClash": case "MutantSurvivorClash":
                SpawnAmount = (int)Random.Range(2f, 10f);
                SpawnRadius = 10;
                Activation = 2;
                TimeTillActivation = Random.Range(-30f, 300f);
                Curious = true;
                break;
            default:
                Debug.LogError("Cannot read ticket " + spawnType);
                break;
        }
        
    }

    public void DoAnUpdate() {

        MainPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        switch(Activation){
            case 0: // if is near
                if(Vector3.Distance(MainPlayer.position, this.transform.position) < SpawnRadius + setDistance) Activate();
                break;
            case 1: // time ran out
                if(TimeTillActivation > 0f){
                    TimeTillActivation -= Time.timeSinceLevelLoad - PrevTime;
                    PrevTime = Time.timeSinceLevelLoad;
                } else {
                    Activate();
                }
                break;
            case 2: // either near or time
                if(Vector3.Distance(MainPlayer.position, this.transform.position) < SpawnRadius + setDistance || TimeTillActivation <= 0f){
                    Activate();
                } else {
                    TimeTillActivation -= Time.timeSinceLevelLoad - PrevTime;
                    PrevTime = Time.timeSinceLevelLoad;
                }
                break;
            case 3: // may be unactivated
                Deactivate();
                break;
            case 4: // activated for good
                Destroy(this.gameObject);
                break;
            case 5: // do something afterwards
                switch(ActivationBonus){
                    case "KillOff":
                        bool subsalive = false;
                        RoundScript RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();
                        for(int killfa = 0; killfa < SubOrdinates.Length; killfa++) if (SubOrdinates[killfa] && SubOrdinates[killfa].GetComponent<MobScript>().MobHealth[0] > 0f) {
                            Transform subtran = SubOrdinates[killfa].transform;
                            subsalive = true;
                            if (Random.Range(0f, 1f) >= 0.8f) {
                                RS.Attack(new string[]{"MosinNagant"}, subtran.position + Vector3.up*1.5f, Vector3.down);
                            } else {
                                RS.Attack(new string[]{"SaphireSpear"}, subtran.position + Vector3.up*1.5f, Vector3.down);
                            }
                        }
                        if (!subsalive) Destroy(this.gameObject);
                        break;
                    case "Chase":
                        if(TimeTillActivation > 0f){
                            TimeTillActivation -= Time.timeSinceLevelLoad - PrevTime;
                            PrevTime = Time.timeSinceLevelLoad;
                        } else {
                            for(int lurefa = 0; lurefa < SubOrdinates.Length; lurefa++) if (SubOrdinates[lurefa]) {
                                SubOrdinates[lurefa].GetComponent<MobScript>().AiTarget = MainPlayer.gameObject;
                                SubOrdinates[lurefa].GetComponent<MobScript>().Angered = 10f;
                            }
                            Destroy(this.gameObject);
                        }
                        break;
                    case "Lure":
                        if(TimeTillActivation > 0f){
                            TimeTillActivation -= Time.timeSinceLevelLoad - PrevTime;
                            PrevTime = Time.timeSinceLevelLoad;
                        } else {
                            for(int lurefa = 0; lurefa < SubOrdinates.Length; lurefa++) if (SubOrdinates[lurefa]) {
                                SubOrdinates[lurefa].GetComponent<MobScript>().React("Curious", 30f, MainPlayer.transform.position + new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f)));
                            }
                            Destroy(this.gameObject);
                        }
                        break;
                }
                break;
        }
        
    }

    void Deactivate(){

        if (SubOrdinates.Length > 0) {
            if(SubOrdinates[DeactivateID] && Vector3.Distance(SubOrdinates[DeactivateID].transform.position, MainPlayer.position) > SpawnRadius + 10f + setDistance) goodtogoDeactivate++;
            else if (!SubOrdinates[DeactivateID]) Destroy(this.gameObject);

            DeactivateID++;
            if(DeactivateID >= SubOrdinates.Length) {
                if(goodtogoDeactivate == SubOrdinates.Length) {
                    this.transform.position = SubOrdinates[0].transform.position;
                    Activation = 0;
                    foreach(GameObject hidsub in SubOrdinates) Destroy(hidsub);
                    SubOrdinates = new GameObject[]{};
                    ResetPosition();
                } 
                DeactivateID = goodtogoDeactivate = 0;
            }
        } else {
            DeactivateID = goodtogoDeactivate = 0;
        }

    }

    void Activate(){

        List<GameObject> addSubs = new List<GameObject>();
        switch(spawnType){
            case "Survivors": case "Bandits": case "BanditAmbush": case "RaidSurvivors": case "RaidBandits": case "MonumentOverride":
                int squadA = Random.Range(1, 9999);
                for(int spon = SpawnAmount; spon > 0; spon--){
                    GameObject NewSurvivor = Instantiate(MobPrefab) as GameObject;
                    NewSurvivor.GetComponent<MobScript>().mobOrigins = spawnType;
                    if(spawnType == "Survivors" || spawnType == "RaidSurvivors") {
                        NewSurvivor.GetComponent<MobScript>().TypeOfMob = 3;
                        Activation = 3;
                    } else if(spawnType == "Bandits" || spawnType == "BanditAmbush" || spawnType == "RaidBandits") {
                        NewSurvivor.GetComponent<MobScript>().TypeOfMob = 2;
                        Activation = 4;
                    } else if (spawnType == "MonumentOverride") {
                        NewSurvivor.GetComponent<MobScript>().TypeOfMob = 1;
                        Activation = 4;
                    }
                    float rdp = Random.Range(-Mathf.PI, Mathf.PI);
                    NewSurvivor.transform.position = this.transform.position + new Vector3(math.sin(rdp) * SpawnRadius, 1000f, math.cos(rdp) * SpawnRadius);
                    //RaycastHit place; if (Physics.Raycast(NewSurvivor.transform.position, Vector3.down, out place, Mathf.Infinity)) NewSurvivor.transform.position = place.point + Vector3.up*1.1f;
                    NewSurvivor.GetComponent<MobScript>().Squad = new int[] {squadA, spon};
                    addSubs.Add(NewSurvivor);

                    if(spawnType == "BanditAmbush"){
                        TimeTillActivation = 0.1f;
                        Activation = 5;
                        ActivationBonus = "Chase";
                    } else if (spawnType == "RaidSurvivors" || spawnType == "RaidBandits" || spawnType == "MonumentOverride"){
                        GameObject PickTarget = MonumentPointsOfInterest[(int)Random.Range(0f, MonumentPointsOfInterest.Length-0.01f)];
                        if(PickTarget){
                            NewSurvivor.transform.position = PickTarget.transform.position + PickTarget.transform.forward* Random.Range(-3f, 3f);

                            RaycastHit CheckVoid;
                            if(Physics.Raycast(NewSurvivor.transform.position, Vector3.down, out CheckVoid, Mathf.Infinity)){
                                NewSurvivor.transform.position = CheckVoid.point + Vector3.up*1.1f;
                            } else {
                                NewSurvivor.transform.position = PickTarget.transform.position;
                            }
                        }
                    }
                }
                SubOrdinates = addSubs.ToArray();
                break;
            case "DeadSurvivors": case "DeadBandits":
                for(int spon = SpawnAmount; spon > 0; spon--){
                    GameObject NewSurvivor = Instantiate(MobPrefab) as GameObject;
                    NewSurvivor.GetComponent<MobScript>().mobOrigins = spawnType;
                    if(spawnType == "DeadSurvivors") NewSurvivor.GetComponent<MobScript>().TypeOfMob = 3;
                    else if(spawnType == "DeadBandits") NewSurvivor.GetComponent<MobScript>().TypeOfMob = 2;
                    float rdp = Random.Range(-1f, 1f);
                    NewSurvivor.transform.position = this.transform.position + new Vector3(math.sin(rdp) * SpawnRadius, 1000f, math.cos(rdp) * SpawnRadius);
                    //if (Physics.Raycast(NewSurvivor.transform.position, Vector3.down, out RaycastHit place, Mathf.Infinity)) NewSurvivor.transform.position = place.point + Vector3.up*1.1f;
                    addSubs.Add(NewSurvivor);
                }
                TimeTillActivation = 1f;
                Activation = 5;
                ActivationBonus = "KillOff";
                SubOrdinates = addSubs.ToArray();
                break;
            case "BanditSurvivorClash":
                int[] squadB = new int[] {Random.Range(1, 4499), Random.Range(4500, 9999)};
                for(int spon = SpawnAmount; spon > 0; spon--){
                    GameObject NewSurvivor = Instantiate(MobPrefab) as GameObject;
                    NewSurvivor.GetComponent<MobScript>().mobOrigins = spawnType;
                    if(spon < SpawnAmount/2) {
                        NewSurvivor.GetComponent<MobScript>().TypeOfMob = 3;
                        NewSurvivor.GetComponent<MobScript>().Squad = new int[] {squadB[0], spon};
                    } else {
                        NewSurvivor.GetComponent<MobScript>().TypeOfMob = 2;
                        NewSurvivor.GetComponent<MobScript>().Squad = new int[] {squadB[1], spon - SpawnAmount/2};
                    }
                    float rdp = Random.Range(-1f, 1f);
                    NewSurvivor.transform.position = this.transform.position + new Vector3(math.sin(rdp) * SpawnRadius, 1000f, math.cos(rdp) * SpawnRadius);
                    //if (Physics.Raycast(NewSurvivor.transform.position, Vector3.down, out RaycastHit place, Mathf.Infinity)) NewSurvivor.transform.position = place.point + Vector3.up*1.1f;
                    addSubs.Add(NewSurvivor);
                }
                Activation = 4;
                SubOrdinates = addSubs.ToArray();
                break;
            case "Mutants": case "MutantsWave":
                Activation = 3;
                for(int spon = SpawnAmount; spon > 0; spon--){
                    GameObject NewSurvivor = Instantiate(MobPrefab) as GameObject;
                    NewSurvivor.GetComponent<MobScript>().mobOrigins = spawnType;
                    if(Random.Range(0f, 1f) > DifficultyLevel){
                        NewSurvivor.GetComponent<MobScript>().TypeOfMob = 1;
                    } else {
                        int[] Specs = new int[]{4, 5, 6, 7, 9, 10, 11, 12, 13};
                        NewSurvivor.GetComponent<MobScript>().TypeOfMob = Specs[(int)Random.Range(0f, Specs.Length-0.01f)];
                    }
                    float rdp = Random.Range(-Mathf.PI, Mathf.PI);
                    NewSurvivor.transform.position = this.transform.position + new Vector3(math.sin(rdp) * SpawnRadius, 1000f, math.cos(rdp) * SpawnRadius);
                    //RaycastHit place; if (Physics.Raycast(NewSurvivor.transform.position, Vector3.down, out place, Mathf.Infinity)) NewSurvivor.transform.position = place.point + Vector3.up*1.1f;
                    addSubs.Add(NewSurvivor);

                    if(spawnType == "MutantsWave"){
                        TimeTillActivation = 1f;
                        Activation = 5;
                        ActivationBonus = "Lure";
                    }
                }
                SubOrdinates = addSubs.ToArray();
                break;
            case "MutantBanditClash": case "MutantSurvivorClash":
                int MutantBias = Random.Range(1, Mathf.Clamp(SpawnAmount-1, 1, 4));
                for(int spon = SpawnAmount; spon > 0; spon--){
                    GameObject NewSurvivor = Instantiate(MobPrefab) as GameObject;
                    NewSurvivor.GetComponent<MobScript>().mobOrigins = spawnType;
                    if(spon <= MutantBias){
                        if (spawnType == "MutantBanditClash") NewSurvivor.GetComponent<MobScript>().TypeOfMob = 2;
                        else NewSurvivor.GetComponent<MobScript>().TypeOfMob = 3;
                    } else {
                        if(Random.Range(0f, 1f) > DifficultyLevel){
                            NewSurvivor.GetComponent<MobScript>().TypeOfMob = 1;
                        } else {
                            int[] Specs = new int[]{4, 5, 6, 7, 9, 10, 11, 12, 13};
                            NewSurvivor.GetComponent<MobScript>().TypeOfMob = Specs[(int)Random.Range(0f, Specs.Length-0.01f)];
                        }
                    }
                    float rdp = Random.Range(-Mathf.PI, Mathf.PI);
                    NewSurvivor.transform.position = this.transform.position + new Vector3(math.sin(rdp) * SpawnRadius, 1000f, math.cos(rdp) * SpawnRadius);
                    //RaycastHit place; if (Physics.Raycast(NewSurvivor.transform.position, Vector3.down, out place, Mathf.Infinity)) NewSurvivor.transform.position = place.point + Vector3.up*1.1f;
                    addSubs.Add(NewSurvivor);
                }
                Activation = 4;
                SubOrdinates = addSubs.ToArray();
                break;
            default:
                Debug.LogError("Cannot activate " + spawnType);
                break;
        }

    }

    string Ticket(string type){
        int offShoot = (int)Random.Range(0f, 15f);
        if(prevSpec == "War" && (type == "Survivor" || type == "Bandit") && Random.Range(0f, 1f) < 0.3f) offShoot = 1;
        switch(type){
            case "Survivor":
                switch(offShoot){
                    case 0:
                        return "RaidSurvivors"; // done
                    case 1:
                        return "BanditSurvivorClash"; // done
                    case 2:
                        return "DeadSurvivors"; // done
                    default:
                        return "Survivors"; // done
                }
            case "Bandit":
                switch(offShoot){
                    case 0:
                        return "RaidBandits"; // done
                    case 1:
                        return "BanditSurvivorClash"; // done
                    case 2:
                        return "DeadBandits"; // done
                    case 3:
                        return "BanditAmbush"; // done
                    default:
                        return "Bandits"; // done
                }
            case "Mutant":
                switch(offShoot){
                    case 0: case 1:
                        return "MutantsWave"; // done
                    case 2:
                        return "MutantBanditClash"; // done
                    case 3:
                        return "MutantSurvivorClash"; // done
                    case 4:
                        return "MonumentOverride"; // done
                    default:
                        return "Mutants"; // done
                }
            default:
                return "";
        }
    }

    void ResetPosition(){

        Vector3[] quarters = new Vector3[]{
            new Vector3(125f, 10f, 125f),
            new Vector3(-125f, 10f, 125f),
            new Vector3(125f, 10f, -125f),
            new Vector3(-125f, 10f, -125f),
        };

        for(int checkpos = 10; checkpos > 0; checkpos--){
            Vector3 offset = quarters[(int)Random.Range(0f, 3.9f)] + new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f));
            if(Vector3.Distance(offset, MainPlayer.position) > setDistance + SpawnRadius + 10f){
                this.transform.position = offset;
                break;
            }
        }

    }

    public void Attract(Vector3 there, float distance){
        if(SubOrdinates.Length <= 0f){
            this.transform.position += Vector3.ClampMagnitude(there - this.transform.position, distance);
        }
    }

}
