using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageClearScene : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {


        //이 아래로 타이머 출력하게 하는 코드

        float clearTime = PlayerPrefs.GetFloat("ClearTime", 0f);
        int minutes = Mathf.FloorToInt(clearTime / 60);
        int seconds = Mathf.FloorToInt(clearTime % 60);
        int milliseconds = Mathf.FloorToInt((clearTime * 100) % 100);

        resultText.text = $"클리어 타임\n{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}
