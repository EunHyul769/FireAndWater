using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCam();
    }

    private void MoveCam()
    {
        if (targetPlayer.position.x - transform.position.x > offsetX)
        {
            transform.position = new Vector3(targetPlayer.position.x - offsetX, targetPlayer.position.y + offsetY, transform.position.z);
        }
        else if (targetPlayer.position.x - transform.position.x < -offsetX)
        {
            transform.position = new Vector3(targetPlayer.position.x + offsetX, targetPlayer.position.y + offsetY, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, targetPlayer.position.y + offsetY, transform.position.z);
        }
    }
}
