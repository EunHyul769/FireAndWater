using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBGMSetter : MonoBehaviour
{
    [SerializeField] private AudioClip stageBGM;
    [SerializeField] private float fadeTime = 1.5f;
    [SerializeField] private float volume = 0.8f;

    void Start()
    {
        if (BGMManager.Instance == null || stageBGM == null) return;
        BGMManager.Instance.PlayWithCrossFade(stageBGM, fadeTime, volume);
    }
}
