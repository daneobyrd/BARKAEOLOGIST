using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Sniffable : MonoBehaviour
{
    
    [Tooltip("Breadcrumbs associate with this smell")]
    public GameObject trail;


    // Start is called before the first frame update
    void Start()
    {
        trail.SetActive(false);
    }

    //show the breadcrumbs related to this sniffable
    public void show()
    {
        trail.SetActive(true);
    }

    //hide the breadcrumbs related to this sniffable
    public void hide()
    {
        trail.SetActive(false);
    }


}
