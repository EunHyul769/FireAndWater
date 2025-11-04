using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [Header("Audio Sources (둘 다 같은 오브젝트에)")]
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;

    [Header("기본 페이드 시간 (초)")]
    [SerializeField] private float defaultFadeTime = 1.5f;

    private AudioSource current;   // 현재 재생 중
    private AudioSource next;      // 다음에 재생할 소스
    private Coroutine fadeRoutine;

    void Awake()
    {
        // 싱글턴 + 씬 넘어도 유지
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 초기 바인딩
        if (sourceA == null) sourceA = gameObject.AddComponent<AudioSource>();
        if (sourceB == null) sourceB = gameObject.AddComponent<AudioSource>();

        foreach (var src in new[] { sourceA, sourceB })
        {
            src.playOnAwake = false;
            src.loop = true;
        }

        current = sourceA;
        next = sourceB;
    }

    //시작용 브금
    public void PlayImmediate(AudioClip clip, float volume = 0.8f)
    {
        if (clip == null) return;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        current.clip = clip;
        current.volume = volume;
        current.Play();
    }

    //부드럽게 전환
    public void PlayWithCrossFade(AudioClip newClip, float fadeTime = -1f, float targetVolume = 0.8f)
    {
        if (newClip == null) return;
        if (fadeTime < 0f) fadeTime = defaultFadeTime;

        // current와 next를 스왑해서 페이드
        var old = current;
        var incoming = next;

        // next 설정
        incoming.clip = newClip;
        incoming.volume = 0f;
        incoming.Play();

        // 다음 호출 대비 포인터 스왑
        current = incoming;
        next = old;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(CrossFadeRoutine(next, current, fadeTime, targetVolume));
    }

    IEnumerator CrossFadeRoutine(AudioSource from, AudioSource to, float time, float targetVol)
    {
        float t = 0f;
        float fromStart = from ? from.volume : 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / time);

            if (to) to.volume = Mathf.Lerp(0f, targetVol, k);
            if (from) from.volume = Mathf.Lerp(fromStart, 0f, k);

            yield return null;
        }

        if (to) to.volume = targetVol;
        if (from) { from.volume = 0f; from.Stop(); }
        fadeRoutine = null;
    }

    public void Stop(float fadeTime = 0.5f)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        if (!current.isPlaying) return;
        fadeRoutine = StartCoroutine(StopRoutine(fadeTime));
    }

    IEnumerator StopRoutine(float time)
    {
        float start = current.volume;
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            current.volume = Mathf.Lerp(start, 0f, t / time);
            yield return null;
        }
        current.Stop();
        current.volume = start;
        fadeRoutine = null;
    }

    public void SetVolume(float vol)    // 0~1
    {
        current.volume = Mathf.Clamp01(vol);
    }

    public void Mute(bool on)
    {
        current.mute = on;
        next.mute = on;
    }
}
