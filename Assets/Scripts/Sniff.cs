using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Controls sniffing and digging
public class Sniff : MonoBehaviour
{
    bool isSniffing;

    Camera mainCam;

    [Tooltip("Postprocess volume active when sniffing")]
    [SerializeField]
    UnityEngine.Rendering.Volume volume;

    [SerializeField]
    float digRange = .5f;

    [Tooltip("The actual player model")]
    [SerializeField]
    Transform player;

    [Tooltip("Particles to play when digging")]
    [SerializeField]
    GameObject digParticles;

    float digCooldown = 0;

    //Used to fade in and out the sniffing effect
    float PPWeight = 0.0f;

    //All smellables inside range
    List<Collider> withinRange = new List<Collider>();

    private DogActions _dogActions;

    // Start is called before the first frame update
    void Start() {
        _dogActions = new DogActions();
        _dogActions.Player.Sniff.performed += sniff;
        _dogActions.Player.Dig.performed += dig;
        _dogActions.Player.Enable();

        mainCam = Camera.main;
        isSniffing = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(withinRange.Count);
        if(digCooldown >= 0)
          digCooldown -= Time.deltaTime;

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

    public void sniff(InputAction.CallbackContext callbackContext)
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

    public void dig(InputAction.CallbackContext callbackContext)
    {


        if (digCooldown > 0)
            return;

        digCooldown = 3f;

        //Perform overlap sphere to find nearby dig spots
        Collider[] hits = Physics.OverlapSphere(player.position, digRange);
        GameObject particles = Instantiate(digParticles, player.position, Quaternion.identity);
        Destroy(particles, 4.0f);

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

