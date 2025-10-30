using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : InteractObject
{

    private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer srUp;
    public Sprite[] pressedSprite;
    public Sprite[] releasedSprite;
    public bool isOpen = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public override void Interact()
    {
        base.Interact();
    }

    public override void InteractOut()
    {
        base.InteractOut();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerCheck(collision))
        {
            //상호작용 실행
            //Interact();
            sr.sprite = pressedSprite[0];
            srUp.sprite = pressedSprite[1];
            isOpen = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (PlayerCheck(collision))
        {
            // 문 닫기
            //InteractOut();
            sr.sprite = releasedSprite[0];
            srUp.sprite = releasedSprite[1];
            isOpen = false;
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
