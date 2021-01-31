﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class TrailEnd : MonoBehaviour
{
    [Tooltip("What object appears when this endpoint is reached")]
    public GameObject item;

    [Tooltip("Particles at the endpoint")]
    public GameObject particles;

    [Tooltip("Source of this scent trail")]
    public Sniffable source;

    public bool displayStars;

    public GameObject starParticles;

    public string message;

    [Tooltip("Audio source to play when this item is found")]
    public AudioSource foundSource;

    private bool found = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        item.SetActive(false);
    }

    public void discover()
    {
        if (found)
            return;

        if(displayStars)
        {
            StartCoroutine(digDelay());
        }

        found = true;
        particles.SetActive(false);
        source.find();

        if (foundSource) {
            foundSource.Play();
        }
    }

    IEnumerator digDelay()
    {
        yield return new WaitForSeconds(1.5f);
        item.SetActive(true);
        GameManager.instance().foundObject(displayStars, message);

        if (displayStars)
        {
            GameObject particles = Instantiate(starParticles, item.transform.position, Quaternion.identity);
            Destroy(particles, 8f);
        }

    }
}
