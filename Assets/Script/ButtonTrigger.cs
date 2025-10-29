using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public Sprite pressedSprite;
    public Sprite releasedSprite;

    private SpriteRenderer sr;
    private bool isPressed = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Press()
    {
        if (isPressed) return;
        isPressed = true;
        sr.sprite = pressedSprite;
        Debug.Log("Button Pressed!");
        // 이벤트 트리거 연결 (예: 문 열기)
    }

    public void Release()
    {
        if (!isPressed) return;
        isPressed = false;
        sr.sprite = releasedSprite;
        Debug.Log("Button Released!");
    }
}
