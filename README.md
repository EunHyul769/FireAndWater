10조 유니티 입문 팀 과제
--

레퍼런스 게임 : Fire&Water<br>
게임 소개 : 2인 협동 2D 퍼즐 게임으로 기믹을 이용해 목표에 도달하는 게임<br>


## 게임 소개
(타이틀이미지)<br>

- 좌우 이동과 점프를 이용해 목표에 도달
- 버튼, 레버, 열쇠와 같은 요소를 이용해 함정을 피하거나 길을 만들어 진행
- 클리어 시간과 수집 아이템을 통해 클리어 시 스테이지 별 점수 획득
<br>


## 팀원 및 역할분담
	
김지훈	중간관리자(전반적 수정&조율)+타이머+씬 백업+bgm<br>
김경찬	플레이어<br>
김주원	오브젝트<br>
김해종	씬 전환+a<br>
전규태	맵 제작<br>


## 기능 설명 
<br>

### 플레이어 이동과 점프

<img width="256" height="184" alt="화면 캡처 2025-11-05 091815" src="https://github.com/user-attachments/assets/ce6158d4-c794-4960-aafb-403c86edc95d" />


각 플레이어는 키보드 w,a,d 와 ▲,◄,► 입력으로 이동 가능하도록 인스펙터에서 설정<br>
플레이어는 ground 레이어인 콜라이더에 위에서만 점프가 가능하도록 하여 중복해서 일어나지 않도록 함<br>

PlayerController.cs
<br>

    private void Move()
    {
        float moveDir = 0;
        if (Input.GetKey(leftKey)) moveDir = -1;
        if (Input.GetKey(rightKey)) moveDir = 1;

        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        if (moveDir > 0 && !facingRight)
            Flip(true);
        else if (moveDir < 0 && facingRight)
            Flip(false);

        animationHandler?.Move(rb.velocity);
    }

    private void Flip(bool faceRight)
    {
        facingRight = faceRight;
        transform.rotation = faceRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    private void Jump()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animationHandler?.Jump(rb.velocity);
        }

        if (!wasGrounded && isGrounded)
            animationHandler?.JumpEnd();
    }

### 상호작용 오브젝트 관리

<img width="369" height="216" alt="화면 캡처 2025-11-05 091423" src="https://github.com/user-attachments/assets/a904c2dd-ab12-429b-8307-6a835e263fdd" />


 플레이어가 상호작용 가능한 오브젝트(버튼, 레버, 수집 아이템 등)은 Interativeobject를 상속 받도록 구성<br>
 트리거 Enter/Exit 시 충돌 중인 플레이어 수를 확인하여 상호작용 결정<br>
 
