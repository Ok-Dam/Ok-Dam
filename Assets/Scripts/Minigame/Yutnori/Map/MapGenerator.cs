using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedPOI
{
    public PointOfInterest prefab;
    [Range(0.1f, 10f)]
    public float weight = 1f;
}
public class ColumnInfo
{
    public bool hasComponentPOI; // 컴포넌트 POI 존재 여부
    public int lowestRowIndex;   // 교체 가능한 최저 층
}

// 맵을 생성하는 클래스
public class MapGenerator : MonoBehaviour
{
    // 맵 오브젝트들이 들어갈 부모 Transform
    [SerializeField] private Transform boardContainer;

    [SerializeField]
    private List<WeightedPOI> weightedPointsOfInterestPrefabs = new();

    private ColumnInfo[] _columnStatus; // 컬럼별 상태 저장
    [SerializeField]
    private PointOfInterest componentPOI; // 필수 생성 POI 프리팹 (인스펙터 할당)
    [SerializeField]
    private PointOfInterest shortcutPOI;

    // 경로(라인) 프리팹
    [SerializeField] private GameObject pathPrefab;
    // 시작 지점의 개수
    [SerializeField] private int numberOfStartingPoints = 4;
    // 맵의 세로 길이(층 수)
    [SerializeField] private int mapLength = 10;
    // 맵의 가로 최대 폭
    [SerializeField] private int maxWidth = 5;
    // 맵의 가로 최대 크기
    [SerializeField] private float xMaxSize;
    // 층 간 y축 간격
    [SerializeField] private float yPadding;
    // 경로가 교차하는 것을 허용할지 여부
    [SerializeField] private bool allowCrisscrossing;
    // 중간 경로 생성 확률
    [Range(0.1f, 1f), SerializeField] private float chancePathMiddle;
    // 양옆 경로 생성 확률
    [Range(0f, 1f), SerializeField] private float chancePathSide;
    // 라인 간격에 곱해지는 값
    [SerializeField, Range(0.9f, 5f)] private float multiplicativeSpaceBetweenLines = 2.5f;
    // 최소 연결 개수에 곱해지는 값
    [SerializeField, Range(1f, 5.5f)] private float multiplicativeNumberOfMinimunConnections = 3f;

    // 각 층별 PointOfInterest 2차원 배열
    private PointOfInterest[][] _pointOfInterestsPerFloor;
    // 생성된 PointOfInterest 리스트
    private readonly List<PointOfInterest> pointsOfInterest = new();
    // 현재까지 생성된 연결(라인) 개수
    private int _numberOfConnections = 0;
    // 라인(경로) 길이
    private float _lineLength;
    // 라인(경로) 높이
    private float _lineHeight;

    // 게임 시작 시 맵 생성
    private void Start()
    {
        RecreateBoard();
    }

    // 맵을 새로 생성하는 함수
    public void RecreateBoard()
    {
        // 라인 프리팹의 실제 길이와 높이 계산
        _lineLength = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * pathPrefab.transform.localScale.z;
        _lineHeight = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * pathPrefab.transform.localScale.y;
        // 기존 맵 오브젝트 모두 삭제
        DestroyImmediateAllChildren(boardContainer);
        // 연결 개수 초기화
        _numberOfConnections = 0;
        // 랜덤 시드 초기화
        GenerateRandomSeed();
        // 기존 PointOfInterest 리스트 초기화
        pointsOfInterest.Clear();
        // 층별 PointOfInterest 배열 초기화
        _pointOfInterestsPerFloor = new PointOfInterest[mapLength][];
        for (int i = 0; i < _pointOfInterestsPerFloor.Length; i++)
        {
            _pointOfInterestsPerFloor[i] = new PointOfInterest[maxWidth];
        }

        // 컬럼 상태 초기화 추가
        _columnStatus = new ColumnInfo[maxWidth];
        for (int i = 0; i < maxWidth; i++)
            _columnStatus[i] = new ColumnInfo { lowestRowIndex = -1 };

        // 맵 생성
        CreateMap();
    }

    // 랜덤 시드 생성 함수
    private void GenerateRandomSeed()
    {
        int tempSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }

