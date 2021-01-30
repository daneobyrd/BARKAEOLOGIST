using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{

    [SerializeField]
    private Text text;

    [SerializeField]
    private CanvasGroup bg;

    private string toWrite;
    private float currAlpha = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        bg.alpha = 0f;
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
        yield return StartCoroutine(FadeGUI(true, .6f, 0));
        foreach (char c in toWrite)
        {
            text.text += c;
            yield return new WaitForSecondsRealtime(.06f);
        }

        StartCoroutine(FadeGUI(false, 2f, toWrite.Length * .05f));
        
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
