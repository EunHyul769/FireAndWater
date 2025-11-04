using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text progressText;
    public Image checkIcon;

    // UI 업데이트 함수
    public void SetData(Achievement data)
    {
        titleText.text = data.title;
        descriptionText.text = data.description;
        progressText.text = $"{data.progress} / {data.goal}";

        // 해금 상태 표시
        checkIcon.gameObject.SetActive(data.isUnlocked);

        // 색상 변경 예시 (해금 시 밝게)
        //GetComponent<Image>().color = data.isUnlocked ? new Color(0.8f, 1f, 0.8f) : Color.white;
    }
}
