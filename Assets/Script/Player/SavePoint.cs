using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public string flagType = "Fire"; // "Fire" or "Water" or "Start" or" Goal"
    private bool isActivated = false; // 깃발이 활성화되었는지 체크

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어만 반응
        if (collision.CompareTag("Player_Fire") || collision.CompareTag("Player_Water"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            // 해당 플레이어 속성과 깃발 속성이 일치하면 저장
            if (player != null && player.playerType == flagType || flagType == "Start")
            {
                player.UpdateSavePoint(transform.position);
                if (!isActivated)
                {
                    isActivated = true;
                    Debug.Log($"{flagType} SavePoint activated at {transform.position}");
                }

                // 깃발 애니메이션 or 색상 변경 시 여기에 추가
                // ex) GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            if (CompareTag("Goal"))
            {
                GameObject startPoint = GameObject.FindWithTag("Start");
                if (startPoint != null)
                {
                    player.UpdateSavePoint(startPoint.transform.position);
                    Debug.Log($"{player.playerType} 스타트 지점으로 가즈아 위치는 ({startPoint.transform.position})");
                }
                else
                {
                    Debug.LogWarning("Start 지점이 어디 있게여 없으니까 이게 뜨지.");
                }
            }
        }
    }
}