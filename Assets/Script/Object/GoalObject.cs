using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalObject : InteractObject
{

    private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer srUp;
    public Sprite[] pressedSprite;
    public Sprite[] releasedSprite;
    public bool isOpen = false;

    public event Action OnActivated;

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
            Debug.Log($"{name} open!!!");
            OnActivated?.Invoke();  // 이벤트 호출
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
}
