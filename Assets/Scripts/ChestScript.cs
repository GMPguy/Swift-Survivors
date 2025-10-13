using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class ChestScript : MonoBehaviour {

    // Main variables
    public string[] Name;
    public float3 Opening;
    public float2 Health;
    public int State; // 0 active, 1 locked, 2 opened, 3 destroyed
    // Main variables

    // Statistics
    public float2 Healths;
    public float4 OpeningTimes;
    public float2 LockChances;
    public Door[] Doors;
    public Part[] AllParts;
    public int ArmorGrade;
    // Statistics

    public Spawner[] Spawners;
    
    public GameScript GS;
    public RoundScript RS;
    public GameObject EffectPrefab;

    float activated;
    float3 doorOpening;

    // Use this for initialization
    void Awake() {
         RoundScript.CachedChest.Add(this);
    }

    // Use this after world spawn
    bool wasStarted;
    public void TheStart () {

        GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("_RoundScript").GetComponent<RoundScript>();

        doorOpening.y = Doors.Length;
        doorOpening.x = doorOpening.y - .001f;

        float diff = RS.DifficultySliderB;
        Health.y = Mathf.Lerp(Healths.x, Healths.y, diff);
        Health.x = Health.y;

        Opening.z = Mathf.Lerp(
            Random.Range(OpeningTimes.x, OpeningTimes.y),
            Random.Range(OpeningTimes.z, OpeningTimes.w),
            diff
        );

        if (Random.value < Mathf.Lerp(LockChances.x, LockChances.y, diff))
            Opening.y = Random.Range(.1f, .9f) * doorOpening.z;

        wasStarted = true;

    }

    public void TheUpdate () {
        
        activated -= Time.deltaTime;

        switch (State) {
            case 2:
                doorOpening.x -= doorOpening.z * Time.deltaTime;

                Door currDoor = Doors[Mathf.FloorToInt(doorOpening.x)];
                currDoor.transform.localRotation = Quaternion.Lerp(
                    Quaternion.Euler(currDoor.ClosedPosition),
                    Quaternion.Euler(currDoor.OpenPosition),
                    doorOpening.x % 1f
                );
                break;
        }

        if (activated <= 0f && RoundScript.CachedChest.Contains(this))
             RoundScript.CachedChest.Remove(this);
        
    }

    public void Activate (float howLong) {
        activated = howLong;

        if (!RoundScript.CachedChest.Contains(this))
             RoundScript.CachedChest.Add(this);
    }

    public void Open (bool byPlayer) {
        
        if (State != 0)
            return;

        Opening.x += Time.deltaTime;

        if (Opening.x >= Opening.y) {
            // Locked
            State = 1;
            if (byPlayer)
                GS.Mess(GS.SetString("Actually it's locked", "A jednak zamknięte"), "Error");
        } else if (Opening.x >= Opening.z) {
            // Opened
            State = 2;

            if (State != 2)
                for (int s = 0; s < Spawners.Length; s++)
                    Spawners[s].Spawn();
            
            Activate(doorOpening.y);
        }

    }

    public void Damage (float Damage, string[] attackType, Transform affected, Vector3 point) {

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

        if (State != 3)
            return;

        // Check if the attack is able to penetrate
        if (ArmorGrade == 3 || (ArmorGrade == 2 && attackType[0] is "Melee" or "Gun") || (ArmorGrade == 1 && attackType[0] == "Melee"))
            return;
        
        // Damage and destroy
        if ((Health.x -= Damage) <= 0) {
            if (State != 2)
                for (int s = 0; s < Spawners.Length; s++)
                    Spawners[s].Spawn();

            State = 3;

            for (int dp = 0; dp < AllParts.Length; dp++)
                DamagePart(AllParts[dp], true);
        }
    }

    void DamagePart (Part part, bool ripOff) {
        if (part.GlassMaterial != "") {
            if (part.transform.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
                foreach (Material mat in mesh.materials)
                    if (mat.name == part.GlassMaterial) {
                        // TODO: add glass breaking effect
                        mat.color = Color.clear;
                    }

            part.GlassMaterial = "";
        }

        if (ripOff && part.transform.GetComponent<Rigidbody>() == null) {
            Rigidbody newRig = part.transform.gameObject.AddComponent<Rigidbody>();

            newRig.AddForce(new (
                Random.Range(-1f, 1f),
                Random.Range(0f, 1f),
                Random.Range(-1f, 1f)
            ));

            newRig.AddTorque(new (
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ));
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
    }

    [System.Serializable]
    public class Part {
        public Transform transform;
        public string GlassMaterial;
        public string HitEffect;
        public bool RipOffOnHit;
    }

}
