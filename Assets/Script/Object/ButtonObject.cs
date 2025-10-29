using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : InteractObject
{
    public Sprite pressedSprite;
    public Sprite releasedSprite;

    private SpriteRenderer sr;
    private bool isPressed = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        //Button toggle
        base.Interact();

        if (!isPressed)
        {
            // action
            isPressed = true;
            // +animation
        }
        else
        {
            return;
        }

    }

    public void InterectOut()
    {
        // when no player interactable 
        if(InteractPlayerNum < 1)
        {
            isPressed = false;
        }
    }



    public void Press()
    {
        if (isPressed) return;
        isPressed = true;
        sr.sprite = pressedSprite;
        Debug.Log("Button Pressed!");
    }

    public void Release()
    {
        if (!isPressed) return;
        isPressed = false;
        sr.sprite = releasedSprite;
        Debug.Log("Button Released!");
    }
}
