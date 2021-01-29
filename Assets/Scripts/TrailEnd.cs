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

    public int ID = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        item.SetActive(false);
    }

    public void discover()
    {
        item.SetActive(true);
        particles.SetActive(false);
        source.find();
    }
}