    // PointOfInterest를 생성하고, 다음 층의 연결을 재귀적으로 생성하는 함수
    private PointOfInterest InstantiatePointOfInterest(int floorN, int xNum)
    {

        if ((floorN + 1) % 5 == 0)
            return _pointOfInterestsPerFloor[floorN][xNum];

        // 이미 해당 위치에 PointOfInterest가 있으면 반환
        if (_pointOfInterestsPerFloor[floorN][xNum] != null)
        {
            return _pointOfInterestsPerFloor[floorN][xNum];
        }

        // x축 한 칸의 크기 계산
        float xSize = xMaxSize / maxWidth;
        // x, y 위치 계산
        float xPos = (xSize * xNum) + (xSize / 2f);
        float yPos = yPadding * floorN;

        // 위치에 랜덤 패딩 추가
        xPos += Random.Range(-xSize / 4f, xSize / 4f);
        yPos += Random.Range(-yPadding / 4f, yPadding / 4f);

        // 실제 위치 벡터 생성
        Vector3 pos = new Vector3(xPos, 0, yPos);
        // 랜덤 PointOfInterest 프리팹 선택
        //PointOfInterest randomPOI = pointsOfInterestPrefabs[Random.Range(0, pointsOfInterestPrefabs.Count)];
        PointOfInterest randomPOI = GetWeightedRandomPOI();

        // 인스턴스 생성 및 리스트에 추가
        PointOfInterest instance = Instantiate(randomPOI, boardContainer);
        pointsOfInterest.Add(instance);

        // 위치 지정
        instance.transform.localPosition = pos;
        // 층별 배열에 저장
        _pointOfInterestsPerFloor[floorN][xNum] = instance;
        // 생성된 연결 개수
        int created = 0;

        // 다음 층의 PointOfInterest를 생성하고 연결하는 내부 함수
        void InstantiateNextPoint(int index_i, int index_j)
        {
            PointOfInterest nextPOI = InstantiatePointOfInterest(index_i, index_j);
            AddLineBetweenPoints(instance, nextPOI);
            instance.NextPointsOfInterest.Add(nextPOI);
            created++;
            _numberOfConnections++;
        }

        // 연결이 하나도 생성되지 않았고, 마지막 층이 아니면 연결 시도
        while (created == 0 && floorN < mapLength - 1)
        {
            // 왼쪽 대각선 연결 시도
            if (xNum > 0 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsPerFloor[floorN + 1][xNum] == null)
                {
                    InstantiateNextPoint(floorN + 1, xNum - 1);
                }
            }

            // 오른쪽 대각선 연결 시도
            if (xNum < maxWidth - 1 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsPerFloor[floorN + 1][xNum] == null)
                {
                    InstantiateNextPoint(floorN + 1, xNum + 1);
                }
            }

            // 직진 연결 시도
            if (Random.Range(0f, 1f) < chancePathMiddle)
            {
                InstantiateNextPoint(floorN + 1, xNum);
            }

            if (randomPOI == componentPOI)
                _columnStatus[xNum].hasComponentPOI = true;

