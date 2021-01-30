using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Number of bones that must be found")]
    [SerializeField]
    int bones;
    private int bonesFound = 0;

    public Canvas winScreen;

    private static GameManager _INSTANCE;

    public static GameManager instance ()
    {
        return _INSTANCE;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_INSTANCE != null && _INSTANCE != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _INSTANCE = this;
        }
    }

    public void findBone()
    {
        bonesFound++;
        Debug.Log("Found a bone! Total: " + bonesFound);

        if (bonesFound >= bones)
        {
            //you win?
            winScreen.gameObject.SetActive(true);
        }
    }
}
