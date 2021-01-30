using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {
    [Tooltip("The track to play. This should loop.")] [SerializeField]
    private AudioClip track;

    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        _audioSource.clip = track;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
