using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAddFade : MonoBehaviour
{
    public float fadeSpeed = 1.0f;
    public float fadeTime = 0f;

    public float colorAlpha;

    private void Awake()
    {
        Color color = GetComponent<TMPro.TextMeshProUGUI>().color;
        GetComponent<TMPro.TextMeshProUGUI>().color = new Color(color.r, color.b, color.g, 0);
    }

    private void Update()
    {
        if (GetComponent<TMPro.TextMeshProUGUI>().color.a == 0)
        {
            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "+0";
            return;
        }

        fadeTime += Time.deltaTime;
        colorAlpha = Mathf.Lerp(2f, 0, fadeTime / fadeSpeed);

        Color color = GetComponent<TMPro.TextMeshProUGUI>().color;
        GetComponent<TMPro.TextMeshProUGUI>().color = new Color(color.r, color.b, color.g, colorAlpha);
    }

    [ContextMenu("Restart fade")]
    public void RestartFade()
    {
        fadeTime = 0;
        Color color = GetComponent<TMPro.TextMeshProUGUI>().color;
        GetComponent<TMPro.TextMeshProUGUI>().color = new Color(color.r, color.b, color.g, 1);
    }
}
