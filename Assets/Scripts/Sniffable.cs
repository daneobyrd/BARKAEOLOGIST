using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Sniffable : MonoBehaviour
{
    
    [Tooltip("Trail elements, in order, excluding the endpoint")]
    public GameObject[] trail;

    [Tooltip("Particles at the source of the trail")]
    public GameObject source;

    [Tooltip("Particles for destination this scent leads to")]
    public GameObject endpointParticles;

    GameObject penultimate;

    bool found = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in trail)
        {
            obj.SetActive(false);
        }

        penultimate = trail[trail.Length - 1];

        TrailPenultimate tp = penultimate.AddComponent(typeof(TrailPenultimate)) as TrailPenultimate;
        tp.setEndpoint(endpointParticles);

    }

    //Show the breadcrumbs related to this sniffable
    public void show()
    {
        if (found)
            return;

        foreach (GameObject obj in trail)
        {
            obj.SetActive(true);
        }
    }

    //Hide the breadcrumbs related to this sniffable
    public void hide()
    {
        foreach (GameObject obj in trail)
        {
            obj.SetActive(false);
        }
    }

    //End of this trail was found, hide it
    public void find()
    {
        hide();
        source.SetActive(false);
        found = true;
    }
}
