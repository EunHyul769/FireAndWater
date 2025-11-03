using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    // Prefab_MapRan들 참조를 위한 List_prefabList 생성 및 초기화
    public List<GameObject> prefabList = new List<GameObject>();
    // Prefab_MapFinish 참조를 위한 GameObject_mapFinish 생성
    public GameObject mapFinish;

    // Transform_MapRan 참조를 위한 mapRanParent 생성
    public Transform mapRanParent;

    // 맵에 배치할 Prefab_MapRan들의 개수 정의 (외부에서 값 수정 가능)
    public int spawnCount = 1;
    // 맵에 배치할 Prefab들의 간격 정의 (외부에서 값 수정 가능)
    public float positionX = 50f;

    void Start()
    {
        // MapRanParent가 비어있을 경우
        if (mapRanParent == null)
        {
            // LogWarning 출력 - "MapRanParent is Null"
            Debug.LogWarning("MapRanParent is Null");
            // 반환 (아래 코드 무시)
            return;
        }

        // MapFinish가 비어있을 경우
        if (mapFinish == null)
        {
            // LogWarning 출력 - "MapFinish is Null"
            Debug.LogWarning("MapFinish is Null");
            // 반환 (아래 코드 무시)
            return;
        }

        // 메서드 - Map 생성 호출
        SpawnMaps();
    }

    // 메서드(알고리즘) - 'Fisher-Yates Shuffle' - List 형식의 매개변수 참조 후 List_list 정의
    public static List<T> FisherYatesShuffleUnity<T>(List<T> list)
    {
        // 반복문 - 참조한 list 내 값들의 개수 -1 만큼
        for (int i = list.Count - 1; i > 0; i--)
        {
            // 임의의 값 생성 및 초기화 - 참조한 list 내 값들의 개수 중 랜덤 값 추출
            int j = Random.Range(0, i + 1);

            // 캐시 값 생성 및 초기화_참조한 list 내 i번 째 값
            T temp = list[i];
            // 참조한 list 내 i번 째 값 초기화_참조한 list 내 추출한 랜덤 값번 째 값
            list[i] = list[j];
            // 참조한 list 내 추출한 랜덤 값번 째 값 초기화_캐시 값
            list[j] = temp;
        }
        // 위 반복문에서 추출되지 않은 값은 참조한 list의 마지막에 배치됨

        // 반환 - 참조한 list로
        return list;
    }

    // 메서드 - Map 생성
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
}
