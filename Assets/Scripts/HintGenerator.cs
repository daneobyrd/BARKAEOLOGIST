using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintGenerator : MonoBehaviour
{
    enum State
    {
        normal,
        missed
    }

    [SerializeField]
    TextWriter writer;

    public float minInterval = 45f;
    public float maxInterval = 70f;
    float interval;

    public string[] normalHints;
    public string[] missedItemHints;
    State state;

    // Start is called before the first frame update
    void Start()
    {
        setNormal();
        interval = (minInterval + maxInterval) / 2;

        writer.addTextToQueue("Hey Furball! Help!");
        writer.addTextToQueue("Try digging [R] the ground in front of you!");
    }

    // Update is called once per frame
    void Update()
    {
        interval -= Time.deltaTime;

        if(interval <= 0)
        {
            interval = Random.Range(minInterval, maxInterval);
            printMessage();
        }
    }

    private void printMessage()
    {
        if (!writer.ready())
            return;

        if (state == State.normal)
        {
            writer.addTextToQueue(normalHints[Random.Range(0, normalHints.Length)]);
        }
        else
        {
            writer.addTextToQueue(missedItemHints[Random.Range(0, normalHints.Length)]);
        }
    }

    public void setNormal()
    {
        state = State.normal;
    }

    public void setMissed()
    {
        state = State.missed;
    }
}
