using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedPOI
{
    public PointOfInterest prefab;
    [Range(0.1f, 10f)]
    public float weight = 1f;
}

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private YutnoriCameraController cameraController;

    // 맵 오브젝트들이 들어갈 부모 Transform
    [SerializeField] private Transform boardContainer;

    [SerializeField]
    private List<WeightedPOI> weightedPointsOfInterestPrefabs = new();

    [SerializeField] private PointOfInterest startPOI;
    [SerializeField] private PointOfInterest endPOI;
    [SerializeField] private PointOfInterest shortcutPOI;

    private PointOfInterest startingPoint; // 플레이어 말들에 전달해줄 시작점 (startPOI 객체)

    [SerializeField] private GameObject pathPrefab; // 경로(라인) 프리팹

    [SerializeField] private int numberOfStartingPoints = 1; // 시작 지점의 개수
    [SerializeField] private int mapLength = 16; // 맵의 세로 길이(층 수)
    [SerializeField] private int maxWidth = 5; // 맵의 가로 최대 폭
    [SerializeField] private float xMaxSize; // 맵의 가로 최대 크기
    [SerializeField] private float yPadding; // 층 간 y축 간격

    // 경로가 교차하는 것을 허용할지 여부
    [SerializeField] private bool allowCrisscrossing;
    
    [Range(0.1f, 1f), SerializeField] private float chancePathMiddle; // 중간 경로 생성 확률
    [Range(0f, 1f), SerializeField] private float chancePathSide; // 양옆 경로 생성 확률

    [SerializeField, Range(0.9f, 5f)] private float multiplicativeSpaceBetweenLines = 2.5f; // 라인 간격에 곱해지는 값
    [SerializeField, Range(1f, 5.5f)] private float multiplicativeNumberOfMinimunConnections = 3f; // 최소 연결 개수에 곱해지는 값

    private PointOfInterest[][] _pointOfInterestsMap; // 행,열 정보 포함 전체 poi 
    private int _numberOfConnections = 0; // 현재까지 생성된 연결(라인) 개수
    private float _lineLength;
    private float _lineHeight;



    private void Awake()
    {
        RecreateBoard();
    }

    // 1. 초기화 -------------------------------------------------------------------
    public void RecreateBoard()
    {
        // 라인 프리팹의 길이와 높이 계산
        _lineLength = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * pathPrefab.transform.localScale.z;
        _lineHeight = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * pathPrefab.transform.localScale.y;

        // 기존 맵 오브젝트 삭제
        DestroyImmediateAllChildren(boardContainer);

        // 초기화
        _numberOfConnections = 0; // 연결 개수

        _pointOfInterestsMap = new PointOfInterest[mapLength][]; // 층별 POI 리스트
        for (int i = 0; i < _pointOfInterestsMap.Length; i++)
        {
            _pointOfInterestsMap[i] = new PointOfInterest[maxWidth];
        }

        // 맵 생성
        CreateMap();
        // 시작점 저장. 시작점은 언제나 0층 첫 번째 행에 생성됨
        startingPoint = _pointOfInterestsMap[0][1]; 
        // 생성한 맵 기준으로 드래그 범위 설정
        cameraController.UpdateClampBounds();
    }
    
    // 2. 맵 생성 -------------------------------------------------------------------
    private void CreateMap()
    {
        // 시작점 생성 (첫 층)
        //List<int> positions = GetRandomIndexes(numberOfStartingPoints);
        //foreach (int j in positions)
        //{
        //    _ = InstantiatePointOfInterest(0, j);
        //}

        InstantiatePointOfInterest(0, 1);

        // 연결 개수 검증 - 최소한의 경로 복잡도와 경로 간 연결점 보장하려고 넣음
        if (_numberOfConnections <= mapLength * multiplicativeNumberOfMinimunConnections)
        {
            Debug.Log($"Recreating board with {_numberOfConnections} connections");
            RecreateBoard();
            return;
        }
    }

    // 3. POI 생성 및 다음 노드와의 연결 ----------------------------------------
    private PointOfInterest InstantiatePointOfInterest(int floor, int column)
    {
        // 이미 해당 위치에 POI가 있으면 그 POI를 반환
        if (_pointOfInterestsMap[floor][column] != null)
        {
            return _pointOfInterestsMap[floor][column];
        }

        // 3.1 POI 생성 --------------------------------------
        // POI 종류 선택, 프리팹 형태로 리턴
        PointOfInterest randomPOI = GetWeightedRandomPOI(floor);

        // 인스턴스 생성, poi 목록에 저장
        PointOfInterest instance = Instantiate(randomPOI, boardContainer);
        _pointOfInterestsMap[floor][column] = instance;

        // 화면상 위치 계산, 지정
        Vector3 pos = GetPositionVector(floor, column);
        instance.transform.localPosition = pos;

        // 3.2 다음 POI 생성 ---------------------------------
        // 내 위에 연결된 노드 없고, 이게 마지막 층 아니라면 노드 생성 시도 
        while (instance.NextPointsOfInterest.Count == 0 && floor < mapLength - 1)
        {
            // 왼쪽 대각선 연결 시도
            if (column > 0 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsMap[floor + 1][column] == null)
                {
                    InstantiateNextPoint(floor + 1, column - 1);
                }
            }

            // 오른쪽 대각선 연결 시도
            if (column < maxWidth - 1 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsMap[floor + 1][column] == null)
                {
                    InstantiateNextPoint(floor + 1, column + 1);
                }
            }

            // 직진 연결 시도
            if (Random.Range(0f, 1f) < chancePathMiddle)
            {
                InstantiateNextPoint(floor + 1, column);
            }
        }

        // 다음 층의 PointOfInterest를 생성하고 연결하는 로컬 함수
        // 로컬 함수: 이 함수 내부에서만 쓸 경우 내부에 생성. 상위 함수와 강하게 결합된 헬퍼 함수를 만들 때 매우 적합. 
        void InstantiateNextPoint(int index_i, int index_j)
        {
            PointOfInterest nextPOI = InstantiatePointOfInterest(index_i, index_j);
            AddLineBetweenPoints(instance, nextPOI);

            // 이미 안 들어가있다면 이후/이전 노드 목록 갱신 
            if (!instance.NextPointsOfInterest.Contains(nextPOI))
                instance.NextPointsOfInterest.Add(nextPOI);

            _numberOfConnections++;
        }

        return instance;
    }

    // 가중치에 따라 노드 종류 결정, 5번째 층엔 shortcutPOI로 고정. POI Prefab 반환
    private PointOfInterest GetWeightedRandomPOI(int currentFloor)
    {
        if (currentFloor == 0) return startPOI;

        if (currentFloor == mapLength - 1) return endPOI;

        // 5번째 층일 경우 shortcutPOI 강제 반환
        if ((currentFloor + 1) % 5 == 0)
        {
            return shortcutPOI;
        }

        // 나머지는 가중치에 따른 랜덤 선정
        if (weightedPointsOfInterestPrefabs.Count == 0)
            throw new System.Exception("프리팹 리스트가 비어있습니다!");

        float totalWeight = 0f;
        foreach (var wpoi in weightedPointsOfInterestPrefabs)
            totalWeight += wpoi.weight;

        if (totalWeight <= Mathf.Epsilon)
            return weightedPointsOfInterestPrefabs[Random.Range(0, weightedPointsOfInterestPrefabs.Count)].prefab;

        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        foreach (var wpoi in weightedPointsOfInterestPrefabs)
        {
            cumulative += wpoi.weight;
            if (randomValue <= cumulative)
                return wpoi.prefab;
        }

        return weightedPointsOfInterestPrefabs[0].prefab;
    }

    // 두 POI 사이 라인(경로) 생성
    private void AddLineBetweenPoints(PointOfInterest thisPoint, PointOfInterest nextPoint)
    {
        float len = _lineLength;
        float height = _lineHeight;

        // 두 점 사이의 방향 벡터 계산
        Vector3 dir = (nextPoint.transform.position - thisPoint.transform.position).normalized;

        // 두 점 사이의 거리 계산
        float dist = Vector3.Distance(thisPoint.transform.position, nextPoint.transform.position);
            
        // 두 점 사이에 들어갈 라인 개수 계산 (패딩 포함)
        int num = (int)(dist / (len * multiplicativeSpaceBetweenLines));

        // 실제 패딩 거리 계산 (num이 정수라서 남는 거리 분배)
        float pad = (dist - (num * len)) / (num + 1);

        // 첫 번째 라인의 위치 계산 (len/2f는 라인 중심)
        Vector3 pos_i = thisPoint.transform.position + (dir * (pad + (len / 2f)));

        // 모든 라인 배치
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = pos_i + ((len + pad) * i * dir);
            GameObject lineCreated = Instantiate(pathPrefab, pos, Quaternion.identity, boardContainer);
            // 라인이 다음 포인트를 바라보도록 회전
            lineCreated.transform.LookAt(nextPoint.transform);
            // 라인 높이만큼 아래로 내림 (중심 맞추기)
            lineCreated.transform.position -= Vector3.up * (height / 2f);
        }
    }

    // 행,열 정수로 받아서 화면 상 위치 계산 후 벡터로 반환. 
    private Vector3 GetPositionVector(int floor, int column)
    {
        // x축 한 칸의 크기 계산
        float xSize = xMaxSize / maxWidth;
        // x, y 위치 계산
        float xPos = (xSize * column) + (xSize / 2f);
        float yPos = yPadding * floor;

        // 첫 층이면 위치 고정
        if(floor == 0)
        {
            xPos = xMaxSize / 2;
            Vector3 startPos = new Vector3(xPos, 0, yPos);
            Debug.Log("Start pos" +  startPos);
            return startPos;
        }

        // 위치에 랜덤 패딩 추가 - 격자에 딱딱 들어맞는 기계적인 느낌의 노드 배치 피하기
        xPos += Random.Range(-xSize / 4f, xSize / 4f);
        yPos += Random.Range(-yPadding / 4f, yPadding / 4f);

        // 실제 위치 벡터 생성
        Vector3 pos = new Vector3(xPos, 0, yPos);

        return pos;
    }

    // Transform의 모든 자식 오브젝트를 즉시 삭제하는 함수
    private void DestroyImmediateAllChildren(Transform transform)
    {
        List<Transform> toKill = new();

        // 삭제할 자식 Transform 리스트에 추가
        foreach (Transform child in transform)
        {
            toKill.Add(child);
        }

        // 뒤에서부터 삭제 (안전하게)
        for (int i = toKill.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(toKill[i].gameObject);
        }
    }

    // 카메라 스크롤에 쓸 맵 높이 구하기
    public Vector2 GetZBounds()
    {
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        if (_pointOfInterestsMap == null) return new Vector2(0, 0);

        foreach (var poiRow in _pointOfInterestsMap)
        {
            if (poiRow == null) continue;
            foreach (var poi in poiRow)
            {
                if (poi == null) continue;
                float z = poi.transform.position.z;
                if (z < minZ) minZ = z;
                if (z > maxZ) maxZ = z;
            }
        }

        // POI가 하나도 없을 경우 예외 처리
        if (minZ == float.MaxValue || maxZ == float.MinValue)
            return new Vector2(0, 0);

        return new Vector2(minZ, maxZ);
    }

    public PointOfInterest getStartingPoint()
    {
        return startingPoint;
    }
}
