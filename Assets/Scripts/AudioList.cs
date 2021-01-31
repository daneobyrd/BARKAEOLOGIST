using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioList {
    public AudioClip[] clips;

    public AudioClip GetRandomClip() {
        if (clips.Length < 1) {
            return null;
        }

        int index = Random.Range(0, clips.Length);
        return clips[index];
    }
}
