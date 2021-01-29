using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff : MonoBehaviour
{
    Camera cam;
    bool isSniffing;
    //All smellables inside range
    List<Collider> withinRange = new List<Collider>();

    [Tooltip("Collider representing range of smelling")]
    SphereCollider smellRange;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        isSniffing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("List size: " + withinRange.Count);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Smellable") && !withinRange.Contains(other))
        {
            
            withinRange.Add(other);
            setSniffableState(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Smellable"))
        {
            
            withinRange.Remove(other);
            setSniffableState(other, false);
        }
    }

    public void sniff()
    {
        isSniffing = !isSniffing;
        if (isSniffing)
        {
            cam.cullingMask |= 1 << LayerMask.NameToLayer("Scents");

            foreach (Collider collider in withinRange)
            {
                setSniffableState(collider, true);
            }
        }
        else
        {
            cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Scents"));

            foreach (Collider collider in withinRange)
            {
                setSniffableState(collider, false);
            }
        }
    }

    //If this collider is associated with a sniffable, set the state of it's breadcrumbs
    private void setSniffableState(Collider collider, bool state)
    {
        Sniffable sniffable = collider.gameObject.GetComponent<Sniffable>();
        if (sniffable != null)
        {
            if (state)
                sniffable.show();
            else
                sniffable.hide();
        }
    }
}
    
