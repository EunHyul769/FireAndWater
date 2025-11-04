using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGMSetter : MonoBehaviour
{
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private float volume = 0.8f;

    void Start()
    {
        // 이미 재생 중이면 다시 안 건드림
        if (BGMManager.Instance == null) return;

        AudioSource src = BGMManager.Instance.GetComponent<AudioSource>();
        if (!src.isPlaying)
            BGMManager.Instance.PlayImmediate(titleBGM, volume);
    }
}
