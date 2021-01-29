using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrailPenultimate : MonoBehaviour
{
    //The second to last part of a scent trail
    //Discovering this object shows the endpoint of the trail

    //NOTE: This script is dynamically added at runtime.
    //Don't add this to anything in the editor!

    //Particles or such to show the endpoint
    GameObject endpoint;

    // Start is called before the first frame update
    void Start()
    {
        //add an rb to this object if it doesn't have one yet
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        }

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void setEndpoint(GameObject obj)
    {
        endpoint = obj;
        endpoint.SetActive(false);
    }

    //Called when player gets close enhough to this trail element
    public void discover()
    {
        endpoint.SetActive(true);
    }
}