            if (_columnStatus[xNum].lowestRowIndex < floorN)
                _columnStatus[xNum].lowestRowIndex = floorN;

        }

        return instance;
    }

    // 맵을 실제로 생성하는 함수
    private void CreateMap()
    {
        // 컬럼 상태 초기화
        _columnStatus = new ColumnInfo[maxWidth];
        for (int i = 0; i < maxWidth; i++)
            _columnStatus[i] = new ColumnInfo { lowestRowIndex = -1 };

        // 1. 규칙 층(5번째 행)에 Shortcut POI 생성 ----------------------------
        for (int floor = 0; floor < mapLength; floor++)
        {
            if ((floor + 1) % 5 == 0) // 1-based 5번째 층 (0-based: 4,9,14...)
            {
                for (int col = 0; col < maxWidth; col++)
                {
                    Vector3 pos = CalculatePosition(floor, col);
                    PointOfInterest instance = Instantiate(shortcutPOI, boardContainer);
                    instance.transform.localPosition = pos;
                    _pointOfInterestsPerFloor[floor][col] = instance;
                    pointsOfInterest.Add(instance);
                }
            }
        }

        // 2. 기존 시작점 생성 로직 -------------------------------------------
        List<int> positions = GetRandomIndexes(numberOfStartingPoints);
        foreach (int j in positions)
        {
            _ = InstantiatePointOfInterest(0, j);
        }

        // 3. 규칙 층 생성 후 연결 로직 추가 ----------------------------------
        for (int floor = 0; floor < mapLength; floor++)
        {
            if ((floor + 1) % 5 == 0 && floor < mapLength - 1) // 마지막 층 제외
            {
                for (int col = 0; col < maxWidth; col++)
                {
                    // 이미 생성된 Shortcut POI에서 다음 층으로 연결 생성
                    CreateConnectionsFromShortcut(floor, col);
                }
            }
        }

        // 4. 컴포넌트 POI 강제 생성 ------------------------------------------
        CheckAndEnforceComponentPOI();

        // 5. 연결 개수 검증 -------------------------------------------------
        if (_numberOfConnections <= mapLength * multiplicativeNumberOfMinimunConnections)
        {
            Debug.Log($"Recreating board with {_numberOfConnections} connections");
            RecreateBoard();
            return;
        }

        Debug.Log($"Created board with {_numberOfConnections} connections");
        Debug.Log($"Created board with {pointsOfInterest.Count} points");
    }

    private void CreateConnectionsFromShortcut(int floor, int col)
    {
        PointOfInterest shortcutNode = _pointOfInterestsPerFloor[floor][col];
        if (shortcutNode == null) return;

        // 생성된 연결 카운트
        int created = 0;

        // 중간, 왼쪽, 오른쪽 연결 시도 (기존 확률 적용)
        if (col > 0 && Random.Range(0f, 1f) < chancePathSide)
        {
            if (allowCrisscrossing || _pointOfInterestsPerFloor[floor + 1][col] == null)
            {
                CreateConnectionToNextFloor(shortcutNode, floor, col, col - 1);
                created++;
            }
        }

        if (col < maxWidth - 1 && Random.Range(0f, 1f) < chancePathSide)
        {
            if (allowCrisscrossing || _pointOfInterestsPerFloor[floor + 1][col] == null)
            {
                CreateConnectionToNextFloor(shortcutNode, floor, col, col + 1);
                created++;
            }
        }

        // 직진 연결 시도 (상황에 따라 강제 연결)
        if (created == 0 || Random.Range(0f, 1f) < chancePathMiddle)
        {
            CreateConnectionToNextFloor(shortcutNode, floor, col, col);
        }
    }

    private void CreateConnectionToNextFloor(PointOfInterest source, int currentFloor, int currentCol, int nextCol)
    {
        // 다음 층의 POI 생성 또는 가져오기
        PointOfInterest nextPOI = InstantiatePointOfInterest(currentFloor + 1, nextCol);

        // 시각적 연결선 추가
        AddLineBetweenPoints(source, nextPOI);

        // 논리적 연결 관계 설정
        source.NextPointsOfInterest.Add(nextPOI);

        // 연결 카운트 증가
        _numberOfConnections++;

        // 컬럼 상태 업데이트 (다음 층 노드가 컴포넌트인 경우)
        if (nextPOI != null && nextPOI.GetType() == componentPOI.GetType())
        {
            _columnStatus[nextCol].hasComponentPOI = true;
        }

        // 최저 층 인덱스 업데이트
        if (_columnStatus[nextCol].lowestRowIndex < currentFloor + 1)
        {
            _columnStatus[nextCol].lowestRowIndex = currentFloor + 1;
        }
    }


    private void CheckAndEnforceComponentPOI()
    {
        for (int col = 0; col < maxWidth; col++)
        {
            if (!_columnStatus[col].hasComponentPOI)
            {
                if (_columnStatus[col].lowestRowIndex == -1)
                {
                    Debug.LogError($"컬럼 {col}에 POI가 없어 컴포넌트 생성 불가!");
                    continue;
                }
                ReplacePOIWithComponent(_columnStatus[col].lowestRowIndex, col);
            }
        }
    }

    private Vector3 CalculatePosition(int floorN, int xNum)
    {
        float xSize = xMaxSize / maxWidth;
        float xPos = (xSize * xNum) + (xSize / 2f);
        float yPos = yPadding * floorN;

        xPos += Random.Range(-xSize / 4f, xSize / 4f);
        yPos += Random.Range(-yPadding / 4f, yPadding / 4f);

        return new Vector3(xPos, 0, yPos);
    }

    private void ReplacePOIWithComponent(int floor, int col)
    {
        // 기존 POI 제거
        PointOfInterest oldPOI = _pointOfInterestsPerFloor[floor][col];
        if (oldPOI != null)
        {
            pointsOfInterest.Remove(oldPOI);
            DestroyImmediate(oldPOI.gameObject);
        }

        // 컴포넌트 POI 생성
        Vector3 pos = CalculatePosition(floor, col);
        PointOfInterest instance = Instantiate(componentPOI, boardContainer);
        instance.transform.localPosition = pos;
        _pointOfInterestsPerFloor[floor][col] = instance;
        pointsOfInterest.Add(instance);

        // 연결 관계 업데이트
        foreach (var poi in pointsOfInterest)
        {
            poi.NextPointsOfInterest.RemoveAll(p => p == oldPOI);
            if (poi != instance)
                poi.NextPointsOfInterest.Add(instance);
        }
    }


    private PointOfInterest GetWeightedRandomPOI()
    {
        if (weightedPointsOfInterestPrefabs.Count == 0)
            throw new System.Exception("프리팹 리스트가 비어있습니다!");

        // 총 가중치 계산
        float totalWeight = 0f;
        foreach (var wpoi in weightedPointsOfInterestPrefabs)
            totalWeight += wpoi.weight;

        if (totalWeight <= Mathf.Epsilon) // 모든 가중치가 0인 경우
            return weightedPointsOfInterestPrefabs[Random.Range(0, weightedPointsOfInterestPrefabs.Count)].prefab;

        // 가중치 기반 선택
        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        foreach (var wpoi in weightedPointsOfInterestPrefabs)
        {
            cumulative += wpoi.weight;
            if (randomValue <= cumulative)
                return wpoi.prefab;
        }

        return weightedPointsOfInterestPrefabs[0].prefab; // fallback
    }


    // 두 PointOfInterest 사이에 라인(경로) 오브젝트를 생성하는 함수
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

    // 0~maxWidth-1 중 n개를 랜덤하게 뽑는 함수 (중복 없음)
    private List<int> GetRandomIndexes(int n)
    {
        List<int> indexes = new List<int>();
        if (n > maxWidth)
        {
            throw new System.Exception("Number of starting points greater than maxWidth!");
        }

        while (indexes.Count < n)
        {
            int randomNum = Random.Range(0, maxWidth);
            if (!indexes.Contains(randomNum))
            {
                indexes.Add(randomNum);
            }
        }
        return indexes;
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
}
