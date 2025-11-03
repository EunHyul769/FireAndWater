using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScriptObject : InteractObject
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private float fadeTime;

    public override void Interact()
    {
        base.Interact();
        //targetText.gameObject.SetActive(true);
        StartCoroutine(FadeText(true));
    }

    public override void InteractOut()
    {
        base.InteractOut();
        //targetText.gameObject.SetActive(false);
        StartCoroutine(FadeText(false));
    }
    IEnumerator FadeText(bool io)
    {
        float time = 0f;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = 0f;
            if (io)
            {
               alpha = Mathf.Clamp01(time / fadeTime);   // Lerp 범위에 맞추기 위한 정규화 
            }
            else
            {
                alpha =  1 - Mathf.Clamp01(time / fadeTime);
            }
            Color newColor = targetText.color;
            newColor.a = alpha;
            targetText.color = newColor;
            yield return null;
        }
    }
}