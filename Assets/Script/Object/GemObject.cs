using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemObject : InteractObject
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerCheck(collision))
        {
            Destroy(gameObject);
            
            //보석 수집 효과 적용
        }
    }

    private bool PlayerCheck(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_Fire"))
        {
            if (Element == ObjectElement.Fire)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (collision.gameObject.CompareTag("Player_Water"))
        {
            if (Element == ObjectElement.Water)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
