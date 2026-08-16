using System;
using UnityEngine;

public class FencePart : MonoBehaviour {

    public MeshRenderer[] Meshes;
    public float Length = 2f;
    public FenceColor[] FenceColors;

    [Serializable]
    public struct FenceColor {
        public string MaterialName;
        public Color32[] Colors;
        public bool LerpColors;
        public int RandomBus;
    }

    public void Paint (float[] RandomBuses) {

        if (FenceColors == null || FenceColors.Length <= 0)
            return;

        foreach (MeshRenderer mesh in Meshes)
            foreach (Material mat in mesh.materials)
                for (int c = 0; c < FenceColors.Length; c++)
                    if (mat.name == FenceColors[c].MaterialName) {
                        float lerp = Mathf.Min(RandomBuses[FenceColors[c].RandomBus], .99f) * FenceColors[c].Colors.Length;

                        if (FenceColors[c].LerpColors) {
                            Color32 colorA = FenceColors[c].Colors[(int)lerp];
                            Color32 colorB = FenceColors[c].Colors[((int)lerp + 1) % FenceColors[c].Colors.Length];

                            mat.color = Color32.Lerp(colorA, colorB, lerp % 1f);
                        } else
                            mat.color = FenceColors[c].Colors[(int)lerp];
                        
                        break;
                    }

    }
}