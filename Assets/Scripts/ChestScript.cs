using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random=UnityEngine.Random;

public class ChestScript : MonoBehaviour {

    // Main variables
    public string[] Name;
    public float3 Opening;
    public float2 Health;
    public int State; // 0 active, 1 locked, 2 opened, 3 destroyed
    public int Seed;
    // Main variables

    // Statistics
    public float2 Healths;
    public float4 OpeningTimes;
    public float2 LockChances;
    public Door[] Doors;
    public Part[] AllParts;
    // Statistics

    public Spawner[] Spawners;
    public GameObject Lock;
    public GameObject MinimapMarker;
    public ColorBank[] colorBanks;
    
    public GameScript GS;
    public RoundScript RS;
    public GameObject EffectPrefab;

    public float activated;
    public float3 doorOpening;
    public AnimationCurve[] openingCurve;
    int prevDoor = -1;
    int pickCurve = 0;

    Vector3 squeezeScale;
    float squeezeTime;

    // Use this for initialization
    void Awake() {
         RoundScript.CachedChest.Add(this);
    }

    // Use this after world spawn
    bool wasStarted;
    public void TheStart () {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();

        // Stats
        float diff = RS.DifficultySliderB;
        Health.y = Mathf.Lerp(Healths.x, Healths.y, diff);
        Health.x = Health.y;

        Seed = Random.Range(int.MinValue, int.MaxValue);

        Opening.z = Mathf.Lerp(
            Random.Range(OpeningTimes.x, OpeningTimes.y),
            Random.Range(OpeningTimes.z, OpeningTimes.w),
            diff
        );

        if (Random.value < Mathf.Lerp(LockChances.x, LockChances.y, diff))
            Opening.y = Mathf.Lerp(.1f, .9f, Mathf.Pow(Random.value, 3)) * Opening.z;
        else
            Opening.y = Opening.z * 2f;

        // Colors
        float[] randoms = new float[colorBanks.Length];
        for (int r = 0; r < randoms.Length; r++)
            randoms[r] = Random.Range(0f, .999f);

        for (int cb = 0; cb < colorBanks.Length; cb++) {
            // Get color
            ColorBank bank = colorBanks[cb];
            float randome = randoms[bank.RandomBus] * bank.Colors.Length;

            Color theColor = Color.black;

            if (bank.IsSmooth) {
                int firstRandom = (int)randome;
                int secondRandom = (firstRandom + 1) % bank.Colors.Length;
                theColor = Color.Lerp(bank.Colors[firstRandom], bank.Colors[secondRandom], randome % 1f);
            } else {
                theColor = bank.Colors[(int)randome];
            }

            // Set color
            for (int mr = 0; mr < bank.Meshes.Length; mr++) {
                MeshRenderer mesh = bank.Meshes[mr];
                foreach (Material mat in mesh.materials)
                    for (int gn = 0; gn < bank.Materials.Length; gn++)
                        if (mat.name == bank.Materials[gn]) {
                            mat.color = theColor;
                            break;
                        }
            }
        }

        wasStarted = true;

    }

    public void TheUpdate () {
        
        activated -= Time.deltaTime;

        switch (State) {
            case 2:
                // Opening of door
                doorOpening.x -= doorOpening.z * Time.deltaTime;

                if (doorOpening.x > 0) {
                    if (prevDoor != Mathf.FloorToInt(doorOpening.x)) {
                        prevDoor = Mathf.FloorToInt(doorOpening.x);
                        pickCurve = Random.Range(0, openingCurve.Length);
                        
                        AudioSource doorSound = Doors[prevDoor].transform.GetComponent<AudioSource>();
                        doorSound.clip = Doors[prevDoor].OpeningSound[Random.Range(0, Doors[prevDoor].OpeningSound.Length)];
                        doorSound.Play();
                    }

                    Door currDoor = Doors[prevDoor];
                    currDoor.transform.localRotation = Quaternion.LerpUnclamped(
                        Quaternion.Euler(currDoor.OpenPosition),
                        Quaternion.Euler(currDoor.ClosedPosition),
                        openingCurve[pickCurve].Evaluate(doorOpening.x % 1f)
                    );

                    currDoor.transform.gameObject.layer = 13;
                }
                break;
        }

        // Squeeze damage
        if ((squeezeTime -= Time.deltaTime) >= 0f)
            this.transform.localScale = Vector3.Lerp(Vector3.one, squeezeScale, squeezeTime);

        if (activated <= 0f && RoundScript.CachedChest.Contains(this)) {
             RoundScript.CachedChest.Remove(this);

             if (State == 3) {
                for (int fp = 0; fp < AllParts.Length; fp++)
                    if (AllParts[fp].transform.TryGetComponent<Rigidbody>(out var rig))
                        Destroy(rig);
             }
        }
        
    }

    public void Activate (float howLong) {
        activated = Mathf.Max(howLong, activated);

        if (!RoundScript.CachedChest.Contains(this))
             RoundScript.CachedChest.Add(this);
    }

    public void Unlock (PlayerScript ps) {
        if (State != 0 && State != 1)
            return;

        // Unlock
        Destroy(Lock);
        Destroy(MinimapMarker);
        ps.CantInteract = .5f;

        Random.InitState(Seed);
        if (State != 2)
            for (int s = 0; s < Spawners.Length; s++)
                Spawners[s].Spawn();

        State = 2;
            
        Activate(doorOpening.y);
    }

