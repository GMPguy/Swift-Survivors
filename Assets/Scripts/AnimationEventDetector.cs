using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventDetector : MonoBehaviour {
    public string ReadMessage;

    public void AnimationEventTrigger (string theNew) =>
        ReadMessage = theNew;
}
