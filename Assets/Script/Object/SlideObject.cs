using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObject : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    [SerializeField] private float moveTime;

    private Coroutine currentCoroutine;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }
    
    public void ActiveSlide()
    {
        StartNewCouroutine(true);
    }

    public void DeactiveSlide()
    {
        StartNewCouroutine(false);
    }

    private void StartNewCouroutine(bool isActive)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(Slide(isActive));
    }
    
    IEnumerator Slide(bool isActive)
    {
        Debug.Log($"{name} Slide!");
        Vector3 targetPos;
        Vector3 moveStartPos = transform.position;
        if (isActive)
        {
            targetPos = startPos + direction * distance;
            Debug.Log(targetPos);
        }
        else
        {
            targetPos = startPos;
        }
        Vector3 vec = targetPos - moveStartPos;
        float moveTimeT = moveTime * Mathf.Sqrt(Mathf.Pow(vec.x, 2) + Mathf.Pow(vec.y, 2)) / distance;
        Debug.Log(moveTimeT);
        float time = 0f;
        while (time < moveTimeT)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / moveTimeT);   // Lerp 범위에 맞추기 위한 정규화

            transform.position = Vector3.Lerp(moveStartPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }
}

