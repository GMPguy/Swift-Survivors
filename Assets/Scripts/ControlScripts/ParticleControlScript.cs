using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControlScript : MonoBehaviour
{

    public int[] QualityBorder = new int[] { 1, 4 };
    public bool HasSet = false;
    public float OriginalEmissionRate;
    public int MaxParticles;
    bool added = false;
    GameScript GS;

    // Start is called before the first frame update
    void Start() {
        OriginalEmissionRate = this.GetComponent<ParticleSystem>().emission.rateOverTime.constant;
        MaxParticles = this.GetComponent<ParticleSystem>().main.maxParticles;
        SetEmission();
    }

    // Update is called once per frame
    public void SetEmission() {

        if (HasSet == false) {
            OriginalEmissionRate = this.GetComponent<ParticleSystem>().emission.rateOverTime.constant;
            MaxParticles = this.GetComponent<ParticleSystem>().main.maxParticles;
            HasSet = true;
        }

        if (GS == null) {
            GS = GameObject.Find("_GameScript").GetComponent<GameScript>();
            if(!added) {
                GS.ParticleCache.Add(this);
                added = true;
            }
        }
        ParticleSystem.EmissionModule Em = this.GetComponent<ParticleSystem>().emission;
        ParticleSystem.MainModule Mm = this.GetComponent<ParticleSystem>().main;
        if (GS.ParticlesQuality < QualityBorder[0]) {
            Em.rateOverTime = 0f;
            Mm.maxParticles = 0;
        } else if (GS.ParticlesQuality >= QualityBorder[1]) {
            Em.rateOverTime = OriginalEmissionRate;
            Mm.maxParticles = MaxParticles;
        } else {
            float PerCent = Mathf.Clamp((GS.GetComponent<GameScript>().ParticlesQuality - QualityBorder[0]) / (QualityBorder[1] - QualityBorder[0]), 0.2f, 1f);
            Em.rateOverTime = OriginalEmissionRate * PerCent;
            Mm.maxParticles = (int)((float)MaxParticles * PerCent);
        }
        

    }
}
