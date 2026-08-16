using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class FenceSpawner : MonoBehaviour {
    
    public float4 DelapitationBias = new (-.5f, .75f, .5f, 2f);
    public float2 EntrancePoint = new (.25f, .75f);

    public float2 RectSize;
    public float Diameter;

    public FencePart[] FenceObjects;

    public void SpawnFence (float diff, Transform parent) {

        // Prepare fence positions
        List<Vector3> fencePos = new ();
        List<Vector3> fenceRot = new ();

        FencePart baseFence = FenceObjects[(int)Random.Range(0f, FenceObjects.Length)];
        float length = baseFence.Length;
        bool rotate = false;

        if (Diameter > 0f) {
            // Spherical area fences
            rotate = true;
            float circumference = Diameter * Mathf.PI;

            int sections = (int) (circumference / length);
            float roundedCircumference = sections * length;

            float radius = roundedCircumference / Mathf.PI;
            radius /= 2f;

            for (int go = 0; go < sections; go++) {
                float lerp = (float)go / sections * Mathf.PI * 2f;

                Vector3 pos = transform.position;
                pos += transform.right * radius * Mathf.Cos(lerp);
                pos += transform.forward * radius * Mathf.Sin(lerp);

                Vector3 rot = (transform.position - pos).normalized;

                fencePos.Add(pos);
                fenceRot.Add(rot);
            }
        } else {
            // Rectangle area fences
            int2 sections = new ( (int) (RectSize.x / length), (int) (RectSize.y / length) );
            float2 roundedLength = new (sections.x * length, sections.y * length);

            int entrancePoint = Random.Range(
                (int) (sections.x * EntrancePoint.x),
                (int) (sections.x * EntrancePoint.y)
            );

            Vector3[] dirs = new Vector3[] {
                transform.right,
                transform.forward
                
            };

            Vector3 forward = transform.forward * roundedLength.y / 2f;
            Vector3 right = transform.right * roundedLength.x / 2f;

            // TODO - kurwa do naprawy ja pierdole
            Vector3[] offs = new Vector3[] {
                -forward - right,
                -forward - right,
                forward - right,
                -forward + right
            };

            for (int si = 0; si < 4; si++) {
                int dir = si % 2;

                Vector3 fencePoint = transform.position + offs[si];

                for (int se = 0; se < sections[dir]; se++) {
                    if (si == 2 && se == entrancePoint)
                        continue;

                    fencePos.Add(fencePoint + dirs[dir] * (length * se));
                    fenceRot.Add(dirs[dir]);
                }
            }
        }

        // Place fences
        int maxBusses = -1;
        float[] randomBusses = new float[0];

        if (baseFence.FenceColors != null && baseFence.FenceColors.Length > 0) {
            for (int fm = 0; fm < baseFence.FenceColors.Length; fm++)
                if (baseFence.FenceColors[fm].RandomBus > maxBusses)
                    maxBusses = baseFence.FenceColors[fm].RandomBus;
            
            randomBusses = new float[maxBusses + 1];
        }
        
        for (int f = 0; f < maxBusses + 1; f++)
            randomBusses[f] = Random.value;

        for (int f = 0; f < fencePos.Count; f++) {
            float delap = Random.Range(
                Mathf.Lerp(DelapitationBias.x, DelapitationBias.z, diff),
                Mathf.Lerp(DelapitationBias.y, DelapitationBias.w, diff)
            );

            if (delap >= 1.25f)
                continue;

            FencePart newFence = GameObject.Instantiate(baseFence.gameObject).GetComponent<FencePart>();

            newFence.transform.position = fencePos[f];
            newFence.transform.forward = fenceRot[f];

            if (rotate)
                newFence.transform.Rotate(Vector3.up * -90f);
            newFence.transform.SetParent(parent);

            if (delap >= 1f) {
                newFence.transform.Rotate(transform.forward * 90f);
                newFence.transform.Rotate(transform.right * Random.Range(0f, 360f));
            } else if (delap > .5f) {
                newFence.transform.position -= Vector3.up * (delap - .5f) / 2f;

                newFence.transform.Rotate(new Vector3 (
                    Random.Range(-30f, 30f),
                    Random.Range(-30f, 30f),
                    Random.Range(-30f, 30f)
                ) * (delap - .5f));
            }

            if (maxBusses >= 0)
                newFence.Paint(randomBusses);
        }
    }

}
