using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Animator boxAnimator;

    [SerializeField]
    private Animator skullAnimator;


    [Tooltip("Time between each letter being printed")]
    public float secPerLetter = 0.06f;

    [Tooltip("SPL * EDF = Time full text is displayed before fade out")]
    public int endDelayFactor = 12;

    [Tooltip("Randomness of SPL")]
    [Range(0.0f, 1.0f)]
    public float jitter;

    private Queue<string> textQueue = new Queue<string>();
    private float currAlpha = 0.0f;

    //prevent trying to print multiple messages at a time
    private bool mutex;

    // Start is called before the first frame update
    void Start()
    {
        jitter = jitter / 2;
        jitter = secPerLetter * jitter;

        text.text = "";
        //bg.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!mutex && textQueue.Count > 0)
        {
            StartCoroutine(Typewriter(textQueue.Dequeue()));
        }
    }

    //Add a string that will be eventually printed in the dialogue box
    public void addTextToQueue(string textToWrite)
    {
        textQueue.Enqueue(string.Copy(textToWrite)); 
    }

    public void clearQueue()
    {
        textQueue.Clear();
    }

    public void testWriter()
    {
        addTextToQueue("Omae wa mou shindeiru!    Nani????");
    }

    IEnumerator Typewriter(string toWrite)
    {
        //yield return StartCoroutine(FadeGUI(true, .6f, 0));

        mutex = true;

        skullAnimator.SetTrigger("SkullTalk");
        boxAnimator.SetTrigger("DialogueStart");
        yield return new WaitForSecondsRealtime(1f);

        foreach (char c in toWrite)
        {
            text.text += c;
            yield return new WaitForSecondsRealtime(secPerLetter);
        }

        //StartCoroutine(FadeGUI(false, 2f, toWrite.Length * .05f));
        yield return new WaitForSecondsRealtime(secPerLetter * endDelayFactor + Random.Range(0, jitter));
        skullAnimator.SetTrigger("SkullTalk");
        boxAnimator.SetTrigger("DialogueDone");

        yield return new WaitForSecondsRealtime(.8f);
        text.text = "";

        mutex = false;
    }
    IEnumerator FadeGUI(bool visible, float time, float initialDelay)
    {
        yield return new WaitForSecondsRealtime(initialDelay);

        float delay = time / 20;
        float step = .05f;
        if(!visible)
        {
            step = -step;
        }

        for (int i = 0; i < 20; i++)
        {
            currAlpha += step;
            Mathf.Clamp(currAlpha, 1f, 0f);
            //bg.alpha = currAlpha;
            yield return new WaitForSecondsRealtime(delay);
        }
    }


}
