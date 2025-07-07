using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class MinimapMarker : MonoBehaviour {

    public float MapSize;
    public float MinimapSize;
    public bool UpdateRotation;

    bool active = true;
    int prevMapType = -1;

    void Start() =>
        CanvasScript.MapMarkers.Add(this);

    public void UpdateMe (float2 mapSize, int MapType, Vector3 cameraForward) {

        // Update according to map difference
        if (prevMapType != MapType) {

            if (MapType == -1 || (MapType == 0 && MinimapSize <= 0f) || (MapSize <= 0f && MapType == 1)) {
                active = false;
                transform.localScale = Vector3.zero;
            } else {
                float ratio = mapSize.y / mapSize.x;
                transform.localScale = Vector3.one * ratio * (MapType == 0 ? MinimapSize : MapSize);
                active = true;
            }

            prevMapType = MapType;
        }

        if (!active)
            return;

        // Update stuff
        if (UpdateRotation)
            transform.LookAt(transform.position + cameraForward);

    }
}
