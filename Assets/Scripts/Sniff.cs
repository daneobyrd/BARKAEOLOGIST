using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff : MonoBehaviour
{
    bool isSniffing;

    Camera mainCam;

    [Tooltip("Postprocess volume active when sniffing")]
    [SerializeField]
    UnityEngine.Rendering.Volume volume;

    [SerializeField]
    float digRange = .3f;

    [Tooltip("The actual player model")]
    [SerializeField]
    Transform player;

    //Used to fade in and out the sniffing effect
    float PPWeight = 0.0f;

    //All smellables inside range
    List<Collider> withinRange = new List<Collider>();

    SphereCollider smellRange;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        isSniffing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSniffing && PPWeight < 1)
        {
            PPWeight += .01f;
            volume.weight = PPWeight;
        }
        else if (!isSniffing && PPWeight > 0)
        {
            PPWeight -= .01f;
            volume.weight = PPWeight;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Scents"))
        {
            TrailPenultimate tp = other.gameObject.GetComponent<TrailPenultimate>();
            if (tp != null)
            {
                tp.discover();
                return;
            }
        }


        if (other.gameObject.layer == LayerMask.NameToLayer("Smellable") && !withinRange.Contains(other))
        {
            withinRange.Add(other);

            if (isSniffing)
              setSniffableState(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Smellable"))
        {
            withinRange.Remove(other);
            //setSniffableState(other, false);
        }
    }

    public void sniff()
    {
        isSniffing = !isSniffing;
        if (isSniffing)
        {
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Scents");
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("ScentPreviews"));
            foreach (Collider collider in withinRange)
            {
                setSniffableState(collider, true);
            }
        }
        else
        {
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Scents"));
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("ScentPreviews");
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

    public void dig()
    {
        //Perform overlap sphere to find nearby dig spots
        //Possibly give cooldown to dig?

        Collider[] hits = Physics.OverlapSphere(player.position, digRange);

        foreach(Collider c in hits)
        {
            TrailEnd end = c.GetComponent<TrailEnd>();
            if (end != null)
            {
                end.discover();
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.position, digRange);
    }
}
    
