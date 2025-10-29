using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverObject : InteractObject
{
    private bool isOn = false;
    private PlayerController nearbyPlayer;
    private void Update()
    {
        if (nearbyPlayer != null && Input.GetKeyDown(nearbyPlayer.interactKey))
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
        // �̰����� �� ����, ��/�� ���� �� ���� ����
    }

    public override void Interact()
    {
        base.Interact();

    }
}
