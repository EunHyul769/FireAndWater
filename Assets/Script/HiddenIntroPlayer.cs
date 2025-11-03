using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class HiddenIntroPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // 영상이 끝났을 때 실행되는 이벤트 등록
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("LevelHidden"); // 히든 스테이지로 이동
    }
}
