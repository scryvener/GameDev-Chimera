using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;
    Color originalColor;
    void Start()
    {
        text = GetComponent<Text>();
        originalColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float fadeOutTime;
    public void TextFadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }

        text.text = "";
        text.color = originalColor;
    }

}
