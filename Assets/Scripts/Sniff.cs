using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff : MonoBehaviour
{
    bool isSniffing;

    [Tooltip("Camera for *SCENTS*")]
    [SerializeField]
    Camera smellCam;

    [Tooltip("Postprocess volume active when sniffing")]
    [SerializeField]
    UnityEngine.Rendering.Volume volume;

    //Used to fade in and out the sniffing effect
    float PPWeight = 0.0f;

    //All smellables inside range
    List<Collider> withinRange = new List<Collider>();

    SphereCollider smellRange;

    // Start is called before the first frame update
    void Start()
    {
        isSniffing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSniffing && PPWeight < 1)
        {
            PPWeight += .02f;
            volume.weight = PPWeight;
        }
        else if (!isSniffing && PPWeight > 0)
        {
            PPWeight -= .02f;
            volume.weight = PPWeight;
        }
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
            smellCam.gameObject.SetActive(true);
            foreach (Collider collider in withinRange)
            {
                setSniffableState(collider, true);
            }
        }
        else
        {
            smellCam.gameObject.SetActive(false);
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
    
