using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private float offsetX;
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
            transform.position = new Vector3(targetPlayer.position.x - offsetX, transform.position.y, transform.position.z);
        }
        else if(targetPlayer.position.x - transform.position.x < -offsetX)
        {
            transform.position = new Vector3(targetPlayer.position.x + offsetX, transform.position.y, transform.position.z);
        }
    }
}
