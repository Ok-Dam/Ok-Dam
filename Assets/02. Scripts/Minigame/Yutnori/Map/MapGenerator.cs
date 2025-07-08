using System.Collections.Generic;
using System.Linq;
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

    // �� ������Ʈ���� �� �θ� Transform
    [SerializeField] private Transform boardContainer;

    [SerializeField]
    private List<WeightedPOI> weightedPointsOfInterestPrefabs = new();

    [SerializeField] private PointOfInterest startPOI;
    [SerializeField] private PointOfInterest endPOI;
    [SerializeField] private PointOfInterest shortcutPOI;

    private PointOfInterest startingPoint; // �÷��̾� ���鿡 �������� ������ (startPOI ��ü)

    [SerializeField] private GameObject pathPrefab; // ���(����) ������
    [SerializeField] private int mapLength = 17; // ���� ���� ����(�� ��)
    [SerializeField] private int maxWidth = 5; // ���� ���� �ִ� ��
    [SerializeField] private float xMaxSize; // ���� ���� �ִ� ũ��
    [SerializeField] private float yPadding; // �� �� y�� ����

    // ��ΰ� �����ϴ� ���� ������� ����
    [SerializeField] private bool allowCrisscrossing;
    
    [Range(0.1f, 1f), SerializeField] private float chancePathMiddle; // �߰� ��� ���� Ȯ��
    [Range(0f, 1f), SerializeField] private float chancePathSide; // �翷 ��� ���� Ȯ��

    [SerializeField, Range(0.9f, 5f)] private float multiplicativeSpaceBetweenLines = 2.5f; // ���� ���ݿ� �������� ��
    [SerializeField, Range(1f, 5.5f)] private float multiplicativeNumberOfMinimunConnections = 3f; // �ּ� ���� ������ �������� ��

    private PointOfInterest[][] _pointOfInterestsMap; // ��,�� ���� ���� ��ü poi 
    private int _numberOfConnections = 0; // ������� ������ ����(����) ����
    private float _lineLength;
    private float _lineHeight;



    private void Awake()
    {
        RecreateBoard();
    }

    // 1. �ʱ�ȭ -------------------------------------------------------------------
    public void RecreateBoard()
    {
        // ���� �������� ���̿� ���� ���
        _lineLength = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * pathPrefab.transform.localScale.z;
        _lineHeight = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * pathPrefab.transform.localScale.y;

        // ���� �� ������Ʈ ����
        DestroyImmediateAllChildren(boardContainer);

        // �ʱ�ȭ
        _numberOfConnections = 0; // ���� ����

        _pointOfInterestsMap = new PointOfInterest[mapLength][]; // ���� POI ����Ʈ
        for (int i = 0; i < _pointOfInterestsMap.Length; i++)
        {
            _pointOfInterestsMap[i] = new PointOfInterest[maxWidth];
        }

        // �� ����
        CreateMap();
        // ������ ����. �������� ������ 0�� ù ��° �࿡ ������
        startingPoint = _pointOfInterestsMap[0][1]; 
        // ������ �� �������� �巡�� ���� ����
        cameraController.UpdateClampBounds();
    }
    
    // 2. �� ���� -------------------------------------------------------------------
    private void CreateMap()
    {
        // ������ ���� (ù ��)
        //List<int> positions = GetRandomIndexes(numberOfStartingPoints);
        //foreach (int j in positions)
        //{
        //    _ = InstantiatePointOfInterest(0, j);
        //}

        InstantiatePointOfInterest(0, 1);

        // ���� ���� ���� - �ּ����� ��� ���⵵�� ��� �� ������ �����Ϸ��� ����
        if (_numberOfConnections <= mapLength * multiplicativeNumberOfMinimunConnections)
        {
            Debug.Log($"Recreating board with {_numberOfConnections} connections");
            RecreateBoard();
            return;
        }
    }

    // 3. POI ���� �� ���� ������ ���� ----------------------------------------
    private PointOfInterest InstantiatePointOfInterest(int floor, int column)
    {
        // �̹� �ش� ��ġ�� POI�� ������ �� POI�� ��ȯ
        if (_pointOfInterestsMap[floor][column] != null)
        {
            return _pointOfInterestsMap[floor][column];
        }

        // 3.1 POI ���� --------------------------------------
        // POI ���� ����, ������ ���·� ����
        PointOfInterest randomPOI = GetWeightedRandomPOI(floor);

        // �ν��Ͻ� ����, poi ��Ͽ� ����
        PointOfInterest instance = Instantiate(randomPOI, boardContainer);
        _pointOfInterestsMap[floor][column] = instance;

        // ȭ��� ��ġ ���, ����
        Vector3 pos = GetPositionVector(floor, column);
        instance.transform.localPosition = pos;

        // 3.2 ���� POI ���� ---------------------------------
        // �� ���� ����� ��� ����, �̰� ������ �� �ƴ϶�� ��� ���� �õ� 
        while (instance.NextPointsOfInterest.Count == 0 && floor < mapLength - 1)
        {
            // ���� �밢�� ���� �õ�
            if (column > 0 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsMap[floor + 1][column] == null)
                {
                    InstantiateNextPoint(floor + 1, column - 1);
                }
            }

            // ������ �밢�� ���� �õ�
            if (column < maxWidth - 1 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsMap[floor + 1][column] == null)
                {
                    InstantiateNextPoint(floor + 1, column + 1);
                }
            }

            // ���� ���� �õ�
            if (Random.Range(0f, 1f) < chancePathMiddle)
            {
                InstantiateNextPoint(floor + 1, column);
            }
        }

        // ���� ���� PointOfInterest�� �����ϰ� �����ϴ� ���� �Լ�
        // ���� �Լ�: �� �Լ� ���ο����� �� ��� ���ο� ����. ���� �Լ��� ���ϰ� ���յ� ���� �Լ��� ���� �� �ſ� ����. 
        void InstantiateNextPoint(int index_i, int index_j)
        {
            PointOfInterest nextPOI = InstantiatePointOfInterest(index_i, index_j);
            AddLineBetweenPoints(instance, nextPOI);

            // �̹� �� ���ִٸ� ����/���� ��� ��� ���� 
            if (!instance.NextPointsOfInterest.Contains(nextPOI))
                instance.NextPointsOfInterest.Add(nextPOI);
            if (!nextPOI.PreviousPointsOfInterest.Contains(instance))
                nextPOI.PreviousPointsOfInterest.Add(instance);

            _numberOfConnections++;
        }

        return instance;
    }

    // ����ġ�� ���� ��� ���� ����, 5��° ���� shortcutPOI�� ����. POI Prefab ��ȯ
    private PointOfInterest GetWeightedRandomPOI(int currentFloor)
    {
        if (currentFloor == 0) return startPOI;

        if (currentFloor == mapLength - 1) return endPOI;

        // 5��° ���� ��� shortcutPOI ���� ��ȯ. ������ �־ currentFloor+1 �ƴ�
        if ((currentFloor) % 5 == 0)
        {
            return shortcutPOI;
        }

        // �������� ����ġ�� ���� ���� ����
        if (weightedPointsOfInterestPrefabs.Count == 0)
            throw new System.Exception("������ ����Ʈ�� ����ֽ��ϴ�!");

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

    // �� POI ���� ����(���) ����
    private void AddLineBetweenPoints(PointOfInterest thisPoint, PointOfInterest nextPoint)
    {
        float len = _lineLength;
        float height = _lineHeight;

        // �� �� ������ ���� ���� ���
        Vector3 dir = (nextPoint.transform.position - thisPoint.transform.position).normalized;

        // �� �� ������ �Ÿ� ���
        float dist = Vector3.Distance(thisPoint.transform.position, nextPoint.transform.position);
            
        // �� �� ���̿� �� ���� ���� ��� (�е� ����)
        int num = (int)(dist / (len * multiplicativeSpaceBetweenLines));

        // ���� �е� �Ÿ� ��� (num�� ������ ���� �Ÿ� �й�)
        float pad = (dist - (num * len)) / (num + 1);

        // ù ��° ������ ��ġ ��� (len/2f�� ���� �߽�)
        Vector3 pos_i = thisPoint.transform.position + (dir * (pad + (len / 2f)));

        // ��� ���� ��ġ
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = pos_i + ((len + pad) * i * dir);
            GameObject lineCreated = Instantiate(pathPrefab, pos, Quaternion.identity, boardContainer);
            // ������ ���� ����Ʈ�� �ٶ󺸵��� ȸ��
            lineCreated.transform.LookAt(nextPoint.transform);
            // ���� ���̸�ŭ �Ʒ��� ���� (�߽� ���߱�)
            lineCreated.transform.position -= Vector3.up * (height / 2f);
        }
    }

    // ��,�� ������ �޾Ƽ� ȭ�� �� ��ġ ��� �� ���ͷ� ��ȯ. 
    private Vector3 GetPositionVector(int floor, int column)
    {
        // x�� �� ĭ�� ũ�� ���
        float xSize = xMaxSize / maxWidth;
        // x, y ��ġ ���
        float xPos = (xSize * column) + (xSize / 2f);
        float yPos = yPadding * floor;

        // ù ���̸� ��ġ ����
        if(floor == 0)
        {
            xPos = xMaxSize / 2;
            Vector3 startPos = new Vector3(xPos, 0, yPos);
            Debug.Log("Start pos" +  startPos);
            return startPos;
        }

        // ��ġ�� ���� �е� �߰� - ���ڿ� ���� ���´� ������� ������ ��� ��ġ ���ϱ�
        xPos += Random.Range(-xSize / 4f, xSize / 4f);
        yPos += Random.Range(-yPadding / 4f, yPadding / 4f);

        // ���� ��ġ ���� ����
        Vector3 pos = new Vector3(xPos, 0, yPos);

        return pos;
    }

    // Transform�� ��� �ڽ� ������Ʈ�� ��� �����ϴ� �Լ�
    private void DestroyImmediateAllChildren(Transform transform)
    {
        List<Transform> toKill = new();

        // ������ �ڽ� Transform ����Ʈ�� �߰�
        foreach (Transform child in transform)
        {
            toKill.Add(child);
        }

        // �ڿ������� ���� (�����ϰ�)
        for (int i = toKill.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(toKill[i].gameObject);
        }
    }

    // ī�޶� ��ũ�ѿ� �� �� ���� ���ϱ�
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

        // POI�� �ϳ��� ���� ��� ���� ó��
        if (minZ == float.MaxValue || maxZ == float.MinValue)
            return new Vector2(0, 0);

        return new Vector2(minZ, maxZ);
    }

    public PointOfInterest getStartingPoint()
    {
        return startingPoint;
    }

    // ���� ����
    public List<PointOfInterest> GetNodesBeforeEndPOI()
    {
        // ������ ��-1�� ������ ��ȯ
        return _pointOfInterestsMap[_pointOfInterestsMap.Length - 2].ToList();
    }

}
