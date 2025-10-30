using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverObject : InteractObject
{
    private bool isOn = false;
    private PlayerController nearbyPlayer;

    private SpriteRenderer sr;
    public Sprite pressedSprite;
    public Sprite releasedSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (nearbyPlayer != null && Input.GetKeyDown(nearbyPlayer.interactKey))
        {
            ToggleLever();
        }


        // 디버깅용 상호작용 입력
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleLever();
        }
    }

    public void SetPlayerInRange(PlayerController player)
    {
        nearbyPlayer = player;
    }

    private void ToggleLever()
    {
        isOn = !isOn;
        Debug.Log("Lever " + (isOn ? "On" : "Off"));

        if (isOn)
        {
            Interact();
        }
        else
        {
            InteractOut();
        }
    }

    public override void Interact()
    {
        //Button toggle
        base.Interact();

        if (isOn == true)
        {
            // action
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
        if(InteractPlayerNum > 0 && isOn == false)
        {
            sr.sprite = releasedSprite;
            if(targetSlide != null)
            {
                targetSlide.GetComponent<SlideObject>().DeactiveSlide();
            }
        }
    }
}
