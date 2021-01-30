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
    private CanvasGroup bg;

    [SerializeField]
    private Animator boxAnimator;


    [Tooltip("Time between each letter being printed")]
    public float secPerLetter = 0.06f;

    [Tooltip("SPL * EDF = Time full text is displayed before fade out")]
    public int endDelayFactor = 12;

    [Tooltip("Randomness of SPL")]
    [Range(0.0f, 1.0f)]
    public float jitter;

    private string toWrite;
    private float currAlpha = 0.0f;

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
        
    }

    public void writeText(string textToWrite)
    {
        StopAllCoroutines();
        text.text = "";

        toWrite = string.Copy(textToWrite);
        StartCoroutine(Typewriter());
    }

    public void testWriter()
    {
        writeText("Omae wa mou shindeiru!    Nani????");
    }

    IEnumerator Typewriter()
    {
        //yield return StartCoroutine(FadeGUI(true, .6f, 0));

        boxAnimator.SetTrigger("DialogueStart");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Animation In");

        foreach (char c in toWrite)
        {
            Debug.Log("Printing letter");
            text.text += c;
            yield return new WaitForSecondsRealtime(secPerLetter);
        }

        //StartCoroutine(FadeGUI(false, 2f, toWrite.Length * .05f));
        yield return new WaitForSecondsRealtime(secPerLetter * endDelayFactor + Random.Range(0, jitter));
        boxAnimator.SetTrigger("DialogueDone");
        Debug.Log("Animation Out");

        yield return new WaitForSecondsRealtime(.8f);
        text.text = "";

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
            bg.alpha = currAlpha;
            yield return new WaitForSecondsRealtime(delay);
        }
    }


}
