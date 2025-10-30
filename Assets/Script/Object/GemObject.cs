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
}
