using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
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
        // 이곳에서 문 열기, 불/물 제어 등 동작 연결
    }
}
