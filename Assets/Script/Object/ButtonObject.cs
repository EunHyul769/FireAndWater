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
            if(targetSlide != null)
            {
                targetSlide.GetComponent<SlideObject>().ActiveSlide();
            }

            // +animation
            sr.sprite = pressedSprite;
        }
        else
        {
            return;
        }

    }

    public override void InteractOut()
    {
        // when no player interactable 
        if(InteractPlayerNum < 1)
        {
            isPressed = false;
            sr.sprite = releasedSprite;
            if(targetSlide != null)
            {
                targetSlide.GetComponent<SlideObject>().DeactiveSlide();
            }
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
