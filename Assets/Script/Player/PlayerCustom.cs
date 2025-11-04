using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCustom : MonoBehaviour
{
    [Header("Fire Player Settings")]
    public Image firePreview; // 미리보기용 이미지
    public Slider fireR;
    public Slider fireG;
    public Slider fireB;

    [Header("Water Player Settings")]
    public Image waterPreview;
    public Slider waterR;
    public Slider waterG;
    public Slider waterB;

    private Color fireColor;
    private Color waterColor;

    private void Start()
    {
        // 기존 저장된 색상 불러오기 (없으면 기본값)
        fireColor = LoadColor("FireColor", new Color(1f, 1f, 1f)); // 기본 빨강 이미지라 모든 값들 최대치
        waterColor = LoadColor("WaterColor", new Color(1f, 1f, 1f)); // 기본 파랑 이미지라 모든 값들 최대치

        UpdateSlidersFromColor(fireR, fireG, fireB, fireColor);
        UpdateSlidersFromColor(waterR, waterG, waterB, waterColor);
    }

    private void Update()
    {
        // 실시간 미리보기
        fireColor = new Color(fireR.value, fireG.value, fireB.value);
        waterColor = new Color(waterR.value, waterG.value, waterB.value);

        firePreview.color = fireColor;
        waterPreview.color = waterColor;
    }

    public void SaveColorsAndTitle()
    {
        SaveColor("FireColor", fireColor);
        SaveColor("WaterColor", waterColor);
        Debug.Log("Player colors saved!");

        SceneManager.LoadScene("TitleScene");
    }

    private void SaveColor(string key, Color color)
    {
        PlayerPrefs.SetFloat(key + "R", color.r);
        PlayerPrefs.SetFloat(key + "G", color.g);
        PlayerPrefs.SetFloat(key + "B", color.b);
    }

    private Color LoadColor(string key, Color defaultColor)
    {
        if (PlayerPrefs.HasKey(key + "R"))
        {
            float r = PlayerPrefs.GetFloat(key + "R");
            float g = PlayerPrefs.GetFloat(key + "G");
            float b = PlayerPrefs.GetFloat(key + "B");
            return new Color(r, g, b);
        }
        return defaultColor;
    }

    private void UpdateSlidersFromColor(Slider r, Slider g, Slider b, Color color)
    {
        r.value = color.r;
        g.value = color.g;
        b.value = color.b;
    }
}
