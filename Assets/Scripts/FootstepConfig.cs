using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootstepConfig", menuName = "Configs/Footsteps")]
public class FootstepConfig : ScriptableObject {

    public Footstep[] MaterialBanks;

    public AudioClip GetFootstep (string theType, ref int sequence) {
        for (int f = 0; f < MaterialBanks.Length; f++)
            if (MaterialBanks[f].FootstepType == theType) {
                sequence = (sequence + Random.Range(1, 2)) % MaterialBanks[f].FootstepSounds.Length;
                return MaterialBanks[f].FootstepSounds[sequence];
            }
        
        Debug.LogWarning($"No material bank of name {theType} found!");
        return null;
    }

    public AudioClip GetFootstep (string theType) {
        for (int f = 0; f < MaterialBanks.Length; f++)
            if (MaterialBanks[f].FootstepType == theType)
                return MaterialBanks[f].FootstepSounds[Random.Range(0, MaterialBanks[f].FootstepSounds.Length)];
        
        Debug.LogWarning($"No material bank of name {theType} found!");
        return null;
    }
    
    [System.Serializable]
    public class Footstep {
        public string FootstepType;
        public AudioClip[] FootstepSounds;
        public AudioClip[] BullethitSounds;
    }

}
