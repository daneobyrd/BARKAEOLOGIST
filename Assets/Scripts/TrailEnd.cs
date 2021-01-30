using System.Collections;
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

        found = true;
        GameManager.instance().findBone();
        item.SetActive(true);
        particles.SetActive(false);
        source.find();
    }
}
