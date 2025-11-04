using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AchievementPopup : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 moveVector;
    public float moveTime;
    public float waitTime;
    void Start()
    {
        transform.position = startPos;
    }
}