<br>

    public virtual void Interact()
    {
    }
    public virtual void InteractOut()
    {
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        InteractPlayerNum++;
        Debug.Log($"{this.name} nearby {InteractPlayerNum}OB");

        if(Type == ObjectType.Buton)
        {
            Interact();
        }
        if(Type == ObjectType.Gate)
        {
            Interact();
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        InteractPlayerNum--;
        if (InteractPlayerNum <= 0 && Type == ObjectType.Buton)
        {
            InteractOut();
        }
        if (Type == ObjectType.Gate)
        {
            InteractOut();
        }
    }
	
<br>

### 스테이지 클리어 UI
스테이지 씬 로드 시 MainUI 씬 또한 로드<br>
스테이지 클리어 시 소요 시간, 점수가 포함된 UI 표시(활성화)<br>

<img width="807" height="455" alt="화면 캡처 2025-11-05 091910" src="https://github.com/user-attachments/assets/6a12a646-1eac-459e-bc64-57a35dc8cf1f" />


GameManager.cs
<br>

```
    public void StageClear()
    {
        Debug.Log($"{currentScene.name} clear!!!");

        //타이머 정지, 클탐 계산
        TimeManager.Instance.StopTimer();
        float clearTime = TimeManager.Instance.GetElapsedTime();
        PlayerPrefs.SetFloat("ClearTime", clearTime);

        //현재 스테이지 이름 기록
        lastStageName = SceneManager.GetActiveScene().name;

        //스테이지클리어ui매니저 찾아서 표시
        StageClearUIManager clearUI = FindObjectOfType<StageClearUIManager>();
        if (clearUI != null)
        {
            clearUI.ShowStageClearUI(clearTime);
        }
        else
        {
            Debug.LogWarning("StageClearUIManager를 찾을 수 없습니다. MainUI 씬이 로드되어 있는지 확인하세요.");
        }

    }
```
<br>

### 히든 맵 랜던 구성

<img width="985" height="361" alt="화면 캡처 2025-11-05 093046" src="https://github.com/user-attachments/assets/6845fb10-d1bd-4027-b0b9-90829f364bd5" />


히든 맵의 장애물 구간을 프리팹으로 나누어 관리, 랜덤하게 생성, 조합하여 스테이지 구현<br>

```

    void SpawnMaps()
    {
        // prefabList가 비어있거나, prefabList에 값이 존재하지 않을 경우
        if (prefabList == null || prefabList.Count == 0)
        {
            // LogWarning 출력 - "PrefabList is Null"
            Debug.LogWarning("PrefabList is Null");
            // 반환 (아래 코드 무시)
            return;
        }

        // List_shuffled 생성 및 초기화 - prefabList 값 참조   <<-- 원본 List를 섞을 경우 원본의 순서가 깨질 수 있기에 복제본을 생성함
        List<GameObject> shuffled = new List<GameObject>(prefabList);
        // shuffled 초기화 - 메서드(알고리즘)_'Fisher-Yates Shuffle'
        shuffled = FisherYatesShuffleUnity(shuffled);

        // 실제 배치할 개수 생성 및 초기화 - 최솟값_배치할 개수 ~ shuffled 개수   <<-- 배치할 개수보다 shuffled 값이 더 작을 수 있기에 최솟값 연산 후 사용
        int count = Mathf.Min(spawnCount, shuffled.Count);
        // 마지막 위치 값_X 생성 및 초기화
        float lastPosX = 0f;

        // 반복문 - 실제 배치할 개수만큼
        for (int i = 0; i < count; i++)
        {
            // 마지막 위치 값_X 초기화 - 배치 간격 + i * 배치 간격   <<-- 최초 시작하는 위치에는 MapStart가 배치되기에 2번 째 위치부터 배치
            lastPosX = positionX + i * positionX;

            // 위치 값 정의 - MapRan의 위치 값 기반, X 값에만 마지막 위치 값_X 연산
            Vector3 pos1 = new Vector3
                (
                mapRanParent.transform.position.x + lastPosX,
                mapRanParent.transform.position.y,
                mapRanParent.transform.position.z
                );
            // GameObject_obj1 생성 - shuffled 내 i번째 값, 위치 값, 회전 값(기본), obj1을 생성에 참고할 위치
            GameObject obj1 = Instantiate(shuffled[i], pos1, Quaternion.identity, mapRanParent);
            // obj1 이름 초기화
            obj1.name = $"Clone_{shuffled[i].name}_{i + 1}";
        }

        // 도착지점 위치 값 정의 - MapRan의 위치 값 기반, X 값에만 마지막 위치 값_X + 배치 간격 연산   <<-- 실제 배치된 개수 다음 위치에 생성하기 위한 연산
        Vector3 finishPos = new Vector3
            (
            mapRanParent.transform.position.x + lastPosX + positionX, 
            mapRanParent.transform.position.y,
            mapRanParent.transform.position.z
            );
        // GameObject_obj2 생성 - mapFinish, 도착지점 위치 값, 회전 값(기본), obj2를 생성에 참고할 위치
        GameObject obj2 = Instantiate(mapFinish, finishPos, Quaternion.identity, mapRanParent);
        // obj2 이름 초기화
        obj2.name = $"Clone_{mapFinish.name}";
    }

```
