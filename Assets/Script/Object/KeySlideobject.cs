using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlideobject : InteractObject
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    [SerializeField] private float moveTime;

    private Coroutine currentCoroutine;

    private Vector3 startPos;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player_Fire") || collision.CompareTag("Player_Water"))
        {
            if(collision.GetComponent<PlayerController>().heldKey != null)          
            {
                if (PlayerCheck(collision))
                {
                    if (CompareKeyElement(collision.GetComponent<PlayerController>().heldKey))
                    {
                        Interact();
                    }
                    // + 사용한 키 삭제 처리
                }
            } 
        }     
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public override void Interact()
    {
        Slide(true);
    }

    public override void InteractOut()
    {

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

    private bool CompareKeyElement(KeyItem keyItem)
    {
        if (Element == ObjectElement.Fire && keyItem.CompareTag("Key_Fire"))
        {
            return true;
        }
        else if (Element == ObjectElement.Water && keyItem.CompareTag("Key_Water"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
