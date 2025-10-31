using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUILoader : MonoBehaviour
{
    private void Awake()
    {
        // MainUI가 이미 로드되어 있는지 확인
        if (SceneManager.GetSceneByName("MainUI").isLoaded == false)
        {
            SceneManager.LoadScene("MainUI", LoadSceneMode.Additive);
        }
    }
}