    public void Open (PlayerScript ps) {
        
        if (State != 0)
            return;

        Opening.x += Time.deltaTime;
        ps.MainCanvas.CSWait = new float[]{ Opening.x / Opening.z, 0.2f};

        if (Opening.x >= Opening.y) {
            // Locked
            State = 1;
            Lock.GetComponent<Interactions>().Options[0] = "Locked";
            Destroy(MinimapMarker);
            if (ps != null)
                GS.Mess(GS.SetString("Actually it's locked", "A jednak zamknięte"), "Error");
        } else if (Opening.x >= Opening.z) {
            // Opened
            Destroy(Lock);
            Destroy(MinimapMarker);
            ps.CantInteract = .5f;

            if (ps)
                RS.SetScore("ChestsOpened_", "/+1");

            Random.InitState(Seed);
            if (State != 2)
                for (int s = 0; s < Spawners.Length; s++)
                    Spawners[s].Spawn();

            State = 2;
            
            Activate(doorOpening.y);
        }

    }

    public void Damage (float Damage, string[] attackType, Transform affected, Vector3 point, GameObject killer = null) {

        // Effects regarding hit parts
        Part hitPart = GetPart(affected);
        if (hitPart != null) {

            if (hitPart.HitEffect != "") {
                EffectScript hitEffect = Instantiate(EffectPrefab).GetComponent<EffectScript>();
                hitEffect.transform.position = point;
                hitEffect.transform.LookAt(point - affected.position);
                hitEffect.EffectName = hitPart.HitEffect;
            }

            DamagePart(hitPart, hitPart.RipOffOnHit);

        }

        if (State == 3)
            return;

        // Check if the attack is able to penetrate
        if (hitPart != null && (hitPart.ArmorGrade == 3 || (hitPart.ArmorGrade == 2 && attackType[0] is "Melee" or "Gun") || (hitPart.ArmorGrade == 1 && attackType[0] == "Melee")))
            return;
        
        // Damage and destroy
        Activate(1f);
        squeezeTime = 1f;
        squeezeScale = new (1.1f, .9f, 1.1f);

        if ((Health.x -= Damage) <= 0) {
            if (State != 2) {
                Random.InitState(Seed);
                for (int s = 0; s < Spawners.Length; s++)
                    Spawners[s].Spawn(new[]{"BrokenChest"});
            }

            State = 3;
            Destroy(Lock);
            Destroy(MinimapMarker);

            for (int dp = 0; dp < AllParts.Length; dp++)
                if (GS.DestructionQuality == 2 || AllParts[dp].IsAlsoDoor)
                    DamagePart(AllParts[dp], true);
            
            if (killer && killer.tag == "Player")
                RS.SetScore("ChestsDestroyed_", "/+1");

            Activate(GS.DestructionQuality == 2 ? 300f : 10f);
        }
    }

    void DamagePart (Part part, bool ripOff) {
        if (part.GlassMaterial != "") {
            if (part.transform.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
                foreach (Material mat in mesh.materials)
                    if (mat.name == part.GlassMaterial) {
                        EffectScript newEffect = Instantiate(EffectPrefab).GetComponent<EffectScript>();
                        newEffect.transform.position = part.transform.position;
                        newEffect.EffectName = "GlassBreak";
                        newEffect.EffectColor = mat.color;
                        mat.color = Color.clear;
                    }

            part.GlassMaterial = "";
        }

        if (ripOff) {
            part.ArmorGrade = 3;

            if (part.BreakSound != null && part.BreakSound.Length > 0) {
                AudioSource breakSound = part.transform.GetComponent<AudioSource>();
                breakSound.clip = part.BreakSound[Random.Range(0, part.BreakSound.Length)];
                breakSound.Play();
            }

            if (GS.DestructionQuality == 0)
                part.transform.localScale = Vector3.zero;
            else
                if (part.transform.GetComponent<Rigidbody>() == null) {
                    Rigidbody newRig = part.transform.gameObject.AddComponent<Rigidbody>();

                    newRig.AddForce(new (
                        Random.Range(-10f, 10f),
                        Random.Range(0f, 5f),
                        Random.Range(-10f, 10f)
                    ), ForceMode.VelocityChange);

                    newRig.AddTorque(new (
                        Random.Range(-100f, 100f),
                        Random.Range(-100f, 100f),
                        Random.Range(-100f, 100f)
                    ), ForceMode.VelocityChange);
                }
        }
    }

    Part GetPart (Transform target) {
        
        for (int ap = 0; ap < AllParts.Length; ap++)
            if (AllParts[ap].transform == target)
                return AllParts[ap];

        return null;

    }

    [System.Serializable]
    public class Door {
        public Transform transform;
        public Vector3 ClosedPosition;
        public Vector3 OpenPosition;
        public AudioClip[] OpeningSound;
    }

    [System.Serializable]
    public class Part {
        public Transform transform;
        public string GlassMaterial;
        public string HitEffect;
        public bool RipOffOnHit;
        public AudioClip[] BreakSound;
        public int ArmorGrade = 0;
        public bool IsAlsoDoor;
    }

    [System.Serializable]
    public class ColorBank {
        public MeshRenderer[] Meshes;
        public string[] Materials;
        public Color[] Colors;
        public bool IsSmooth;
        public int RandomBus;
    }

}
