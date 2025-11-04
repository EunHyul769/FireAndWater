#10조 유니티 입문 팀 과제
--
레퍼런스 게임 : Fire&Water<br>
게임 소개 : 2인 협동 2D 퍼즐 게임으로 기믹을 이용해 목표에 도달하는 게임<br>

---
# 프로젝트 팀원 및 역할분담
	
김지훈	중간관리자(전반적 수정&조율)+타이머+씬 백업+bgm<br>
김경찬	플레이어<br>
김주원	오브젝트<br>
김해종	씬 전환+a<br>
전규태	맵 제작<br>

---

## 🎮 게임 소개
|게임 시연|
|:---:|
|![게임시연영상](https://github.com/user-attachments/assets/b11c516a-42d3-4d9a-bb8e-e3d55b6e3560)|

- 장애물을 피하고 아이템을 먹으며 점수 획득
- 슬라이드, 점프와 같은 액션 가능
- 일정 시간마다 체력이 닳으며 체력이 없으면 점수와 같이 결과창 출력
- 
<br>

---

## 🧩 주요 기능 코드 
<br>

### 점프와 슬라이드 

<br>

        void Jump()
    {
    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
    jumpCount++;
    isGrounded = false;

    sfxController?.PlayJumpSFX();
    }
----------------------------------------------------------------------
    void Slide()
    {
    if (!isSlide && isGrounded)
    {
        isSlide = true;
        animator.SetTrigger("isSlide");
        playerCollider.size = sliderColliderSize;
        playerCollider.offset = sliderColliderOffset;

        sfxController?.PlaySlideSFX();
    }

    }

<br>

|점프|슬라이드|
|:---:|:---:|
|![Jump](https://github.com/user-attachments/assets/989d2c2f-3081-4914-a822-58cd614ea2df)| ![Slide](https://github.com/user-attachments/assets/0da77af1-f1c0-42b6-a9a4-b249c314da1e) |


### 피격 & 무적 상태

 - 플레이어가 장애물에 부딪히면 Hp가 감소하고 일시적으로 무적상태가 됩니다.
 
   <br>
```
    public void TakeDamage(int damage)
    {
    if (isInvincible || isHit)
    {
        Debug.Log("무적");
        return;
    }


    currentHp -= damage;
    if (currentHp <= 0)
    {
        currentHp = 0;
        Die();
        return;

    }


    if (invCoroutine != null)
    {
        StopCoroutine(invCoroutine);

    }

    if (damage > tickDamage) // 틱뎀에 의한 무적발생 방지
    {
        invCoroutine = StartCoroutine(OnHitRoutine());
    }

    }
```
-----
```
IEnumerator OnHitRoutine()
    {
    isHit = true;
    isInvincible = true;
    sfxController?.PlayHitSFX();
    animator.SetBool("isHit", true);
    yield return new WaitForSeconds(hitAnimeDuration);
    animator.SetBool("isHit", false);
    isHit = false;
    yield return new WaitForSeconds(invincibleDuration - hitAnimeDuration); // anim 시간 이후 남은 무적시간 지속
    isInvincible = false;
    }
```

|충돌 모션|
|:---:|
|![Bump](https://github.com/user-attachments/assets/9bdb6c98-4014-4bd1-bfb2-1786075a8303)|

<br>

    
### 사망 처리 & 결과창 이동
- Hp가 0이 되면 사망하며 결과창으로 이동합니다.

<br>

```
     public void Die()
    {
     if (isDie) return;
     isDie = true;
     currentHp = 0;
     _rigidbody2D.velocity = Vector2.zero;

     Debug.Log("Player Die");
     animator.SetTrigger("isDie");
     _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce * 1.5f);
     //애니메이션 추가 예정? 혹은 바로 결과창?
     sfxController?.PlayDieSFX();
     GameManager.instance.EndGame();
    }
```
|사망 & 결과창 전환|
|:---:|
|![DieResult](https://github.com/user-attachments/assets/c76ea744-dc1f-4c46-84d6-6045c1dd323d)|

<br>

### 배경 무한 스크롤
- 머티리얼을 이용해 오프셋x갑3ㅅ 변경으로 무한으로 이어지는것 처럼 보여지는 백그라운드 로직

```
private Renderer rend;
float offsetX = 0f;
void Start()
{
    rend = GetComponent<Renderer>();
}
void Update()
{
    offsetX += GameManager.instance.groundSpeed * 0.01f * Time.deltaTime;
    rend.material.mainTextureOffset = new Vector2(offsetX, 0);
}
```
|배경 무한 스크롤|
|:---:|
|![배경 무한 스크롤](https://github.com/user-attachments/assets/4f52e4c2-8b5d-4109-8716-faf089917613)|


<br>

### 맵 오브젝트 폴링
- 배열 안에 프리팹을 넣고 생성, 비활성화 후
- 특정 조건 시 리스트에 있는 프리팹을 랜덤으로 활성화

```
  using UnityEngine;
using System.Collections.Generic;
public class MapManager : MonoBehaviour
{
    public GameObject[] mapPrefabs;
    private List<GameObject> mapPool = new List<GameObject>();
    private float spawnX = 60f;
    private GameObject currentMap;
    void Start()
    {
        InitializePool();
        ActivateRandomMap();
    }
    void Update()
    {
        if (currentMap != null && currentMap.transform.position.x <= 0f)
        {
            ActivateRandomMap();
        }
    }
    void InitializePool()
    {
        for (int i = 0; i < mapPrefabs.Length; i++)
        {
            GameObject obj = Instantiate(mapPrefabs[i], transform);
            obj.SetActive(false);
            mapPool.Add(obj);
        }
    }
    void ActivateRandomMap()
    {
        GameObject map = GetInactiveMap();
        if (map != null)
        {
            map.transform.localPosition = new Vector3(spawnX, 0, 0);
            map.SetActive(true);
            currentMap = map;
        }
    }
    GameObject GetInactiveMap()
    {
        List<GameObject> inactiveMaps = new List<GameObject>();
        foreach (GameObject obj in mapPool)
        {
            if (!obj.activeInHierarchy)
                inactiveMaps.Add(obj);
        }
        if (inactiveMaps.Count > 0)
        {
            int randIndex = Random.Range(0, inactiveMaps.Count);
            return inactiveMaps[randIndex];
        }
        return null;
    }
}

  ```

-----

- 타일맵의 스프라이트 이름을 가져와 효과를 적용시키고 해당 위치의 타일맵을 삭제하는 로직

```
  private void OnTriggerStay2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        if (GameManager.instance.IsPlaying)
        {
            Bounds bounds = other.bounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            for (float x = min.x; x <= max.x; x += 0.3f)
            {
                for (float y = min.y; y <= max.y; y += 0.3f)
                {
                    Vector3Int cellPos = itemTilemap.WorldToCell(new Vector3(x, y, 0));
                    TileBase tile = itemTilemap.GetTile(cellPos);
                    if (tile == null) continue;
                    string tileName = tile.name;
                    switch (tileName)
                    {
                        case "gem_blue":
                            GameManager.instance.AddScore(1000);
                            break;
                        case "gem_red":
                            GameManager.instance.AddScore(5000);
                            break;
                        case "gem_green":
                            GameManager.instance.AddScore(10000);
                            break;
                        case "gem_yellow":
                            GameManager.instance.AddScore(15000);
                            break;
                        case "conveyor":
                            GameManager.instance.BoostSpeed(4f, 2f);
                            break;
                        case "hud_heart":
                            other.GetComponent<PlayerMove>().Heal(10);
                            break;
                    }
                    sfx?.PlayItemSound();
                    itemTilemap.SetTile(cellPos, null);
                }
            }
        }
    }
}

  ```

|오브젝트 풀링|
|:---:|
|![20251104-1102-11 0874801](https://github.com/user-attachments/assets/18cde563-b8fc-41d2-b719-59a4d8d98741)|

<br>

### UI 버튼 공통 컨트롤러
- 모든 UI 컨트롤러에서 **상속받는 부모 클래스**
- 버튼을 등록하고, 씬 전환 시 자동으로 리스너를 해체하도록 구현되어 이벤트 중복 실행이나 누락을 방지

```
public abstract class BaseUIButtonController : MonoBehaviour
{
    // 버튼과 그 버튼이 눌렸을 때 실행할 Action을 저장하는 클래스
    private class ButtonListener
    {
        public Button button;
        public UnityEngine.Events.UnityAction action;
    }
    private readonly List<ButtonListener> buttonListeners = new List<ButtonListener>();
    // 버튼을 안전하게 등록하는 함수
    protected void RegisterButton(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
        {
            //Debug.LogWarning($"{name}: 등록하려는 버튼이 null입니다!");
            return;
        }
        button.onClick.AddListener(action);
        buttonListeners.Add(new ButtonListener { button = button, action = action });
    }
    // 모든 등록된 버튼 리스너를 해제
    protected virtual void OnDestroy()
    {
        foreach (var listener in buttonListeners)
        {
            if (listener.button != null)
                listener.button.onClick.RemoveListener(listener.action);
        }
        buttonListeners.Clear();
    }
}
```

----

## ⚙️ 개발 환경 및 기술 스택
- **엔진** : Unity 2022.3.62f2
- **언어** : C#
- **버전 관리** : Git / GitHub
- **협업 툴** : ZEP / Slack
- **플랫폼** : PC,Mobile

  <br>
  <br>
