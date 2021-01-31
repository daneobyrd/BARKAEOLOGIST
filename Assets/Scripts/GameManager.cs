using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Number of bones that must be found")]
    [SerializeField]
    int bones;

    [Tooltip("Refrence to dialogue box")]
    [SerializeField]
    TextWriter writer;

    private int bonesFound = 0;

    public Transform winScreen;

    public GameUIManager uIManager;

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

    public void foundObject(bool isBone, string msg)
    {
        writer.addTextToQueue(msg);
        if (isBone)
        {
            bonesFound++;
            uIManager.ToggleBone(bonesFound - 1);
        }

        if (bonesFound >= bones)
        {
            //you win?
            winScreen.gameObject.SetActive(true);
            uIManager.winButton.Select();
            Time.timeScale = 0;
        }
    }
}
